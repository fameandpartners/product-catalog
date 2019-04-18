using System.Linq;
using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class ProductRenderComponentRepository : BaseRepository<ProductRenderComponent>, IProductRenderComponentRepository
    {
        public ProductRenderComponentRepository(FameContext context) : base(context)
        {
        }

        public IQueryable<string> GetRenderComponentIdsByProductVersionId(int productVersionId)
        {
            return Get().Where(p => p.ProductVersionId == productVersionId).Select(p => p.ComponentTypeId);
        }
    }
}