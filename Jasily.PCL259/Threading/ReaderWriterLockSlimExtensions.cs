using System;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily.Threading
{
    public static class ReaderWriterLockSlimExtensions
    {
        public static Releaser<ReaderWriterLockSlim> AcquireReadLock([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterReadLock();
            return GetReadReleaser(locker);
        }

        public static Releaser<ReaderWriterLockSlim> TryAcquireReadLock([NotNull] this ReaderWriterLockSlim locker,
            int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(millisecondsTimeout)
                ? GetReadReleaser(locker)
                : Releaser<ReaderWriterLockSlim>.Default;
        }

        public static Releaser<ReaderWriterLockSlim> TryAcquireReadLock([NotNull] this ReaderWriterLockSlim locker,
            TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(timeout) ? GetReadReleaser(locker) : Releaser<ReaderWriterLockSlim>.Default;
        }

        public static Releaser<ReaderWriterLockSlim> AcquireWriteLock([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterWriteLock();
            return GetWriteReleaser(locker);
        }

        public static Releaser<ReaderWriterLockSlim> TryAcquireWriteLock([NotNull] this ReaderWriterLockSlim locker,
            int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(millisecondsTimeout)
                ? GetWriteReleaser(locker)
                : Releaser<ReaderWriterLockSlim>.Default;
        }

        public static Releaser<ReaderWriterLockSlim> TryAcquireWriteLock([NotNull] this ReaderWriterLockSlim locker,
            TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(timeout)
                ? GetWriteReleaser(locker)
                : Releaser<ReaderWriterLockSlim>.Default;
        }

        private static Releaser<ReaderWriterLockSlim> GetReadReleaser(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = new Releaser<ReaderWriterLockSlim>(true, locker);
            releaser.ReleaseRaised += ExitReadLockHandler;
            return releaser;
        }

        private static Releaser<ReaderWriterLockSlim> GetWriteReleaser(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = new Releaser<ReaderWriterLockSlim>(true, locker);
            releaser.ReleaseRaised += ExitWriteLockHandler;
            return releaser;
        }

        private static void ExitReadLockHandler(Releaser<ReaderWriterLockSlim> releaser, ReaderWriterLockSlim locker)
        {
            releaser.ReleaseRaised -= ExitReadLockHandler;
            locker.ExitReadLock();
        }

        private static void ExitWriteLockHandler(Releaser<ReaderWriterLockSlim> releaser, ReaderWriterLockSlim locker)
        {
            releaser.ReleaseRaised -= ExitWriteLockHandler;
            locker.ExitWriteLock();
        }
    }
}