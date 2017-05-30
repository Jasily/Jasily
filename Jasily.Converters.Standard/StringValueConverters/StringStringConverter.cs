namespace Jasily.Converters.StringValueConverters
{
    public class StringStringConverter : BaseStringConverter<string>
    {
        protected override string ConvertCore(string value)
        {
            return value;
        }

        protected override bool TryConvertCore(string value, out string result)
        {
            result = value;
            return true;
        }
    }
}