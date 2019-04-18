using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class FacetMetaService : BaseService<FacetMeta>, IFacetMetaService
    {
        private readonly IRepositories _repositories;
        private readonly IFacetMetaRepository _FacetMetaRepo;

        public FacetMetaService(IRepositories repositories)
            : base(repositories.FacetMeta.Value)
        {
            _repositories = repositories;
            _FacetMetaRepo = repositories.FacetMeta.Value;
        }
    }
}