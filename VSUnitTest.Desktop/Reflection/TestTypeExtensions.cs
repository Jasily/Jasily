using System.Collections.Generic;
using System.Reflection;
using Jasily.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Reflection
{
    [TestClass]
    public class TestTypeExtensions
    {
        [TestMethod]
        public void TestValue()
        {
            Assert.AreEqual(typeof(Dictionary<int, string>), typeof(Dictionary<,>).FastMakeGenericType(typeof(int), typeof(string)));
            Assert.AreEqual(typeof(Dictionary<int, string>), typeof(Dictionary<,>).FastMakeGenericType(typeof(int), typeof(string)));
            Assert.AreEqual(typeof(Dictionary<int, string>), typeof(Dictionary<,>).FastMakeGenericType(typeof(int), typeof(string)));
            Assert.AreEqual(typeof(Dictionary<int, string>), typeof(Dictionary<,>).FastMakeGenericType(typeof(int), typeof(string)));
        }

        [TestMethod, TestCategory("Benchmarks")]
        public void PerformanceTest()
        {
            CodeTimerHelper.FastThen(
                () => typeof(Dictionary<,>).FastMakeGenericType(typeof(int), typeof(string)),
                () => typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(string)),
                100000);
        }
    }
}