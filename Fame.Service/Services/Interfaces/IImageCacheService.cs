using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fame.Service.DTO;

namespace Fame.Service
{
    public interface IImageCacheService
    {
        Task EnsureExists(FileMeta file, Func<Task<Stream>> loadFile);
        Task<Stream> GetOrSet(FileMeta file, Func<Task<Stream>> loadFile);
        Task<Stream> Get(FileMeta file);
        Task<bool> Exists(FileMeta file);
        Task Set(FileMeta file, Stream stream);
        Task<IEnumerable<FileMeta>> List(string folder);
    }
}
