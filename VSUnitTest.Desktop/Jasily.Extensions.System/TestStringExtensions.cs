using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.Extensions.System
{
    [TestClass]
    public class TestStringExtensions
    {
        [TestMethod]
        public void TestBetween()
        {
            Assert.AreEqual("", "".Between("1", "3"));
            Assert.AreEqual("", "12345".Between("4", "4"));
            Assert.AreEqual("", "12345".Between("5", "4"));
            Assert.AreEqual("", "12345".Between("1", ""));
            Assert.AreEqual("2", "12345".Between("1", "3"));
            Assert.AreEqual("23", "12345".Between("1", "4"));
            Assert.AreEqual("234", "12345".Between("1", "5"));
        }
    }
}
