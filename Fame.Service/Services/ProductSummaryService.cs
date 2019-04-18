using System;
using System.Collections.Generic;
using System.Linq;
using Fame.Common;
using Fame.Common.Extensions;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Fame.Service.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Fame.Service.Services
{
    internal class ProductSummaryService : IProductSummaryService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly FameConfig _fameConfig;
        private readonly IProductVersionRepository _productVersionRepo;
        private readonly IProductRenderComponentRepository _productRenderComponentRepo;
        private readonly IOptionRepository _optionRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IRenderPositionRepository _renderPositinRepo;
        private readonly ICurationMediaRepository _curationMediaRepo;

        public ProductSummaryService(IRepositories repositories, IOptions<FameConfig> fameConfig, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _fameConfig = fameConfig.Value;
            _groupRepo = repositories.Group.Value;
            _optionRepo = repositories.Option.Value;
            _productVersionRepo = repositories.ProductVersion.Value;
            _productRenderComponentRepo = repositories.ProductRenderComponent.Value;
            _renderPositinRepo = repositories.RenderPosition.Value;
            _curationMediaRepo = repositories.CurationMedia.Value;
        }

        public ProductSummary GetProductSummary(string id, string localisationCode)
        {
            var cacheKey = CacheKey.Create(GetType(), "InstantiateProductSummary", id, localisationCode);
            return _distributedCache.GetOrSet(cacheKey, () => InstantiateProductSummary(id, localisationCode));
        }

        private ProductSummary InstantiateProductSummary(string id, string localisationCode)
        {
            var productVersion = _productVersionRepo.GetLatest(id, VersionState.Active) ?? _productVersionRepo.GetLatest(id, VersionState.Archived);
            if (productVersion == null) return null;
            var price = productVersion.Prices.SingleOrDefault(p => p.LocalisationCode == localisationCode);
            var paymentMethods = new Dictionary<string, bool>();
            if (localisationCode == _fameConfig.Localisation.AU) paymentMethods.Add("afterPay", true);

            var productSummary = new ProductSummary
            {
                ProductId = productVersion.ProductId,
                Price = price?.PriceInMinorUnits ?? 0,
                ProductType = productVersion.Product.ProductType,
                PreviewType = productVersion.Product.PreviewType,
                CartId = productVersion.ProductId,
                ProductVersionId = productVersion.ProductVersionId,
                IsAvailable = productVersion.VersionState == VersionState.Active,
                ProductRenderComponents = _productRenderComponentRepo.GetRenderComponentIdsByProductVersionId(productVersion.ProductVersionId).ToList(),
                Components = GetComponents(productVersion.ProductVersionId, localisationCode),
                Groups = GetGroups(productVersion.ProductVersionId, productVersion.Product.PreviewType),
                CurationMeta = new CurationMeta
                {
                    Name = productVersion.Product.Title,
                    PermaLink = productVersion.ProductId, // TODO: Real PermaLink
                },
                Media = productVersion.Product.PreviewType == PreviewType.Cad ? GetMediaListItems(productVersion.ProductId) : new List<MediaListItem>(),
                PaymentMethods = paymentMethods,
                LayerCads = new List<LayerCad>(),
                RenderPositions = GetRenderPositions(productVersion),
                Size = new ProductSize
                {
                    MinHeightCm = _fameConfig.Size.MinHeightCm,
                    MaxHeightCm = _fameConfig.Size.MaxHeightCm,
                    MinHeightInch = _fameConfig.Size.MinHeightInch,
                    MaxHeightInch = _fameConfig.Size.MaxHeightInch
                }
            };
            return productSummary;
        }

        private List<MediaListItem> GetMediaListItems(string productId)
        {
            return _curationMediaRepo.Get()
                    .Where(cm => cm.Curation.ProductId == productId)
                    .ToMediaListItem(_fameConfig.Curations.Url);
        }

        private List<RenderPositionSummary> GetRenderPositions(ProductVersion productVersion)
        {
            return _renderPositinRepo.Get().Select(r => new RenderPositionSummary {Orientation = r.Orientation, RenderPositionId = r.RenderPositionId, Zoom = r.Zoom}).ToList();
        }

        private List<GroupSummary> GetGroups(int productVersionId, PreviewType previewType)
        {
            return _groupRepo.Get()
                .Where(g => g.ProductVersionId == productVersionId)
                .OrderBy(x => x.Order)
                .Select(g => new GroupSummary
                {
                    Id = g.GroupId,
                    Title = g.Title,
                    Slug = g.Slug, 
                    Hidden = g.Hidden,
                    SelectionTitle = g.SelectionTitle,
                    SectionGroups = g.SectionGroups.OrderBy(sg => sg.Order).Select(sg => new SectionGroupSummary
                    {
                        PreviewType = previewType,
                        Title = sg.Title,
                        Slug = sg.Slug,
                        AggregateTitle = sg.AggregateTitle,
                        RenderPositionId = sg.RenderPosition.RenderPositionId,
                        Sections = sg.Sections.Select(s => new SectionSummary
                        {
                            ComponentTypeId = s.ComponentTypeId,
                            ComponentTypeCategory = s.ComponentType.ComponentTypeCategory.ToString(),
                            Title = s.Title,
                            SelectionType = s.SelectionType.ToString(),
                            Options = s.CompatibleOptions.Select(co => new CompatibleOptionSummary
                            {
                                Code = co.Option.ComponentId,
                                IsDefault = co.IsDefault,
                                ParentOptionId = co.ParentOption.ComponentId
                            }).ToList()
                        }).ToList()
                    }).ToList()
                })
                .ToList();
        }

        private List<ComponentSummary> GetComponents(int productVersionId, string localisationCode)
        {
            return _optionRepo.Get()
                .Where(o => o.ProductVersionId == productVersionId)
                .Select(o => new ComponentSummary
                {
                    CartId = o.Component.CartId ?? o.ComponentId,
                    Code = o.ComponentId,
                    Title = o.Title,
                    ComponentTypeId = o.Component.ComponentTypeId,
                    Price = o.Prices.Single(p => p.LocalisationCode == localisationCode).PriceInMinorUnits,
                    IsProductCode = o.Component.ComponentType.IsProductCode,
                    IsRecommended = false, //TODO: Only required when we import the existing products out of the product catalogue, there are no recommended components for 'bridesmaids'.
                    ComponentTypeCategory = o.Component.ComponentType.ComponentTypeCategory.ToString(),
                    IncompatibleWith = o.Incompatibilities
                                        .Select(ig => new
                                        {
                                            ig.ParentOption.ComponentId,
                                            IncompatibleOptions = ig.IncompatibleOptions.Select(io => io.Option.ComponentId).ToList()
                                        })
                                        .GroupBy(ig => ig.ComponentId)
                                        .Select(ig => new IncompatibleWith
                                        {
                                            ComponentId = ig.Key,
                                            Incompatibilities = ig.Select(i => i.IncompatibleOptions).ToList()
                                        })
                                        .ToList(),
                    OptionRenderComponents = o.OptionRenderComponents.Select(orc => orc.ComponentTypeId).ToList(),
                    RenderPositionId = o.Component.RenderPosition.RenderPositionId,
                    SortOrder = o.Component.Sort,
                    Meta = o.Component.ComponentMeta.ToDictionary(cm => cm.Key, cm => cm.Value)
                })
                .ToList();
        }
    }
}
