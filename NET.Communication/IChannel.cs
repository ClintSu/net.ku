using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Communication
{
    public interface IChannel
    {
        /// <summary>
        /// 优先级
        /// </summary>
        int Priority { get;}
        /// <summary>
        /// 通信类型名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Read<T>();
        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Write<T>();
    }
}
