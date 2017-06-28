using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class ArgumentResolver<T> : IArgumentResolver<T>
    {
        private bool TryResolveArgument(IServiceProvider provider, ParameterInfo parameter, OverrideArguments arguments, out T value)
        {
            if (arguments.TryGetValue(parameter, out value, provider))
            {
                return true;
            }

            var scoped = provider.GetRequiredService<IScopedArguments<T>>();
            if (scoped.TryGetValue(parameter.Name, out value)) return true;

            var singleton = provider.GetRequiredService<ISingletonArguments<T>>();
            if (singleton.TryGetValue(parameter.Name, out value)) return true;

            var obj = provider.GetService(typeof(T));
            if (obj != null)
            {
                value = (T)obj;
                return true;
            }

            value = default(T);
            return false;
        }

        public object ResolveArgument(IServiceProvider provider, ParameterInfo parameter, OverrideArguments arguments, object defaultValue)
        {
            if (this.TryResolveArgument(provider, parameter, arguments, out var value)) return value;
            if (parameter.HasDefaultValue) return defaultValue;

            throw new ParameterResolveException(parameter);
        }

        public T ResolveArgument(IServiceProvider provider, ParameterInfo parameter, OverrideArguments arguments, T defaultValue)
        {
            if (this.TryResolveArgument(provider, parameter, arguments, out var value)) return value;
            if (parameter.HasDefaultValue) return defaultValue;

            throw new ParameterResolveException(parameter);
        }
    }
}