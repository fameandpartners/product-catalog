using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class SectionGroupRepository : BaseRepository<SectionGroup>, ISectionGroupRepository
    {
        public SectionGroupRepository(FameContext context) : base(context)
        {
        }
    }
}