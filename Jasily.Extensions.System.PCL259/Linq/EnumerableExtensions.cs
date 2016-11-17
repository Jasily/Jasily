using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// CancellationToken support for enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <param name="checkCycle"></param>
        /// <returns></returns>
        public static IEnumerable<T> EnumerateWithToken<T>([NotNull] this IEnumerable<T> source,
            CancellationToken token, int checkCycle = 30)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (checkCycle <= 0) throw new ArgumentOutOfRangeException(nameof(checkCycle));

            return EnumerateWithTokenIterator(source, token, checkCycle);
        }

        private static IEnumerable<T> EnumerateWithTokenIterator<T>([NotNull] this IEnumerable<T> source,
            CancellationToken token, int checkCycle)
        {
            Debug.Assert(source != null);
            Debug.Assert(checkCycle > 0);

            using (var enumerator = source.GetEnumerator())
            {
                if (checkCycle == 1)
                {
                    while (enumerator.MoveNext())
                    {
                        token.ThrowIfCancellationRequested();
                        yield return enumerator.Current;
                    }
                }
                else
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        for (var i = 0u; i < checkCycle; i++)
                        {
                            if (enumerator.MoveNext()) yield return enumerator.Current;
                            else yield break;
                        }
                    }
                }
            }
        }
    }
}