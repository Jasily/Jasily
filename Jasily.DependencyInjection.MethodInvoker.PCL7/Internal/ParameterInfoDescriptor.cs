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

        public override object ResolveArgumentObject(IServiceProvider provider, OverrideArguments arguments)
        {
            var p = this;
            if (arguments.TryGetValue<T>(this.Parameter.Name, out var value))
            {
                return value;
            }

            for (var i = 0; i < this.ArgumentsTypes.Length; i++)
            {
                if (provider.GetService(this.ArgumentsTypes[i]) is IArguments<T> args &&
                    args.TryGetValue(p.Parameter.Name, out value))
                {
                    return value;
                }
            }

            if (p.Parameter.HasDefaultValue) return p.Parameter.DefaultValue;

            throw new InvalidOperationException();
        }

        public T ResolveArgumentValue(IServiceProvider provider, OverrideArguments arguments)
        {
            if (arguments.TryGetValue<T>(this.Parameter.Name, out var value))
            {
                return value;
            }

            for (var i = 0; i < this.ArgumentsTypes.Length; i++)
            {
                if (provider.GetService(this.ArgumentsTypes[i]) is IArguments<T> args &&
                    args.TryGetValue(this.Parameter.Name, out value))
                {
                    return value;
                }
            }

            if (this.Parameter.HasDefaultValue) return this.DefaultValue;

            throw new InvalidOperationException();
        }
    }
}
