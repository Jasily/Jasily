namespace Jasily.Converters.StringValueConverters
{
    internal class UInt32StringConverter : BaseStringConverter<uint>
    {
        protected override uint ConvertCore(string value)
        {
            return uint.Parse(value);
        }

        protected override bool TryConvertCore(string value, out uint result)
        {
            return uint.TryParse(value, out result);
        }
    }
}