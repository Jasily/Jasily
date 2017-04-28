using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace VSUnitTest.Desktop.Jasily.Extensions.System.Collections.Generic
{
    [TestClass]
    public class TestReadOnlyListExtensions
    {
        [TestMethod]
        public void TestGetByIndex()
        {
            CollectionAssert.AreEqual(new int[] { 2, 3 }, new int[] { 1, 2, 3, 4, 5 }.GetByIndex(1, -2).ToArray());
            CollectionAssert.AreEqual(new int[] { 2 }, new int[] { 1, 2, 3, 4, 5 }.GetByIndex(1, 2).ToArray());
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }.GetByIndex(1, 7).ToArray());
            CollectionAssert.AreEqual(new int[] { 5 }, new int[] { 1, 2, 3, 4, 5 }.GetByIndex(-1, 7).ToArray());
            CollectionAssert.AreEqual(new int[] { }, new int[] { 1, 2, 3, 4, 5 }.GetByIndex(-101, -12).ToArray());
        }
    }
}
