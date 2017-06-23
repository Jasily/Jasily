using JetBrains.Annotations;

namespace Jasily.Cache.Funcs
{
    public static class StringFunc
    {
        [NotNull]
        public static FuncTemplate<string, bool> IsNullOrEmpty { get; } = new FuncTemplate<string, bool>(string.IsNullOrEmpty);

        [NotNull]
        public static FuncTemplate<string, bool> IsNullOrWhiteSpace { get; } = new FuncTemplate<string, bool>(string.IsNullOrWhiteSpace);

        [NotNull]
        public static FuncTemplate<string, int> Length { get; } = new FuncTemplate<string, int>(s => s.Length);
    }
}