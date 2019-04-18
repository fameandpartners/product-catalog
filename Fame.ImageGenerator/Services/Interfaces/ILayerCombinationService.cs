using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fame.Data.Models;
using Fame.ImageGenerator.DTO;
using Fame.Service.DTO;
using Fame.Service;

namespace Fame.ImageGenerator.Services.Interfaces
{
    public interface ILayerCombinationService
    {
        Task<ILookup<string, RenderLayer>> GetLayers(string productId, Orientation orientation);
        Task<ILookup<string, RenderLayer>> GetLayersCached(string productId, Orientation orientation);
        Task DeleteLayerCache(string productId);
        IEnumerable<FileMeta> GetRenderLayersForCombination(ILookup<string, RenderLayer> layers, ProductCombination combination);
        IEnumerable<FileMeta> GetRenderLayersForCombination(ILookup<string, RenderLayer> layers, IEnumerable<string> combination);
        IEnumerable<Zoom> GetZoomsForSize(Size size);
    }
}
