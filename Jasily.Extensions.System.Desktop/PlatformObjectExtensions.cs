using System.Windows;
using System.Windows.Threading;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    public static class PlatformObjectExtensions
    {
        /// <summary>
        /// get UI thread Dispatcher
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        [NotNull]
        public static Dispatcher GetUIDispatcher<T>([CanBeNull] this T _)
            => Application.Current.Dispatcher;
    }
}