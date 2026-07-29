using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LegalHarvest.Modules
{
    public class ProcessListModule : ICollectorModule
    {
        public string ModuleName => "ProcessList";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    items.Add(new CollectedItem { Category = "Process", Name = process.ProcessName, Value = $"PID={process.Id}", Timestamp = DateTime.Now });
                }
                catch { }
            }
            return items;
        }
    }
}
