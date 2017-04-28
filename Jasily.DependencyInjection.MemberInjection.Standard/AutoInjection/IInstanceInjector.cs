namespace Jasily.DependencyInjection.MemberInjection.AutoInjection
{
    public interface IInstanceInjector<in T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> is null.</exception>
        /// <param name="instance"></param>
        void Inject(T instance);
    }
}
