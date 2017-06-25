using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    /// <summary>
    /// Extension methods for <see cref="EventHandler"/>
    /// </summary>
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public static class EventHandlerExtensions
    {
        #region invoke

        /// <summary>
        /// Invoke use argument <see cref="EventArgs.Empty"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Invoke([NotNull] this EventHandler e, object sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            e.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Invoke one by one.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Invoke([NotNull] this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            for (var index = 0; index < args.Length; index++)
            {
                e.Invoke(sender, args[index]);
            }
        }

        /// <summary>
        /// Invoke one by one.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Invoke([NotNull] this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        /// <summary>
        /// Invoke use argument <see langword="default"/>(T).
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            e.Invoke(sender, default(T));
        }

        /// <summary>
        /// Invoke one by one.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            for (var index = 0; index < args.Length; index++)
            {
                e.Invoke(sender, args[index]);
            }
        }

        /// <summary>
        /// Invoke one by one.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        #endregion

        #region async

        /// <summary>
        /// Invoke on thread pool, use argument <see cref="EventArgs.Empty"/>..
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync([NotNull] this EventHandler e, object sender)
        {
            return InvokeAsync(e, sender, EventArgs.Empty);
        }

        /// <summary>
        /// Invoke on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync([NotNull] this EventHandler e, object sender, EventArgs arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return Task.Run(() => e.Invoke(sender, arg));
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync([NotNull] this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync([NotNull] this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        /// <summary>
        /// Invoke on thread pool, use argument <see langword="default"/>(T).
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return Task.Run(() => e.Invoke(sender, default(T)));
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender, T arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return Task.Run(() => e.Invoke(sender, arg));
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            return Task.Run(() => e.Invoke(sender, args));
        }

        #endregion

        #region fire (only task need fire. For other, use e?.invoke())

        /// <summary>
        /// Invoke on thread pool, use argument <see cref="EventArgs.Empty"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        [NotNull]
        public static async Task FireAsync([CanBeNull] this EventHandler e, object sender)
        {
            var invokeAsync = e?.InvokeAsync(sender);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        [NotNull]
        public static async Task FireAsync([CanBeNull] this EventHandler e, object sender, EventArgs arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="args"/> is null.</exception>
        [NotNull]
        public static async Task FireAsync([CanBeNull] this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="args"/> is null.</exception>
        [NotNull]
        public static async Task FireAsync([CanBeNull] this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke on thread pool, use argument <see langword="default"/>(T).
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        [NotNull]
        public static async Task FireAsync<T>([CanBeNull] this EventHandler<T> e, object sender)
        {
            var invokeAsync = e?.InvokeAsync(sender);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke on thread pool.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        [NotNull]
        public static async Task FireAsync<T>([CanBeNull] this EventHandler<T> e, object sender, T arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="args"/> is null.</exception>
        [NotNull]
        public static async Task FireAsync<T>([CanBeNull] this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke one by one on thread pool.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="args"/> is null.</exception>
        [NotNull]
        public static async Task FireAsync<T>([CanBeNull] this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync.ConfigureAwait(false);
        }

        #endregion
    }
}