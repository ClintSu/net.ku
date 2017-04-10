using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace NET.Utilities
{
    /// <summary>
    /// 应用程序缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        public static object GetCache(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey];
        }

        public static void SetCache(string cacheKey, object objObject)
        {
            HttpRuntime.Cache.Insert(cacheKey, objObject);
        }

        public static void SetCache(string cacheKey, object objObject, TimeSpan Timeout)
        {
            HttpRuntime.Cache.Insert(cacheKey, objObject, (CacheDependency)null, DateTime.MaxValue, Timeout, CacheItemPriority.NotRemovable, (CacheItemRemovedCallback)null);
        }

        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            HttpRuntime.Cache.Insert(cacheKey, objObject, (CacheDependency)null, absoluteExpiration, slidingExpiration);
        }

        public static void RemoveAllCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        public static void RemoveAllCache()
        {
            Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
                cache.Remove(enumerator.Key.ToString());
        }
    }
}
