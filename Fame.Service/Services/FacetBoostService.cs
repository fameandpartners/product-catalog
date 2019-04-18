using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public class FacetBoostService : BaseService<FacetBoost>, IFacetBoostService
    {
        private readonly IFacetBoostRepository _facetBoostRepo;

        public FacetBoostService(IRepositories repositories)
            : base(repositories.FacetBoost.Value)
        {
            _facetBoostRepo = repositories.FacetBoost.Value;
        }

        public List<FacetBoostRule> GetBoostRules()
        {
            return _facetBoostRepo.Get().Select(f => new FacetBoostRule(f.FacetBoostId, f.BoostWeight, f.BoostRule, f.Collections.Select(c => c.CollectionId).ToList())).ToList();
        }
    }
}