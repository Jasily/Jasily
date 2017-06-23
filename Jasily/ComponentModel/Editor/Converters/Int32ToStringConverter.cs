namespace Jasily.ComponentModel.Editor.Converters
{
    public class Int32ToStringConverter : ToStringConverter<int>
    {
        public override int ConvertBack(string value) => int.Parse(value);
    }
}