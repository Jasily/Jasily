using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.ViewManagement;
using JetBrains.Annotations;

namespace Windows.ApplicationModel.Core
{
    public static class CoreApplicationViewExtensions
    {
        public static DataTransferManager GetDataTransferManager([NotNull] CoreApplicationView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            return DataTransferManager.GetForCurrentView();
        }

        public static ApplicationView GetApplicationView([NotNull] CoreApplicationView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            return ApplicationView.GetForCurrentView();
        }
    }
}
