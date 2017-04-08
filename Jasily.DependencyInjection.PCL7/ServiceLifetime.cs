namespace Jasily.DependencyInjection
{
    public enum ServiceLifetime
    {
        /// <summary>
        /// Singleton is Thread safe.
        /// </summary>
        Singleton,

        /// <summary>
        /// Scoped is NOT Thread safe.
        /// </summary>
        Scoped,

        /// <summary>
        /// Transient is NOT Thread safe.
        /// </summary>
        Transient
    }
}