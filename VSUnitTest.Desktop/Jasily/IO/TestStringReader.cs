using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringReader = Jasily.IO.StringReader;

namespace VSUnitTest.Desktop.Jasily.IO
{
    [TestClass]
    public class TestStringReader
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string doc = "abcsrwqrqwfsdgds";
            var reader = new StringReader(doc);

            Assert.AreEqual(doc, reader.ReadToEnd());
            Assert.AreEqual("", reader.ReadToEnd());

            reader.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(doc, reader.ReadToEnd());

            reader.Seek(1, SeekOrigin.Begin);
            Assert.AreEqual(doc.Substring(1), reader.ReadToEnd());

            reader.Seek(1, SeekOrigin.Begin);
            reader.Seek(3, SeekOrigin.Current);
            Assert.AreEqual(doc.Substring(4), reader.ReadToEnd());

            reader.Seek(4, SeekOrigin.Begin);
            reader.Seek(-1, SeekOrigin.Current);
            Assert.AreEqual(doc.Substring(3), reader.ReadToEnd());

            reader.Seek(1, SeekOrigin.Begin);
            reader.Seek(-3, SeekOrigin.End);
            Assert.AreEqual(doc.Substring(doc.Length - 3, 3), reader.ReadToEnd());
        }
    }
}
