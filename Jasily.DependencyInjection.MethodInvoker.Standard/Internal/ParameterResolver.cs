using System;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal abstract class ParameterResolver
    {
        public static ParameterResolver Build(ParameterInfo parameter)
        {
            var type = typeof(ParameterResolver<>).MakeGenericType(parameter.ParameterType);
            return (ParameterResolver) Activator.CreateInstance(type, parameter);
        }

        protected ParameterResolver(ParameterInfo parameter)
        {
            this.Parameter = parameter;
        }

        public ParameterInfo Parameter { get; }

        public abstract object ResolveArgumentObject(IServiceProvider provider, OverrideArguments arguments);
    }

    internal class ParameterResolver<T> : ParameterResolver
    {
        private readonly object _defaultObject;
        private readonly T _defaultValue;
        private IArgumentResolver<T> _argumentResolver;

        public ParameterResolver(ParameterInfo parameter) : base(parameter)
        {
            if (this.Parameter.HasDefaultValue)
            {
                this._defaultObject = this.Parameter.DefaultValue;
                this._defaultValue = (T)this._defaultObject;
            }
        }

        private IArgumentResolver<T> GetArgumentResolver(IServiceProvider provider)
        {
            if (this._argumentResolver == null)
            {
                var resolver = provider.GetRequiredService<IArgumentResolver<T>>();
                Interlocked.CompareExchange(ref this._argumentResolver, resolver, null);
            }
            return this._argumentResolver;
        }

        public override object ResolveArgumentObject(IServiceProvider provider, OverrideArguments arguments)
        {
            return this.GetArgumentResolver(provider)
                .ResolveArgument(provider, this.Parameter, arguments, this._defaultObject);
        }

        public T ResolveArgumentValue(IServiceProvider provider, OverrideArguments arguments)
        {
            return this.GetArgumentResolver(provider)
                .ResolveArgument(provider, this.Parameter, arguments, this._defaultValue);
        }
    }
}
