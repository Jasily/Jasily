using System;

namespace Jasily.Extensions.System
{
    public static class StringComparisonExtensions
    {
        /// <summary>
        /// get <see cref="StringComparer"/> by <see cref="StringComparison"/>.
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static StringComparer GetStringComparer(this StringComparison comparison)
        {
            switch (comparison)
            {
                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;
                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;
                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;
                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;
                default:
                    throw new ArgumentOutOfRangeException(nameof(comparison), comparison, null);
            }
        }
    }
}