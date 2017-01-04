using System;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily
{
    public sealed class Releaser<T> : IDisposable
    {
        public event TypedEventHandler<Releaser<T>, T> ReleaseRaised;
        private readonly T state;
        private int disposed;

        private void Release() => this.ReleaseRaised?.Invoke(this, this.state);

        public Releaser(bool isAcquired = false, T state = default(T))
        {
            this.IsAcquired = isAcquired;
            this.state = state;
        }

        public bool IsAcquired { get; }

        public Releaser<T> AcquiredCallback([NotNull] Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (this.IsAcquired) action();
            return this;
        }

        public void Dispose()
        {
            if (this.disposed == 1 || Interlocked.CompareExchange(ref this.disposed, 1, 0) != 0)
                throw new ObjectDisposedException("Releaser");
            this.Release();
        }
    }

    public struct ValueReleaser<T> : IDisposable
    {
        public event TypedEventHandler<ValueReleaser<T>, T> ReleaseRaised;
        private readonly T state;

        private void Release() => this.ReleaseRaised?.Invoke(this, this.state);

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

        public void Dispose() => this.Release();
    }
}