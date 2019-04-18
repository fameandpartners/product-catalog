using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Fame.Common;
using Fame.Service.DTO;
using Microsoft.Extensions.Options;

namespace Fame.Service.Services
{
    public class S3ProductRenderService : S3Service
    {
        public S3ProductRenderService(IAmazonS3 s3Client, IOptions<FameConfig> fameConfig) : base(s3Client, fameConfig.Value.ImageProcessing.S3BucketName)
        {}
    }

    public class S3CurationImageService : S3Service
    {
        public S3CurationImageService(IAmazonS3 s3Client, IOptions<FameConfig> fameConfig) : base(s3Client, fameConfig.Value.Curations.S3BucketName)
        { }
    }

    public class S3DocumentService : S3Service
    {
        public S3DocumentService(IAmazonS3 s3Client, IOptions<FameConfig> fameConfig) : base(s3Client, fameConfig.Value.Document.S3BucketName)
        { }
    }

    public abstract class S3Service : IIOService
    {
        private readonly string _bucketName;
        IAmazonS3 _s3Client;

        public S3Service(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
        }

        async public Task<FileMeta> GetFile(string path)
        {
            try
            {
                var metaData = await _s3Client.GetObjectMetadataAsync(_bucketName, path);

                return new FileMeta()
                {
                    FullPath = path,
                    LastModified = metaData.LastModified
                };
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw ex;
            }
        }

        async public Task DeleteFile(FileMeta file)
        {
            try
            {
                await _s3Client.DeleteObjectAsync(_bucketName, file.FullPath);
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return;
                }
                throw ex;
            }
        }

        async public Task<IEnumerable<FileMeta>> ListFolder(string folder)
        {
            IEnumerable<S3Object> results = Array.Empty<S3Object>();

            string continuationToken = null;
            do
            {
                var list = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request()
                {
                    BucketName = _bucketName,
                    ContinuationToken = continuationToken,
                    Prefix = folder
                });

                continuationToken = list.NextContinuationToken;

                results = results.Concat(list.S3Objects);


            } while (continuationToken != null);

            return results
                .Select(x => new FileMeta()
                {
                    FullPath = x.Key,
                    LastModified = x.LastModified
                })
                .ToList();
        }

        public async Task<Stream> ReadFile(string fullpath)
        {
            try
            {
                return await _s3Client.GetObjectStreamAsync(_bucketName, fullpath, new Dictionary<string, object>());
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw ex;
            }
        }

        public async Task WriteFile(FileMeta file, Stream data)
        {
            PutObjectRequest putRequest2 = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = file.FullPath,
                InputStream = data,
                AutoCloseStream = false,
                AutoResetStreamPosition = true
            };
            putRequest2.Metadata.Add("Last-Modified", file.LastModified.ToString());

            await _s3Client.PutObjectAsync(putRequest2);
        }

        public Task CopyBatch(Dictionary<string, List<string>> locationPaths)
        {
            throw new NotImplementedException();
        }
    }
}
