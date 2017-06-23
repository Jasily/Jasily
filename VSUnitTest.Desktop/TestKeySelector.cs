using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily
{
    [TestClass]
    public class TestKeySelector
    {
        public int Value { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var comparer = KeySelector.CreateComparer<TestKeySelector, int>(z => z.Value);
            var s1 = new TestKeySelector { Value = 1 };
            var s2 = new TestKeySelector { Value = 1 };
            var s3 = new TestKeySelector { Value = 2 };
            Assert.IsTrue(comparer.Equals(s1, s2));
            Assert.IsFalse(comparer.Equals(s3, s2));
            Assert.IsFalse(comparer.Equals(s3, s2));
            Assert.AreEqual(s1.Value.GetHashCode(), comparer.GetHashCode(s1));
            Assert.AreEqual(s2.Value.GetHashCode(), comparer.GetHashCode(s2));
            Assert.AreEqual(s3.Value.GetHashCode(), comparer.GetHashCode(s3));
        }
    }
}