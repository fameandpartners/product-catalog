using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class OptionPriceRepository : BaseRepository<OptionPrice>, IOptionPriceRepository
    {
        public OptionPriceRepository(FameContext context) : base(context)
        {
        }
    }
}