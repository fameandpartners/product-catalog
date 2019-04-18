using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetGroupRepository : BaseRepository<FacetGroup>, IFacetGroupRepository
    {
        public FacetGroupRepository(FameContext context) : base(context)
        {
        }
    }
}