using JetBrains.Annotations;

namespace Jasily.Cache.Funcs
{
    public static class ObjectFunc
    {
        private static System.Func<object, bool> isNull;

        [NotNull]
        public static System.Func<object, bool> IsNull()
            => isNull ?? (isNull = o => ReferenceEquals(null, o));

        private static class C<T>
        {
            public static System.Func<T, bool> alwaysFalse;
            public static System.Func<T, bool> alwaysTrue;
            public static System.Func<T> alwaysDefault;
        }

        [NotNull]
        public static System.Func<T, bool> AlwaysFalse<T>()
            => C<T>.alwaysFalse ?? (C<T>.alwaysFalse = _ => false);

        [NotNull]
        public static System.Func<T, bool> AlwaysTrue<T>()
            => C<T>.alwaysTrue ?? (C<T>.alwaysTrue = _ => true);

        [NotNull]
        public static System.Func<TResult> AlwaysDefault<TResult>()
            => C<TResult>.alwaysDefault ?? (C<TResult>.alwaysDefault = () => default(TResult));

        private static class C<T, TResult>
        {
            public static System.Func<T, TResult> alwaysDefault;
        }

        [NotNull]
        public static System.Func<T, TResult> AlwaysDefault<T, TResult>()
            => C<T, TResult>.alwaysDefault ?? (C<T, TResult>.alwaysDefault = _ => default(TResult));
    }
}