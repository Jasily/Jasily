using System;
using System.IO;
using JetBrains.Annotations;

namespace Jasily.IO
{
    public class WrapStream : Stream
    {
        public WrapStream([NotNull] Stream baseStream)
        {
            if (baseStream == null) throw new ArgumentNullException(nameof(baseStream));
            this.BaseStream = baseStream;
        }

        public Stream BaseStream { get; }

        public override void Flush() => this.BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => this.BaseStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => this.BaseStream.Seek(offset, origin);

        public override void SetLength(long value) => this.BaseStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => this.BaseStream.Write(buffer, offset, count);

        public override bool CanRead => this.BaseStream.CanRead;

        public override bool CanSeek => this.BaseStream.CanSeek;

        public override bool CanWrite => this.BaseStream.CanWrite;

        public override long Length => this.BaseStream.Length;

        public override long Position
        {
            get { return this.BaseStream.Position; }
            set { this.BaseStream.Position = value; }
        }
    }
}