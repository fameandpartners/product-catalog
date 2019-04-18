using Amazon.Lambda.APIGatewayEvents;
using Fame.Data.Models;
using Fame.ImageGenerator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fame.Service;
using Fame.Service.DTO;

namespace Fame.ImageGenerator.Workers
{
    public class LayeringAdhocWorker : IWorker<APIGatewayProxyRequest>
    {
        private readonly ILayerCombinationService _layerCombinationService;
        private readonly LayeringWorker _layeringWorker;
        private readonly IImageCacheService _imageCacheService;


        public LayeringAdhocWorker(LayeringWorker layeringWorker, ILayerCombinationService layerCombinationService, ProductRenderCacheService imageCacheService)
        {
            _layeringWorker = layeringWorker;
            _layerCombinationService = layerCombinationService;
            _imageCacheService = imageCacheService;
        }

        public async Task<object> Process(APIGatewayProxyRequest e)
        {
            var path = e.PathParameters["file"];

            using (var data = await Process(path, DateTime.MinValue))
            {
                return new APIGatewayProxyResponse()
                {
                    Body = ToBase64(data),
                    IsBase64Encoded = true,

                    Headers = new Dictionary<string, string> {
                        {"Content-Type", "image/png"}
                    },
                    StatusCode = 200
                };
            }
        }

        public async Task<Stream> Process(string path, DateTime lastModified)
        {
            var urlComponents = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
            var productId = urlComponents[0];

            var sizeString = urlComponents[2];
            var size = Size.ALL_SIZES.First(i => $"{i.Width}x{i.Height}" == sizeString);
            var sizes = new Size[] { size, Size.SIZE_OPTION_704 }.Distinct();

            var fileName = urlComponents[3];
            var options = fileName.Replace(".png", "").Replace(".jpg", "").Split("~");

            var positionId = urlComponents[1];
            var orientation = Enum.GetValues(typeof(Orientation)).Cast<Orientation>().Single(o => positionId.IndexOf(o.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0);
            var zoom = Enum.GetValues(typeof(Zoom)).Cast<Zoom>().Single(z => positionId.IndexOf(z.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0);

            var normalizedFileName = FileMeta.GetFileName(options, "png");
            var fileMeta = FileMeta.GetForRenderedImage(productId, zoom, orientation, size, normalizedFileName, lastModified);


            var image = await _imageCacheService.Get(fileMeta);
            if (image != null)
            {
                return image;
            }


                var layers = await GetLayers(productId, orientation, options);
                var images = await _layeringWorker.CreateImages(productId, layers, sizes, orientation, zoom);
            foreach (var i in images)
            {
                var generatedImage = i.Value;
                var generatedSize = i.Key;

                var fileMetaForSize = FileMeta.GetForRenderedImage(productId, zoom, orientation, generatedSize, normalizedFileName, lastModified);
                await _imageCacheService.Set(fileMetaForSize, generatedImage);
            }


            var result = images[size];
            result.Position = 0;

            return result;
        }


        public async Task<IEnumerable<FileMeta>> GetLayers(string productId, Orientation orientation, string[] options)
        {
            var allLayers = await _layerCombinationService.GetLayersCached(productId, orientation);


            return _layerCombinationService.GetRenderLayersForCombination(allLayers, options).ToList();
        }


        private String ToBase64(Stream stream)
        {
            if (stream is MemoryStream)
            {
                return Convert.ToBase64String(((MemoryStream)stream).ToArray());
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
    }
}
