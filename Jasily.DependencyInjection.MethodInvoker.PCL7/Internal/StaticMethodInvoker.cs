using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class StaticMethodInvoker : MethodInvoker, IStaticMethodInvoker
    {
        private Func<OverrideArguments, object> func;

        public StaticMethodInvoker(IServiceProvider serviceProvider, MethodInfo method)
            : base(serviceProvider, method)
        {
            this.func = this.ImplFunc();
        }

        public object Invoke(OverrideArguments arguments)
        {
            return this.func(arguments);
        }

        private Func<OverrideArguments, object> ImplFunc()
        {
            var count = 0;
            if (this.Parameters.Length == 0)
            {
                return a =>
                {
                    if (Interlocked.Increment(ref count) == 4)
                    {
                        Task.Run(() => Volatile.Write(ref this.func, this.CompileFunc()));
                    }

                    try
                    {
                        return this.Method.Invoke(null, null);
                    }
                    catch (TargetInvocationException e)
                    {
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                        throw;
                    }
                };
            }
            else
            {
                return a =>
                {
                    if (Interlocked.Increment(ref count) == 4)
                    {
                        Task.Run(() => Volatile.Write(ref this.func, this.CompileFunc()));
                    }

                    try
                    {
                        return this.Method.Invoke(null, this.ResolveArguments(a));
                    }
                    catch (TargetInvocationException e)
                    {
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                        throw;
                    }
                };
            }
        }

        private Func<OverrideArguments, object> CompileFunc()
        {
            Expression body = this.Parameters.Length == 0
                ? Expression.Call(this.Method)
                : Expression.Call(this.Method, this.ResolveArgumentsExpressions());
            body = this.ResolveBodyExpressions(body);

            if (this.Method.ReturnType == typeof(void))
            {
                var action = Expression.Lambda<Action<OverrideArguments>>(body,
                    ParameterOverrideArguments
                ).Compile();
                return x =>
                {
                    action(x);
                    return null;
                };
            }
            else
            {
                return Expression.Lambda<Func<OverrideArguments, object>>(body,
                    ParameterOverrideArguments
                ).Compile();
            }
        }
    }
}
