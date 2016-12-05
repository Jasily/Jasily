using System;
using System.Linq;
using System.Reflection;
using Jasily.Cache;
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

        private static MethodInfo GetSingleMethod<T>([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            return TypeDescriptor<T>.RuntimeMethods(methodName).Single();
        }

        public void Invoke<T>(T obj, [NotNull] string methodName)
            => this.Invoke(obj, GetSingleMethod<T>(methodName));

        public TResult Invoke<T, TResult>(T obj, string methodName) 
            => this.Invoke<T, TResult>(obj, GetSingleMethod<T>(methodName));

        public void Invoke<T>(T obj, MethodInfo methodInfo)
        {
            throw new NotImplementedException();
        }

        public TResult Invoke<T, TResult>(T obj, MethodInfo methodInfo)
        {
            throw new NotImplementedException();
        }
    }
}