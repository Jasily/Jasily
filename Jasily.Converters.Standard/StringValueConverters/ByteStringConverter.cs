namespace Jasily.Converters.StringValueConverters
{
    public class ByteStringConverter : BaseStringConverter<byte>
    {
        protected override byte ConvertCore(string value)
        {
            return byte.Parse(value);
        }

        protected override bool TryConvertCore(string value, out byte result)
        {
            return byte.TryParse(value, out result);
        }
    }
}