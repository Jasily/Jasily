namespace Jasily.Interfaces.Collections.Generic
{
    public interface IGetKey<out T>
    {
        T GetKey();
    }
}