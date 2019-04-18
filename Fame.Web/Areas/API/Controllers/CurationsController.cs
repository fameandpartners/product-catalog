using System.Linq;
using System.Threading.Tasks;
using Fame.Common;
using Fame.Search.Services;
using Microsoft.AspNetCore.Mvc;
using Fame.Service.ChangeTracking;
using Fame.Service.Services;
using Microsoft.Extensions.Options;

namespace Fame.Web.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Curations")]
    public class CurationsController : Controller
    {
        private readonly IProductDocumentService productDocumentService;
        private readonly FameConfig _fameConfig;
        private readonly ICurationSearchService _curationSearchService;

        public CurationsController(
            ICurationSearchService curationSearchService, 
            IUnitOfWork unitOfWork, 
            IOptions<FameConfig> fameConfig,
            IProductDocumentService productDocumentService) 
        {
            this.productDocumentService = productDocumentService;
            _fameConfig = fameConfig.Value;
            _curationSearchService = curationSearchService;
        }
        
        [Route("{locale}")]
        [HttpGet]
        public async Task<IActionResult> GetCurations([FromRoute] string locale, string[] pids, bool noMedia = false)
        {
            if (!ModelState.IsValid || !_fameConfig.Localisation.IsValidLocaleCode(locale)) return BadRequest(ModelState);
            var curations = await _curationSearchService.GetCurationsAsync(pids, locale, noMedia);
            return curations == null ? NotFound() as IActionResult : Ok(curations);
        }
        
        [Route("{locale}/{pid}")]
        [HttpGet]
        public async Task<IActionResult> GetCurations([FromRoute] string locale, string pid, bool noMedia = false)
        {
            if (!ModelState.IsValid || !_fameConfig.Localisation.IsValidLocaleCode(locale)) return BadRequest(ModelState);
            var curations = await _curationSearchService.GetCurationsAsync(new [] {pid}, locale, noMedia);
            return curations.Any() ? Ok(curations.FirstOrDefault()) : NotFound() as IActionResult;
        }
        
        [Route("silhouette/{locale}/{silhouette}")]
        [HttpGet]
        public async Task<IActionResult> GetCurationsBySilhouette([FromRoute] string locale, string silhouette)
        {
            if (!ModelState.IsValid || !_fameConfig.Localisation.IsValidLocaleCode(locale)) return BadRequest(ModelState);
            var curations = await _curationSearchService.GetCurationsBySilhouetteAsync(silhouette, locale);
            return Ok(curations);
        }
    }
}