using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class ComponentTypeRepository : BaseRepository<ComponentType>, IComponentTypeRepository
    {
        public ComponentTypeRepository(FameContext context) : base(context)
        {
        }
    }
}