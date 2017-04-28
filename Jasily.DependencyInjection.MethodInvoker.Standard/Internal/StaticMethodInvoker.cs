using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class StaticMethodInvoker : MethodInvoker,
        IStaticMethodInvoker, IStaticMethodInvoker<object>
    {
        private Action<OverrideArguments> func;

        public StaticMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.func = this.ImplFunc();
        }

        public object Invoke(OverrideArguments arguments)
        {
            this.func(arguments);
            return null;
        }

        private Action<OverrideArguments> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold == 0) return this.CompileFunc();
            var count = 0;
            return args =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                this.InvokeMethod<object>(null, args);
            };
        }

        private Action<OverrideArguments> CompileFunc()
        {
            Expression body = this.Parameters.Length == 0
                ? Expression.Call(this.Method)
                : Expression.Call(this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Action<OverrideArguments>>(body,
                    ParameterOverrideArguments
                ).Compile();
        }

        public IStaticMethodInvoker<TResult> CastTo<TResult>()
        {
            throw new InvalidOperationException("method has no return value.");
        }
    }

    internal class StaticMethodInvoker<TResult> : MethodInvoker,
        IStaticMethodInvoker, IStaticMethodInvoker<TResult>
    {
        private Func<OverrideArguments, TResult> func;

        public StaticMethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.func = this.ImplFunc();
        }

        public TResult Invoke(OverrideArguments arguments)
        {
            return this.func(arguments);
        }

        object IStaticMethodInvoker.Invoke(OverrideArguments arguments)
        {
            return this.Invoke(arguments);
        }

        private Func<OverrideArguments, TResult> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold == 0) return this.CompileFunc();
            var count = 0;
            return args =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                return this.InvokeMethod<TResult>(null, args);
            };
        }

        private Func<OverrideArguments, TResult> CompileFunc()
        {
            Expression body = this.Parameters.Length == 0
                ? Expression.Call(this.Method)
                : Expression.Call(this.Method, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Func<OverrideArguments, TResult>>(body,
                    ParameterOverrideArguments
                ).Compile();
        }

        public IStaticMethodInvoker<TResult2> CastTo<TResult2>()
        {
            var smi = this as IStaticMethodInvoker<TResult2>;
            if (smi != null) return smi;
            throw new InvalidOperationException($"method return value type is {this.Method.ReturnType}, not {typeof(TResult2)}.");
        }
    }
}
