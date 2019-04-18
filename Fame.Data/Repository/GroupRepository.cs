using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(FameContext context) : base(context)
        {
        }
    }
}