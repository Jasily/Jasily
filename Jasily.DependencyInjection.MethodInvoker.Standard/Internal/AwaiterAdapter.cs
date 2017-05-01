using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal abstract class AwaiterAdapter : IAwaiterAdapter
    {
        public AwaiterAdapter(Type awaiterType, IServiceProvider serviceProvider)
        {
            this.IsCompletedMethod = awaiterType.GetRuntimeProperties()
                .Where(z => z.Name == nameof(TaskAwaiter.IsCompleted))
                .Where(z => z.GetIndexParameters().Length == 0)
                .FirstOrDefault();

            this.GetResultMethod = awaiterType.GetRuntimeMethods()
                .Where(z => z.Name == nameof(TaskAwaiter.GetResult))
                .Where(z => z.GetParameters().Length == 0)
                .FirstOrDefault();

            this.ServiceProvider = serviceProvider;

            this.IsValueType = awaiterType.GetTypeInfo().IsValueType;
        }

        public IServiceProvider ServiceProvider { get; }

        public PropertyInfo IsCompletedMethod { get; }

        public MethodInfo GetResultMethod { get; }

        public bool IsValueType { get; }

        public bool IsAwaitable => this.IsCompletedMethod != null && this.GetResultMethod != null;

        public abstract object GetResult(object instance);
    }

    internal class AwaiterAdapter<T> : AwaiterAdapter, IAwaiterAdapter<T, object>
    {
        private readonly IInstanceMethodInvoker<T> invoker;

        public AwaiterAdapter(IServiceProvider serviceProvider) : base(typeof(T), serviceProvider)
        {
            if (this.IsAwaitable)
            {
                this.invoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<T>>()
                    .GetInstanceMethodInvoker(this.GetResultMethod);
            }
        }

        public object GetResult(T instance)
        {
            if (this.IsValueType || instance == null) throw new ArgumentNullException(nameof(instance));
            if (!this.IsAwaitable) throw new InvalidOperationException("is NOT awaitable.");

            this.invoker.Invoke(instance, this.ServiceProvider, default(OverrideArguments));
            return null;
        }

        public override object GetResult(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return this.GetResult((T)instance);
        }
    }

    internal class AwaiterAdapter<T, TResult> : AwaiterAdapter, IAwaiterAdapter<T, TResult>
    {
        private readonly IInstanceMethodInvoker<T, TResult> invoker;

        public AwaiterAdapter(IServiceProvider serviceProvider) : base(typeof(T), serviceProvider)
        {
            if (this.IsAwaitable)
            {
                this.invoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<T>>()
                    .GetInstanceMethodInvoker(this.GetResultMethod)
                    .CastTo<TResult>();
            }
        }

        public TResult GetResult(T instance)
        {
            if (!this.IsValueType && instance == null) throw new ArgumentNullException(nameof(instance));
            if (!this.IsAwaitable) throw new InvalidOperationException("is NOT awaitable.");

            return this.invoker.Invoke(instance, this.ServiceProvider, default(OverrideArguments));
        }

        public override object GetResult(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return this.GetResult((T)instance);
        }
    }
}
