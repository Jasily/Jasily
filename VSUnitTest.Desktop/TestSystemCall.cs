using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily
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
