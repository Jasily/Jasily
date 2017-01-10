using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public class MethodInvoker
    {
        private readonly ServiceProvider provider;

        public MethodInvoker([NotNull] ServiceProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            this.provider = provider;
        }

        public void Invoke<T>(T obj, [NotNull] MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            throw new NotImplementedException();
        }

        public TResult Invoke<T, TResult>(T obj, [NotNull] MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.ReturnType == typeof(void)) throw new InvalidOperationException("method has no return value.");
            throw new NotImplementedException();
        }
    }
}