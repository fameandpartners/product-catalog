using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class SectionGroupService : BaseService<SectionGroup>, ISectionGroupService
    {
        private readonly IRepositories _repositories;

        public SectionGroupService(IRepositories repositories)
            : base(repositories.SectionGroup.Value)
        {
            _repositories = repositories;
        }
    }
}