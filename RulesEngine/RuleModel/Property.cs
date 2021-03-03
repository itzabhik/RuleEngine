using System;
using System.Collections.Generic;

namespace RulesEngine.RuleModel
{
    internal enum PropertyCategory
    {
        Instance,
        Dynamic,
        Calculated,
        NotFound
    }
    internal class Property:IEquatable<Property>
    {
        internal static Property PropertyNotFound;
        static Property()
        {
            PropertyNotFound = new Property(PropertyCategory.NotFound);
        }

        
        public Property(string name, Type type, PropertyCategory category) 
        {
            Name = name;
            Type = type;
            Category = category;
            DefaultValue = Getdefault(type);
        }
        public Property(string name, Type type, object defaultValue, PropertyCategory category)
            : this(name, type, category)
        {
            DefaultValue = defaultValue;
        }
        internal Property(PropertyCategory category)
        {
            Category = category;
        }
        public object DefaultValue { get; }
        public string Name { get; }
        public Type Type { get; }
        public PropertyCategory Category { get; }

        public bool IsInstanceType
        {
            get
            {
                return Category == PropertyCategory.Instance;
            }
        }

        public bool IsDynamicType
        {
            get
            {
                return Category == PropertyCategory.Dynamic;
            }
        }

        public bool IsCalculated
        {
            get
            {
                return Category == PropertyCategory.Calculated;
            }
        }

        public bool IsNotFound
        {
            get
            {
                return Category == PropertyCategory.NotFound;
            }
        }

        internal string ResolveRuleDynamicContext()
        {
            if (Type.IsValueType)
            {
                if (Type == typeof(byte))
                    return FormatWithDoubleQuotes("ByteProp", Name);
                if (Type == typeof(int))
                    return FormatWithDoubleQuotes("IntProp", Name);
                if (Type == typeof(long))
                    return FormatWithDoubleQuotes("LongProp", Name);
                if (Type == typeof(float))
                    return FormatWithDoubleQuotes("FloatProp", Name);
                if (Type == typeof(double))
                    return FormatWithDoubleQuotes("DoubleProp", Name);
                if (Type == typeof(decimal))
                    return FormatWithDoubleQuotes("DecimalProp", Name);
                if (Type == typeof(bool))
                    return FormatWithDoubleQuotes("BooleanProp", Name);
                if (Type == typeof(DateTime))
                    return FormatWithDoubleQuotes("DateTimeProp", Name);

                if (Type == typeof(char))
                    return FormatWithDoubleQuotes("CharProp", Name);
            }

            if(Type==typeof(List<RuleAwareEntity>))
                return FormatWithDoubleQuotes("EnumerableComplexProp", Name);
            if (Type == typeof(List<int>))
                return FormatWithDoubleQuotes("EnumerableIntProp", Name);
            if (Type == typeof(List<long>))
                return FormatWithDoubleQuotes("EnumerableLongProp", Name);
            if (Type == typeof(List<float>))
                return FormatWithDoubleQuotes("EnumerableFloatProp", Name);
            if (Type == typeof(List<double>))
                return FormatWithDoubleQuotes("EnumerableDoubleProp", Name);
            if (Type == typeof(List<decimal>))
                return FormatWithDoubleQuotes("EnumerableDecimalProp", Name);
            if (Type == typeof(List<string>))
                return FormatWithDoubleQuotes("EnumerableStringProp", Name);
            if (Type == typeof(List<char>))
                return FormatWithDoubleQuotes("EnumerableCharProp", Name);

            if (Type == typeof(string))
                    return FormatWithDoubleQuotes("StringProp", Name);
            
            if(Type.IsClass)
                return FormatWithDoubleQuotes("ComplexProp", Name);
            

            return string.Empty;
        }

        private string FormatWithDoubleQuotes(string functionName,string name)
        {
            return string.Format("{0}(\"{1}\")", functionName, name);
        }

        public static object Getdefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            if (type == typeof(string))
                return string.Empty;
            if (type == typeof(List<RuleAwareEntity>))
                return new List<RuleAwareEntity>();

            if (type == typeof(List<int>))
                return new List<int>();
            if (type == typeof(List<long>))
                return new List<long>();
            if (type == typeof(List<float>))
                return new List<float>();
            if (type == typeof(List<double>))
                return new List<double>();
            if (type == typeof(List<decimal>))
                return new List<decimal>();
            if (type == typeof(List<string>))
                return new List<string>();
            if (type == typeof(List<char>))
                return new List<char>();
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Property prop = obj as Property;
            if (prop == null)
                return false;
            return Equals(prop);
        }

        public bool Equals(Property prop)
        {
            if (prop == null)
                return false;
            return this.Name == prop.Name
                && this.Type == prop.Type
                && this.Category==prop.Category;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                hash = hash * 23 + Category.GetHashCode();
                return hash;
            }
        }
    }
}