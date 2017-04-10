using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    public class DesSecurity
    {
        private static string key = "netskycn";

        public static string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        private DesSecurity()
        {
        }

        public static string EncryptString(string encryptString, string key)
        {
            if (string.IsNullOrEmpty(encryptString))
                throw new ArgumentNullException("encryptString", "不能为空");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "不能为空");
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = bytes;
            return Convert.ToBase64String(EncryptBytes(Encoding.UTF8.GetBytes(encryptString), bytes, keyIV));
        }

        public static string EncryptString(string encryptString)
        {
            return EncryptString(encryptString, key);
        }

        public static byte[] EncryptBytes(byte[] sourceBytes, byte[] keyBytes, byte[] keyIV)
        {
            if (sourceBytes == null || keyBytes == null || keyIV == null)
                throw new ArgumentNullException("sourceBytes和keyBytes", "不能为空。");
            keyBytes = CheckByteArrayLength(keyBytes);
            keyIV = CheckByteArrayLength(keyIV);
            DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return array;
        }

        public static string DecryptString(string decryptString, string key)
        {
            if (string.IsNullOrEmpty(decryptString))
                throw new ArgumentNullException("decryptString", "不能为空");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "不能为空");
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = bytes;
            return Encoding.UTF8.GetString(DecryptBytes(Convert.FromBase64String(decryptString), bytes, keyIV));
        }

        public static string DecryptString(string decryptString)
        {
            return DecryptString(decryptString, key);
        }

        public static byte[] DecryptBytes(byte[] sourceBytes, byte[] keyBytes, byte[] keyIV)
        {
            if (sourceBytes == null || keyBytes == null || keyIV == null)
                throw new ArgumentNullException("soureBytes和keyBytes及keyIV", "不能为空。");
            keyBytes = CheckByteArrayLength(keyBytes);
            keyIV = CheckByteArrayLength(keyIV);
            DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return array;
        }

        private static byte[] CheckByteArrayLength(byte[] byteArray)
        {
            byte[] numArray = new byte[8];
            if (byteArray.Length < 8)
                return Encoding.UTF8.GetBytes("12345678");
            if (byteArray.Length % 8 == 0 && byteArray.Length <= 64)
                return byteArray;
            Array.Copy((Array)byteArray, 0, (Array)numArray, 0, 8);
            return numArray;
        }
    }
}
