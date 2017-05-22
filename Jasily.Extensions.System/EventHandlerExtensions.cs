using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    public static class EventHandlerExtensions
    {
        public static void Invoke([NotNull] this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke([NotNull] this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        #region async

        public static Task InvokeAsync([NotNull] this EventHandler e, object sender, EventArgs arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return Task.Run(() => e.Invoke(sender, arg));
        }

        public static Task InvokeAsync([NotNull] this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        public static Task InvokeAsync([NotNull] this EventHandler e, object sender,
            [NotNull] IEnumerable<EventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        public static Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender, T arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return Task.Run(() => e.Invoke(sender, arg));
        }

        public static Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        public static Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender,
            [NotNull] IEnumerable<T> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        #endregion

        #region begin

        public static void BeginInvoke(this EventHandler e, object sender, EventArgs arg)
            => InvokeAsync(e, sender, arg);

        public static void BeginInvoke(this EventHandler e, object sender, params EventArgs[] args)
            => InvokeAsync(e, sender, args);

        public static void BeginInvoke(this EventHandler e, object sender, IEnumerable<EventArgs> args)
            => InvokeAsync(e, sender, args);

        public static void BeginInvoke<T>(this EventHandler<T> e, object sender)
            => InvokeAsync(e, sender);

        public static void BeginInvoke<T>(this EventHandler<T> e, object sender, T arg)
            => InvokeAsync(e, sender, arg);

        public static void BeginInvoke<T>(this EventHandler<T> e, object sender, params T[] args)
            => InvokeAsync(e, sender, args);

        public static void BeginInvoke<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
            => InvokeAsync(e, sender, args);

        #endregion

        #region fire

        /*
         * Only task need fire. For other, use e?.invoke(). 
         */

        public static async Task FireAsync(this EventHandler e, object sender, EventArgs arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync(this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync(this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, T arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        #endregion
    }
}