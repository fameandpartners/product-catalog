using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fame.Background;
using Fame.Data.Models;
using Fame.ImageGenerator.Services.Interfaces;
using Fame.ImageGenerator.Workers;
using Fame.Service.ChangeTracking;
using Fame.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fame.Web.Controllers
{
    [Route("ImagePreview")]
    public class ImagePreviewController : Controller
    {
        private readonly IProductVersionService _productVersionService;

        private readonly IProductCombinationService _productCombinationService;
        private readonly LayeringAdhocWorker _layeringAdhocWorker;
        private readonly ILayerCombinationService _layerCombinationService;


        public ImagePreviewController(IBaseServices services, LayeringAdhocWorker layeringAdhocWorker, ILayerCombinationService layerCombinationService, IProductCombinationService productCombinationService)
        {
            _productVersionService = services.ProductVersion.Value;
            _productCombinationService = productCombinationService;
            _layeringAdhocWorker = layeringAdhocWorker;
            _layerCombinationService = layerCombinationService;
        }

        [HttpGet("{productId}/{positionId}/{size}/{fileName}")]
        public async Task<IActionResult> Image(string productId, string positionId, string size, string fileName)
        {
            var data = await _layeringAdhocWorker.Process($"{productId}/{positionId}/{size}/{fileName}", DateTime.MaxValue);
            return new FileStreamResult(data, "image/png");
        }

        [HttpGet("debug/{productId}/{positionId}/{size}/{fileName}")]
        public async Task<IActionResult> ImageDebug(string productId, string positionId, string size, string fileName)
        {
            var orientation = Enum.GetValues(typeof(Orientation)).Cast<Orientation>().Single(o => positionId.IndexOf(o.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0);
            var options = fileName.Replace(".png", "").Replace(".jpg", "").Split("~");

            var layers = await _layeringAdhocWorker.GetLayers(productId, orientation, options);

            var result = layers.Select((l) => new
            {
                LastModified = l.LastModified,
                FileName = l.FileName,
                Url = $"https://s3.amazonaws.com/fame-product-renders-dev/{productId}/layers/{orientation}/None/1056x1056/{l.FileName}"
            });

            return new JsonResult(result);
        }
    }
}
