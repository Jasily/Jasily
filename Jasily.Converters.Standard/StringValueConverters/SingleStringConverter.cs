namespace Jasily.Converters.StringValueConverters
{
    internal class SingleStringConverter : BaseStringConverter<float>
    {
        protected override float ConvertCore(string value)
        {
            return float.Parse(value);
        }

        protected override bool TryConvertCore(string value, out float result)
        {
            return float.TryParse(value, out result);
        }
    }
}
