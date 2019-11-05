using System.Threading.Tasks;
using Fame.Service.ChangeTracking;
using Fame.Service.Services;
using Fame.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fame.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductionSheetController : Controller
    {
        private readonly IComponentService _componentService;

        public ProductionSheetController(
            IComponentService componentService,
            IUnitOfWork unitOfWork)
        {
            _componentService = componentService;
        }
        
        public async Task<IActionResult> Index(string pid)
        {
            var productionSheetViewModel = await _componentService.GetProductionSheetAsync(pid);
            if(productionSheetViewModel!=null)
                return View(productionSheetViewModel);
            return View("Find", new FindModel("Not Found"));            
        }

        [Authorize]
        [HttpGet("/admin/productionsheet/find")]
        public IActionResult Find()
        {
            return View(new FindModel());
        }
    }
}
