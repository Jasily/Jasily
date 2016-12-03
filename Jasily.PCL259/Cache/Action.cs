namespace Jasily.Cache
{
    public static class Action
    {
        public static System.Action Empty { get; } = () => { };
    }

    public static class Action<T>
    {
        public static System.Action<T> Empty { get; } = _ => { };
    }


}