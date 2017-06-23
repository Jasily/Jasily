namespace Jasily.ComponentModel.Editor
{
    public interface IReverseConverter
    {
        bool CanConvertBack(object value);

        object ConvertBack(object value);
    }

    public interface IReverseConverter<in TIn, out TOut>
    {
        bool CanConvertBack(TIn value);

        TOut ConvertBack(TIn value);
    }
}