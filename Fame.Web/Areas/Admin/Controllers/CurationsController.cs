using Fame.Service.ChangeTracking;
using Fame.Service.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fame.Web.Areas.Admin.Models;
using Fame.Service;
using Fame.Search.Services;
using Fame.Data.Models;
using System;
using PagedList.Core;
using Microsoft.Extensions.Options;
using Fame.Common;
using Fame.Web.Code.Extensions;
using System.Linq;

namespace Fame.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CurationsController : Controller
    {
        private readonly FameConfig _fameConfig;
        private readonly ICurationService _curationService;
        private readonly ICurationMediaService _curationMediaService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurationSearchService _curationSearchService;

        public CurationsController(
            IOptions<FameConfig> fameConfig,
            IImageManipulatorService imageManipulatorService,
            CurationImageCacheService imageCacheService,
            IBaseServices services,
            ICurationSearchService curationSearchService,
            IUnitOfWork unitOfWork)
        {
            _fameConfig = fameConfig.Value;
            _curationService = services.Curation.Value;
            _curationMediaService = services.CurationMedia.Value;
            _unitOfWork = unitOfWork;
            _curationSearchService = curationSearchService;
        }

        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            var curations = _curationService.GetCurations(page, pageSize);
            var model = new Tuple<IPagedList<Curation>, FameConfig>(curations, _fameConfig);
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var curation = await GetCurationEditModel(id);
            if (curation == null) return RedirectToAction("Index").WithNotification(NotificationType.Error, $"Invalid Curation Id : {id}");
            return View(curation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit( CurationEditModel model)
        {
            var curation = _curationService.GetById(model.PID);
            if (curation == null) return View(model).WithNotification(NotificationType.Error, $"Error - Invalid PID");
            if (!ModelState.IsValid) {
                var curationModel = await GetCurationEditModel(model.PID);
                curationModel.OverlayText = model.OverlayText;
                return View(curationModel).WithNotification(NotificationType.Error, $"Error - Please check any validation errors and try again");
            }
            curation.OverlayText = model.OverlayText;
            _curationService.Update(curation);
            if (model.CuratedMedia != null && model.CuratedMedia.Any())
            {
                var curationMedia = model.CuratedMedia.ToCurationMedia();
                _curationMediaService.Update(model.PID, curationMedia);
            }
			_unitOfWork.Save();
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Curation Saved");
        }

		public async Task<IActionResult> PopulateSearchMeta(string id)
		{
			await _curationSearchService.UpsertCuration(id);
			_unitOfWork.Save();
            return RedirectToAction("Edit", new { id });
		}

        private async Task<CurationEditModel> GetCurationEditModel(string id)
        {
            var curation = await _curationService.GetCuration(id);
            return new CurationEditModel
            {
                PID = id,
                Name = curation.Name,
                Description = curation.Description,
                OverlayText = curation.OverlayText,
                CuratedMedia = curation.Media
                    .OrderBy(c => c.SortOrder)
                    .ToList()
                    .ToCurationMediaEditModel(),
                OnBodyUrlDomain = _fameConfig.Curations.Url,
                CurationComponents = curation.CurationComponents,
                Facet = curation.Facet,
                Product = curation.Product
            };
        }
    }
}
