using System;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal sealed class NonTaskAwaiterAdapter : IAwaitableAdapter
    {
        public bool IsAwaitable => false;

        public bool IsCompleted(object instance)
        {
            throw new InvalidOperationException("is NOT awaitable.");
        }

        public object GetResult(object instance)
        {
            throw new InvalidOperationException("is NOT awaitable.");
        }

        public void OnCompleted(object instance, Action continuation)
        {
            throw new InvalidOperationException("is NOT awaitable.");
        }

        public static NonTaskAwaiterAdapter Instance { get; } = new NonTaskAwaiterAdapter();
    }
}