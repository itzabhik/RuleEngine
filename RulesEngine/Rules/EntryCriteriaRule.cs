
using System;
using RulesEngine.RuleEngineContext;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;

namespace RulesEngine.Rules
{
    class EntryCriteriaRule<TEntity> : AbstractRule<TEntity> where TEntity : RuleAwareEntity
    {
        public static EntryCriteriaRule<TEntity> TrueRule => new EntryCriteriaRule<TEntity>(T => true,"DefaultTrueRule");
        public static EntryCriteriaRule<TEntity> FalseRule => new EntryCriteriaRule<TEntity>(T => false, "FalseRule");

        private readonly bool _collectDiagnostic;
        public EntryCriteriaRule(RuleEngineContext.RuleEngineContext context, string rule, Func<TEntity, bool> rulefunc, string name, string description="", bool collectDiagnostic = true) 
            : base(context, rule, rulefunc, name,description)
        {
            this._collectDiagnostic = collectDiagnostic;
        }

        private EntryCriteriaRule(Func<TEntity, bool> rulefunc,string name)
            : base(RuleEngineContextHolder.GetContext(""), name, rulefunc, name, "")
        {

        }

      
        public override bool Execute(TEntity entity)
        {
            bool result= base.Execute(entity);
            if (!result)
                return result;
            if (_collectDiagnostic)
                entity.EntryCriteriaDiagnostic(Rule);

            return result;
        }

       
    }
}
