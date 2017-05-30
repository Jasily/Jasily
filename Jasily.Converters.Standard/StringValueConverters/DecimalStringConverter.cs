using System;

namespace Jasily.Converters.StringValueConverters
{
    public class DecimalStringConverter : BaseStringConverter<decimal>
    {
        protected override decimal ConvertCore(string value)
        {
            return decimal.Parse(value);
        }

        protected override bool TryConvertCore(string value, out decimal result)
        {
            return decimal.TryParse(value, out result);
        }
    }
}