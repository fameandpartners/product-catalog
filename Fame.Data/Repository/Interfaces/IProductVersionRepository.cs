using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public interface IProductVersionRepository : IBaseRepository<ProductVersion>
    {
        ProductVersion GetLatest(string productId, VersionState? versionState = null);
        Task<ProductVersion> GetLatestAsync(string productId, VersionState? active = null);
        IQueryable<ProductVersion> GetActive();
    }
}