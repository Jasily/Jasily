using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Collections.Generic
{
    public class KeySelectorComparer
    {
        public static KeySelectorComparer<TItem, TKey> Create<TItem, TKey>([NotNull] Func<TItem, TKey> keySelector)
            => new KeySelectorComparer<TItem, TKey>(keySelector);

        public static KeySelectorComparer<T, string> Create<T>([NotNull] Func<T, string> keySelector,
            [NotNull] StringComparer comparer)
            => new StringKeySelectorComparer<T>(keySelector, comparer);

        private class StringKeySelectorComparer<T> : KeySelectorComparer<T, string>
        {
            public StringKeySelectorComparer([NotNull] Func<T, string> keySelector, [NotNull] StringComparer comparer)
                : base(keySelector, comparer, comparer)
            {
            }
        }
    }

    public class KeySelectorComparer<TItem, TKey> : IComparer<TItem>, IEqualityComparer<TItem>
    {
        private readonly Func<TItem, TKey> keySelector;
        private readonly IComparer<TKey> comparer;
        private readonly IEqualityComparer<TKey> equalityComparer; 

        public KeySelectorComparer([NotNull] Func<TItem, TKey> keySelector)
            : this(keySelector, Comparer<TKey>.Default, EqualityComparer<TKey>.Default)
        {
        }

        protected KeySelectorComparer([NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer, [NotNull] IEqualityComparer<TKey> equalityComparer)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));
            this.keySelector = keySelector;
            this.comparer = comparer;
            this.equalityComparer = equalityComparer;
        }

        public int Compare(TItem x, TItem y)
            => this.comparer.Compare(this.keySelector(x), this.keySelector(y));

        public bool Equals(TItem x, TItem y)
            => this.equalityComparer.Equals(this.keySelector(x), this.keySelector(y));

        public int GetHashCode(TItem obj)
            => this.equalityComparer.GetHashCode(this.keySelector(obj));
    }
}