using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Iterator<TSource>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            var iteratorVariable = new Set<TSource>(comparer);
            foreach (var local in second)
            {
                iteratorVariable.Add(local);
            }
            foreach (var iteratorVariable1 in first)
            {
                if (!iteratorVariable.Add(iteratorVariable1))
                {
                    continue;
                }
                yield return iteratorVariable1;
            }
        }

        public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            var removingItemsDictionary = new Dictionary<T, int>();
            var _count = source.Count();
            var _items = new T[_count];
            var j = 0;
            foreach (var item in items)
            {
                if (!removingItemsDictionary.ContainsKey(item))
                {
                    removingItemsDictionary[item] = 1;
                }
            }
            for (var i = 0; i < _count; i++)
            {
                var current = source.ElementAt(i);
                if (!removingItemsDictionary.ContainsKey(current))
                {
                    _items[j++] = current;
                }
            }
            return _items;
        }
    }
}