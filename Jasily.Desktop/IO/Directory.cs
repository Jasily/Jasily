
namespace Jasily.IO
{
    using SIO = System.IO;

    public static class Directory
    {
        public static void EnsureParentDirectory(string path)
        {
            var dir = SIO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir)) EnsureDirectory(dir);
        }

        public static void EnsureDirectory(string path)
        {
            if (SIO.Directory.Exists(path)) return;
            var parent = SIO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(parent)) EnsureDirectory(parent);
            SIO.Directory.CreateDirectory(path);
        }
    }
}