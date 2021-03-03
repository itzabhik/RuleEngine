using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RulesEngine.RuleParser
{
   
    public static class List
    {
       
        public static List<byte> OfByte(params byte[] values)
        {
            if (values == null)
                return new List<byte>();
            return new List<byte>(values);
        }

        public static List<byte> Append(List<byte> source, params byte[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }
        

        public static List<int> OfInt(params int[] values)
        {
            if (values == null)
                return new List<int>();
            return new List<int>(values);
        }

        public static List<int> Append(List<int> source, params int[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }
       
        public static List<long> OfLong(params long[] values)
        {
            if (values == null)
                return new List<long>();
            return new List<long>(values);
        }

        public static List<long> Append(List<long> source, params long[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }

        public static List<float> OfFloat(params float[] values)
        {
            if (values == null)
                return new List<float>();
            return new List<float>(values);
        }
        public static List<float> Append(List<float> source, params float[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }

        public static List<double> OfDouble(params double[] values)
        {
            if (values == null)
                return new List<double>();
            return new List<double>(values);
        }
        public static List<double> Append(List<double> source, params double[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }
        public static List<decimal> OfDecimal(params decimal[] values)
        {
            if (values == null)
                return new List<decimal>();
            return new List<decimal>(values);
        }
        public static List<decimal> Append(List<decimal> source, params decimal[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }

        public static List<char> OfChar(params char[] values)
        {
            if (values == null)
                return new List<char>();
            return new List<char>(values);
        }
        public static List<char> Append(List<char> source, params char[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }


        public static List<string> OfString(params string[] values)
        {
            if (values == null)
                return new List<string>();
            return new List<string>(values);
        }
       
        public static List<string> Append(List<string> source, params string[] values)
        {
            if (values != null)
                source.AddRange(values);
            return source;
        }
        

    }
}
