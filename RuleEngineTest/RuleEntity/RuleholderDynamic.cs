using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineTest.RuleEntity
{
    class RuleholderDynamic:DynamicClass
    {
        DynamicPropertyBag _bag = new DynamicPropertyBag();
        public RuleholderDynamic(string name, List<DynamicProperty> _properties)
        {
            _bag.Initialize(_properties);
            Name = name;
           
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var property = _bag[binder.Name];
            if (property != null)
            {
          
                result = Convert.ChangeType(GetDynamicPropertyValue(binder.Name), property.Type);
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
                SetDynamicPropertyValue(name, Convert.ChangeType(value, property.Type));
            }
            else
            {
                SetDynamicPropertyValue(name, Convert.ChangeType(value, property.Type));

               
            }
        }

        public string Name { get; }
    }
}
