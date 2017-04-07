﻿using System;
using System.Diagnostics;
using Jasily.Core;
using Jasily.Interfaces;
using JetBrains.Annotations;

namespace Windows.UI.Xaml.Controls
{
    public static class ContentDialogButtonClickDeferralExtensions
    {
        public static IDisposable<ContentDialogButtonClickDeferral> AsDisposable([NotNull] this ContentDialogButtonClickDeferral deferral)
        {
            if (deferral == null) throw new ArgumentNullException(nameof(deferral));
            return new ContentDialogButtonClickDeferralDisposable(deferral);
        }

        private sealed class ContentDialogButtonClickDeferralDisposable : IDisposable<ContentDialogButtonClickDeferral>
        {
            public ContentDialogButtonClickDeferralDisposable([NotNull] ContentDialogButtonClickDeferral deferral)
            {
                Debug.Assert(deferral != null);
                this.DisposeObject = deferral;
            }

            public ContentDialogButtonClickDeferral DisposeObject { get; }

            public void Dispose() => this.DisposeObject.Complete();
        }
    }
}