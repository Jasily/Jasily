namespace Jasily.Cache.Descriptors
{
    public interface IDescriptor<out T>
        where T : class
    {
        T DescriptedObject { get; }
    }
}