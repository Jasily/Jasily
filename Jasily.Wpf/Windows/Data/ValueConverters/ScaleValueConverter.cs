﻿using System;
using Jasily.Wpf.Windows.Data.ValueConverters.Internal;
#if WINDOWS_DESKTOP

#elif WINDOWS_UWP
using Jasily.UI.Xaml.Data.ValueConverters.Internal;
#endif

#if WINDOWS_DESKTOP
namespace Jasily.Wpf.Windows.Data.ValueConverters
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
                if (double.TryParse((string)parameter, out var tmp)) scale = tmp;
            }

            if (value is double)
            {
                return (double)value * scale;
            }

            return 1;
        }
    }
}