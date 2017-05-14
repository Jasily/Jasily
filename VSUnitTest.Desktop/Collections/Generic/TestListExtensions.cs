using System.Collections.Generic;
using Jasily.Extensions.System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Collections.Generic
{
    [TestClass]
    public class TestListExtensions
    {
        [TestMethod]
        public void TestMakeAsCopy()
        {
            var list0 = new List<int>();
            var list1 = new List<int> { 1, 2, 3 };
            var list2 = new List<int> { 4, 5 };
            var list3 = new List<int> { 6, 7, 8, 9 };
            list0.MakeEqualsTo(list1);
            CollectionAssert.AreEqual(list1, list0);
            list0.MakeEqualsTo(list2);
            CollectionAssert.AreEqual(list2, list0);
            list0.MakeEqualsTo(list3);
            CollectionAssert.AreEqual(list3, list0);
        }
    }
}
