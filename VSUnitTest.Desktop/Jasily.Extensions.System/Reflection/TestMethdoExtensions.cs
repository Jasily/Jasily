using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.Extensions.System.Reflection
{
    [TestClass]
    public class TestMethdoExtensions
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
            var func = typeof(TestMethdoExtensions)
                .GetMethod(nameof(this.IsNullOrWhiteSpace2))
                .Compile<TestMethdoExtensions, Func<TestMethdoExtensions, string, bool>>();
            Assert.AreEqual(true, func(new TestMethdoExtensions(), string.Empty));
            Assert.AreEqual(false, func(new TestMethdoExtensions(), "_"));
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
            var func = typeof(TestMethdoExtensions)
                .GetMethod(nameof(this.IsNullOrWhiteSpace2))
                .CompileFunc();
            Assert.AreEqual(true, (bool)func(new TestMethdoExtensions(), new object[] { string.Empty }));
            Assert.AreEqual(false, (bool)func(new TestMethdoExtensions(), new object[] { "_" }));
        }
    }
}
