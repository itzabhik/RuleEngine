

using System.Collections.Generic;

namespace RulesEngine.RuleParser
{
    class AbstractCustomPlaceHolderProvider: ICustomPlaceHolderProvider
    {
        protected Dictionary<string, string> _placeholders = new Dictionary<string, string>();
        public void AddPlaceHolder(string placeHolderName, string placeHolderExpression)
        {
            _placeholders.Add(placeHolderName, placeHolderExpression);
        }

        public void AddPlaceHolders(Dictionary<string, string> placeholders)
        {
            foreach (var item in placeholders)
            {
                _placeholders.Add(item.Key, item.Value);
            }
        }
    }
}
