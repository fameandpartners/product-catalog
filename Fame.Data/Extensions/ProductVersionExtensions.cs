using Fame.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fame.Data.Extensions
{
    public static class ProductVersionExtensions
    {
        public static IQueryable<ProductVersion> GetLatest(this IQueryable<ProductVersion> repo, string productId, VersionState? versionState = null)
        {
            return repo
                .Where(pv => pv.ProductId == productId && (versionState == null || pv.VersionState == versionState.Value))
                .OrderByDescending(pv => pv.ProductVersionId)
                .Include((x) => x.Product)
                .Include((x) => x.Prices);
        }
    }
}
