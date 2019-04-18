using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(FameContext context) : base(context)
        {
        }
    }
}