using System;
using System.Threading.Tasks;

namespace Jasily
{
    public static class GlobalEvent<T, TEventArgs>
    {
        public static TypedEventHandler<T, TEventArgs> OnEventRaised;

        public static void Raise(T obj, TEventArgs args)
        {
            OnEventRaised?.Invoke(obj, args);
            GlobalEvent<TEventArgs>.Raise(obj, args);
        }

        public static void BeginRaise(T obj, TEventArgs args)
        {
            OnEventRaised?.BeginInvoke(obj, args);
            GlobalEvent<TEventArgs>.BeginRaise(obj, args);
        }

        public static async Task RaiseAsync(T obj, TEventArgs args)
        {
            await OnEventRaised.FireAsync(obj, args);
            await GlobalEvent<TEventArgs>.RaiseAsync(obj, args);
        }
    }

    public static class GlobalEvent<TEventArgs>
    {
        public static EventHandler<TEventArgs> OnEventRaised;

        public static void Raise(object obj, TEventArgs args) => OnEventRaised?.Invoke(obj, args);

        public static void BeginRaise(object obj, TEventArgs args) => OnEventRaised?.BeginInvoke(obj, args);

        public static Task RaiseAsync(object obj, TEventArgs args) => OnEventRaised.FireAsync(obj, args);
    }
}