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

        public Releaser()
             : this(false)
        {
        }

        public Releaser(bool isAcquired, T state = default(T))
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

        public Releaser<T> AcquiredCallback([NotNull] Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (this.IsAcquired) action(this.state);
            return this;
        }

        public void Dispose()
        {
            if (this.IsAcquired && this.disposed == 0 && Interlocked.CompareExchange(ref this.disposed, 1, 0) == 0)
            {
                this.ReleaseRaised?.Invoke(this, this.state);
            }
        }
    }
}