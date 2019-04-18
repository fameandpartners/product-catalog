using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class CollectionProductRepository : BaseRepository<CollectionProduct>, ICollectionProductRepository
    {
        public CollectionProductRepository(FameContext context) : base(context)
        {
        }
    }
}