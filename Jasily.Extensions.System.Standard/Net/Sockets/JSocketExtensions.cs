using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Net.Sockets
{
    /// <summary>
    /// extensions for <see cref="Socket"/>.
    /// </summary>
    public static class JSocketExtensions
    {
        /// <summary>
        /// Exception info see Socket.SendAsync()
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async ValueTask<SocketError> TrySendAsync([NotNull] this Socket socket, [NotNull] SocketAsyncEventArgs args)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
            if (args == null) throw new ArgumentNullException(nameof(args));
            
            var tcs = new TaskCompletionSource<SocketError>();

            void OnComplete(object s, SocketAsyncEventArgs e)
            {
                args.Completed -= OnComplete;
                tcs.SetResult(args.SocketError);
            }

            args.Completed += OnComplete;
            
            if (!socket.SendAsync(args))
                return args.SocketError;
            else
                return await tcs.Task.ConfigureAwait(false);
        }
    }
}