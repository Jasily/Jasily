using JetBrains.Annotations;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace System
{
    public static class PlatformObjectExtensions
    {
        [NotNull]
        public static CoreDispatcher GetUIDispatcher<T>([CanBeNull] this T _)
            => (CoreApplication.GetCurrentView() ?? CoreApplication.MainView).Dispatcher;
    }
}