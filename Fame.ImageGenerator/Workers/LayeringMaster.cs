using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Fame.Common;
using Fame.Data.Models;
using Fame.ImageGenerator.DTO;
using Fame.ImageGenerator.Services.Interfaces;
using Fame.Service.DTO;
using Fame.Service.Extensions;
using Fame.Service.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Fame.Service;

namespace Fame.ImageGenerator.Workers
{
    public class LayeringMaster
    {
        private readonly IProductCombinationService _productCombinationService;
        private readonly IAmazonSQS _sqsClient;
        private readonly IProductVersionService _productVersionService;
        private readonly IRenderPositionService _renderPositionService;
        private readonly Lazy<LayeringWorker> _layeringWorker;
        private readonly ILayerCombinationService _layerCombinationService;
        private readonly string _sqsQueue;
        private readonly ILogger<LayeringMaster> _logger;


        public LayeringMaster(IProductCombinationService productCombinationService, IAmazonSQS sqsClient, IProductVersionService productVersionService, IRenderPositionService renderPositionService, Lazy<LayeringWorker> layeringWorker, ILayerCombinationService layerCombinationService, IOptions<FameConfig> fameConfig, ILogger<LayeringMaster> logger)
        {
            _sqsClient = sqsClient;
            _productCombinationService = productCombinationService;
            _productVersionService = productVersionService;
            _renderPositionService = renderPositionService;
            _layeringWorker = layeringWorker;
            _layerCombinationService = layerCombinationService;
            _sqsQueue = fameConfig.Value.ImageProcessing.LayeringQueue;
            _logger = logger;
        }

        public async Task<Response> ProcessProductRenders(Request request)
        {
            var productVersion = _productVersionService.GetLatest(request.ProductId);
            var layers = new Dictionary<Orientation, Lazy<ILookup<string, RenderLayer>>>();
            foreach (var orientation in Enum.GetValues(typeof(Orientation)).Cast<Orientation>())
            {
                layers.Add(orientation, new Lazy<ILookup<string, RenderLayer>>(() => _layerCombinationService.GetLayers(request.ProductId, orientation).Result));
            }

            var productRenderPositionIds = GetProductRenderPositions(productVersion.Product.PreviewType);
            var productRenderPositions = _renderPositionService
                .Get()
                .Where(rp => productRenderPositionIds.Contains(rp.RenderPositionId));

            var requests = _productCombinationService.GetCombinationsForProduct(productVersion, true)
               .SelectMany(combination =>
               {
                   return productRenderPositions.Select(renderPosition =>
                   {
                       var allLayers = layers[renderPosition.Orientation];

                       return MapToRequest(combination, allLayers.Value, renderPosition, false);
                   });
               });

            return await ProcessRenders(request, requests);
        }


        public async Task<Response> ProcessOptionRenders(Request request)
        {
            var productVersion = _productVersionService.GetLatest(request.ProductId);
            var layers = new Dictionary<Orientation, Lazy<ILookup<string, RenderLayer>>>();
            foreach (var orientation in Enum.GetValues(typeof(Orientation)).Cast<Orientation>())
            {
                layers.Add(orientation, new Lazy<ILookup<string, RenderLayer>>(() => _layerCombinationService.GetLayers(request.ProductId, orientation).Result));
            }

            var productRenderComponentTypes = productVersion.ProductRenderComponents.Select(rc => rc.ComponentType).ToHashSet();

            var items = productVersion.Options
            .Select(option =>
            {
                var renderPosition = GetProductRenderPositions(productVersion, option);


                var renderSections = option
                    .OptionRenderComponents
                    .Select(y => y.ComponentType)
                    // some groups don't have all component types (e.g. no waistband)
                    .Where(ct => productRenderComponentTypes.Contains(ct));

                Option startingOption = null;

                if (option.Component.ComponentTypeId == "extra")
                {
                    startingOption = option;
                }
                else
                {
                    renderSections = renderSections.Append(option.Component.ComponentType);
                }

                return new RenderPositionsAndComponentTypes(renderPosition, renderSections.ToHashSet(), startingOption);
            })
            .Distinct()
            .Where(renderPositionAndRenderSections => renderPositionAndRenderSections.RenderPosition != null)
            .SelectMany(renderPositionAndRenderSections =>
            {
                var renderPosition = renderPositionAndRenderSections.RenderPosition;
                var renderSections = renderPositionAndRenderSections.ComponentTypes.ToList();
                var startingOption = renderPositionAndRenderSections.StartingOption;


                var allLayers = layers[renderPosition.Orientation];

                return _productCombinationService
                .GetCombinationsForComponentTypes(renderSections, productVersion, startingOption != null ? new Option[] { startingOption } : new Option[] { })
                .Select(x => MapToRequest(x, allLayers.Value, renderPosition, true));
            });

            return await ProcessRenders(request, items);
        }

