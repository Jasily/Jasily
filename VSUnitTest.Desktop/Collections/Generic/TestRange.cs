using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Collections.Generic
{
    [TestClass]
    public class TestRange
    {
        [TestMethod]
        public void TestMethod()
        {
            var range = new Range<int>(0, 5, RangeMode.IncludeMin);
            Assert.AreEqual("[0, 5)", range.ToString());

            Assert.AreEqual(false, range.Contains(-1));
            Assert.AreEqual(true, range.Contains(0));
            Assert.AreEqual(true, range.Contains(1));
            Assert.AreEqual(true, range.Contains(2));
            Assert.AreEqual(true, range.Contains(3));
            Assert.AreEqual(true, range.Contains(4));
            Assert.AreEqual(false, range.Contains(5));

            Assert.AreEqual(1, range.CompareTo(-1));
            Assert.AreEqual(0, range.CompareTo(0));
            Assert.AreEqual(0, range.CompareTo(1));
            Assert.AreEqual(0, range.CompareTo(2));
            Assert.AreEqual(0, range.CompareTo(3));
            Assert.AreEqual(0, range.CompareTo(4));
            Assert.AreEqual(-1, range.CompareTo(5));

            Assert.AreEqual(true, new Range<int>(0, 5, RangeMode.IncludeMin) > new Range<int>(-1));
            Assert.AreEqual(false, new Range<int>(0, 5, RangeMode.IncludeMin) > new Range<int>(0));
            Assert.AreEqual(false, new Range<int>(0, 5, RangeMode.IncludeMin) < new Range<int>(-1));
            Assert.AreEqual(true, new Range<int>(0, 5, RangeMode.IncludeMin) < new Range<int>(5));
        }
    }
}