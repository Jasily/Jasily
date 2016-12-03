using System;
using System.Globalization;

#if WINDOWS_DESKTOP

using System.Windows.Data;
namespace Jasily.Windows.Data.ValueConverters.Internal
{

#elif WINDOWS_UWP

using Windows.UI.Xaml.Data;
namespace Jasily.UI.Xaml.Data.ValueConverters.Internal
{

#endif
    public abstract class InvariantValueConverter : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter);

        public virtual object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
            => this.Convert(value, targetType, parameter);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => this.ConvertBack(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => this.ConvertBack(value, targetType, parameter);
    }
}