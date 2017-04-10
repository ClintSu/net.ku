using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    /// <summary>
    /// 类型安全转换，省略try...catch
    /// </summary>
    public static class SafeConvert
    {
        public static object ChangeType(object obj, Type conversionType)
        {
            switch (conversionType.Name.ToLower())
            {
                case "int32":
                    return (object)SafeConvert.ToInt32(obj);
                case "string":
                    return (object)SafeConvert.ToString(obj);
                default:
                    return Convert.ChangeType(obj, conversionType);
            }
        }

        public static bool IsDBNull(object obj)
        {
            return Convert.IsDBNull(obj);
        }

        public static Guid ToGuid(object obj)
        {
            if (obj != null)
            {
                if (obj != DBNull.Value)
                {
                    try
                    {
                        return new Guid(obj.ToString());
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
            }
            return Guid.Empty;
        }

        public static TimeSpan ToTimeSpan(object obj)
        {
            return SafeConvert.ToTimeSpan(obj, TimeSpan.Zero);
        }

        public static TimeSpan ToTimeSpan(object obj, TimeSpan defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToTimeSpan(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static TimeSpan ToTimeSpan(string s, TimeSpan defaultValue)
        {
            TimeSpan result;
            if (!TimeSpan.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static TimeSpan ToTimeSpan(string s)
        {
            return SafeConvert.ToTimeSpan(s, TimeSpan.Zero);
        }

        public static string ToString(object obj)
        {
            if (obj != null)
                return obj.ToString();
            return string.Empty;
        }

        public static string ToString(string s)
        {
            return SafeConvert.ToString(s, string.Empty);
        }

        public static string ToString(string s, string defaultString)
        {
            if (s == null)
                return defaultString;
            return s.ToString();
        }

        public static string ToString(object s, string defaultString)
        {
            if (s == null)
                return defaultString;
            return s.ToString();
        }

        public static double ToDouble(string s, double defaultValue)
        {
            double result;
            if (!double.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static double ToDouble(string s)
        {
            return SafeConvert.ToDouble(s, 0.0);
        }

        public static double ToDouble(object obj)
        {
            return SafeConvert.ToDouble(obj, 0.0);
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToDouble(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static float ToSingle(string s, float defaultValue)
        {
            float result;
            if (!float.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static float ToSingle(string s)
        {
            return SafeConvert.ToSingle(s, 0.0f);
        }

        public static float ToSingle(object obj)
        {
            return SafeConvert.ToSingle(obj, 0.0f);
        }

        public static float ToSingle(object obj, float defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToSingle(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static Decimal ToDecimal(string s, Decimal defaultValue)
        {
            Decimal result;
            if (!Decimal.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static Decimal ToDecimal(string s)
        {
            return SafeConvert.ToDecimal(s, new Decimal(0));
        }

        public static Decimal ToDecimal(object obj)
        {
            return SafeConvert.ToDecimal(obj, new Decimal(0));
        }

        public static Decimal ToDecimal(object obj, Decimal defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToDecimal(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static bool ToBoolean(string s, bool defaultValue)
        {
            if (s == "1")
                return true;
            bool result;
            if (!bool.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static bool ToBoolean(string s)
        {
            return SafeConvert.ToBoolean(s, false);
        }

        public static bool ToBoolean(object obj)
        {
            return SafeConvert.ToBoolean(obj, false);
        }

        public static bool ToBoolean(object obj, bool defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToBoolean(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static char ToChar(string s, char defaultValue)
        {
            char result;
            if (!char.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static char ToChar(string s)
        {
            return SafeConvert.ToChar(s, char.MinValue);
        }

        public static char ToChar(object obj)
        {
            return SafeConvert.ToChar(obj, char.MinValue);
        }

        public static char ToChar(object obj, char defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToChar(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static byte ToByte(string s, byte defaultValue)
        {
            byte result;
            if (!byte.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static byte ToByte(string s)
        {
            return SafeConvert.ToByte(s, (byte)0);
        }

        public static byte ToByte(object obj)
        {
            return SafeConvert.ToByte(obj, (byte)0);
        }

        public static byte ToByte(object obj, byte defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToByte(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static sbyte ToSByte(string s, sbyte defaultValue)
        {
            sbyte result;
            if (!sbyte.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static sbyte ToSByte(string s)
        {
            return SafeConvert.ToSByte(s, (sbyte)0);
        }

        public static sbyte ToSByte(object obj)
        {
            return SafeConvert.ToSByte(obj, (sbyte)0);
        }

        public static sbyte ToSByte(object obj, sbyte defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToSByte(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static short ToInt16(string s, short defaultValue)
        {
            short result;
            if (!short.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static short ToInt16(string s)
        {
            return SafeConvert.ToInt16(s, (short)0);
        }

        public static short ToInt16(object obj)
        {
            return SafeConvert.ToInt16(obj, (short)0);
        }

        public static short ToInt16(object obj, short defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToInt16(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static ushort ToUInt16(string s, ushort defaultValue)
        {
            ushort result;
            if (!ushort.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static ushort ToUInt16(string s)
        {
            return SafeConvert.ToUInt16(s, (ushort)0);
        }

        public static ushort ToUInt16(object obj)
        {
            return SafeConvert.ToUInt16(obj, (ushort)0);
        }

        public static ushort ToUInt16(object obj, ushort defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToUInt16(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static int ToInt32(string s, int defaultValue)
        {
            int result;
            if (!int.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static int ToInt32(string s)
        {
            return SafeConvert.ToInt32(s, 0);
        }

        public static int ToInt32(object obj)
        {
            return SafeConvert.ToInt32(obj, 0);
        }

        public static int ToInt32(object obj, int defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToInt32(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static uint ToUInt32(string s, uint defaultValue)
        {
            uint result;
            if (!uint.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static uint ToUInt32(string s)
        {
            return SafeConvert.ToUInt32(s, 0U);
        }

        public static uint ToUInt32(object obj)
        {
            return SafeConvert.ToUInt32(obj, 0U);
        }

        public static uint ToUInt32(object obj, uint defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToUInt32(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static long ToInt64(string s, long defaultValue)
        {
            long result;
            if (!long.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static long ToInt64(string s)
        {
            return SafeConvert.ToInt64(s, 0L);
        }

        public static long ToInt64(object obj)
        {
            return SafeConvert.ToInt64(obj, 0L);
        }

        public static long ToInt64(object obj, long defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToInt64(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static ulong ToUInt64(string s, ulong defaultValue)
        {
            ulong result;
            if (!ulong.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static ulong ToUInt64(string s)
        {
            return SafeConvert.ToUInt64(s, 0UL);
        }

        public static ulong ToUInt64(object obj)
        {
            return SafeConvert.ToUInt64(obj, 0UL);
        }

        public static ulong ToUInt64(object obj, ulong defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToUInt64(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static DateTime ToDateTime(string s, DateTime defaultValue)
        {
            DateTime result;
            if (!DateTime.TryParse(s, out result))
                return defaultValue;
            return result;
        }

        public static DateTime ToDateTime(string s)
        {
            return SafeConvert.ToDateTime(s, DateTime.MinValue);
        }

        public static DateTime ToDateTime(object obj)
        {
            return SafeConvert.ToDateTime(obj, DateTime.MinValue);
        }

        public static DateTime ToDateTime(object obj, DateTime defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToDateTime(obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static object ToEnum(Type enumType, string text, object defaultValue)
        {
            if (Enum.IsDefined(enumType, (object)text))
                return Enum.Parse(enumType, text, false);
            return defaultValue;
        }

        public static object ToEnum(Type enumType, object obj, object defaultValue)
        {
            if (obj != null)
                return SafeConvert.ToEnum(enumType, obj.ToString(), defaultValue);
            return defaultValue;
        }

        public static object ToEnum(Type enumType, int index)
        {
            return Enum.ToObject(enumType, index);
        }

        public static List<T> SafeStringToArray<T>(string str, char[] separator, Converter<string, T> converter)
        {
            List<T> objList = new List<T>();
            if (string.IsNullOrEmpty(str))
                return objList;
            string[] strArray = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (strArray == null || strArray.Length == 0)
                return objList;
            foreach (string input in strArray)
            {
                try
                {
                    objList.Add(converter(input));
                }
                catch
                {
                }
            }
            return objList;
        }

        public static string ArrayToString<T>(List<T> inputs, string separator)
        {
            if (inputs == null || inputs.Count == 0)
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (T input in inputs)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(separator);
                stringBuilder.Append(input.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
