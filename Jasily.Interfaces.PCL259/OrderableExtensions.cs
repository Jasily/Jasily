namespace Jasily.Interfaces
{
    public static class OrderableExtensions
    {
        public static IOrderable AsOrderable<T>(this T obj) => obj as IOrderable ?? OrderableImpl.Value;

        private class OrderableImpl : IOrderable
        {
            public static readonly OrderableImpl Value = new OrderableImpl();

            public int GetOrderCode() => 0;
        }
    }
}