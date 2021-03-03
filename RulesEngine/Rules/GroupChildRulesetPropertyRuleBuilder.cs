using RulesEngine.GroupRuleSet;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Ruleset;
using System;

namespace RulesEngine.Rules
{
    public class GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> where TEntity : RuleAwareEntity
    {
               
        private GroupChildRuleSetBuilder<TEntity, TKey> _groupChildRuleSetBuilder;
        PropertyRuleBuilder<TEntity> _propertyBuilder;

        private GroupChildRulesetPropertyRuleBuilder(RuleEngineContext.RuleEngineContext context, Func<TEntity, bool> rule1, string name, 
            string description, RulesetBuilder<TEntity> ruleSetBuilder, GroupChildRuleSetBuilder<TEntity, TKey> groupChildRuleSetBuilder, 
            IPropertyRuleExpressionParser<TEntity> propertyparser)
        {
                 
            this._groupChildRuleSetBuilder = groupChildRuleSetBuilder;
            _propertyBuilder = ruleSetBuilder.WithRule(rule1, name, description);
        }

        private GroupChildRulesetPropertyRuleBuilder(RuleEngineContext.RuleEngineContext context, string rule,
             IRuleExpressionParser<TEntity> ruleparser,
            string name, string description,
            RulesetBuilder<TEntity> ruleSetBuilder, GroupChildRuleSetBuilder<TEntity, TKey> groupChildRuleSetBuilder, 
            IPropertyRuleExpressionParser<TEntity> propertyparser)
        {
         
            
            this._groupChildRuleSetBuilder = groupChildRuleSetBuilder;
            _propertyBuilder = ruleSetBuilder.WithRule(rule, name, description);
        }

        public new GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> SetProperty<TType>(string propertyName, TType value)
        {
            
            return SetProperty(propertyName, t => value);
        }

        public new GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> SetPropertyExpression(string propertyName, string valueExpression)
        {
            _propertyBuilder.SetPropertyExpression(propertyName, valueExpression);
            return this;
        }

        public new GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> SetProperty<TType>(string propertyName, Func<TEntity, TType> setterFunc)
        {
            _propertyBuilder.SetProperty(propertyName, setterFunc);
            return this;
        }

        public new GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> SetAction(Action<TEntity> action)
        {
            _propertyBuilder.SetAction(action);
            return this;
        }

        public GroupChildRuleSetBuilder<TEntity, TKey> Attach()
        {
            _propertyBuilder.Attach();
           
            return _groupChildRuleSetBuilder;
        }
        internal static GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> Create
            (RuleEngineContext.RuleEngineContext context, Func<TEntity, bool> rule, string name, string description, RulesetBuilder<TEntity> ruleSetBuilder
            , GroupChildRuleSetBuilder<TEntity, TKey> groupChildRuleSetBuilder, IPropertyRuleExpressionParser<TEntity> propertyparser) 
        {
            return new GroupChildRulesetPropertyRuleBuilder<TEntity, TKey>
               (context, rule, name, description, ruleSetBuilder, groupChildRuleSetBuilder, propertyparser);
        }

        internal static GroupChildRulesetPropertyRuleBuilder<TEntity, TKey> Create
            (RuleEngineContext.RuleEngineContext context, string rule, IRuleExpressionParser<TEntity> ruleparser, 
            string name, string description,
            RulesetBuilder<TEntity> ruleSetBuilder, 
            GroupChildRuleSetBuilder<TEntity, TKey> groupChildRuleSetBuilder, IPropertyRuleExpressionParser<TEntity> propertyparser) 
           
        {
            return new GroupChildRulesetPropertyRuleBuilder<TEntity, TKey>
                (context, rule, ruleparser, name, description, ruleSetBuilder, groupChildRuleSetBuilder, propertyparser);
        }
    }
}
