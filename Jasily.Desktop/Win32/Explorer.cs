using System.Diagnostics;

namespace Jasily.Win32
{
    public static class Explorer
    {
        public static void OpenAndSelect(string path)
        {
            var argument = "/select, \"" + path + "\"";
            using (Process.Start("explorer.exe", argument)) { }
        }
    }
}