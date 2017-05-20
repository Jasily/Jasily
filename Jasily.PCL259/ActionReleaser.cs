using System;
using JetBrains.Annotations;

namespace Jasily
{
    /// <summary>
    /// 
    /// </summary>
    public struct ActionReleaser : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public event TypedEventHandler<ActionReleaser> ReleaseRaised;

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => this.ReleaseRaised?.Invoke(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ActionReleaser<T> : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public event TypedEventHandler<ActionReleaser<T>, T> ReleaseRaised;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ActionReleaser([CanBeNull] T value)
        {
            ReleaseRaised = null;
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public T Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => this.ReleaseRaised?.Invoke(this, this.Value);
    }
}