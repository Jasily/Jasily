using System.Threading;

namespace Jasily
{
    public static class Singleton
    {
        public static T InstanceOf<T>() where T : class, new()
        {
            if (Instance<T>.instance == null)
            {
                Interlocked.CompareExchange(ref Instance<T>.instance, new T(), null);
            }

            return Instance<T>.instance;
        }

        private static class Instance<T> where T : class
        {
            // ReSharper disable once InconsistentNaming
            public static T instance;
        }
    }
}