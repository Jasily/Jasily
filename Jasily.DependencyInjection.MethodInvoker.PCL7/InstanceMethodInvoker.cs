using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class InstanceMethodInvoker : MethodInvoker
    {
        private Func<object, IServiceProvider, OverrideArguments, object> func;

        public InstanceMethodInvoker(MethodInfo method) : base(method)
        {
            this.func = this.ImplFunc();
        }

        public override object Invoke(object instance, IServiceProvider provider, OverrideArguments arguments)
        {
            if (object.Equals(instance, null)) throw new ArgumentNullException();
            return this.func(instance, provider, arguments);
        }

        private Func<object, IServiceProvider, OverrideArguments, object> ImplFunc()
        {
            if (this.Parameters.Length == 0)
            {
                return (i, p, a) =>
                {
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
                    try
                    {
                        return this.Method.Invoke(i, ResolveArguments(this, p, a));
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
