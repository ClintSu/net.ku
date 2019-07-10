using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TestMac
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            button.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            MacList = new ObservableCollection<mac>();
            ClassifyList = new ObservableCollection<mac>();
            this.mac.ItemsSource = MacList;
            this.classify.ItemsSource = ClassifyList;
        }
        private ObservableCollection<mac> macList = new ObservableCollection<mac>();
        public ObservableCollection<mac> MacList
        {
            get { return macList; }
            set
            {
                macList = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("MacList"));
                }
            }
        }

        private ObservableCollection<mac> classifyList = new ObservableCollection<mac>();
        public ObservableCollection<mac> ClassifyList
        {
            get { return classifyList; }
            set
            {
                classifyList = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ClassifyList"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (button.Content.ToString() == "开始")
            {
                button.Content = "停止";
                button.Background = new SolidColorBrush(Color.FromRgb(240, 10, 10));
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
                dispatcherTimer.Start();
                text.Text = "测试中...";
            }
            else
            {
                button.Content = "开始";
                button.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                dispatcherTimer.Stop();
                text.Text = "已停止...";
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var pmac = GetMacByNetworkInterface();
            var mac = new mac() { Name = pmac };

            MacList.Add(mac);

            if (ClassifyList.Any(x => x.Name == pmac))
            {
                return;
            }
            ClassifyList.Add(mac);

            if (ClassifyList.Count >= 2)
            {
                text.Text = "出现不同Mac：" + ClassifyList.Count;
            }
        }
        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static string GetMacByNetworkInterface()
        {
            string macs = "";
            try
            {
                string mac;
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    mac = ni.GetPhysicalAddress().ToString();
                    if (mac.Length == 12)
                        macs += (!string.IsNullOrEmpty(macs) ? "," : "") + mac;
                }
                return macs;
            }
            catch (Exception ex)
            {

            }
            return macs;
        }
        /// <summary>
        /// 获得本机真实物理网卡MAC
        /// </summary>
        /// <returns></returns>
        private string GetPhysicsNetworkMac()
        {
            string macs = "";
            List<string> macList = new List<string>();
            NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in fNetworkInterfaces)
            {
                string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                if (rk != null)
                {
                    // 区分 PnpInstanceID   // 如果前面有 PCI 就是本机的真实网卡 
                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI")
                    {
                        macList.Add(adapter.GetPhysicalAddress().ToString());
                    }
                }
            }
            foreach (var mac in StringSort.Sort(macList))
            {
                macs += (!string.IsNullOrEmpty(macs) ? "," : "") + mac;
            }
            return macs;
        }


        protected override void OnClosed(EventArgs e)
        {
            dispatcherTimer.Stop();
            base.OnClosed(e);
        }

    }
    public class mac : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged; 
    }
}
