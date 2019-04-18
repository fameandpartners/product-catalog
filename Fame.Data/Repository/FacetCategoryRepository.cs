using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetCategoryRepository : BaseRepository<FacetCategory>, IFacetCategoryRepository
    {
        public FacetCategoryRepository(FameContext context) : base(context)
        {
        }
    }
}