using System;
using Jasily.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JDayOfWeek = Jasily.DayOfWeek;

namespace Jasily
{
    [TestClass]
    public class TestDayOfWeek
    {
        [TestMethod]
        public void TestToJasilyDayOfWeek()
        {
            Assert.AreEqual(JDayOfWeek.Sunday, System.DayOfWeek.Sunday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Monday, System.DayOfWeek.Monday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Tuesday, System.DayOfWeek.Tuesday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Wednesday, System.DayOfWeek.Wednesday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Thursday, System.DayOfWeek.Thursday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Friday, System.DayOfWeek.Friday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Saturday, System.DayOfWeek.Saturday.ToJasilyDayOfWeek());
        }
    }
}
