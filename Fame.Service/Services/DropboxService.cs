using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Fame.Common;
using Microsoft.Extensions.Options;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public class DropboxService : IIOService
    {
        private readonly DropboxClient _dropboxClient;
        private bool isComplete = false;

        public DropboxService(IOptions<FameConfig> fameConfig)
        {
            var accessToken = fameConfig.Value.Dropbox.AccessKey;
            _dropboxClient = new DropboxClient(accessToken);
        }

        public DropboxService(string accessToken)
        {
            _dropboxClient = new DropboxClient(accessToken);
        }

        public async Task<Stream> ReadFile(string fullPath)
        {
            var content = await _dropboxClient.Files.DownloadAsync(fullPath);
            using (var originalStream = await content.GetContentAsStreamAsync())
            {
                var memoryStream = new MemoryStream();
                await originalStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        public async Task<IEnumerable<FileMeta>> ListFolder(string folderName)
        {
            IEnumerable<Metadata> results = Array.Empty<Metadata>();

            string cursor = null;
            do
            {
                ListFolderResult list;
                if (cursor != null)
                {
                    list = await _dropboxClient.Files.ListFolderContinueAsync(cursor);
                }
                else
                {
                    list = await _dropboxClient.Files.ListFolderAsync(folderName);
                }


                cursor = list.HasMore ? list.Cursor : null;

                results = results.Concat(list.Entries);


            } while (cursor != null);

            return results
                .Where(e => e.IsFile)
                .Select(e => new FileMeta()
                {
                    FullPath = e.PathLower,
                    LastModified = e.AsFile.ServerModified
                })
                .ToList();
        }

        public async Task WriteFile(FileMeta file, Stream data)
        {
            await _dropboxClient.Files.UploadAsync(
                clientModified: file.LastModified,
                path: file.FullPath,
                body: data
            );
        }

        public Task<FileMeta> GetFile(string path)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(FileMeta file)
        {
            throw new NotImplementedException();
        }

        public async Task CopyBatch(Dictionary<string, List<string>> locationPaths)
        {
            foreach (var keyValuePair in locationPaths)
            {
                foreach (var pathItem in keyValuePair.Value)
                {
                    var relocationPath = new RelocationPath(keyValuePair.Key, pathItem);
                    try
                    {
                        // Although we could add all the locations in a batch they would fail so add them one at a time so we can catch the exception
                        await _dropboxClient.Files.CopyBatchAsync(new List<RelocationPath>() { relocationPath }, true, false, true);
                    }
                    catch (StructuredException<RelocationError>)
                    {
                        // swallow relocation errors
                    }
                }
            }
        }
    }
}
