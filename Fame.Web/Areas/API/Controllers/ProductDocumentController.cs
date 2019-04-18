using System.Threading.Tasks;
using Fame.Search.DTO;
using Fame.Search.Services;
using Microsoft.AspNetCore.Mvc;
using Fame.Service.ChangeTracking;
using System.Collections.Generic;

namespace Fame.Web.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/ProductDocument")]
    public class ProductDocumentController : Controller
    {
        private readonly IProductDocumentService _productDocumentService;

        public ProductDocumentController(IProductDocumentService productDocumentService, IUnitOfWork unitOfWork) 
        {
            _productDocumentService = productDocumentService;
        }
        
        // GET: api/ProductDocument
        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery] SearchArgs searchArgs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (searchArgs.Collections == null)
            {
                searchArgs.Collections = new List<string> { "ccs" }; // return Custom Clothing Studio Collection search results by default 
            }

            var docs = await _productDocumentService.GetAsync(searchArgs);

            return Ok(docs);
        }
    }
}