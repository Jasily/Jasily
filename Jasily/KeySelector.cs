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

        /// <summary>
        /// provide a easy way to create <see cref="IEqualityComparer{T}"/> and <see cref="IComparer{T}"/> by single member for object.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static KeySelectorComparer<TItem, TKey> CreateComparer<TItem, TKey>([NotNull] Func<TItem, TKey> keySelector)
        {
            return new KeySelectorComparer<TItem, TKey>(keySelector);
        }

        public static KeySelectorComparer<TItem, TKey> CreateComparer<TItem, TKey>([NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer)
        {
            return new KeySelectorComparer<TItem, TKey>(keySelector, comparer, EqualityComparer<TKey>.Default);
        }

        public static KeySelectorComparer<TItem, TKey> CreateComparer<TItem, TKey>([NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IEqualityComparer<TKey> equalityComparer)
        {
            return new KeySelectorComparer<TItem, TKey>(keySelector, Comparer<TKey>.Default, equalityComparer);
        }

        public static KeySelectorComparer<TItem, TKey> CreateComparer<TItem, TKey>([NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer, [NotNull] IEqualityComparer<TKey> equalityComparer)
        {
            return new KeySelectorComparer<TItem, TKey>(keySelector, comparer, equalityComparer);
        }

        public static KeySelectorComparer<T, string> CreateComparer<T>([NotNull] Func<T, string> keySelector, [NotNull] StringComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            return new KeySelectorComparer<T, string>(keySelector, comparer, comparer);
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