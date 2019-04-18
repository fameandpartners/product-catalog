using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetRepository : BaseRepository<Facet>, IFacetRepository
    {
        public FacetRepository(FameContext context) : base(context)
        {
        }
    }
}