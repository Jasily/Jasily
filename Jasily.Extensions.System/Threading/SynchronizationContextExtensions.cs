using System;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Threading
{
    public static class SynchronizationContextExtensions
    {
        public static void Send([NotNull] this SynchronizationContext context, [NotNull] Action action)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Send(_ => action(), null);
        }

        public static void Send<T>([NotNull] this SynchronizationContext context, [NotNull] Action<T> action, T state)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Send(_ => action(state), null);
        }

        public static void Send([NotNull] this SynchronizationContext context, [NotNull] EventHandler handler,
            object sender, EventArgs e)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            context.Send(_ => handler(sender, e), null);
        }

        public static void Send<T>([NotNull] this SynchronizationContext context, [NotNull] EventHandler<T> handler,
            object sender, T state)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            context.Send(_ => handler(sender, state), null);
        }

        public static void Post([NotNull] this SynchronizationContext context, [NotNull] Action action)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Post(_ => action(), null);
        }

        public static void Post<T>([NotNull] this SynchronizationContext context, [NotNull] Action<T> action, T state)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Post(_ => action(state), null);
        }

        public static void Post([NotNull] this SynchronizationContext context, [NotNull] EventHandler handler,
            object sender, EventArgs e)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            context.Post(_ => handler(sender, e), null);
        }

        public static void Post<T>([NotNull] this SynchronizationContext context, [NotNull] EventHandler<T> handler,
            object sender, T state)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            context.Post(_ => handler(sender, state), null);
        }
    }
}