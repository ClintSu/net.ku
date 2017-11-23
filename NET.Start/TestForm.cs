using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NET.Utilities;

namespace NET.Start
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          // MessageBox.Show(NET.Utilities.ChineseConverter.ToPinYin("小李子。"," "));

            string privateKey = string.Empty;
            string publicKey = string.Empty;

            NET.Utilities.RsaSecurity rsa=new NET.Utilities.RsaSecurity();
            rsa.RsaKey(out privateKey,out publicKey);

            string r1 = GetMD5Str("1");

            string s1 = rsa.Encrypt(publicKey, r1);

            string e1 = rsa.Decrypt(privateKey, s1);

            byte[] hashData = new byte[] { };
            rsa.GetHash("1", ref hashData);

            byte[] hashData2 = new byte[] { };
            rsa.GetHash("2", ref hashData2);

            byte[] eData = r1.GetBytes();
            var b1 = rsa.SignatureFormatter(privateKey, hashData, ref eData);
            var b2 = rsa.SignatureDeformatter(publicKey, hashData, eData);


        }

        public static string GetMD5Str(string toCryString)
        {
            MD5CryptoServiceProvider hashmd5;
            hashmd5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(toCryString))).Replace("-", "").ToUpper();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string privateKey = string.Empty;
            string publicKey = string.Empty;
            NET.Utilities.RsaSecurity rsa = new NET.Utilities.RsaSecurity();
            rsa.RsaKey(out privateKey, out publicKey);

            string r1 = GetMD5Str("1");
            string s1 = rsa.Encrypt(publicKey, r1);
            string e1 = rsa.Decrypt(privateKey, s1);

            NET.Utilities.AesSecurity aes = new NET.Utilities.AesSecurity(e1);
            //var tt = IOHelper.ReadFile(@"C:\Users\chao\Desktop\自定义文件格式研究\Docs\MP4\MP4_4.mp4");

            var tt = @"C:\Users\chao\Desktop\自定义文件格式研究\Docs\MP4\MP4_4.mp4";
            var rr = tt.ReadToBytes();

            string qz = "jgro" + "mp4".PadLeft(8, '.'); //jgro ,jgri //4+8 (类型+文件格式)
            byte[] typeBytes = qz.ToByteArray();

            string hashValue = string.Empty;
            rsa.GetFileHash(tt, ref hashValue);  //32(文件名称HashData)
            byte[] hashData = hashValue.ToByteArray();

            List<byte> byteList = new List<byte>();
            byteList.AddRange(typeBytes);
            byteList.AddRange(hashData);


            byteList.AddRange(rr);

            var kk= @"C:\Users\chao\Desktop\自定义文件格式研究\Docs\MP4\MP4_4.jgr";
            byteList.ToArray().SaveToFile(kk);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string privateKey = string.Empty;
            string publicKey = string.Empty;
            NET.Utilities.RsaSecurity rsa = new NET.Utilities.RsaSecurity();
            rsa.RsaKey(out privateKey, out publicKey);
            var tt = @"C:\Users\chao\Desktop\自定义文件格式研究\Docs\MP4\MP4_4.jgr";

            FileStream fileStream = new FileStream(tt, FileMode.Open);
            byte[] b1 = new byte[4];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[32];

            fileStream.Position = 0;
            fileStream.Read(b1, 0, 4);
            fileStream.Position = 4;
            fileStream.Read(b2, 0, 8);
            fileStream.Position = 12;
            fileStream.Read(b3, 0, 32);

            var k1 = Encoding.Default.GetString(b1);
            var k2 = Encoding.Default.GetString(b2).Replace('.',' ').Trim();
            var k3 = Encoding.Default.GetString(b3);

            string r1 = GetMD5Str("1");
            byte[] eData = r1.GetBytes();
            var t1 = rsa.SignatureFormatter(privateKey, b3, ref eData);
            var t2 = rsa.SignatureDeformatter(publicKey, b3, eData);

            fileStream.Close();
        }
    }
}
