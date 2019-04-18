using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Autofac;
using Fame.Common;
using Fame.Data.Models;
using Fame.ImageGenerator.Services.Interfaces;
using Fame.Service.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Fame.Service;
using Fame.Service.DTO;
using Fame.Service.Services;

namespace Fame.ImageGenerator.Workers
{
    public class FileSyncMaster
    {
        private readonly IIOService _dropboxService;
        private readonly IIOService _s3Service;
        private readonly IAmazonSQS _sqsClient;
        private readonly Lazy<FileSyncWorker> _fileSyncWorker;
        private readonly string _snsQueue;
        private readonly ILayerCombinationService _layerCombinationService;

        public FileSyncMaster(DropboxService dropboxService, S3ProductRenderService s3Service, IAmazonSQS sqsClient, Lazy<FileSyncWorker> fileSyncWorker, IOptions<FameConfig> fameConfig, ILayerCombinationService layerCombinationService)
        {
            _dropboxService = dropboxService;
            _s3Service = s3Service;
            _sqsClient = sqsClient;
            _fileSyncWorker = fileSyncWorker;
            _snsQueue = fameConfig.Value.ImageProcessing.FileSyncQueue;
            _layerCombinationService = layerCombinationService;
        }

        public async Task<Response> Process(Request request)
        {
            var messages = Enum
                .GetValues(typeof(Orientation))
                .OfType<Orientation>()
                .Select(async orientation =>
                {
                    string folderName = $"{request.DropboxFolder}/{orientation}";
                    var dropboxContent = await _dropboxService.ListFolder(folderName);
                    var s3Content = await _s3Service.ListFolder(FileMeta.GetLayerBaseFolder(request.GroupId, orientation));
                    var s3ContentLookup = s3Content.ToLookup(x => x.FileName);

                    var countDeleted = s3ContentLookup
                                .Where(files => !dropboxContent.Any(dropBoxFile => dropBoxFile.FileName == files.Key))
                                .AsParallel()
                                .WithDegreeOfParallelism(2)
                                .Select(files =>
                                {
                                    foreach (var file in files)
                                    {
                                        _s3Service.DeleteFile(file).Wait();
                                    }
                                    return true;
                                })
                                .Count();

                    var count = dropboxContent
                        .Where(file => !AllZoomsExist(s3ContentLookup, file, orientation))
                        .Select(file => new FileSyncWorker.Request(file, request.GroupId, orientation))
                        .Batch(10)
                        .AsParallel()
                        .WithDegreeOfParallelism(20)
                        .Select(fileSyncRequests =>
                        {

                            _sqsClient.SendMessageBatchAsync(
                                queueUrl: _snsQueue,
                                entries: fileSyncRequests.Select((r, i) => new SendMessageBatchRequestEntry()
                                {
                                    Id = i.ToString(),
                                    MessageBody = JsonConvert.SerializeObject(r)
                                }).ToList()
                            ).Wait();

                            return fileSyncRequests.Count();
                        })
                        .Sum();

                    return $"Successfully processed folder '{folderName}' with '{count}' files, deleted '{countDeleted}'";
                })
                .Select(x => x.Result)
                .ToArray();

            return new Response(messages, request);
        }

        private Boolean AllZoomsExist(ILookup<string, FileMeta> files, FileMeta file, Orientation orientation)
        {
            var s3Files = files[file.FileName];

            return Size.ALL_SIZES.All(size =>
            {
                return _layerCombinationService.GetZoomsForSize(size).All((zoom) =>
                {
                    return s3Files.Any(s3File => s3File.LastModified >= file.LastModified && s3File.FullPath.Contains(orientation.ToString()) && s3File.FullPath.Contains(zoom.ToString()) && s3File.FullPath.Contains(size.Height.ToString()));
                });
            });
        }

        public class Response
        {
            public string[] Messages { get; set; }
            public Request Request { get; set; }

            public Response(string[] messages, Request request)
            {
                Messages = messages;
                Request = request;
            }
        }

        public class Request
        {
            public string GroupId { get; set; }
            public string DropboxFolder { get; set; }

            public Request(string groupId, string dropboxFolder)
            {
                GroupId = groupId;
                DropboxFolder = dropboxFolder;
            }
        }
    }
}
