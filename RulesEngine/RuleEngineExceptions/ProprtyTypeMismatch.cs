using System;


namespace RulesEngine.RuleEngineExceptions
{
    public class ProprtyTypeMismatch: Exception
    {
        public ProprtyTypeMismatch(string property, Type type, string entityType)
            :base(string.Format
                 ("The property {0} is not of type {1} for Entity Type: {2} " , 
                property, type, entityType))
        {

        }
    }
}
