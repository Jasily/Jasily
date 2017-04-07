using System;
using System.Diagnostics;
using Jasily.Core;
using Jasily.Interfaces;
using JetBrains.Annotations;

namespace Windows.ApplicationModel
{
    public static class SuspendingDeferralExtensions
    {
        public static IDisposable<T> AsDisposable<T>([NotNull] this T deferral)
           where T : class, ISuspendingDeferral
        {
            if (deferral == null) throw new ArgumentNullException(nameof(deferral));
            return new SuspendingDeferralDisposable<T>(deferral);
        }

        private sealed class SuspendingDeferralDisposable<T> : IDisposable<T>
            where T : class, ISuspendingDeferral
        {
            public SuspendingDeferralDisposable([NotNull] T deferral)
            {
                Debug.Assert(deferral != null);
                this.DisposeObject = deferral;
            }

            public T DisposeObject { get; }

            public void Dispose() => this.DisposeObject.Complete();
        }
    }
}