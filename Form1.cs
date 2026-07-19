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
        public string WorkshopId { get; set; }
        public string CollectionUrl => $"https://steamcommunity.com/sharedfiles/filedetails/?id={WorkshopId}";
        public string Description { get; set; }
        public List<string> ImageUrls { get; set; }

        public Color BgColor { get; set; }
        public Color CardColor { get; set; }
        public Color AccentColor { get; set; }
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
        private Button btnVk;

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
                    Name = "FNAF RP КР",
                    ConnectUrl = "steam://connect/46.174.48.152:27017",
                    IpAddress = "46.174.48.152:27017",
                    VkUrl = "https://vk.com/fnafrp",
                    WorkshopId = "3543269106",
                    ImageUrls = new List<string>
                    {
                        "https://urf.im/static/img/fnaf_russia/fnaf4.png",
                        "https://urf.im/static/img/fnaf_russia/fnaf5.png",
                        "https://urf.im/static/img/fnaf_russia/fnaf1.png"
                    },
                    Description = "FNAF RP - сервер в многопользовательской и garry's mod, позволяющий окунуться в atmosphere увлекательной и по своему страшной вселенной Five Nights at Freddy's. Именно ты решаешь кем ты хочешь стать: жутким аниматроником, работником пиццерии, блатным гопником, или простым городским жителем. Огромное количество профессий, игровых механик, и многого другого уже ждет тебя на сервере!\r\n\r\nНастало время выбрать свою роль в этом увлекательном мире вселенной Five Nights at Freddy's!",
                    BgColor = Color.FromArgb(40, 12, 35), CardColor = Color.FromArgb(24, 6, 20), AccentColor = Color.FromArgb(210, 80, 230)
                },
                new PremiumServer
                {
                    Name = "HL2RP ALYX",
                    ConnectUrl = "steam://connect/46.174.48.152:27020",
                    IpAddress = "46.174.48.152:27020",
                    VkUrl = "https://vk.com/alyxrp",
                    WorkshopId = "3704341067",
                    ImageUrls = new List<string> { "https://urf.im/static/img/alyx/screen_2.jpg", "https://urf.im/static/img/alyx/screen_4.jpg" },
                    Description = "HLRP:Alyx - сервер в многопользовательской и garry's mod, где вы полностью погрузитесь во времена Half Life 2 после 7 часовой войны, когда люди начали убегать из городов и организовывать повстанческие группировки, и когда Гордон Фримэн примкнул к повстанцам и рука об руку начал с ними спасать мир от порабощения Альянсом сразитесь с Universal Union, которые недалекие умы называют комбайнами.\r\n\r\nПритеснение жителей Альянсом, сопротивление Повстанцев, рейды, перестрелки, шпионские вылазки, зачистки - всё это ждёт вас в мрачном порабощенном городе на нашем ХЛ2РП сервере.",
                    BgColor = Color.FromArgb(38, 24, 15), CardColor = Color.FromArgb(24, 14, 8), AccentColor = Color.FromArgb(245, 150, 70)
                },
                new PremiumServer
                {
                    Name = "METRO RP",
                    ConnectUrl = "steam://connect/46.174.50.193:27018",
                    IpAddress = "46.174.50.193:27018",
                    VkUrl = "https://vk.com/metro2033urfim",
                    WorkshopId = "1750726415",
                    ImageUrls = new List<string> { "https://urf.im/static/img/metro/img_5.jpg", "https://urf.im/static/img/metro/img_2.jpg" },
                    Description = "Метро RP \"Мертвая Москва\" - это RP сервер в многопользовательской игре garry's mod, который закинет Вас в Москву, пережившую ядерную войну, чьи жители укрылись в местном городском метрополитене и начали там, на меленьких станциях и огромных туннелях, новую жизнь.\r\n\r\nСтань одним из жителей метро, начни свою историю, пройдя путь от обычного доходяги до предводителя одного из государств метрополитена!",
                    BgColor = Color.FromArgb(22, 22, 22), CardColor = Color.FromArgb(14, 14, 14), AccentColor = Color.FromArgb(255, 75, 25)
                },
                new PremiumServer
                {
                    Name = "HALF-LIFE 2 RP",
                    ConnectUrl = "steam://connect/46.174.48.152:27016",
                    IpAddress = "46.174.48.152:27016",
                    VkUrl = "https://vk.com/hl2rp",
                    WorkshopId = "2800996404",
                    ImageUrls = new List<string> { "https://urf.im/static/img/hl2/screen_2.png", "https://urf.im/static/img/hl2/screen_5.png" },
                    Description = "HL2RP За Фрименом - это HL2RP сервер в многопользовательской игре garry's mod, где вы полностью погрузитесь во времена Half Life 2 после 7 часовой войны, когда люди начали убегать из городов и организовывать повстанческие группировки, и когда Гордон Фримэн примкнул к повстанцам и рука об руку начал с ними спасать мир от порабощения Альянсом сразитесь с Universal Union, которые недалекие умы называют комбайнами.\r\n\r\nПритеснение жителей Альянсом, сопротивление Повстанцев, рейды, перестрелки, шпионские вылазки, зачистки - всё это ждёт вас в мрачном порабощенном городе на нашем ХЛ2РП сервере.",
                    BgColor = Color.FromArgb(25, 30, 36), CardColor = Color.FromArgb(14, 18, 22), AccentColor = Color.FromArgb(40, 230, 160)
                },
                new PremiumServer
                {
                    Name = "WW2 RP",
                    ConnectUrl = "steam://connect/46.174.50.193:27017",
                    IpAddress = "46.174.50.193:27017",
                    VkUrl = "https://vk.com/ww2rp",
                    WorkshopId = "3591521878",
                    ImageUrls = new List<string> { "https://urf.im/static/img/ww2/screen_3.png", "https://urf.im/static/img/ww2/screen_5.png" },
                    Description = "ВТОРАЯ МИРОВАЯ ВОЙНА, жизнь в оккупации - это Serious RP сервер в многопользовательской игре garry's mod, где вам предстоит ощутить на себе все тяготы и лишения жизни в оккупированном Париже. На дворе 1942 год, год определивший исход войны.\r\n\r\nВоенные парады, тяжелые дни оккупации, бравые офицеры, продажные генералы, нстоящие герои войны - всё это ждёт вас в оккупированном Париже.",
                    BgColor = Color.FromArgb(28, 30, 22), CardColor = Color.FromArgb(16, 18, 12), AccentColor = Color.FromArgb(220, 175, 95)
                },
                new PremiumServer
                {
                    Name = "SCP RP",
                    ConnectUrl = "steam://connect/46.174.48.152:27018",
                    IpAddress = "46.174.48.152:27018",
                    VkUrl = "https://vk.com/roleplayscp",
                    WorkshopId = "3662413720",
                    ImageUrls = new List<string> { "https://urf.im/static/img/scp/screen_3.jpg", "https://urf.im/static/img/scp/screen_4.jpg" },
                    Description = "SCP RP Containment Breach - это RP сервер в многопользовательской игре garry's mod, где тебе придётся пройти не легкий путь для того что бы выбраться из бункера ЗОНЫ 51, где исследуются обьекты, которые смертельно опасны и страшны для людей. Конечно ты сможешь выбрать другой путь и сам поиграть за смертельно опасного SCP.\r\n\r\nОхранники, Страшные SCP, атмосфера ЗОНЫ 51, головокружительные захватывающие локации - всё это про наш SCP RP .",
                    BgColor = Color.FromArgb(16, 24, 38), CardColor = Color.FromArgb(10, 15, 24), AccentColor = Color.FromArgb(30, 180, 255)
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

            lblDescription = new Label { Location = new Point(670, 110), Size = new Size(440, 345), Font = new Font("Segoe UI Semibold", 10.5f), ForeColor = Color.White, TextAlign = ContentAlignment.TopLeft };
            this.Controls.Add(lblDescription);

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

            btnVk = new Button { Text = "VKONTAKTE", Font = new Font("Segoe UI Black", 9, FontStyle.Bold), ForeColor = Color.FromArgb(200, 200, 200), FlatStyle = FlatStyle.Flat, AutoSize = true, Cursor = Cursors.Hand, Margin = new Padding(0, 0, 20, 0) };
            btnVk.FlatAppearance.BorderSize = 0;
            btnVk.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnVk.MouseEnter += (s, e) => { if (selectedServer != null && !isContentErrorState) btnVk.ForeColor = selectedServer.AccentColor; };
            btnVk.MouseLeave += (s, e) => btnVk.ForeColor = Color.FromArgb(200, 200, 200);
            btnVk.Click += (s, e) => { if (btnVk.Tag != null) Process.Start(new ProcessStartInfo { FileName = btnVk.Tag.ToString(), UseShellExecute = true }); };
            socialPanel.Controls.Add(btnVk);

            AddSocialButton("DISCORD", "https://discord.com/invite/urf-im-219688289286750209");
            AddSocialButton("WEBSITE", "https://urf.im");
        }

        private void SelectServer(PremiumServer server)
        {
            selectedServer = server;
            lblDescription.Text = server.Description;

            isContentErrorState = false;
            btnPlay.Text = "ПОДКЛЮЧИТЬСЯ";
            btnPlay.Font = new Font("Segoe UI Black", 18, FontStyle.Bold);

            this.BackColor = server.BgColor;
            lblLogo.ForeColor = server.AccentColor;
            btnPlay.BackColor = server.AccentColor;

            btnVk.Tag = server.VkUrl;

            foreach (Button btn in topMenu.Controls)
            {
                btn.ForeColor = (btn.Tag == server) ? server.AccentColor : Color.FromArgb(160, 160, 175);
            }

            _ = LoadServerLiveImageAsync(server);
        }

        private async void BtnClearCache_Click(object sender, EventArgs e)
        {
            string steamPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string;
            if (string.IsNullOrEmpty(steamPath))
            {
                MessageBox.Show("Не удалось автоматически найти установленный Steam в системе.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string gmodPath = Path.Combine(steamPath.Replace('/', '\\'), @"steamapps\common\GarrysMod\garrysmod");

            if (!Directory.Exists(gmodPath))
            {
                MessageBox.Show("Папка Garry's Mod не найдена по стандартному пути Steam. Убедитесь, что игра установлена.", "Папка не найдена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Вы действительно хотите очистить кэш игры?\nЭто удалит временные файлы, загруженные аддоны серверов и исправит большинство Lua-ошибок / вылетов.", "Очистка кэша", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            // Блокировка интерфейса во избежание двойного нажатия
            btnClearCache.Enabled = false;
            btnPlay.Enabled = false;
            string originalText = btnClearCache.Text;
            btnClearCache.Text = "ОЧИСТКА... ПОЖАЛУЙСТА, ПОДОЖДИТЕ";

            string[] foldersToClean = new string[]
            {
                Path.Combine(gmodPath, "cache"),
                Path.Combine(gmodPath, "downloads"),
                Path.Combine(gmodPath, "download"),
                Path.Combine(gmodPath, "bin")
            };

            // Перенос тяжелой операции в фоновый Task.Run, чтобы избежать намертво зависшего UI
            int deletedFilesCount = await Task.Run(() =>
            {
                int count = 0;
                foreach (var folder in foldersToClean)
                {
                    if (Directory.Exists(folder))
                    {
                        try
                        {
                            foreach (var file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
                            {
                                try { File.Delete(file); count++; } catch { }
                            }
                            foreach (var subDir in Directory.GetDirectories(folder))
                            {
                                try { Directory.Delete(subDir, true); } catch { }
                            }
                        }
                        catch { }
                    }
                }
                return count;
            });

            // Восстановление UI
            btnClearCache.Text = originalText;
            btnClearCache.Enabled = true;
            btnPlay.Enabled = true;

            MessageBox.Show($"Очистка успешно завершена!\nУдалено файлов: {deletedFilesCount}.\nРекомендуется перезапустить лаунчер или Steam перед входом.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (Exception)
            {
                isContentErrorState = true;
                btnPlay.Enabled = true;

                btnPlay.BackColor = Color.FromArgb(205, 40, 40);
                btnPlay.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                btnPlay.Text = "СКАЧАТЬ В STEAM И ЗАПУСТИТЬ";

                MessageBox.Show("Рекомендуем скачать контент самостоятельно для избежания долгих загрузок и ERROR.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (process == null) throw new Exception("Ошибка запуска SteamCMD.");
                process.WaitForExit();
                if (process.ExitCode != 0) throw new Exception($"SteamCMD ошибка: {process.ExitCode}");
            }
        }

        private async Task LoadServerLiveImageAsync(PremiumServer server)
        {
            if (server.ImageUrls == null || server.ImageUrls.Count == 0)
            {
                GenerateProceduralCard(server);
                return;
            }

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

                        using (Pen gridPen = new Pen(Color.FromArgb(25, server.AccentColor), 1))
                        {
                            for (int i = 0; i < finalBmp.Width; i += 30) g.DrawLine(gridPen, i, 0, i, finalBmp.Height);
                            for (int j = 0; j < finalBmp.Height; j += 30) g.DrawLine(gridPen, 0, j, finalBmp.Width, j);
                        }
                    }

                    if (selectedServer == server)
                    {
                        if (pbServerScreenshot.Image != null) pbServerScreenshot.Image.Dispose();
                        pbServerScreenshot.Image = finalBmp;
                    }
                }
            }
            catch
            {
                GenerateProceduralCard(server);
            }
        }

        private void GenerateProceduralCard(PremiumServer server)
        {
            Bitmap bmp = new Bitmap(pbServerScreenshot.Width, pbServerScreenshot.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 0), new Point(bmp.Width, bmp.Height), server.CardColor, Color.FromArgb(10, Color.Black)))
                {
                    g.FillRectangle(lgb, 0, 0, bmp.Width, bmp.Height);
                }

                using (Pen gridPen = new Pen(Color.FromArgb(30, server.AccentColor), 1))
                {
                    for (int i = 0; i < bmp.Width; i += 25) g.DrawLine(gridPen, i, 0, i, bmp.Height);
                    for (int j = 0; j < bmp.Height; j += 25) g.DrawLine(gridPen, 0, j, bmp.Width, j);
                }

                using (SolidBrush accentBrush = new SolidBrush(Color.FromArgb(100, server.AccentColor)))
                {
                    g.FillRectangle(accentBrush, 0, 0, 8, bmp.Height);
                }

                Font nameFont = new Font("Segoe UI Black", 24, FontStyle.Bold);
                Size textSize = TextRenderer.MeasureText(server.Name.ToUpper(), nameFont);
                int posX = (bmp.Width - textSize.Width) / 2;
                int posY = (bmp.Height - textSize.Height) / 2;

                TextRenderer.DrawText(g, server.Name.ToUpper(), nameFont, new Point(posX + 2, posY + 2), Color.FromArgb(40, server.AccentColor));
                TextRenderer.DrawText(g, server.Name.ToUpper(), nameFont, new Point(posX, posY), Color.White);

                Font subFont = new Font("Segoe UI", 9, FontStyle.Bold);
                TextRenderer.DrawText(g, $"// SYSTEM READY // PROTOCOL ACTIVE", subFont, new Point(25, bmp.Height - 35), Color.FromArgb(140, server.AccentColor));
            }

            if (pbServerScreenshot.Image != null) pbServerScreenshot.Image.Dispose();
            pbServerScreenshot.Image = bmp;
        }

        private void AddSocialButton(string text, string url)
        {
            Button btn = new Button { Text = text, Font = new Font("Segoe UI Black", 9, FontStyle.Bold), ForeColor = Color.FromArgb(200, 200, 200), FlatStyle = FlatStyle.Flat, AutoSize = true, Cursor = Cursors.Hand, Margin = new Padding(0, 0, 20, 0) };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.MouseEnter += (s, e) => { if (selectedServer != null && !isContentErrorState) btn.ForeColor = selectedServer.AccentColor; };
            btn.MouseLeave += (s, e) => btn.ForeColor = Color.FromArgb(200, 200, 200);
            btn.Click += (s, e) => Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            socialPanel.Controls.Add(btn);
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
