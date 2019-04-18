using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetMetaRepository : BaseRepository<FacetMeta>, IFacetMetaRepository
    {
        public FacetMetaRepository(FameContext context) : base(context)
        {
        }
    }
}