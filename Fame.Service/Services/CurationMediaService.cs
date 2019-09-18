using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Microsoft.AspNetCore.Http;
using Fame.Service.Extensions;
using System;
using Fame.Service.ChangeTracking;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using Fame.Common;
using Microsoft.Extensions.Options;
using Fame.Common.Extensions;

namespace Fame.Service.Services
{
    public class CurationMediaService : BaseService<CurationMedia>, ICurationMediaService
    {
        private readonly ILogger<CurationMediaService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositories _repositories;
        private readonly ICurationMediaRepository _curationMediaRepo;
        private readonly ICacheService _cacheService;
        private readonly ICurationRepository _curationRepo;
        private readonly ICurationMediaVariantRepository _curationMediaVariantRepo;
        private readonly ICurationComponentRepository _curationComponentRepo;
        private readonly IImageCacheService _imageStoreService;
        private readonly IImageManipulatorService _imageManipulatorService;
        private readonly FameConfig _fameConfig;

        public CurationMediaService(
                IOptions<FameConfig> fameConfig,
                ILogger<CurationMediaService> logger,
                IUnitOfWork unitOfWork,
                IRepositories repositories,
                CurationImageCacheService imageCacheService,
                ICacheService cacheService,
                IImageManipulatorService imageManipulatorService
                )
                : base(repositories.CurationMedia.Value)
        {
            _fameConfig = fameConfig.Value;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repositories = repositories;
            _curationMediaRepo = repositories.CurationMedia.Value;
            _cacheService = cacheService;
            _curationRepo = repositories.Curation.Value;
            _curationMediaVariantRepo = repositories.CurationMediaVariant.Value;
            _curationComponentRepo = repositories.CurationComponent.Value;
            _imageStoreService = imageCacheService;
            _imageManipulatorService = imageManipulatorService;
        }

        public async Task<List<CurationMedia>> AddMediaFormFiles(string pid, List<IFormFile> files)
        {
            return await DoAddMedia(pid, files.Where(f => f.Length > 0).ToDictionary(f => f.FileName, f => f.OpenReadStream()));
        }

        public async Task<CurationMedia> AddMediaAtPosition(string pid, int sortOrder, string fileName, Stream stream, DateTime LastModified)
        {
            // Save Original File in order to get dimensions and ensure we only create variations smaller than the original file
            var widthAndHeight = GetImageDimension(stream);
            if (widthAndHeight == null) return null;

            // Create a new Creation Media object
            var curationMedia = new CurationMedia
            {
                PID = pid,
                Type = MediaType.Photo,
                FitDescription = null,
                SizeDescription = null,
                SortOrder = sortOrder,
                PLPSortOrder = sortOrder,
                LastModified = LastModified,
                CurationMediaVariants = new List<CurationMediaVariant>()
            };

            // Create all the variations of the media metadata
            foreach (var size in Size.CreateVariations(widthAndHeight.Item1, widthAndHeight.Item2))
            {
                curationMedia.CurationMediaVariants.Add(new CurationMediaVariant
                {
                    IsOriginal = size.IsOriginal,
                    Height = size.Height,
                    Width = size.Width,
                    Quality = size.Quality,
                    Ext = size.IsOriginal ? Path.GetExtension(fileName) : ".jpeg"
                });
            }

            Insert(curationMedia);
            _unitOfWork.Save();
            //插入数据库后才知道其id所以要先写数据库，如果上传失败则从数据库删除
            try
            {
                _logger.LogInformation("CurationMedia - Add Media - Uploading Original File");
                // Save Original image
                var originalCurationMediaVariant = curationMedia.CurationMediaVariants.Single(cmv => cmv.IsOriginal);
                var originalMeta = FileMeta.GetForOnBodyImage(originalCurationMediaVariant);
                await _imageStoreService.Set(originalMeta, stream);

                // Save Variations of image
                foreach (var curationMediaVariant in curationMedia.CurationMediaVariants.Where(cmv => !cmv.IsOriginal))
                {
                    stream.Position = 0;
                    _logger.LogInformation("CurationMedia - Add Media - Uploading Variation");
                    var meta = FileMeta.GetForOnBodyImage(curationMediaVariant);
                    var resizedImage = await _imageManipulatorService.ResizeToJpeg(stream, curationMediaVariant.ToSize());
                    await _imageStoreService.Set(meta, resizedImage);
                }
            }
            catch (Exception ex)
            {
                Delete(curationMedia);
                _unitOfWork.Save();
                throw ex;
            }
            return curationMedia;
        }

