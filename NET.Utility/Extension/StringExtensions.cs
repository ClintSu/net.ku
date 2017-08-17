using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace System
{
    /// <summary>
    ///     String常用扩展
    /// </summary>
    public static class StringExtensions
    {
        #region 打开文件或网址

        /// <summary>
        ///     打开文件或网址
        /// </summary>
        /// <param name="s"></param>
        public static void Open(this string s)
        {
            if (s == "")
            {
                return;
            }
            Process.Start(s);
        }

        #endregion

        #region 执行DOS命令

        /// <summary>
        ///     执行DOS命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="error">执行命令产生的错误</param>
        /// <returns></returns>
        public static string ExecuteDos(this string cmd, out string error)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            process.Start();

            process.StandardInput.WriteLine(cmd);
            process.StandardInput.WriteLine("exit");
            error = process.StandardError.ReadToEnd();
            return process.StandardOutput.ReadToEnd();
        }

        #endregion

        #region 常用扩展

        /// <summary>
        ///     如果字符串为NULL 则返回string.Empty，否则，返回原字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NullToEmpty(this string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>忽略大小写的字符串相等比较，判断是否以任意一个待比较字符串相等</summary>
        /// <param name="value">字符串</param>
        /// <param name="strs">待比较字符串数组</param>
        /// <returns></returns>
        public static bool EqualIgnoreCase(this string value, params string[] strs)
        {
            foreach (var item in strs)
            {
                if (string.Equals(value, item, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>忽略大小写的字符串开始比较，判断是否以任意一个待比较字符串开始</summary>
        /// <param name="value">字符串</param>
        /// <param name="strs">待比较字符串数组</param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(this string value, params string[] strs)
        {
            if (string.IsNullOrEmpty(value)) return false;

            foreach (var item in strs)
            {
                if (value.StartsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>忽略大小写的字符串结束比较，判断是否以任意一个待比较字符串结束</summary>
        /// <param name="value">字符串</param>
        /// <param name="strs">待比较字符串数组</param>
        /// <returns></returns>
        public static bool EndsWithIgnoreCase(this string value, params string[] strs)
        {
            if (string.IsNullOrEmpty(value)) return false;

            foreach (var item in strs)
            {
                if (value.EndsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>指示指定的字符串是 null 还是 String.Empty 字符串</summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return value == null || value.Length <= 0;
        }

        /// <summary>是否空或者空白字符串</summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value != null)
            {
                for (var i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i])) return false;
                }
            }
            return true;
        }

        /// <summary>拆分字符串，过滤空格，无效时返回空数组</summary>
        /// <param name="value">字符串</param>
        /// <param name="separators">分组分隔符，默认逗号分号</param>
        /// <returns></returns>
        public static string[] Split(this string value, params string[] separators)
        {
            if (string.IsNullOrEmpty(value)) return new string[0];
            if (separators == null || separators.Length < 1 || separators.Length == 1 && separators[0].IsNullOrEmpty())
                separators = new[] {",", ";"};

            return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>拆分字符串成为整型数组，默认逗号分号分隔，无效时返回空数组</summary>
        /// <remarks>过滤空格、过滤无效、不过滤重复</remarks>
        /// <param name="value">字符串</param>
        /// <param name="separators">分组分隔符，默认逗号分号</param>
        /// <returns></returns>
        public static int[] SplitAsInt(this string value, params string[] separators)
        {
            if (string.IsNullOrEmpty(value)) return new int[0];
            if (separators == null || separators.Length < 1) separators = new[] {",", ";"};

            var ss = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<int>();
            foreach (var item in ss)
            {
                var id = 0;
                if (!int.TryParse(item.Trim(), out id)) continue;

                // 本意只是拆分字符串然后转为数字，不应该过滤重复项
                //if (!list.Contains(id))
                list.Add(id);
            }

            return list.ToArray();
        }

        /// <summary>拆分字符串成为名值字典。逗号分号分组，等号分隔</summary>
        /// <param name="value">字符串</param>
        /// <param name="nameValueSeparator">名值分隔符，默认等于号</param>
        /// <param name="separators">分组分隔符，默认逗号分号</param>
        /// <returns></returns>
        public static IDictionary<string, string> SplitAsDictionary(this string value, string nameValueSeparator = "=",
            params string[] separators)
        {
            var dic = new Dictionary<string, string>();
            if (value.IsNullOrWhiteSpace()) return dic;

            if (string.IsNullOrEmpty(nameValueSeparator)) nameValueSeparator = "=";
            if (separators == null || separators.Length < 1) separators = new[] {",", ";"};

            var ss = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (ss == null || ss.Length < 1) return null;

            foreach (var item in ss)
            {
                var p = item.IndexOf(nameValueSeparator);
                // 在前后都不行
                if (p <= 0 || p >= item.Length - 1) continue;

                var key = item.Substring(0, p).Trim();
                dic[key] = item.Substring(p + nameValueSeparator.Length).Trim();
            }

            return dic;
        }

        /// <summary>把一个列表组合成为一个字符串，默认逗号分隔</summary>
        /// <param name="value"></param>
        /// <param name="separator">组合分隔符，默认逗号</param>
        /// <returns></returns>
        public static string Join(this IEnumerable value, string separator = ",")
        {
            var sb = new StringBuilder();
            if (value != null)
            {
                foreach (var item in value)
                {
                    sb.Separate(separator).Append(item + "");
                }
            }
            return sb.ToString();
        }

        /// <summary>把一个列表组合成为一个字符串，默认逗号分隔</summary>
        /// <param name="value"></param>
        /// <param name="separator">组合分隔符，默认逗号</param>
        /// <param name="func">把对象转为字符串的委托</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> value, string separator = ",", Func<T, string> func = null)
        {
            var sb = new StringBuilder();
            if (value != null)
            {
                if (func == null) func = obj => obj + "";
                foreach (var item in value)
                {
                    sb.Separate(separator).Append(func(item));
                }
            }
            return sb.ToString();
        }

        /// <summary>追加分隔符字符串，忽略开头，常用于拼接</summary>
        /// <param name="sb">字符串构造者</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static StringBuilder Separate(this StringBuilder sb, string separator)
        {
            if (sb == null || string.IsNullOrEmpty(separator)) return sb;

            if (sb.Length > 0) sb.Append(separator);

            return sb;
        }

        /// <summary>字符串转数组</summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码，默认utf-8无BOM</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string value, Encoding encoding = null)
        {
            if (value == null) return null;
            if (value == string.Empty) return new byte[0];

            if (encoding == null) encoding = Encoding.UTF8;
            return encoding.GetBytes(value);
        }

        /// <summary>格式化字符串。特别支持无格式化字符串的时间参数</summary>
        /// <param name="value">格式字符串</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static string F(this string value, params object[] args)
        {
            if (string.IsNullOrEmpty(value)) return value;

            // 特殊处理时间格式化。这些年，无数项目实施因为时间格式问题让人发狂
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] is DateTime)
                {
                    // 没有写格式化字符串的时间参数，一律转为标准时间字符串
                    if (value.Contains("{" + i + "}")) args[i] = ((DateTime) args[i]).ToFullString();
                }
            }

            return string.Format(value, args);
        }

        #endregion

        #region 序列化

        /// <summary>
        ///     Json反序列化
        /// </summary>
        /// <typeparam name="T">T 对象</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns>T 对象</returns>
        public static T DeserializeJson<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        ///     Json序列化
        /// </summary>
        /// <typeparam name="T">T 对象</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>Json字符串</returns>
        public static string SerializerJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        #endregion

        #region Format

        /// <summary>
        ///     格式化字符串
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        ///     反转字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Reverse(this string s)
        {
            var input = s.ToCharArray();
            var output = new char[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                output[input.Length - 1 - i] = input[i];
            }

            return output.ToString();
        }

        /// <summary>
        ///     将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。
        /// </summary>
        /// <param name="s">复合格式字符串。</param>
        /// <param name="objs">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns></returns>
        public static string Format(this string s, params object[] objs)
        {
            return string.Format(s, objs);
        }

        #endregion

        #region Regex

        /// <summary>
        ///     字符串是否匹配
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        ///     获取匹配的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string s, string pattern)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            return Regex.Match(s, pattern).Value;
        }

        /// <summary>
        ///     快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="value">需验证的字符串。</param>
        /// <param name="express">正则表达式的内容。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(this string value, string express)
        {
            var myRegex = new Regex(express);
            return value.Length != 0 && myRegex.IsMatch(value);
        }

        /// <summary>
        ///     普通的域名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCommonDomain(this string value)
        {
            return value.ToLower().QuickValidate("^(www.)?(\\w+\\.){1,3}(org|org.cn|gov.cn|com|cn|net|cc)$");
        }

        /// <summary>
        ///     检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// </summary>
        /// <param name="value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumeric(this string value)
        {
            return value.QuickValidate("^[-]?[1-9]*[0-9]*$");
        }

        /// <summary>
        ///     检查一个字符串是否是纯字母和数字构成的，一般用于查询字符串参数的有效性验证。
        /// </summary>
        /// <param name="value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsLetterOrNumber(this string value)
        {
            return value.QuickValidate("^[a-zA-Z0-9_]*$");
        }

        /// <summary>
        ///     判断是否是数字，包括小数和整数。
        /// </summary>
        /// <param name="value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumber(this string value)
        {
            return value.QuickValidate("^(0|([1-9]+[0-9]*))(.[0-9]+)?$");
        }

        /// <summary>
        ///     判断一个字符串是否为邮编
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsZipCode(this string value)
        {
            return value.QuickValidate("^([0-9]{6})$");
        }

        /// <summary>
        ///     判断一个字符串是否为邮件
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(this string value)
        {
            var regex = new Regex(@"^\w+([-+.]\w+)*@(\w+([-.]\w+)*\.)+([a-zA-Z]+)+$", RegexOptions.IgnoreCase);
            return regex.Match(value).Success;
        }

        /// <summary>
        ///     判断一个字符串是否为ID格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIdCard(this string value)
        {
            Regex regex;
            string[] strArray;
            if ((value.Length != 15) && (value.Length != 0x12))
            {
                return false;
            }
            if (value.Length == 15)
            {
                regex = new Regex(@"^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$");
                if (!regex.Match(value).Success)
                {
                    return false;
                }
                strArray = regex.Split(value);
                try
                {
                    DateTime dt;
                    return DateTime.TryParse("20" + strArray[2] + "-" + strArray[3] + "-" + strArray[4], out dt);
                }
                catch
                {
                    return false;
                }
            }
            regex = new Regex(@"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9Xx])$");
            if (!regex.Match(value).Success)
            {
                return false;
            }
            strArray = regex.Split(value);
            try
            {
                DateTime dt;
                return DateTime.TryParse(strArray[2] + "-" + strArray[3] + "-" + strArray[4], out dt);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     判断是不是纯中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsChinese(this string value)
        {
            var regex = new Regex(@"^[\u4E00-\u9FA5\uF900-\uFA2D]+$", RegexOptions.IgnoreCase);
            return regex.Match(value).Success;
        }

        /// <summary>
        ///     判断一个字符串是否为手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhone(this string value)
        {
            var regex = new Regex(@"^(13|15)\d{9}$", RegexOptions.IgnoreCase);
            return regex.Match(value).Success;
        }

        /// <summary>
        ///     判断一个字符串是否为电话号码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTelphone(this string value)
        {
            var regex = new Regex(@"^(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{3,5})?$", RegexOptions.IgnoreCase);
            return regex.Match(value).Success;
        }

        /// <summary>
        ///     判断一个字符串是否为网址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUrl(this string value)
        {
            var regex = new Regex(@"(http://)?([\w-]+\.)*[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
            return regex.Match(value).Success;
        }

        /// <summary>
        ///     判断一个字符串是否为IP地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIp(this string value)
        {
            var regex =
                new Regex(
                    @"^(((2[0-4]{1}[0-9]{1})|(25[0-5]{1}))|(1[0-9]{2})|([1-9]{1}[0-9]{1})|([0-9]{1})).(((2[0-4]{1}[0-9]{1})|(25[0-5]{1}))|(1[0-9]{2})|([1-9]{1}[0-9]{1})|([0-9]{1})).(((2[0-4]{1}[0-9]{1})|(25[0-5]{1}))|(1[0-9]{2})|([1-9]{1}[0-9]{1})|([0-9]{1})).(((2[0-4]{1}[0-9]{1})|(25[0-5]{1}))|(1[0-9]{2})|([1-9]{1}[0-9]{1})|([0-9]{1}))$",
                    RegexOptions.IgnoreCase);
            return regex.Match(value).Success;
        }

        /// <summary>
        ///     判断一个字符串是否为字母加数字
        ///     Regex("[a-zA-Z0-9]?"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWordAndNum(this string value)
        {
            var regex = new Regex("[a-zA-Z0-9]?");
            return regex.Match(value).Success;
        }

        #endregion

        #region Convert

        /// <summary>
        ///     判断字符串是否是Int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

        /// <summary>
        ///     把字符串转换为整形
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        /// <summary>
        ///     是否是DateTime时间
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string s)
        {
            DateTime t;
            return DateTime.TryParse(s, out t);
        }

        /// <summary>
        ///     转换为DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string s)
        {
            return DateTime.Parse(s);
        }

        /// <summary>
        ///     转换指定格式的时间
        /// </summary>
        /// <param name="s"></param>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string s, string format, IFormatProvider provider)
        {
            return DateTime.ParseExact(s, format, provider);
        }

        /// <summary>
        ///     首字母小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamel(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s[0].ToString().ToLower() + s.Substring(1);
        }

        /// <summary>
        ///     首字母大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPascal(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s[0].ToString().ToUpper() + s.Substring(1);
        }

        /// <summary>
        ///     把string转换为Byte数组
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>Byte数组</returns>
        public static byte[] ToByteArray(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        /// <summary>
        ///     把byte数组转换为string
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>string</returns>
        public static string ToString(this byte[] data)
        {
            return Encoding.ASCII.GetString(data);
        }

        #endregion

        #region 隐藏敏感信息

        /// <summary>
        ///     隐藏敏感信息
        /// </summary>
        /// <param name="info">信息实体</param>
        /// <param name="left">左边保留的字符数</param>
        /// <param name="right">右边保留的字符数</param>
        /// <param name="basedOnLeft">
        ///     当长度异常时，是否显示左边
        ///     <code>true</code>显示左边，<code>false</code>显示右边
        /// </param>
        /// <returns></returns>
        public static string HideSensitiveInfo(this string info, int left, int right, bool basedOnLeft = true)
        {
            if (string.IsNullOrEmpty(info))
            {
                throw new ArgumentNullException(info);
            }
            var sbText = new StringBuilder();
            var hiddenCharCount = info.Length - left - right;
            if (hiddenCharCount > 0)
            {
                string prefix = info.Substring(0, left), suffix = info.Substring(info.Length - right);
                sbText.Append(prefix);
                for (var i = 0; i < hiddenCharCount; i++)
                {
                    sbText.Append("*");
                }
                sbText.Append(suffix);
            }
            else
            {
                if (basedOnLeft)
                {
                    if (info.Length > left && left > 0)
                    {
                        sbText.Append(info.Substring(0, left) + "****");
                    }
                    else
                    {
                        sbText.Append(info.Substring(0, 1) + "****");
                    }
                }
                else
                {
                    if (info.Length > right && right > 0)
                    {
                        sbText.Append("****" + info.Substring(info.Length - right));
                    }
                    else
                    {
                        sbText.Append("****" + info.Substring(info.Length - 1));
                    }
                }
            }
            return sbText.ToString();
        }

        /// <summary>
        ///     隐藏敏感信息
        /// </summary>
        /// <param name="info">信息实体</param>
        /// <param name="left">左边保留的字符数</param>
        /// <param name="right">右边保留的字符数</param>
        /// <param name="basedOnLeft">当长度异常时，是否显示左边</param>
        /// <code>true</code>
        /// 显示左边，
        /// <code>false</code>
        /// 显示右边
        /// <returns></returns>
        public static string HideSensitiveInfo1(this string info, int left, int right, bool basedOnLeft = true)
        {
            if (string.IsNullOrEmpty(info))
            {
                throw new ArgumentNullException(info);
            }
            var sbText = new StringBuilder();
            var hiddenCharCount = info.Length - left - right;
            if (hiddenCharCount > 0)
            {
                string prefix = info.Substring(0, left), suffix = info.Substring(info.Length - right);
                sbText.Append(prefix);
                sbText.Append("****");
                sbText.Append(suffix);
            }
            else
            {
                if (basedOnLeft)
                {
                    if (info.Length > left && left > 0)
                    {
                        sbText.Append(info.Substring(0, left) + "****");
                    }
                    else
                    {
                        sbText.Append(info.Substring(0, 1) + "****");
                    }
                }
                else
                {
                    if (info.Length > right && right > 0)
                    {
                        sbText.Append("****" + info.Substring(info.Length - right));
                    }
                    else
                    {
                        sbText.Append("****" + info.Substring(info.Length - 1));
                    }
                }
            }
            return sbText.ToString();
        }

        /// <summary>
        ///     隐藏敏感信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="sublen">信息总长与左子串（或右子串）的比例</param>
        /// <param name="basedOnLeft" />
        /// 当长度异常时，是否显示左边，默认true，默认显示左边
        /// <code>true</code>
        /// 显示左边，
        /// <code>false</code>
        /// 显示右边
        /// <returns></returns>
        public static string HideSensitiveInfo(this string info, int sublen = 3, bool basedOnLeft = true)
        {
            if (string.IsNullOrEmpty(info))
            {
                throw new ArgumentNullException(info);
            }
            if (sublen <= 1)
            {
                sublen = 3;
            }
            var subLength = info.Length/sublen;
            if (subLength > 0 && info.Length > subLength*2)
            {
                string prefix = info.Substring(0, subLength), suffix = info.Substring(info.Length - subLength);
                return prefix + "****" + suffix;
            }
            if (basedOnLeft)
            {
                var prefix = subLength > 0 ? info.Substring(0, subLength) : info.Substring(0, 1);
                return prefix + "****";
            }
            var suffixs = subLength > 0 ? info.Substring(info.Length - subLength) : info.Substring(info.Length - 1);
            return "****" + suffixs;
        }

        /// <summary>
        ///     隐藏右键详情
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="left">邮件头保留字符个数，默认值设置为3</param>
        /// <returns></returns>
        public static string HideEmailDetails(this string email, int left = 3)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(email);
            }
            if (!Regex.IsMatch(email, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                return HideSensitiveInfo(email);
            var suffixLen = email.Length - email.LastIndexOf('@');
            return HideSensitiveInfo(email, left, suffixLen, false);
        }

        #endregion

        #region ConfigurationManager

        /// <summary>
        ///     获取 Configuration 值
        /// </summary>
        /// <param name="key">Configuration key值</param>
        /// <returns></returns>
        public static string GetConfiguration(this string key)
        {
            if (key.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(key));
            }

            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        /// <summary>
        ///     获取 AppSetting 值
        /// </summary>
        /// <param name="key">AppSetting key值</param>
        /// <returns></returns>
        public static string GetAppSetting(this string key)
        {
            if (key.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(key));
            }

            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        ///     获取配置文件Section节点值
        /// </summary>
        /// <typeparam name="T">Section节点类型</typeparam>
        /// <param name="sectionName">Section节点名称</param>
        /// <returns></returns>
        public static T GetSection<T>(this string sectionName) where T : ConfigurationSection
        {
            return (T) ConfigurationManager.GetSection(sectionName);
        }

        #endregion

        #region 截取扩展

        /// <summary>确保字符串以指定的另一字符串开始，不区分大小写</summary>
        /// <param name="str">字符串</param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static string EnsureStart(this string str, string start)
        {
            if (string.IsNullOrEmpty(start)) return str;
            if (string.IsNullOrEmpty(str)) return start;

            if (str.StartsWith(start, StringComparison.OrdinalIgnoreCase)) return str;

            return start + str;
        }

        /// <summary>确保字符串以指定的另一字符串结束，不区分大小写</summary>
        /// <param name="str">字符串</param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string EnsureEnd(this string str, string end)
        {
            if (string.IsNullOrEmpty(end)) return str;
            if (string.IsNullOrEmpty(str)) return end;

            if (str.EndsWith(end, StringComparison.OrdinalIgnoreCase)) return str;

            return str + end;
        }

        /// <summary>从当前字符串开头移除另一字符串，不区分大小写，循环多次匹配前缀</summary>
        /// <param name="str">当前字符串</param>
        /// <param name="starts">另一字符串</param>
        /// <returns></returns>
        public static string TrimStart(this string str, params string[] starts)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (starts == null || starts.Length < 1 || string.IsNullOrEmpty(starts[0])) return str;

            for (var i = 0; i < starts.Length; i++)
            {
                if (str.StartsWith(starts[i], StringComparison.OrdinalIgnoreCase))
                {
                    str = str.Substring(starts[i].Length);
                    if (string.IsNullOrEmpty(str)) break;

                    // 从头开始
                    i = -1;
                }
            }
            return str;
        }

        /// <summary>从当前字符串结尾移除另一字符串，不区分大小写，循环多次匹配后缀</summary>
        /// <param name="str">当前字符串</param>
        /// <param name="ends">另一字符串</param>
        /// <returns></returns>
        public static string TrimEnd(this string str, params string[] ends)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (ends == null || ends.Length < 1 || string.IsNullOrEmpty(ends[0])) return str;

            for (var i = 0; i < ends.Length; i++)
            {
                if (str.EndsWith(ends[i], StringComparison.OrdinalIgnoreCase))
                {
                    str = str.Substring(0, str.Length - ends[i].Length);
                    if (string.IsNullOrEmpty(str)) break;

                    // 从头开始
                    i = -1;
                }
            }
            return str;
        }

        /// <summary>从字符串中检索子字符串，在指定头部字符串之后，指定尾部字符串之前</summary>
        /// <remarks>常用于截取xml某一个元素等操作</remarks>
        /// <param name="str">目标字符串</param>
        /// <param name="after">头部字符串，在它之后</param>
        /// <param name="before">尾部字符串，在它之前</param>
        /// <param name="startIndex">搜索的开始位置</param>
        /// <param name="positions">位置数组，两个元素分别记录头尾位置</param>
        /// <returns></returns>
        public static string Substring(this string str, string after, string before = null, int startIndex = 0,
            int[] positions = null)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (string.IsNullOrEmpty(after) && string.IsNullOrEmpty(before)) return str;

            /*
             * 1，只有start，从该字符串之后部分
             * 2，只有end，从开头到该字符串之前
             * 3，同时start和end，取中间部分
             */

            var p = -1;
            if (!string.IsNullOrEmpty(after))
            {
                p = str.IndexOf(after, startIndex);
                if (p < 0) return null;
                p += after.Length;

                // 记录位置
                if (positions != null && positions.Length > 0) positions[0] = p;
            }

            if (string.IsNullOrEmpty(before)) return str.Substring(p);

            var f = str.IndexOf(before, p >= 0 ? p : startIndex);
            if (f < 0) return null;

            // 记录位置
            if (positions != null && positions.Length > 1) positions[1] = f;

            if (p >= 0)
                return str.Substring(p, f - p);
            return str.Substring(0, f);
        }

        /// <summary>根据最大长度截取字符串，并允许以指定空白填充末尾</summary>
        /// <param name="str">字符串</param>
        /// <param name="maxLength">截取后字符串的最大允许长度，包含后面填充</param>
        /// <param name="pad">需要填充在后面的字符串，比如几个圆点</param>
        /// <returns></returns>
        public static string Cut(this string str, int maxLength, string pad = null)
        {
            if (string.IsNullOrEmpty(str) || maxLength <= 0 || str.Length < maxLength) return str;

            // 计算截取长度
            var len = maxLength;
            if (!string.IsNullOrEmpty(pad)) len -= pad.Length;
            if (len <= 0) return pad;

            return str.Substring(0, len) + pad;
        }

        ///// <summary>根据最大长度截取字符串（二进制计算长度），并允许以指定空白填充末尾</summary>
        ///// <remarks>默认采用Default编码进行处理，其它编码请参考本函数代码另外实现</remarks>
        ///// <param name="str">字符串</param>
        ///// <param name="maxLength">截取后字符串的最大允许长度，包含后面填充</param>
        ///// <param name="pad">需要填充在后面的字符串，比如几个圆点</param>
        ///// <param name="strict">严格模式时，遇到截断位置位于一个字符中间时，忽略该字符，否则包括该字符。默认true</param>
        ///// <returns></returns>
        //public static String CutBinary(this String str, Int32 maxLength, String pad = null, Boolean strict = true)
        //{
        //    if (String.IsNullOrEmpty(str) || maxLength <= 0 || str.Length < maxLength) return str;

        //    var encoding = Encoding.Default;

        //    var buf = encoding.GetBytes(str);
        //    if (buf.Length < maxLength) return str;

        //    // 计算截取字节长度
        //    var len = maxLength;
        //    if (!String.IsNullOrEmpty(pad)) len -= encoding.GetByteCount(pad);
        //    if (len <= 0) return pad;

        //    // 计算截取字符长度。避免把一个字符劈开
        //    var clen = 0;
        //    while (true)
        //    {
        //        try
        //        {
        //            clen = encoding.GetCharCount(buf, 0, len);
        //            break;
        //        }
        //        catch (DecoderFallbackException)
        //        {
        //            // 发生了回退，减少len再试
        //            len--;
        //        }
        //    }
        //    // 可能过长，修正
        //    if (strict) while (encoding.GetByteCount(str.ToCharArray(), 0, clen) > len) clen--;

        //    return str.Substring(0, clen) + pad;
        //}

        /// <summary>从当前字符串开头移除另一字符串以及之前的部分</summary>
        /// <param name="str">当前字符串</param>
        /// <param name="starts">另一字符串</param>
        /// <returns></returns>
        public static string CutStart(this string str, params string[] starts)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (starts == null || starts.Length < 1 || string.IsNullOrEmpty(starts[0])) return str;

            for (var i = 0; i < starts.Length; i++)
            {
                var p = str.IndexOf(starts[i]);
                if (p >= 0)
                {
                    str = str.Substring(p + starts[i].Length);
                    if (string.IsNullOrEmpty(str)) break;
                }
            }
            return str;
        }

        /// <summary>从当前字符串结尾移除另一字符串以及之后的部分</summary>
        /// <param name="str">当前字符串</param>
        /// <param name="ends">另一字符串</param>
        /// <returns></returns>
        public static string CutEnd(this string str, params string[] ends)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (ends == null || ends.Length < 1 || string.IsNullOrEmpty(ends[0])) return str;

            for (var i = 0; i < ends.Length; i++)
            {
                var p = str.LastIndexOf(ends[i]);
                if (p >= 0)
                {
                    str = str.Substring(0, p);
                    if (string.IsNullOrEmpty(str)) break;
                }
            }
            return str;
        }

        #endregion

        #region LD编辑距离算法

        /// <summary>编辑距离搜索，从词组中找到最接近关键字的若干匹配项</summary>
        /// <remarks>
        ///     算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="key">关键字</param>
        /// <param name="words">词组</param>
        /// <returns></returns>
        public static string[] LevenshteinSearch(string key, string[] words)
        {
            if (IsNullOrWhiteSpace(key)) return new string[0];

            var keys = key.Split(new[] {' ', '　'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in keys)
            {
                var maxDist = (item.Length - 1)/2;

                var q = from str in words
                    where item.Length <= str.Length
                          && Enumerable.Range(0, maxDist + 1)
                              .Any(dist =>
                              {
                                  return Enumerable.Range(0, Math.Max(str.Length - item.Length - dist + 1, 0))
                                      .Any(
                                          f =>
                                          {
                                              return LevenshteinDistance(item, str.Substring(f, item.Length + dist)) <=
                                                     maxDist;
                                          });
                              })
                    orderby str
                    select str;
                words = q.ToArray();
            }

            return words;
        }

        /// <summary>编辑距离</summary>
        /// <remarks>
        ///     又称Levenshtein距离（也叫做Edit Distance），是指两个字串之间，由一个转成另一个所需的最少编辑操作次数。
        ///     许可的编辑操作包括将一个字符替换成另一个字符，插入一个字符，删除一个字符。
        ///     算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int LevenshteinDistance(string str1, string str2)
        {
            var n = str1.Length;
            var m = str2.Length;
            var C = new int[n + 1, m + 1];
            int i, j, x, y, z;
            for (i = 0; i <= n; i++)
                C[i, 0] = i;
            for (i = 1; i <= m; i++)
                C[0, i] = i;
            for (i = 0; i < n; i++)
                for (j = 0; j < m; j++)
                {
                    x = C[i, j + 1] + 1;
                    y = C[i + 1, j] + 1;
                    if (str1[i] == str2[j])
                        z = C[i, j];
                    else
                        z = C[i, j] + 1;
                    C[i + 1, j + 1] = Math.Min(Math.Min(x, y), z);
                }
            return C[n, m];
        }

        #endregion

        #region LCS算法

        /// <summary>最长公共子序列搜索，从词组中找到最接近关键字的若干匹配项</summary>
        /// <remarks>
        ///     算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="key"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string[] LCSSearch(string key, string[] words)
        {
            if (IsNullOrWhiteSpace(key) || words == null || words.Length == 0) return new string[0];

            var keys = key
                .Split(new[] {' ', '\u3000'}, StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(s => s.Length)
                .ToArray();

            //var q = from sentence in items.AsParallel()
            var q = from word in words
                let MLL = LCSDistance(word, keys)
                where MLL >= 0
                orderby (MLL + 0.5)/word.Length, word
                select word;

            return q.ToArray();
        }

        /// <summary>
        ///     最长公共子序列问题是寻找两个或多个已知数列最长的子序列。
        ///     一个数列 S，如果分别是两个或多个已知数列的子序列，且是所有符合此条件序列中最长的，则 S 称为已知序列的最长公共子序列。
        ///     The longest common subsequence (LCS) problem is to find the longest subsequence common to all sequences in a set of
        ///     sequences (often just two). Note that subsequence is different from a substring, see substring vs. subsequence. It
        ///     is a classic computer science problem, the basis of diff (a file comparison program that outputs the differences
        ///     between two files), and has applications in bioinformatics.
        /// </summary>
        /// <remarks>
        ///     算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="word"></param>
        /// <param name="keys">多个关键字。长度必须大于0，必须按照字符串长度升序排列。</param>
        /// <returns></returns>
        public static int LCSDistance(string word, string[] keys)
        {
            var sLength = word.Length;
            var result = sLength;
            var flags = new bool[sLength];
            var C = new int[sLength + 1, keys[keys.Length - 1].Length + 1];
            //int[,] C = new int[sLength + 1, words.Select(s => s.Length).Max() + 1];
            foreach (var key in keys)
            {
                var wLength = key.Length;
                int first = 0, last = 0;
                int i = 0, j = 0, LCS_L;
                //foreach 速度会有所提升，还可以加剪枝
                for (i = 0; i < sLength; i++)
                    for (j = 0; j < wLength; j++)
                        if (word[i] == key[j])
                        {
                            C[i + 1, j + 1] = C[i, j] + 1;
                            if (first < C[i, j])
                            {
                                last = i;
                                first = C[i, j];
                            }
                        }
                        else
                            C[i + 1, j + 1] = Math.Max(C[i, j + 1], C[i + 1, j]);

                LCS_L = C[i, j];
                if (LCS_L <= wLength >> 1)
                    return -1;

                while (i > 0 && j > 0)
                {
                    if (C[i - 1, j - 1] + 1 == C[i, j])
                    {
                        i--;
                        j--;
                        if (!flags[i])
                        {
                            flags[i] = true;
                            result--;
                        }
                        first = i;
                    }
                    else if (C[i - 1, j] == C[i, j])
                        i--;
                    else // if (C[i, j - 1] == C[i, j])
                        j--;
                }

                if (LCS_L <= (last - first + 1) >> 1)
                    return -1;
            }

            return result;
        }

        #endregion
    }
}