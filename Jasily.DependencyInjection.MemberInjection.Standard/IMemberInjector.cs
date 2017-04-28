using System;

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
        /// <param name="instance"></param>
        /// <param name="isRequired"></param>
        void Inject(T instance, bool isRequired);
    }
}
