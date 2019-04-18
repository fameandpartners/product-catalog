using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class FacetCategoryConfigurationService : BaseService<FacetCategoryConfiguration>, IFacetCategoryConfigurationService
    {
        private readonly IRepositories _repositories;

        public FacetCategoryConfigurationService(IRepositories repositories)
            : base(repositories.FacetCategoryConfiguration.Value)
        {
            _repositories = repositories;
        }
    }
}