using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class StaticMethodInvoker : MethodInvoker
    {
        private Func<IServiceProvider, OverrideArguments, object> func;

        public StaticMethodInvoker(MethodInfo method) : base(method)
        {
            this.func = this.ImplFunc();
        }

        public override object Invoke(object instance, IServiceProvider provider, OverrideArguments arguments)
        {
            return this.func(provider, arguments);
        }

        private Func<IServiceProvider, OverrideArguments, object> ImplFunc()
        {
            var count = 0;
            if (this.Parameters.Length == 0)
            {
                return (p, a) =>
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
                return (p, a) =>
                {
                    if (Interlocked.Increment(ref count) == 4)
                    {
                        Task.Run(() => Volatile.Write(ref this.func, this.CompileFunc()));
                    }

                    try
                    {
                        return this.Method.Invoke(null, this.ResolveArguments(p, a));
                    }
                    catch (TargetInvocationException e)
                    {
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                        throw;
                    }
                };
            }
        }

        private Func<IServiceProvider, OverrideArguments, object> CompileFunc()
        {
            Expression body = this.Parameters.Length == 0
                ? Expression.Call(this.Method)
                : Expression.Call(this.Method, this.ResolveArgumentsExpressions());
            body = this.ResolveBodyExpressions(body);

            if (this.Method.ReturnType == typeof(void))
            {
                var action = Expression.Lambda<Action<IServiceProvider, OverrideArguments>>(body,
                    ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
                return (z, x) =>
                {
                    action(z, x);
                    return null;
                };
            }
            else
            {
                return Expression.Lambda<Func<IServiceProvider, OverrideArguments, object>>(body,
                    ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
            }
        }
    }
}
