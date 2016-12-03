
namespace Jasily.Cache
{
    public static class Func<TResult>
    {
        public static System.Func<TResult> Default { get; } = () => default(TResult);
    }

    public static class Func<T, TResult>
    {
        public static System.Func<T, TResult> Default { get; } = _ => default(TResult);
    }
}