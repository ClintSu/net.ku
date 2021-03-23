using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    public class MD5Helper
    {
        // 格式化md5 hash 字节数组所用的格式（两位小写16进制数字） 
        private static readonly string hexFormat = "x2";
        private MD5Helper() { }

        public static string MD5(string str)
        {
            return MD5(str, Encoding.UTF8);
        }

        public static string MD5(string str, Encoding encodeType)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var byteValue = encodeType.GetBytes(str);
            var hashValue = md5.ComputeHash(byteValue);
            md5.Clear();
            var sb = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                sb.Append(hashValue[i].ToString(hexFormat));
            }
            return sb.ToString();
        }
    }
}
