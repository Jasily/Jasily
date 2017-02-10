using System;
using Jasily;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily
{
    [TestClass]
    public class TestSystemCall
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine(SystemCall.OutputReadToEnd("dir"));
        }
    }
}
