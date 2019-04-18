using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class RenderPositionRepository : BaseRepository<RenderPosition>, IRenderPositionRepository
    {
        public RenderPositionRepository(FameContext context) : base(context)
        {
        }
    }
}