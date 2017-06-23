using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily
{
    public static class TypedEventHandlerExtensions
    {
        public static void Invoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] IEnumerable<TEventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        #region async

        public static Task InvokeAsync<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return Task.Run(() => e.Invoke(sender, arg));
        }

        public static Task InvokeAsync<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] params TEventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        public static Task InvokeAsync<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] IEnumerable<TEventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        #endregion

        #region begin

        public static void BeginInvoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg)
            => InvokeAsync(e, sender, arg);

        public static void BeginInvoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, params TEventArgs[] args)
            => InvokeAsync(e, sender, args);

        public static void BeginInvoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, IEnumerable<TEventArgs> args)
            => InvokeAsync(e, sender, args);

        #endregion

        #region fire

        /*
         * Only task need fire. For other, use e?.invoke(). 
         */

        public static async Task FireAsync<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] params TEventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] IEnumerable<TEventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        #endregion
    }
}