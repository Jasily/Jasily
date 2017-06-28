using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// Allow to override resolve arguments logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IArgumentResolver<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="parameter"></param>
        /// <param name="arguments"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ParameterResolveException"></exception>
        object ResolveArgument([NotNull] IServiceProvider provider, [NotNull] ParameterInfo parameter,
            OverrideArguments arguments, object defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="parameter"></param>
        /// <param name="arguments"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ParameterResolveException"></exception>
        T ResolveArgument([NotNull] IServiceProvider provider, [NotNull] ParameterInfo parameter,
            OverrideArguments arguments, T defaultValue);
    }
}