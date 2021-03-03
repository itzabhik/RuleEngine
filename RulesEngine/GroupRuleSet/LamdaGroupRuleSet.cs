using System;
using System.Collections.Generic;
using System.Linq;
using RulesEngine.RuleEngineContext;
using RulesEngine.RuleEngineMetadata;
using RulesEngine.RuleModel;
using RulesEngine.Rules;
using RulesEngine.Ruleset;

namespace RulesEngine.GroupRuleSet
{
    internal class LamdaGroupRuleSet<TEntity,TKey> : AbstractGroupRuleSet<TEntity,TKey> where TEntity : RuleAwareEntity
    {
        private readonly Func<TEntity, TKey> _keySelector;

        public LamdaGroupRuleSet
            (IEnumerable<IRuleset<TEntity>> ruleSets, EntryCriteriaRule<TEntity> entryPoint, 
            IEnumerable<AggregatePropertyMetadata<TEntity>> aggregateProps, RuleEngineContext.RuleEngineContext context, 
            Func<TEntity, TKey> keySelector, string name, string description) 
            : base(ruleSets, entryPoint, aggregateProps, context, string.Empty, name, description)
        {
            _keySelector = keySelector;
        }
        protected override IEnumerable<IGrouping<TKey, TEntity>> CreateGroupEnumerable(IEnumerable<TEntity> entity)
        {
            return entity
                     .Where(e => _entryPoint.Rulefunc(e))
                     .GroupBy(_keySelector);

        }
    }
}
