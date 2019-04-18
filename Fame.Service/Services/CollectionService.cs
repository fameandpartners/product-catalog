using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class CollectionService : BaseService<Collection>, ICollectionService
    {
        private readonly IRepositories _repositories;
        private readonly ICollectionRepository _collectionRepo;

        public CollectionService(IRepositories repositories)
            : base(repositories.Collection.Value)
        {
            _repositories = repositories;
            _collectionRepo = repositories.Collection.Value;
        }
    }
}