using RulesEngine.RuleModel;
namespace RulesEngine.RuleParser
{
    interface IPlaceHolderParser<TEntity> : ICustomPlaceHolderProvider where TEntity : RuleAwareEntity
    {
        string ParsePlaceHolder(RuleEngineContext.RuleEngineContext context, string placeHolderExpression);

    }
}
