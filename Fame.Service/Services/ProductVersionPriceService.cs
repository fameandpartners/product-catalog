using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class ProductVersionPriceService : BaseService<ProductVersionPrice>, IProductVersionPriceService
    {
        private readonly IRepositories _repositories;

        public ProductVersionPriceService(IRepositories repositories)
            : base(repositories.ProductVersionPrice.Value)
        {
            _repositories = repositories;
        }
    }
}