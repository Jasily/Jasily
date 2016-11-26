using System.Threading.Tasks;

namespace Jasily.Cache
{
    public static class TaskCache
    {
        public static Task<bool> True { get; } = Task.FromResult(true);

        public static Task<bool> False { get; } = Task.FromResult(false);
    }
}