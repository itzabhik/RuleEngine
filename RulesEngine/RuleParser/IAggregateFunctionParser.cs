using RulesEngine.RuleModel;
using System;


namespace RulesEngine.RuleParser
{
    interface IAggregateFunctionParser<TEntity,TType> : ICustomPlaceHolderProvider where TEntity : RuleAwareEntity
    {
        Func<TEntity, TType> ParserRule(RuleEngineContext.RuleEngineContext contex, string ruleExpression);
    }
}
