using RulesEngine.RuleEngineContext;
using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace RulesEngine.RuleParser
{
    class PropertyRuleExpressionParser<TEntity> : AbstractCustomPlaceHolderProvider,
        IPropertyRuleExpressionParser<TEntity> where TEntity : RuleAwareEntity
    {

        public Func<TEntity, bool> ParserRule(RuleEngineContext.RuleEngineContext contex, string propertyName, string ruleExpression)
        {
            IPlaceHolderParser<TEntity> placeHolderParser = RuleExpressionParserFactory.CreatePalceHolderParser<TEntity>();
            placeHolderParser.AddPlaceHolders(_placeholders);


            string strexpr = placeHolderParser.ParsePlaceHolder(contex, ruleExpression);


            string strexpression = "SetPropertyValue(" + "\"" + propertyName + "\","
                + strexpr + ")";

           
            try
            {
                var expression = DynamicExpressionParser.ParseLambda<TEntity, bool>
                    (contex.EntityContextMetadata.GetRuleParserConfig(), true, strexpression);
                return expression.Compile();
            }
            catch (Exception ex)
            {
                throw new ExpressionParserException("PropertyRuleExpressionParser", ruleExpression, strexpression, contex.RuleEngineId, ex);
            }

        }
    }
}
