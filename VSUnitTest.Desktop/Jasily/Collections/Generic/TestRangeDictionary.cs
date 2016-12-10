using Jasily.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.Collections.Generic
{
    [TestClass]
    public class TestRangeDictionary
    {
        [TestMethod]
        public void TestMethod()
        {
            var dict = RangeDictionary.Create<int, string>(new Range<int>(int.MinValue, int.MaxValue));
            dict.Add(new Range<int>(0, 5, RangeMode.IncludeMin), "605");

            Assert.AreEqual(false, dict.ContainsKey(-1));
            Assert.AreEqual(true, dict.ContainsKey(0));
            Assert.AreEqual(true, dict.ContainsKey(1));
            Assert.AreEqual(true, dict.ContainsKey(2));
            Assert.AreEqual(true, dict.ContainsKey(3));
            Assert.AreEqual(true, dict.ContainsKey(4));
            Assert.AreEqual(false, dict.ContainsKey(5));

            Set(dict, new Range<int>(4, 8, RangeMode.IncludeMax));
            Set(dict, new Range<int>(8, 12, RangeMode.IncludeMax));
            Set(dict, new Range<int>(-10, 1, RangeMode.IncludeMin));
            Set(dict, new Range<int>(50, 52, RangeMode.IncludeMin));
            Set(dict, new Range<int>(49, 58, RangeMode.IncludeMin));
            Assert.AreEqual(5, dict.Count);

            dict.RemoveKey(new Range<int>(int.MinValue, int.MaxValue));
            Assert.AreEqual(0, dict.Count);
        }

        private static void Set(RangeDictionary<int, string> dict, Range<int> range)
        {
            dict.Set(range, range.ToString());
        }
    }
}