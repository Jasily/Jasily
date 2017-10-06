using System.Collections.Generic;
using Jasily.Reflection.GenericMakers;
using Jasily.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.GenericMakers
{
    [TestClass]
    public class TestGenericMaker
    {
        [TestMethod, TestCategory("Benchmarks")]
        public void PerformanceTest()
        {
            var maker = new GenericTypeMaker(typeof(Dictionary<,>));

            CodeTimerHelper.FastThen(
                () => maker.MakeGenericType(typeof(int), typeof(string)),
                () => typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(string)),
                1000000);
        }
    }
}