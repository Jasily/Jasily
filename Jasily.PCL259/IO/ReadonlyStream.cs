using System;
using System.IO;

namespace Jasily.IO
{
    public abstract class ReadonlyStream : Stream
    {
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}