namespace Jasily.ComponentModel.Editor
{
    public interface IConverter
    {
        bool CanConvert(object value);

        object Convert(object value);
    }

    public interface IConverter<in TIn, out TOut>
    {
        bool CanConvert(TIn value);

        TOut Convert(TIn value);
    }
}