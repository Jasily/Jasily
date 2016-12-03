using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class EnumerableAsyncExtensions
    {
        #region all or any

        public static async Task<bool> AllAsync<TSource>([NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, Task<bool>> predicateAsync)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicateAsync == null) throw new ArgumentNullException(nameof(predicateAsync));
            foreach (var item in source)
            {
                if (!await predicateAsync(item).ConfigureAwait(false)) return false;
            }
            return true;
        }

        public static async Task<bool> AnyAsync<TSource>([NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, Task<bool>> predicateAsync)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicateAsync == null) throw new ArgumentNullException(nameof(predicateAsync));
            foreach (var item in source)
            {
                if (await predicateAsync(item).ConfigureAwait(false)) return true;
            }
            return false;
        }

        #endregion
    }
}