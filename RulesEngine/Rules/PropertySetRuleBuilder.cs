using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.RuleSet;
using System;
using System.Collections.Generic;


namespace RulesEngine.Rules
{
    public class PropertySetRuleBuilder<TEntity>:AbstractRuleBuilder<TEntity> where TEntity : DynamicRuleEntity
    {
       
        private readonly string _rule;
        private readonly IRuleExpressionParser<TEntity> _ruleparser;
        
        private RuleSetBuilder<TEntity> _ruleSetBuilder;
        private readonly string _name;
        private readonly string _description;
        private Func<TEntity, bool> _rulefunc;


        private PropertySetRuleBuilder
            (string entityType, string rule, IRuleExpressionParser<TEntity> ruleparser, RuleSetBuilder<TEntity> ruleSetBuilder,
            string name, string description = "")
            : base(entityType)
        {
            
            this._rule = rule;
            this._ruleparser = ruleparser;
            this._ruleSetBuilder = ruleSetBuilder;
            this._name = name;
            this._description = description;
        }

        private PropertySetRuleBuilder(string entityType, Func<TEntity, bool> rulefunc, RuleSetBuilder<TEntity> ruleSetBuilder, 
            string name, string description = "")
            : base(entityType)
        {
            this._rulefunc = rulefunc;
            this._ruleSetBuilder = ruleSetBuilder;
            this._name = name;
            this._description = description;
        }

        public new PropertySetRuleBuilder<TEntity> SetProperty<TType>(string propertyName, TType value)
        {
            return SetProperty(propertyName, t => value);
        }

        public new PropertySetRuleBuilder<TEntity> SetPropertyExpression<TType>(string propertyName, string valueExpression)
        {
            base.SetPropertyExpression<TType>(propertyName, valueExpression);
            return this;
        }

        public new PropertySetRuleBuilder<TEntity>  SetProperty<TType>(string propertyName, Func<TEntity, TType> setterFunc)
        {
            base.SetProperty(propertyName, setterFunc);
            return this;
        }

        public RuleSetBuilder<TEntity> Attach()
        {
            PropertySetRule<TEntity> proprule = null;
            if(!string.IsNullOrEmpty(_rule))
                proprule = new PropertySetRule<TEntity>(_entityType, _rule, _ruleparser, _properties,_name,_description);
            else
            {
                proprule = new PropertySetRule<TEntity>(_rulefunc, _properties, _name, _description);
            }
            _ruleSetBuilder.AddRule(proprule);
            return _ruleSetBuilder;
        }
        internal static PropertySetRuleBuilder<TEntity> Create(string entityType, string rule, 
            IRuleExpressionParser<TEntity> ruleparser, RuleSetBuilder<TEntity> ruleSetBuilder, string name, string description = "")
        {
            return new PropertySetRuleBuilder<TEntity>(entityType, rule, ruleparser, ruleSetBuilder,name,description);
        }

        internal static PropertySetRuleBuilder<TEntity> Create
            (string entityType, Func<TEntity, bool> rulefunc, RuleSetBuilder<TEntity> ruleSetBuilder,string name,string description="") 
        {
            return new PropertySetRuleBuilder<TEntity>(entityType, rulefunc,  ruleSetBuilder, name, description);
        }
    }
}
