namespace Jasily.Linq.Expressions.Cache
{
    public static class Extensions
    {
        public static TTo ConvertChecked<TFrom, TTo>(this TFrom source)
            => Cache.ConvertChecked<TFrom, TTo>.Func(source);

        public static TTo ConvertUnchecked<TFrom, TTo>(this TFrom source)
            => Cache.ConvertUnchecked<TFrom, TTo>.Func(source);
    }
}