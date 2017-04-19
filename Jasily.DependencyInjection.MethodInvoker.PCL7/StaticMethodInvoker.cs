using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

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
            if (this.Parameters.Length == 0)
            {
                return (p, a) =>
                {
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
                    try
                    {
                        return this.Method.Invoke(null, ResolveArguments(this, p, a));
                    }
                    catch (TargetInvocationException e)
                    {
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                        throw;
                    }
                };
            }
        }
    }
}
