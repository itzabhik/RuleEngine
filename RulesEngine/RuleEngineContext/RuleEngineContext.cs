
using System;

namespace RulesEngine.RuleEngineContext
{
    public class RuleEngineContext : IRuleEngineContext
    {
        internal RuleEntityContextMetadata EntityContextMetadata { get; }

      
        public string RootEntityType { get; internal set; }
        public string RuleEngineId { get; }

        public RuleEngineContext(string ruleEngineId)
        {
            EntityContextMetadata = new RuleEntityContextMetadata();
           
            RuleEngineId = ruleEngineId;
        }

        public void AssociateDynamicType<TEntity>(string entityType, string alias)
        {
            AssociateDynamicType(entityType, typeof(TEntity), alias);


        }
        public void AssociateDynamicType(string entityType, Type type, string alias)
        {
            EntityContextMetadata.AssociateDynamicType(entityType, type, alias);
        }
       

        public void AssociateChildDynamicType<TEnity, TChildEntity>(string parententityType, string childEntityType, string parentProperty)
        {
            EntityContextMetadata.AssociateChildDynamicType<TEnity, TChildEntity>
                (parententityType, childEntityType, parentProperty);
        }




    }
}
