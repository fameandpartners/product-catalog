using Fame.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fame.Service.Services
{
    public interface IProductVersionService : IBaseService<ProductVersion>
    {
        ProductVersion CreateNewVersion(string productId, string factory, bool active);
        ProductVersion GetLatest(string productId);
        Task<Dictionary<string, ProductVersion>> GetProductVersionsAsync();
        int GetLatestProductVersionId(string productId);
    }
}