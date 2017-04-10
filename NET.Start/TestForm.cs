using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NET.Security;

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
          // MessageBox.Show(NET.Utility.ChineseConverter.ToPinYin("小李子。"," "));

            string privateKey = string.Empty;
            string publicKey = string.Empty;

            NET.Security.Rsa rsa=new Rsa();
            rsa.RsaKey(out privateKey,out publicKey);
        }
    }
}
