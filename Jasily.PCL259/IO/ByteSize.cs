using System;
using Jasily.Cache;

namespace Jasily.IO
{
    public struct ByteSize
    {
        public long Value { get; }

        public ByteSize(long value)
        {
            this.Value = value;
        }

        public static implicit operator ByteSize(long length) => new ByteSize(length);

        public static implicit operator long(ByteSize size) => size.Value;

        public enum UnitType
        {
            Byte = 0,

            KByte = 1,

            MByte = 2,

            GByte = 3,

            TByte = 4,

            PByte = 5,

            EByte = 6,
        }

        public double Format(UnitType unit)
        {
            if (!Enum<UnitType>.IsDefined(unit)) throw new ArgumentOutOfRangeException(nameof(unit));

            var l = Convert.ToDouble(this.Value);
            var level = (int)unit - (int)UnitType.Byte;
            while (level > 0)
            {
                level--;
                l /= 1024;
            }
            return l;
        }

        public double Format(out UnitType unit)
        {
            var l = Convert.ToDouble(this.Value);
            var level = 0;
            while (l > 1024)
            {
                level++;
                l /= 1024;
            }
            unit = (UnitType)level;
            return l;
        }

        public override string ToString()
        {
            UnitType unit;
            var value = this.Format(out unit);
            return string.Format("{0,-6:##0.000} {1}", value, unit);
        }
    }
}