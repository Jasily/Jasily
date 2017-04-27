using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Threading
{
    public static class SemaphoreSlimExtensions
    {
        private static Releaser<SemaphoreSlim> CreateReleaser(SemaphoreSlim semaphore)
        {
            var releaser = new Releaser<SemaphoreSlim>(true, semaphore);
            releaser.ReleaseRaised += Releaser_ReleaseRaised;
            return releaser;
        }

        private static void Releaser_ReleaseRaised(Releaser<SemaphoreSlim> sender, SemaphoreSlim e)
        {
            sender.ReleaseRaised -= Releaser_ReleaseRaised;
            Debug.Assert(e != null);
            e.Release();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            await semaphore.WaitAsync();
            return CreateReleaser(semaphore);
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            int millisecondsTimeout)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(millisecondsTimeout)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(millisecondsTimeout, cancellationToken)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            TimeSpan timeout)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(timeout)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(timeout, cancellationToken)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }
    }
}