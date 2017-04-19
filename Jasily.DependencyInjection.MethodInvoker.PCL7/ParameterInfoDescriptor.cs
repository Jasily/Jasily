using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
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

        public abstract object ResolveArgument(IServiceProvider provider, OverrideArguments arguments);
    }

    internal class ParameterInfoDescriptor<T> : ParameterInfoDescriptor
    {
        public ParameterInfoDescriptor(ParameterInfo parameter) : base(parameter)
        {
            this.ScopedArgumentsType = typeof(IScopedArguments<T>);
            this.SingletonArgumentsType = typeof(ISingletonArguments<T>);

            this.ArgumentsTypes = new[]
            {
                this.ScopedArgumentsType,
                this.SingletonArgumentsType
            };
        }

        public Type ScopedArgumentsType { get; }

        public Type SingletonArgumentsType { get; }

        public Type[] ArgumentsTypes { get; }

        public override object ResolveArgument(IServiceProvider provider, OverrideArguments arguments)
        {
            var p = this;
            if (arguments.TryGetValue(this.Parameter.Name, out var value))
            {
                try
                {
                    return (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new InvalidOperationException();
                }
            }

            for (var i = 0; i < this.ArgumentsTypes.Length; i++)
            {
                if (provider.GetService(this.ArgumentsTypes[i]) is IArguments s &&
                    s.TryGetValue(p.Parameter.Name, out value))
                {
                    return value;
                }
            }

            if (p.Parameter.HasDefaultValue)
            {
                return p.Parameter.DefaultValue;
            }

            throw new InvalidOperationException();
        }
    }
}
