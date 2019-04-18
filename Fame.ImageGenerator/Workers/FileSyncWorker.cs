using System;
using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using Fame.Data.Models;
using Fame.ImageGenerator.Services.Interfaces;
using Newtonsoft.Json;
using Fame.Service;
using Fame.Service.DTO;
using Fame.Service.Services;

namespace Fame.ImageGenerator.Workers
{
    public class FileSyncWorker : IWorker<SQSEvent>
    {
        private readonly IIOService _drobboxService;
        private readonly IImageManipulatorService _imageManipulatorService;
        private readonly IImageCacheService _imageCacheService;
        private readonly ILayerCombinationService _layerCombinationService;

        public FileSyncWorker(DropboxService dropboxService, IImageManipulatorService imageManipulatorService, ProductRenderCacheService imageCacheService, ILayerCombinationService layerCombinationService)
        {
            _drobboxService = dropboxService;
            _imageManipulatorService = imageManipulatorService;
            _imageCacheService = imageCacheService;
            _layerCombinationService = layerCombinationService;
        }

        public async Task<Object> Process(SQSEvent sqsEvent)
        {
            foreach (var message in sqsEvent.Records)
            {
                var request = JsonConvert.DeserializeObject<Request>(message.Body);

                await ProcessRecord(request);
            }

            return new Response("Successfuly processed");
        }

        async public Task ProcessRecord(Request request)
        {
            await EnsureResized(request.ProductId, request.Orientation, request.File);
        }

        async public Task EnsureResized(string groupId, Orientation orientation, FileMeta file)
        {
            var originalFileMeta = FileMeta.GetForResizedLayer(groupId, orientation, null, file, Size.ORIGINAL_RENDER_SIZE);

            foreach (var size in Size.ALL_SIZES)
            {
                foreach (var zoom in _layerCombinationService.GetZoomsForSize(size))
                {
                    var resizedFileMeta = FileMeta.GetForResizedLayer(groupId, orientation, zoom, file, size);

                    await _imageCacheService.GetOrSet(resizedFileMeta, async () =>
                    {
                        var baseFile = await _imageCacheService.GetOrSet(originalFileMeta, () => _drobboxService.ReadFile(file.FullPath));
                        return await _imageManipulatorService.ResizeAndCrop(baseFile, size, orientation, zoom);
                    });
                }
            }
        }

        public class Request
        {
            public FileMeta File { get; set; }
            public string ProductId { get; set; }
            public Orientation Orientation { get; set; }

            public Request(FileMeta file, string productId, Orientation orientation)
            {
                File = file;
                ProductId = productId;
                Orientation = orientation;
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
