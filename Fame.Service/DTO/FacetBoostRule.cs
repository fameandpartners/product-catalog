using System.Collections.Generic;

namespace Fame.Service.DTO
{
    public class FacetBoostRule
    {
        public FacetBoostRule(int facetBoostId, decimal boostWeight, string boostRule, List<string> collections)
        {
            FacetBoostId = facetBoostId;
            BoostWeight = boostWeight;
            Rule = new ComponentRuleSet(boostRule, collections);
        }
        public int FacetBoostId { get; }
        public decimal BoostWeight { get; }
        public ComponentRuleSet Rule { get; }
    }
}