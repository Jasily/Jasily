using System.Threading.Tasks;
using TASK = System.Threading.Tasks.Task;

namespace Jasily.Cache
{
    public static class Task
    {
        public static Task<object> Null { get; } = TASK.FromResult<object>(null);

        public static Task<bool> True { get; } = TASK.FromResult(true);

        public static Task<bool> False { get; } = TASK.FromResult(false);

        private static class C<T>
        {
            public static readonly Task<T> @default = TASK.FromResult<T>(default(T));
        }

        public static Task<T> Default<T>() => C<T>.@default;
    }
}