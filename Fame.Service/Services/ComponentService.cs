using Fame.Common;
using Fame.Common.Extensions;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Service.Services
{
    public class ComponentService : BaseService<Component>, IComponentService
    {
        private readonly IRepositories _repositories;
        private readonly FameConfig _fameConfig;
        private readonly IDistributedCache _distributedCache;
        private readonly IComponentRepository _componentRepo;

        public ComponentService(IRepositories repositories,
            IOptions<FameConfig> fameConfig,
            IDistributedCache distributedCache)
            : base(repositories.Component.Value)
        {
            _repositories = repositories;
            _fameConfig = fameConfig.Value;
            _distributedCache = distributedCache;
            _componentRepo = repositories.Component.Value;
        }
        
        public Component Upsert(Component component)
        {
            var dbcomponent = _componentRepo.FindById(component.ComponentId);

            // Insert if component exists
            if (dbcomponent == null)
            {
                _componentRepo.Insert(component);
                return component;
            }

            // Otherwise Update
            dbcomponent.Title = component.Title;
            dbcomponent.RenderPositionId = component.RenderPosition.RenderPositionId;
            dbcomponent.CartId = component.CartId;
            dbcomponent.Sort = component.Sort;
            dbcomponent.ComponentTypeId = component.ComponentTypeId;
            dbcomponent.Indexed = component.Indexed;
            dbcomponent.ComponentMeta = component.ComponentMeta;
            dbcomponent.ManufacturingSortOrder = component.ManufacturingSortOrder;
            dbcomponent.IsolateInSummary = component.IsolateInSummary;
            dbcomponent.PreviewZoom = component.PreviewZoom;
            _componentRepo.Update(dbcomponent);
            return dbcomponent;
        }
        
        public async Task<ProductionSheetViewModel> GetProductionSheetAsync(string pid)
        {
            var cacheKey = CacheKey.Create(GetType(), "InstantiateProductionSheetAsync", pid);

            var productionSheet = _distributedCache.GetOrSetAsync(
                cacheKey,
                () => InstantiateProductionSheetAsync(pid)
            );

            return await productionSheet;
        }

        private async Task<ProductionSheetViewModel> InstantiateProductionSheetAsync(string pid)
        {
            var pidItems = pid.Split("~");
            var productId = pidItems.First();
            var productVersion = await _repositories.ProductVersion.Value.GetLatestAsync(productId, VersionState.Active);
            var componentIds = pidItems.Skip(1).ToList();

            var components = await _repositories.Component.Value.Get()
                .Where(c => componentIds.Contains(c.ComponentId))
                .Select(c => new ComponentViewModel
                {
                    Title = c.Title,
                    ComponentId = c.ComponentId,
                    ComponentTypeId = c.ComponentTypeId,
                    IsolateInSummary = c.IsolateInSummary,
                    PreviewZoom = c.PreviewZoom
                })
                .OrderBy(c => c.ComponentId)
                .ToListAsync();

            return new ProductionSheetViewModel(productId, productVersion.Product.Title, _fameConfig.Render.DefaultColor, components, _fameConfig.Render.Url, productVersion.ProductVersionId);
        }

        public IDictionary<string, IEnumerable<string>> GetSizesDictionary()
        {
            return _repositories.Component.Value.Get()
                .Where(f => f.ComponentTypeId == "size")
                .SelectMany(f => f.ComponentMeta)
                .GroupBy(fm => fm.Key)
                .ToDictionary(f => f.Key, f => f.Select(fg => fg.Value));
        }
    }
}
