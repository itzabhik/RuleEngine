using RulesEngine.RuleModel;
using System;
using System.Linq.Dynamic.Core;

namespace RulesEngine.RuleParser
{
    interface IGroupKeyExpressionParser<TEntity>: ICustomPlaceHolderProvider where TEntity : RuleAwareEntity
    {
        Delegate ParseGroupString(RuleEngineContext.RuleEngineContext contex, string placeHolderExpression);
    }
}
