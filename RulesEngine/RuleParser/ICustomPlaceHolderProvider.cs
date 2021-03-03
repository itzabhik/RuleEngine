using System.Collections.Generic;

namespace RulesEngine.RuleParser
{
    internal interface ICustomPlaceHolderProvider
    {
        void AddPlaceHolder(string placeHolderName, string placeHolderExpression);
        void AddPlaceHolders(Dictionary<string,string> placeholders);
    }
}