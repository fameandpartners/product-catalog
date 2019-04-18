using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class CollectionFacetBoostService : BaseService<CollectionFacetBoost>, ICollectionFacetBoostService
    {
        private readonly IRepositories _repositories;
        private readonly ICollectionFacetBoostRepository _collectionFacetBoostRepo;

        public CollectionFacetBoostService(IRepositories repositories)
            : base(repositories.CollectionFacetBoost.Value)
        {
            _repositories = repositories;
            _collectionFacetBoostRepo = repositories.CollectionFacetBoost.Value;
        }
    }
}