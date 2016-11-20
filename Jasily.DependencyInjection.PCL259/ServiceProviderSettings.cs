namespace Jasily.DependencyInjection
{
    public struct ServiceProviderSettings
    {
        /// <summary>
        /// after this call count, will compile the Expression.
        /// default value is 2.
        /// </summary>
        public int? CompileAfterCallCount;

        public bool EnableDebug;
    }
}