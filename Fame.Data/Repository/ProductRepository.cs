using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(FameContext context) : base(context)
        {
        }
    }
}
