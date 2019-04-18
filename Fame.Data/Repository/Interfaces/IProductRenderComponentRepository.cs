using System.Linq;
using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public interface IProductRenderComponentRepository : IBaseRepository<ProductRenderComponent>
    {
        IQueryable<string> GetRenderComponentIdsByProductVersionId(int productVersionId);
    }
}