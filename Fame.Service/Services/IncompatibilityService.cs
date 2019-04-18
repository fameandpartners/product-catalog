using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class IncompatibilityService : BaseService<Incompatibility>, IIncompatibilityService
    {
        private readonly IRepositories _repositories;

        public IncompatibilityService(IRepositories repositories)
            : base(repositories.Incompatibility.Value)
        {
            _repositories = repositories;
        }
    }
}