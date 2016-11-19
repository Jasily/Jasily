using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jasily.IO
{
    public sealed class MergedReadonlyStream : ReadonlyStream
    {
        private readonly Stream[] streams;

        public MergedReadonlyStream(IEnumerable<Stream> source)
        {
            this.streams = source.ToArray();
            foreach (var stream in this.streams)
            {
                if (stream == null) throw new ArgumentException("stream must not null.", nameof(source));
                if (!stream.CanRead) throw new ArgumentException("stream must can read.", nameof(source));
            }
            this.CanSeek = this.streams.All(z => z.CanSeek);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!this.CanSeek) throw new NotSupportedException();

            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.Position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Position += offset;
                    break;

                case SeekOrigin.End:
                    this.Position = this.Length + offset;
                    break;
            }

            return this.Position;
        }

        public override bool CanSeek { get; }

        public override long Length => this.streams.Sum(z => z.Length);

        public override long Position
        {
            get { return this.streams.Sum(z => z.Position); }
            set
            {
                foreach (var i in this.streams)
                {
                    if (value <= 0)
                        i.Position = 0;
                    else
                    {
                        i.Position = Math.Min(i.Length, value);
                        value -= i.Length;
                    }
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var readed = 0;
            for (var i = 0; readed < count && i < this.streams.Length; i++)
            {
                readed += this.streams[i].Read(buffer, offset + readed, count - readed);
            }
            return readed;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var stream in this.streams)
                {
                    stream.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}