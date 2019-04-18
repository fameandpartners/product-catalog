using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class ComponentRepository : BaseRepository<Component>, IComponentRepository
    {
        public ComponentRepository(FameContext context) : base(context)
        {
        }
    }
}