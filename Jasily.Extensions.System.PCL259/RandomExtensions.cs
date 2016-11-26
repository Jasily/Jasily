using JetBrains.Annotations;

namespace System
{
    public static class RandomExtensions
    {
        public static bool NextBoolean([NotNull] this Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            return random.Next(2) == 1;
        }

        public static byte[] NextBytes([NotNull] this Random random, int byteCount)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            var buffer = new byte[byteCount];
            random.NextBytes(buffer);
            return buffer;
        }

        public static int NextInt32([NotNull] this Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            return BitConverter.ToInt32(random.NextBytes(4), 0);
        }

        public static uint NextUInt32([NotNull] this Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            return BitConverter.ToUInt32(random.NextBytes(4), 0);
        }

        public static long NextInt64([NotNull] this Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            return BitConverter.ToInt64(random.NextBytes(8), 0);
        }

        public static ulong NextUInt64([NotNull] this Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            return BitConverter.ToUInt64(random.NextBytes(8), 0);
        }
    }
}