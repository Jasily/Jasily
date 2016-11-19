using System;
using JetBrains.Annotations;

namespace Jasily
{
    public struct Releaser<T> : IDisposable
    {
        public event TypedEventHandler<Releaser<T>, T> ReleaseRaised;
        private readonly T state;

        private void Release() => this.ReleaseRaised?.Invoke(this, this.state);

        public Releaser(bool isAcquired, T state = default(T))
        {
            ReleaseRaised = null;
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

        public void Dispose() => this.Release();
    }
}