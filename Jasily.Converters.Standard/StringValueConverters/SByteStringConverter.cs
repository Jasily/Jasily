namespace Jasily.Converters.StringValueConverters
{
    public class SByteStringConverter : BaseStringConverter<sbyte>
    {
        protected override sbyte ConvertCore(string value)
        {
            return sbyte.Parse(value);
        }

        protected override bool TryConvertCore(string value, out sbyte result)
        {
            return sbyte.TryParse(value, out result);
        }
    }
}