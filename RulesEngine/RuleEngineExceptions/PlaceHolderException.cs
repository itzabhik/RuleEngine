using System;


namespace RulesEngine.RuleEngineExceptions
{
    public class PlaceHolderException:Exception
    {
        public PlaceHolderException(string placeholder, string rulesetName, string ruleEngineId)
        : base(String.Format(string.Format("Place holder {0} exists for Ruleset{1} in Rule Engine {2}", placeholder, rulesetName, ruleEngineId)))
        {


        }
        public PlaceHolderException(string placeholder,string property, string entityType,  string ruleEngineId)
        : base(string.Format("Placeholder '{0}' is of the same name with Property '{1}' for EntityType '{2}' in  in Rule Engine {3} "
            , placeholder, property, entityType, ruleEngineId))
        {

        }
    }
}
