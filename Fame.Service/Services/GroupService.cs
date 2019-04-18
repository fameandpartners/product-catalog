using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class GroupService : BaseService<Group>, IGroupService
    {
        private readonly IRepositories _repositories;

        public GroupService(IRepositories repositories)
            : base(repositories.Group.Value)
        {
            _repositories = repositories;
        }
    }
}