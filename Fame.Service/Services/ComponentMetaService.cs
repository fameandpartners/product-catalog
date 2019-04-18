using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class ComponentMetaService : BaseService<ComponentMeta>, IComponentMetaService
    {
        private readonly IRepositories _repositories;
        private readonly IComponentMetaRepository _componentMetaRepo;

        public ComponentMetaService(IRepositories repositories)
            : base(repositories.ComponentMeta.Value)
        {
            _repositories = repositories;
            _componentMetaRepo = repositories.ComponentMeta.Value;
        }
    }
}