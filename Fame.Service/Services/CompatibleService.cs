using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class CompatibleOptionService : BaseService<CompatibleOption>, ICompatibleOptionService
    {
        private readonly IRepositories _repositories;

        public CompatibleOptionService(IRepositories repositories)
            : base(repositories.CompatibleOption.Value)
        {
            _repositories = repositories;
        }
    }
}