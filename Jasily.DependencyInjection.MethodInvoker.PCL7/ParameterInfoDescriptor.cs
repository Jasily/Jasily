using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class ParameterInfoDescriptor
    {
        public ParameterInfoDescriptor(ParameterInfo parameter)
        {
            this.Parameter = parameter;

            this.TransientArgumentType = typeof(ITransientArguments<>).MakeGenericType(parameter.ParameterType);
            this.ScopedArgumentsType = typeof(IScopedArguments<>).MakeGenericType(parameter.ParameterType);
            this.SingletonArgumentsType = typeof(ISingletonArguments<>).MakeGenericType(parameter.ParameterType);

            this.ArgumentsTypes = new[]
            {
                this.TransientArgumentType,
                this.ScopedArgumentsType,
                this.SingletonArgumentsType
            };
        }

        public ParameterInfo Parameter { get; }

        public Type TransientArgumentType { get; }

        public Type ScopedArgumentsType { get; }

        public Type SingletonArgumentsType { get; }

        public Type[] ArgumentsTypes { get; }

        public object ResolveArgument(IServiceProvider provider, OverrideArguments arguments)
        {
            var p = this;
            if (arguments.TryGetValue(this.Parameter.Name, out var value))
            {
                return value;
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
