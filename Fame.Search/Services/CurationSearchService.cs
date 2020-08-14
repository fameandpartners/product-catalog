using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fame.Common.Extensions;
using Fame.Data.Models;
using Fame.ImageGenerator;
using Fame.Service.ChangeTracking;
using Fame.Service.DTO;
using Fame.Service.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Fame.Search.Services
{
    public class CurationSearchService : ICurationSearchService
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IProductDocumentService _productDocumentService;
        private readonly IOptionPriceService _optionPriceService;
        private readonly ICurationService _curationService;
        private readonly IProductVersionService _productVersionService;
        private readonly ILogger<CurationSearchService> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly ICacheService _cacheService;
        private readonly ICurationMediaService _curationMediaService;
        private readonly DropboxService _dropboxService;
        private readonly IWorkflowService _workflowService;

        public CurationSearchService(
			IUnitOfWork unitOfWork,
            IProductDocumentService productDocumentService,
            IBaseServices services,
            DropboxService dropboxService,
            ILogger<CurationSearchService> logger,
            IDistributedCache distributedCache)
        {
            _dropboxService = dropboxService;
            _cacheService = services.Cache.Value;
            _curationMediaService = services.CurationMedia.Value;
            _optionPriceService = services.OptionPrice.Value;
            _optionPriceService = services.OptionPrice.Value;
            _curationService = services.Curation.Value;
            _productVersionService = services.ProductVersion.Value;
            _logger = logger;
            _distributedCache = distributedCache;
			_unitOfWork = unitOfWork;
			_productDocumentService = productDocumentService;
            _workflowService = services.Workflow.Value;
        }
        
        public async Task<Curation> UpsertCuration(string PID)
        {
			_logger.LogInformation($"CurationSearchService - UpsertCuration - {PID}");
            // Set the PrimarySilhouetteId from the search and determine the ProductDocumentVersionId.
            var productVersions = await _productVersionService.GetProductVersionsAsync();
            var pidModel = new PIDModel(PID, productVersions);
            if (pidModel.InvalidCombination) {
				_logger.LogError("CurationSearchService - UpsertCuration - Invalid PID");
				return null;
			};
            pidModel = await _productDocumentService.SetVariationMetaAsync(pidModel); 
            if (pidModel == null) {
				_logger.LogError("CurationSearchService - UpsertCuration - PID not indexed in Elastic Search");
				return null;
			}
            
            var curation = _curationService.GetById(PID);
			if (curation == null) {
				curation = pidModel.ToCuration();
				_curationService.Insert(curation);
			} else
			{
				var updatedCuration = pidModel.ToCuration();
				curation.PrimarySilhouetteId = updatedCuration.PrimarySilhouetteId;
				curation.ProductDocumentVersionId = updatedCuration.ProductDocumentVersionId;
				curation.Name = updatedCuration.Name;
                curation.Description = updatedCuration.Description;
                curation.TaxonString = updatedCuration.TaxonString;
                _curationService.Update(curation);
            }
            _cacheService.DeleteWithPrefix(CachePrefix.Curation);
            return curation;
        }

		public async Task UpsertAllCurations()
		{
			foreach (var pid in _curationService.GetPIDs())
			{
				await UpsertCuration(pid);
			}
			_unitOfWork.Save();
        }

        public async Task<List<ProductListItem>> GetCurationsBySilhouetteAsync(string silhouetteId, string locale)
        {
            var pids = _curationService.GetPIDsBySilhouette(silhouetteId);
            var cacheKey = CacheKey.Create(CachePrefix.Curation, GetType(), "GetProductListItems", string.Join("-", pids), locale);
            return await _distributedCache.GetOrSetAsync(cacheKey, () => GetProductListItems(pids, locale, false));
        }

        public async Task<List<ProductListItem>> GetCurationsAsync(string[] pids, string locale, bool noMediaForCadImages)
        {
            var distinctPids = pids.Distinct().ToArray();
            var cacheKey = CacheKey.Create(CachePrefix.Curation, GetType(), "GetProductListItems", string.Join("-", distinctPids.OrderBy(p => p)), locale, noMediaForCadImages);
            return await _distributedCache.GetOrSetAsync(cacheKey, () => GetProductListItems(distinctPids, locale, noMediaForCadImages));
        }
        
		/// <summary>
		/// ProductListItems are a superset of curations
		/// </summary>
		/// <param name="pids"></param>
		/// <param name="locale"></param>
		/// <returns></returns>
        private async Task<List<ProductListItem>> GetProductListItems(string[] pids, string locale, bool noMediaForCadImages)
        {
            var result = new List<PIDModel>();
            var productVersions = await _productVersionService.GetProductVersionsAsync();
            var pidGroups = pids.Select(s => new PIDModel(s, productVersions, locale)).GroupBy(s => s.ProductVersionId);

            // We need to calculate the price here becuase these PIDs may have extras so we can't use the calculated price from the search as products with extras aren't in the search.
            foreach (var pidModelGroup in pidGroups)
            {
                var allComponentIds = pidModelGroup.SelectMany(s => s.ComponentIds).Distinct();
                
                foreach (var pidModel in pidModelGroup)
                {
                    pidModel.SetPrice(_optionPriceService.GetComponentPriceDictionary(pidModelGroup.Key, allComponentIds, locale));
                    if (!pidModel.InvalidCombination) result.Add(pidModel);
                }
            }

            //// Populate VariationMeta (From Search) - Name, Description, PrimarySilhouetteId
            result = await _productDocumentService.SetVariationMetaAsync(result);
            
            //// Populate CurationMedia
            result = _curationMediaService.AssignMediaListItems(result, noMediaForCadImages);
            
            return result.Select(c => c.ToProductListItem()).ToList();
        }

        public async Task<List<string>> ImportCurations(string path)
        {
            _curationMediaService.ArchiveAll(); //Archive all curations, they will be re-activated as they are re-imported.

            var curationPaths = new List<string>();
            var dropboxContent = await _dropboxService.ListFolder(path);

            foreach (var item in dropboxContent)
            {
                var pathWithoutExtension = item.FileName.Split(".").FirstOrDefault().Split("-");
                if (pathWithoutExtension.Length != 2) continue;
                var pidString = pathWithoutExtension[0].ToUpper();
                var parts = pidString.Split('~');
                var productId = parts.First();
                var componentIds = parts.Skip(1).OrderBy(c => c);
                var pid = $"{productId}~{String.Join('~', componentIds)}";
                if (!int.TryParse(pathWithoutExtension[1], out int sortPosition)) continue;
                if (!_curationService.Exists(pid))
                {
                    var errorMessage = $"CurationSearchService - Import Curations - Invalid Curation Id: {pid}";
                    Console.WriteLine("Dropbox path:");
                    Console.WriteLine(path);
                    try
                    {
                        var curation = await UpsertCuration(pid);
                        if (curation == null)
                        {
                            _logger.LogError(errorMessage);
                            continue;
                        }
                        _unitOfWork.Save();
                    }
                    catch (Exception)
                    {

                        _logger.LogError(errorMessage);
                        throw new ApplicationException(errorMessage);
                    }
                }
                if (_curationMediaService.ShouldAddMedia(pid, sortPosition, item.LastModified))
                {
                    var stream = await _dropboxService.ReadFile(item.FullPath);
                    await _curationMediaService.AddMediaAtPosition(pid, sortPosition, item.FileName, stream, item.LastModified);
                    stream.Dispose();
                }

                curationPaths.Add(item.FileName);
            }
                      
            _curationMediaService.DeleteArchivedMedia();

            return curationPaths;
        }
    }
}
