using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleEngineMetadata;
using System;
using System.Collections.Generic;

namespace RulesEngine.RuleModel
{
    enum BagType
    {
        Dynamic,
        Calculated
    }

    class PropertyBag
    {
      
        private Dictionary<Property, object> _propertyValue = new Dictionary<Property, object>();
       
       
        private readonly string _entityType;

        public BagType BagType { get; }

        public PropertyBag(BagType bagType, string entityType)
        {
            BagType = bagType;
            this._entityType = entityType;
        }

        public object this[string name]
        {
            get
            {
                Property prop = SearchProperty(name);
                object value;
                _propertyValue.TryGetValue(prop, out value);
                if (value == null)
                    value = prop.DefaultValue;
                 return value;
            }
            set
            {
                Property prop = SearchProperty(name);
                _propertyValue[prop] = value;
            }
        }

        internal void Initialize(Property property, object defaultValue)
        {
            if (_propertyValue.ContainsKey(property))
                throw new Exception("Property Already Created");
            else
                _propertyValue.Add(property, defaultValue);
        }

        internal void Initialize(Property property)
        {
            Initialize(property, property.DefaultValue);
        }



        public Property SearchProperty(string property)
        {
            if (BagType == BagType.Dynamic)
            {
                if (ContainsProperty(property))
                    return RuleAwareEntityPropertyInfo.GetDynamicProperty(_entityType, property);
            }
            if (BagType == BagType.Calculated)
            {
                if (ContainsProperty(property))
                    return RuleAwareEntityPropertyInfo.GetCalculatedProperty(_entityType, property);
            }

            throw new PropertyNotFoundException(property, _entityType);
        }
        public bool ContainsProperty(string property)
        {
            if(BagType == BagType.Dynamic)
            {
                var prop = RuleAwareEntityPropertyInfo.GetDynamicProperty(_entityType, property);
                if (prop.IsNotFound)
                    return false;
                return true;
            }

            if (BagType == BagType.Calculated)
            {
                var prop = RuleAwareEntityPropertyInfo.GetCalculatedProperty(_entityType, property);
                if (prop.IsNotFound)
                    return false;
                return true;
            }
            return false;

        }

    }
}
