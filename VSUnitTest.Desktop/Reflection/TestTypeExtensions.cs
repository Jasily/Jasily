using System.Collections.Generic;
using System.Reflection;
using Jasily.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Reflection
{
    [TestClass]
    public class TestTypeExtensions
    {
        [TestMethod, TestCategory("Benchmarks")]
        public void PerformanceTest()
        {
            CodeTimerHelper.FastThen(
                () => typeof(Dictionary<,>).FastMakeGenericType(typeof(int), typeof(string)),
                () => typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(string)),
                1000000);
        }
    }
}