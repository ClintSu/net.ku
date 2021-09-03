
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utils
{
    public static class StringExtensions
    {
        public static bool IsNull(this string value)
        {
            return value == null;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        public static short ToInt16(this string value)
        {
            short number;
            Int16.TryParse(value, out number);
            return number;
        }

        public static int ToInt32(this string value)
        {
            int number;
            Int32.TryParse(value, out number);
            return number;
        }

        public static long ToInt64(this string value)
        {
            long number;
            Int64.TryParse(value, out number);
            return number;
        }

        public static decimal ToDecimal(this string value)
        {
            decimal number;
            Decimal.TryParse(value, out number);
            return number;
        }

        public static bool ToBoolean(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value");
            }
            string val = value.ToLower().Trim();
            switch (val)
            {
                case "false":
                    return false;
                case "true":
                    return true;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    throw new ArgumentException("Invalid boolean");
            }
        }

        public static T ToEnum<T>(this string value, T defaultValue = default(T)) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Type T Must of type System.Enum");
            }

            T result;
            bool isParsed = Enum.TryParse(value, true, out result);
            return isParsed ? result : defaultValue;
        }

        public static IEnumerable<T> SplitTo<T>(this string value, params char[] separator) where T : IConvertible
        {
            return value.Split(separator, StringSplitOptions.None).Select(s => (T)Convert.ChangeType(s, typeof(T)));
        }

        public static IEnumerable<T> SplitTo<T>(this string value, StringSplitOptions options, params char[] separator) where T : IConvertible
        {
            return value.Split(separator, options).Select(s => (T)Convert.ChangeType(s, typeof(T)));
        }

        public static string Reverse(this string val)
        {
            var chars = new char[val.Length];
            for (int i = val.Length - 1, j = 0; i >= 0; --i, ++j)
            {
                chars[j] = val[i];
            }
            val = new String(chars);
            return val;
        }

        public static IDictionary<string, object>? JsonToDictionary(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            return (Dictionary<string, object>?)System.Text.Json.JsonSerializer.Deserialize(value, typeof(Dictionary<string, object>));
        }

        public static T? JsonToObject<T>(this string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }

        public static string CreateParameters(this string value, bool useOr)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            IDictionary<string, object>? searchParamters = value.JsonToDictionary();
            var @params = new StringBuilder("");
            if (searchParamters == null)
            {
                return @params.ToString();
            }
            for (int i = 0; i <= searchParamters.Count() - 1; i++)
            {
                string key = searchParamters.Keys.ElementAt(i);
                var val = (string)searchParamters[key];
                if (!string.IsNullOrEmpty(key))
                {
                    @params.Append(key).Append(" like '").Append(val.Trim()).Append("%' ");
                    if (i < searchParamters.Count() - 1 && useOr)
                    {
                        @params.Append(" or ");
                    }
                    else if (i < searchParamters.Count() - 1)
                    {
                        @params.Append(" and ");
                    }
                }
            }
            return @params.ToString();
        }
    }
}
