using RulesEngine.RuleEngineMetadata;
using System;

namespace RulesEngine.RuleModel
{
    partial class RuleAwareEntity
    {
        public static void CreateDynamicPropertyForType<TEnity, TType>(string entityType, string name)
        {
            RuleAwareEntityPropertyInfo.CreateDynamicProperty<TEnity, TType>(entityType, name);
        }
        public static void CreateDynamicPropertyForType<TEnity, TType>(string entityType, string name, TType defaultValue)
        {
            RuleAwareEntityPropertyInfo.CreateDynamicProperty<TEnity, TType>(entityType, name, defaultValue);
        }

        public static void CreateDynamicPropertyForType<TEnity>(string entityType, string name, Type type)
        {
            RuleAwareEntityPropertyInfo.CreateDynamicProperty<TEnity>(entityType, name, type);
        }

        public static void CreateDynamicPropertyForType<TEnity>(string entityType, string name, Type type, object defaultValue)
        {
            RuleAwareEntityPropertyInfo.CreateDynamicProperty<TEnity>(entityType, name, type, defaultValue);
        }
    }
}
