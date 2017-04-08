using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily
{
    public struct CachedHashCode<T> : IEquatable<CachedHashCode<T>>, IEquatable<T>
    {
        public CachedHashCode(T value) : this(EqualityComparer<T>.Default, value)
        {
        }

        public CachedHashCode([NotNull] IEqualityComparer<T> comparer, T value)
        {
            this.Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            this.Value = value;
            this.HashCode = comparer.GetHashCode(value);
        }

        public IEqualityComparer<T> Comparer { get; }

        public int HashCode { get; }

        public T Value { get; }

        [Pure]
        public bool Equals(CachedHashCode<T> other)
        {
            if (this.Comparer == null) throw new ArgumentNullException(nameof(this.Comparer));
            if (other.Comparer != this.Comparer) return false;
            return this.HashCode == other.HashCode && this.Comparer.Equals(this.Value, other.Value);
        }

        [Pure]
        public bool Equals(T value)
        {
            if (this.Comparer == null) throw new ArgumentNullException(nameof(this.Comparer));
            return this.Equals(value, this.Comparer.GetHashCode(value));
        }

        [Pure]
        internal bool Equals(T value, int hashCode)
        {
            Debug.Assert(this.Comparer != null);
            return hashCode == this.HashCode && this.Comparer.Equals(value, this.Value);
        }
    }
}