using System.Threading.Tasks;
using Fame.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fame.Web.Areas.API.Controllers
{
    [Produces("text/csv")]
    [Route("api/ProductFeed")]
    public class ProductFeedController : Controller
    {
        private readonly IFeedMetaService _feedMetaService;
        private readonly S3DocumentService _documentService;

        public ProductFeedController(
            IFeedMetaService feedMetaService,
            S3DocumentService documentService)
        {
            _feedMetaService = feedMetaService;
           _documentService = documentService;
        }

        // GET: api/ProductFeed
        [HttpGet]
        public async Task<FileResult> Index()
        {
            var latestFeed = _feedMetaService.GetLatest();
            if (latestFeed == null) return null;
            var doc = await _documentService.ReadFile(latestFeed.S3Path);
            return File(doc, "application/zip", latestFeed.Zipped ? latestFeed.ZipFileName : latestFeed.FileName);
        }
    }
}
