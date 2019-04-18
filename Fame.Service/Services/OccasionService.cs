using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Microsoft.EntityFrameworkCore;

namespace Fame.Service.Services
{
    public class OccasionService : BaseService<Occasion>, IOccasionService
    {
        private readonly IRepositories _repositories;
        private readonly IOccasionRepository _occasionRepo;

        public OccasionService(IRepositories repositories)
            : base(repositories.Occasion.Value)
        {
            _repositories = repositories;
            _occasionRepo = repositories.Occasion.Value;
        }

        public Dictionary<string, string> GetLookup()
        {
            return _occasionRepo.Get().ToDictionary(o => o.OccasionId, o => o.OccasionName);
        }

        public Dictionary<string, ComponentRuleSet> GetOccasionRuleSets()
        {
            var componentRuleSets = new Dictionary<string, ComponentRuleSet>();
            foreach (var occasion in _occasionRepo.Get().Include(c => c.Collections))
            {
                var compatibilityRules = string.Join("&", new string[] { occasion.ComponentCompatibilityRule, occasion.FacetCompatibilityRule }.Where(c => !string.IsNullOrEmpty(c)));
                var occasions = occasion.Collections.Select(c => c.CollectionId).ToList();
                componentRuleSets.Add(occasion.OccasionId, new ComponentRuleSet(compatibilityRules, occasions));
            }

            return componentRuleSets;
        }
    }
}