using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Fame.Common;
using Fame.Data.Models;
using Fame.Search.DTO;
using Fame.Search.Extensions;
using Fame.Search.Models;
using Fame.Service.ChangeTracking;
using Fame.Service.DTO;
using Fame.Service.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using System.IO.Compression;

namespace Fame.Search.Services
{
    public class ProductFeedService : IProductFeedService
    {
        private readonly FameConfig _fameConfig;
        private readonly IUnitOfWork _unitOfWork;
        private readonly S3DocumentService _documentService;
        private readonly IElasticSearch _elasticSearch;
        private readonly IFacetService _facetService;
        private readonly IFeedMetaService _feedMetaService;
        private readonly IComponentService _componentService;

        public ProductFeedService(
            IUnitOfWork unitOfWork,
            S3DocumentService documentService,
            IOptions<FameConfig> fameConfig,
            IElasticSearch elasticSearch,
            ILogger<ProductDocumentService> logger,
            IFacetService facetService,
            IFeedMetaService feedMetaService,
            IComponentService componentService)
        {
            _fameConfig = fameConfig.Value;
            _unitOfWork = unitOfWork;
            _documentService = documentService;
            _elasticSearch = elasticSearch;
            _facetService = facetService;
            _feedMetaService = feedMetaService;
            _componentService = componentService;
        }

        public async Task DeleteFeed(int id)
        {
            var entityToDelete = _feedMetaService.GetById(id);
            if (entityToDelete != null)
            {
                _feedMetaService.Delete(entityToDelete);
                await _documentService.DeleteFile(FileMeta.GetForDocument(entityToDelete));
            }
            _unitOfWork.Save();
        }

        public async Task GenerateFeed()
        {
            var priceAU = _fameConfig.Localisation.AU;
            var priceUS = _fameConfig.Localisation.US;
            var facetLookup = _facetService.GetFacetDictionary();
            var sizes = _componentService.GetSizesDictionary();
            var sizesAu = string.Join("|", sizes["sizeAu"]);
            var sizesUs = string.Join("|", sizes["sizeUs"]);

            var results = _elasticSearch.Client.GetAll<ProductDocument>(new MatchAllQuery());

            var data = results
                .SelectMany(r => r.ProductVariations)
                .Select(v => new FeedProduct()
                {
                    SKU = v.PID,
                    Title = v.Name,
                    Description = v.Description,
                    Keywords = string.Join("|", v.Facets.Select(f => facetLookup[f])),
                    Image = GenerateImageUrl(v.PID),
                    LinkAU = GenerateUrlAU(v.PID),
                    LinkUS = GenerateUrlUS(v.PID),
                    PriceAU = v.Price[priceAU] / 100,
                    PriceUS = v.Price[priceUS] / 100,
                    SizesAU = sizesAu,
                    SizesUS = sizesUs,
                    Silhouette = v.PrimarySilhouetteName,
                    Occasions = v.OccasionNames == null ? null : string.Join("|", v.OccasionNames),
                    Length = v.LengthName,
                    Color = v.ColorName,
                });

            var feedMeta = FeedMeta.Create("csv", true);

            string tempFile = Path.GetTempFileName();

            using (FileStream fs = new FileStream(tempFile, FileMode.Open))
            {
                using (var zipArchive = new ZipArchive(fs, ZipArchiveMode.Create, true))
                {
                    var zipFile = zipArchive.CreateEntry(feedMeta.FileName);
                    using (var zipStream = zipFile.Open())
                    {
                        using (var zipWriter = new StreamWriter(zipStream))
                        using (var csvWriter = new CsvWriter(zipWriter))
                        {
                            csvWriter.Configuration.HasHeaderRecord = true;
                            csvWriter.Configuration.AutoMap<FeedProduct>();
                            csvWriter.WriteRecords(data);
                        }
                    }
                }
            }

            using (var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
            { 
                await _documentService.WriteFile(FileMeta.GetForDocument(feedMeta), fileStream);
            }
            
            _feedMetaService.Insert(feedMeta);
            _unitOfWork.Save();
        }

        // TODO: Move to shared helper
        private string GenerateImageUrl(string pid)
        {
            var components = pid.Split("~").ToList();
            var productId = components.First();
            components.RemoveAt(0);

            return $@"https://product-renders-fallback.fameandgroups.com/ImagePreview/{productId}/{Orientation.Front}{(Zoom.None)}/{"704x704"}/{string.Join('~', components)}.png";
        }
        private string GenerateUrlAU(string pid)
        {
            return $@"https://www.fameandpartners.com.au/dresses/custom-dress-{pid}";
        }
        private string GenerateUrlUS(string pid)
        {
            return $@"https://www.fameandpartners.com/dresses/custom-dress-{pid}";
        }

    }
}
