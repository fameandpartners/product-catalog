using System.Collections.Generic;
using Fame.Data.Models;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public interface IFacetService : IBaseService<Facet>
    {
        Facet Upsert(Facet facet);
        List<string> FacetIds();
        List<GroupedFacet> GroupedFacets(List<string> collections, List<string> facets = null);
        Dictionary<string,ComponentRuleSet> ComponentRuleSets();
        List<FacetPriority> GetFacetPriorities();
        IDictionary<string, string> GetFacetDictionary();
        IDictionary<string, string[]> FacetTaxonDictionary();
    }
}