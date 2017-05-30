namespace Jasily.Converters.StringValueConverters
{
    public class Int16StringConverter : BaseStringConverter<short>
    {
        protected override short ConvertCore(string value)
        {
            return short.Parse(value);
        }

        protected override bool TryConvertCore(string value, out short result)
        {
            return short.TryParse(value, out result);
        }
    }
}