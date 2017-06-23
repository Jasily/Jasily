using System;
using System.Threading;

namespace Jasily.Threading
{
    /// <summary>
    /// The <see cref="ReentrantReleaser"/> is thread-saftly. it only raise `ReleaseRaised` once.
    /// </summary>
    public class ReentrantReleaser<T> : IDisposable
    {
        /// <summary>
        /// a event when 
        /// </summary>
        public event TypedEventHandler<ReentrantReleaser<T>, T> ReleaseRaised;
        /// <summary>
        /// the disponsed flag. 1 mean disponsed, 0 mean not disponsed.
        /// </summary>
        private int _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="neverRaise"></param>
        public ReentrantReleaser(T value, bool neverRaise = false)
        {
            this.Value = value;
            this._disposed = neverRaise ? 1 : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// raise <see cref="ReleaseRaised"/>.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed == 0 && Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
            {
                this.ReleaseRaised?.Invoke(this, this.Value);
            }
        }
    }

    /// <summary>
    /// The <see cref="ReentrantReleaser"/> is thread-saftly. it only raise `ReleaseRaised` once.
    /// </summary>
    public class ReentrantReleaser : IDisposable
    {
        /// <summary>
        /// a event when 
        /// </summary>
        public event TypedEventHandler<ReentrantReleaser> ReleaseRaised;
        /// <summary>
        /// the disponsed flag. 1 mean disponsed, 0 mean not disponsed.
        /// </summary>
        private int _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neverRaise"></param>
        public ReentrantReleaser(bool neverRaise = false)
        {
            this._disposed = neverRaise ? 1 : 0;
        }

        /// <summary>
        /// raise <see cref="ReleaseRaised"/>.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed == 0 && Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
            {
                this.ReleaseRaised?.Invoke(this);
            }
        }
    }
}