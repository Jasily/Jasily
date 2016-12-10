using Jasily.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.Collections.Generic
{
    [TestClass]
    public class TestCriticalValue
    {
        [TestMethod]
        public void TestMethod()
        {
            var a = new CriticalValue<int>(5, true);
            var b = new CriticalValue<int>(4, true);
            Assert.IsTrue(a > b);
            Assert.IsTrue(b < a);
            Assert.IsTrue(b != a);
            Assert.IsTrue(a != b);
            Assert.IsTrue(a >= b);
            Assert.IsTrue(b <= a);
            Assert.IsTrue(a.CompareTo(b) > 0);

            var c = new CriticalValue<int>(5, false);
            Assert.IsTrue(a > c);
            Assert.IsTrue(c < a);
            Assert.IsTrue(c != a);
            Assert.IsTrue(a != c);
            Assert.IsTrue(a >= c);
            Assert.IsTrue(c <= a);
            Assert.IsTrue(a.CompareTo(c) > 0);

            var d = new CriticalValue<int>(5, true);
            Assert.IsFalse(a > d);
            Assert.IsFalse(d < a);
            Assert.IsFalse(d != a);
            Assert.IsFalse(a != d);
            Assert.IsTrue(a >= d);
            Assert.IsTrue(d <= a);
            Assert.IsTrue(a.CompareTo(d) == 0);
        }
    }
}
