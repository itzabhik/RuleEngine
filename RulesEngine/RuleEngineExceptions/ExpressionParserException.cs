using System;


namespace RulesEngine.RuleEngineExceptions
{
    public class ExpressionParserException : Exception
    {
       
        public ExpressionParserException(string type,string inputExpression, string ruleEngineId, Exception innerException)
            : base(string.Format("Input Expression: '{0}' For Type: '{1}' -> Parsing failed in Rule Engine {2}"
                , inputExpression, type, ruleEngineId), innerException)
        {

        }

        public ExpressionParserException(string type, string inputExpresiion, string placeHolderParsedException, 
            string ruleEngineId, Exception innerException) : base(string.Format("Input Expression: '{0}' " +
                "PacheHolder Parsed: '{1}'" +
                " For Type: '{2}' -> Parsing failed in Rule Engine {3}"
                , inputExpresiion, placeHolderParsedException, type, ruleEngineId), innerException)

        {
          
        }
    }
}
