using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class IncompatibilityRepository : BaseRepository<Incompatibility>, IIncompatibilityRepository
    {
        public IncompatibilityRepository(FameContext context) : base(context)
        {
        }
    }
}