using Fame.Data.Models;

namespace Fame.Data.Repository
{
	public class CurationMediaRepository : BaseRepository<CurationMedia>, ICurationMediaRepository
    {
        public CurationMediaRepository(FameContext context) : base(context)
        {
        }
    }
}