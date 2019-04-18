using System.Diagnostics;
using Fame.Service.ChangeTracking;
using Fame.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fame.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IUnitOfWork unitOfWork) 
        {}

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new {Area = "Admin"});
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
