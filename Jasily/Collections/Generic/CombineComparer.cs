using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach

namespace Jasily.Collections.Generic
{
    public class CombineComparer<T> : IComparer<T>, IEqualityComparer<T>
    {
        private readonly IComparer<T>[] _comparers;
        private readonly IEqualityComparer<T>[] _equalityComparers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparers"></param>
        /// <exception cref="ArgumentNullException"><paramref name="comparers"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="comparers"/> is empty.</exception>
        public CombineComparer([NotNull] params IComparer<T>[] comparers)
            : this(comparers, new IEqualityComparer<T>[] { EqualityComparer<T>.Default })
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equalityComparers"></param>
        /// <exception cref="ArgumentNullException"><paramref name="equalityComparers"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="equalityComparers"/> is empty.</exception>
        public CombineComparer([NotNull] params IEqualityComparer<T>[] equalityComparers)
            : this(new IComparer<T>[] { Comparer<T>.Default }, equalityComparers)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparers"></param>
        /// <param name="equalityComparers"></param>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="ArgumentException">one of arguments is empty.</exception>
        public CombineComparer([NotNull] IEnumerable<IComparer<T>> comparers, [NotNull] IEnumerable<IEqualityComparer<T>> equalityComparers)
        {
            if (comparers == null) throw new ArgumentNullException(nameof(comparers));
            if (equalityComparers == null) throw new ArgumentNullException(nameof(equalityComparers));

            this._comparers = comparers.ToArray();
            if (this._comparers.Length == 0) throw new ArgumentException();
            this._equalityComparers = equalityComparers.ToArray();
            if (this._equalityComparers.Length == 0) throw new ArgumentException();
        }

        public bool Equals(T x, T y)
        {
            for (var i = 0; i < this._equalityComparers.Length; i++)
            {
                if (!this._equalityComparers[i].Equals(x, y)) return false;
            }
            return true;
        }

        public int GetHashCode(T obj)
        {
            var sum = 0;
            for (var i = 0; i < this._equalityComparers.Length; i++)
            {
                sum ^= this._equalityComparers[i].GetHashCode(obj);
            }
            return sum;
        }

        public int Compare(T x, T y)
        {
            for (var i = 0; i < this._comparers.Length; i++)
            {
                var r = this._comparers[i].Compare(x, y);
                if (r != 0) return r;
            }
            return 0;
        }
    }
}