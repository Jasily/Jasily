namespace Jasily.Converters.StringValueConverters
{
    internal class Int32StringConverter : BaseStringConverter<int>
    {
        protected override int ConvertCore(string value)
        {
            return int.Parse(value);
        }

        protected override bool TryConvertCore(string value, out int result)
        {
            return int.TryParse(value, out result);
        }
    }
}
