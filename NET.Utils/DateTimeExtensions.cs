
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utils
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///DateTime?转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToString(this DateTime? obj,string format)
        {
            if(obj == null) return string.Empty;
            return ((DateTime)obj).ToString(format);
        }

        /// <summary>
        /// DateTime?转日期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DateTime? obj)
        {
            if (obj == null) return DateTime.MinValue;
            return (DateTime)obj;
        }

        /// <summary>
        /// 当月最近一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 当月最后一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime LastDayOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// 之前最后一个星期几
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfweek"></param>
        /// <returns></returns>
        public static DateTime LastDayOfWeek(this DateTime date, DayOfWeek dayOfweek)
        {
            int delta = -7;
            DateTime targetDate;
            do
            {
                targetDate = date.AddDays(delta);
                delta++;
            } while (targetDate.DayOfWeek != dayOfweek);
            return targetDate;
        }

        /// <summary>
        /// 之后最近一个星期几
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfweek"></param>
        /// <returns></returns>
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek dayOfweek)
        {
            int delta = 7;
            DateTime targetDate;
            do
            {
                targetDate = date.AddDays(delta);
                delta--;
            } while (targetDate.DayOfWeek != dayOfweek);
            return targetDate;
        }

        /// <summary>
        /// 当月最后一个星期几
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfweek"></param>
        /// <returns></returns>
        public static DateTime LastDayOfWeekOfTheMonth(this DateTime date, DayOfWeek dayOfweek)
        {
            DateTime lastDayOfTheMonth = date.LastDayOfTheMonth();
            if (lastDayOfTheMonth.DayOfWeek == dayOfweek)
            {
                return lastDayOfTheMonth;
            }
            return lastDayOfTheMonth.LastDayOfWeek(dayOfweek);
        }

        /// <summary>
        /// 当月最近一个星期几
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfweek"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeekOfTheMonth(this DateTime date, DayOfWeek dayOfweek)
        {
            DateTime firstDayOfTheMonth = date.FirstDayOfTheMonth();
            if (firstDayOfTheMonth.DayOfWeek == dayOfweek)
            {
                return firstDayOfTheMonth;
            }
            return firstDayOfTheMonth.NextDayOfWeek(dayOfweek);
        }

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="date">基础日期</param>
        /// <param name="hour">时</param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime date, int hour)
        {
            return date.SetTime(hour, 0, 0, 0);
        }

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="date">基础日期</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime date, int hour, int minute)
        {
            return date.SetTime(hour, minute, 0, 0);
        }

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="date">基础日期</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second)
        {
            return date.SetTime(hour, minute, second, 0);
        }

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="date">基础日期</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// 日期差
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DateDiff GetDiff(this DateTime fromDate, DateTime toDate)
        {
            return toDate >= fromDate ? new DateDiff(fromDate, toDate) : new DateDiff(toDate, fromDate);
        }
    }
}
