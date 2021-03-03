using RulesEngine.RuleModel;
using System.Collections.Generic;

namespace RulesEngine.RuleSet
{
    interface IGroupRuleSet<TEntity> 
    {
        IEnumerable<IRuleSet<TEntity>> ExecutedRuleSet { get; }
        string GroupExpression { get; }

        string Name { get; }
        string Description { get; }

        void Execute(IEnumerable<TEntity> entity);
        bool HasSuccessRuleset();


    }
}
