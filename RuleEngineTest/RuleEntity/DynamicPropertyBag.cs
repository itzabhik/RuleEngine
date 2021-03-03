using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineTest.RuleEntity
{
    class DynamicPropertyBag
    {
        private Dictionary<String, DynamicProperty> _properties = new Dictionary<string, DynamicProperty>();

        public DynamicProperty this[string name]
        {
            get
            {
                return _properties[name];
            }
            set
            {
                _properties[name] = value;
            }
        }

        internal void Initialize(List<DynamicProperty> properties)
        {
            properties.ForEach(p => _properties.Add(p.Name, p));
        }
    }
}
