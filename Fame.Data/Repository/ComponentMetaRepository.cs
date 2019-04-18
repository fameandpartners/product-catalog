using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class ComponentMetaRepository : BaseRepository<ComponentMeta>, IComponentMetaRepository
    {
        public ComponentMetaRepository(FameContext context) : base(context)
        {
        }
    }
}