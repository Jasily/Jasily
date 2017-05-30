namespace Jasily.Converters.StringValueConverters
{
    internal class Int64StringConverter : BaseStringConverter<long>
    {
        protected override long ConvertCore(string value)
        {
            return long.Parse(value);
        }

        protected override bool TryConvertCore(string value, out long result)
        {
            return long.TryParse(value, out result);
        }
    }
}
