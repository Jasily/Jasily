using System;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal abstract class MethodInvoker : BaseInvoker
    {
        public MethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
            : base(factory, method)
        {
            this.Method = method;
        }

        public MethodInfo Method { get; }

        protected T InvokeMethod<T>(object instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            var a = this.Parameters.Length == 0 ? null : this.ResolveArguments(serviceProvider, args);

            try
            {
                return (T) this.Method.Invoke(instance, a);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }
    }
}
