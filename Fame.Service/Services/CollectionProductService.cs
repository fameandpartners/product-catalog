using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class CollectionProductService : BaseService<CollectionProduct>, ICollectionProductService
    {
        private readonly IRepositories _repositories;
        private readonly ICollectionProductRepository _collectionProductRepo;

        public CollectionProductService(IRepositories repositories)
            : base(repositories.CollectionProduct.Value)
        {
            _repositories = repositories;
            _collectionProductRepo = repositories.CollectionProduct.Value;
        }
    }
}