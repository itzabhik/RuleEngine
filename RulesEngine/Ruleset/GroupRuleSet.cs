using RulesEngine.RuleExecutionRule;
using RulesEngine.RuleModel;
using RulesEngine.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace RulesEngine.RuleSet
{
    class GroupRuleSet<TEntity> : IGroupRuleSet<TEntity>
    {
        private readonly EntryCriteriaRule<TEntity> entryPoint;
        private readonly RuleExecutionRuleEnum ruleExecutionRule;
        private readonly IEnumerable<IRuleSet<TEntity>> _ruleSets = new List<IRuleSet<TEntity>>();
        private readonly IEnumerable<AggregatePropertyMetadata> _aggregateProps;
        private List<IRuleSet<TEntity>> _executedRuleSet;

        internal GroupRuleSet(IEnumerable<IRuleSet<TEntity>> ruleSets, EntryCriteriaRule<TEntity> entryPoint,
            RuleExecutionRuleEnum ruleExecutionRule,
            IEnumerable<AggregatePropertyMetadata> aggregateProps,
            string filterExpression,
            string groupExpression,
            string name, string description)
        {
            this.entryPoint = entryPoint;
            this.ruleExecutionRule = ruleExecutionRule;
            Name = name;
            Description = description;
            GroupExpression = groupExpression;
            _aggregateProps = aggregateProps;
            _ruleSets= ruleSets;
            _executedRuleSet = new List<IRuleSet<TEntity>>();
        }

        public string GroupExpression { get; }

        public string Name { get; }

        public string Description { get; }

        public void Execute(IEnumerable<TEntity> entity)
        {
            if (entity == null)
                throw new ArgumentException("Entity cannot be null", "entity");

            var groupEnumerable = entity.AsQueryable()
                .Where(e=>entryPoint.Rulefunc(e))
                .GroupBy(GroupExpression);

            List<Grouping<TEntity>> groups = new List<Grouping<TEntity>>();

            foreach (var item in groupEnumerable)
            {
                var val = (IGrouping<DynamicClass, TEntity>)item;
                var grouping = new Grouping<TEntity>(val.AsQueryable(), _aggregateProps);
                grouping.CreateAggregateProperties(val);
            }
            foreach (var item in groups)
            {
                foreach (var ruleEntity in item.Entity)
                {
                    item.CreateAggregateProperties(ruleEntity);

                    foreach (var ruleSet in _ruleSets)
                    {
                        ruleSet.Execute(ruleEntity);
                        if(ruleSet.HasSuccessRule())
                        {
                            _executedRuleSet.Add(ruleSet);
                        }
                    }
                }
                
            }

        }

        public IEnumerable<IRuleSet<TEntity>> ExecutedRuleSet
        {
            get
            {
                return _executedRuleSet;
            }
        }


        public bool HasSuccessRuleset()
        {
            throw new NotImplementedException();
        }
    }
}
