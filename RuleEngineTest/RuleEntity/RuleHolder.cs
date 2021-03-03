using System;
using System.Collections.Generic;
using System.Dynamic;

namespace RuleEngineTest.RuleEntity
{

    class RuleHolder : DynamicObject
    {
        PropertyBag _bag = new PropertyBag();

        public RuleHolder(string name,List<Property> _properties)
        {
            _bag.Initialize(_properties);
            Name = name;
        }

        public string Name { get; }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
           
            return base.TryConvert(binder, out result);
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var property = _bag[binder.Name];
            if(property!=null)
            {
                result = Convert.ChangeType(property.Value, property.Type);
                return true;
            }
            result = null;
            return false;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetMember(binder.Name, value);
            return true;
        }

        public void SetMember(string name, object value)
        {
            var property = _bag[name];
            if (property != null)
            {
                property.Value = Convert.ChangeType(property.Value, property.Type);
            }
            else
            {

                _bag[name] = new Property(name, value.GetType())
                {
                    Value = value
                };
            }
        }
    }
}
