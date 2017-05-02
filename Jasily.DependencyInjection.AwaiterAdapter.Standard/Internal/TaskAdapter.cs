using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Jasily.DependencyInjection.MethodInvoker;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class UnawaitableTaskAwaiterAdapter : ITaskAwaiterAdapter
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
    }

    internal class UnawaitableTaskAwaiterAdapter<T> : UnawaitableTaskAwaiterAdapter, ITaskAwaiterAdapter<T>
    {
        public bool IsCompleted(T instance)
        {
            throw new InvalidOperationException("is NOT awaitable.");
        }

        public void GetResult(T instance)
        {
            throw new InvalidOperationException("is NOT awaitable.");
        }

        public void OnCompleted(T instance, Action continuation)
        {
            throw new InvalidOperationException("is NOT awaitable.");
        }
    }

    internal class TaskAdapter
    {
        internal sealed class NonTaskAwaiterAdapter : ITaskAwaiterAdapter
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
        }

        protected static NonTaskAwaiterAdapter NonTaskAwaiterInstance { get; } = new NonTaskAwaiterAdapter();
    }

    internal class TaskAdapter<T> : TaskAdapter, ITaskAdapter<T>
    {
        private readonly IServiceProvider _serviceProvider;

        public TaskAdapter(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;

            var method = typeof(T).GetRuntimeMethods()
                .Where(z => z.Name == nameof(Task.GetAwaiter))
                .Where(z => z.GetParameters().Length == 0)
                .FirstOrDefault(z => z.ReturnType != typeof(void));

            if (method != null)
            {
                this.TaskAwaiterAdapter = TryGetAwaiter(serviceProvider, typeof(T), method.ReturnType);
            }

            this.TaskAwaiterAdapter = this.TaskAwaiterAdapter ?? NonTaskAwaiterInstance;
        }

        public ITaskAwaiterAdapter TaskAwaiterAdapter { get; }

        private static ITaskAwaiterAdapter TryGetAwaiter(IServiceProvider provider, Type instanceType, Type awaiterType)
        {
            if (awaiterType != typeof(void) &&
                typeof(INotifyCompletion).GetTypeInfo().IsAssignableFrom(awaiterType.GetTypeInfo()))
            {
                var resultType = awaiterType.GetRuntimeMethods()
                    .Where(z => z.Name == nameof(TaskAwaiter.GetResult))
                    .Where(z => z.GetParameters().Length == 0)
                    .Select(z => z.ReturnType)
                    .FirstOrDefault();

                if (resultType != null)
                {
                    if (resultType == typeof(void))
                    {
                        return (ITaskAwaiterAdapter)provider.GetRequiredService(typeof(TaskAwaiterAdapter<,>)
                            .FastMakeGenericType(instanceType, awaiterType));
                    }
                    else
                    {
                        return (ITaskAwaiterAdapter)provider.GetRequiredService(typeof(TaskAwaiterAdapter<,,>)
                            .FastMakeGenericType(instanceType, awaiterType, resultType));
                    }
                }
            }

            return null;
        }
    }
}
