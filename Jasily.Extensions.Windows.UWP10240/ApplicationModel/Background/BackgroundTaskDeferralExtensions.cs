using System;
using System.Diagnostics;
using Jasily.Interfaces;
using JetBrains.Annotations;

namespace Windows.ApplicationModel.Background
{
    public static class BackgroundTaskDeferralExtensions
    {
        public static IDisposable<BackgroundTaskDeferral> AsDisposable([NotNull] this BackgroundTaskDeferral deferral)
        {
            if (deferral == null) throw new ArgumentNullException(nameof(deferral));
            return new BackgroundTaskDeferralDisposable(deferral);
        }

        private sealed class BackgroundTaskDeferralDisposable : IDisposable<BackgroundTaskDeferral>
        {
            public BackgroundTaskDeferralDisposable([NotNull] BackgroundTaskDeferral deferral)
            {
                Debug.Assert(deferral != null);
                this.DisposeObject = deferral;
            }

            public BackgroundTaskDeferral DisposeObject { get; }

            public void Dispose() => this.DisposeObject.Complete();
        }
    }
}