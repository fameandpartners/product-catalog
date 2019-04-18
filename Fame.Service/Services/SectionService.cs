using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class SectionService : BaseService<Section>, ISectionService
    {
        private readonly IRepositories _repositories;

        public SectionService(IRepositories repositories)
            : base(repositories.Section.Value)
        {
            _repositories = repositories;
        }
    }
}