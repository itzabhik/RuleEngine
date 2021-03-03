

using System;

namespace RulesEngine.RuleEngineContext
{
    public interface IRuleEngineContext
    {
        string RootEntityType { get; }
        string RuleEngineId { get; }
        void AssociateDynamicType<TEntity>(string entityType, string alias);
        void AssociateDynamicType(string entityType, Type type, string alias);
        
        void AssociateChildDynamicType<TEnity, TChildEntity>(string parententityType, string childEntityType, string parentProperty);
       
      
    }
}
