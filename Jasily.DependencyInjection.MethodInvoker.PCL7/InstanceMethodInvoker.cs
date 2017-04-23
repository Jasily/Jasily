using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal sealed class InstanceMethodInvoker<T> : MethodInvoker, IInstanceMethodInvoker<T>
    {
        private readonly bool isValueType;
        private Func<T, IServiceProvider, OverrideArguments, object> func;

        public InstanceMethodInvoker(MethodInfo method, bool isValueType) : base(method)
        {
            this.isValueType = isValueType;
            this.func = this.ImplFunc();
        }

        public object Invoke(T instance, IServiceProvider provider, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException();
            return this.func(instance, provider, arguments);
        }

        private Func<T, IServiceProvider, OverrideArguments, object> ImplFunc()
        {
            var count = 0;
            if (this.Parameters.Length == 0)
            {
                return (i, p, a) =>
                {
                    if (Interlocked.Increment(ref count) == 4)
                    {
                        Task.Run(() => Volatile.Write(ref this.func, this.CompileFunc()));
                    }

                    try
                    {
                        return this.Method.Invoke(i, null);
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
                return (i, p, a) =>
                {
                    if (Interlocked.Increment(ref count) == 4)
                    {
                        Task.Run(() => Volatile.Write(ref this.func, this.CompileFunc()));
                    }

                    try
                    {
                        return this.Method.Invoke(i, this.ResolveArguments(p, a));
                    }
                    catch (TargetInvocationException e)
                    {
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                        throw;
                    }
                };
            }
        }

        private Func<T, IServiceProvider, OverrideArguments, object> CompileFunc()
        {
            var instance = Expression.Parameter(typeof(T));

            Expression body = this.Parameters.Length == 0
                ? Expression.Call(instance, this.Method)
                : Expression.Call(instance, this.Method, this.ResolveArgumentsExpressions());
            body = this.ResolveBodyExpressions(body);

            if (this.Method.ReturnType == typeof(void))
            {
                var action = Expression.Lambda<Action<T, IServiceProvider, OverrideArguments>>(body,
                    instance, ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
                return (i, z, x) =>
                {
                    action(i, z, x);
                    return null;
                };
            }
            else
            {
                return Expression.Lambda<Func<T, IServiceProvider, OverrideArguments, object>>(body,
                    instance, ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
            }
        }
    }
}
