
namespace NET.Utils;
public class TimestampHelper
{
    /// <summary>
    /// 获取时间戳Timestamp
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long GetTimestamp(DateTime dateTime)
    {
        DateTime dt = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
        return (dateTime.Ticks - dt.Ticks) / 10000;  //除10000调整为13位      
    }

    /// <summary>
    /// 时间戳Timestamp转换日期
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    /// 

    public static DateTime GetDateTime(long timestamp)
    {
        DateTime dt = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
        long ts = timestamp * 10000;
        TimeSpan toNow = new TimeSpan(ts);
        return dt.Add(toNow);
    }

    /// <summary>
    /// 时间戳Timestamp转换日期
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime GetDateTime(string timestamp)
    {
        DateTime dt = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
        long ts = long.Parse(timestamp + "0000");
        TimeSpan toNow = new TimeSpan(ts);
        return dt.Add(toNow);
    }

}
