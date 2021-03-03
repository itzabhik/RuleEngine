using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleModel;
using System;
using System.Linq.Dynamic.Core;

namespace RulesEngine.RuleParser
{
    class AggregateFunctionParser<TEntity, TType> : AbstractCustomPlaceHolderProvider, IAggregateFunctionParser<TEntity, TType>
         where TEntity : RuleAwareEntity
    {
        public Func<TEntity, TType> ParserRule(RuleEngineContext.RuleEngineContext contex, string ruleExpression)
        {
            IPlaceHolderParser<TEntity> placeHolderParser = RuleExpressionParserFactory.CreatePalceHolderParser<TEntity>();
            placeHolderParser.AddPlaceHolders(_placeholders);

            var strexpression = placeHolderParser.ParsePlaceHolder(contex, ruleExpression);

            try
            {
                var exprep = DynamicExpressionParser
                  .ParseLambda<TEntity, TType>(contex.EntityContextMetadata.GetRuleParserConfig(),
                  true, strexpression);

                return exprep.Compile();
            }
            catch (Exception ex)
            {

                throw new ExpressionParserException("AggregateFunctionParser", ruleExpression, strexpression, contex.RuleEngineId, ex);
            }

        }
    }
}
