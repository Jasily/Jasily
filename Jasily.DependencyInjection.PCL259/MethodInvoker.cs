using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public class MethodInvoker
    {
        private readonly ServiceProvider provider;

        public MethodInvoker([NotNull] ServiceProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            this.provider = provider.CreateScope();
        }

        public void Invoke<T>(T obj, [NotNull] MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            this.InternalInvoke(obj, methodInfo);
        }

        public TResult Invoke<T, TResult>(T obj, [NotNull] MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.ReturnType == typeof(void)) throw new InvalidOperationException("method has no return value.");
            return (TResult) this.InternalInvoke(obj, methodInfo);
        }

        public object InternalInvoke(object obj, [NotNull] MethodInfo methodInfo)
        {
            var parameters = this.ResolveParameters(methodInfo);
            object value;
            try
            {
                value = methodInfo.Invoke(obj, parameters);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
            return value;
        }

        private object[] ResolveParameters(MethodBase methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var parameterValues = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var result = this.provider.GetService(parameter.ParameterType, parameter.Name);
                if (!result.HasValue) throw new InvalidOperationException();
                parameterValues[i] = result.Value;
            }
            return parameterValues;
        }
    }
}