using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.ComponentModel
{
    [TestClass]
    public class TestPropertySelector
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("", PropertySelector<int[]>.Root);
            Assert.AreEqual("Length", PropertySelector<int[]>.Root
                .Select(z => z.Length));
            Assert.AreEqual("Item1.Item1", PropertySelector<Tuple<Tuple<string, string>>>.Root
                .Select(z => z.Item1.Item1));
            Assert.AreEqual("Item1.Item1.Length", PropertySelector<Tuple<Tuple<int[], string>>>.Root
                .Select(z => (((object)z.Item1.Item1) as int[]).Length));
            Assert.AreEqual("Item1.Item1.Length", PropertySelector<Tuple<Tuple<int[], string>>>.Root
                .Select(z => ((int[])(object)z.Item1.Item1).Length));
            Assert.AreEqual("Item1.Item1.Length", PropertySelector<Tuple<Tuple<int[], string>>>.Root
                .Select(z => ((int[])((Tuple<int[], string>)z.Item1).Item1 as int[]).Length));
            Assert.AreEqual("Item1.Length", PropertySelector<Tuple<string[], string>>.Root
                .Select(z => z.Item1).SelectMany(z => z).Select(z => z.Length));
        }
    }
}
