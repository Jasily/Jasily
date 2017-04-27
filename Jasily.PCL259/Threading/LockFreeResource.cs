using System;
using System.Threading;

namespace Jasily.Threading
{
    public class LockFreeResource
    {
        private readonly int maxCount;
        private int currentCount;

        public int CurrentCount => this.currentCount;

        public LockFreeResource()
            : this(1, 1)
        {
        }

        public LockFreeResource(int initialCount)
            : this(initialCount, initialCount)
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

        public Releaser<int> Acquire(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            while (true)
            {
                var current = this.currentCount;
                if (count > current) return new Releaser<int>();
                if (Interlocked.CompareExchange(ref this.currentCount, current - count, current) == current)
                {
                    var locker = new Releaser<int>(true, count);
                    locker.ReleaseRaised += this.Locker_ReleaseRaised;
                    return locker;
                }
            }
        }

        private void Locker_ReleaseRaised(Releaser<int> sender, int e)
        {
            sender.ReleaseRaised -= this.Locker_ReleaseRaised;
            this.Release(e);
        }

        public Releaser<int> AcquireOrThrow(int count = 1)
        {
            var releaser = this.Acquire(count);
            if (!releaser.IsAcquired) throw new InvalidOperationException();
            return releaser;
        }
    }
}