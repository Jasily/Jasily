using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Reflection
{
    [TestClass]
    public class TestConstructorInfoExtensions
    {
        public class X
        {
            public X(string f)
            {
                
            }
        }

        public class Y
        {
        }

        [TestMethod]
        public void TestCompileMethod_0()
        {
            var func1 = typeof(X).GetConstructors()[0].Compile<Func<string, X>>();
            var func2 = typeof(X).GetConstructors()[0].Compile();
            Assert.IsInstanceOfType(func1(""), typeof(X));
            Assert.IsInstanceOfType(func2(new object[] {""}), typeof(X));
        }

        [TestMethod]
        public void TestCompileMethod_1()
        {
            var func1 = typeof(Y).GetConstructors()[0].Compile<Func<Y>>();
            var func2 = typeof(Y).GetConstructors()[0].Compile();
            Assert.IsInstanceOfType(func1(), typeof(Y));
            Assert.IsInstanceOfType(func2(new object[] { }), typeof(Y));
            Assert.IsInstanceOfType(func2(null), typeof(Y));
        }
    }
}
