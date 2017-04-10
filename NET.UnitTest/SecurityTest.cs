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
            rsa.RsaKey(out privateKey,out publicKey);
            var encrypted = rsa.Encrypt(publicKey, content);
            var decrypted = rsa.Decrypt(privateKey, encrypted);
            Assert.AreEqual(content, decrypted);

            string sprivateKey = string.Empty;
            string spublicKey = string.Empty;
            string hashContent = string.Empty;
            string hashContentSignature = string.Empty;
            rsa.RsaKey(out sprivateKey,out spublicKey);
            rsa.GetHash(content, ref hashContent);
            rsa.SignatureFormatter(sprivateKey, hashContent, ref hashContentSignature);
            var result = rsa.SignatureDeformatter(spublicKey, hashContent, hashContentSignature);
            Assert.AreEqual(result,true);

        }
    }
}
