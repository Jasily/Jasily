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

        public void Invoke<T>(T obj, string methodName)
        {
            throw new NotImplementedException();
        }

        public TResult Invoke<T, TResult>(T obj, string methodName)
        {
            throw new NotImplementedException();
        }

        public void Invoke<T>(T obj, MethodInfo methodInfo)
        {
            
        }
    }
}