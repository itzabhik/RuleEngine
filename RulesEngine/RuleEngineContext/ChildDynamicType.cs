using System;

namespace RulesEngine.RuleEngineContext
{
    class ChildDynamicType : IEquatable<ChildDynamicType>
    {
        public ChildDynamicType(string entityType, Type childType)
        {
            EntityType = entityType;
            ChildType = childType;
        }

        public string EntityType { get; }
        public Type ChildType { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var rhs = obj as ChildDynamicType;
            if (rhs == null)
                return false;
            return Equals(rhs);
        }

        public bool Equals(ChildDynamicType other)
        {
            if (other == null)
                return false;
            return this.EntityType == other.EntityType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EntityType.GetHashCode();
                hash = hash * 23 + ChildType.GetHashCode();
                
                return hash;
            }

           
        }
    }
}
