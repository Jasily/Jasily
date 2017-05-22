using System;
using System.Runtime.ExceptionServices;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    public static class ExceptionExtensions
    {
        public static void ReThrow([NotNull] this Exception e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            ExceptionDispatchInfo.Capture(e).Throw();
        }
    }
}