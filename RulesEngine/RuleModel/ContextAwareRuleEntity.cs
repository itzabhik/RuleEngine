
using RulesEngine.Rules;
using RulesEngine.Ruleset;
using System;
namespace RulesEngine.RuleModel
{
    public class ContextAwareRuleEntity : IEntityIdProvider
    {
        internal DynamicContext _dynamicContext;
        private object _id;
        public ContextAwareRuleEntity(string enitityType)
        {

            EntityType = enitityType;
            _dynamicContext = new DynamicContext(enitityType);


        }
        public object Id { get => _id;
            set
            {
                if (IsIdSet())
                    throw new ArgumentException("Id is already set");
                _id = value;
            }
        }
        public string EntityType { get; }

        public IRuleset ExecutedRuleSet { get; private set; }

        public IRule ExecutedRule { get; private set; }

        public IRule ExecutedEntryCriteria { get; private set; }

        internal void SetExecutedRuleSet(IRuleset ruleSet)
        {
            ExecutedRuleSet = ruleSet;
        }

        internal void SetExecutedRule(IRule rule)
        {
            ExecutedRule = rule;
        }

        internal void SetExecutedEntryCriteriaRule(IRule rule)
        {
            ExecutedEntryCriteria = rule;
        }

        public bool IsIdSet()
        {
            return (_id != null);
        }

    }


}
