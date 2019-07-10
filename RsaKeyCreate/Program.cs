using System.IO;
using System.Security.Cryptography;

namespace RsaKeyCreate
{
    class Program
    {
        static void Main(string[] args)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            using (StreamWriter writer = new StreamWriter("PrivateKey.xml"))  //这个文件要保密...
            {

                writer.WriteLine(rsa.ToXmlString(true));

            }
            using (StreamWriter writer = new StreamWriter("PublicKey.xml"))
            {

                writer.WriteLine(rsa.ToXmlString(false));

            }
        }
    }
}
