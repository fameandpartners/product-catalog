using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fame.Service.DTO;

namespace Fame.Service
{
	public interface IIOService
	{
		Task<Stream> ReadFile(string path);
		Task WriteFile(FileMeta file, Stream data);
		Task<FileMeta> GetFile(string path);
		Task<IEnumerable<FileMeta>> ListFolder(string path);
		Task DeleteFile(FileMeta file);
        Task CopyBatch(Dictionary<string, List<string>> locationPaths);
    }
}
