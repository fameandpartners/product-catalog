using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class CurationRepository : BaseRepository<Curation>, ICurationRepository
    {
        public CurationRepository(FameContext context) : base(context)
        {
        }
    }
}