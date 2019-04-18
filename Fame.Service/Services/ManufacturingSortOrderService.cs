using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class ManufacturingSortOrderService : BaseService<ManufacturingSortOrder>, IManufacturingSortOrderService
    {
        private readonly IRepositories _repositories;
        private readonly IManufacturingSortOrderRepository _manufacturingSortOrderRepo;

        public ManufacturingSortOrderService(IRepositories repositories)
            : base(repositories.ManufacturingSortOrder.Value)
        {
            _repositories = repositories;
            _manufacturingSortOrderRepo = repositories.ManufacturingSortOrder.Value;
        }

        public ManufacturingSortOrder Upsert(ManufacturingSortOrder manufacturingSortOrder)
        {
            var entityToUpdate = _manufacturingSortOrderRepo.FindById(manufacturingSortOrder.Id);

            // Insert if exists
            if (entityToUpdate == null)
            {
                _manufacturingSortOrderRepo.Insert(manufacturingSortOrder);
                return manufacturingSortOrder;
            }

            // Otherwise Update
            entityToUpdate.Order = manufacturingSortOrder.Order;
            _manufacturingSortOrderRepo.Update(entityToUpdate);
            return entityToUpdate;
        }
    }
}