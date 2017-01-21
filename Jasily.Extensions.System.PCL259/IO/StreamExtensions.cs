using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.IO
{
    public static class StreamExtensions
    {
        public const int DefaultCopyBufferSize = 81920;

        #region to array

        private static MemoryStream InitToArray([NotNull] this Stream stream, bool readFromStart)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var capacity = -1;
            try
            {
                if (stream.CanSeek)
                {
                    capacity = Convert.ToInt32(stream.Length);
                }
            }
            catch (NotSupportedException) { }

            if (readFromStart && stream.Position != 0)
            {
                stream.Position = 0;
            }

            return capacity != -1 ? new MemoryStream(capacity) : new MemoryStream();
        }

        public static byte[] ToArray([NotNull] this Stream stream, bool readFromStart = false)
        {
            using (var ms = InitToArray(stream, readFromStart))
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static byte[] ToArray([NotNull] this Stream stream, CancellationToken cancellationToken, bool readFromStart = false)
        {
            using (var ms = InitToArray(stream, readFromStart))
            {
                stream.CopyTo(ms, cancellationToken);
                return ms.ToArray();
            }
        }

        public static async Task<byte[]> ToArrayAsync([NotNull] this Stream stream, bool readFromStart = false)
        {
            using (var ms = InitToArray(stream, readFromStart))
            {
                if (stream.CanSeek && stream.Position != 0) stream.Position = 0;
                await stream.CopyToAsync(ms).ConfigureAwait(false);
                return ms.ToArray();
            }
        }

        public static async Task<byte[]> ToArrayAsync(this Stream stream, CancellationToken cancellationToken, bool readFromStart = false)
        {
            using (var ms = InitToArray(stream, readFromStart))
            {
                if (stream.CanSeek && stream.Position != 0) stream.Position = 0;
                await stream.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
                return ms.ToArray();
            }
        }

        #endregion

        #region copy to

        public static void CopyTo([NotNull] this Stream stream, [NotNull] Stream destination, CancellationToken cancellationToken,
            int bufferSize = DefaultCopyBufferSize)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            var buffer = new byte[bufferSize];
            int read;
            cancellationToken.ThrowIfCancellationRequested();
            while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                destination.Write(buffer, 0, read);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public static Task CopyToAsync([NotNull] this Stream stream, Stream destination, CancellationToken cancellationToken)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            return stream.CopyToAsync(destination, DefaultCopyBufferSize, cancellationToken);
        }

        public static async Task CopyToAsync([NotNull] this Stream stream, [NotNull] Stream destination,
            [NotNull] IObserver<long> observer, int bufferSize = DefaultCopyBufferSize)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");

            long total = 0;
            var buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                total += bytesRead;
                observer.OnNext(total);
            }
            observer.OnCompleted();
        }

        #endregion

        #region write

        public static void Write([NotNull] this Stream stream, [NotNull] byte[] buffer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            stream.Write(buffer, 0, buffer.Length);
        }

        public static Task WriteAsync([NotNull] this Stream stream, [NotNull] byte[] buffer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public static void Write([NotNull] this Stream stream, ArraySegment<byte> buffer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            buffer.ThrowIfDefault();
            stream.Write(buffer.Array, buffer.Offset, buffer.Count);
        }

        public static Task WriteAsync([NotNull] this Stream stream, ArraySegment<byte> buffer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            buffer.ThrowIfDefault();
            return stream.WriteAsync(buffer.Array, buffer.Offset, buffer.Count);
        }

        #endregion

        #region read

        public static IEnumerable<byte[]> ReadAsEnumerable([NotNull] this Stream stream, int maxCount, bool reuseBuffer = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (maxCount < 1) throw new ArgumentOutOfRangeException(nameof(maxCount));

            var buffer = new byte[maxCount];
            int readed;
            while ((readed = stream.Read(buffer, 0, maxCount)) != 0)
            {
                yield return readed != maxCount ? buffer.ToArray() : reuseBuffer ? buffer : buffer.ToArray();
            }
        }

        #endregion
    }
}