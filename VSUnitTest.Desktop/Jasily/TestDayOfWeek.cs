using System;
using Jasily;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DayOfWeek = System.DayOfWeek;
using JDayOfWeek = Jasily.DayOfWeek;

namespace VSUnitTest.Desktop.Jasily
{
    [TestClass]
    public class TestDayOfWeek
    {
        public static JDayOfWeek ToJasilyDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday: return JDayOfWeek.Sunday;
                case DayOfWeek.Monday: return JDayOfWeek.Monday;
                case DayOfWeek.Tuesday: return JDayOfWeek.Tuesday;
                case DayOfWeek.Wednesday: return JDayOfWeek.Wednesday;
                case DayOfWeek.Thursday: return JDayOfWeek.Thursday;
                case DayOfWeek.Friday: return JDayOfWeek.Friday;
                case DayOfWeek.Saturday: return JDayOfWeek.Saturday;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [TestMethod]
        public void TestToJasilyDayOfWeek()
        {
            Assert.AreEqual(JDayOfWeek.Sunday, DayOfWeek.Sunday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Monday, DayOfWeek.Monday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Tuesday, DayOfWeek.Tuesday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Wednesday, DayOfWeek.Wednesday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Thursday, DayOfWeek.Thursday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Friday, DayOfWeek.Friday.ToJasilyDayOfWeek());
            Assert.AreEqual(JDayOfWeek.Saturday, DayOfWeek.Saturday.ToJasilyDayOfWeek());
        }

        [TestMethod]
        public void TestToJasilyDayOfWeekPerformance()
        {
            Action action1 = () =>
            {
                // switch-case mode.
                Assert.AreEqual(JDayOfWeek.Sunday, ToJasilyDayOfWeek(DayOfWeek.Sunday));
                Assert.AreEqual(JDayOfWeek.Monday, ToJasilyDayOfWeek(DayOfWeek.Monday));
                Assert.AreEqual(JDayOfWeek.Tuesday, ToJasilyDayOfWeek(DayOfWeek.Tuesday));
                Assert.AreEqual(JDayOfWeek.Wednesday, ToJasilyDayOfWeek(DayOfWeek.Wednesday));
                Assert.AreEqual(JDayOfWeek.Thursday, ToJasilyDayOfWeek(DayOfWeek.Thursday));
                Assert.AreEqual(JDayOfWeek.Friday, ToJasilyDayOfWeek(DayOfWeek.Friday));
                Assert.AreEqual(JDayOfWeek.Saturday, ToJasilyDayOfWeek(DayOfWeek.Saturday));
            };
            Action action2 = () =>
            {
                // extension method.
                Assert.AreEqual(JDayOfWeek.Sunday, DayOfWeek.Sunday.ToJasilyDayOfWeek());
                Assert.AreEqual(JDayOfWeek.Monday, DayOfWeek.Monday.ToJasilyDayOfWeek());
                Assert.AreEqual(JDayOfWeek.Tuesday, DayOfWeek.Tuesday.ToJasilyDayOfWeek());
                Assert.AreEqual(JDayOfWeek.Wednesday, DayOfWeek.Wednesday.ToJasilyDayOfWeek());
                Assert.AreEqual(JDayOfWeek.Thursday, DayOfWeek.Thursday.ToJasilyDayOfWeek());
                Assert.AreEqual(JDayOfWeek.Friday, DayOfWeek.Friday.ToJasilyDayOfWeek());
                Assert.AreEqual(JDayOfWeek.Saturday, DayOfWeek.Saturday.ToJasilyDayOfWeek());
            };
            using (var timer = new CodeTimer())
            {
                var r1 = timer.Test(10000000, action1);
                var r2 = timer.Test(10000000, action2);
                Assert.IsTrue(r2.CpuCycles < r1.CpuCycles); // should fast then switch-case.
            }
        }
    }
}
