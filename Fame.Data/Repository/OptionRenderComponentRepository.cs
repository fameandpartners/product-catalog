using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class OptionRenderComponentRepository : BaseRepository<OptionRenderComponent>, IOptionRenderComponentRepository
    {
        public OptionRenderComponentRepository(FameContext context) : base(context)
        {
        }
    }
}