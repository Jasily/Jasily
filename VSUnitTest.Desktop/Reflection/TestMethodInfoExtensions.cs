using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Reflection
{
    [TestClass]
    public class TestMethodInfoExtensions
    {
        public bool IsNullOrWhiteSpace2(string test) => string.IsNullOrWhiteSpace(test);

        [TestMethod]
        public void TestStaticMethod()
        {
            var func = typeof(string)
                .GetMethod(nameof(string.IsNullOrWhiteSpace))
                .Compile<string, Func<string, bool>>();
            Assert.AreEqual(true, func(string.Empty));
            Assert.AreEqual(false, func("_"));
        }

        [TestMethod]
        public void TestInstanceMethod()
        {
            var func = typeof(TestMethodInfoExtensions)
                .GetMethod(nameof(this.IsNullOrWhiteSpace2))
                .Compile<TestMethodInfoExtensions, Func<TestMethodInfoExtensions, string, bool>>();
            Assert.AreEqual(true, func(new TestMethodInfoExtensions(), string.Empty));
            Assert.AreEqual(false, func(new TestMethodInfoExtensions(), "_"));
        }

        [TestMethod]
        public void TestStaticMethod2()
        {
            var func = typeof(string)
                .GetMethod(nameof(string.IsNullOrWhiteSpace))
                .CompileFunc();
            Assert.AreEqual(true, (bool) func(null, new object[] { string.Empty }));
            Assert.AreEqual(false, (bool)func(null, new object[] { "_" }));
        }

        [TestMethod]
        public void TestInstanceMethod2()
        {
            var func = typeof(TestMethodInfoExtensions)
                .GetMethod(nameof(this.IsNullOrWhiteSpace2))
                .CompileFunc();
            Assert.AreEqual(true, (bool)func(new TestMethodInfoExtensions(), new object[] { string.Empty }));
            Assert.AreEqual(false, (bool)func(new TestMethodInfoExtensions(), new object[] { "_" }));
        }
    }
}