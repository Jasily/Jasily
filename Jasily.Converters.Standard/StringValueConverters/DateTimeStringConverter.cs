using System;

namespace Jasily.Converters.StringValueConverters
{
    public class DateTimeStringConverter : BaseStringConverter<DateTime>
    {
        protected override DateTime ConvertCore(string value)
        {
            return DateTime.Parse(value);
        }

        protected override bool TryConvertCore(string value, out DateTime result)
        {
            return DateTime.TryParse(value, out result);
        }
    }
}