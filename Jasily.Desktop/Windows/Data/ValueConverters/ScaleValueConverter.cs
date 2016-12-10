using System;

#if WINDOWS_DESKTOP
using Jasily.Windows.Data.ValueConverters.Internal;
#elif WINDOWS_UWP
using Jasily.UI.Xaml.Data.ValueConverters.Internal;
#endif

#if WINDOWS_DESKTOP
namespace Jasily.Windows.Data.ValueConverters
#elif WINDOWS_UWP
namespace Jasily.UI.Xaml.Data.ValueConverters
#endif
{
    public class ScaleValueConverter : InvariantValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter)
        {
            if (targetType != typeof(double)) throw new InvalidOperationException();

            double scale = 1;
            if (parameter is double)
            {
                scale = (double)parameter;
            }
            else if (parameter is string)
            {
                double tmp;
                if (double.TryParse((string)parameter, out tmp)) scale = tmp;
            }

            if (value is double)
            {
                return (double)value * scale;
            }

            return 1;
        }
    }
}