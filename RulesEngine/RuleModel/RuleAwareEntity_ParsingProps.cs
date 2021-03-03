using System;
using System.Collections.Generic;

namespace RulesEngine.RuleModel
{
    partial class RuleAwareEntity
    {
        public byte ByteProp(string property)
        {
            return GetPropertyOfType<byte>(property);
        }
        public int IntProp(string property)
        {
            return GetPropertyOfType<int>(property);
        }

        public long LongProp(string property)
        {
            return GetPropertyOfType<long>(property);
        }

        public float FloatProp(string property)
        {
            return GetPropertyOfType<float>(property);
        }

        public double DoubleProp(string property)
        {
            return GetPropertyOfType<double>(property);
        }
        public decimal DecimalProp(string property)
        {
            return GetPropertyOfType<decimal>(property);
        }

        public bool BooleanProp(string property)
        {
            return GetPropertyOfType<bool>(property);
        }
        public DateTime DateTimeProp(string property)
        {
            return GetPropertyOfType<DateTime>(property);
        }

        public string StringProp(string property)
        {
            return GetPropertyOfType<string>(property);
        }

        public char CharProp(string property)
        {
            return GetPropertyOfType<char>(property);
        }

        public RuleAwareEntity ComplexProp(string property)
        {
            return GetPropertyOfType<RuleAwareEntity>(property);
        }

        public List<RuleAwareEntity> EnumerableComplexProp(string property)
        {
            return GetPropertyOfType<List<RuleAwareEntity>>(property);
        }

        public List<int> EnumerableIntProp(string property)
        {
            return GetPropertyOfType<List<int>>(property);
        }

        public List<long> EnumerableLongProp(string property)
        {
            return GetPropertyOfType<List<long>>(property);
        }

        public List<float> EnumerableFloatProp(string property)
        {
            return GetPropertyOfType<List<float>>(property);
        }

        public List<double> EnumerableDoubleProp(string property)
        {
            return GetPropertyOfType<List<double>>(property);
        }
        public List<decimal> EnumerableDecimalProp(string property)
        {
            return GetPropertyOfType<List<decimal>>(property);
        }

        public List<string> EnumerableStringProp(string property)
        {
            return GetPropertyOfType<List<string>>(property);
        }

        public List<char> EnumerableCharProp(string property)
        {
            return GetPropertyOfType<List<char>>(property);
        }

        public object Prop(string property)
        {
            return GetPropertyOfType<object>(property, false);
        }
    }
}
