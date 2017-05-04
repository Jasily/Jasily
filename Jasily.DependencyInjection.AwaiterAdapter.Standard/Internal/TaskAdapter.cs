using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Jasily.DependencyInjection.MethodInvoker;
using Microsoft.Extensions.DependencyInjection;

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
