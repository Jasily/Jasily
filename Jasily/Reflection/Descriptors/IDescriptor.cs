namespace Jasily.Reflection.Descriptors
{
    public interface IDescriptor<out T>
        where T : class
    {
        T DescriptedObject { get; }
    }
}