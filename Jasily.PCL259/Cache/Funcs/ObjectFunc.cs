using JetBrains.Annotations;

namespace Jasily.Cache.Funcs
{
    /// <summary>
    /// provide a lot cached func instance for used.
    /// Save your GC.
    /// </summary>
    public static class ObjectFunc
    {
        [NotNull]
        public static System.Func<object, bool> IsNull() => C.IsNull ?? (C.IsNull = o => ReferenceEquals(null, o));

        [NotNull]
        public static System.Func<T, bool> AlwaysFalse<T>()
            => C<T>.AlwaysFalse ?? (C<T>.AlwaysFalse = _ => false);

        [NotNull]
        public static System.Func<T, bool> AlwaysTrue<T>()
            => C<T>.AlwaysTrue ?? (C<T>.AlwaysTrue = _ => true);

        [NotNull]
        public static System.Func<TResult> AlwaysDefault<TResult>()
            => C<TResult>.AlwaysDefault ?? (C<TResult>.AlwaysDefault = () => default(TResult));

        [NotNull]
        public static System.Func<T, TResult> AlwaysDefault<T, TResult>()
            => C<T, TResult>.AlwaysDefault ?? (C<T, TResult>.AlwaysDefault = _ => default(TResult));

        [NotNull]
        public static System.Func<T, T> ReturnSelf<T>()
            => C<T>.ReturnSelf ?? (C<T>.ReturnSelf = z => z);

        private static class C
        {
            public static System.Func<object, bool> IsNull;
        }

        private static class C<T>
        {
            public static System.Func<T, bool> AlwaysFalse;
            public static System.Func<T, bool> AlwaysTrue;
            public static System.Func<T> AlwaysDefault;
            public static System.Func<T, T> ReturnSelf;
        }

        private static class C<T, TResult>
        {
            public static System.Func<T, TResult> AlwaysDefault;
        }
    }
}