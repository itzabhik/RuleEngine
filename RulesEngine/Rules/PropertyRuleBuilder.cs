using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Ruleset;
using System;
using System.Collections.Generic;


namespace RulesEngine.Rules
{
    public class PropertyRuleBuilder<TEntity>:AbstractRuleBuilder<TEntity> where TEntity : RuleAwareEntity
    {
       
        private readonly string _rule;
        private readonly IRuleExpressionParser<TEntity> _ruleparser;
        private readonly IPropertyRuleExpressionParser<TEntity> _propertyparser;
        private RulesetBuilder<TEntity> _ruleSetBuilder;
        private readonly string _name;
        private readonly string _description;
        private Func<TEntity, bool> _rulefunc;
        private PropertyRuleBuilder
            (RuleEngineContext.RuleEngineContext context, string rule, 
            IRuleExpressionParser<TEntity> ruleparser, 
            IPropertyRuleExpressionParser<TEntity> propertyparser,
            RulesetBuilder<TEntity> ruleSetBuilder,
            string name, string description = "")
            : base(context)
        {
            
            this._rule = rule;
            this._ruleparser = ruleparser;
            this._propertyparser = propertyparser;
            this._ruleSetBuilder = ruleSetBuilder;
            this._name = name;
            this._description = description;
        }

        private PropertyRuleBuilder(RuleEngineContext.RuleEngineContext context, Func<TEntity, 
            bool> rulefunc,
            IPropertyRuleExpressionParser<TEntity> propertyparser,
            RulesetBuilder<TEntity> ruleSetBuilder, 
            string name, string description = "")
            : base(context)
        {
            this._rulefunc = rulefunc;
            _propertyparser = propertyparser;
            this._ruleSetBuilder = ruleSetBuilder;
            this._name = name;
            this._description = description;
        }

        public new PropertyRuleBuilder<TEntity> SetProperty<TType>(string propertyName, TType value)
        {
            return SetProperty(propertyName, t => value);
        }

        public new PropertyRuleBuilder<TEntity> SetPropertyExpression(string propertyName, string valueExpression)
        {
            base.SetPropertyExpression(propertyName, valueExpression);
            return this;
        }

        public new PropertyRuleBuilder<TEntity>  SetProperty<TType>(string propertyName, Func<TEntity, TType> setterFunc)
        {
            base.SetProperty(propertyName, setterFunc);
            return this;
        }

        public new PropertyRuleBuilder<TEntity> SetAction(Action<TEntity> action)
        {
            base.SetAction(action);
            return this;
        }

        public RulesetBuilder<TEntity> Attach()
        {
            return _ruleSetBuilder;
        }

        internal IPropertyRule<TEntity> Compile()
        {
            CompilePropertyExpression(_propertyparser);

            PropertyRule<TEntity> proprule = null;
            if (!string.IsNullOrEmpty(_rule))
                proprule = new PropertyRule<TEntity>(_context, _rule, _ruleparser.ParserRule(_context, _rule), _properties, _name, _description);
            else
            {
                proprule = new PropertyRule<TEntity>(_context, _name,_rulefunc, _properties, _name, _description);
            }
            return proprule;
        }
        internal static PropertyRuleBuilder<TEntity> Create(RuleEngineContext.RuleEngineContext context, string rule, 
            IRuleExpressionParser<TEntity> ruleparser, IPropertyRuleExpressionParser<TEntity> propertyparser,
            RulesetBuilder<TEntity> ruleSetBuilder, 
            string name, 
            string description = "")
        {
            return new PropertyRuleBuilder<TEntity>(context, rule, ruleparser, propertyparser, ruleSetBuilder,name,description);
        }

        internal static PropertyRuleBuilder<TEntity> Create
            (RuleEngineContext.RuleEngineContext context, Func<TEntity, bool> rulefunc, IPropertyRuleExpressionParser<TEntity> propertyparser,
            RulesetBuilder<TEntity> ruleSetBuilder,string name,string description="") 
        {
            return new PropertyRuleBuilder<TEntity>(context, rulefunc, propertyparser,  ruleSetBuilder, name, description);
        }
    }
}
