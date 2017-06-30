using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MemberInjection
{
    /// <summary>
    /// provide instance injector interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInstanceInjector<in T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> or <paramref name="instance"/> is null.</exception>
        /// <param name="serviceProvider"></param>
        /// <param name="instance"></param>
        void Inject([NotNull] IServiceProvider serviceProvider, [NotNull]  T instance);
    }
}
