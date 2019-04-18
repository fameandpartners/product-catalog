using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public class LocalFileService : IIOService
	{
		string _baseFolder;

		public LocalFileService(IConfiguration configuration)
		{
			_baseFolder = configuration["filecache"];
		}

		public LocalFileService(string baseFolder)
		{
			_baseFolder = baseFolder;
		}
        
		public async Task<FileMeta> GetFile(string relativePath)
		{
			var path = Path.Combine(_baseFolder, relativePath);

			if (!File.Exists(path))
			{
				return null;
			}

			return new FileMeta()
			{
				FullPath = relativePath,
				LastModified = File.GetLastWriteTime(path)
			};
		}

		public async Task<IEnumerable<FileMeta>> ListFolder(string folderName)
		{
			var path = Path.Combine(_baseFolder, folderName);
			return Directory
				.EnumerateFiles(path)
				.Select(file => (this as IIOService).GetFile(file).Result);
		}

		public Task<Stream> ReadFile(string fullpath)
		{
			return Task.FromResult<Stream>(File.OpenRead(Path.Combine(_baseFolder, fullpath)));
		}

		async public Task DeleteFile(FileMeta file)
		{
			File.Delete(file.FullPath);
		}

		async Task IIOService.WriteFile(FileMeta file, Stream data)
		{
			var path = GetLocalPath(file);
			var folder = Path.GetDirectoryName(path);

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			if (File.Exists(path))
			{
				File.Delete(path);
			}


			using (FileStream fileStream = File.Create(path))
			{
				await data.CopyToAsync(fileStream);
			}

			File.SetLastWriteTime(path, file.LastModified);
		}

		private string GetLocalPath(FileMeta file)
		{
			return Path.Combine(_baseFolder, file.FullPath);
		}

        public Task CopyBatch(Dictionary<string, List<string>> locationPaths)
        {
            throw new System.NotImplementedException();
        }
    }
}
