using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using D = System.Diagnostics.Debug;

namespace Jasily.Diagnostics
{
    public static class Debug
    {
        #region assert

        [Conditional("DEBUG")]
        public static void AssertForEach<T>([NotNull] IEnumerable<T> items, [NotNull] Func<T, bool> tester)
        {
            D.Assert(items != null && tester != null);
            foreach (var item in items)
            {
                D.Assert(tester(item));
            }
        }

        #endregion

        #region pointer

        [Conditional("DEBUG")]
        public static void Pointer(string message = null,
            [CallerFilePath] string path = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            var msg = message == null
                ? $"[POINTER] {path} ({line}) {member}"
                : $"[POINTER] [{message}] {path} ({line}) {member}";
            D.WriteLine(msg);
        }

        #endregion
    }
}