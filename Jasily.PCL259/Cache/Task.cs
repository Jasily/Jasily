using System.Threading.Tasks;
using T = System.Threading.Tasks.Task;

namespace Jasily.Cache
{
    public static class Task
    {
        public static Task<bool> True { get; } = T.FromResult(true);

        public static Task<bool> False { get; } = T.FromResult(false);
    }
}