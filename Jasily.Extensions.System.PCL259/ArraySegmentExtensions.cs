namespace System
{
    public static class ArraySegmentExtensions
    {
        public static bool IsValid<T>(this ArraySegment<T> segment) => segment.Array != null;

        public static void ThrowIfInvalid<T>(this ArraySegment<T> segment, string paramName)
        {
            if (!segment.IsValid())
            {
                throw new ArgumentException("ArraySegment is invalid", paramName ?? nameof(segment));
            }
        }

        public static T Index<T>(this ArraySegment<T> segment, int index)
        {
            segment.ThrowIfInvalid(nameof(segment));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= segment.Count) throw new IndexOutOfRangeException();
            return segment.Array[segment.Offset + index];
        }
    }
}