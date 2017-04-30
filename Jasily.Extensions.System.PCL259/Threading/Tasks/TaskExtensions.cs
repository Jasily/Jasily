using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static void EnsureStarted([NotNull] this Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == TaskStatus.Created) task.Start();
        }

        public static void EnsureStarted([NotNull] this Task task, [NotNull] TaskScheduler scheduler)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));
            if (task.Status == TaskStatus.Created) task.Start(scheduler);
        }

        /// <summary>
        /// return await Task.Run(async () => selector(await task));
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="task"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async ValueTask<TTo> SelectAsync<TFrom, TTo>([NotNull] this Task<TFrom> task,
            [NotNull] Func<TFrom, TTo> selector)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return await Task.Run(async () => 
                selector(await task.ConfigureAwait(false))
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="task"></param>
        /// <param name="asyncSelector"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static async ValueTask<TTo> AsyncSelectAsync<TFrom, TTo>([NotNull] this Task<TFrom> task,
            [NotNull] Func<TFrom, Task<TTo>> asyncSelector)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (asyncSelector == null) throw new ArgumentNullException(nameof(asyncSelector));
            return await Task.Run(async () =>
                await asyncSelector(await task.ConfigureAwait(false)).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// create <see cref="ValueTask{TResult}"/> from <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ValueTask<TResult> AsTaskResult<TResult>([CanBeNull] this TResult value)
        {
            return new ValueTask<TResult>(value);
        }
    }
}