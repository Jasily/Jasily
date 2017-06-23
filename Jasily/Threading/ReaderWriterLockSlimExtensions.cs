using System;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locker"></param>
        /// <returns></returns>
        public static ReentrantReleaser<ReaderWriterLockSlim> AcquireReadLock([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterReadLock();
            return GetReadReleaser(locker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static ReentrantReleaser<ReaderWriterLockSlim> TryAcquireReadLock([NotNull] this ReaderWriterLockSlim locker,
            int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(millisecondsTimeout)
                ? GetReadReleaser(locker)
                : Releaser.CreateReentrantReleaser(locker, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ReentrantReleaser<ReaderWriterLockSlim> TryAcquireReadLock([NotNull] this ReaderWriterLockSlim locker,
            TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(timeout) ? GetReadReleaser(locker) : Releaser.CreateReentrantReleaser(locker, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locker"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ReentrantReleaser<ReaderWriterLockSlim> AcquireWriteLock([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterWriteLock();
            return GetWriteReleaser(locker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static ReentrantReleaser<ReaderWriterLockSlim> TryAcquireWriteLock([NotNull] this ReaderWriterLockSlim locker,
            int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(millisecondsTimeout)
                ? GetWriteReleaser(locker)
                : Releaser.CreateReentrantReleaser(locker, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ReentrantReleaser<ReaderWriterLockSlim> TryAcquireWriteLock([NotNull] this ReaderWriterLockSlim locker,
            TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(timeout)
                ? GetWriteReleaser(locker)
                : Releaser.CreateReentrantReleaser(locker, true);
        }

        private static ReentrantReleaser<ReaderWriterLockSlim> GetReadReleaser(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = Releaser.CreateReentrantReleaser(locker);
            releaser.ReleaseRaised += ExitReadLockHandler;
            return releaser;
        }

        private static ReentrantReleaser<ReaderWriterLockSlim> GetWriteReleaser(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = Releaser.CreateReentrantReleaser(locker);
            releaser.ReleaseRaised += ExitWriteLockHandler;
            return releaser;
        }

        private static void ExitReadLockHandler(ReentrantReleaser<ReaderWriterLockSlim> releaser, ReaderWriterLockSlim locker)
        {
            releaser.ReleaseRaised -= ExitReadLockHandler;
            locker.ExitReadLock();
        }

        private static void ExitWriteLockHandler(ReentrantReleaser<ReaderWriterLockSlim> releaser, ReaderWriterLockSlim locker)
        {
            releaser.ReleaseRaised -= ExitWriteLockHandler;
            locker.ExitWriteLock();
        }
    }
}