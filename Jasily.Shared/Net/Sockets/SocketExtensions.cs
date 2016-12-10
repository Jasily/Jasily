using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Net.Sockets
{
    public static class SocketExtensions
    {
        /// <summary>
        /// Exception info see Socket.SendAsync()
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<SocketError> TrySendAsync([NotNull] this Socket socket, [NotNull] SocketAsyncEventArgs args)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
            if (args == null) throw new ArgumentNullException(nameof(args));

            var tcs = new TaskCompletionSource<SocketError>();
            
            EventHandler<SocketAsyncEventArgs> onComplete = null;
            onComplete = (s, e) =>
            {
                args.Completed -= onComplete;
                tcs.SetResult(args.SocketError);
            };
            args.Completed += onComplete;

            if (!socket.SendAsync(args))
                return args.SocketError;
            else
                return await tcs.Task.ConfigureAwait(false);
        }
    }
}