using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MemberInjection
{
    public interface IMemberInjectorFactory<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="property"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IMemberInjector<T> GetMemberInjector([NotNull] PropertyInfo property);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="field"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IMemberInjector<T> GetMemberInjector([NotNull] FieldInfo field);
    }
}
