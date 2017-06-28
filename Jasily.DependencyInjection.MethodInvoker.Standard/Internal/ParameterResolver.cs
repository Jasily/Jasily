using System;
using System.Reflection;

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
        private readonly T _defaultValue;

        public ParameterResolver(ParameterInfo parameter) : base(parameter)
        {
            this.ScopedArgumentsType = typeof(IScopedArguments<T>);
            this.SingletonArgumentsType = typeof(ISingletonArguments<T>);

            this.ArgumentsTypes = new[]
            {
                this.ScopedArgumentsType,
                this.SingletonArgumentsType
            };

            if (this.Parameter.HasDefaultValue) this._defaultValue = (T)this.Parameter.DefaultValue;
        }

        public Type ScopedArgumentsType { get; }

        public Type SingletonArgumentsType { get; }

        public Type[] ArgumentsTypes { get; }

        private bool TryResolveArgument(IServiceProvider provider, OverrideArguments arguments, out T value)
        {
            if (arguments.TryGetValue(this.Parameter, out value, provider))
            {
                return true;
            }

            for (var i = 0; i < this.ArgumentsTypes.Length; i++)
            {
                if (provider.GetService(this.ArgumentsTypes[i]) is IArguments<T> args &&
                    args.TryGetValue(this.Parameter.Name, out value))
                {
                    return true;
                }
            }

            var obj = provider.GetService(typeof(T));
            if (obj != null)
            {
                value = (T)obj;
                return true;
            }

            value = default(T);
            return false;
        }

        public override object ResolveArgumentObject(IServiceProvider provider, OverrideArguments arguments)
        {
            if (this.TryResolveArgument(provider, arguments, out var value)) return value;
            if (this.Parameter.HasDefaultValue) return this.Parameter.DefaultValue;

            throw new ParameterResolveException(this.Parameter);
        }

        public T ResolveArgumentValue(IServiceProvider provider, OverrideArguments arguments)
        {
            if (this.TryResolveArgument(provider, arguments, out var value)) return value;
            if (this.Parameter.HasDefaultValue) return this._defaultValue;

            throw new ParameterResolveException(this.Parameter);
        }
    }
}
