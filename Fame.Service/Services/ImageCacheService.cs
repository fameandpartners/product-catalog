using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fame.Service.DTO;
using Fame.Service.Services;

namespace Fame.Service
{
    public class ProductRenderCacheService : ImageCacheService
    {
        public ProductRenderCacheService(S3ProductRenderService service) : base(service)
        {}
    }

    public class CurationImageCacheService : ImageCacheService
    {
        public CurationImageCacheService(S3CurationImageService service) : base(service)
        {}
    }

    public abstract class ImageCacheService : IImageCacheService
    {
        IIOService _ioService;

        public ImageCacheService(IIOService service)
        {
            _ioService = service;
        }

        public async Task EnsureExists(FileMeta file, Func<Task<Stream>> loadFile)
        {
            if (!await Exists(file))
            {
                using (var data = await loadFile())
                {
                    await Set(file, data);
                }
            }
        }

        public Task<Stream> Get(FileMeta file)
        {
            return _ioService.ReadFile(file.FullPath);
        }

        public async Task<Stream> GetOrSet(FileMeta file, Func<Task<Stream>> loadFile)
        {
            await EnsureExists(file, loadFile);

            return await Get(file);
        }

        public Task<IEnumerable<FileMeta>> List(string folder)
        {
            return _ioService.ListFolder(folder);
        }

        public Task Set(FileMeta file, Stream data)
        {
            return _ioService.WriteFile(file, data);
        }

        public async Task<bool> Exists(FileMeta fileMeta)
        {
            var file = await _ioService.GetFile(fileMeta.FullPath);

            if (file == null)
            {
                return false;
            }

            return file.LastModified >= fileMeta.LastModified;
        }
    }
}
