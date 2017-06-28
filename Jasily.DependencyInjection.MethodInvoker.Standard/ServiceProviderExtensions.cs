using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// Extension methods for <see cref="IServiceProvider"/>
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Full API for <see cref="MethodInvoker"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static MethodInvokerProvider AsMethodInvokerProvider([NotNull] this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            return new MethodInvokerProvider(serviceProvider);
        }
    }
}