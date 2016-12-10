﻿using System;
using JetBrains.Annotations;

namespace Windows.System.Profile
{
    public static class AnalyticsVersionInfoExtensions
    {
        public static DeviceFamilyType GetDeviceFamilyType([NotNull] this AnalyticsVersionInfo info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            switch (info.DeviceFamily)
            {
                case "Desktop":
                case "Windows.Desktop":
                    return DeviceFamilyType.Desktop;

                case "Mobile":
                case "Windows.Mobile":
                    return DeviceFamilyType.Mobile;
                default:
                    return DeviceFamilyType.Unknown;
            }
        }
    }
}