using JetBrains.Annotations;

namespace Jasily.Cache.Funcs
{
    public static class StringFunc
    {
        private static System.Func<string, bool> isNullOrEmpty;
        private static System.Func<string, bool> isNullOrWhiteSpace;

        [NotNull]
        public static System.Func<string, bool> IsNullOrEmpty()
            => isNullOrEmpty ?? (isNullOrEmpty = string.IsNullOrEmpty);

        [NotNull]
        public static System.Func<string, bool> IsNullOrWhiteSpace()
            => isNullOrWhiteSpace ?? (isNullOrWhiteSpace = string.IsNullOrWhiteSpace);
    }
}