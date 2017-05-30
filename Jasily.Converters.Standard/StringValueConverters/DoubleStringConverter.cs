namespace Jasily.Converters.StringValueConverters
{
    internal class DoubleStringConverter : BaseStringConverter<double>
    {
        protected override double ConvertCore(string value)
        {
            return double.Parse(value);
        }

        protected override bool TryConvertCore(string value, out double result)
        {
            return double.TryParse(value, out result);
        }
    }
}
