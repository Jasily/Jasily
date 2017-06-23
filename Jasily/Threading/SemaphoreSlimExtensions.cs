using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public static class SemaphoreSlimExtensions
    {
        private static ReentrantReleaser<SemaphoreSlim> CreateReleaser(SemaphoreSlim semaphore)
        {
            var releaser = Releaser.CreateReentrantReleaser(semaphore);
            releaser.ReleaseRaised += Releaser_ReleaseRaised;
            return releaser;
        }

        private static void Releaser_ReleaseRaised(ReentrantReleaser<SemaphoreSlim> sender, SemaphoreSlim e)
        {
            sender.ReleaseRaised -= Releaser_ReleaseRaised;
            Debug.Assert(e != null);
            e.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semaphore"></param>
        /// <returns></returns>
        public static async Task<ReentrantReleaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            await semaphore.WaitAsync();
            return CreateReleaser(semaphore);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static async Task<ReentrantReleaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            int millisecondsTimeout)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(millisecondsTimeout)
                ? CreateReleaser(semaphore)
                : Releaser.CreateReentrantReleaser(semaphore, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<ReentrantReleaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(millisecondsTimeout, cancellationToken)
                ? CreateReleaser(semaphore)
                : Releaser.CreateReentrantReleaser(semaphore, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<ReentrantReleaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            TimeSpan timeout)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(timeout)
                ? CreateReleaser(semaphore)
                : Releaser.CreateReentrantReleaser(semaphore, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<ReentrantReleaser<SemaphoreSlim>> LockAsync([NotNull] this SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
            return await semaphore.WaitAsync(timeout, cancellationToken)
                ? CreateReleaser(semaphore)
                : Releaser.CreateReentrantReleaser(semaphore, true);
        }
    }
}