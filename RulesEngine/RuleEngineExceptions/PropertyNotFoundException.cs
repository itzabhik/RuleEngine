using System;


namespace RulesEngine.RuleEngineExceptions
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(string property, string entityType)
            :base(string.Format("Property {0} missing in Entity Type {1}", property, entityType))
        {

        }
    }
}
