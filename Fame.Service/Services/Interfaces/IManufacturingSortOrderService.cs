using Fame.Data.Models;

namespace Fame.Service.Services
{
    public interface IManufacturingSortOrderService : IBaseService<ManufacturingSortOrder>{
        ManufacturingSortOrder Upsert(ManufacturingSortOrder manufacturingSortOrder);
    }
}