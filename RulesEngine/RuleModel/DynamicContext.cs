

using System;

namespace RulesEngine.RuleModel
{
    class DynamicContext
    {
        private PropertyBag _dynamicPropertyBag;
        private PropertyBag _calculatedPropertyBag;
      

        public DynamicContext(string entityType)
        {
            _dynamicPropertyBag = new PropertyBag( BagType.Dynamic, entityType);
            _calculatedPropertyBag = new PropertyBag( BagType.Calculated, entityType);
        }

        
        internal void InitializeCalculatedProperty(Property property, object value)
        {
            _calculatedPropertyBag.Initialize(property, value);
        }

        internal bool ContainsDynamicProperty(string name)
        {
            return _dynamicPropertyBag.ContainsProperty(name);
        }

        internal void SetDynamicProperty(string name, object value)
        {
            _dynamicPropertyBag[name] = value;
        }

        internal void SetCalculatedProperty(string name, object value)
        {
            _calculatedPropertyBag[name] = value;
        }

        internal bool ContainsCalculatedProperty(string name)
        {
            return _calculatedPropertyBag.ContainsProperty(name);
        }

        internal Property SearchCalculatedProperty(string property)
        {
            return _calculatedPropertyBag.SearchProperty(property);
        }

        internal Property SearchDynamicProperty(string property)
        {
            return _dynamicPropertyBag.SearchProperty(property);
        }

        internal object GatCalculatedProperty(string property)
        {
            return _calculatedPropertyBag[property];
        }

        internal object GatDynamicProperty(string property)
        {
            return _dynamicPropertyBag[property];
        }
    }
}
