using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Fame.Common;
using Fame.Common.Extensions;
using Fame.Data.Migrations;
using Fame.Data.Models;
using Fame.Search.DTO;
using Fame.Search.Models;
using Fame.Service.DTO;
using Fame.Service.Services;
using Hangfire;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using FacetCount = Fame.Search.DTO.FacetCount;
using ProductSummary = Fame.Search.DTO.ProductSummary;
using SortOrder = Fame.Search.DTO.SortOrder;

namespace Fame.Search.Services
{
    public class ProductDocumentService : IProductDocumentService
    {
        private readonly FameConfig _fameConfig;
        private readonly IElasticSearch _elasticSearch;
        private readonly IFacetService _facetService;
        private readonly IFacetConfigurationService _facetConfigurationService;
        private readonly ILogger<ProductDocumentService> _logger;
        private readonly IDistributedCache _distributedCache;

        public ProductDocumentService(
            IOptions<FameConfig> fameConfig, 
            IElasticSearch elasticSearch, 
            IFacetConfigurationService facetConfigurationService, 
            ILogger<ProductDocumentService> logger,
            IDistributedCache distributedCache,
            IFacetService facetService)
        {
            _facetConfigurationService = facetConfigurationService;
            _logger = logger;
            _distributedCache = distributedCache;
            _fameConfig = fameConfig.Value;
            _elasticSearch = elasticSearch;
            _facetService = facetService;
        }

        public async Task<PIDModel> SetVariationMetaAsync(PIDModel pidModel)
        {
            return (await SetVariationMetaAsync(new List<PIDModel> {pidModel})).FirstOrDefault();
        }

        public async Task<List<PIDModel>> SetVariationMetaAsync(List<PIDModel> pidModels)
        {
            if (pidModels.Any(p => p.InvalidCombination)) return null;
            var cacheKey = CacheKey.Create(CachePrefix.Curation, GetType(), "SetVariationMetaFromSearchAsync", string.Join("-", pidModels.Select(p => p.PID)));
            var variationMeta = (await _distributedCache.GetOrSetAsync(cacheKey, () => GetVariationMetaAsync(pidModels.Select(p => p.ProductDocumentVersionId).ToList())));
            pidModels.ForEach(p => {
                if (variationMeta.ContainsKey(p.ProductDocumentVersionId)) {
                    var productVariation = variationMeta[p.ProductDocumentVersionId];
                    p.Name = productVariation.Name;
                    p.Description = productVariation.Description;
                    p.PrimarySilhouetteId = productVariation.PrimarySilhouetteId;
                    p.TaxonString = productVariation.TaxonString;
                }
            });
            return pidModels;
        }

        private async Task<Dictionary<string, ISearchMeta>> GetVariationMetaAsync(List<string> ProductDocumentVersionIds)
        {
            var productVariationDictionary = await GetProductVariations(ProductDocumentVersionIds);

			var result = new Dictionary<string, ISearchMeta>();
            foreach (var productDocumentVersionId in ProductDocumentVersionIds)
            {
				if (productVariationDictionary.ContainsKey(productDocumentVersionId) && !result.ContainsKey(productDocumentVersionId))
				{
					var productVariation = productVariationDictionary[productDocumentVersionId];
                    if (productVariation != null) 
                    {
                        var searchMeta = new SearchMeta
                        {
                            ProductDocumentVersionId = productDocumentVersionId,
                            Name = productVariation.Name,
                            Description = productVariation.Description,
                            PrimarySilhouetteId = productVariation.PrimarySilhouetteId,
                            TaxonString = string.Join("|", productVariation.Taxons),
                        };
                        result.Add(productDocumentVersionId ,searchMeta);
                    }
				}
            }

            return result;
        }

        private async Task<Dictionary<string, ProductVariation>> GetProductVariations(List<string> ProductDocumentVersionIds)
		{
			var must = new List<QueryContainer>();
            var container = new QueryContainer();

            foreach (var ProductDocumentVersionId in ProductDocumentVersionIds)
            {
                container |= new TermQuery { Field = "productVariations.pID.keyword", Value = ProductDocumentVersionId };
            }
            must.Add(container);

            var searchRequest = new SearchRequest<ProductDocument>
            {
                Size = ProductDocumentVersionIds.Count,
                Query = new BoolQuery { Must = must }
            };

            //var json = _elasticSearch.Client.RequestResponseSerializer.SerializeToString(searchRequest);

            var searchResults = (await _elasticSearch.Client.SearchAsync<ProductDocument>(searchRequest)).Documents;

			var result = new Dictionary<string, ProductVariation>();
            foreach (var productDocumentVersionId in ProductDocumentVersionIds)
            {
                var productVariation = searchResults.SingleOrDefault(s => s.ProductVariations.Any(pv => pv.PID == productDocumentVersionId))?.ProductVariations.FirstOrDefault(pv => pv.PID == productDocumentVersionId);
                if (!result.ContainsKey(productDocumentVersionId))
                {
                    result.Add(productDocumentVersionId, productVariation);
                }
            }

			return result;
		}

