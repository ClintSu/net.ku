using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    ///     object扩展集合
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     比较对象是否在某些对象集合中
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool In(this object obj, params object[] args)
        {
            return args.Any(i => i.Equals(obj));
        }

        /// <summary>
        ///     比较对象是否在某些对象集合中
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool In(this object obj, IEnumerable enumerable)
        {
            return enumerable.Cast<object>().Contains(obj);
        }

        /// <summary>
        ///     强制转换为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">要转换的对象</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object obj)
        {
            return (T) obj;
        }

        /// <summary>
        ///     获取 Object 类型的DescriptionAttribute标注的内容信息
        /// </summary>
        /// <param name="this">Object属性本身</param>
        /// <returns>标注内容</returns>
        /// <summary>
        ///     测试对象是否有指定的Attribute标注
        /// </summary>
        /// <param name="this">Object属性本身</param>
        /// <param name="attributeType">要测试的Attribute标注类型</param>
        /// <returns></returns>
        public static bool IsAttribute(this object @this, Type attributeType)
        {
            return @this.GetType().GetCustomAttributes(attributeType, false).FirstOrDefault() == null;
        }


        /// </summary>
        /// 获取对象指定 Properity 的 DescriptionAttribute 标注值
        /// <summary>
        /// <param name="propertyName">Properity名称</param>
        /// <returns></returns>
        public static string GetDescription(this object @this, string propertyName)
        {
            var p = @this.GetType().GetProperty(propertyName);
            var des = (DescriptionAttribute)p?.GetCustomAttribute(typeof(DescriptionAttribute), false);
            return des?.Description;
        }
        /// <summary>
        /// 获取 Object 类型的DescriptionAttribute标注的内容信息
        /// </summary>
        /// <param name="this">Object属性本身</param>
        /// <returns>标注内容</returns>
        public static string GetDescription(this Type @this)
        {
            var attribute = @this.GetCustomAttribute(typeof(DescriptionAttribute));
            return ((DescriptionAttribute)attribute)?.Description;
        }
    }
}