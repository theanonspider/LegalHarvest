using System;
using System.Windows.Forms;

namespace LegalHarvest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            LegalGuard.EnsureAuthorized();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RedTigerStyleForm());
        }
    }
}
