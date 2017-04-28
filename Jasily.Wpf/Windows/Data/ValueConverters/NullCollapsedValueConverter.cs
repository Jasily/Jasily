using System;

#if WINDOWS_DESKTOP
using System.Windows;
using Jasily.Windows.Data.ValueConverters.Internal;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Jasily.UI.Xaml.Data.ValueConverters.Internal;
#endif

#if WINDOWS_DESKTOP
namespace Jasily.Windows.Data.ValueConverters
#elif WINDOWS_UWP
namespace Jasily.UI.Xaml.Data.ValueConverters
#endif
{
    public class NullCollapsedValueConverter : InvariantValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter)
            => ReferenceEquals(value, null) ? Visibility.Collapsed : Visibility.Visible;
    }
}