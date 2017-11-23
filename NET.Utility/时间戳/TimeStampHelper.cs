using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    public class TimeStampHelper
    {
        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <param name="len">长度，默认13位</param>
        /// <returns></returns>
        public static string GetTimeStamp(int len = 13)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string gts= Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return gts.Substring(0, len);
        }

        /// <summary>
        /// 获取时间戳日期
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertDataTime(string timeStamp)
        {
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long time = long.Parse(timeStamp.PadRight(17,'0'));
            TimeSpan ts = new TimeSpan(time);
            return dt.Add(ts);
        }

        /// <summary>
        /// 获取以0点0分0秒开始的日期
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime GetStartDateTime(DateTime d)
        {
            if (d.Hour != 0)
            {
                var year = d.Year;
                var month = d.Month;
                var day = d.Day;
                var hour = "0";
                var minute = "0";
                var second = "0";
                d = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
            }
            return d;
        }

        /// <summary>
        /// 获取以23点59分59秒结束的日期
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime GetEndDateTime(DateTime d)
        {
            if (d.Hour != 23)
            {
                var year = d.Year;
                var month = d.Month;
                var day = d.Day;
                var hour = "23";
                var minute = "59";
                var second = "59";
                d = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
            }
            return d;
        }

        /// <summary>
        /// 格式化日期【2017-04-11 15:20:00】
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static string FormatDate(DateTime dt)
        {
           return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化日期【2017-04-11 15:20:00】
        /// </summary>
        /// <param name="strDate">字串</param>
        /// <returns></returns>
        public static DateTime FormatDate(string strDate)
        {
            if (!string.IsNullOrEmpty(strDate))
            {
                return Convert.ToDateTime(strDate);
            }
            return DateTime.Now;
        }
    }
}
