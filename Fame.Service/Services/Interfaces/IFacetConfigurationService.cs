using System.Collections.Generic;
using Fame.Data.Models;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public interface IFacetConfigurationService : IBaseService<FacetConfiguration>
    {
        FacetConfiguration Upsert(FacetConfiguration facetConfiguration);
        Dictionary<string,List<FacetCategorySummary>> AllConfigurations();
    }
}