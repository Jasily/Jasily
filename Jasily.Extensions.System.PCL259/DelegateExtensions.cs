using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System
{
    public static class DelegateExtensions
    {
        public static Task InvokeAsync([NotNull] this Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            return Task.Run(() => action.Invoke());
        }

        public static void BeginInvoke([NotNull] this Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            InvokeAsync(action);
        }

        public static Task<T> InvokeAsync<T>([NotNull] this Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return Task.Run(() => func.Invoke());
        }

        public static void BeginInvoke<T>([NotNull] this Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            InvokeAsync(func);
        }
    }
}