using Fame.Data.Models;

namespace Fame.Service.Services
{
    public interface IRenderPositionService : IBaseService<RenderPosition>
    {
        RenderPosition Upsert(RenderPosition renderPosition);
        RenderPosition FindByRenderPositionId(string renderPositionId);
    }
}