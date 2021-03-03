using System;

namespace RuleEngineTest.RuleEntity
{
    internal class Property
    {
        public Property(string name, Type type)
        {
            Name = name;
            Type = type;
            Value = Getdefault();
        }

        public object Value { get; set; }
        public string Name { get; }
        public Type Type { get; }

        private object Getdefault()
        {
            if (Type.IsValueType)
            {
                return Activator.CreateInstance(Type);
            }
            return null;
        }
    }
}