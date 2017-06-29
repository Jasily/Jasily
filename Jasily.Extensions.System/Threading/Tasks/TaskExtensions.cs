using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Threading.Tasks
{
    /// <summary>
    /// Extension methods for <see cref="Task"/>
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Ensure task start. If not, start it.
        /// </summary>
        /// <param name="task"></param>
        public static void EnsureStarted([NotNull] this Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == TaskStatus.Created) task.Start();
        }

        /// <summary>
        /// Ensure task start. If not, start it.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        public static void EnsureStarted([NotNull] this Task task, [NotNull] TaskScheduler scheduler)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));
            if (task.Status == TaskStatus.Created) task.Start(scheduler);
        }

        /// <summary>
        /// return selector(<see langword="await"/> task.ConfigureAwait(<see langword="false"/>));
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TNewResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<TNewResult> SelectAsync<TResult, TNewResult>([NotNull] this Task<TResult> task,
            [NotNull] Func<TResult, TNewResult> selector)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return selector(await task.ConfigureAwait(false));
        }

        /// <summary>
        /// return <see langword="await"/> taskSelector(<see langword="await"/> 
        /// task.ConfigureAwait(<see langword="false"/>)).ConfigureAwait(<see langword="false"/>);
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TNewResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="taskSelector"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static async Task<TNewResult> SelectTaskAsync<TResult, TNewResult>([NotNull] this Task<TResult> task,
            [NotNull] Func<TResult, Task<TNewResult>> taskSelector)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (taskSelector == null) throw new ArgumentNullException(nameof(taskSelector));
            return await taskSelector(await task.ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}