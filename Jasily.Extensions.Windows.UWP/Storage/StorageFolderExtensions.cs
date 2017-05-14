using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Jasily.Extensions.System;
using JetBrains.Annotations;

namespace Jasily.Extensions.Windows.Storage
{
    public static class StorageFolderExtensions
    {
        public static async Task<string> TreeToStringAsync([NotNull] this IStorageFolder folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var sb = new StringBuilder();
            async Task WriteAsync(IStorageFolder currentFolder, int deepLevel)
            {
                var indent = deepLevel * 2;
                foreach (var item in await currentFolder.GetItemsAsync())
                {
                    if (item is IStorageFolder f)
                    {
                        sb.AppendLine($"{' '.Repeat(indent)}[FOLDER] {f.Name}");
                        await WriteAsync(f, deepLevel + 1);
                    }
                    else
                    {
                        sb.AppendLine($"{' '.Repeat(indent)}[FILE] {item.Name}");
                    }
                }
            }

            await WriteAsync(folder, 0);
            return sb.ToString();
        }
    }
}