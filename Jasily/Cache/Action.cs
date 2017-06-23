// ReSharper disable InconsistentNaming
namespace Jasily.Cache
{
    public static class Action
    {
        public static System.Action Empty() => C.empty;

        public static System.Action<T> Empty<T>() => C<T>.empty;

        public static System.Action<T1, T2> Empty<T1, T2>() => C<T1, T2>.empty;

        public static System.Action<T1, T2, T3> Empty<T1, T2, T3>() => C<T1, T2, T3>.empty;

        private static class C
        {
            public static readonly System.Action empty = () => { };
        }

        private static class C<T>
        {
            public static readonly System.Action<T> empty = _ => { };
        }

        public static class C<T1, T2>
        {
            public static readonly System.Action<T1, T2> empty = (_1, _2) => { };
        }

        public static class C<T1, T2, T3>
        {
            public static readonly System.Action<T1, T2, T3> empty = (_1, _2, _3) => { };
        }
    }
}