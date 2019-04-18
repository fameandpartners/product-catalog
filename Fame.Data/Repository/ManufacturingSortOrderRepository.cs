using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class ManufacturingSortOrderRepository : BaseRepository<ManufacturingSortOrder>, IManufacturingSortOrderRepository
    {
        public ManufacturingSortOrderRepository(FameContext context) : base(context)
        {
        }
    }
}