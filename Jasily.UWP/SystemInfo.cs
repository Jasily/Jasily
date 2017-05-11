using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.System.Profile;

namespace Jasily
{
    public static class SystemInfo
    {
        public static Version OSVersion { get; }

        public static string SystemFamilyName => AnalyticsInfo.VersionInfo.DeviceFamily;

        static SystemInfo()
        {
            // os version
            var sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            var v = ulong.Parse(sv);
            var v1 = (int)((v >> 48) & 0xFFFF);
            var v2 = (int)((v >> 32) & 0xFFFF);
            var v3 = (int)((v >> 16) & 0xFFFF);
            var v4 = (int)(v & 0xFFFF);
            OSVersion = new Version(v1, v2, v3, v4);
        }

    }
}