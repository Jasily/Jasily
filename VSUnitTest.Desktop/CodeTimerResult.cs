using System.Diagnostics;
using System.Text;

namespace VSUnitTest.Desktop
{
    [DebuggerDisplay("{ToString()}")]
    public struct CodeTimerResult
    {
        public int Iteration { get; }

        public long ElapsedMilliseconds { get; }

        public ulong CpuCycles { get; }

        public int[] Generations { get; }

        internal CodeTimerResult(int iteration, long timeElapsed, ulong cpuCycles, int[] generations)
        {
            this.Iteration = iteration;
            this.ElapsedMilliseconds = timeElapsed;
            this.CpuCycles = cpuCycles;
            this.Generations = generations;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[NS per op]    ").Append(this.UnitTimeByNS()).AppendLine(" ns");
            sb.Append("[Time Elapsed] ").Append(this.ElapsedMilliseconds).AppendLine(" ms");
            sb.Append("[CPU Cycles]   ").Append(this.CpuCycles).AppendLine();
            for (var i = 0; i < this.Generations.Length; i++)
            {
                sb.Append($"[Gen {i}]        ").Append(this.Generations[i]).AppendLine();
            }
            return sb.ToString();
        }

        public double UnitTimeByNS() => (double) this.ElapsedMilliseconds * 1000 / this.Iteration;
    }
}