using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jasily.Interfaces.Runtime.Serialization.Json;
using Jasily.Interfaces.Xml.Serialization;
using JetBrains.Annotations;

namespace Jasily.Net
{
    public static class WebResultExtensions
    {
        #region convert

        public static WebResult<string> AsText([NotNull] this WebResult<byte[]> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(ByteArrayExtensions.GetString);
        }

        public static WebResult<string> AsText([NotNull] this WebResult<byte[]> webResult,
            [NotNull] Encoding encoding)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            return webResult.Cast(z => z.GetString(encoding));
        }

        public static WebResult<T> AsXml<T>([NotNull] this WebResult<byte[]> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(XmlSerializerExtensions.XmlToObject<T>);
        }

        public static WebResult<T> AsJson<T>([NotNull] this WebResult<byte[]> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(JsonSerializerExtensions.JsonToObject<T>);
        }

        #region async

        public static async Task<WebResult<string>> AsText([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsText();
        }

        public static async Task<WebResult<string>> AsText([NotNull] this Task<WebResult<byte[]>> webResult,
            [NotNull] Encoding encoding)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsText(encoding);
        }

        public static async Task<WebResult<T>> AsXml<T>([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsXml<T>();
        }

        public static async Task<WebResult<T>> AsJson<T>([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsJson<T>();
        }

        #endregion

        private static byte[] AsBytes(Stream input)
            => input.ToArray();

        private static byte[] AsBytes(Stream input, CancellationToken cancellationToken)
            => input.ToArray(cancellationToken);

        #endregion

        #region base request

        #region GetResultAsync

        public static async Task<WebResult> GetResultAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                return new WebResult(await request.GetResponseAsync());
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }
        }

        public static async Task<WebResult> GetResultAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                return new WebResult(await request.GetResponseAsync(cancellationToken));
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }
        }

        public static async Task<WebResult<T>> GetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Func<Stream, T> selector)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            try
            {
                return await Task.Run(async () =>
                {
                    var response = await request.GetResponseAsync();
                    using (var stream = response.GetResponseStream())
                    {
                        return new WebResult<T>(response, selector(stream));
                    }
                });
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
            catch (IOException e) when (e.InnerException?.GetType().FullName == "System.Net.Sockets.SocketException")
            {
                if (Debugger.IsAttached) Debugger.Break();
                return new WebResult<T>(new WebException(e.InnerException.Message, e));
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw;
            }
        }

        public static async Task<WebResult<T>> GetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Func<Stream, CancellationToken, T> selector, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            try
            {
                return await Task.Run(async () =>
                {
                    var response = await request.GetResponseAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    using (var stream = response.GetResponseStream())
                    {
                        return new WebResult<T>(response, selector(stream, cancellationToken));
                    }
                }, cancellationToken);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
            catch (IOException e) when (e.InnerException?.GetType().FullName == "System.Net.Sockets.SocketException")
            {
                if (Debugger.IsAttached) Debugger.Break();
                return new WebResult<T>(new WebException(e.InnerException.Message, e));
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw;
            }
        }

        #endregion

        #region GetResultAsBytesAsync

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return await request.GetResultAsync(AsBytes);
        }

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return await request.GetResultAsync(AsBytes, cancellationToken);
        }

        #endregion

        #region SendAndGetResultAsync

        public static async Task<WebResult> SendAndGetResultAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            try
            {
                await request.SendAsync(input);
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }

            return await request.GetResultAsync();
        }

        public static async Task<WebResult> SendAndGetResultAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            try
            {
                await request.SendAsync(input, cancellationToken);
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }

            return await request.GetResultAsync(cancellationToken);
        }

        public static async Task<WebResult<T>> SendAndGetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, [NotNull] Func<Stream, T> selector)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            try
            {
                await request.SendAsync(input);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }

            return await request.GetResultAsync(selector);
        }

        public static async Task<WebResult<T>> SendAndGetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, [NotNull] Func<Stream, T> selector, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            try
            {
                await request.SendAsync(input, cancellationToken);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
            return await request.GetResultAsync<T>(selector);
        }

        #endregion

        #region SendAndGetResultAsBytesAsync

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            return await request.SendAndGetResultAsync(input, AsBytes);
        }

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            return await request.SendAndGetResultAsync(input, AsBytes, cancellationToken);
        }

        #endregion

        #endregion
    }
}