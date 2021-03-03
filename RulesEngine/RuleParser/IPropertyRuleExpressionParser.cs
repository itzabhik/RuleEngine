using RulesEngine.RuleModel;
using System;

namespace RulesEngine.RuleParser
{
    internal interface IPropertyRuleExpressionParser<TEntity> : ICustomPlaceHolderProvider where TEntity : RuleAwareEntity
    {
        Func<TEntity, bool> ParserRule(RuleEngineContext.RuleEngineContext contex, string propertyName, string ruleExpression);
    }
}