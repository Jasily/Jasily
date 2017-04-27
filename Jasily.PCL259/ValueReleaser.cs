using System;
using JetBrains.Annotations;

namespace Jasily
{
    /// <summary>
    /// If EventHandler is NOT reentrant, use <see cref="Threading.Releaser{T}"/> instead.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ValueReleaser<T> : IDisposable
    {
        public event TypedEventHandler<ValueReleaser<T>, T> ReleaseRaised;

        public ValueReleaser(bool isAcquired, T acquiredObject = default(T))
        {
            ReleaseRaised = null;
            this.IsAcquired = isAcquired;
            this.AcquiredObject = acquiredObject;
        }

        public T AcquiredObject { get; }

        public bool IsAcquired { get; }

        public ValueReleaser<T> AcquiredCallback([NotNull] Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (this.IsAcquired) action();
            return this;
        }

        public ValueReleaser<T> AcquiredCallback([NotNull] Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (this.IsAcquired) action(this.AcquiredObject);
            return this;
        }

        public void Dispose()
        {
            if (this.IsAcquired) this.ReleaseRaised?.Invoke(this, this.AcquiredObject);
        }
    }
}