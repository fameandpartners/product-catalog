using Fame.Common;
using Microsoft.AspNetCore.Mvc;
using Fame.Service.ChangeTracking;
using Fame.Service.Services;
using Microsoft.Extensions.Options;

namespace Fame.Web.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/ProductSummary")]
    public class ProductSummaryController : Controller
    {
        private readonly FameConfig _fameConfig;
        private readonly IProductSummaryService _productSummaryService;

        public ProductSummaryController(IBaseServices services, IUnitOfWork unitOfWork, IOptions<FameConfig> fameConfig) 
        {
            _fameConfig = fameConfig.Value;
            _productSummaryService = services.ProductSummary.Value;
        }

        // GET: api/ProductSummary/5
        [HttpGet("{id}/{locale}")]
        public IActionResult GetProductSummary([FromRoute] string id, [FromRoute] string locale)
        {
            if (!ModelState.IsValid || !_fameConfig.Localisation.IsValidLocaleCode(locale))
            {
                return BadRequest(ModelState);
            }

            var productSummary = _productSummaryService.GetProductSummary(id, locale);

            if (productSummary == null)
            {
                return NotFound();
            }

            return Ok(productSummary);
        }
    }
}