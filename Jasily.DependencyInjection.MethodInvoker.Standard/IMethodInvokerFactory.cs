using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMethodInvokerFactory
    {
        /// <summary>
        /// all available constructors.
        /// </summary>
        IReadOnlyList<ConstructorInfo> Constructors { get; }

        /// <summary>
        /// all available methods.
        /// </summary>
        IReadOnlyList<MethodInfo> Methods { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IStaticMethodInvoker GetStaticMethodInvoker([NotNull] MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructor"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="constructor"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IStaticMethodInvoker GetConstructorInvoker([NotNull] ConstructorInfo constructor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IObjectMethodInvoker GetObjectMethodInvoker([NotNull] MethodInfo method);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMethodInvokerFactory<in T> : IMethodInvokerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IInstanceMethodInvoker<T> GetInstanceMethodInvoker([NotNull] MethodInfo method);
    }
}
