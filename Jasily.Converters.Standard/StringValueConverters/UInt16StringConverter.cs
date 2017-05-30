namespace Jasily.Converters.StringValueConverters
{
    public class UInt16StringConverter : BaseStringConverter<ushort>
    {
        protected override ushort ConvertCore(string value)
        {
            return ushort.Parse(value);
        }

        protected override bool TryConvertCore(string value, out ushort result)
        {
            return ushort.TryParse(value, out result);
        }
    }
}