        [DisableConcurrentExecution(600)]
        public async Task AddAsync(List<ProductDocument> productDocuments, int maxPage = 50)
        {
            if (maxPage > 30)
                maxPage = 30;
            for (var i = 0 ; i < productDocuments.Count ; i = i + maxPage) {
                var bulkRequest = new BulkRequest(_fameConfig.Elastic.SearchIndexName) {Operations = new List<IBulkOperation>()};
                foreach (var productDocument in productDocuments.Skip(i).Take(maxPage).ToList())
                {
                    bulkRequest.Operations.Add(new BulkIndexOperation<ProductDocument>(productDocument));
                }
                var response = await _elasticSearch.Client.BulkAsync(bulkRequest);
                if (!response.IsValid) _logger.LogError($"ERROR ADDING PRODUCT TO SEARCH{response.DebugInformation.Take(1000).ToString().Replace("{", " ").Replace("}", " ")}");
                else
                    _logger.LogInformation("ADDING SUCCESS");
            }
        }

        public async Task<ProductResult> GetAsync(SearchArgs searchArgs)
        {
            var cacheKey = CacheKey.Create(GetType(), "GetProductResult", searchArgs);
            return await _distributedCache.GetOrSetAsync(cacheKey,() => GetProductResult(searchArgs));
        }

        private async Task<ProductResult> GetProductResult(SearchArgs searchArgs)
        {
            var pageSize = searchArgs.PageSize == 0 ? _fameConfig.Elastic.SearchPageSize : searchArgs.PageSize;
            
            var groupedFacets = _facetService.GroupedFacets(searchArgs.Collections, searchArgs.Facets);
            var nonAggregatedSelectedGroupedFacets = groupedFacets.Where(gf => gf.Facets.Any(f => f.Selected) && !gf.IsAggregated).Select(gf => new { gf.GroupId, Facets = gf.Facets.Where(f => f.Selected).Select(f=>f.FacetId) });

            var must = new List<QueryContainer>
            {
                new NumericRangeQuery {Field = new Field(ProductDocument.WeightProperty), GreaterThan = 0}
            };

            if (searchArgs.Collections != null)
            {
                var container = new QueryContainer();
                foreach (var collection in searchArgs.Collections)
                {
                    container |= new MatchQuery { Field = new Field(ProductDocument.CollectionsProperty), Query = collection };
                }
                must.Add(container);
            }
            
            foreach (var groupedFacet in groupedFacets.Where(gf => gf.Facets.Any(f => f.Selected)))
            {
                var container = new QueryContainer();
                foreach (var facet in groupedFacet.Facets.Where(f => f.Selected))
                {
                    container |= new MatchQuery {Field = new Field(ProductDocument.FacetProperty),Query = facet.FacetId};
                }
                must.Add(container);
            }

            var searchRequest = new SearchRequest<ProductDocument>
            {
                Size = pageSize,
                Query = new BoolQuery
                {
                    Must = must
                },
                Sort = new List<ISort>
                {
                    new SortField {
                        Field = string.IsNullOrEmpty(searchArgs.SortField) ? ProductSortField.SortWeight : searchArgs.SortField, 
                        Order = searchArgs.SortOrder == SortOrder.Ascending ? Nest.SortOrder.Ascending : Nest.SortOrder.Descending
                    },
                    new SortField
                    {
                        Field = ProductDocument.IndexProperty, 
                        Order = Nest.SortOrder.Ascending
                    },
                },
                SearchAfter = searchArgs.SearchAfter,
            };

            var json = _elasticSearch.Client.RequestResponseSerializer.SerializeToString(searchRequest);

            var results = (await _elasticSearch.Client.SearchAsync<ProductDocument>(searchRequest)).Documents;
            
            var taskList = groupedFacets.Select(gf => GetFacetCountAsync(gf, groupedFacets));
            var aggResults = await Task.WhenAll(taskList);
            var facetGroups = ConsolidateAggregations(aggResults);

            var aggregatedFacetGroups = new List<KeyValuePair<string, List<string>>>();
            foreach (var facetGroupSummary in facetGroups.Where(fg => fg.Value.IsAggregated))
            {
                var facetIds = facetGroupSummary.Value.Facets.Select(f => f.FacetId).ToList();
                if (facetIds.Any(f => searchArgs.Facets.Contains(f))) facetIds = facetIds.Where(f => searchArgs.Facets.Contains(f)).ToList();
                aggregatedFacetGroups.Add(new KeyValuePair<string, List<string>>(facetGroupSummary.Key, facetIds));
            }

            var index = 0;
            var productResults = new List<ProductSummary>();
            foreach (var productDocument in results)
            {
                ProductVariation productVariation = null;
                var potentialMatches = nonAggregatedSelectedGroupedFacets.Any() ? productDocument.ProductVariations.Where(pv => nonAggregatedSelectedGroupedFacets.All(nafg => nafg.Facets.Any(f => pv.Facets.Contains(f)) )).ToList() : productDocument.ProductVariations;
                switch (potentialMatches.Count)
                {
                    case 0:
                        _logger.LogWarning("Warning: Cannot find ProductVariation in ProductDocument Search");
                        break;
                    case 1:
                        productVariation = potentialMatches.First();
                        break;
                    default:
                        productVariation = CalculateProductVariation(potentialMatches, aggregatedFacetGroups, index);
                        break;
                }

                index++;

                if (productVariation == null) continue;

                productResults.Add(new ProductSummary
                {
                    Price = productVariation.Price,
                    PID = productVariation.PID,
                    ComponentIdPath = productVariation.ComponentIdPath,
                    ProductId = productVariation.ProductId,
                    Name = productVariation.Name,
                    ProductVersionId = productDocument.ProductVersionId,
                    SortWeight = productDocument.SortWeight,
                    ////Diagnostic fields
                    //Facets = productVariation.Facets,
                    //Index = productDocument.Index
                });
            }
            
            var lastDocument = results.LastOrDefault();
            var productReult = new ProductResult()
            {
                FacetConfigurations = _facetConfigurationService.AllConfigurations(),
                FacetGroups = facetGroups,
                Results = productResults,
                LastValue = lastDocument?.SortWeight,
                LastIndex = lastDocument?.Index,
                HasMore = results.Count == pageSize
            };

            return productReult;
        }

