using RulesEngine.RuleModel;

namespace RulesEngine.Rules
{
    public interface IRule<TEntity>:IRule where TEntity : RuleAwareEntity
    {
        bool Execute(TEntity entity);
    }

    public interface IRule
    {
        string Rule { get; }

        string RuleName { get; }

        string RuleDescription { get; }
    }
}
