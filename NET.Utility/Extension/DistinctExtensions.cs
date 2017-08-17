using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    ///     Distinct扩展集合
    /// </summary>
    public static class DistinctExtensions
    {
        /// <summary>
        ///     Distinct扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector,
            IEqualityComparer<TV> comparer = default(EqualityComparer<TV>))
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector, comparer));
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private readonly IEqualityComparer<TV> _comparer;
        private readonly Func<T, TV> _keySelector;

        public CommonEqualityComparer(Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            _keySelector = keySelector;
            _comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, TV> keySelector)
            : this(keySelector, EqualityComparer<TV>.Default)
        {
        }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}