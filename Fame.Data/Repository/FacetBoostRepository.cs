using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetBoostRepository : BaseRepository<FacetBoost>, IFacetBoostRepository
    {
        public FacetBoostRepository(FameContext context) : base(context)
        {
        }
    }
}