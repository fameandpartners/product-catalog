using System.Threading.Tasks;
using Fame.Background;
using Fame.Search.Services;
using Fame.Service.ChangeTracking;
using Fame.Service.Services;
using Fame.Web.Code.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fame.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FeedController : Controller
    {
        private readonly IFeedMetaService _feedMetaService;
        private readonly IProductFeedService _productFeedService;
        private readonly IImportService _importService;
        private readonly ICacheService _cacheService;
        private readonly IUnitOfWork _unitOfWork;

        public FeedController(
            IProductFeedService productFeedService,
            IFeedMetaService feedMetaService,
            IImportService importService,
            ICacheService cacheService,
            IUnitOfWork unitOfWork)
        {
            _feedMetaService = feedMetaService;
            _productFeedService = productFeedService;
            _importService = importService;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult Index(int page = 1)
        {
            var model = _feedMetaService.GetPage(page);
            return View(model);
        }

        public IActionResult GenerateFeed()
        {
            Job.Enqueue(() => _productFeedService.GenerateFeed());
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "You've generated a new feed, come back later to download.");
        }

        public async Task<IActionResult> DeleteFeed(int id)
        {
            await _productFeedService.DeleteFeed(id);
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Feed Version Deleted");
        }
    }
}
