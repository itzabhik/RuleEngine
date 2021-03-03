using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using System;
using System.Collections.Generic;

namespace RulesEngine.Rules
{
    class PropertyRule<TEntity> : AbstractRule<TEntity>, IPropertyRule<TEntity> where TEntity : RuleAwareEntity
    {
        private readonly Dictionary<PropertyHolder, Func<TEntity, bool>> _properties 
            = new Dictionary<PropertyHolder, Func<TEntity, bool>>();
        private readonly bool _collectDiagnostic;

        public PropertyRule(RuleEngineContext.RuleEngineContext context,string rule, Func<TEntity, bool> rulefunc,
            Dictionary<PropertyHolder, Func<TEntity, bool>> properties, string name, string description="", bool collectDiagnostic = true) 
            : base(context, rule,rulefunc, name, description)
        {
            _properties = properties;
            this._collectDiagnostic = collectDiagnostic;
        }
        public void SetPropertyValues(TEntity entity)
        {
            foreach (var item in _properties)
            {
                item.Value(entity);
            }

            CollectDiagnostic(entity);

        }

        private void CollectDiagnostic(TEntity entity)
        {
            if (_collectDiagnostic)
            {
               
                entity.RulePassedDiagnostic(Rule);
                HashSet<string> uniqueProp = new HashSet<string>();

                foreach (var item in _properties)
                {
                    if(item.Key.IsAction)
                    {
                        entity.PropertySetDiagnostic(item.Key.Name,"Action Executed");
                    }
                    else if(uniqueProp.Add(item.Key.Name))
                        entity.PropertySetDiagnostic(item.Key.Name, entity.Prop(item.Key.Name));
                    
                }
            }
                
        }

        public class PropertyHolder
        {

            public PropertyHolder(string name, bool isAction=false)
            {
                Name = name;
                IsAction = isAction;
            }

            public string Name { get; }
            public bool IsAction { get; }
        }
    }
}
