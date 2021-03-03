using RulesEngine.RuleEngineContext;
using RulesEngine.RuleEngineMetadata;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Rules;
using RulesEngine.Ruleset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace RulesEngine.GroupRuleSet
{
    abstract class AbstractGroupRuleSet<TEntity, TKey> : IGroupRuleSet<TEntity> where TEntity : RuleAwareEntity
    {
        protected readonly EntryCriteriaRule<TEntity> _entryPoint;
        protected readonly IEnumerable<IRuleset<TEntity>> _ruleSets = new List<IRuleset<TEntity>>();
        protected readonly IEnumerable<AggregatePropertyMetadata<TEntity>> _aggregateProps;
        protected readonly RuleEngineContext.RuleEngineContext _context;

        protected List<IRuleset<TEntity>> _executedRuleSet;
       

        internal AbstractGroupRuleSet(IEnumerable<IRuleset<TEntity>> ruleSets, EntryCriteriaRule<TEntity> entryPoint,
            IEnumerable<AggregatePropertyMetadata<TEntity>> aggregateProps,
           RuleEngineContext.RuleEngineContext context,
            string groupKey,
            string name, string description)
        {
            this._entryPoint = entryPoint;
            
            Name = name;
            Description = description;
            GroupKey = groupKey;
            _aggregateProps = aggregateProps;
            this._context = context;
            _ruleSets = ruleSets;
            _executedRuleSet = new List<IRuleset<TEntity>>();

           
        }

        public string GroupKey { get; }

        public string Name { get; }

        public string Description { get; }

        public void Execute(IEnumerable<TEntity> entity)
        {
            if (entity == null)
                throw new ArgumentException("Entity cannot be null", "entity");
             var groupEnumerable = CreateGroupEnumerable(entity);
            SyncGroupEnumerableExecution(groupEnumerable);
           
        }

        public void ExecuteAsync(IEnumerable<TEntity> entity)
        {
            if (entity == null)
                throw new ArgumentException("Entity cannot be null", "entity");
            var groupEnumerable = CreateGroupEnumerable(entity);
            AyncGroupEnemerableExecution(groupEnumerable);
        }


        private void SyncGroupEnumerableExecution(IEnumerable<IGrouping<TKey, TEntity>> groupEnumerable)
        {
            foreach (var item in groupEnumerable)
            {
                ExecuteGroup(item, _executedRuleSet);
            }
        }

        private void AyncGroupEnemerableExecution(IEnumerable<IGrouping<TKey, TEntity>> groupEnumerable)
        {
            object sync = new Object();
            Parallel.ForEach(groupEnumerable, () => { return new List<IRuleset<TEntity>>(); },
               (item, state, localList) =>
               {
                   ExecuteGroup(item, localList);
                   return localList;
               }, ruleSet => { lock (sync) _executedRuleSet.AddRange(ruleSet); });
        }
      
        abstract protected IEnumerable<IGrouping<TKey, TEntity>> CreateGroupEnumerable(IEnumerable<TEntity> entity);
      
        public IEnumerable<IRuleset<TEntity>> ExecutedRuleSet
        {
            get
            {
                return _executedRuleSet;
            }
        }
        public bool HasSuccessRuleset()
        {
            return _executedRuleSet != null && _executedRuleSet.Count() > 0;
        }

        private void ExecuteGroup(IGrouping<TKey,TEntity> item, List<IRuleset<TEntity>> localList)
        {
           var grouping = new Grouping<TEntity>(_context, item, _aggregateProps);

            ExecuteGroupChildRuleSet(grouping, localList);
        }
        private void ExecuteGroupChildRuleSet(Grouping<TEntity> item, List<IRuleset<TEntity>> executedRuleset)
        {
            foreach (var ruleEntity in item.Entity)
            {
                item.AttachAggregatePropertyToEntity(ruleEntity);

                foreach (var ruleSet in _ruleSets)
                {
                    ruleSet.Execute(ruleEntity);
                    if (ruleSet.HasSuccessRule())
                    {
                        executedRuleset.Add(ruleSet);
                    }
                }

            }
        }

       
    }
}
