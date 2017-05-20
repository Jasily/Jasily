using Jasily.Threading;

namespace Jasily
{
    /// <summary>
    /// provide static methods to create releasers.
    /// </summary>
    public static class Releaser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ChainReleaser<T> CreateChainReleaser<T>(T obj) => new ChainReleaser<T>().Next(obj);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ActionReleaser CreateActionReleaser() => new ActionReleaser();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ActionReleaser<T> CreateActionReleaser<T>(T value) => new ActionReleaser<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neverRaise"></param>
        /// <returns></returns>
        public static ReentrantReleaser CreateReentrantReleaser(bool neverRaise = false)
            => new ReentrantReleaser(neverRaise);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="neverRaise"></param>
        /// <returns></returns>
        public static ReentrantReleaser<T> CreateReentrantReleaser<T>(T value, bool neverRaise = false) 
            => new ReentrantReleaser<T>(value, neverRaise);
    }
}