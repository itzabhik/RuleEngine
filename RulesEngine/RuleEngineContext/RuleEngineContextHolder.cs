using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;

namespace RulesEngine.RuleEngineContext
{
    public static class RuleEngineContextHolder
    {
        private static Dictionary<string, IRuleEngineContext> _contexts
         = new Dictionary<string, IRuleEngineContext>();

        public static IRuleEngineContext GenerateContext(string ruleEngineId, string entityType)
        {
            if(_contexts.ContainsKey(ruleEngineId))
            {
                _contexts.Remove(ruleEngineId);
            }

            RuleEngineContext context = CreateEmptyContext(ruleEngineId);
            context.RootEntityType = entityType;
            _contexts.Add(ruleEngineId, context);
            return context;
        }

        public static IRuleEngineContext GenerateContext( string entityType)
        {
            return GenerateContext(Guid.NewGuid().ToString(), entityType);
        }

        internal static RuleEngineContext GetContext(string ruleEngineId)
        {
            try
            {
                IRuleEngineContext context;
                _contexts.TryGetValue(ruleEngineId, out context);
                if (context == null)
                    return CreateEmptyContext(ruleEngineId);

                return context as RuleEngineContext;
            }
            catch (System.Exception ex)
            {

                throw new System.Exception("Rule Engine Context cannot be Null",ex);
            }
        }

        private static RuleEngineContext CreateEmptyContext(string ruleEngineId)
        {
            return new RuleEngineContext(ruleEngineId) { RootEntityType =""};
        }
    }
}
