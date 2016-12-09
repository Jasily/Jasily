using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace VSUnitTest.Desktop
{
    /// <summary>
    /// <seealso cref="http://www.cnblogs.com/jeffreyzhao/archive/2009/03/10/codetimer.html"/>
    /// </summary>
    public sealed class CodeTimer : IDisposable
    {
        private readonly ProcessPriorityClass cachedProcessPriorityClass;
        private readonly ThreadPriority cachedThreadPriority;
        private readonly int maxGeneration;

        public CodeTimer()
        {
            this.cachedProcessPriorityClass = Process.GetCurrentProcess().PriorityClass;
            this.cachedThreadPriority = Thread.CurrentThread.Priority;

            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            this.maxGeneration = GC.MaxGeneration + 1;

            this.Test(1, () => { });
        }

        public CodeTimerResult Test(int iteration, Action action)
        {
            // 1.
            GC.Collect(this.maxGeneration - 1, GCCollectionMode.Forced);
            var gcCounts = new int[this.maxGeneration];
            for (var i = 0; i < this.maxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 2.
            var watch = new Stopwatch();
            watch.Start();
            var cycleCount = GetCycleCount();
            for (var i = 0; i < iteration; i++) action();
            var cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            var gens = new int[this.maxGeneration];
            for (var i = 0; i < this.maxGeneration; i++)
            {
                gens[i] = GC.CollectionCount(i) - gcCounts[i];
            }

            return new CodeTimerResult(iteration, watch.ElapsedMilliseconds, cpuCycles, gens);
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        public void Dispose()
        {
            Process.GetCurrentProcess().PriorityClass = this.cachedProcessPriorityClass;
            Thread.CurrentThread.Priority = this.cachedThreadPriority;
        }
    }
}