using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FacetConfigurationRepository : BaseRepository<FacetConfiguration>, IFacetConfigurationRepository
    {
        public FacetConfigurationRepository(FameContext context) : base(context)
        {
        }
    }
}