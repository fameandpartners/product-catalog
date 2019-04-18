using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class ProductVersionPriceRepository : BaseRepository<ProductVersionPrice>, IProductVersionPriceRepository
    {
        public ProductVersionPriceRepository(FameContext context) : base(context)
        {
        }
    }
}