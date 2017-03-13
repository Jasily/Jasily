using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Collections.Generic
{
    public class KeySelectorComparer<TItem, TKey> : IComparer<TItem>, IEqualityComparer<TItem>
    {
        private readonly Func<TItem, TKey> keySelector;
        private readonly IComparer<TKey> comparer;
        private readonly IEqualityComparer<TKey> equalityComparer; 

        public KeySelectorComparer([NotNull] Func<TItem, TKey> keySelector)
            : this(keySelector, null, null)
        {
        }

        protected KeySelectorComparer([NotNull] Func<TItem, TKey> keySelector,
            [CanBeNull] IComparer<TKey> comparer, [CanBeNull] IEqualityComparer<TKey> equalityComparer)
        {
            this.keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            this.comparer = comparer ?? Comparer<TKey>.Default;
            this.equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
        }

        public int Compare(TItem x, TItem y)
            => this.comparer.Compare(this.keySelector(x), this.keySelector(y));

        public bool Equals(TItem x, TItem y)
            => this.equalityComparer.Equals(this.keySelector(x), this.keySelector(y));

        public int GetHashCode(TItem obj)
            => this.equalityComparer.GetHashCode(this.keySelector(obj));
    }
}