using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class OptionRepository : BaseRepository<Option>, IOptionRepository
    {
        public OptionRepository(FameContext context) : base(context)
        {
        }
    }
}