        private static Dictionary<string, FacetGroupSummary> ConsolidateAggregations(IReadOnlyCollection<AggregationGroup> aggregationGroups)
        {
            var baseFacetGroup = aggregationGroups.SingleOrDefault(ag => ag.GroupedFacet.IsCategoryFacet);
            baseFacetGroup?.RemoveZeroDocCount();
            foreach (var aggregationGroup in aggregationGroups.Where(ag => !ag.GroupedFacet.IsCategoryFacet))
            {
                if (baseFacetGroup == null) baseFacetGroup = aggregationGroup;
                else baseFacetGroup.Update(aggregationGroup);
            }
            return baseFacetGroup.FacetGroups.ToDictionary(fg => fg.GroupId, fg => fg);
        }
        
        private async Task<AggregationGroup> GetFacetCountAsync(GroupedFacet groupedFacet, List<GroupedFacet> groupedFacets)
        {
            var allFacets = groupedFacets.SelectMany(g => g.Facets).ToList();
            
            var currentGroupedFacets = groupedFacet.IsCategoryFacet ? groupedFacets.Where(fg => fg.IsCategoryFacet) : groupedFacets.Where(fg => fg.GroupId != groupedFacet.GroupId);
            
            var aggregationsContainer = new List<QueryContainer>();
            allFacets.ForEach(f => aggregationsContainer.Add(new TermQuery { Field = new Field(ProductDocument.FacetProperty), Value = f.FacetId }));

            var must = new List<QueryContainer>();

            foreach (var currentGroupedFacet in currentGroupedFacets.Where(fg => fg.Facets.Any(f => f.Selected)))
            {
                var container = new QueryContainer();
                foreach (var facet in currentGroupedFacet.Facets.Where(f => f.Selected))
                {
                    container |= new MatchQuery { Field = new Field(ProductDocument.FacetProperty), Query = facet.FacetId };
                }
                must.Add(container);
            }

            var searchRequest = new SearchRequest<ProductDocument>
            {
                Size = 0,
                Query = new BoolQuery { Must = must },
                Aggregations = new AggregationDictionary
                {
                    {
                        "facet_aggs1",
                        new AggregationContainer
                        {
                            Filters = new FiltersAggregation("facet_filters")
                            {
                                Filters = aggregationsContainer
                            }
                        }
                    },
                }
            };

            //var json = _elasticSearch.Client.RequestResponseSerializer.SerializeToString(searchRequest);

            var aggregateDictionary = (await _elasticSearch.Client.SearchAsync<ProductDocument>(searchRequest)).Aggregations;

            var facetCounts = aggregateDictionary
                .SelectMany(kv => ((BucketAggregate)kv.Value).Items)
                .Select(b => ((FiltersBucketItem)b).DocCount)
                .Select((t, i) => new FacetCount
                {
                    DocCount = t,
                    FacetId = allFacets[i].FacetId,
                    Title = allFacets[i].Title,
                    Order = allFacets[i].Order,
                    PreviewImage = allFacets[i].PreviewImage,
                    Subtitle = allFacets[i].Subtitle,
                    FacetMeta = allFacets[i].FacetMeta
                })
                .ToDictionary(fc => fc.FacetId, fc => fc);


            return new AggregationGroup
            {
                FacetGroups = groupedFacets.Select(g => new FacetGroupSummary
                {
                    GroupId = g.GroupId,
                    Facets = g.Facets.Select(f => new { facet = facetCounts[f.FacetId], selected = f.Selected }).Where(f => f.facet.DocCount > 0 || f.selected).Select(f => f.facet).OrderBy(f => f.Order).ToList(), 
                    Title = g.Title, 
                    Subtitle = g.Subtitle,
                    IsAggregated = g.IsAggregated, 
                    IsCategoryFacet = g.IsCategoryFacet, 
                    Collapsed = g.Collapsed, 
                    Multiselect = g.Multiselect,
                    Slug = g.Slug,
                    Name = g.Name
                }).ToList(),
                GroupedFacet = groupedFacet
            };
        }

