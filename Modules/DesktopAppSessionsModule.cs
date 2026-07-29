using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class DesktopAppSessionsModule : ICollectorModule
    {
        public string ModuleName => "DesktopAppSessions";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Skype
            string skypePath = Path.Combine(appData, "Skype");
            if (Directory.Exists(skypePath))
                foreach (string profile in Directory.GetDirectories(skypePath))
                {
                    string configFile = Path.Combine(profile, "config.xml");
                    if (File.Exists(configFile)) items.Add(new CollectedItem { Category = "Skype", Name = "Config", Value = configFile, Timestamp = DateTime.Now });
                }

            // Slack
            string slackPath = Path.Combine(appData, "Slack", "Local Storage", "leveldb");
            if (Directory.Exists(slackPath)) items.Add(new CollectedItem { Category = "Slack", Name = "LocalStorage", Value = slackPath, Timestamp = DateTime.Now });

            // WhatsApp Desktop
            string whatsappPath = Path.Combine(localAppData, "WhatsApp");
            if (Directory.Exists(whatsappPath))
                foreach (string file in Directory.GetFiles(whatsappPath, "*", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "WhatsApp", Name = file, Value = file, Timestamp = DateTime.Now });

            // Microsoft Teams
            string teamsPath = Path.Combine(appData, "Microsoft", "Teams");
            if (Directory.Exists(teamsPath)) items.Add(new CollectedItem { Category = "Teams", Name = "TeamsData", Value = teamsPath, Timestamp = DateTime.Now });

            return items;
        }
    }
}
