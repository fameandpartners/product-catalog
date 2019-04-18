using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public class FacetService : BaseService<Facet>, IFacetService
    {
        private readonly IFacetRepository _facetRepo;
        private readonly IFacetGroupRepository _facetGroupRepo;
        private readonly IComponentTypeRepository _componentTypeRepo;

        public FacetService(IRepositories repositories)
            : base(repositories.Facet.Value)
        {
            _facetRepo = repositories.Facet.Value;
            _facetGroupRepo = repositories.FacetGroup.Value;
            _componentTypeRepo = repositories.ComponentType.Value;
        }

        public Facet Upsert(Facet facet)
        {
            var dbFacet = _facetRepo.FindById(facet.FacetId);

            // Insert if exists
            if (dbFacet == null)
            {
                _facetRepo.Insert(facet);
                return facet;
            }

            // Otherwise Update
            dbFacet.Title = facet.Title;
            dbFacet.Subtitle = facet.Subtitle;
            dbFacet.Order = facet.Order;
            dbFacet.PreviewImage = facet.PreviewImage;
            dbFacet.CompatibilityRule = facet.CompatibilityRule;
            dbFacet.TagPriority = facet.TagPriority;
            dbFacet.Description = facet.Description;
            dbFacet.Name = facet.Name;
            dbFacet.Collections = facet.Collections;
            dbFacet.TaxonString = facet.TaxonString;
            _facetRepo.Update(dbFacet);
            return dbFacet;
        }

        public List<string> FacetIds()
        {
            return _facetRepo.Get().Select(s => s.FacetId).ToList();
        }

        public List<GroupedFacet> GroupedFacets(List<string> collections, List<string> selectedFacetIds = null)
        {
            if (selectedFacetIds == null) selectedFacetIds = new List<string>();
            var facets = _facetGroupRepo.Get().Where(fg =>fg.Facets.Any(f => f.Collections.Any(c => collections.Contains(c.CollectionId)))).Select(fg => new GroupedFacet
            {
                GroupId = fg.FacetGroupId, 
                Title = fg.Title, 
                Subtitle = fg.Subtitle, 
                IsAggregated = fg.IsAggregatedFacet,
                IsCategoryFacet = fg.IsCategoryFacet,
                Multiselect = fg.Multiselect,
                Collapsed = fg.Collapsed,
                Slug = fg.Slug,
                Name = fg.Name,
                Facets = fg.Facets.Where(f => f.Collections.Any(c => collections.Contains(c.CollectionId))).Select(f => new FacetSummary
                {
                    FacetId = f.FacetId, 
                    Title = f.Title, 
                    Subtitle = f.Subtitle, 
                    Order = f.Order, 
                    PreviewImage = f.PreviewImage, 
                    FacetMeta = f.FacetMeta.ToDictionary(fm => fm.Key, fm => fm.Value),
                    Selected = false
                }).ToList()
            }).ToList();

            foreach (var groupedFacet in facets)
            {
                groupedFacet.Facets.ForEach(f => f.Selected = selectedFacetIds.Contains(f.FacetId));
            }

            return facets;
        }

        public Dictionary<string, ComponentRuleSet> ComponentRuleSets()
        {
            var componentRuleSets = new Dictionary<string, ComponentRuleSet>();
            foreach (var facet in _facetRepo.Get().Select(f => new {f.CompatibilityRule, f.FacetId, f.Collections}))
            {
                componentRuleSets.Add(facet.FacetId, new ComponentRuleSet(facet.CompatibilityRule, facet.Collections.Select(c => c.CollectionId).ToList()));
            }

            return componentRuleSets;
        }

        public List<FacetPriority> GetFacetPriorities()
        {
            return _facetRepo.Get()
                .Where(f => f.FacetGroup.ProductNameOrder.HasValue)
                .Select(f => new FacetPriority
                {
                    Description = f.Description, 
                    FacetId = f.FacetId, 
                    Name = f.Name, 
                    FacetGroupOrder = f.FacetGroup.ProductNameOrder.Value, 
                    TagPriority = f.TagPriority,
                    IsPrimarySilhouette = f.FacetGroup.PrimarySilhouette
                })
                .OrderBy(f => f.FacetGroupOrder)
                .ThenBy(f => f.TagPriority)
                .ToList();
        }
        public IDictionary<string, string> GetFacetDictionary()
        {
            return _facetRepo.Get().ToDictionary(f => f.FacetId, f => string.IsNullOrWhiteSpace(f.Name) ? f.FacetId : f.Name);
        }

        public IDictionary<string, string[]> FacetTaxonDictionary()
        {
            return _facetRepo.Get()
                .Where(f => f.TaxonString != null && f.TaxonString != string.Empty)
                .Select(f => new { f.FacetId, f.TaxonString })
                .ToList()
                .ToDictionary(f => f.FacetId, f => f.TaxonString.Split("|"));
        }
    }
}
