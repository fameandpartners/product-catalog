using System.Collections.Generic;
using Fame.Data.Models;

namespace Fame.Service.Services
{
    public interface IProductService : IBaseService<Product>
    {
        Product Upsert(Product product);
        List<Product> GetIndexableProducts();
        List<string> GetAllDropNames(bool excludeForLayering = false, VersionState? versionState = null);
        List<string> GetAllProductIdsByDropName(string dropName);
        List<string> GetActiveProductIdsByDropName(string dropName);
    }
}