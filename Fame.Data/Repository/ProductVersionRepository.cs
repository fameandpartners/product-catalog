using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fame.Data.Extensions;
using Fame.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Fame.Data.Repository
{
    public class ProductVersionRepository : BaseRepository<ProductVersion>, IProductVersionRepository
    {
        public ProductVersionRepository(FameContext context) : base(context)
        { }

        public ProductVersion GetLatest(string productId, VersionState? versionState = null)
        {
            return DbSet.GetLatest(productId, versionState).FirstOrDefault();
        }

        public async Task<ProductVersion> GetLatestAsync(string productId, VersionState? versionState = null)
        {
            return await DbSet.GetLatest(productId, versionState).FirstOrDefaultAsync();
        }

        public IQueryable<ProductVersion> GetActive()
        {
            return DbSet
                .Where(pv => pv.VersionState == VersionState.Active)
                .Include(pv => pv.Product)
                .Include(pv => pv.Prices)
                .Include(pv => pv.Options)
                .ThenInclude(o => o.Component)
                .ThenInclude(c => c.ComponentType);
        }
    }
}