using RulesEngine.RuleExecutionRule;
using RulesEngine.RuleModel;
using RulesEngine.Rules;
using System.Collections.Generic;

namespace RulesEngine.Ruleset
{
    class Ruleset<TEntity>:IRuleset<TEntity> where TEntity : RuleAwareEntity
    {
        private readonly RuleEngineContext.RuleEngineContext _context;
        private readonly EntryCriteriaRule<TEntity> _entryPoint;
        private readonly RuleExecutionRuleEnum _ruleExecutionRule;
      
        private readonly List<IPropertyRule<TEntity>> _rules = new List<IPropertyRule<TEntity>>();
        
        internal Ruleset(RuleEngineContext.RuleEngineContext context, IEnumerable<IPropertyRule<TEntity>> rules, EntryCriteriaRule<TEntity> entryPoint,
            RuleExecutionRuleEnum ruleExecutionRule,string name, string description)
        {
            this._context = context;
            this._entryPoint = entryPoint;
            this._ruleExecutionRule = ruleExecutionRule;
            Name = name;
            Description = description;
           
            _rules.AddRange(rules);
        }

        public void Execute(TEntity entity)
        {
            if (!_entryPoint.Execute(entity))
                return;
            ExecutePropertyRule(entity);

        }

        private void ExecutePropertyRule(TEntity entity)
        {
            entity.SetExecutedRuleSet(this);
            entity.SetExecutedEntryCriteriaRule(_entryPoint);

            var executedRule = RuleExecutionFactory
                .CreateRuleExecutor(_ruleExecutionRule)
                .ExecuteRule(entity, _rules);

            ExecutedRule = executedRule;
            if (HasSuccessRule())
            {
                entity.SetExecutedRule(ExecutedRule);
                executedRule.SetPropertyValues(entity);
            }
        }

        public bool HasSuccessRule()
        {
            if (ExecutedRule == null)
                return false;
            return true;
        }

        public IRule ExecutedRule { get; private set; }
        public string Name { get; }
        public string Description { get; }
    }
}
