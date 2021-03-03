using RulesEngine.RuleModel;
using RulesEngine.Rules;



namespace RulesEngine.Ruleset
{
    public interface IRuleset<TEntity>: IRuleset where TEntity : RuleAwareEntity
    {
        void Execute(TEntity entity);
    }

    public interface IRuleset
    {
        string Name { get; }
        string Description { get; }
        bool HasSuccessRule();
        IRule ExecutedRule { get; }

    }
}
