using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Jasily.IO.Internal;
using JetBrains.Annotations;

namespace Jasily.IO
{
    public sealed class AsyncWriteStream : WrapStream
    {
        private readonly int bufferSize;
        private readonly Queue<Message> messages = new Queue<Message>(); 
        private readonly Queue<ArraySegment<byte>> bufferPool = new Queue<ArraySegment<byte>>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(0);

        private int totalBufferUsed;
        private int disponseState;

        public AsyncWriteStream([NotNull] Stream baseStream, int bufferSize)
            : base(baseStream)
        {
            throw new NotImplementedException();
            this.bufferSize = bufferSize;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var tcs = new TaskCompletionSource<bool>();
            
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

        private void SendMessage(Message message)
        {
            Debug.Assert(message != Message.Buffer);
            if (this.disponseState == 1 && message != Message.Dispose)
            {
                throw new ObjectDisposedException("stream is Disposed.");
            }
            lock (this.messages)
            {
                this.messages.Enqueue(message);
            }
            this.semaphore.Release(1);
        }

        private Message WaitMessage()
        {
            this.semaphore.Wait();
            lock (this.messages)
            {
                var message = this.messages.Peek();
                if (message != Message.Dispose)
                {
                    this.messages.Dequeue();
                }
                return message;
            }
        }

        private void SendBuffer(ArraySegment<byte> buffer)
        {
            lock (this.messages)
            {
                this.messages.Enqueue(Message.Buffer);
            }
            lock (this.bufferPool)
            {
                this.totalBufferUsed += buffer.Count;
                this.bufferPool.Enqueue(buffer);
            }
            this.semaphore.Release(1);
        }

        private ArraySegment<byte> DequeueBuffer()
        {
            lock (this.bufferPool)
            {
                var buffer = this.bufferPool.Dequeue();
                this.totalBufferUsed -= buffer.Count;
                return buffer;
            }
        }

        public Task Start()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    var msg = this.WaitMessage();

                    if (msg == Message.Dispose)
                    {
                        break;
                    }

                    switch (msg)
                    {
                        case Message.Buffer:
                            this.BaseStream.Write(this.DequeueBuffer());
                            break;

                        case Message.Flush:
                            this.BaseStream.Flush();
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            });
        }

        public override void Flush() => this.SendMessage(Message.Flush);

        protected override void Dispose(bool disposing)
        {
            if (this.disponseState == 0)
            {
                if (Interlocked.CompareExchange(ref this.disponseState, 1, 0) == 0)
                {
                    this.SendMessage(Message.Dispose);
                }
            }

            base.Dispose(disposing);
        }

        private enum Message
        {
            Buffer,

            Flush,

            Dispose
        }
    }
}