using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fame.Data.Models;
using Fame.Search.Models;
using Fame.Service.ChangeTracking;
using Fame.Service.DTO;
using Fame.Service.Services;
using Microsoft.Extensions.Logging;

namespace Fame.Search.Services
{
    public partial class IndexService : IIndexService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IProductCombinationService _productCombinationService;
        private readonly IWorkflowService _workflowService;
        private readonly IProductService _productService;
        private readonly IProductVersionService _productVersionService;
        private readonly IProductDocumentService _productDocumentService;
        private readonly IFacetService _facetService;
        private readonly IFacetBoostService _facetBoostService;
        private readonly IOccasionService _occasionService;
        private readonly ILogger<IndexService> _logger;
        private readonly IElasticSearch _elasticSearch;
        private readonly IComponentTypeService _componentTypeService;
        private readonly HttpClient _httpClient;


        public IndexService(
            IUnitOfWork unitOfWork,
            ICacheService cacheService,
            IProductCombinationService productCombinationService,
            IWorkflowService workflowService,
            IProductService productService,
            IProductVersionService productVersionService,
            IProductDocumentService productDocumentService,
            IComponentTypeService componentTypeService,
            IFacetService facetService,
            IFacetBoostService facetBoostService,
            IOccasionService occasionService,
            ILogger<IndexService> logger,
            IElasticSearch elasticSearch)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _productCombinationService = productCombinationService;
            _workflowService = workflowService;
            _productService = productService;
            _productVersionService = productVersionService;
            _productDocumentService = productDocumentService;
            _facetService = facetService;
            _facetBoostService = facetBoostService;
            _occasionService = occasionService;
            _logger = logger;
            _elasticSearch = elasticSearch;
            _componentTypeService = componentTypeService;
            _httpClient = new HttpClient();
        }

        public async Task FullIndex()
        {
            await DropIndex();
            var weightDictionary = _componentTypeService.GetWeightDictionary();
            var facetTaxonDictionary = _facetService.FacetTaxonDictionary();
            var componentRuleSets = _facetService.ComponentRuleSets();
            var facetBoostRules = _facetBoostService.GetBoostRules();
            var facetPriorities = _facetService.GetFacetPriorities();
            var occasionRuleSets = _occasionService.GetOccasionRuleSets();
            var occasionLookup = _occasionService.GetLookup();
            var index = 0;
            foreach (var product in _productService.GetIndexableProducts())
            {
                var productVersion = _productVersionService.GetLatest(product.ProductId);
                var productDocuments = new Dictionary<string, ProductDocument>();
                var pids = new Dictionary<string, string>();
                var productCollections = product.Collections.Select(c => c.CollectionId).ToList();

                foreach (var productCombination in _productCombinationService.GetCombinationsForProduct(productVersion, true))
                {
                    index++;
                    var allComponents = productCombination.Options
                        .OrderBy(c => c.ComponentId)
                        .Select(s => new { Id = s.ComponentId, s.Component.ComponentType.AggregateOnIndex, s.Component.ComponentType.ComponentTypeId, Name = s.Component.Title })
                        .ToList();
                    var productId = productCombination.ProductVersion.ProductId;
                    var productAggregationKey = $"{productId}~{string.Join("~", allComponents.Where(c => !c.AggregateOnIndex).Select(c => c.Id).ToList())}";

                    if (!productDocuments.ContainsKey(productAggregationKey))
                    {
                        productDocuments.Add(productAggregationKey, new ProductDocument
                        {
                            Index = index,
                            ProductName = product.Title,
                            ProductId = product.ProductId,
                            ProductVersionId = productVersion.ProductVersionId,
                            SortWeight = CalculateSortWeight(productCombination.Options, weightDictionary, facetBoostRules, product.ProductId, productCollections),
                            Id = productAggregationKey,
                            AggregatedFacets = new List<string>(),
                            ProductVariations = new List<ProductVariation>()
                        });
                    }
                    var currentProductDocument = productDocuments[productAggregationKey];

                    var ids = allComponents.Select(c => c.Id).OrderBy(c => c).ToList();
                    var componentIdPath = string.Join("~", ids);
                    var pid = $"{productId}~{componentIdPath}";
                    if (pids.ContainsKey(pid)) continue; // Temp fix to remove duplicates from ProductCombinationService
                    pids.Add(pid, pid);
                    ids.Add(productId);

                    var facets = MatchFacets(ids, componentRuleSets, productCollections);
                    var taxons = MatchTaxons(facetTaxonDictionary, facets);
                    currentProductDocument.AggregatedFacets = currentProductDocument.AggregatedFacets.Union(facets).ToList();
                    var variationMeta = CalculateVariationMeta(facets, facetPriorities);
                    var occassions = CalculateOccasions(ids, facets, occasionRuleSets, productCollections).Select(o => occasionLookup[o]).ToList();
                    currentProductDocument.Collections = productCollections;

                    if (productId == "FPG4196")
                    {
                        Console.WriteLine("Add ProductVariation:");
                        if (facets != null)
                        {
                            Console.WriteLine("facets:");
                            facets.ForEach(ft =>
                              {
                                  Console.WriteLine(ft);
                              });
                        }
                        Console.WriteLine(productId);
                        Console.WriteLine(pid);
                        Console.WriteLine(variationMeta.Description);
                        foreach (var ta in taxons)
                        {
                            Console.WriteLine(ta);
                        }
                    }

                    currentProductDocument.ProductVariations.Add(new ProductVariation()
                    {
                        Facets = facets,
                        ComponentIdPath = componentIdPath,
                        Price = CalculatePrice(productCombination),
                        ProductId = productId,
                        PID = pid,
                        Name = variationMeta.Name ?? product.Title,
                        Description = variationMeta.Description,
                        PrimarySilhouetteId = variationMeta.PrimarySilhouetteId,
                        PrimarySilhouetteName = variationMeta.PrimarySilhouetteName,
                        OccasionNames = occassions,
                        LengthName = allComponents.Where(c => c.ComponentTypeId == "length").FirstOrDefault()?.Name,
                        ColorName = allComponents.Where(c => c.ComponentTypeId == "color").FirstOrDefault()?.Name,
                        Taxons = taxons
                    });
                }

                await _productDocumentService.AddAsync(productDocuments.Select(kv => kv.Value).ToList());
                productDocuments.Clear();
            }
            _cacheService.DeleteAll(); // Requests may have been made while the index was running and only partial results would be cached.
            _unitOfWork.Save();
        }

        private List<string> MatchTaxons(IDictionary<string, string[]> facetTaxonDictionary, List<string> facets)
        {
            var taxons = new List<string>();
            facets.ForEach(f => { 
                if (facetTaxonDictionary.ContainsKey(f))
                { 
                    taxons.AddRange(facetTaxonDictionary[f]);      
                }
            });
            return taxons;
        }

        private static VariationMeta CalculateVariationMeta(List<string> facets, List<FacetPriority> facetPriorities)
        {
            var meta = new VariationMeta();

            foreach (var facetPriorityGroup in facetPriorities.GroupBy(f => f.FacetGroupOrder))
            {
                foreach (var facetPriority in facetPriorityGroup)
                {
                    if (!facets.Contains(facetPriority.FacetId)) continue;
                    meta.Name = MergeString(meta.Name, facetPriority.Name, " ");
                    meta.Description = MergeString(meta.Description, facetPriority.Description, "\n\n");
                    if (facetPriority.IsPrimarySilhouette) 
                    {
                        meta.PrimarySilhouetteId = facetPriority.FacetId;
                        meta.PrimarySilhouetteName = facetPriority.Name;
                    }
                    break;
                }
            }

            return meta;
        }
        
        private static List<string> CalculateOccasions(List<string> componentIds, List<string> facetIds, Dictionary<string, ComponentRuleSet> componentRuleSets, List<string> collections)
        {
            var ids = new List<string>();
            ids.AddRange(componentIds);
            ids.AddRange(facetIds);

            var occasions = new List<string>();

            foreach (var componentRuleSet in componentRuleSets)
            {
                if (componentRuleSet.Value.Collections.Any(c => collections.Contains(c)) && componentRuleSet.Value.IsMatch(ids))
                {
                    occasions.Add(componentRuleSet.Key);
                }
            }

            return occasions;
        }

        public class VariationMeta
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string PrimarySilhouetteId { get; set; }
            public string PrimarySilhouetteName { get; internal set; }
        }

        private Dictionary<string, int> CalculatePrice(ProductCombination productCombination)
        {
            var prices = new Dictionary<string, int>();

            // Add option prices in each localisation
            foreach (var option in productCombination.Options)
            {
                foreach (var optionPrice in option.Prices)
                {
                    if (!prices.ContainsKey(optionPrice.LocalisationCode)) prices.Add(optionPrice.LocalisationCode, 0);
                    prices[optionPrice.LocalisationCode] = prices[optionPrice.LocalisationCode] + optionPrice.PriceInMinorUnits;
                }
            }

            // Add base product price in each localisation
            foreach (var productVersionPrice in productCombination.ProductVersion.Prices)
            {
                if (!prices.ContainsKey(productVersionPrice.LocalisationCode)) prices.Add(productVersionPrice.LocalisationCode, 0);
                prices[productVersionPrice.LocalisationCode] = prices[productVersionPrice.LocalisationCode] + productVersionPrice.PriceInMinorUnits;
            }

            return prices;
        }

        private static decimal CalculateSortWeight(IReadOnlyCollection<Option> productOptions, IReadOnlyDictionary<string, SortWeight> weightDictionary, List<FacetBoostRule> boostRules, string productId, List<string> collections)
        {
            decimal weight = 1;
            var optionIds = productOptions.Select(s => s.OptionId).ToHashSet();
            foreach (var option in productOptions)
            {
                var isDefault = option.CompatibleOptions.Any(co => co.IsDefault && co.ParentOptionId.HasValue && optionIds.Contains(co.ParentOptionId.Value));
                if (isDefault)
                {
                    weight = weight * weightDictionary[option.Component.ComponentTypeId].Default;
                }
                else
                {
                    weight = weight * weightDictionary[option.Component.ComponentTypeId].Other;
                }
            }

            var ids = productOptions.Select(po => po.ComponentId).ToList();
            ids.Add(productId);
            foreach (var componentRuleSet in boostRules)
            {
                if (componentRuleSet.Rule.Collections.Any(c => collections.Contains(c)) && componentRuleSet.Rule.IsMatch(ids))
                {
                    weight = weight * componentRuleSet.BoostWeight;
                }
            }

            return weight;
        }

        private static List<string> MatchFacets(List<string> componentIds, Dictionary<string, ComponentRuleSet> componentRuleSets, List<string> collections)
        {
            var facets = new List<string>();
            
            foreach (var componentRuleSet in componentRuleSets)
            {
                if (componentRuleSet.Value.Collections.Any(c => collections.Contains(c)) && componentRuleSet.Value.IsMatch(componentIds))
                {
                    facets.Add(componentRuleSet.Key);
                }
            }

            return facets;
        }

        private static string MergeString(string currentString, string newString, string delimiter)
        {
            if (string.IsNullOrWhiteSpace(newString))
            {
                return currentString;
            }

            if (string.IsNullOrWhiteSpace(currentString))
            {
                return newString;
            }

            if (currentString.Contains(newString))
            {
                return currentString;
            }

            return $"{currentString}{delimiter}{newString}";
        }

        public async Task<bool> DropIndex()
        {
            _httpClient.BaseAddress = _elasticSearch.ConnectionUri;
            await _httpClient.DeleteAsync("/" + _elasticSearch.IndexName);
            return true;
        }
    }
}
