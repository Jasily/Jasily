using System;
using System.Diagnostics;
using System.Threading;

namespace Jasily.Threading
{
    public class LockFreeResource
    {
        private readonly int maxCount;
        private int currentCount;

        public int CurrentCount => this.currentCount;

        public LockFreeResource() : this(1, 1)
        {
        }

        public LockFreeResource(int initialCount) : this(initialCount, initialCount)
        {

        }

        public LockFreeResource(int initialCount, int maxCount)
        {
            if (initialCount < 0 || maxCount == 0 || initialCount > maxCount) throw new ArgumentOutOfRangeException();
            this.currentCount = initialCount;
            this.maxCount = maxCount;
        }

        public int Release(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            while (true)
            {
                var current = this.currentCount;
                if (current + count > this.maxCount) throw new InvalidOperationException();
                if (Interlocked.CompareExchange(ref this.currentCount, current + count, current) == current)
                    return current;
            }
        }

        public ReentrantReleaser<AcquireResult> Acquire(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            while (true)
            {
                var current = this.currentCount;
                if (count > current) return Releaser.CreateReentrantReleaser(AcquireResult.Default, true);
                if (Interlocked.CompareExchange(ref this.currentCount, current - count, current) == current)
                {
                    var releaser = Releaser.CreateReentrantReleaser(AcquireResult.Create(count), true);
                    releaser.ReleaseRaised += this.Locker_ReleaseRaised;
                    return releaser;
                }
            }
        }

        private void Locker_ReleaseRaised(ReentrantReleaser<AcquireResult> sender, AcquireResult e)
        {
            sender.ReleaseRaised -= this.Locker_ReleaseRaised;
            Debug.Assert(e.Count > 0);
            this.Release(e.Count);
        }

        public ReentrantReleaser<AcquireResult> AcquireOrThrow(int count = 1)
        {
            var releaser = this.Acquire(count);
            if (releaser.Value.Count == 0) throw new InvalidOperationException();
            return releaser;
        }

        public struct AcquireResult
        {
            internal static readonly AcquireResult Default = default (AcquireResult);

            internal static AcquireResult Create(int count) => new AcquireResult(count);

            internal AcquireResult(int count)
            {
                this.Count = count;
            }

            /// <summary>
            /// 
            /// </summary>
            public int Count { get; }
        }
    }
}