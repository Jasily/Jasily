namespace Jasily.ComponentModel.Editor
{
    public interface ITwoWayConverter : IConverter, IReverseConverter
    {
        
    }

    public interface ITwoWayConverter<TIn, TOut> : IConverter<TIn, TOut>, IReverseConverter<TOut, TIn>
    {
    }
}