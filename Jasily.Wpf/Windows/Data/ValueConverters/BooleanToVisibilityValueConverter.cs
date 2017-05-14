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
    /// <summary>
    /// <list type="bullet">
    /// <listheader>
    /// <term></term>
    /// <description>parameter is two chars string, def is `02`:</description>
    /// </listheader>
    /// <item>
    /// <description>0: Visible</description>
    /// </item>
    /// <item>
    /// <description>1: Hidden (only work for WPF)</description>
    /// </item>
    /// <item>
    /// <description>2: Collapsed</description>
    /// </item>
    /// </list>
    /// e.g: 01 mean true to `Visible` else `Hidden`
    /// </summary>
    public class BooleanToVisibilityValueConverter : InvariantValueConverter
    {
        /// <summary>
        /// true => Visible; false => Collapsed;
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        internal static object Convert(bool value, Type targetType) => Convert(value, targetType, null);

        internal static object Convert(bool value, Type targetType, string parameter)
        {
            if (targetType != typeof(Visibility)) throw new NotSupportedException();

            switch (parameter)
            {
#if WINDOWS_DESKTOP
                case "01":
                    return value ? Visibility.Visible : Visibility.Hidden;

                case "21":
                    return value ? Visibility.Collapsed : Visibility.Hidden;

                case "10":
                    return value ? Visibility.Hidden : Visibility.Visible;

                case "12":
                    return value ? Visibility.Hidden : Visibility.Collapsed;

#endif

                case "02":
                default:
                    return value ? Visibility.Visible : Visibility.Collapsed;

                case "20":
                    return value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public override object Convert(object value, Type targetType, object parameter)
        {
            var boolValue = value as bool? ?? false;
            return Convert(boolValue, targetType, parameter as string);
        }
    }
}