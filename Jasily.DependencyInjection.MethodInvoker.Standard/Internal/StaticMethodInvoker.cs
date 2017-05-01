using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class StaticMethodInvoker : MethodInvoker,
        IStaticMethodInvoker, IStaticMethodInvoker<object>
    {
        private Action<IServiceProvider, OverrideArguments> func;

        public StaticMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.func = this.ImplFunc();
        }

        public object Invoke(IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            this.func(serviceProvider, arguments);
            return null;
        }

        private Action<IServiceProvider, OverrideArguments> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold <= 0) return this.CompileFunc();
            var count = 0;
            return (p, args) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                this.InvokeMethod<object>(null, p, args);
            };
        }

        private Action<IServiceProvider, OverrideArguments> CompileFunc()
        {
            Expression body = this.Parameters.Length == 0
                ? Expression.Call(this.Method)
                : Expression.Call(this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Action<IServiceProvider, OverrideArguments>>(body,
                    ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
        }

        public IStaticMethodInvoker<TResult> CastTo<TResult>()
        {
            return (IStaticMethodInvoker<TResult>)this;
        }
    }

    internal class StaticMethodInvoker<TResult> : MethodInvoker,
        IStaticMethodInvoker, IStaticMethodInvoker<TResult>
    {
        private Func<IServiceProvider, OverrideArguments, TResult> func;

        public StaticMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.func = this.ImplFunc();
        }

        public TResult Invoke(IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            return this.func(serviceProvider, arguments);
        }

        object IStaticMethodInvoker.Invoke(IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            return this.Invoke(serviceProvider, arguments);
        }

        private Func<IServiceProvider, OverrideArguments, TResult> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold <= 0) return this.CompileFunc();
            var count = 0;
            return (p, args) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                return this.InvokeMethod<TResult>(null, p, args);
            };
        }

        private Func<IServiceProvider, OverrideArguments, TResult> CompileFunc()
        {
            Expression body = this.Parameters.Length == 0
                ? Expression.Call(this.Method)
                : Expression.Call(this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Func<IServiceProvider, OverrideArguments, TResult>>(body,
                    ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
        }

        public IStaticMethodInvoker<TResult2> CastTo<TResult2>()
        {
            return (IStaticMethodInvoker<TResult2>)this;
        }
    }
}
