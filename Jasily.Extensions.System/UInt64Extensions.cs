using System.Runtime.CompilerServices;

namespace Jasily.Extensions.System
{
    public static class UInt64Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint OfLower(this ulong value) => (uint)(value & uint.MaxValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint OfHigh(this ulong value) => (uint)(value >> 32);
    }
}