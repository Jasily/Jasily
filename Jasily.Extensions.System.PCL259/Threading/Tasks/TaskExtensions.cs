using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static void EnsureStarted([NotNull] this Task task, TaskScheduler scheduler = null)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            if (task.Status == TaskStatus.Created)
            {
                if (scheduler == null)
                {
                    task.Start();
                }
                else
                {
                    task.Start(scheduler);
                }
            }
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
        public static async Task<TTo> SelectAsync<TFrom, TTo>([NotNull] this Task<TFrom> task,
            [NotNull] Func<TFrom, TTo> selector)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return await Task.Run(async () => selector(await task)).ConfigureAwait(false);
        }
    }
}