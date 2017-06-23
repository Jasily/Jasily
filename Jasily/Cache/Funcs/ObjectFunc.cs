using System;
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
        public static FuncTemplate<object, bool> IsNull { get; } = new FuncTemplate<object, bool>(o => ReferenceEquals(null, o));

        [NotNull]
        public static FuncTemplate<object, bool> IsNotNull { get; } = new FuncTemplate<object, bool>(o => !ReferenceEquals(null, o));

        [NotNull]
        public static FuncTemplate<T, bool> ReturnFalse<T>() => C<T>.ReturnFalseFunc;

        [NotNull]
        public static FuncTemplate<T, bool> ReturnTrue<T>() => C<T>.ReturnTrueFunc;

        [NotNull]
        public static System.Func<TResult> ReturnDefault<TResult>() => C<TResult>.ReturnDefaultFunc;

        [NotNull]
        public static FuncTemplate<T, TResult> ReturnDefault<T, TResult>() => C<T, TResult>.ReturnDefaultFunc;

        [NotNull]
        public static FuncTemplate<T, T> ReturnSelf<T>() => C<T>.ReturnSelfFunc;

        private static class C<T>
        {
            public static readonly Func<T> ReturnDefaultFunc = () => default(T);

            public static readonly FuncTemplate<T, bool> ReturnFalseFunc = new FuncTemplate<T, bool>(_ => false);
            public static readonly FuncTemplate<T, bool> ReturnTrueFunc = new FuncTemplate<T, bool>(_ => true);
            public static readonly FuncTemplate<T, T> ReturnSelfFunc = new FuncTemplate<T, T>(o => o);
        }

        private static class C<T, TResult>
        {
            public static readonly FuncTemplate<T, TResult> ReturnDefaultFunc = new FuncTemplate<T, TResult>(_ => default(TResult));
        }
    }
}