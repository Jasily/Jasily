using Jasily.Text;

namespace Jasily.IO
{
    public interface IStringSegmentReader
    {
        StringSegment ReadToEnd();
    }
}