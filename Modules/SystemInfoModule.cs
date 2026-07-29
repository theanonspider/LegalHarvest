using System;
using System.Collections.Generic;
using System.Net;

namespace LegalHarvest.Modules
{
    public class SystemInfoModule : ICollectorModule
    {
        public string ModuleName => "SystemInfo";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>
            {
                new CollectedItem { Category = "SystemInfo", Name = "Hostname", Value = Environment.MachineName, Timestamp = DateTime.Now },
                new CollectedItem { Category = "SystemInfo", Name = "UserName", Value = Environment.UserName, Timestamp = DateTime.Now },
                new CollectedItem { Category = "SystemInfo", Name = "OSVersion", Value = Environment.OSVersion.ToString(), Timestamp = DateTime.Now },
                new CollectedItem { Category = "SystemInfo", Name = "CurrentDirectory", Value = Environment.CurrentDirectory, Timestamp = DateTime.Now }
            };
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    items.Add(new CollectedItem { Category = "SystemInfo", Name = "LocalIP", Value = ip.ToString(), Timestamp = DateTime.Now });
            return items;
        }
    }
}
