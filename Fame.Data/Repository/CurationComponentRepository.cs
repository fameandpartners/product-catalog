using Fame.Data.Models;

namespace Fame.Data.Repository
{
	public class CurationComponentRepository : BaseRepository<CurationComponent>, ICurationComponentRepository
    {
        public CurationComponentRepository(FameContext context) : base(context)
        {
        }
    }
}