        private async Task<List<CurationMedia>> DoAddMedia(string pid, Dictionary<string, Stream> files)
        {
            //Ensure Curation (PID) exists
            if (!_curationRepo.Get().Any(c => c.PID == pid))
            {
                _logger.LogError("CurationMedia - Add Media - PID doesn't exist");
                return null;
            };

            var curationMediaList = new List<CurationMedia>();

            _logger.LogInformation("CurationMedia - Add Media - Looping through uploaded files");
            var sortOrder = _curationMediaRepo.Get().Where(cm => cm.PID == pid).OrderByDescending(cm => cm.SortOrder).Select(cm => cm.SortOrder).FirstOrDefault();
            foreach (var file in files)
            {
                sortOrder += 1;
                var curationMedia = await AddMediaAtPosition(pid, sortOrder, file.Key, file.Value, DateTime.UtcNow);
                curationMediaList.Add(curationMedia);
            }

            if (!curationMediaList.Any())
            {
                _logger.LogError("CurationMedia - Add Media - No files uploaded");
                return null;
            }
            _cacheService.DeleteWithPrefix(CachePrefix.Curation);
            return curationMediaList;
        }

        private Tuple<int, int> GetImageDimension(Stream stream)
        {
            try
            {
                using (var image = Image.Load(stream))
                {
                    int height = image.Height;
                    int width = image.Width;

                    stream.Position = 0;
                    return new Tuple<int, int>(width, height);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("unable to determine image dimensions", e);
                return null;
            }
        }

        public List<PIDModel> AssignMediaListItems(List<PIDModel> pidModels, bool noMediaForCadImages)
        {            
            //HACK: Don't return media for CADs because all media is sent to the client in the ProductSummary for CADs.
            var pids = pidModels.WhereIf(noMediaForCadImages, p => p.PreviewType != PreviewType.Cad).Select(pm => pm.PID);
            var mediaListItems = GetMediaListItems(pids);

            var pidDictionary = pidModels.ToDictionary(c => c.PID, c => c);
            foreach (var mediaGroup in mediaListItems)
            {
                pidDictionary[mediaGroup.Key].Media = mediaGroup.Value.Media;
                pidDictionary[mediaGroup.Key].OverlayText = mediaGroup.Value.OverlayText;
            }

            return pidDictionary.Select(kv => kv.Value).ToList();
        }

        private Dictionary<string, ProductListItem> GetMediaListItems(IEnumerable<string> pids)
        {
            return _curationRepo.Get()
                    .Where(cm => pids.Contains(cm.PID))
                    .ToMediaListDictionary(_fameConfig.Curations.Url);
        }

        public void Update(string pid, List<CurationMedia> curationMedia)
        {
            var curationMediaDictionary = _curationMediaRepo.Get().Where(cm => curationMedia.Select(c => c.Id).Contains(cm.Id)).ToDictionary(c => c.Id, c => c);
            foreach (var cm in curationMedia)
            {
                var curationMediaEntity = curationMediaDictionary[cm.Id];
                curationMediaEntity.FitDescription = cm.FitDescription;
                curationMediaEntity.SizeDescription = cm.SizeDescription;
                curationMediaEntity.SortOrder = cm.SortOrder;
                curationMediaEntity.PLPSortOrder = cm.PLPSortOrder;
                curationMediaEntity.LastModified = cm.LastModified;
                _curationMediaRepo.Update(curationMediaEntity);
            }
            _cacheService.DeleteWithPrefix(CachePrefix.Curation);
        }

        public string DeleteMedia(int id)
        {
            var pid = _curationMediaRepo.Get().Where(c => c.Id == id).Select(c => c.PID).First();
            _curationMediaVariantRepo.DeleteWhere(cmv => cmv.CurationMedia.Id == id);
            _curationMediaRepo.DeleteWhere(cmv => cmv.Id == id);
            _cacheService.DeleteWithPrefix(CachePrefix.Curation);
            return pid;
        }

        public bool ShouldAddMedia(string pid, int sortPosition, DateTime lastModified)
        {
            var media = _curationMediaRepo.Get().Where(cm => cm.PID == pid && cm.SortOrder == sortPosition).ToList();
            if (!media.Any()) return true;

            // Add the LastModified date for all media that doesn't have it yet and set the Archived to false to achnowledge that the media still exists.
            foreach (var item in media)
            {
                item.Archived = false;
                if (item.LastModified == null)
                { 
                    item.LastModified = lastModified;
                    _curationMediaRepo.Update(item);
                    _cacheService.DeleteWithPrefix(CachePrefix.Curation);
                }
                _unitOfWork.Save();
            }

            // Delete old media and add new media if there is more than one item in this sort position (duplicated somehow) or if the media is newer (oldMedia.LastModified < newMedia.LastModified)
            if (media.Count > 1 || media.Any(m => m.LastModified < lastModified))
            {
                media.ForEach(m => DeleteMedia(m.Id));
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public void ArchiveAll()
        {
            var curationMediaItems = _curationMediaRepo.Get().Where(cm => cm.Archived == false).ToList();
            foreach (var cm in curationMediaItems)
            {
                cm.Archived = true;
            }
            _unitOfWork.Save();
        }

        public void DeleteArchivedMedia()
        {
            foreach (var id in _curationMediaRepo.Get().Where(cm => cm.Archived).Select(cm => cm.Id))
            {
                DeleteMedia(id);
            }
            _unitOfWork.Save();
        }
    }
}