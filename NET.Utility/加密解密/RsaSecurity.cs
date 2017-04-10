using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    public class RsaSecurity
    {
        #region RSA加解密
        /// <summary>
        /// RSA密钥产生
        /// </summary>
        /// <param name="xmlPrivateKeys">私有Key</param>
        /// <param name="xmlPublicKey">共有Key</param>
        public void RsaKey(out string xmlPrivateKeys, out string xmlPublicKey)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                xmlPrivateKeys = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="xmlPublickKey">公钥</param>
        /// <param name="content">加密内容(长度小于117)</param>
        /// <returns></returns>
        public string Encrypt(string xmlPublickKey, string content)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublickKey);
                byte[] cipherBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                return Convert.ToBase64String(cipherBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="content">解密内容</param>
        /// <returns></returns>
        public string Decrypt(string xmlPrivateKey, string content)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                byte[] cipherBytes = rsa.Decrypt(Convert.FromBase64String(content), false);
                return Encoding.UTF8.GetString(cipherBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取Hash描述
        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="HashData">Hash描述</param>
        /// <returns></returns>
        public bool GetHash(string strSource,ref byte[] hashData)
        {
            try
            {
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(strSource);
                hashData = MD5.ComputeHash(buffer);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="strHashData">Hash描述</param>
        /// <returns></returns>
        public bool GetHash(string strSource, ref string strHashData)
        {
            try
            {
                //从字符串中取得Hash描述 
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(strSource);
                byte[] hashData = MD5.ComputeHash(buffer);
                strHashData = Convert.ToBase64String(hashData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <param name="HashData">Hash描述</param>
        /// <returns></returns>
        public bool GetHash(FileStream objFile, ref byte[] hashData)
        {
            try
            {
                //从文件中取得Hash描述 
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                hashData = MD5.ComputeHash(objFile);
                objFile.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <param name="strHashData">Hash描述</param>
        /// <returns></returns>
        public bool GetHash(FileStream objFile, ref string strHashData)
        {
            try
            {
                //从文件中取得Hash描述 
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                byte[] hashData = MD5.ComputeHash(objFile);
                objFile.Close();
                strHashData = Convert.ToBase64String(hashData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RSA签名
        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strPrivateKey">私钥</param>
        /// <param name="HashByteSignature">待签名Hash描述</param>
        /// <param name="EncryptedSignatureData">签名结果</param>
        /// <returns></returns>
        public bool SignatureFormatter(string strPrivateKey,byte[] HashByteSignature,ref byte[] EncryptedSignatureData)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(strPrivateKey);
                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("MD5");
                EncryptedSignatureData = rsaFormatter.CreateSignature(HashByteSignature);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strPrivateKey">私钥</param>
        /// <param name="HashByteSignature">待签名Hash描述</param>
        /// <param name="EncryptedSignatureData">签名结果</param>
        /// <returns></returns>
        public bool SignatureFormatter(string strPrivateKey, string strHashbyteSignature, ref string strEncryptedSignatureData)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(strPrivateKey);
                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("MD5");
                var EncryptedSignatureData = rsaFormatter.CreateSignature(Convert.FromBase64String(strHashbyteSignature));
                strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RSA签名验证
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="HashbyteDeformatter">Hash描述</param>
        /// <param name="DeformatterData">签名后的结果</param>
        /// <returns></returns>
        public bool SignatureDeformatter(string strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(strKeyPublic);
                RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                RSADeformatter.SetHashAlgorithm("MD5");
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="strHashbyteDeformatter">Hash描述</param>
        /// <param name="strDeformatterData">签名后的结果</param>
        /// <returns></returns>
        public bool SignatureDeformatter(string strKeyPublic, string strHashbyteDeformatter, string strDeformatterData)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(strKeyPublic);
                RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                RSADeformatter.SetHashAlgorithm("MD5");
                if (RSADeformatter.VerifySignature(Convert.FromBase64String(strHashbyteDeformatter), Convert.FromBase64String(strDeformatterData)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
