using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleModel;
using System.Collections.Generic;

namespace RulesEngine.RuleParser
{
    class PlaceHolderParser<TEntity> : AbstractCustomPlaceHolderProvider,
        IPlaceHolderParser<TEntity> where TEntity : RuleAwareEntity
    {
        public string ParsePlaceHolder(RuleEngineContext.RuleEngineContext context, string placeHolderExpression)
        {
            try
            {
                return StringRulePLaceHolderParser.ParseRulePlaceHolder<TEntity>(context, placeHolderExpression, _placeholders);
            }
            catch (System.Exception ex)
            {
                throw new ExpressionParserException("ParsePlaceHolder", placeHolderExpression, context.RuleEngineId, ex);
            }
        }
    }
}
