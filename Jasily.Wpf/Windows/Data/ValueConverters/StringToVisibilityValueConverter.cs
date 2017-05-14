using System;
using System.Windows;
using Jasily.Wpf.Windows.Data.ValueConverters.Internal;
#if WINDOWS_DESKTOP

#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Jasily.UI.Xaml.Data.ValueConverters.Internal;
#endif

#if WINDOWS_DESKTOP
namespace Jasily.Wpf.Windows.Data.ValueConverters
#elif WINDOWS_UWP
namespace Jasily.UI.Xaml.Data.ValueConverters
#endif
{
    public class StringToVisibilityValueConverter : InvariantValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter)
        {
            var boolValue = parameter as string == nameof(string.IsNullOrWhiteSpace)
                ? string.IsNullOrWhiteSpace(value as string)
                : string.IsNullOrEmpty(value as string);
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}