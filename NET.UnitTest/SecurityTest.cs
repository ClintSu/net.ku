using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NET.UnitTest
{
    [TestClass]
    public class SecurityTest
    {
        [TestMethod]
        public void TestRsa()
        {
            string privateKey = string.Empty;
            string publicKey = string.Empty;
            string content = @"123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567";
            NET.Security.Rsa rsa= new NET.Security.Rsa();
            rsa.RsaKey(out privateKey,out publicKey); //生成密钥
            var encrypted = rsa.Encrypt(publicKey, content);  //值加密
            var decrypted = rsa.Decrypt(privateKey, encrypted); //值解密
            Assert.AreEqual(content, decrypted);

            string sPrivateKey = string.Empty;
            string sPublicKey = string.Empty;
            string hashContent = string.Empty;
            string hashContentSignature = string.Empty;
            rsa.RsaKey(out sPrivateKey,out sPublicKey); //生成密钥
            rsa.GetHash(content, ref hashContent);  //Hash值
            rsa.SignatureFormatter(sPrivateKey, hashContent, ref hashContentSignature);  //签名
            var result = rsa.SignatureDeformatter(sPublicKey, hashContent, hashContentSignature);  //验证签名
            Assert.AreEqual(result,true);

        }
    }
}
