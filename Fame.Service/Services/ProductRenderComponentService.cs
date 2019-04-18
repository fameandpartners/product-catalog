using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class ProductRenderComponentService : BaseService<ProductRenderComponent>, IProductRenderComponentService
    {
        private readonly IRepositories _repositories;

        public ProductRenderComponentService(IRepositories repositories)
            : base(repositories.ProductRenderComponent.Value)
        {
            _repositories = repositories;
        }
    }
}