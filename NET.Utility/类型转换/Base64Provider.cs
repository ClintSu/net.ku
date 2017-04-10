using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    public class Base64Provider
    {
        private Base64Provider()
        {
        }
        public static string EncodeBase64String(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException("source", "不能为空。");
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }
        public static string DecodeBase64String(string result)
        {
            if (string.IsNullOrEmpty(result))
                throw new ArgumentNullException("result", "不能为空。");
            return Encoding.UTF8.GetString(Convert.FromBase64String(result));
        }
    }
}
