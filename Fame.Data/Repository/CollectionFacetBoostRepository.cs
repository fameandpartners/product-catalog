using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class CollectionFacetBoostRepository : BaseRepository<CollectionFacetBoost>, ICollectionFacetBoostRepository
    {
        public CollectionFacetBoostRepository(FameContext context) : base(context)
        {
        }
    }
}