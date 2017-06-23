namespace Jasily.IO
{
    public interface IStreamReadHandler : IStreamHandler
    {
        void OnReaded(byte[] buffer, int offset, int count);
    }
}