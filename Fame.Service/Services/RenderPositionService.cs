using System.Linq;
using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class RenderPositionService : BaseService<RenderPosition>, IRenderPositionService
    {
        private readonly IRepositories _repositories;
        private readonly IRenderPositionRepository _renderPositionRepo;

        public RenderPositionService(IRepositories repositories)
            : base(repositories.RenderPosition.Value)
        {
            _repositories = repositories;
            _renderPositionRepo = repositories.RenderPosition.Value;
        }

        public RenderPosition Upsert(RenderPosition renderPosition)
        {
            var dbrenderPosition = _renderPositionRepo.FindById(renderPosition.RenderPositionId);

            // Insert if renderPosition exists
            if (dbrenderPosition == null)
            {
                _renderPositionRepo.Insert(renderPosition);
                return renderPosition;
            }

            // Otherwise Update
            dbrenderPosition.Orientation = renderPosition.Orientation;
            dbrenderPosition.Zoom = renderPosition.Zoom;
            _renderPositionRepo.Update(dbrenderPosition);
            return dbrenderPosition;
        }

        public RenderPosition FindByRenderPositionId(string renderPositionId) {
            return _renderPositionRepo.Get().Where(x => x.RenderPositionId == renderPositionId).Single();
        }
    }

}