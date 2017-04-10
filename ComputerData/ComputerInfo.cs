using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;

namespace ComputerData
{
    public class ComputerInfo
    {
        public static string GetComputerName()
        {
            try
            {
                var environmentVariable = System.Environment.GetEnvironmentVariable("ComputerName");
                if (environmentVariable != null)
                    return environmentVariable.Trim();
                return "unknow";
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public static string GetCpuId()
        {
            try
            {
                //获取CPU序列号代码
                string st = "";//cpu序列号
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    st = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return st.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public static string GetDiskId()
        {
            try
            {
                //获取硬盘ID
                String st = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    st = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return st.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public static string GetIpAddress()
        {
            try
            {
                //获取IP地址
                IPHostEntry myEntry = Dns.GetHostEntry(Dns.GetHostName());
                var firstOrDefault = myEntry.AddressList.FirstOrDefault<IPAddress>(e => e.AddressFamily.ToString().Equals("InterNetwork"));
                if ( firstOrDefault !=  null)
                    return firstOrDefault.ToString().Trim();
                return "unknow";
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public static string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public static string GetMotherBoardId()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("WIN32_BaseBoard");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    st = (string)mo.Properties["SerialNumber"].Value;
                    break;
                }
                moc = null;
                mc = null;
                return st.Trim();
            }
            catch
            {
                return "unknow";
            }
        }

        public static string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
                return st.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public static string GetTotalPhysicalMemory()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                moc = null;
                mc = null;
                return st.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public static string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    st = mo["UserName"].ToString();
                }
                moc = null;
                mc = null;
                return st.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
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
            catch
            {

            }
            return macs;
        }
    }
}