

using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;

namespace RulesEngine.RuleParser
{
 
    class GroupKeyExpressionParser<TEntity> : AbstractCustomPlaceHolderProvider, 
        IGroupKeyExpressionParser<TEntity> where TEntity : RuleAwareEntity
    {
        public Delegate ParseGroupString(RuleEngineContext.RuleEngineContext contex, string placeHolderExpression)
        {
            IPlaceHolderParser<TEntity> placeHolderParser = RuleExpressionParserFactory.CreatePalceHolderParser<TEntity>();
            placeHolderParser.AddPlaceHolders(_placeholders);

            string[] expressions = placeHolderExpression.Split(',');
            int propertyCounter = 1;
            PlaceHolderTextParser place = new PlaceHolderTextParser(placeHolderExpression);
            List<string> lstExpressions = new List<string>();

            foreach (var expr in expressions)
            {
                try
                {
                    string parsedExpression = placeHolderParser.ParsePlaceHolder(contex, string.Format("({0}) as Prop{1}", expr,
                    propertyCounter));
                    lstExpressions.Add(parsedExpression);
                    propertyCounter = propertyCounter + 1;
                }
                catch (System.Exception ex)
                {
                    throw new ExpressionParserException("GroupKeyExpressionParser",placeHolderExpression, contex.RuleEngineId, ex);
                }
                
            }

            var inputExpression = string.Format("new ({0})", string.Join(",", lstExpressions));

            try
            {
                var lamdaexpr = DynamicExpressionParser
                    .ParseLambda(contex.EntityContextMetadata.GetRuleParserConfig(), true, typeof(TEntity), null, inputExpression);
              
                return lamdaexpr.Compile();
            }
            catch (Exception ex)
            {
                throw new ExpressionParserException("GroupKeyExpressionParser", placeHolderExpression, inputExpression, contex.RuleEngineId, ex);
            }
        }
    }
}
