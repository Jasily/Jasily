using System;
using System.Diagnostics.Contracts;
using T = System.Threading.Timeout;

namespace Jasily.Threading
{
    /// <summary>
    /// alse see: http://referencesource.microsoft.com/#mscorlib/system/threading/SpinWait.cs,b8cdeb634d79d613
    /// 使用算法来构造从生成 JasilyTimeout 开始到将此超时传递给系统 API 的时间。
    /// </summary>
    public class Timeout
    {
        private readonly uint created;
        private readonly uint timeout;

        private Timeout(int origin)
        {
            Contract.Assert(origin == T.Infinite || origin > 0);

            this.Value = origin;
            this.created = CurrentTickCount;
            this.timeout = (uint)origin + this.created;
        }

        /// <summary>
        /// reset by 49.7 day.
        /// </summary>
        private static uint CurrentTickCount => (uint)Environment.TickCount;

        public int Value { get; }

        /// <summary>
        /// return how many milliseconds after Timeout created.
        /// </summary>
        public uint TimeOffset
        {
            get
            {
                var cur = (uint) Environment.TickCount;
                if (cur > this.created) return cur - this.created;
                return cur + (uint.MaxValue - this.created);
            }
        }

        /// <summary>
        /// 剩下的时间
        /// </summary>
        public int LeftTime
        {
            get
            {
                var offset = this.TimeOffset;
                return offset > this.Value ? 0 : (int)(this.Value - offset);
            }
        }

        public bool IsTimeout => (uint)Environment.TickCount >= this.timeout;

        public static implicit operator Timeout(int milliseconds)
        {
            if (milliseconds == T.Infinite)
                return InfiniteTimeSpan;
            if (milliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "value must > 0");
            return new Timeout(milliseconds);
        }

        public static readonly Timeout InfiniteTimeSpan = new Timeout(T.Infinite);
    }
}