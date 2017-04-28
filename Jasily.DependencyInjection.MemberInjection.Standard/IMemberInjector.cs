namespace Jasily.DependencyInjection.MemberInjection
{
    public interface IMemberInjector<in T>
    {
        void Inject(T instance, bool isRequired);
    }
}
