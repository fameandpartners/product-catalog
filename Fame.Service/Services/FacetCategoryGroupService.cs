using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class FacetCategoryGroupService : BaseService<FacetCategoryGroup>, IFacetCategoryGroupService
    {
        private readonly IRepositories _repositories;

        public FacetCategoryGroupService(IRepositories repositories)
            : base(repositories.FacetCategoryGroup.Value)
        {
            _repositories = repositories;
        }
    }
}