using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utility
{
    public class KConverter
    {
        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="value">转换值</param>
        /// <param name="fromBase">输入进制类型</param>
        /// <param name="toBase">输出进制类型</param>
        /// <returns></returns>
        public static string ConvetString(string value,int fromBase,int toBase)
        {
            int intValue = Convert.ToInt32(value, fromBase);
            return Convert.ToString(intValue, toBase);
        }

        /// <summary>
        /// byte[]转string(16进制)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("X2"));
                }
                hexString = sb.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] ToHexBytes(string hexStr)
        {
            hexStr = hexStr.Replace(" ", "");
            if ((hexStr.Length % 2) != 0)
            {
                hexStr += "";
            }
            byte[] returnBytes = new byte[hexStr.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
