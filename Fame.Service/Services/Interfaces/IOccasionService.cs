using System.Collections.Generic;
using Fame.Data.Models;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public interface IOccasionService : IBaseService<Occasion>
    {
        Dictionary<string, ComponentRuleSet> GetOccasionRuleSets();
        Dictionary<string, string> GetLookup();
    }
}