using RulesEngine.RuleModel;
using RulesEngine.Rules;
using System.Collections.Generic;

namespace RulesEngine.RuleExecutionRule
{
    interface IRuleExecutor
    {
        IPropertyRule<TEntity> ExecuteRule<TEntity>(TEntity entity, IEnumerable<IPropertyRule<TEntity>> ruleList) where TEntity : RuleAwareEntity;
         
    }
}
