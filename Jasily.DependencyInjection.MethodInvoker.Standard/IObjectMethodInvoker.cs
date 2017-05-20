using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// 
    /// </summary>
    public interface IObjectMethodInvoker
    {
        /// <summary>
        /// invoke instance method.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        object Invoke([NotNull] object instance, [NotNull]  IServiceProvider serviceProvider, OverrideArguments arguments);
    }
}