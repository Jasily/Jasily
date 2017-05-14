using System;
using System.Linq;
using Jasily.Extensions.System;
using Jasily.Extensions.System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Linq
{
    [TestClass]
    public class TestEnumerableExtensions
    {
        [TestMethod]
        public void TestAppend()
        {
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }.Append(4).ToArray(), new[] { 1, 2, 3, 4 });
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }.Append(4, Position.Begin).ToArray(), new[] { 4, 1, 2, 3 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }.Append(4, 0).ToArray(), new[] { 4, 1, 2, 3 });
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }.Append(4, 1).ToArray(), new[] { 1, 4, 2, 3 });
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }.Append(4, 3).ToArray(), new[] { 1, 2, 3, 4 });
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }.Append(4, 4).ToArray(), new[] { 1, 2, 3, 4 });
        }
    }
}
