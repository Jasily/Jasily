using Jasily.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily
{
    [TestClass]
    public class TestEmpty
    {
        [TestMethod]
        public void TestProperties()
        {
            Assert.IsNotNull(Empty<int>.Array);
            Assert.IsNotNull(Empty<int>.Enumerable);
        }
    }
}
