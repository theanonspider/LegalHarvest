using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace LegalHarvest.Modules
{
    public class InstalledSoftwareModule : ICollectorModule
    {
        public string ModuleName => "InstalledSoftware";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string[] keys = { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" };
            foreach (string keyPath in keys)
            {
                using (var key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key == null) continue;
                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        using (var subkey = key.OpenSubKey(subkeyName))
                        {
                            string name = subkey?.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(name))
                                items.Add(new CollectedItem { Category = "InstalledSoftware", Name = name, Value = subkey.GetValue("DisplayVersion")?.ToString() ?? "", Timestamp = DateTime.Now });
                        }
                    }
                }
            }
            return items;
        }
    }
}
