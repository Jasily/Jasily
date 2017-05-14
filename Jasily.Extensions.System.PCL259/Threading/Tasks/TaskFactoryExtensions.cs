using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Threading.Tasks
{
    public static class TaskFactoryExtensions
    {
        public static Task<T> FromAsync<T>([CanBeNull] this TaskFactory<T> factory,
            [NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, T> endMethod,
            CancellationToken cancellationToken) => TaskFactoryHelper.FromAsync(beginMethod, endMethod, cancellationToken);

        public static Task<T> FromAsync<T>([CanBeNull] this TaskFactory<T> factory,
            [NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, CancellationToken, T> endMethod,
            CancellationToken cancellationToken) => TaskFactoryHelper.FromAsync(beginMethod, endMethod, cancellationToken);

        public static Task<T> FromAsync<T>([CanBeNull] this TaskFactory<T> factory,
            [NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, T> endMethod,
            CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback) => TaskFactoryHelper.FromAsync(beginMethod, endMethod, cancellationToken, cancelingCallback);

        public static Task FromAsync([CanBeNull] this TaskFactory factory,
            [NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult> endMethod,
            CancellationToken cancellationToken) => TaskFactoryHelper.FromAsync(beginMethod, endMethod, cancellationToken);

        public static Task FromAsync([CanBeNull] this TaskFactory factory,
            [NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult, CancellationToken> endMethod,
            CancellationToken cancellationToken) => TaskFactoryHelper.FromAsync(beginMethod, endMethod, cancellationToken);

        public static Task FromAsync([CanBeNull] this TaskFactory factory,
            [NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult> endMethod,
            CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback) => TaskFactoryHelper.FromAsync(beginMethod, endMethod, cancellationToken, cancelingCallback);
    }
}