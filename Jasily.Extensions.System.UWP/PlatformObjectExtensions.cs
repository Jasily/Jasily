using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    public static class PlatformObjectExtensions
    {
        [NotNull]
        public static CoreDispatcher GetUIDispatcher<T>([CanBeNull] this T _)
            => (CoreApplication.GetCurrentView() ?? CoreApplication.MainView).Dispatcher;
    }
}