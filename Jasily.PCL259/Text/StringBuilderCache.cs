using System;
using System.Diagnostics;
using System.Text;

namespace Jasily.Text
{
    public static class StringBuilderCache
    {
        private const int MaxBuilderSize = 360;
        private const int DefaultCapacity = 16;

        [ThreadStatic]
        private static StringBuilder cachedInstance;

        public static ActionReleaser<StringBuilder> Acquire(int capacity = DefaultCapacity)
        {
            StringBuilder sb;
            if (capacity <= MaxBuilderSize &&
                cachedInstance != null &&
                capacity <= cachedInstance.Capacity)
            {
                sb = cachedInstance;
                cachedInstance = null;
            }
            else
            {
                sb = new StringBuilder(capacity);
            }
            Debug.Assert(sb != null);
            var releaser = Releaser.CreateActionReleaser(sb);
            releaser.ReleaseRaised += Releaser_ReleaseRaised;
            return releaser;
        }

        private static void Releaser_ReleaseRaised(ActionReleaser<StringBuilder> sender, StringBuilder e)
        {
            Debug.Assert(e != null);
            if (e.Capacity <= MaxBuilderSize)
            {
                e.Clear();
                cachedInstance = e;
            }
        }
    }
}