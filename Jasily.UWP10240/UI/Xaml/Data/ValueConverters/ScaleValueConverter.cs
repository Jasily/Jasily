using System;

using JetBrains.Annotations;

namespace Jasily.UI.Xaml.Data.ValueConverters
{
    public class ScaleValueConverter : BaseValueConverter
    {
        public override object Convert([CanBeNull] object value, [NotNull] Type targetType,
            [CanBeNull] object parameter, [CanBeNull] string language)
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