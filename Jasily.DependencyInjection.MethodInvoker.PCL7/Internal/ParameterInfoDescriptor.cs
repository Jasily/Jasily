using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal abstract class ParameterInfoDescriptor
    {
        public static ParameterInfoDescriptor Build(ParameterInfo parameter)
        {
            var type = typeof(ParameterInfoDescriptor<>).MakeGenericType(parameter.ParameterType);
            return (ParameterInfoDescriptor) Activator.CreateInstance(type, parameter);
        }

        public ParameterInfoDescriptor(ParameterInfo parameter)
        {
            this.Parameter = parameter;
        }

        public ParameterInfo Parameter { get; }

        public abstract object ResolveArgumentObject(IServiceProvider provider, OverrideArguments arguments);
    }

    internal class ParameterInfoDescriptor<T> : ParameterInfoDescriptor
    {
        private readonly T DefaultValue;

        public ParameterInfoDescriptor(ParameterInfo parameter) : base(parameter)
        {
            this.ScopedArgumentsType = typeof(IScopedArguments<T>);
            this.SingletonArgumentsType = typeof(ISingletonArguments<T>);

            this.ArgumentsTypes = new[]
            {
                this.ScopedArgumentsType,
                this.SingletonArgumentsType
            };

            if (this.Parameter.HasDefaultValue) this.DefaultValue = (T)this.Parameter.DefaultValue;
        }

        public Type ScopedArgumentsType { get; }

        public Type SingletonArgumentsType { get; }

        public Type[] ArgumentsTypes { get; }

        private bool TryResolveArgument(IServiceProvider provider, OverrideArguments arguments, out T value)
        {
            if (arguments.TryGetValue<T>(this.Parameter, out value))
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

            if (this.Parameter.HasDefaultValue) return this.DefaultValue;

            throw new ParameterResolveException(this.Parameter);
        }
    }
}
