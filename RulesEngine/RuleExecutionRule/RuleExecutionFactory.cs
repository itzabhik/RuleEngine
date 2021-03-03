using System;

namespace RulesEngine.RuleExecutionRule
{
    static class RuleExecutionFactory
    {
        internal static IRuleExecutor CreateRuleExecutor(RuleExecutionRuleEnum ruleExecutionRule)
        {
            switch (ruleExecutionRule)
            {
                case RuleExecutionRuleEnum.FirstMatch:
                    return new FirstRuleExecutor();
                case RuleExecutionRuleEnum.LastMatch:
                    return new LastRuleExecutor();
               
                default:
                    throw new ArgumentOutOfRangeException(nameof(ruleExecutionRule), ruleExecutionRule, null);

            }
        }
    }
}