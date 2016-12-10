using System;
using Jasily.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.ComponentModel
{
    [TestClass]
    public class TestPropertySelector
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("Length",
                PropertySelector<int[]>.From(z => z.Length));
            Assert.AreEqual("Item1.Item1",
                PropertySelector<Tuple<Tuple<string, string>>>.From(z => z.Item1.Item1));
            Assert.AreEqual("Item1.Item1.Length",
                PropertySelector<Tuple<Tuple<int[], string>>>.From(z => (((object)z.Item1.Item1) as int[]).Length));
            Assert.AreEqual("Item1.Item1.Length",
                PropertySelector<Tuple<Tuple<int[], string>>>.From(z => ((int[])(object)z.Item1.Item1).Length));
            Assert.AreEqual("Item1.Item1.Length",
                PropertySelector<Tuple<Tuple<int[], string>>>.From(z => ((int[])((Tuple<int[], string>)z.Item1).Item1 as int[]).Length));
            Assert.AreEqual("Item1.Length",
                PropertySelector<Tuple<string[], string>>.From(z => z.Item1).SelectMany(z => z).Select(z => z.Length));
        }
    }
}
