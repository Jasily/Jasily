namespace Jasily.Cache.Internal
{
    /// <summary>
    /// <typeparamref name="T" /> should always be no-status.
    /// never use <code>object.ReferenceEquals()</code> for test this instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Default<T>
        where T : class, new()
    {
        private static T value;

        public static T Value() => value ?? (value = new T());

        public static void Dispose() => value = null;
    }
}