        private RenderPosition GetProductRenderPositions(ProductVersion productVersion, Option option)
        {
            foreach (var group in productVersion.Groups)
            {
                foreach (var sectionGroup in group.SectionGroups)
                {
                    foreach (var section in sectionGroup.Sections)
                    {
                        if (section.ComponentType == option.Component.ComponentType)
                        {
                            return sectionGroup.RenderPosition;
                        }
                    }
                }
            }

            throw new ArgumentException("cant find render position");
        }

        public async Task<Response> ProcessRenders(Request request, IEnumerable<LayeringWorker.Request> items)
        {
            items
                .Where(x => !request.OnlyIncludeComponentIds.Any() || request.OnlyIncludeComponentIds.Any(x.FileName.Contains))
                .Batch(10)
                .AsParallel()
                .WithDegreeOfParallelism(20)
                .ForAll(records =>
                {
                    try
                    {
                        _sqsClient.SendMessageBatchAsync(
                            _sqsQueue,
                            records.Select((record, i) => new Amazon.SQS.Model.SendMessageBatchRequestEntry()
                            {
                                Id = i.ToString(),
                                MessageBody = JsonConvert.SerializeObject(record)
                            }).ToList()
                        ).Wait();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Error publishing rendering task {JsonConvert.SerializeObject(records)}");
                    }
                });

            return new Response($"Successfully triggered events");
        }

        private string[] GetProductRenderPositions(PreviewType previewType)
        {
            if (previewType == PreviewType.Render)
            {
                return new string[] { "FrontNone", "BackNone" };
            }

            if (previewType == PreviewType.Cad)
            {
                return new string[] { "CadNone" };
            }

            throw new ArgumentException("unknown preview type");
        }


        private LayeringWorker.Request MapToRequest(ProductCombination combination, ILookup<string, RenderLayer> allLayers, RenderPosition renderPosition, bool isOption)
        {
            var layers = _layerCombinationService.GetRenderLayersForCombination(allLayers, combination).ToArray();

            return new LayeringWorker.Request(
                layers: layers,
                productId: combination.ProductVersion.ProductId,
                fileName: FileMeta.GetFileName(combination.Options.Select(x => x.ComponentId), "png"),
                zoom: renderPosition.Zoom,
                orientation: renderPosition.Orientation,
                isOption: isOption
            );
        }


        public class Response
        {
            public string Message { get; set; }

            public Response(string message)
            {
                Message = message;
            }
        }

        public class Request
        {
            public string ProductId { get; set; }
            public ISet<string> OnlyIncludeComponentIds { get; set; }

            public Request(string productId, ISet<string> onlyIncludeComponentIds)
            {
                ProductId = productId;
                OnlyIncludeComponentIds = onlyIncludeComponentIds;
            }
        }

        private class RenderPositionsAndComponentTypes : IEquatable<RenderPositionsAndComponentTypes>
        {
            public readonly RenderPosition RenderPosition;
            public readonly HashSet<ComponentType> ComponentTypes;
            public readonly Option StartingOption;

            public RenderPositionsAndComponentTypes(RenderPosition renderPosition, HashSet<ComponentType> componentTypes, Option startingOption)
            {
                RenderPosition = renderPosition;
                ComponentTypes = componentTypes;
                StartingOption = startingOption;
            }

            public override bool Equals(object obj)
            {
                var types = obj as RenderPositionsAndComponentTypes;
                return Equals(types);

            }

            public bool Equals(RenderPositionsAndComponentTypes other)
            {
                return other != null &&
                       EqualityComparer<RenderPosition>.Default.Equals(RenderPosition, other.RenderPosition) &&
                                                       ComponentTypes.SetEquals(other.ComponentTypes) &&
                                                       EqualityComparer<Option>.Default.Equals(StartingOption, other.StartingOption);
            }

            public override int GetHashCode()
            {
                var hashCode = 1043848019;
                hashCode = hashCode * -1521134295 + EqualityComparer<RenderPosition>.Default.GetHashCode(RenderPosition);
                // don't include ComponentTypes since it doesn't have a stable hash
                hashCode = hashCode * -1521134295 + EqualityComparer<Option>.Default.GetHashCode(StartingOption);
                return hashCode;
            }
        }
    }
}
