using System;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class TaskAdapter<T> : ITaskAdapter<T>
    {
        public TaskAdapter(IServiceProvider serviceProvider)
        {
            
        }

        public IAwaitableAdapter AwaitableAdapter { get; }
    }
}
