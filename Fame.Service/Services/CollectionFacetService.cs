using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class CollectionFacetService : BaseService<CollectionFacet>, ICollectionFacetService
    {
        private readonly IRepositories _repositories;
        private readonly ICollectionFacetRepository _collectionFacetRepo;

        public CollectionFacetService(IRepositories repositories)
            : base(repositories.CollectionFacet.Value)
        {
            _repositories = repositories;
            _collectionFacetRepo = repositories.CollectionFacet.Value;
        }
    }
}