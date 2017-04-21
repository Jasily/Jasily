using System;
using System.Linq;
using Jasily.Cache;
using JetBrains.Annotations;
using D = System.DayOfWeek;

namespace Jasily
{
    public static class DayOfWeekExtensions
    {
        public static DayOfWeek ToJasilyDayOfWeek(this D dayOfWeek)
        {
            return (DayOfWeek) (1 << (int) dayOfWeek);
        }

        public static DayOfWeek ToJasilyDayOfWeek([NotNull] this D[] dayOfWeeks)
        {
            if (dayOfWeeks == null) throw new ArgumentNullException(nameof(dayOfWeeks));

            return dayOfWeeks.Length == 0
                ? DayOfWeek.None
                : dayOfWeeks.Aggregate(DayOfWeek.None, (current, dayOfWeek) => current.Or(dayOfWeek));
        }

        public static DayOfWeek Or(this DayOfWeek dayOfWeek, DayOfWeek other)
            => dayOfWeek | other;

        public static DayOfWeek Or(this DayOfWeek dayOfWeek, D other)
            => dayOfWeek.Or(other.ToJasilyDayOfWeek());

        public static DayOfWeek Or(this D dayOfWeek, D other)
            => dayOfWeek.ToJasilyDayOfWeek().Or(other.ToJasilyDayOfWeek());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DayOfWeek[] ToDayOfWeeks(this DayOfWeek dayOfWeek)
        {
            var flags = FastEnum<DayOfWeek>.SplitFlags(dayOfWeek);
            if (flags == null) throw new ArgumentException();
            return flags;
        }
    }
}