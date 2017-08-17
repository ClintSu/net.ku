using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace NET.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var netTime = NetTimeHelper.NetTime;

            if (DateTime.Now.ToString("yyyy-mm-dd HH:mm") != netTime.ToString("yyyy-mm-dd HH:mm"))
                LocalTimeSync.SyncTime(netTime);//同步服务器的时间 

            //ShowNetworkInterfaceMessage();
            //Console.WriteLine(GetPhysicsNetworkMac());
            //Console.ReadLine();
        }






        public static string GetPhysicsNetworkMac()
        {
            string macs = "";
            string mac;
            NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in fNetworkInterfaces)
            {
                string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                if (rk != null)
                {
                    // 区分 PnpInstanceID  
                    // 如果前面有 PCI 就是本机的真实网卡 
                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI")
                    {
                        //macs += adapter.GetPhysicalAddress().ToString();
                        mac = adapter.GetPhysicalAddress().ToString();
                        macs += (!string.IsNullOrEmpty(macs) ? "," : "") + mac;
                    }
                }
            }

            return macs;
        }


        public static void ShowNetworkInterfaceMessage()
        {
            NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in fNetworkInterfaces)
            {
                #region " 网卡类型 "
                string fCardType = "未知网卡";
                string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                if (rk != null)
                {
                    // 区分 PnpInstanceID  
                    // 如果前面有 PCI 就是本机的真实网卡 
                    // MediaSubType 为 01 则是常见网卡，02为无线网卡。 
                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                    if (fPnpInstanceID.Length > 3 &&
                        fPnpInstanceID.Substring(0, 3) == "PCI")
                        fCardType = "物理网卡";
                    else if (fMediaSubType == 1)
                        fCardType = "虚拟网卡";
                    else if (fMediaSubType == 2)
                        fCardType = "无线网卡";
                }
                #endregion
                #region 网卡信息
                System.Console.WriteLine("-----------------------------------------------------------");
                System.Console.WriteLine("-- " + fCardType);
                System.Console.WriteLine("-----------------------------------------------------------");
                System.Console.WriteLine("Id .................. : {0}", adapter.Id); // 获取网络适配器的标识符 
                System.Console.WriteLine("Name ................ : {0}", adapter.Name); // 获取网络适配器的名称 
                System.Console.WriteLine("Description ......... : {0}", adapter.Description); // 获取接口的描述 
                System.Console.WriteLine("Interface type ...... : {0}", adapter.NetworkInterfaceType); // 获取接口类型 
                System.Console.WriteLine("Is receive only...... : {0}", adapter.IsReceiveOnly); // 获取 Boolean 值，该值指示网络接口是否设置为仅接收数据包。 
                System.Console.WriteLine("Multicast............ : {0}", adapter.SupportsMulticast); // 获取 Boolean 值，该值指示是否启用网络接口以接收多路广播数据包。 
                System.Console.WriteLine("Speed ............... : {0}", adapter.Speed); // 网络接口的速度 
                System.Console.WriteLine("Physical Address .... : {0}", adapter.GetPhysicalAddress().ToString()); // MAC 地址 

                IPInterfaceProperties fIPInterfaceProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = fIPInterfaceProperties.UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                {
                    if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                        System.Console.WriteLine("Ip Address .......... : {0}", UnicastIPAddressInformation.Address); // Ip 地址 
                }
                System.Console.WriteLine();
                #endregion
            }
            System.Console.ReadKey();
        }

    }
}
