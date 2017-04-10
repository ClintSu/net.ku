using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerData
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
            this.btnRead.Focus();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            var strInfo = string.Empty;

            strInfo += PadLeftEx("电脑名", 8, '.') + "|" + ComputerInfo.GetComputerName() + "\n";
            strInfo += PadLeftEx("用户名", 8, '.') + "|" + ComputerInfo.GetUserName() + "\n";
            strInfo += PadLeftEx("Ip地址", 8, '.') + "|" + ComputerInfo.GetIpAddress() + "\n";
            strInfo += PadLeftEx("Mac地址", 8, '.') + "|" + ComputerInfo.GetMacAddress() + "\n";
            strInfo += PadLeftEx("CPU编号", 8, '.') + "|" + ComputerInfo.GetCpuId() + "\n";
            strInfo += PadLeftEx("硬盘编号", 8, '.') + "|" + ComputerInfo.GetDiskId() + "\n";
            strInfo += PadLeftEx("主板编号", 8, '.') + "|" + ComputerInfo.GetMotherBoardId() + "\n";
            strInfo += PadLeftEx("系统类型", 8, '.') + "|" + ComputerInfo.GetSystemType() + "\n";
            strInfo += PadLeftEx("物理内存", 8, '.') + "|" + ComputerInfo.GetTotalPhysicalMemory() + "\n";
            strInfo += PadLeftEx("Mac列表", 8, '.') + "|" + ComputerInfo.GetMacByNetworkInterface() + "\n";

            var testMD5 = GetMD5Str(ComputerInfo.GetMacByNetworkInterface() + ComputerInfo.GetMotherBoardId());

            strInfo += @"Mac列表\主板的MD5:" + testMD5;

            this.rtbInfo.Text = strInfo;

            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start(); //  开始监视代码运行时间  

            ////var tt = GetMD5Str(ComputerInfo.GetMacAddress() + ComputerInfo.GetCpuId());

            ////ComputerInfo.GetCpuId();
            ////ComputerInfo.GetDiskId();
            //ComputerInfo.GetMotherBoardId();
            //ComputerInfo.GetMacByNetworkInterface();
            //stopwatch.Stop(); //  停止监视  
            //TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间  
            //double hours = timespan.TotalHours; // 总小时  
            //double minutes = timespan.TotalMinutes;  // 总分钟  
            //double seconds = timespan.TotalSeconds;  //  总秒数  
            //double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数 
        }

        public static string GetMD5Str(string toCryString)
        {
            MD5CryptoServiceProvider hashmd5;
            hashmd5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(toCryString))).Replace("-", "").ToUpper();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.rtbInfo.Text)) return;
            Clipboard.SetDataObject(rtbInfo.Text);
            MessageBox.Show(@"已复制到剪贴板！");
        }

        private string PadLeftEx(string str, int totalByteCount, char c)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount, c);
            return w;
        }

        private string PadRightEx(string str, int totalByteCount, char c)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount, c);
            return w;
        }
    }
}
