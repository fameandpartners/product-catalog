using System.Collections.Generic;
using Fame.Data.Models;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public interface IComponentTypeService : IBaseService<ComponentType>
    {
        ComponentType Upsert(ComponentType componentType);
        Dictionary<string, SortWeight> GetWeightDictionary();
    }
}