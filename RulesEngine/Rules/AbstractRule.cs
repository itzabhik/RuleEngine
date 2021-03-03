using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using System;

namespace RulesEngine.Rules
{
    abstract class AbstractRule<TEntity> : IRule<TEntity> where TEntity : RuleAwareEntity
    {
        protected RuleEngineContext.RuleEngineContext _context;

       
        public AbstractRule(RuleEngineContext.RuleEngineContext context, string rule, Func<TEntity, bool> rulefunc, string name, string description)
        {
            _context = context;
            Rule = rule;
            Rulefunc = rulefunc;
            RuleName = name;
            RuleDescription = description;
        }

        public string Rule { get; }
        public Func<TEntity, bool> Rulefunc { get; }
        public string RuleName { get; }

        public string RuleDescription { get; }

        public virtual bool Execute(TEntity entity)
        {
            return Rulefunc(entity);
        }
    }
}
