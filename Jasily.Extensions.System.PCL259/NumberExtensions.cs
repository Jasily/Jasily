namespace System
{
    public static class NumberExtensions
    {
        /// <summary>
        /// 0 => { 0 };
        /// 65535 => { 6, 5, 5, 3, 5 }
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] GetEachNumbers(this ushort number)
        {
            // ushort.MaxValue == 65535
            return InternalGetEachNumbers(number, 5);
        }

        /// <summary>
        /// 0 => { 0 };
        /// 4294967295 => { 4, 2, 9, 4, 9, 6, 7, 2, 9, 5 }
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] GetEachNumbers(this uint number)
        {
            // uint.MaxValue == 42949_67295
            return InternalGetEachNumbers(number, 10);
        }

        /// <summary>
        /// 0 => { 0 };
        /// 18446744073709551615 => { 1, 8, 4, 4, 6, 7, 4, 4, 0, 7, 3, 7, 0, 9, 5, 5, 1, 6, 1, 5 }
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] GetEachNumbers(this ulong number)
        {
            // ulong.MaxValue == 18446_74407_37095_51615
            return InternalGetEachNumbers(number, 20);
        }

        private static byte[] InternalGetEachNumbers(ulong number, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            var index = 0;
            while (number != 0)
            {
                index++;
                buffer[bufferSize - index] = (byte)(number % 10);
                number /= 10;
            }
            if (index == 0) return new byte[] { 0 };
            var ret = new byte[index];
            Array.Copy(buffer, bufferSize - index, ret, 0, index);
            return ret;
        }
    }
}