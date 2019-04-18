using Fame.Data.Models;
using Fame.Service.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fame.Service.Services
{
    public interface IComponentService : IBaseService<Component>
    {
        Component Upsert(Component component);
        Task<ProductionSheetViewModel> GetProductionSheetAsync(string pid);
        IDictionary<string, IEnumerable<string>> GetSizesDictionary();
    }
}