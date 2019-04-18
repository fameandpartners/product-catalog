using Fame.Data.Models;

namespace Fame.Data.Repository
{
	public class CurationMediaVariantRepository : BaseRepository<CurationMediaVariant>, ICurationMediaVariantRepository
    {
        public CurationMediaVariantRepository(FameContext context) : base(context)
        {
        }
    }
}