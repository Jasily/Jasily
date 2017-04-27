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

        public static PropertySelector<TProperty> SelectProperty<T, TProperty>([CanBeNull] this T obj,
            [NotNull] Expression<Func<T, TProperty>> selector)
            => PropertySelector<T>.Root.Select(selector);

        public static PropertySelector<TProperty> SelectProperty<T, TProperty>(
            [NotNull] Expression<Func<T, TProperty>> selector)
            => PropertySelector<T>.Root.Select(selector);

        public static PropertySelector<TProperty> SelectProperty<T, TProperty>([CanBeNull] this T obj,
            [NotNull] Expression<Func<T, IEnumerable<TProperty>>> selector)
            => PropertySelector<T>.Root.Select(selector);

        public static PropertySelector<TProperty> SelectProperty<T, TProperty>(
            [NotNull] Expression<Func<T, IEnumerable<TProperty>>> selector)
            => PropertySelector<T>.Root.Select(selector);

        #endregion
    }
}