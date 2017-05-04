using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal sealed class InstanceMethodInvoker<T> : MethodInvoker,
        IInstanceMethodInvoker<T>, IInstanceMethodInvoker<T, object>
    {
        private readonly bool isValueType;
        private Action<T, IServiceProvider, OverrideArguments> func;

        public InstanceMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.isValueType = factory.IsValueType;
            this.func = this.ImplFunc();
        }

        public object Invoke(T instance, IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException(nameof(instance));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            this.func(instance, serviceProvider, arguments);
            return null;
        }

        private Action<T, IServiceProvider, OverrideArguments> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold <= 0) return this.CompileFunc();
            var count = 0;
            return (instance, provider, args) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                this.InvokeMethod<object>(instance, provider, args);
            };
        }

        private Action<T, IServiceProvider, OverrideArguments> CompileFunc()
        {
            var instance = Expression.Parameter(typeof(T));

            Expression body = this.Parameters.Length == 0
                ? Expression.Call(instance, this.Method)
                : Expression.Call(instance, this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Action<T, IServiceProvider, OverrideArguments>>(body,
                    instance, ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
        }
    }

    internal sealed class InstanceMethodInvoker<T, TResult> : MethodInvoker,
        IInstanceMethodInvoker<T>, IInstanceMethodInvoker<T, TResult>
    {
        private readonly bool isValueType;
        private Func<T, IServiceProvider, OverrideArguments, TResult> func;

        public InstanceMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.isValueType = factory.IsValueType;
            this.func = this.ImplFunc();
        }

        public TResult Invoke(T instance, IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException(nameof(instance));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            return this.func(instance, serviceProvider, arguments);
        }

        object IInstanceMethodInvoker<T>.Invoke(T instance, IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException();
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            return this.func(instance, serviceProvider, arguments);
        }

        private Func<T, IServiceProvider, OverrideArguments, TResult> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold <= 0) return this.CompileFunc();
            var count = 0;
            return (i, p, a) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                return this.InvokeMethod<TResult>(i, p, a);
            };
        }

        private Func<T, IServiceProvider, OverrideArguments, TResult> CompileFunc()
        {
            var instance = Expression.Parameter(typeof(T));

            Expression body = this.Parameters.Length == 0
                ? Expression.Call(instance, this.Method)
                : Expression.Call(instance, this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Func<T, IServiceProvider, OverrideArguments, TResult>>(body,
                    instance, ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
        }
    }
}
