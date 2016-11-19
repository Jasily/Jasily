using System;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily.Threading
{
    public static class ReaderWriterLockSlimExtensions
    {
        public static Releaser<ReaderWriterLockSlim> Read([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterReadLock();
            return GetReleaserForRead(locker);
        }

        public static Releaser<ReaderWriterLockSlim> Write([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterWriteLock();
            return GetReleaserForWrite(locker);
        }

        public static Releaser<ReaderWriterLockSlim> TryRead([NotNull] this ReaderWriterLockSlim locker,
            int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(millisecondsTimeout)
                ? GetReleaserForRead(locker)
                : new Releaser<ReaderWriterLockSlim>();
        }

        public static Releaser<ReaderWriterLockSlim> TryRead([NotNull] this ReaderWriterLockSlim locker,
            TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(timeout) ? GetReleaserForRead(locker) : new Releaser<ReaderWriterLockSlim>();
        }

        public static Releaser<ReaderWriterLockSlim> TryWrite([NotNull] this ReaderWriterLockSlim locker,
            int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(millisecondsTimeout)
                ? GetReleaserForWrite(locker)
                : new Releaser<ReaderWriterLockSlim>();
        }

        public static Releaser<ReaderWriterLockSlim> TryWrite([NotNull] this ReaderWriterLockSlim locker,
            TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(timeout)
                ? GetReleaserForWrite(locker)
                : new Releaser<ReaderWriterLockSlim>();
        }

        private static Releaser<ReaderWriterLockSlim> GetReleaserForRead(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = new Releaser<ReaderWriterLockSlim>(true, locker);
            releaser.ReleaseRaised += ExitReadLockHandler;
            return releaser;
        }

        private static Releaser<ReaderWriterLockSlim> GetReleaserForWrite(ReaderWriterLockSlim locker)
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