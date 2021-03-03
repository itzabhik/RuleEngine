using RulesEngine.RuleEngineMetadata;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Rules;
using RulesEngine.Ruleset;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace RulesEngine.GroupRuleSet
{
    class DynamicGroupRuleSet<TEntity> : AbstractGroupRuleSet<TEntity> where TEntity : RuleAwareEntity
    {
        private string _parsedExpression;
        public DynamicGroupRuleSet(IEnumerable<IRuleset<TEntity>> ruleSets, EntryCriteriaRule<TEntity> entryPoint,
            IEnumerable<AggregatePropertyMetadata<TEntity>> aggregateProps,
             RuleEngineContext.RuleEngineContext context,
            string groupKey, string name, string description, IGroupKeyExpressionParser<TEntity> _groupKeyExpressionParser) 
            : base(ruleSets, entryPoint, aggregateProps, context, groupKey, name, description)
        {
            _parsedExpression = _groupKeyExpressionParser.ParseGroupString(context, GroupKey);
        }
        protected override IQueryable CreateGroupQueryable(IEnumerable<TEntity> entity)
        {
            
            return entity.AsQueryable()
                .Where(e => _entryPoint.Rulefunc(e))
                .GroupBy(_parsedExpression);

        }

        protected override IQueryable<TEntity> GetEntity(object item)
        {
            return ((IGrouping<DynamicClass, TEntity>)item).AsQueryable();
        }
    }
}
