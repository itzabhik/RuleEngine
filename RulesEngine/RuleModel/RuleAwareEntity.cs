using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleEngineMetadata;
using System;
using System.Reflection;

namespace RulesEngine.RuleModel
{
    public partial class RuleAwareEntity : ContextAwareRuleEntity
    {
      
        public RuleAwareEntity(string entityType)
            : base(entityType)
        { 
            
        }

       
        internal void InitializeCalculatedProperty(Property property, object value)
        {
            _dynamicContext.InitializeCalculatedProperty(property, value);
        }

        public virtual object this[string property]
        {
            get
            {
                return Prop(property);
            }
            set
            {
                SetPropertyValue(property, value);
            }
        }

        public bool SetPropertyValue(string property, object value)
        {
            if (IsComplexProperty(property) && TrySetComplexProperty(property, value))
            {
                return true;
            }
            else if (TrySetDynamicProperty(property, value))
            {
                return true;
            }

            else if (TrySetCalculatedProperty(property, value))
            {
                return true;
            }

            else if (TrySetInstanceProperty(property, value))
            {
                return true;
            }

            throw new PropertyNotFoundException(property, EntityType);
        }

        private bool TrySetComplexProperty(string property, object value)
        {
            if (TrySetInstanceProperty(property, value))
            {
                return true;
            }
            string propertyToSet;
            RuleAwareEntity ruleEntity = GetComplexDynamicPropertyInstance(property, out propertyToSet);
            if (ruleEntity != null)
            {
                return ruleEntity.SetPropertyValue(propertyToSet, value);

            }
            return false;
        }

        public bool TrySetCalculatedProperty(string name, object value)
        {

            if (_dynamicContext.ContainsCalculatedProperty(name))
            {
                _dynamicContext.SetCalculatedProperty(name, value);
                
                return true;
            }

            return false;
        }
        public bool TrySetDynamicProperty(string name, object value)
        {
            if (_dynamicContext.ContainsDynamicProperty(name))
            {
                _dynamicContext.SetDynamicProperty(name, value);
                return true;
            }

            return false;
        }

        public bool TrySetInstanceProperty(string name, object value)
        {
            PropertyInfo prop;
            object instance = null;
            if (!TryGetInstanceProperty(name, out prop, out instance))
                return false;
            prop.SetValue(instance, value);
            return true;

        }


        public TType GetPropertyOfType<TType>(string property, bool checkType = true)
        {
            TType val = default(TType);

            if (IsComplexProperty(property) && TryGetComplexPropetyOfType(property, out val, checkType))
            {
                return val;
            }
            else if (TryGetDynamicPropetyOfType(property, out val, checkType))
                return val;
            else if (TryGetCalculatedPropetyOfType(property, out val, checkType))
                return val;
            else if (TryGetInstancePropetyOfType(property, out val, checkType))
                return val;

            throw new PropertyNotFoundException(property, EntityType);

        }

        public bool TryGetComplexPropetyOfType<TType>(string property, out TType value, bool checkType)
        {
            value = default(TType);
            if (TryGetInstancePropetyOfType(property, out value, checkType))
                return true;
            string instanceProperty;
            var dynamicRule = GetComplexDynamicPropertyInstance(property, out instanceProperty);

            if (dynamicRule != null)
            {
                value = dynamicRule.GetPropertyOfType<TType>(instanceProperty, checkType);
                return true;
            }

            return false;
        }

        public bool TryGetCalculatedPropetyOfType<TType>(string property, out TType value, bool checkType)
        {

            value = default(TType);

            if (!_dynamicContext.ContainsCalculatedProperty(property))
                return false;

            var prop = _dynamicContext.SearchCalculatedProperty(property);

            if (checkType && prop.Type != typeof(TType))
                throw new ProprtyTypeMismatch(property, typeof(TType), EntityType);
                    

            value = (TType)Convert.ChangeType(_dynamicContext.GatCalculatedProperty(property), prop.Type);
            return true;
        }

        

        public bool TryGetInstancePropetyOfType<TType>(string property, out TType value, bool checkType = true)
        {
            value = default(TType);
            PropertyInfo prop;
            object instance;
            if (!TryGetInstanceProperty(property, out prop, out instance))
                return false;

            if (checkType && prop.PropertyType != typeof(TType))
                throw  new ProprtyTypeMismatch(property, typeof(TType), EntityType);

            value = (TType)prop.GetValue(instance);
            return true;
        }

        public bool TryGetDynamicPropetyOfType<TType>(string property, out TType value, bool checkType = true)
        {
            value = default(TType);

            if (!_dynamicContext.ContainsDynamicProperty(property))
            {
                return false;
            }


            var prop = _dynamicContext.SearchDynamicProperty(property);

            if (checkType && prop.Type != typeof(TType))
                throw new ProprtyTypeMismatch(property, typeof(TType), EntityType);

            value = (TType)Convert.ChangeType(_dynamicContext.GatDynamicProperty(property), prop.Type);
            return true;
        }

        private bool TryGetInstanceProperty(string property, out PropertyInfo info, out object instance)
        {
            if (property.Contains("."))
            {
                info = GetInstanceComplexPropertyInstance(this, property, out instance);
            }
            else
            {

                info = RuleAwareEntityPropertyInfo.GetInstancePropertyInfo(GetType(), property);
                instance = this;
            }


            if (info == null)
                return false;
            return true;
        }

        private RuleAwareEntity GetComplexDynamicPropertyInstance(string complexProperty, out string instanceProperty)
        {
            instanceProperty = string.Empty;
            RuleAwareEntity obj = this;
            var props = complexProperty.Split('.');
            instanceProperty = props[props.Length - 1];
            for (int i = 0; i <= props.Length - 2; i++)
            {
                string part = props[i];

                if (obj == null) { return null; }

                var dynamicRule = obj as RuleAwareEntity;
                if (dynamicRule == null)
                    return null;
                var gg = dynamicRule.Prop(part).GetType();
                obj = dynamicRule.Prop(part) as RuleAwareEntity;
            }


            return obj;
        }

        private PropertyInfo GetInstanceComplexPropertyInstance(object obj, string propName, out object instance)
        {
            instance = null;
            PropertyInfo info = null;
            foreach (String part in propName.Split('.'))
            {
                if (obj == null) { return null; }

                instance = obj;

                Type type = obj.GetType();

                info = RuleAwareEntityPropertyInfo.GetInstancePropertyInfo(type, part);
                if (info == null)
                {
                    return null;
                }

                obj = info.GetValue(obj, null);
            }

            return info;

        }
        private bool IsComplexProperty(string complexProperty)
        {
            return complexProperty.Contains(".");
        }

    }
}
