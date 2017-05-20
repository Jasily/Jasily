using JetBrains.Annotations;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// extension methods for the module.
    /// </summary>
    public static class MethodInvokerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="factory"/>
        /// or <paramref name="instance"/> 
        /// or <paramref name="method"/>
        /// or <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeInstanceMethod<T>([NotNull] this IMethodInvokerFactory<T> factory,
            [NotNull] MethodInfo method, [NotNull] T instance, [NotNull] IServiceProvider serviceProvider,
            OverrideArguments arguments = default(OverrideArguments))
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                return factory.GetInstanceMethodInvoker(method).Invoke(instance, scope.ServiceProvider, arguments);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="method"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="factory"/> 
        /// or <paramref name="method"/> 
        /// or <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeStaticMethod<T>([NotNull] this IMethodInvokerFactory<T> factory,
            [NotNull] MethodInfo method, [NotNull] IServiceProvider serviceProvider,
            OverrideArguments arguments = default(OverrideArguments))
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                return factory.GetStaticMethodInvoker(method).Invoke(scope.ServiceProvider, arguments);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="constructor"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="factory"/> 
        /// or <paramref name="constructor"/> 
        /// or <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static T InvokeConstructor<T>([NotNull] this IMethodInvokerFactory<T> factory,
            [NotNull] ConstructorInfo constructor, [NotNull] IServiceProvider serviceProvider,
            OverrideArguments arguments = default(OverrideArguments))
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                return factory.GetConstructorInvoker(constructor).HasResult<T>().Invoke(scope.ServiceProvider, arguments);
            }
        }

        #region default(OverrideArguments)

        /// <summary>
        /// invoke object method.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static object Invoke([NotNull] this IObjectMethodInvoker invoker,
            [NotNull] object instance, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(instance, serviceProvider, default(OverrideArguments));
        }

        /// <summary>
        /// invoke instance method.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static TResult Invoke<T, TResult>([NotNull] this IInstanceMethodInvoker<T, TResult> invoker,
            [NotNull] T instance, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(instance, serviceProvider, default(OverrideArguments));
        }

        /// <summary>
        /// invoke instance method.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static object Invoke<T>([NotNull] this IInstanceMethodInvoker<T> invoker,
            [NotNull] T instance, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(instance, serviceProvider, default(OverrideArguments));
        }

        /// <summary>
        /// invoke static method.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="invoker"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static TResult Invoke<TResult>([NotNull] this IStaticMethodInvoker<TResult> invoker, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(serviceProvider, default(OverrideArguments));
        }

        /// <summary>
        /// invoke static method.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="invoker"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static object Invoke([NotNull] this IStaticMethodInvoker invoker, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(serviceProvider, default(OverrideArguments));
        }

        #endregion

        #region cast

        /// <summary>
        /// Specific that method has return value (<typeparamref name="TResult"/>). 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static IInstanceMethodInvoker<T, TResult> HasResult<T, TResult>([NotNull] this IInstanceMethodInvoker<T> invoker)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return (IInstanceMethodInvoker<T, TResult>) invoker;
        }

        /// <summary>
        /// Specific that method has return value (<typeparamref name="TResult"/>).
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static IStaticMethodInvoker<TResult> HasResult<TResult>([NotNull] this IStaticMethodInvoker invoker)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return (IStaticMethodInvoker<TResult>)invoker;
        }

        #endregion

        /// <summary>
        /// provide full API explore.
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
