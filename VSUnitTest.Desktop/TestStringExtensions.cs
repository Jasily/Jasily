using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily
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

        [TestMethod]
        public void TestGetString()
        {
            Assert.AreEqual("12345", "12345".ToCharArray().GetString());
        }

        [TestMethod]
        public void TestAfterFirst()
        {
            Assert.AreEqual("2345", "012345".AfterFirst("1"));
            Assert.AreEqual("2345", "012345".AfterFirst("1"));
            Assert.AreEqual("2345", "012345".AfterFirst('1'));
            Assert.AreEqual("012345", "012345".AfterFirst('k'));
            Assert.AreEqual("345", "012345".AfterFirst('1', '2'));
        }

        [TestMethod]
        public void TestAfterLast()
        {
            Assert.AreEqual("5", "012345".AfterLast("4"));
            Assert.AreEqual("5", "012345".AfterLast('4'));
            Assert.AreEqual("45", "012345".AfterLast('3', '0'));
        }

        [TestMethod]
        public void TestCommonStart()
        {
            var test1 = new string[] { };
            Assert.AreEqual("", test1.CommonStart());

            var test2 = new[] { "12" };
            Assert.AreEqual("12", test2.CommonStart());

            var test3 = new[] { "", "", "" };
            Assert.AreEqual("", test3.CommonStart());

            var test4 = new[] { "", "12", "" };
            Assert.AreEqual("", test4.CommonStart());

            var test5 = new[] { "12", "12", "123" };
            Assert.AreEqual("12", test5.CommonStart());
        }

        [TestMethod]
        public void TestCommonEnd()
        {
            var test1 = new string[] { };
            Assert.AreEqual("", test1.CommonEnd());

            var test2 = new[] { "12" };
            Assert.AreEqual("12", test2.CommonEnd());

            var test3 = new[] { "", "", "" };
            Assert.AreEqual("", test3.CommonEnd());

            var test4 = new[] { "", "12", "" };
            Assert.AreEqual("", test4.CommonEnd());

            var test5 = new[] { "12", "12", "123" };
            Assert.AreEqual("", test5.CommonEnd());

            var test6 = new[] { "012", "012", "312" };
            Assert.AreEqual("12", test6.CommonEnd());
        }

        [TestMethod]
        public void TestStartsWith()
        {
            Assert.AreEqual(true, "".StartsWith(0, "", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(true, "123".StartsWith(0, "12", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(true, "A123".StartsWith(0, "a", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void TestReplace()
        {
            Assert.AreEqual("1234-c-C6789_1234-c-C6789", "1234abcABC6789_1234abcABC6789".Replace("ab", "-", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("__123abcABC", "abcABC123abcABC".ReplaceStart("abc", "_", StringComparison.OrdinalIgnoreCase));
        }
    }
}
