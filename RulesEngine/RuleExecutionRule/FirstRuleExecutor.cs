using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RulesEngine.RuleModel;
using RulesEngine.Rules;

namespace RulesEngine.RuleExecutionRule
{
    class FirstRuleExecutor : IRuleExecutor
    {
        public IPropertyRule<TEntity> ExecuteRule<TEntity>(TEntity entity, IEnumerable<IPropertyRule<TEntity>> rules) where TEntity : RuleAwareEntity

        {
            IPropertyRule<TEntity> appliedRule = rules.FirstOrDefault(r => r.Execute(entity));

            return appliedRule;
        }
    }
}
