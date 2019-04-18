using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetCategoryGroupRepository : BaseRepository<FacetCategoryGroup>, IFacetCategoryGroupRepository
    {
        public FacetCategoryGroupRepository(FameContext context) : base(context)
        {
        }
    }
}