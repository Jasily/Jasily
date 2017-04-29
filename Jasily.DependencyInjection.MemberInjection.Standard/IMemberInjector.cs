using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MemberInjection
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMemberInjector<in T>
    {
        /// <summary>
        /// inject value from <see cref="IServiceProvider"/> to <paramref name="instance"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="instance"></param>
        /// <param name="isRequired"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> or <paramref name="instance"/> is null.</exception>
        void Inject([NotNull] IServiceProvider serviceProvider, [NotNull] T instance, bool isRequired);
    }
}
