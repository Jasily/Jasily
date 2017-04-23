using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal sealed class InstanceMethodInvoker<T> : MethodInvoker, IInstanceMethodInvoker<T>
    {
        private readonly bool isValueType;
        private Func<T, OverrideArguments, object> func;

        public InstanceMethodInvoker(IServiceProvider serviceProvider, MethodInfo method, bool isValueType)
            : base(serviceProvider, method)
        {
            this.isValueType = isValueType;
            this.func = this.ImplFunc();
        }

        public object Invoke(T instance, OverrideArguments arguments)
        {
            if (!this.isValueType && object.Equals(instance, null)) throw new ArgumentNullException();
            return this.func(instance, arguments);
        }

        private Func<T, OverrideArguments, object> ImplFunc()
        {
            var count = 0;
            if (this.Parameters.Length == 0)
            {
                return (i, a) =>
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
                return (i, a) =>
                {
                    if (Interlocked.Increment(ref count) == 4)
                    {
                        Task.Run(() => Volatile.Write(ref this.func, this.CompileFunc()));
                    }

                    try
                    {
                        return this.Method.Invoke(i, this.ResolveArguments(a));
                    }
                    catch (TargetInvocationException e)
                    {
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                        throw;
                    }
                };
            }
        }

        private Func<T, OverrideArguments, object> CompileFunc()
        {
            var instance = Expression.Parameter(typeof(T));

            Expression body = this.Parameters.Length == 0
                ? Expression.Call(instance, this.Method)
                : Expression.Call(instance, this.Method, this.ResolveArgumentsExpressions());
            body = this.ResolveBodyExpressions(body);

            if (this.Method.ReturnType == typeof(void))
            {
                var action = Expression.Lambda<Action<T, OverrideArguments>>(body,
                    instance, ParameterOverrideArguments
                ).Compile();
                return (i, x) =>
                {
                    action(i, x);
                    return null;
                };
            }
            else
            {
                return Expression.Lambda<Func<T, OverrideArguments, object>>(body,
                    instance, ParameterOverrideArguments
                ).Compile();
            }
        }
    }
}
