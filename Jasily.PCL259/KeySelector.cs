using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Jasily.Collections.Generic;
using Jasily.ComponentModel;
using JetBrains.Annotations;

namespace Jasily
{
    /// <summary>
    /// provide static methods.
    /// </summary>
    public static class KeySelector
    {
        #region comparer

        public static KeySelectorComparer<TItem, TKey> CreateComparer<TItem, TKey>([NotNull] Func<TItem, TKey> keySelector)
           => new KeySelectorComparer<TItem, TKey>(keySelector);

        public static KeySelectorComparer<T, string> CreateComparer<T>([NotNull] Func<T, string> keySelector,
            [NotNull] StringComparer comparer)
            => new StringKeySelectorComparer<T>(keySelector, comparer);

        private class StringKeySelectorComparer<T> : KeySelectorComparer<T, string>
        {
            public StringKeySelectorComparer([NotNull] Func<T, string> keySelector, [NotNull] StringComparer comparer)
                : base(keySelector, comparer, comparer)
            {
            }
        }

        #endregion

        #region property

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="_"></param>
        /// <param name="selector"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="selector"/> is null.</exception>
        /// <returns></returns>
        public static PropertySelector<TProperty> SelectProperty<T, TProperty>([CanBeNull] this T _,
            [NotNull] Expression<Func<T, TProperty>> selector)
            => PropertySelector<T>.Root.Select(selector);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="selector"/> is null.</exception>
        /// <returns></returns>
        public static PropertySelector<TProperty> SelectProperty<T, TProperty>(
            [NotNull] Expression<Func<T, TProperty>> selector)
            => PropertySelector<T>.Root.Select(selector);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="_"></param>
        /// <param name="selector"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="selector"/> is null.</exception>
        /// <returns></returns>
        public static PropertySelector<TProperty> SelectProperty<T, TProperty>([CanBeNull] this T _,
            [NotNull] Expression<Func<T, IEnumerable<TProperty>>> selector)
            => PropertySelector<T>.Root.SelectMany(selector);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="selector"/> is null.</exception>
        /// <returns></returns>
        public static PropertySelector<TProperty> SelectProperty<T, TProperty>(
            [NotNull] Expression<Func<T, IEnumerable<TProperty>>> selector)
            => PropertySelector<T>.Root.SelectMany(selector);

        #endregion
    }
}