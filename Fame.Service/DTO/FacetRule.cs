using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    public abstract class FacetRule
    {
        public OperatorType? Operator { get; set; }

        internal abstract bool ValidateMatch(List<string> componentIds);

        public bool IsMatch(List<string> componentIds, bool? isACurrentMatch = null)
        {
            if (isACurrentMatch.HasValue != Operator.HasValue) throw new ArgumentException("Invalid Component Rule.");

            var isMatch = ValidateMatch(componentIds);

            switch (Operator)
            {
                case OperatorType.And:
                    return isACurrentMatch.Value && isMatch;
                case OperatorType.Or:
                    return isACurrentMatch.Value || isMatch;
            }

            return isMatch;
        }
    }

    public class ComponentRule : FacetRule
    {
        private readonly bool _isANotRule;
        private readonly string _componentId;

        public ComponentRule(string componentId, OperatorType? operatorType)
        {
            if (componentId.StartsWith('!'))
            {
                _isANotRule = true;
                _componentId = componentId.Replace("!", "");
            }
            else
            {
                _componentId = componentId;
            }
            Operator = operatorType;
        }

        internal override bool ValidateMatch(List<string> componentIds)
        {
            var isMatch = componentIds.Contains(_componentId);
            if (_isANotRule) isMatch = !isMatch;
            return isMatch;
        }
    }


    public class ComponentRuleGroup : FacetRule
    {
        public List<ComponentRule> ComponentRules { get; set; } = new List<ComponentRule>();

        internal override bool ValidateMatch(List<string> componentIds)
        {
            bool? isACurrentMatch = null;
            foreach (var componentRule in ComponentRules)
            {
                isACurrentMatch = componentRule.IsMatch(componentIds, isACurrentMatch);
            }
            if (!isACurrentMatch.HasValue) throw new ArgumentException("Invalid Component Rule.");
            return isACurrentMatch.Value;
        }
    }

    public class ComponentRuleSet : FacetRule
    {
        private readonly List<ComponentRuleGroup> _ruleGroups = new List<ComponentRuleGroup>();

        public List<string> Collections { get; private set; }

        public ComponentRuleSet(string ruleStr, List<string> collections)
        {
            Collections = collections ?? new List<string>();
            if (!ruleStr.StartsWith("{")) throw new ArgumentException($"Invalid Component Rule: {ruleStr}");
            ComponentRuleGroup currentGroup = null;
            string currentComponent = null;
            OperatorType? currentOperator = null;
            bool? requireChar = null; // Ensure two operator aren't in a row.
            foreach (var c in ruleStr)
            {
                switch (c)
                {
                    case '{':
                        if (requireChar == true || currentGroup != null) throw new ArgumentException($"Invalid Component Rule. {ruleStr}");
                        currentGroup = new ComponentRuleGroup { Operator = currentOperator };
                        currentOperator = null;
                        requireChar = true;
                        break;
                    case '}':
                        if (requireChar == true || currentGroup == null) throw new ArgumentException($"Invalid Component Rule: {ruleStr}");
                        currentGroup.ComponentRules.Add(new ComponentRule(currentComponent, currentOperator));
                        _ruleGroups.Add(currentGroup);
                        currentGroup = null;
                        currentComponent = null;
                        currentOperator = null;
                        requireChar = false;
                        break;
                    case '|':
                        if (requireChar == true) throw new ArgumentException($"Invalid Component Rule: {ruleStr}");
                        if (currentComponent != null) currentGroup?.ComponentRules.Add(new ComponentRule(currentComponent, currentOperator));
                        currentComponent = null;
                        currentOperator = OperatorType.Or;
                        if (currentGroup != null) requireChar = true;
                        break;
                    case '&':
                        if (requireChar == true) throw new ArgumentException($"Invalid Component Rule: {ruleStr}");
                        if (currentComponent != null) currentGroup?.ComponentRules.Add(new ComponentRule(currentComponent, currentOperator));
                        currentComponent = null;
                        currentOperator = OperatorType.And;
                        if (currentGroup != null) requireChar = true;
                        break;
                    default:
                        if (requireChar == false || currentGroup == null) throw new ArgumentException($"Invalid Component Rule: {ruleStr}");
                        currentComponent = currentComponent + c;
                        requireChar = null;
                        break;
                }
            }
        }

        internal override bool ValidateMatch(List<string> componentIds)
        {
            bool? isACurrentMatch = null;
            foreach (var componentRule in _ruleGroups)
            {
                isACurrentMatch = componentRule.IsMatch(componentIds, isACurrentMatch);
            }
            if (!isACurrentMatch.HasValue) throw new ArgumentException("Invalid Component Rule.");
            return isACurrentMatch.Value;
        }
    }

    public enum OperatorType
    {
        And,
        Or
    }
}
