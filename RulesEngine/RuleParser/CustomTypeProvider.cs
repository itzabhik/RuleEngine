using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace RulesEngine.RuleParser
{
    class CustomTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        private HashSet<Type> _types;
        public CustomTypeProvider(HashSet<Type> types) : base()
        {
            _types = types;
        }

        public override HashSet<Type> GetCustomTypes()
        {
            return _types;
        }


    }
}
