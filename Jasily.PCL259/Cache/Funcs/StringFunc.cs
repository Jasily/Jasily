using JetBrains.Annotations;

namespace Jasily.Cache.Funcs
{
    public static class StringFunc
    {
        private static System.Func<string, bool> isNullOrEmpty;
        private static System.Func<string, bool> isNullOrWhiteSpace;
        private static System.Func<string, int> length;

        [NotNull]
        public static System.Func<string, bool> IsNullOrEmpty()
            => isNullOrEmpty ?? (isNullOrEmpty = string.IsNullOrEmpty);

        [NotNull]
        public static System.Func<string, bool> IsNullOrWhiteSpace()
            => isNullOrWhiteSpace ?? (isNullOrWhiteSpace = string.IsNullOrWhiteSpace);

        public static System.Func<string, int> Length()
            => length ?? (length = s => s.Length);
    }
}