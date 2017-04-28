using System;

using Windows.UI.Xaml;
using JetBrains.Annotations;

namespace Jasily.UI.Xaml.Data.ValueConverters
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
    public class BooleanToVisibilityValueConverter : BaseValueConverter
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
                case "01":
                default:
                    return value ? Visibility.Visible : Visibility.Collapsed;

                case "10":
                    return value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public override object Convert([CanBeNull] object value, [NotNull] Type targetType, [CanBeNull] object parameter, [CanBeNull] string language)
        {
            var boolValue = value as bool? ?? false;
            return Convert(boolValue, targetType, parameter as string);
        }
    }
}