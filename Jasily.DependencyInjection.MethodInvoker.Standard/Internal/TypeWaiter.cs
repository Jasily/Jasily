using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class TypeWaiter
    {
        internal sealed class NonAwaiterAdapter : IAwaiterAdapter
        {
            public bool IsAwaitable => false;

            public object GetResult(object instance)
            {
                throw new InvalidOperationException("is NOT awaitable.");
            }
        }

        protected static NonAwaiterAdapter NonAwaiterInstance { get; } = new NonAwaiterAdapter();
    }

    internal class TypeWaiter<T> : TypeWaiter, ITypeWaiter<T>
    {
        private readonly IServiceProvider serviceProvider;

        public TypeWaiter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            var method = typeof(T).GetRuntimeMethods()
                .Where(z => z.Name == nameof(Task.GetAwaiter))
                .Where(z => z.GetParameters().Length == 0)
                .Where(z => z.ReturnType != typeof(void))
                .FirstOrDefault();

            if (method != null)
            {
                this.GetAwaiterInvoker = this.serviceProvider
                    .GetRequiredService<IMethodInvokerFactory<T>>()
                    .GetInstanceMethodInvoker(method);

                this.AwaiterAdapter = TryGetAwaiter(serviceProvider, method.ReturnType);
            }

            this.AwaiterAdapter = this.AwaiterAdapter ?? NonAwaiterInstance;
        }

        public IInstanceMethodInvoker<T> GetAwaiterInvoker { get; }

        public IAwaiterAdapter AwaiterAdapter { get; }

        private static IAwaiterAdapter TryGetAwaiter(IServiceProvider provider, Type awaiterType)
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
                        return (IAwaiterAdapter)provider.GetRequiredService(typeof(AwaiterAdapter<>)
                            .MakeGenericType(awaiterType));
                    }
                    else
                    {
                        return (IAwaiterAdapter)provider.GetRequiredService(typeof(AwaiterAdapter<,>)
                            .MakeGenericType(awaiterType, resultType));
                    }
                }
            }

            return null;
        }

        public TResult GetResult<TAwaiter, TResult>(T instance)
        {
            if (this.AwaiterAdapter is IAwaiterAdapter<TAwaiter, TResult> adapter)
            {
                var invoker = this.GetAwaiterInvoker.CastTo<TAwaiter>();
                var awaiter = invoker.Invoke(instance, this.serviceProvider);
                return adapter.GetResult(awaiter);
            }
            else throw new InvalidOperationException();
        }

        public object GetResult(object instance)
        {
            var awaiter = this.GetAwaiterInvoker.Invoke((T)instance, this.serviceProvider);
            return this.AwaiterAdapter.GetResult(awaiter);
        }
    }
}
