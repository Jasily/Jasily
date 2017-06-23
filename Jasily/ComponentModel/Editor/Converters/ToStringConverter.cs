
namespace Jasily.ComponentModel.Editor.Converters
{
    public abstract class ToStringConverter<T> : TwoWayConverter<T, string>
    {
        public override bool CanConvert(T value) => true;

        public override bool CanConvertBack(string value) => true;

        public override string Convert(T value) => value?.ToString();
    }
}