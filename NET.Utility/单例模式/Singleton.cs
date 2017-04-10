using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NET.Utilities
{
    public class Singleton<T> where T : class, new()
    {
        private static T _Instance;

        public static T Instance
        {
            get
            {
                Interlocked.CompareExchange<T>(ref Singleton<T>._Instance, Activator.CreateInstance<T>(), default(T));
                return Singleton<T>._Instance;
            }
        }
    }
}
