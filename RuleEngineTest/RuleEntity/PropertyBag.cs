using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineTest.RuleEntity
{
    class PropertyBag
    {
        private Dictionary<String, Property> _properties = new Dictionary<string, Property>();

        public Property this[string name]
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

        internal void Initialize(List<Property> properties)
        {
            properties.ForEach(p => _properties.Add(p.Name, p));
        }
    }
}
