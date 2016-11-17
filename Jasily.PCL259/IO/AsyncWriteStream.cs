using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.IO
{
    public sealed class AsyncWriteStream : WrapStream
    {
        private readonly int bufferSize;

        public AsyncWriteStream([NotNull] Stream baseStream, int bufferSize)
            : base(baseStream)
        {
            this.bufferSize = bufferSize;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (count > this.bufferSize)
            {
                await this.BaseStream.WriteAsync(buffer, offset, count, cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            throw new NotImplementedException();
        }
    }
}