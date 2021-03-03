using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Ruleset;
using System;

namespace RulesEngine.Rules
{
    public class DefaultRuleBuilder<TEntity> : AbstractRuleBuilder<TEntity> 
        where TEntity : RuleAwareEntity
    {
        internal RulesetBuilder<TEntity> _ruleSetBuilder;
        private readonly IPropertyRuleExpressionParser<TEntity> _propertyparser;
        private DefaultRuleBuilder(RuleEngineContext.RuleEngineContext contex, RulesetBuilder<TEntity> ruleSetBuilder,
            IPropertyRuleExpressionParser<TEntity> propertyparser)
            :base(contex)
        {
            _ruleSetBuilder = ruleSetBuilder;
            _propertyparser = propertyparser;
        }
        public new DefaultRuleBuilder<TEntity> SetProperty<TType>(string propertyName, TType value)
        {
            return SetProperty(propertyName, t => value);
        }

        public new DefaultRuleBuilder<TEntity> SetPropertyExpression(string propertyName, string valueExpression)
        {
            base.SetPropertyExpression(propertyName, valueExpression);
            return this;
        }

        public new DefaultRuleBuilder<TEntity> SetProperty<TType>(string propertyName, Func<TEntity, TType> setterFunc)
        {
            base.SetProperty(propertyName, setterFunc);
            return this;
        }

        public DefaultRuleBuilder<TEntity> SetAction(Action<TEntity> action)
        {
            base.SetAction(action);
            return this;
        }

        public IRuleset<TEntity> Compile()
        {
            return _ruleSetBuilder.Compile();
        }

        internal IPropertyRule<TEntity> CompileInternal()
        {
            CompilePropertyExpression(_propertyparser);
            PropertyRule<TEntity> proprule = new PropertyRule<TEntity>(_context, DefaultCriteriaRuleName(),t => true, _properties, DefaultCriteriaRuleName());
            return proprule;
        }

        private string DefaultCriteriaRuleName()
        {
            return String.Format("Default-Ruleset({0})", _ruleSetBuilder._name);
        }

        internal static DefaultRuleBuilder<TEntity> Create(RuleEngineContext.RuleEngineContext contex,
            RulesetBuilder<TEntity> ruleSetBuilder, IPropertyRuleExpressionParser<TEntity> propertyparser)
        {
            return new DefaultRuleBuilder<TEntity>(contex, ruleSetBuilder, propertyparser);
        }

        
    }
}
