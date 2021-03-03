using System;
using System.Collections.Generic;
using RulesEngine.RuleModel;
using RulesEngine.Rules;

namespace RulesEngine.RuleExecutionRule
{
    class LastRuleExecutor : IRuleExecutor
    {
        public IPropertyRule<TEntity> ExecuteRule<TEntity>(TEntity entity, IEnumerable<IPropertyRule<TEntity>> ruleList) where TEntity : RuleAwareEntity

        {
            IPropertyRule<TEntity> appliedRule = null;
            foreach (var rule in ruleList)
            {
                if (!rule.Execute(entity))
                    continue;
                appliedRule = rule;
            }
            return appliedRule;
        }
    }
}
