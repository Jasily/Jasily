using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Windows.Storage
{
    public static class StorageFolderExtensions
    {
        [Conditional("DEBUG")]
        public static async void Print(this IStorageFolder folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            await Print(folder, 0);
        }

        private static async Task Print(this IStorageFolder folder, int deep)
        {
            Debug.Assert(folder != null);
            foreach (var item in await folder.GetItemsAsync())
            {
                var subFolder = item as IStorageFolder;
                if (subFolder != null)
                {
                    Debug.WriteLine($"{' '.Repeat(deep)}[FOLDER] {subFolder.Name}");
                    await Print(subFolder, deep + 1);
                }
                else
                {
                    Debug.WriteLine($"{' '.Repeat(deep)}[FILE] {item.Name}");
                }
            }
        }
    }
}