namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IArguments
    {
        bool TryGetValue(string key, out object value);
    }
}
