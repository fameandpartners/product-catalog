using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fame.Common.Extensions;
using Fame.Data.Models;
using Fame.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Fame.Service.Services
{
    public class ProductVersionService : BaseService<ProductVersion>, IProductVersionService
    {
        private readonly IProductVersionRepository _repo;
        private readonly IDistributedCache _distributedCache;

        public ProductVersionService(IRepositories repositories,
            IDistributedCache distributedCache)
            : base(repositories.ProductVersion.Value)
        {
            _repo = repositories.ProductVersion.Value;
            _distributedCache = distributedCache;
        }

        public ProductVersion CreateNewVersion(string productId, string factory, bool active)
        {
            var latestVersion = _repo.GetLatest(productId);
            if (latestVersion != null)
            {
                latestVersion.VersionState = VersionState.Archived;
                _repo.Update(latestVersion);
            }

            var productVersion = new ProductVersion
            {
                CreatedDate = DateTime.UtcNow,
                ProductId = productId,
                Factory = factory,
                VersionState = active ? VersionState.Active : VersionState.Pending
            };
            _repo.Insert(productVersion);
            return productVersion;
        }

        public ProductVersion GetLatest(string productId)
        {
            return _repo
                .Get()
                .Include(x => x.Prices)
                .Include(x => x.Options)
                    .ThenInclude(x => x.Component.RenderPosition)
                .Include(x => x.Options)
                    .ThenInclude(x => x.Prices)
                .Include(x => x.Options)
                .ThenInclude(x => x.Component.ComponentType)
                        .ThenInclude(x => x.ChildComponentTypes)
                .Include(x => x.Options)
                    .ThenInclude(x => x.Incompatibilities)
                        .ThenInclude(x => x.IncompatibleOptions)
                .Include(x => x.Options)
                    .ThenInclude(x => x.CompatibleOptions)
                        .ThenInclude(x => x.ParentOption)
                .Include(x => x.Options)
                    .ThenInclude(x => x.OptionRenderComponents)
                        .ThenInclude(x => x.ComponentType)
                .Include(x => x.Product)
                .Include(x => x.ProductRenderComponents)
                    .ThenInclude(x => x.ComponentType)
                .Include(x => x.Groups)
                    .ThenInclude(x => x.SectionGroups)
                        .ThenInclude(x => x.Sections)
                            .ThenInclude(x => x.ComponentType)
                .Include(x => x.Groups)
                    .ThenInclude(x => x.SectionGroups)
                        .ThenInclude(x => x.RenderPosition)
                .Where(x => x.Product.ProductId == productId)
                .OrderByDescending(x => x.CreatedDate)
                .First();
        }

        public int GetLatestProductVersionId(string productId)
        {
            return _repo
                .Get()
                .Where(x => x.Product.ProductId == productId)
                .OrderByDescending(x => x.CreatedDate)
                .Select(p => p.ProductVersionId)
                .First();
        }

        public async Task<Dictionary<string, ProductVersion>> GetProductVersionsAsync()
        {
            var cacheKey = CacheKey.Create(GetType(), "GetProductVersionsAsync");
            return await _distributedCache.GetOrSetAsync(cacheKey, () => _repo.GetActive().ToDictionaryAsync(d => d.ProductId, d => d));
        }
    }
}