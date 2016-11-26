namespace Jasily.Interfaces
{
    public interface IValueable<out T>
    {
        T GetValue();
    }
}