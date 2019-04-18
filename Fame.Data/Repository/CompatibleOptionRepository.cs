using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class CompatibleOptionRepository : BaseRepository<CompatibleOption>, ICompatibleOptionRepository
    {
        public CompatibleOptionRepository(FameContext context) : base(context)
        {
        }
    }
}