using System;
using RulesEngine.GroupRuleSet;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Ruleset;

namespace RulesEngine.Rules
{
    public class GroupChildRulesetDefaultRuleBuilder<TEntity, TKey> where TEntity : RuleAwareEntity
    {
        private readonly RulesetBuilder<TEntity> _ruleSetBuilder;
        private GroupRuleSetBuilder<TEntity, TKey> _groupRuleSetBuilder;
        private DefaultRuleBuilder<TEntity> _defaultProp;
        private GroupChildRulesetDefaultRuleBuilder(RuleEngineContext.RuleEngineContext context, RulesetBuilder<TEntity> ruleSetBuilder,
            GroupRuleSetBuilder<TEntity, TKey> groupRuleSetBuilder, IPropertyRuleExpressionParser<TEntity> propertyparser)
        {
            this._ruleSetBuilder = ruleSetBuilder;
            this._groupRuleSetBuilder = groupRuleSetBuilder;

            _defaultProp = ruleSetBuilder.WithDefaultRule();
           
        }

        public new GroupChildRulesetDefaultRuleBuilder<TEntity, TKey> SetProperty<TType>(string propertyName, TType value)
        {
            return SetProperty(propertyName, t => value);
        }

        public new GroupChildRulesetDefaultRuleBuilder<TEntity, TKey> SetPropertyExpression(string propertyName, string valueExpression)
        {
            _defaultProp.SetPropertyExpression(propertyName, valueExpression);
            return this;
        }

        public new GroupChildRulesetDefaultRuleBuilder<TEntity, TKey> SetProperty<TType>(string propertyName, Func<TEntity, TType> setterFunc)
        {
            _defaultProp.SetProperty(propertyName, setterFunc);
            return this;
        }

        public new GroupChildRulesetDefaultRuleBuilder<TEntity, TKey> SetAction(Action<TEntity> action)
        {
            _defaultProp.SetAction(action);
            return this;
        }

        public GroupRuleSetBuilder<TEntity, TKey> Attach()
        {
           
            _groupRuleSetBuilder.AddRuleSetBuillder(_ruleSetBuilder);
            
            return _groupRuleSetBuilder;
        }

        internal static GroupChildRulesetDefaultRuleBuilder<TEntity, TKey> Create(RuleEngineContext.RuleEngineContext context, RulesetBuilder<TEntity> ruleSetBuilder,
            GroupRuleSetBuilder<TEntity, TKey> groupRuleSetBuilder, IPropertyRuleExpressionParser<TEntity> propertyparser) 
        {
            return new GroupChildRulesetDefaultRuleBuilder<TEntity, TKey>(context, ruleSetBuilder, groupRuleSetBuilder, propertyparser);
        }
    }
}