using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace UrfLauncher
{
    public class PremiumServer
    {
        public string Name { get; set; }
        public string ConnectUrl { get; set; }
        public string IpAddress { get; set; }
        public string VkUrl { get; set; }
        public string DiscordUrl { get; set; }
        public string NewsJsonUrl { get; set; }
        public string WorkshopId { get; set; }
        public string CollectionUrl => $"https://steamcommunity.com/sharedfiles/filedetails/?id={WorkshopId}";
        public string Description { get; set; }
        public List<string> ImageUrls { get; set; }

        public Color BgColor { get; set; }
        public Color CardColor { get; set; }
        public Color AccentColor { get; set; }
    }

    public class NewsItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishDate { get; set; }
    }

    public partial class Form1 : Form
    {
        private List<PremiumServer> servers;
        private PremiumServer selectedServer;
        private int borderRadius = 45;
        private Random rnd = new Random();
        private static readonly HttpClient netClient = new HttpClient();

        private bool isContentErrorState = false;

        private FlowLayoutPanel topMenu;
        private PictureBox pbServerScreenshot;
        private Label lblDescription;
        private Button btnPlay;
        private Button btnClearCache;
        private FlowLayoutPanel socialPanel;
        private Label lblLogo;

        private Panel newsPanel;
        private Label lblNewsHeader;
        private FlowLayoutPanel newsListContainer;

        public Form1()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            InitializeComponent();
            InitServersData();
            BuildCyberLayout();

            SelectServer(servers[0]);
        }

        private void InitServersData()
        {
            servers = new List<PremiumServer>
            {
                new PremiumServer
                {
                    Name = "FALLOUT RP",
                    ConnectUrl = "steam://connect/46.174.50.193:27021",
                    IpAddress = "46.174.50.193:27021",
                    VkUrl = "https://vk.com/falloutrp",
                    DiscordUrl = "https://discord.gg/s6yXZmg59b",
                    NewsJsonUrl = "https://raw.githubusercontent.com/det1xz12-lab/URF-Launcher/main/news_fallout.json",
                    WorkshopId = "3701259766",
                    ImageUrls = new List<string> { "https://urf.im/static/img/fallout_franchise/img_1.jpg", "https://urf.im/static/img/fallout_franchise/img_2.jpg" },
                    Description = "Fallout RP: New Vegas - это RP сервер в многопользовательской игре garry's mod, являющийся полным воплощением вселенной Fallout.",
                    BgColor = Color.FromArgb(15, 30, 20), CardColor = Color.FromArgb(10, 20, 12), AccentColor = Color.FromArgb(50, 220, 90)
                },
                new PremiumServer
                {
                    Name = "FNAF RP КР",
                    ConnectUrl = "steam://connect/46.174.48.152:27017",
                    IpAddress = "46.174.48.152:27017",
                    VkUrl = "https://vk.com/fnafrp",
                    DiscordUrl = "https://discord.gg/cKu3nZUbYj",
                    NewsJsonUrl = "https://raw.githubusercontent.com/det1xz12-lab/URF-Launcher/main/news_fnaf.json",
                    WorkshopId = "3543269106",
                    ImageUrls = new List<string> { "https://urf.im/static/img/fnaf_russia/fnaf4.png" },
                    Description = "FNAF RP - сервер в многопользовательской игре garry's mod, позволяющий окунуться в атмосферу Five Nights at Freddy's.",
                    BgColor = Color.FromArgb(40, 12, 35), CardColor = Color.FromArgb(24, 6, 20), AccentColor = Color.FromArgb(210, 80, 230)
                },
                new PremiumServer
                {
                    Name = "HL2RP ALYX",
                    ConnectUrl = "steam://connect/46.174.48.152:27020",
                    IpAddress = "46.174.48.152:27020",
                    VkUrl = "https://vk.com/alyxrp",
                    DiscordUrl = "https://discord.gg/cZm4HSdqnC",
                    NewsJsonUrl = "https://raw.githubusercontent.com/det1xz12-lab/URF-Launcher/main/news_alyx.json",
                    WorkshopId = "3704341067",
                    ImageUrls = new List<string> { "https://urf.im/static/img/alyx/screen_2.jpg", "https://urf.im/static/img/alyx/screen_4.jpg" },
                    Description = "HLRP:Alyx - сервер в многопользовательской игре garry's mod, где вы полностью погрузитесь во времена Half Life 2 после 7 часовой войны.",
                    BgColor = Color.FromArgb(38, 24, 15), CardColor = Color.FromArgb(24, 14, 8), AccentColor = Color.FromArgb(245, 150, 70)
                },
                new PremiumServer
                {
                    Name = "METRO RP",
                    ConnectUrl = "steam://connect/46.174.50.193:27018",
                    IpAddress = "46.174.50.193:27018",
                    VkUrl = "https://vk.com/metro2033urfim",
                    DiscordUrl = "https://discord.gg/AV5bUzu6vF",
                    NewsJsonUrl = "https://raw.githubusercontent.com/det1xz12-lab/URF-Launcher/main/news_metro.json",
                    WorkshopId = "1750726415",
                    ImageUrls = new List<string> { "https://urf.im/static/img/metro/img_5.jpg" },
                    Description = "Метро RP \"Мертвая Москва\" - это RP сервер в многопользовательской игре garry's mod по мотивам вселенной Метро 2033.",
                    BgColor = Color.FromArgb(22, 22, 22), CardColor = Color.FromArgb(14, 14, 14), AccentColor = Color.FromArgb(255, 75, 25)
                },
                new PremiumServer
                {
                    Name = "HALF-LIFE 2 RP",
                    ConnectUrl = "steam://connect/46.174.50.193:27015",
                    IpAddress = "46.174.50.193:27015",
                    VkUrl = "https://vk.com/city17rp",
                    DiscordUrl = "https://discord.gg/R7x7tR6gQZ",
                    NewsJsonUrl = "https://raw.githubusercontent.com/det1xz12-lab/URF-Launcher/main/news_hl2rp.json",
                    WorkshopId = "1750726415",
                    ImageUrls = new List<string> { "https://urf.im/static/img/c17/c17_1.jpg" },
                    Description = "Классический Half-Life 2 RP сервер в тоталитарном Сити-17 под контролем Альянса.",
                    BgColor = Color.FromArgb(18, 28, 38), CardColor = Color.FromArgb(10, 18, 26), AccentColor = Color.FromArgb(40, 160, 240)
                }
            };
        }

        private void BuildCyberLayout()
        {
            this.Text = "URF.IM LAUNCHER BY DET1XZ";
            this.Size = new Size(1150, 660);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, borderRadius, borderRadius));
            this.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); } };

            lblLogo = new Label { Text = "URF.IM", Font = new Font("Segoe UI Black", 22, FontStyle.Bold), Location = new Point(35, 20), AutoSize = true };
            this.Controls.Add(lblLogo);

            Button btnClose = new Button { Text = "✕", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(220, 220, 220), FlatStyle = FlatStyle.Flat, Size = new Size(35, 35), Location = new Point(1090, 18), Cursor = Cursors.Hand };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(190, 15, 30);
            btnClose.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnClose);

            topMenu = new FlowLayoutPanel { Location = new Point(180, 20), Size = new Size(890, 55), FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent, WrapContents = false, AutoScroll = true };
            this.Controls.Add(topMenu);

            foreach (var server in servers)
            {
                Button btnMenu = new Button
                {
                    Text = server.Name.ToUpper(),
                    Font = new Font("Segoe UI Black", 9, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    AutoSize = true,
                    Height = 35,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 0, 15, 0),
                    Tag = server
                };
                btnMenu.FlatAppearance.BorderSize = 0;
                btnMenu.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnMenu.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnMenu.Click += (s, e) => SelectServer((PremiumServer)((Button)s).Tag);
                topMenu.Controls.Add(btnMenu);
            }

            pbServerScreenshot = new PictureBox { Location = new Point(40, 110), Size = new Size(600, 360), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.FromArgb(20, 20, 20) };
            pbServerScreenshot.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                GraphicsPath path = GetRoundRectPath(0, 0, pbServerScreenshot.Width, pbServerScreenshot.Height, 35);
                pbServerScreenshot.Region = new Region(path);
            };
            this.Controls.Add(pbServerScreenshot);

            lblDescription = new Label { Location = new Point(670, 110), Size = new Size(440, 180), Font = new Font("Segoe UI Semibold", 9.5f), ForeColor = Color.White, TextAlign = ContentAlignment.TopLeft };
            this.Controls.Add(lblDescription);

            newsPanel = new Panel { Location = new Point(670, 305), Size = new Size(440, 165), BackColor = Color.FromArgb(25, 0, 0, 0) };
            newsPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                GraphicsPath path = GetRoundRectPath(0, 0, newsPanel.Width, newsPanel.Height, 20);
                newsPanel.Region = new Region(path);
                using (Pen borderPen = new Pen(Color.FromArgb(50, selectedServer != null ? selectedServer.AccentColor : Color.White), 1))
                {
                    e.Graphics.DrawPath(borderPen, path);
                }
            };

            lblNewsHeader = new Label { Text = "📢 ПОСЛЕДНИЕ АНОНСЫ И НОВОСТИ", Location = new Point(12, 10), AutoSize = true, Font = new Font("Segoe UI Black", 9f, FontStyle.Bold), ForeColor = Color.White };
            newsPanel.Controls.Add(lblNewsHeader);

            newsListContainer = new FlowLayoutPanel { Location = new Point(10, 32), Size = new Size(420, 125), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
            newsPanel.Controls.Add(newsListContainer);

            this.Controls.Add(newsPanel);

            btnPlay = new Button { Text = "ПОДКЛЮЧИТЬСЯ", Font = new Font("Segoe UI Black", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(40, 500), Size = new Size(420, 75), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.Paint += (s, e) => { btnPlay.Region = new Region(GetRoundRectPath(0, 0, btnPlay.Width, btnPlay.Height, 30)); };
            btnPlay.Click += BtnPlay_Click;
            this.Controls.Add(btnPlay);

            btnClearCache = new Button { Text = "ОЧИСТИТЬ КЭШ ИГРЫ", Font = new Font("Segoe UI Black", 11, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.FromArgb(45, 45, 50), Location = new Point(40, 590), Size = new Size(420, 40), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnClearCache.FlatAppearance.BorderSize = 0;
            btnClearCache.Paint += (s, e) => { btnClearCache.Region = new Region(GetRoundRectPath(0, 0, btnClearCache.Width, btnClearCache.Height, 15)); };
            btnClearCache.Click += BtnClearCache_Click;
            this.Controls.Add(btnClearCache);

            socialPanel = new FlowLayoutPanel { Location = new Point(670, 525), Size = new Size(440, 50), FlowDirection = FlowDirection.LeftToRight };
            this.Controls.Add(socialPanel);
        }

        private void SelectServer(PremiumServer server)
        {
            selectedServer = server;
            lblDescription.Text = server.Description;

            isContentErrorState = false;
            btnPlay.Text = "ПОДКЛЮЧИТЬСЯ";
            btnPlay.Font = new Font("Segoe UI Black", 18, FontStyle.Bold);

            this.BackColor = server.BgColor;
            socialPanel.BackColor = server.BgColor;
            lblLogo.ForeColor = server.AccentColor;
            btnPlay.BackColor = server.AccentColor;
            lblNewsHeader.ForeColor = server.AccentColor;
            newsPanel.Invalidate();

            socialPanel.Controls.Clear();
            AddSocialButton("VKONTAKTE", server.VkUrl);
            AddSocialButton("DISCORD", server.DiscordUrl);
            AddSocialButton("WEBSITE", "https://urf.im");

            foreach (Button btn in topMenu.Controls)
            {
                btn.ForeColor = (btn.Tag == server) ? server.AccentColor : Color.FromArgb(160, 160, 175);
            }

            _ = LoadServerLiveImageAsync(server);
            _ = FetchJsonNewsAsync(server);
        }

        private async Task FetchJsonNewsAsync(PremiumServer server)
        {
            newsListContainer.Controls.Clear();
            Label lblLoading = new Label { Text = "Загрузка новостей...", AutoSize = true, Font = new Font("Segoe UI", 8.5f, FontStyle.Italic), ForeColor = Color.Gray };
            newsListContainer.Controls.Add(lblLoading);

            List<NewsItem> items = await Task.Run(() => GetGitHubJsonNews(server.NewsJsonUrl, server.DiscordUrl));

            if (selectedServer != server) return;

            newsListContainer.Controls.Clear();

            foreach (var item in items)
            {
                Panel newsCard = new Panel { Size = new Size(395, 36), Margin = new Padding(0, 0, 0, 4), Cursor = Cursors.Hand, BackColor = Color.FromArgb(20, 255, 255, 255) };

                Label lblTitle = new Label
                {
                    Text = item.Title,
                    Location = new Point(5, 3),
                    Size = new Size(385, 18),
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoEllipsis = true
                };

                Label lblDate = new Label
                {
                    Text = item.PublishDate.ToString("dd.MM.yyyy"),
                    Location = new Point(5, 20),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = server.AccentColor
                };

                newsCard.Controls.Add(lblTitle);
                newsCard.Controls.Add(lblDate);

                Action openLink = () => { try { Process.Start(new ProcessStartInfo { FileName = item.Url, UseShellExecute = true }); } catch { } };
                newsCard.Click += (s, e) => openLink();
                lblTitle.Click += (s, e) => openLink();
                lblDate.Click += (s, e) => openLink();

                newsListContainer.Controls.Add(newsCard);
            }
        }

        private List<NewsItem> GetGitHubJsonNews(string jsonUrl, string defaultDiscordUrl)
        {
            var list = new List<NewsItem>();

            if (!string.IsNullOrEmpty(jsonUrl))
            {
                try
                {
                    string json = netClient.GetStringAsync(jsonUrl).Result;

                    string[] rawItems = json.Split(new[] { "{" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var raw in rawItems)
                    {
                        if (!raw.Contains("title")) continue;

                        string title = ExtractJsonValue(raw, "title");
                        string url = ExtractJsonValue(raw, "url");
                        string dateStr = ExtractJsonValue(raw, "date");

                        if (string.IsNullOrEmpty(url)) url = defaultDiscordUrl;

                        DateTime date;
                        if (!DateTime.TryParse(dateStr, out date)) date = DateTime.Now;

                        list.Add(new NewsItem
                        {
                            Title = string.IsNullOrEmpty(title) ? "Новость проекта" : title,
                            Url = url,
                            PublishDate = date
                        });

                        if (list.Count >= 4) break;
                    }
                }
                catch { }
            }

            if (list.Count == 0)
            {
                list.Add(new NewsItem { Title = "📢 Перейти в Discord сообщество", Url = defaultDiscordUrl, PublishDate = DateTime.Now });
                list.Add(new NewsItem { Title = "💬 Чат игроков и поддержка", Url = defaultDiscordUrl, PublishDate = DateTime.Now.AddDays(-1) });
            }

            return list;
        }

        private string ExtractJsonValue(string jsonBlock, string key)
        {
            try
            {
                string search = $"\"{key}\":";
                int startIndex = jsonBlock.IndexOf(search);
                if (startIndex == -1) return "";

                startIndex += search.Length;
                int firstQuote = jsonBlock.IndexOf("\"", startIndex);
                int secondQuote = jsonBlock.IndexOf("\"", firstQuote + 1);

                if (firstQuote != -1 && secondQuote != -1)
                {
                    return jsonBlock.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                }
            }
            catch { }
            return "";
        }

        private void AddSocialButton(string text, string url)
        {
            Label btn = new Label
            {
                Text = text,
                Font = new Font("Segoe UI Black", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 200, 200),
                BackColor = Color.Transparent,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 5, 20, 0),
                Tag = url
            };

            btn.MouseEnter += (s, e) => { if (selectedServer != null) btn.ForeColor = selectedServer.AccentColor; };
            btn.MouseLeave += (s, e) => btn.ForeColor = Color.FromArgb(200, 200, 200);
            btn.Click += (s, e) => {
                if (btn.Tag != null) Process.Start(new ProcessStartInfo { FileName = btn.Tag.ToString(), UseShellExecute = true });
            };

            socialPanel.Controls.Add(btn);
        }

        private async void BtnClearCache_Click(object sender, EventArgs e)
        {
            string steamPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string;
            if (string.IsNullOrEmpty(steamPath))
            {
                MessageBox.Show("Не удалось найти Steam.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string gmodPath = Path.Combine(steamPath.Replace('/', '\\'), @"steamapps\common\GarrysMod\garrysmod");

            var confirm = MessageBox.Show("Очистить кэш игры для решения вылетов?", "Очистка кэша", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            btnClearCache.Enabled = false;
            btnPlay.Enabled = false;
            btnClearCache.Text = "ОЧИСТКА...";

            string[] folders = { Path.Combine(gmodPath, "cache"), Path.Combine(gmodPath, "downloads"), Path.Combine(gmodPath, "download") };

            int deleted = await Task.Run(() =>
            {
                int count = 0;
                foreach (var folder in folders)
                {
                    if (Directory.Exists(folder))
                    {
                        try
                        {
                            foreach (var file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
                            {
                                try { File.Delete(file); count++; } catch { }
                            }
                        }
                        catch { }
                    }
                }
                return count;
            });

            btnClearCache.Text = "ОЧИСТИТЬ КЭШ ИГРЫ";
            btnClearCache.Enabled = true;
            btnPlay.Enabled = true;

            MessageBox.Show($"Очищено файлов: {deleted}.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void BtnPlay_Click(object sender, EventArgs e)
        {
            if (selectedServer == null) return;

            if (isContentErrorState)
            {
                try { Process.Start(new ProcessStartInfo { FileName = selectedServer.CollectionUrl, UseShellExecute = true }); } catch { }

                btnPlay.Text = "ЗАПУСК ИГРЫ...";
                btnPlay.Enabled = false;
                await Task.Delay(800);

                Process.Start(new ProcessStartInfo { FileName = selectedServer.ConnectUrl, UseShellExecute = true });

                isContentErrorState = false;
                btnPlay.Text = "ПОДКЛЮЧИТЬСЯ";
                btnPlay.BackColor = selectedServer.AccentColor;
                btnPlay.Font = new Font("Segoe UI Black", 18, FontStyle.Bold);
                btnPlay.Enabled = true;
                return;
            }

            btnPlay.Enabled = false;
            string originalText = btnPlay.Text;

            try
            {
                if (!string.IsNullOrEmpty(selectedServer.WorkshopId))
                {
                    btnPlay.Text = "ПРОВЕРКА КОНТЕНТА...";
                    await Task.Run(() => DownloadContentViaSteamCMD(selectedServer.WorkshopId));
                }

                btnPlay.Text = "ЗАПУСК СЕРВЕРА...";
                await Task.Delay(800);

                Process.Start(new ProcessStartInfo { FileName = selectedServer.ConnectUrl, UseShellExecute = true });
                btnPlay.Text = originalText;
                btnPlay.Enabled = true;
            }
            catch
            {
                isContentErrorState = true;
                btnPlay.Enabled = true;
                btnPlay.BackColor = Color.FromArgb(205, 40, 40);
                btnPlay.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                btnPlay.Text = "СКАЧАТЬ В STEAM И ЗАПУСТИТЬ";
            }
        }

        private void DownloadContentViaSteamCMD(string workshopId)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string steamCmdDir = Path.Combine(rootPath, "SteamCMD");
            string steamCmdExe = Path.Combine(steamCmdDir, "steamcmd.exe");

            if (!File.Exists(steamCmdExe))
            {
                Directory.CreateDirectory(steamCmdDir);
                string zipPath = Path.Combine(steamCmdDir, "steamcmd.zip");

                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", zipPath);
                }

                ZipFile.ExtractToDirectory(zipPath, steamCmdDir);
                File.Delete(zipPath);
            }

            string arguments = $"+login anonymous +workshop_download_item 4000 {workshopId} validate +quit";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = steamCmdExe,
                Arguments = arguments,
                WorkingDirectory = steamCmdDir,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (Process process = Process.Start(startInfo))
            {
                if (process == null) throw new Exception();
                process.WaitForExit();
            }
        }

        private async Task LoadServerLiveImageAsync(PremiumServer server)
        {
            if (server.ImageUrls == null || server.ImageUrls.Count == 0) return;

            try
            {
                string randomUrl = server.ImageUrls[rnd.Next(server.ImageUrls.Count)];
                byte[] imageBytes = await netClient.GetByteArrayAsync(randomUrl);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image loadedImg = Image.FromStream(ms);
                    Bitmap finalBmp = new Bitmap(pbServerScreenshot.Width, pbServerScreenshot.Height);

                    using (Graphics g = Graphics.FromImage(finalBmp))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawImage(loadedImg, 0, 0, finalBmp.Width, finalBmp.Height);

                        using (LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 0), new Point(0, finalBmp.Height), Color.Transparent, Color.FromArgb(200, server.CardColor)))
                        {
                            g.FillRectangle(lgb, 0, 0, finalBmp.Width, finalBmp.Height);
                        }
                    }

                    if (selectedServer == server)
                    {
                        if (pbServerScreenshot.Image != null) pbServerScreenshot.Image.Dispose();
                        pbServerScreenshot.Image = finalBmp;
                    }
                }
            }
            catch { }
        }

        private GraphicsPath GetRoundRectPath(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
    }
}
