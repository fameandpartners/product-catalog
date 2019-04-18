using System.Collections.Generic;
using Fame.Data.Models;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public interface IFacetBoostService : IBaseService<FacetBoost>
    {
        List<FacetBoostRule> GetBoostRules();
    }
}