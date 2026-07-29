using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;

namespace LegalHarvest.Modules
{
    public class HardwareInfoModule : ICollectorModule
    {
        public string ModuleName => "HardwareInfo";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor"))
                    foreach (var obj in searcher.Get())
                        items.Add(new CollectedItem { Category = "Hardware", Name = "CPU", Value = obj["Name"].ToString(), Timestamp = DateTime.Now });

                using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
                {
                    ulong totalRam = 0;
                    foreach (var obj in searcher.Get())
                        totalRam += (ulong)obj["Capacity"];
                    items.Add(new CollectedItem { Category = "Hardware", Name = "RAM (GB)", Value = (totalRam / (1024 * 1024 * 1024)).ToString(), Timestamp = DateTime.Now });
                }

                using (var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController"))
                    foreach (var obj in searcher.Get())
                        items.Add(new CollectedItem { Category = "Hardware", Name = "GPU", Value = obj["Name"].ToString(), Timestamp = DateTime.Now });

                items.Add(new CollectedItem { Category = "Hardware", Name = "Screen", Value = $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}", Timestamp = DateTime.Now });
            }
            catch { }
            return items;
        }
    }
}
