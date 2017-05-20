using System.Collections.Generic;
using Jasily.Extensions.System.Reflection;
using Jasily.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.DependencyInjection.Typed
{
    [TestClass]
    public class TestGenericTypeMaker
    {
        [TestMethod]
        public void TestValue()
        {
            var sc = new ServiceCollection();
            sc.AddGenericTypeFactory();
            var p = sc.BuildServiceProvider();
            var maker = p.GetRequiredService<IGenericTypeFactory>().GetTypeMaker(typeof(Dictionary<,>));

            Assert.AreEqual(typeof(Dictionary<int, string>), maker.MakeGenericType(typeof(int), typeof(string)));
            Assert.AreEqual(typeof(Dictionary<int, string>), maker.MakeGenericType(typeof(int), typeof(string)));
            Assert.AreEqual(typeof(Dictionary<int, string>), maker.MakeGenericType(typeof(int), typeof(string)));
            Assert.AreEqual(typeof(Dictionary<int, string>), maker.MakeGenericType(typeof(int), typeof(string)));
        }

        [TestMethod, TestCategory("Benchmarks")]
        public void PerformanceTest()
        {
            var sc = new ServiceCollection();
            sc.AddGenericTypeFactory();
            var p = sc.BuildServiceProvider();
            var maker = p.GetRequiredService<IGenericTypeFactory>().GetTypeMaker(typeof(Dictionary<,>));

            CodeTimerHelper.FastThen(
                () => maker.MakeGenericType(typeof(int), typeof(string)),
                () => typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(string)),
                10000000);
        }
    }
}