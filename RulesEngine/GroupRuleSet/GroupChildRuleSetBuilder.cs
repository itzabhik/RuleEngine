using RulesEngine.RuleExecutionRule;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Rules;
using RulesEngine.Ruleset;
using System;
using System.Collections.Generic;

namespace RulesEngine.GroupRuleSet
{
    public class GroupChildRuleSetBuilder<TEntity,TKey> where TEntity : RuleAwareEntity
    {
       
        private readonly GroupRuleSetBuilder<TEntity, TKey> _groupRuleSetBuilder;
       
        private RulesetBuilder<TEntity> _ruleSetBuilder;
        private readonly RuleEngineContext.RuleEngineContext _context;
        private IRuleExpressionParser<TEntity> _ruleparser;
        private readonly IPropertyRuleExpressionParser<TEntity> _propertyparser;
       

        private GroupChildRuleSetBuilder(RuleEngineContext.RuleEngineContext context, string name, string description, 
            IRuleExpressionParser<TEntity> ruleparser,
            IPropertyRuleExpressionParser<TEntity> propertyparser,
            GroupRuleSetBuilder<TEntity, TKey> groupRuleSetBuilder )
        {
            this._context = context;
            _ruleparser = ruleparser;
    
            this._groupRuleSetBuilder = groupRuleSetBuilder;
            this._propertyparser = propertyparser;
            _ruleSetBuilder = RulesetBuilder<TEntity>.Create(context)
                .WithName(name)
                .WithDescription(description)
                .SetPropertyRuleParser(propertyparser)
                .SetRuleParser(ruleparser);

        }

        public GroupChildRuleSetBuilder<TEntity, TKey> WithHaving(string entryCriteria)
        {
            _ruleSetBuilder.WithEntryCriteria(entryCriteria);
            return this;
        }
        public GroupChildRuleSetBuilder<TEntity, TKey> WithHaving(Func<TEntity, bool> rulefunc)
        {
            _ruleSetBuilder.WithEntryCriteria(rulefunc);
            return this;
        }

        public GroupChildRuleSetBuilder<TEntity, TKey> WithJobExecutionRule(RuleExecutionRuleEnum rulesExecutionRule)
        {
            _ruleSetBuilder.WithJobExecutionRule(rulesExecutionRule);
            return this;
        }

        public GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> WithRule(string rule)
        {
            return WithRule(rule, Guid.NewGuid().ToString(), string.Empty);
        }
        public GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> WithRule(string rule, string name, string description = "")
        {
           return GroupChildRulesetPropertyRuleBuilder<TEntity, TKey>
                .Create(_context, rule,_ruleparser, name, description, _ruleSetBuilder, this,_propertyparser);

        }
        public GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> WithRule(Func<TEntity, bool> rule)
        {
            return WithRule(rule, Guid.NewGuid().ToString(), string.Empty);
        }
        public GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> WithRule(Func<TEntity, bool> rule, string name, string description = "")
        {
            var propBuider = GroupChildRulesetPropertyRuleBuilder<TEntity, TKey>
                .Create(_context, rule, name, description, _ruleSetBuilder, this,_propertyparser);
            return propBuider;
        }
        public GroupChildRulesetDefaultRuleBuilder<TEntity, TKey> DefaultRule()
        {
            var defaultRuleBuilder = GroupChildRulesetDefaultRuleBuilder<TEntity, TKey>
                .Create(_context, _ruleSetBuilder, _groupRuleSetBuilder, _propertyparser);
            return defaultRuleBuilder;
        }

        public GroupRuleSetBuilder<TEntity, TKey> Attach()
        {
            _groupRuleSetBuilder.AddRuleSetBuillder(_ruleSetBuilder);
            return _groupRuleSetBuilder;
        }

        internal static GroupChildRuleSetBuilder<TEntity, TKey> Create(RuleEngineContext.RuleEngineContext context, string name,string description,
            IRuleExpressionParser<TEntity> ruleparser,IPropertyRuleExpressionParser<TEntity> propertyparser,
        GroupRuleSetBuilder<TEntity, TKey> groupRuleSetBuilder)
        {
            return new GroupChildRuleSetBuilder<TEntity, TKey>(context, name, description, ruleparser, propertyparser, groupRuleSetBuilder);
        }

       
    }
}
