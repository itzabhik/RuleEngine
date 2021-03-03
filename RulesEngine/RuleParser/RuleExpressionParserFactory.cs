
using RulesEngine.RuleModel;

namespace RulesEngine.RuleParser
{
    static class RuleExpressionParserFactory
    {
        internal static IRuleExpressionParser<TEntity> CreateStringRuleParser<TEntity>() 
            where TEntity : RuleAwareEntity
        {
            return new StringRuleExpressionParser<TEntity>();
        }

        internal static IPropertyRuleExpressionParser<TEntity> CreateStringPropertyRuleParser<TEntity>()
            where TEntity : RuleAwareEntity
        {
            return new PropertyRuleExpressionParser<TEntity>();
        }

        internal static IPlaceHolderParser<TEntity> CreatePalceHolderParser<TEntity>()
           where TEntity : RuleAwareEntity
        {
            return new PlaceHolderParser<TEntity>();
        }

        internal static IGroupKeyExpressionParser<TEntity> CreateGroupKeyParser<TEntity>()
           where TEntity : RuleAwareEntity
        {
            return new GroupKeyExpressionParser<TEntity>();
        }

        internal static IAggregateFunctionParser<TEntity, TType> CreateAggregateFunctionParser<TEntity, TType>()
         where TEntity : RuleAwareEntity
        {
            return new AggregateFunctionParser<TEntity, TType>();
        }
    }
}
