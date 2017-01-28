using System;
using Jasily.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily
{
    public class KeySelector
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
    }
}