using System;
using JetBrains.Annotations;

namespace Jasily.IO
{
    public class StreamReadProgressHandler : IStreamReadHandler
    {
        private readonly IProgress<long> progress;
        private long current;

        public StreamReadProgressHandler([NotNull] IProgress<long> progress)
        {
            this.progress = progress ?? throw new ArgumentNullException(nameof(progress));
        }

        public void OnReaded(byte[] buffer, int offset, int count)
        {
            this.current += count;
            this.progress.Report(this.current);
        }

        public void OnCompleted() { }
    }
}