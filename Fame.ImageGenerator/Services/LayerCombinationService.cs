using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fame.Common.Extensions;
using Fame.Data.Models;
using Fame.ImageGenerator.DTO;
using Fame.ImageGenerator.Services.Interfaces;
using Fame.Service.DTO;
using Fame.Service.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Fame.Service;

namespace Fame.ImageGenerator.Services
{
    public class LayerCombinationService : ILayerCombinationService
    {
        protected IImageCacheService _imageCache;
        protected IDistributedCache _cache;

        public LayerCombinationService(ProductRenderCacheService imageCache, IDistributedCache distributedCache)
        {
            _imageCache = imageCache;
            _cache = distributedCache;
        }

        private async Task<IEnumerable<RenderLayer>> LoadLayers(string productId, Orientation orientation)
        {
            return (await _imageCache.List(FileMeta.GetFolder(productId, orientation, null, Size.ORIGINAL_RENDER_SIZE)))
                .Select(ParseFile)
                .ToList();
        }

        private ILookup<string, RenderLayer> MapLayers(IEnumerable<RenderLayer> layers)
        {
            return layers
                .SelectMany(l => l.Combinations.First().Select(component => new Tuple<string, RenderLayer>(component, l)))
                .ToLookup(
                    t => t.Item1,
                    t => t.Item2
                );
        }



        public async Task<ILookup<string, RenderLayer>> GetLayers(string productId, Orientation orientation)
        {
            var layers = await LoadLayers(productId, orientation);

            return MapLayers(layers);
        }

        public IEnumerable<FileMeta> GetRenderLayersForCombination(ILookup<string, RenderLayer> layers, ProductCombination combination)
        {
            var components = combination.Options
                .Select(x => x.ComponentId);

            return GetRenderLayersForCombination(layers, components);
        }


        public IEnumerable<FileMeta> GetRenderLayersForCombination(ILookup<string, RenderLayer> layers, IEnumerable<string> components)
        {
            var sanitizedComponents = components
                .Select(x => x.ToLower())
                .ToArray();

            var matchingLayers = sanitizedComponents
                .SelectMany(component => layers[component])
                .Where(l => LayerContainsAllComponents(l.Combinations, sanitizedComponents))
                .ToArray();

            var mostSpecificLayers = matchingLayers
                .Where(layer => IsMostSpecific(matchingLayers, sanitizedComponents, layer))
                .OrderBy(l => l.ZIndex);

            return mostSpecificLayers.Select(x => x.File);
        }

        protected bool IsMostSpecific(IEnumerable<RenderLayer> layers, IEnumerable<string> components, RenderLayer layer)
        {
            if (layer.AlwaysInclude)
            {
                return true;
            }

            return !layers
                .Where(x => x.ZIndex == layer.ZIndex)
                .Any(l =>
            {
                var lCombinations = GetMatchingComponents(l.Combinations, components);
                var layerCombinations = GetMatchingComponents(layer.Combinations, components);

                return lCombinations.ContainsAll(layerCombinations) && !layerCombinations.ContainsAll(lCombinations);
            });
        }

        protected RenderLayer ParseFile(FileMeta file)
        {
            var components = file.FileName
                .ToLower()
                .Replace(".png", "")
                .Replace(".jpg", "")
                .Replace("~front", "")
                .Replace("~back", "")
                .Replace("~swatch", "")
                .Split("~");

            var layer = components.FirstOrDefault(x => x.StartsWith("layer", StringComparison.InvariantCultureIgnoreCase)) ?? "layer0";

            var alwaysInclude = components.FirstOrDefault(x => "always".Equals(x, StringComparison.InvariantCultureIgnoreCase));

            var combinations = components
                .Where(x => x != layer)
                .Where(x => x != alwaysInclude)
                .Select(x => x.Split(",").ToHashSet() as ISet<string>)
                .ToHashSet();

            var zindex = 0;
            Int32.TryParse(layer.Replace("layer", ""), out zindex);

            return new RenderLayer()
            {
                File = file,
                Combinations = combinations,
                ZIndex = zindex,
                AlwaysInclude = alwaysInclude != null,
            };
        }

        public IEnumerable<Zoom> GetZoomsForSize(Size size)
        {
            if (Size.OPTION_SIZES.Contains(size))
            {
                return new Zoom[] { Zoom.None, Zoom.Bottom, Zoom.Middle, Zoom.Top };
            }
            else
            {
                return new Zoom[] { Zoom.None };
            }
        }

        private static bool LayerContainsAllComponents(ISet<ISet<string>> layer, IEnumerable<string> components)
        {
            return layer.All(componentsInLayer => componentsInLayer.Any(componentInLayer => components.Contains(componentInLayer)));
        }

        private static IEnumerable<string> GetMatchingComponents(ISet<ISet<string>> layer, IEnumerable<string> components)
        {
            return components.Where(component => layer.Any(x => x.Contains(component)));
        }

        public async Task<ILookup<string, RenderLayer>> GetLayersCached(string productId, Orientation orientation)
        {
            var cacheKey = CacheKey.Create(GetType(), "LoadLayers", productId, orientation);

            var layers = await _cache.GetOrSetAsync(
                cacheKey,
                () => LoadLayers(productId, orientation),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                }
            );

            return MapLayers(layers);
        }

        public async Task DeleteLayerCache(string productId)
        {
            foreach (var orientation in Enum.GetValues(typeof(Orientation)).Cast<Orientation>())
            {
                await _cache.RemoveAsync(CacheKey.Create(GetType(), "LoadLayers", productId, orientation).Key);
            }
        }
    }
}
