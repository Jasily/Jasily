using System;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class TaskAdapter<T> : ITaskAdapter<T>
    {
        public TaskAdapter(IServiceProvider serviceProvider)
        {
            this.AwaitableAdapter = Internal.AwaitableAdapter.GetAwaitableAdapter(serviceProvider, typeof(T));
        }

        public IAwaitableAdapter AwaitableAdapter { get; }
    }
}
