using RulesEngine.RuleModel;
using System;

namespace RulesEngine.RuleParser
{
    interface IRuleExpressionParser<TEntity>:ICustomPlaceHolderProvider where TEntity : RuleAwareEntity
    {
        Func<TEntity, bool> ParserRule(RuleEngineContext.RuleEngineContext contex, string ruleExpression);
    }
}
