using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetCategoryConfigurationRepository : BaseRepository<FacetCategoryConfiguration>, IFacetCategoryConfigurationRepository
    {
        public FacetCategoryConfigurationRepository(FameContext context) : base(context)
        {
        }
    }
}