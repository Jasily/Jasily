using System;
using JetBrains.Annotations;

namespace Jasily.IO
{
    public class StreamReadObserverHandler : IStreamReadHandler
    {
        private readonly IObserver<long> observer;
        private long current;

        public StreamReadObserverHandler([NotNull] IObserver<long> observer)
        {
            this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        public void OnReaded(byte[] buffer, int offset, int count)
        {
            this.current += count;
            this.observer.OnNext(this.current);
        }

        public void OnCompleted() => this.observer.OnCompleted();
    }
}