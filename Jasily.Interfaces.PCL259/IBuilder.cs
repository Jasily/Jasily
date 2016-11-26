namespace Jasily.Interfaces
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}