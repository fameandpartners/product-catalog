using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class CollectionFacetRepository : BaseRepository<CollectionFacet>, ICollectionFacetRepository
    {
        public CollectionFacetRepository(FameContext context) : base(context)
        {
        }
    }
}