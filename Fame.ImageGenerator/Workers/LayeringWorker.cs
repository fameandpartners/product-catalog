using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using Fame.Data.Models;
using Newtonsoft.Json;
using Fame.Service;
using Fame.Service.DTO;
using Fame.Service.Services;

namespace Fame.ImageGenerator.Workers
{
    public class LayeringWorker : IWorker<SQSEvent>
    {
        private readonly IImageManipulatorService _imageManipulatorService;
        private readonly IImageCacheService _imageCacheService;

        public LayeringWorker(IImageManipulatorService imageManipulatorService, ProductRenderCacheService imageCacheService)
        {
            _imageManipulatorService = imageManipulatorService;
            _imageCacheService = imageCacheService;
        }

        public async Task<Object> Process(SQSEvent snsEvent)
        {
            foreach (var message in snsEvent.Records)
            {
                var request = JsonConvert.DeserializeObject<Request>(message.Body);

                await ProcessRecord(request);
            }

            return new Response("Successfuly processed");
        }

        public async Task ProcessRecord(Request request)
        {
            if (!request.Layers.Any())
            {
                return;
            }

            var testSize = request.IsOption ? Size.OPTION_SIZES.Last() : Size.PRODUCT_SIZES.Last();
            var testMeta = FileMeta.GetForRenderedImage(
                groupId: request.ProductId,
                zoom: request.Zoom,
                orientation: request.Orientation,
                size: testSize,
                files: request.Layers,
                fileName: request.FileName
            );

            if (!await _imageCacheService.Exists(testMeta))
            {
                var sizes = request.IsOption ? Size.OPTION_SIZES : Size.PRODUCT_SIZES;
                var images = await CreateImages(request.ProductId, request.Layers, sizes, request.Orientation, request.Zoom);

                foreach (var image in images)
                {
                    var meta = FileMeta.GetForRenderedImage(
                        groupId: request.ProductId,
                        zoom: request.Zoom,
                        orientation: request.Orientation,
                        size: image.Key,
                        files: request.Layers,
                        fileName: request.FileName
                    );
                    await _imageCacheService.Set(meta, image.Value);
                }
            }
        }

        public async Task<IDictionary<Size, Stream>> CreateImages(string productId, IEnumerable<FileMeta> layers, IEnumerable<Size> sizes, Orientation orientation, Zoom zoom)
        {
            var workingSize = sizes.OrderByDescending(s => s.Width).First();

            var files = layers
                .Select(layer => _imageCacheService.Get(FileMeta.GetForResizedLayer(productId, orientation, zoom, layer, workingSize)).Result);

            return await _imageManipulatorService.LayerAndCrop(files, sizes);
        }

        public class Request
        {
            public FileMeta[] Layers;
            public string ProductId { get; set; }
            public string FileName { get; set; }
            public Zoom Zoom;
            public Orientation Orientation;
            public bool IsOption { get; set; }

            public Request(FileMeta[] layers, string productId, string fileName, Zoom zoom, Orientation orientation, bool isOption)
            {
                Layers = layers;
                ProductId = productId;
                FileName = fileName;
                Zoom = zoom;
                Orientation = orientation;
                IsOption = isOption;
            }
        }

        public class Response
        {
            public string Message { get; set; }

            public Response(string message)
            {
                Message = message;
            }
        }
    }
}
