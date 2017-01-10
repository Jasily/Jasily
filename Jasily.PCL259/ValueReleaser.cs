using System;
using JetBrains.Annotations;

namespace Jasily
{
    /// <summary>
    /// For value typed ValueReleaser, be careful reentrant ReleaseRaised event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ValueReleaser<T> : IDisposable
    {
        public event TypedEventHandler<ValueReleaser<T>, T> ReleaseRaised;
        private readonly T state;

        public ValueReleaser(bool isAcquired, T state = default(T))
        {
            ReleaseRaised = null;
            this.IsAcquired = isAcquired;
            this.state = state;
        }

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
            if (this.IsAcquired) action(this.state);
            return this;
        }

        public void Dispose()
        {
            if (this.IsAcquired) this.ReleaseRaised?.Invoke(this, this.state);
        }
    }
}