        private ProductVariation CalculateProductVariation(IReadOnlyCollection<ProductVariation> potentialMatches, IReadOnlyCollection<KeyValuePair<string, List<string>>> aggregatedFacetGroups, int index)
        {
            if (!aggregatedFacetGroups.Any())
            {
                _logger.LogError("Warning: There are no Aggregated Facets to select a variation from.");
            }

            if (!potentialMatches.Any())
            {
                _logger.LogError("Warning: There are no potential matches to select a variation from.");
            }

            var isFinalLoop = aggregatedFacetGroups.Count == 1;

            var distinctFacetIds = potentialMatches.SelectMany(pm => pm.Facets).Distinct().ToList();
            var facetGroup = aggregatedFacetGroups.First().Value.Where(f => distinctFacetIds.Contains(f)).ToList();

            var sortedFacetIds = facetGroup.Select((t, i) => facetGroup[(index + i) % facetGroup.Count]).ToList();
            
            foreach (var facetId in sortedFacetIds)
            {
                var facetMatches = potentialMatches.Where(pv => pv.Facets.Any(f => f == facetId)).ToList();

                if (!facetMatches.Any()) continue;

                if (!isFinalLoop)
                {
                    var productVariation = CalculateProductVariation(facetMatches, aggregatedFacetGroups.Skip(1).ToList(), index);
                    if (productVariation != null) return productVariation;
                    continue;
                }

                if (facetMatches.Count > 0) return facetMatches.First();
            }

            return null;
        }

		internal class AggregationGroup
        {
            public GroupedFacet GroupedFacet { get; set; }
            public List<FacetGroupSummary> FacetGroups { get; set; }

            public void Update(AggregationGroup aggregationGroup)
            {
                var facetGroupId = aggregationGroup.GroupedFacet.GroupId;
                var categoryFacetGroup = FacetGroups.Single(fg => fg.GroupId == facetGroupId);
                var replacememntFacetGroup = aggregationGroup.FacetGroups.Single(fg => fg.GroupId == facetGroupId);
                categoryFacetGroup.Facets = replacememntFacetGroup.Facets;
            }

            public void RemoveZeroDocCount()
            {
                foreach (var facetGroupSummary in FacetGroups)
                {
                    facetGroupSummary.Facets = facetGroupSummary.IsCategoryFacet ? 
                        facetGroupSummary.Facets.Select(f => f.SetPositiveDocCount()).ToList() : 
                        facetGroupSummary.Facets.Where(f => f.DocCount > 0).ToList();
                }
            }
        }
    }

}
