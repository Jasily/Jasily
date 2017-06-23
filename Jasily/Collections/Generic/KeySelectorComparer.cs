using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Collections.Generic
{
    /// <summary>
    /// compare object by selector.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class KeySelectorComparer<TItem, TKey> : IComparer<TItem>, IEqualityComparer<TItem>
    {
        private readonly Func<TItem, TKey> _keySelector;
        private readonly IComparer<TKey> _comparer;
        private readonly IEqualityComparer<TKey> _equalityComparer;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="keySelector"></param>
        /// <exception cref="ArgumentNullException">one of argument is null.</exception>
        public KeySelectorComparer([NotNull] Func<TItem, TKey> keySelector)
            : this(keySelector, Comparer<TKey>.Default, EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <param name="equalityComparer"></param>
        /// <exception cref="ArgumentNullException">one of argument is null.</exception>
        public KeySelectorComparer([NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer, [NotNull] IEqualityComparer<TKey> equalityComparer)
        {
            this._keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            this._comparer = comparer ?? throw new ArgumentNullException(nameof(keySelector));
            this._equalityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(TItem x, TItem y) => this._comparer.Compare(this._keySelector(x), this._keySelector(y));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(TItem x, TItem y) => this._equalityComparer.Equals(this._keySelector(x), this._keySelector(y));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(TItem obj) => this._equalityComparer.GetHashCode(this._keySelector(obj));
    }
}