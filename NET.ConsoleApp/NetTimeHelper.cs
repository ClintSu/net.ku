using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NET.ConsoleApp
{
    public static class NetTimeHelper
    {

        public static DateTime NetTime
        {
            get { return DateTime.Parse(GetWebsiteDateTime()); }
        }

        /// <summary>
        /// 获取网络时间
        /// </summary>
        /// <returns></returns>
        public static string GetWebsiteDateTime()
        {
            string netTime = string.Empty;
            List<string> webList = new List<string>
            {
                "http://www.beijing-time.org/",
                "https://www.baidu.com/",
                "https://www.taobao.com/",
                "http://www.qq.com/",
                "https://www.360.cn/"
                
            };
            foreach (var url in webList)
            {
                netTime= GetWebsiteDateTime(url);
                if (netTime != null)
                {
                    break; 
                }
            }
            return netTime;
        }
        /// <summary>
        /// 获取网络时间
        /// </summary>
        /// <param name="webUrl">网站地址</param>
        /// tg:https://www.baidu.com/
        /// <returns></returns>
        public static string GetWebsiteDateTime(string webUrl)
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            string netTime = string.Empty;
            try
            {
                request = WebRequest.Create(webUrl);
                request.Timeout = 1000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse) request.GetResponse();
                headerCollection = response.Headers;
                foreach (var hd in headerCollection.AllKeys)
                {
                    if (hd.ToLower() == "date")
                    {
                        netTime = headerCollection[hd];
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                request?.Abort();
                response?.Close();
                headerCollection?.Clear();
            }
            return netTime;
        }
    }
}
