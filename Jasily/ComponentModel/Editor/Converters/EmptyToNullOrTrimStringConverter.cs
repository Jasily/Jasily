namespace Jasily.ComponentModel.Editor.Converters
{
    public sealed class EmptyToNullOrTrimStringConverter : ToStringConverter<string>
    {
        public override string ConvertBack(string value) => string.IsNullOrEmpty(value) ? null : value.Trim();
    }
}