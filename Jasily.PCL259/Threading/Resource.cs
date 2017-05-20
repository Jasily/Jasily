using System;
using System.Threading;

namespace Jasily.Threading
{
    public class Resource
    {
        private readonly int maxCount;
        private int currentCount;

        public int CurrentCount => this.currentCount;

        public Resource() : this(1, 1)
        {
        }

        public Resource(int initialCount) : this(initialCount, initialCount)
        {

        }

        public Resource(int initialCount, int maxCount)
        {
            if (initialCount < 0 || maxCount == 0 || initialCount > maxCount) throw new ArgumentOutOfRangeException();
            this.currentCount = initialCount;
            this.maxCount = maxCount;
        }

        public int Release(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            if (this.currentCount + count > this.maxCount) throw new SemaphoreFullException();
            this.currentCount += count;
            return this.currentCount - count;
        }

        public ReentrantReleaser<int> Acquire(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            if (count > this.currentCount) return Releaser.CreateReentrantReleaser(0, true);
            this.currentCount -= count;
            var locker = Releaser.CreateReentrantReleaser(count);
            locker.ReleaseRaised += this.Locker_ReleaseRaised;
            return locker;
        }

        private void Locker_ReleaseRaised(ReentrantReleaser<int> sender, int e)
        {
            sender.ReleaseRaised -= this.Locker_ReleaseRaised;
            this.Release(e);
        }
    }
}