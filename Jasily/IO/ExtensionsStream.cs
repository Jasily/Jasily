using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jasily.IO.Internal;
using JetBrains.Annotations;

namespace Jasily.IO
{
    public class ExtensionsStream : WrapStream
    {
        private readonly IStreamReadHandler[] readHandlers;

        public ExtensionsStream([NotNull] Stream baseStream, [NotNull] IEnumerable<IStreamHandler> handlers)
            : base(baseStream)
        {
            if (handlers == null) throw new ArgumentNullException(nameof(handlers));
            var array = handlers.ToArray();
            this.readHandlers = array.OfType<IStreamReadHandler>().ToArray();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var readed = base.Read(buffer, offset, count);
            foreach (var plugin in this.readHandlers)
            {
                plugin.OnReaded(buffer, offset, readed);
                if (readed < count) plugin.OnCompleted();
            }
            return readed;
        }
    }
}