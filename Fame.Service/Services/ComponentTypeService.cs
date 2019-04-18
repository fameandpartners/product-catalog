using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public class ComponentTypeService : BaseService<ComponentType>, IComponentTypeService
    {
        private readonly IRepositories _repositories;
        private readonly IComponentTypeRepository _repo;

        public ComponentTypeService(IRepositories repositories)
            : base(repositories.ComponentType.Value)
        {
            _repositories = repositories;
            _repo = repositories.ComponentType.Value;
        }

        public ComponentType Upsert(ComponentType componentType)
        {
            var dbEntity = _repo.FindById(componentType.ComponentTypeId);
            if (dbEntity == null)
            {
                _repo.Insert(componentType);
                return componentType;
            }

            dbEntity.Title = componentType.Title;
            dbEntity.ComponentTypeCategory = componentType.ComponentTypeCategory;
            dbEntity.IsProductCode = componentType.IsProductCode;
            dbEntity.SelectionTitle = componentType.SelectionTitle;
            dbEntity.SortWeightDefault = componentType.SortWeightDefault;
            dbEntity.SortWeightOther = componentType.SortWeightOther;
            dbEntity.AggregateOnIndex = componentType.AggregateOnIndex;
            _repo.Update(dbEntity);
            return dbEntity;
        }

        public Dictionary<string, SortWeight> GetWeightDictionary()
        {
            return _repo.Get().ToDictionary(c => c.ComponentTypeId,
                c => new SortWeight
                {
                    Default = c.SortWeightDefault,
                    Other = c.SortWeightOther
                });
        }
    }
}