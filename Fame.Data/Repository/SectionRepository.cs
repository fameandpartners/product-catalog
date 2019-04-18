using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class SectionRepository : BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(FameContext context) : base(context)
        {
        }
    }
}