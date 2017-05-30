namespace Jasily.Converters.StringValueConverters
{
    internal class BooleanStringConverter : BaseStringConverter<bool>
    {
        protected override bool ConvertCore(string value)
        {
            return bool.Parse(value);
        }

        protected override bool TryConvertCore(string value, out bool result)
        {
            return bool.TryParse(value, out result);
        }
    }
}
