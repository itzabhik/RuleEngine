using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace RulesEngine.RuleEngineContext
{
    internal class RuleEntityContextMetadata
    {
        private Dictionary<string,Type> _dynamicTypes=new Dictionary<string, Type>();
       

        private  Dictionary<string, Dictionary<string, ChildDynamicType>> _childDynamicTypes
          = new Dictionary<string, Dictionary<string, ChildDynamicType>>();

        private  Dictionary<string, string> _alias
          = new  Dictionary<string, string>();

        private static  HashSet<Type> _custoTypes=new HashSet<Type>();

       

        static RuleEntityContextMetadata()
        {
            _custoTypes.Add(typeof(RuleAwareEntity));
            _custoTypes.Add(typeof(List<RuleAwareEntity>));
            _custoTypes.Add(typeof(List));
            _custoTypes.Add(typeof(IEnumerable<RuleAwareEntity>));
            _custoTypes.Add(typeof(IQueryable<RuleAwareEntity>));
            _custoTypes.Add(typeof(IQueryable));
        }

        internal void AssociateDynamicType(string entityType, Type type, string alias)
        {
            if(!_dynamicTypes.ContainsKey(entityType))
            {
                _dynamicTypes.Add(entityType, type);
                AttachCustomType(type);
            }

            AssociateAliasForDynamicType(entityType, alias);
        }

        internal  ChildDynamicType GetChildDynamicType(string entityType, string property)
        {
            if(_childDynamicTypes.ContainsKey(entityType))
            {
                var childType = _childDynamicTypes[entityType];
                if (childType.ContainsKey(property))
                    return childType[property];
            }
            return null;
        }

        internal Type GetTypeForEntityType(string entityType)
        {
            Type type= null;

            _dynamicTypes.TryGetValue(entityType, out type);

            return type;
        }


        public void AttachCustomType(Type type)
        {
            if (type.IsValueType)
                return;
            _custoTypes.Add(type);
        }

        internal  ParsingConfig GetRuleParserConfig()
        {
            return new ParsingConfig
            {
                CustomTypeProvider = new CustomTypeProvider(_custoTypes)
            };
        }


        internal  void AssociateChildDynamicType<TEnity, TChildEntity>(string parententityType, string childEntityType, string parentProperty)
        {

            var childEntity = new ChildDynamicType(childEntityType, typeof(TChildEntity));

            if (_childDynamicTypes.ContainsKey(parententityType))
            {
                var propHolder = _childDynamicTypes[parententityType];
                if (!propHolder.ContainsKey(parentProperty))
                    propHolder.Add(parentProperty, childEntity);
            }
            else
            {
                _childDynamicTypes.Add(parententityType, new Dictionary<string, ChildDynamicType>
                {
                    {parentProperty, childEntity}
                });
            }

          
        }
       
        internal  void AssociateAliasForDynamicType(string entityType, string alias)
        {
            if (!_alias.ContainsKey(entityType))
            {
                _alias.Add(alias, entityType);
            }
            
        }

        internal  string GetDynamicTypeFromAlias( string alias)
        {
            if (_alias.ContainsKey(alias))
            {
                return _alias[alias];
            }
            return string.Empty;
        }


     

    }
}
