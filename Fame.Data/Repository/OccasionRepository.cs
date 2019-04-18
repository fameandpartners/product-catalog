using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class OccasionRepository : BaseRepository<Occasion>, IOccasionRepository
    {
        public OccasionRepository(FameContext context) : base(context)
        {
        }
    }
}