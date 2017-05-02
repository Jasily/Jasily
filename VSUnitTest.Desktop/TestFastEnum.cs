using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily
{
    [TestClass]
    public class TestFastEnum
    {
        public enum Enum1
        {
            Value1,
            Value2,
            Value3
        }

        [Flags]
        public enum FlagEnum1
        {
            None = 0,
            Value1 = 1,
            Value2 = 1 << 1,
            Value3 = 1 << 2
        }

        private void Test<T>(T t) where T : struct, IComparable, IFormattable
        {
            Assert.AreEqual(t.ToString(), FastEnum<T>.ToString(t));
        }

        [TestMethod]
        public void TestMethod1()
        {
            this.Test(Enum1.Value1);
            this.Test(Enum1.Value1 | Enum1.Value1);
            this.Test(Enum1.Value1 | Enum1.Value2);
            this.Test(FlagEnum1.Value1);
            this.Test(FlagEnum1.Value1 | FlagEnum1.Value1);
            this.Test(FlagEnum1.Value1 | FlagEnum1.Value2);
        }
    }
}
