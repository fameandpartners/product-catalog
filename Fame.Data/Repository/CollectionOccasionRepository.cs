using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class CollectionOccasionRepository : BaseRepository<CollectionOccasion>, ICollectionOccasionRepository
    {
        public CollectionOccasionRepository(FameContext context) : base(context)
        {
        }
    }
}