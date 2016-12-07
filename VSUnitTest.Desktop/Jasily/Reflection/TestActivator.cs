using System;
using Jasily.Reflection;
using Jasily.Reflection.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.Reflection
{
    [TestClass]
    public class TestActivator
    {
        [TestMethod]
        public void TestMethodA()
        {
            var a = Activator<A>.CreateInstance();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(A));
        }

        public class A { }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestMethodB()
        {
            var a = Activator<B>.CreateInstance();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(B));
        }

        public class B
        {
            public B(string s)
            {

            }
        }

        [TestMethod]
        public void TestMethodC()
        {
            var a = Activator<C>.CreateInstance();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(C));
            Assert.AreEqual(a.S, "6");
        }

        public class C
        {
            public C(string s = "6")
            {
                this.S = s;
            }

            public string S;
        }

        [TestMethod]
        public void TestMethodD()
        {
            var a = Activator<D>.CreateInstance();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(D));
            Assert.AreEqual(a.S, null);
        }

        public class D
        {
            public D()
            {

            }

            public D(string s = "6")
            {
                this.S = s;
            }

            public string S;
        }

        [TestMethod]
        public void TestMethodE()
        {
            var a = Activator<E>.CreateInstance();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(E));
            Assert.AreEqual(a.S, "6");
        }

        public class E
        {
            public E()
            {

            }

            [Entry]
            public E(string s = "6")
            {
                this.S = s;
            }

            public string S;
        }

        [TestMethod]
        public void TestMethodF()
        {
            var a = Activator<F>.CreateInstance();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(F));
            Assert.AreEqual(a.S, "6");
        }

        public class F
        {
            public F()
            {

            }

            [Entry]
            public F([DefaultValue("6")] string s)
            {
                this.S = s;
            }

            public string S;
        }
    }
}