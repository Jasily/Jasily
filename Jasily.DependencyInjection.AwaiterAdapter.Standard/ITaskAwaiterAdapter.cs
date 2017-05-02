using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITaskAwaiterAdapter
    {
        /// <summary>
        /// whether is adapter is valid or NOT
        /// </summary>
        bool IsAwaitable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        bool IsCompleted([NotNull] object instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        object GetResult([NotNull] object instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="continuation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <exception cref="InvalidCastException"></exception>
        void OnCompleted([NotNull] object instance, [NotNull]  Action continuation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    public interface ITaskAwaiterAdapter<in TTask> : ITaskAwaiterAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <returns></returns>
        bool IsCompleted([NotNull] TTask instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        void GetResult([NotNull] TTask instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="continuation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        void OnCompleted([NotNull] TTask instance, [NotNull]  Action continuation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface ITaskAwaiterAdapter<in TTask, out TResult> : ITaskAwaiterAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <returns></returns>
        bool IsCompleted([NotNull] TTask instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <returns></returns>
        TResult GetResult([NotNull] TTask instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="continuation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        void OnCompleted([NotNull] TTask instance, [NotNull]  Action continuation);
    }
}
