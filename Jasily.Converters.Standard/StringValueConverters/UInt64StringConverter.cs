namespace Jasily.Converters.StringValueConverters
{
    internal class UInt64StringConverter : BaseStringConverter<ulong>
    {
        protected override ulong ConvertCore(string value)
        {
            return ulong.Parse(value);
        }

        protected override bool TryConvertCore(string value, out ulong result)
        {
            return ulong.TryParse(value, out result);
        }
    }
}