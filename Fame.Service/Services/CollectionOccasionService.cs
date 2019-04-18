using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class CollectionOccasionService : BaseService<CollectionOccasion>, ICollectionOccasionService
    {
        private readonly IRepositories _repositories;
        private readonly ICollectionOccasionRepository _collectionOccasionRepo;

        public CollectionOccasionService(IRepositories repositories)
            : base(repositories.CollectionOccasion.Value)
        {
            _repositories = repositories;
            _collectionOccasionRepo = repositories.CollectionOccasion.Value;
        }
    }
}