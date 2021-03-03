using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using System;
using System.Collections.Generic;

namespace RulesEngine.Rules
{
    public abstract class AbstractRuleBuilder<TEntity> where TEntity : RuleAwareEntity
    {
        internal Dictionary<PropertyRule<TEntity>.PropertyHolder, Func<TEntity, bool>> _properties;
        private List<TemporarypropertyHolder> _tempPrpertyHolder;
        internal readonly RuleEngineContext.RuleEngineContext _context;
        private static string actionpropertyName = "[[Action]]";
        protected AbstractRuleBuilder(RuleEngineContext.RuleEngineContext context)
        {
            _properties = new Dictionary<PropertyRule<TEntity>.PropertyHolder, Func<TEntity, bool>>();
            _tempPrpertyHolder = new List<TemporarypropertyHolder>();
            _context = context;


        }
        public void SetProperty<TType>(string propertyName, TType value)
        {
             SetProperty(propertyName, t => value);
        }
        public void SetPropertyExpression(string propertyName, string valueExpression)
        {
            TemporarypropertyHolder holder = new TemporarypropertyHolder();
            holder.propertyName = propertyName;
            holder.propertyexpression = valueExpression;


            _tempPrpertyHolder.Add(holder);
        }
        public void SetProperty<TType>(string propertyName, Func<TEntity, TType> setterFunc)
        {
            TemporarypropertyHolder holder = new TemporarypropertyHolder();
            holder.propertyName = propertyName;
            holder.propertyFunction = t => t.SetPropertyValue(propertyName, setterFunc(t));
           _tempPrpertyHolder.Add(holder);

        }

        internal void SetAction(Action<TEntity> action) 
        {
            Func<TEntity, bool> actionProp = t =>
              {
                  action(t);
                  return true;
              };
            TemporarypropertyHolder holder = new TemporarypropertyHolder();
            holder.propertyName = actionpropertyName;
            holder.propertyFunction = actionProp;
            _tempPrpertyHolder.Add(holder);
        }

        internal void CompilePropertyExpression(IPropertyRuleExpressionParser<TEntity> propertyparser)
        {
            foreach (var item in _tempPrpertyHolder)
            {
                var property = new PropertyRule<TEntity>.PropertyHolder(item.propertyName, item.isAction);
                if(item.IsExpression())
                {
                    _properties
                        .Add(property, propertyparser.ParserRule(_context, item.propertyName, item.propertyexpression));
                }
                else
                {
                    _properties
                       .Add(property, item.propertyFunction);
                }

            }
        }

        class TemporarypropertyHolder
        {
            public string propertyName;
            public string propertyexpression;
            public Func<TEntity, bool> propertyFunction;
            public bool isAction
            {
                get
                {
                    return propertyName.Equals(actionpropertyName);
                }
            }
            public bool IsExpression()
            {
                return !string.IsNullOrEmpty(propertyexpression);
            }
        }
    }
}
