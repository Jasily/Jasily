namespace Jasily.Converters.StringValueConverters
{
    public class CharStringConverter : BaseStringConverter<char>
    {
        protected override char ConvertCore(string value)
        {
            return char.Parse(value);
        }

        protected override bool TryConvertCore(string value, out char result)
        {
            return char.TryParse(value, out result);
        }
    }
}