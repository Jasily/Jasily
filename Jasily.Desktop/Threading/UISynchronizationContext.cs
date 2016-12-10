using System;
using System.Threading;
using System.Threading.Tasks;

#if WINDOWS_DESKTOP
using System.Windows;
#endif

#if  WINDOWS_UWP
using Windows.UI.Core;
using global::Windows.ApplicationModel.Core;
#endif

namespace Jasily.Threading
{
    public static class UISynchronizationContext
    {
        private static Task<SynchronizationContext> instance;

        public static Task<SynchronizationContext> GetForCurrentViewAsync()
        {
            if (instance != null) return instance;
            return InitializeAsync();
        }

        private static async Task<SynchronizationContext> InitializeAsync()
        {
#if WINDOWS_UWP || WINDOWS_DESKTOP
            SynchronizationContext sc = null;
#if WINDOWS_UWP
            var dispatcher = (CoreApplication.GetCurrentView() ?? CoreApplication.MainView)?.Dispatcher;
            if (dispatcher == null) throw new NotSupportedException();
            if (dispatcher.HasThreadAccess)
            {
                sc = SynchronizationContext.Current;
            }
            else
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () => sc = SynchronizationContext.Current);
            }
#elif WINDOWS_DESKTOP
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                sc = SynchronizationContext.Current;
            }
            else
            {
                await dispatcher.InvokeAsync(() => sc = SynchronizationContext.Current);
            }
#endif
            instance = Task.FromResult(sc);
            return sc;

#else
            throw new NotSupportedException();
#endif
        }
    }
}