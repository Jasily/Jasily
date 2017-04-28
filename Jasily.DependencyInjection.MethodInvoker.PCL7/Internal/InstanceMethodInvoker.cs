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
        private Action<T, OverrideArguments> func;

        public InstanceMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.isValueType = factory.IsValueType;
            this.func = this.ImplFunc();
        }

        public object Invoke(T instance, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException(nameof(instance));
            this.func(instance, arguments);
            return null;
        }

        private Action<T, OverrideArguments> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold == 0) return this.CompileFunc();
            var count = 0;
            return (instance, args) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                this.InvokeMethod<object>(instance, args);
            };
        }

        private Action<T, OverrideArguments> CompileFunc()
        {
            var instance = Expression.Parameter(typeof(T));

            Expression body = this.Parameters.Length == 0
                ? Expression.Call(instance, this.Method)
                : Expression.Call(instance, this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Action<T, OverrideArguments>>(body,
                    instance, ParameterOverrideArguments
                ).Compile();
        }
    }

    internal sealed class InstanceMethodInvoker<T, TResult> : MethodInvoker,
        IInstanceMethodInvoker<T>, IInstanceMethodInvoker<T, TResult>
    {
        private readonly bool isValueType;
        private Func<T, OverrideArguments, TResult> func;

        public InstanceMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.isValueType = factory.IsValueType;
            this.func = this.ImplFunc();
        }

        public TResult Invoke(T instance, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException(nameof(instance));
            return this.func(instance, arguments);
        }

        object IInstanceMethodInvoker<T>.Invoke(T instance, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException();
            return this.func(instance, arguments);
        }

        private Func<T, OverrideArguments, TResult> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold == 0) return this.CompileFunc();
            var count = 0;
            return (i, a) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                return this.InvokeMethod<TResult>(i, a);
            };
        }

        private Func<T, OverrideArguments, TResult> CompileFunc()
        {
            var instance = Expression.Parameter(typeof(T));

            Expression body = this.Parameters.Length == 0
                ? Expression.Call(instance, this.Method)
                : Expression.Call(instance, this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Func<T, OverrideArguments, TResult>>(body,
                    instance, ParameterOverrideArguments
                ).Compile();
        }
    }
}
