using RulesEngine.RuleModel;
using RulesEngine.Ruleset;
using System.Collections.Generic;

namespace RulesEngine.GroupRuleSet
{
    public interface IGroupRuleSet<TEntity> where TEntity : RuleAwareEntity
    {
        IEnumerable<IRuleset<TEntity>> ExecutedRuleSet { get; }
        string GroupKey { get; }

        string Name { get; }
        string Description { get; }

        void Execute(IEnumerable<TEntity> entity);

        void ExecuteAsync(IEnumerable<TEntity> entity);
        bool HasSuccessRuleset();


    }
}
