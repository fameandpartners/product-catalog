using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Microsoft.EntityFrameworkCore;

namespace Fame.Service.Services
{
    public class FacetConfigurationService : BaseService<FacetConfiguration>, IFacetConfigurationService
    {
        private readonly IRepositories _repositories;
        private readonly IFacetConfigurationRepository _facetConfigurationRepo;

        public FacetConfigurationService(IRepositories repositories)
            : base(repositories.FacetConfiguration.Value)
        {
            _repositories = repositories;
            _facetConfigurationRepo = repositories.FacetConfiguration.Value;
        }      

        public FacetConfiguration Upsert(FacetConfiguration facetConfiguration)
        {
            var dbFacetConfiguration = _facetConfigurationRepo.FindById(facetConfiguration.FacetConfigurationId);

            // Insert if exists
            if (dbFacetConfiguration == null)
            {
                _facetConfigurationRepo.Insert(facetConfiguration);
                return facetConfiguration;
            }

            // Otherwise Update
            dbFacetConfiguration.Title = facetConfiguration.Title;
            dbFacetConfiguration.FacetCategoryConfigurations = facetConfiguration.FacetCategoryConfigurations;
            _facetConfigurationRepo.Update(dbFacetConfiguration);
            return dbFacetConfiguration;
        }

        public Dictionary<string, List<FacetCategorySummary>> AllConfigurations()
        {
            return _facetConfigurationRepo.Get()
                .Include(c => c.FacetCategoryConfigurations)
                    .ThenInclude(c => c.FacetCategory)
                        .ThenInclude(c => c.FacetCategoryGroups)
                            .ThenInclude(c => c.FacetGroup)
                .ToDictionary(
                    c => c.FacetConfigurationId,
                    c => c.FacetCategoryConfigurations
                        .OrderBy(fcc => fcc.FacetCategory.Sort)
                        .Select(cc => new FacetCategorySummary
                        {
                            Name = cc.FacetCategory.Title, 
                            HideHeader = cc.FacetCategory.HideHeader,
                            FacetGroupIds = cc.FacetCategory.FacetCategoryGroups.OrderBy(fc => fc.FacetGroup.Sort).Select(fcg => fcg.FacetGroupId).ToList()
                        }).ToList());
        }
    }
}
