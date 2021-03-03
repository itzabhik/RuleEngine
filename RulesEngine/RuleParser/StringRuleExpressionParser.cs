using RulesEngine.RuleEngineContext;
using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleModel;
using System;
using System.Linq.Dynamic.Core;

namespace RulesEngine.RuleParser
{
    class StringRuleExpressionParser<TEntity> : AbstractCustomPlaceHolderProvider, IRuleExpressionParser<TEntity>
        where TEntity : RuleAwareEntity
    {
        public Func<TEntity, bool> ParserRule(RuleEngineContext.RuleEngineContext contex, string ruleExpression)
        {
            IPlaceHolderParser<TEntity> placeHolderParser = RuleExpressionParserFactory.CreatePalceHolderParser<TEntity>();
            placeHolderParser.AddPlaceHolders(_placeholders);

            var strexpression = placeHolderParser.ParsePlaceHolder(contex, ruleExpression);

            try
            {
                var expression = DynamicExpressionParser.
                                            ParseLambda<TEntity, bool>(contex.EntityContextMetadata.GetRuleParserConfig(),true, strexpression);

                return expression.Compile();
            }
            catch (Exception ex)
            {
                throw new ExpressionParserException("StringRuleExpressionParser", ruleExpression, strexpression, contex.RuleEngineId, ex);
            }
        }

    }
}
