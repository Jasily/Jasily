using System;
using Windows.ApplicationModel.DataTransfer;
using JetBrains.Annotations;

namespace Windows.UI.ViewManagement
{
    public static class ApplicationViewExtensions
    {
        public static DataTransferManager GetDataTransferManager([NotNull] ApplicationView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            return DataTransferManager.GetForCurrentView();
        }
    }
}
