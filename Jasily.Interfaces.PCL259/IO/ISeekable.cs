using System.IO;

namespace Jasily.Interfaces.IO
{
    public interface ISeekable
    {
        void Seek(int offset, SeekOrigin origin);
    }
}