using Jasily.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.Text
{
    [TestClass]
    public class TestStringSegment
    {
        private static StringSegment Sample1() => new StringSegment(string.Empty);

        private static StringSegment Sample2() => new StringSegment("12345");

        private static StringSegment Sample3() => new StringSegment("12345", 1);

        private static StringSegment Sample4() => new StringSegment("12345", 1, 3);

        [TestMethod]
        public void TestToString()
        {
            Assert.AreEqual(string.Empty, Sample1().ToString());
            
            Assert.AreEqual("12345", Sample2().ToString());
            Assert.AreEqual(0, Sample2().StartIndex);
            Assert.AreEqual(5, Sample2().Count);
            
            Assert.AreEqual("2345", Sample3().ToString());
            Assert.AreEqual(1, Sample3().StartIndex);
            Assert.AreEqual(4, Sample3().Count);
            
            Assert.AreEqual("234", Sample4().ToString());
            Assert.AreEqual(1, Sample4().StartIndex);
            Assert.AreEqual(3, Sample4().Count);
        }

        [TestMethod]
        public void TestSubSegment()
        {
            Assert.AreEqual(Sample2().ToString().Substring(1), Sample2().SubSegment(1).ToString());
            Assert.AreEqual(Sample3().ToString().Substring(1), Sample3().SubSegment(1).ToString());
            Assert.AreEqual(Sample4().ToString().Substring(1), Sample4().SubSegment(1).ToString());

            Assert.AreEqual(Sample2().ToString().Substring(1, 2), Sample2().SubSegment(1, 2).ToString());
            Assert.AreEqual(Sample3().ToString().Substring(1, 2), Sample3().SubSegment(1, 2).ToString());
            Assert.AreEqual(Sample4().ToString().Substring(1, 2), Sample4().SubSegment(1, 2).ToString());
        }
    }
}
