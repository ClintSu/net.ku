using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }
    }
}
