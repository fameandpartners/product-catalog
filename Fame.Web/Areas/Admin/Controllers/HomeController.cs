using Fame.Background;
using Fame.Common;
using Fame.Data.Models;
using Fame.ImageGenerator.Workers;
using Fame.Search.Services;
using Fame.Service.ChangeTracking;
using Fame.Service.Clients.Interfaces;
using Fame.Service.DTO;
using Fame.Service.Services;
using Fame.Web.Areas.Admin.Models;
using Fame.Web.Code.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fame.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly FameConfig _fameConfig;
        private readonly IWorkflowService _workflowService;
        private readonly IImportService _importService;
        private readonly ICacheService _cacheService;
        private readonly IIndexService _indexService;
        private readonly ICurationSearchService _curationSearchService;
        private readonly LayeringMaster _layeringMaster;
        private readonly FileSyncMaster _fileSyncMaster;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpreeExportService _spreeExportService;
        private readonly ISpreeClient _spreeClient;
        private readonly IProductService _productService;
        private readonly IProductVersionService _productVersionService;

        public HomeController(
            IOptions<FameConfig> fameConfig,
            IBaseServices services,
            IImportService importService,
            ICacheService cacheService,
            IIndexService indexService,
            ICurationSearchService curationSearchService,
            LayeringMaster layeringMaster,
            FileSyncMaster fileSyncMaster,
            IUnitOfWork unitOfWork,
            ISpreeExportService spreeExportService,
            ISpreeClient spreeClient)
        {
            _fameConfig = fameConfig.Value;
            _workflowService = services.Workflow.Value;
            _importService = importService;
            _cacheService = cacheService;
            _indexService = indexService;
            _curationSearchService = curationSearchService;
            _layeringMaster = layeringMaster;
            _fileSyncMaster = fileSyncMaster;
            _unitOfWork = unitOfWork;
            _spreeExportService = spreeExportService;
            _spreeClient = spreeClient;
            _productService = services.Product.Value;
            _productVersionService = services.ProductVersion.Value;
        }

        public IActionResult Index()
        {
            var workflowModel = new WorkflowViewModel
            {
                Workflow = _workflowService.GetWorkflow(),
                LayeringDropNames = _productService.GetAllDropNames(true, VersionState.Active),
                SpreeDropNames = _productService.GetAllDropNames()
            };
            return View(workflowModel);
        }

        public IActionResult CurationImport()
        {
            Job.Enqueue(() => _curationSearchService.ImportCurations(_fameConfig.Curations.DropboxImportPath));
            _workflowService.TriggerWorkflowStep(WorkflowStep.CurationsImport);
            _unitOfWork.Save();
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Curation Triggered - Errors and progress can be viewed in Hangfire");
        }

        public IActionResult Import()
        {
            Job.Enqueue(() => _importService.Import());
            _workflowService.TriggerWorkflowStep(WorkflowStep.ProductImport);
            _unitOfWork.Save();
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Import Triggered - Errors and progress can be viewed in Hangfire");
        }

        public IActionResult DeleteCache()
        {
            _cacheService.DeleteAll();
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Cache Deleted");
        }

        public IActionResult ReIndex()
        {
            Job.Enqueue(() => _indexService.FullIndex());
            _workflowService.TriggerWorkflowStep(WorkflowStep.SearchIndex);
            _unitOfWork.Save();
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Search Index Triggered - Errors and progress can be viewed in Hangfire");
        }

        public IActionResult PopulateAllSearchMeta()
        {
            Job.Enqueue(() => _curationSearchService.UpsertAllCurations());
            _workflowService.TriggerWorkflowStep(WorkflowStep.SilhouetteData);
            _unitOfWork.Save();
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Silhouette Data Triggered - Errors and progress can be viewed in Hangfire");
        }

        [HttpPost]
        public IActionResult Trigger(string mode, string dropName)
        {
            if (dropName == "PleaseSelect") return RedirectToAction("Index").WithNotification(NotificationType.Error, "Please select a drop");
            var productIds = _productService.GetActiveProductIdsByDropName(dropName);

            if (mode == "layering")
            {
                TriggerLayering(productIds);
                return RedirectToAction("Index").WithNotification(NotificationType.Success, "Layering Triggered - Errors and progress can be viewed in Hangfire");
            }
            else
            {
                TriggerFileSync(productIds);
                return RedirectToAction("Index").WithNotification(NotificationType.Success, "File Sync Triggered - Errors and progress can be viewed in Hangfire");
            }
        }

        private void TriggerLayering(List<string> productIds)
        {
            ISet<string> onlyComponentIds = new HashSet<string>();

            foreach (var productId in productIds)
            {
                Job.Enqueue(() => _layeringMaster.ProcessOptionRenders(new LayeringMaster.Request(productId, onlyComponentIds)));
                Job.Enqueue(() => _layeringMaster.ProcessProductRenders(new LayeringMaster.Request(productId, onlyComponentIds)));
            }
            _workflowService.TriggerWorkflowStep(WorkflowStep.Layering);
            _unitOfWork.Save();
        }

        private void TriggerFileSync(List<string> productIds)
        {
            foreach (var productId in productIds)
            {
                var version = _productVersionService.GetLatest(productId);
                Job.Enqueue(() => _fileSyncMaster.Process(new FileSyncMaster.Request(productId, version.Product.DropBoxAssetFolder)));
            }
            _workflowService.TriggerWorkflowStep(WorkflowStep.FileSync);
            _unitOfWork.Save();
        }

        [HttpPost]
        public IActionResult SpreeImport(string DropName, string prodnames)
        {
            if (DropName == "PleaseSelect" && string.IsNullOrEmpty(prodnames.Trim())) return RedirectToAction("Index").WithNotification(NotificationType.Error, "Please select a drop");
            if(!string.IsNullOrEmpty(prodnames.Trim()))
            {
                var productIds = prodnames.Trim().Split(';');
                foreach (var productId in productIds)
                {
                    Job.Enqueue(() => ImportProductWithSpreeClient(productId.Trim()));
                }
            }
            else
            {
                var productIds = _productService.GetAllProductIdsByDropName(DropName);
                foreach (var productId in productIds)
                {
                    Job.Enqueue(() => ImportProductWithSpreeClient(productId));
                }
            }
            _workflowService.TriggerWorkflowStep(WorkflowStep.SpreeExport);
            _unitOfWork.Save();
            return RedirectToAction("Index").WithNotification(NotificationType.Success, "Spree Export Triggered - Go to Sidekiq in the Spree Admin area to view progress");
        }

        public async Task ImportProductWithSpreeClient(string productId)
        {
            var productVersionId = _productVersionService.GetLatestProductVersionId(productId);
            var data = await _spreeExportService.GetExport(productVersionId);
            await _spreeClient.ImportProduct(new SpreeImport[] { data });
        }
    }
}
