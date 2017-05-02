using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.TestUtils
{
    public static class CodeTimerHelper
    {
        /// <summary>
        /// assert first action will fast then second action how many times.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="times"></param>
        /// <param name="testTime"></param>
        public static void FastThen(Action first, Action second, int times, int testTime)
        {
            Assert.IsNotNull(first);
            Assert.IsNotNull(second);
            // pre-compile
            first();
            second();
            // test
            using (var timer = new CodeTimer())
            {
                var r1 = timer.Test(testTime, first);
                var r2 = timer.Test(testTime, second);
                Assert.IsTrue(checked(r1.ElapsedMilliseconds * times) < r2.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// assert first action will fast then second action.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="testTime"></param>
        public static void FastThen(Action first, Action second, int testTime)
        {
            Assert.IsNotNull(first);
            Assert.IsNotNull(second);
            // pre-compile
            first();
            second();
            // test
            using (var timer = new CodeTimer())
            {
                var r1 = timer.Test(testTime, first);
                var r2 = timer.Test(testTime, second);
                Console.WriteLine(" First Action: ==========");
                Console.WriteLine(r1);
                Console.WriteLine("Second Action: ==========");
                Console.WriteLine(r2);
                Assert.IsTrue(r1.ElapsedMilliseconds < r2.ElapsedMilliseconds);
            }
        }
    }
}