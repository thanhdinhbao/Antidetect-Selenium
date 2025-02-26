using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.DevTools.V133;
using OpenQA.Selenium.DevTools.V133.Emulation;
using OpenQA.Selenium.DevTools;

namespace Antidetect_Selenium
{
    public partial class FMain : Form
    {
        private List<(IWebDriver Driver, string Port, string ProfilePath)> drivers = new List<(IWebDriver, string, string)>(); // Lưu driver, port, và đường dẫn profile
        private int startPort = 9515; // Port mặc định cho ChromeDriver
        private string profilesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles"); // Thư mục Profiles
        private Random random = new Random();
        public FMain()
        {
            InitializeComponent();
            EnsureProfilesDirectoryExists();
            LoadExistingProfiles();
        }

        string GetNextProfileName()
        {
            string[] existingProfiles = Directory.GetDirectories(profilesDir, "Profile*")
                                                 .Select(Path.GetFileName)
                                                 .Where(name => name.StartsWith("Profile") && int.TryParse(name.Substring(7), out _))
                                                 .OrderBy(name => int.Parse(name.Substring(7)))
                                                 .ToArray();

            int nextProfileNumber = 1;
            foreach (string profile in existingProfiles)
            {
                if (int.Parse(profile.Substring(7)) != nextProfileNumber)
                {
                    break;
                }
                nextProfileNumber++;
            }

            return $"Profile{nextProfileNumber}";
        }

        private void EnsureProfilesDirectoryExists()
        {
            if (!Directory.Exists(profilesDir))
            {
                Directory.CreateDirectory(profilesDir);
            }
        }

        private void LoadExistingProfiles()
        {
            dataGridViewDrivers.Rows.Clear();
            string[] profileDirs = Directory.GetDirectories(profilesDir);
            foreach (string dir in profileDirs)
            {
                string configPath = Path.Combine(dir, "config.json");
                if (File.Exists(configPath))
                {
                    try
                    {
                        string json = File.ReadAllText(configPath);
                        var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        string port = config["Port"];
                        string profileName = Path.GetFileName(dir);

                        int rowIndex = dataGridViewDrivers.Rows.Add(profileName, port, "Không hoạt động");
                        DataGridViewButtonCell startButton = new DataGridViewButtonCell { Value = "Khởi tạo" };
                        DataGridViewButtonCell stopButton = new DataGridViewButtonCell { Value = "Tắt" };
                        DataGridViewButtonCell deleteButton = new DataGridViewButtonCell { Value = "Xóa" };
                        dataGridViewDrivers.Rows[rowIndex].Cells["Start"] = startButton;
                        dataGridViewDrivers.Rows[rowIndex].Cells["Stop"] = stopButton;
                        dataGridViewDrivers.Rows[rowIndex].Cells["Delete"] = deleteButton;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi tải profile {Path.GetFileName(dir)}: " + ex.Message);
                    }
                }
            }
        }

        private string GetRandomItem(ComboBox comboBox)
        {
            return comboBox.Items[random.Next(comboBox.Items.Count)].ToString();
        }

        private string[] RandomRes(int minw, int minh)
        {
            Random rand = new Random();
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int width = rand.Next(minw, screenWidth);
            int height = rand.Next(minh, screenHeight);

            return new string[] { width.ToString(), height.ToString() };
        }



        private void EnsureProfileDirectory(string profilePath)
        {
            if (!Directory.Exists(profilePath))
            {
                Directory.CreateDirectory(profilePath);
            }
            // Đảm bảo quyền truy cập
            try
            {
                File.WriteAllText(Path.Combine(profilePath, "test.txt"), "Test");
                File.Delete(Path.Combine(profilePath, "test.txt"));
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"Không có quyền truy cập vào thư mục profile: {profilePath}. Vui lòng chạy ứng dụng với quyền Admin.");
                throw;
            }
        }

        private async void btnStartDriver_Click(object sender, EventArgs e)
        {
            bool ext = false;
            try
            {
                int numToCreate = (int)numDrivers.Value;
                //dataGridViewDrivers.Rows.Clear();

                var tasks = new List<Task>();
                for (int i = 0; i < numToCreate; i++)
                {
                    int currentIndex = i;
                    tasks.Add(Task.Run(async () =>
                    {
                        await Task.Delay(100 * currentIndex);
                        ChromeOptions options = new ChromeOptions();

                        if (chkRandomParams.Checked)
                        {
                            string driver_info = GetRandomItem(cbDriverInfo);

                            string ua = GetRandomItem(cbUserAgent);
                            options.AddArgument($"--user-agent={ua}");

                            string resolution = GetRandomItem(cbScreenRes);
                            string[] res;

                            if (resolution == "Random")
                            {
                                res = RandomRes(800, 600);
                            }
                            else
                            {
                                res = resolution.Split('x');
                            }

                            options.AddArgument($"--window-size={res[0]},{res[1]}");


                            string timezone = GetRandomItem(cbTimezone);
                            options.AddArgument($"--timezone={timezone}");


                            string webGL = GetRandomItem(cbWebGL);
                            string audio = GetRandomItem(cbAudio);
                            string font = GetRandomItem(cbFont);
                            string webRTC = GetRandomItem(cbWebRTC);
                            string canvas = GetRandomItem(cbCanvas);
                            string cpu = GetRandomItem(cbCPU);
                            string ram = GetRandomItem(cbRAM);

                            if (cbExtension.Checked)
                            {
                                string path = Environment.CurrentDirectory + "\\ext\\Canvas_Fingerprint_Defender.crx";
                                options.AddExtension(path);
                                ext = true;
                            }

                            if (webRTC == "Disabled")
                            {
                                options.AddArgument("--disable-webrtc");
                                options.AddArgument("--disable-blink-features=AutomationControlled");
                                options.AddArgument("--disable-features=WebRTC-H264WithOpenH264FFmpeg");
                                options.AddArgument("--disable-ipc-flooding-protection");
                                options.AddArgument("--disable-webgl");
                                options.AddArgument("--disable-client-side-phishing-detection");
                                options.AddArgument("--disable-popup-blocking");
                            }

                            // Đảm bảo thư mục profile có quyền truy cập
                            string port = (startPort + currentIndex).ToString();
                            string profileName = GetNextProfileName();
                            string profilePath = Path.Combine(profilesDir, profileName);
                            EnsureProfileDirectory(profilePath);

                            options.AddArgument($"--user-data-dir={profilePath}");
                            options.AddArgument($"--remote-debugging-port={port}");
                            options.AddExcludedArgument("enable-automation");
                            options.AddAdditionalOption("useAutomationExtension", false);
                            options.AddUserProfilePreference("credentials_enable_service", false); // Tắt lưu mật khẩu
                            options.AddUserProfilePreference("profile.password_manager_enabled", false); // Tắt quản lý mật khẩu

                            IWebDriver driver = null;
                            try
                            {
                                ChromeDriverService cService = ChromeDriverService.CreateDefaultService();
                                cService.HideCommandPromptWindow = true;
                                driver = new ChromeDriver(cService, options);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khởi tạo driver trên port {port}: " + ex.Message);
                                return;
                            }

                            lock (drivers)
                            {
                                drivers.Add((driver, port, profilePath));
                            }

                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                            // Fake WebGL
                            js.ExecuteScript($@"
                                Object.defineProperty(window.WebGLRenderingContext.prototype, 'getParameter', {{
                                    value: function(param) {{
                                        if (param === 37445) return '{webGL}'; // VENDOR
                                        if (param === 37446) return 'Fake Renderer'; // RENDERER
                                        return null;
                                    }}
                                }});
                            ");

                            // Fake Canvas
                            if (canvas != "Default")
                            {
                                js.ExecuteScript(@"
                                    HTMLCanvasElement.prototype.getContext = function() {
                                        var ctx = this.__proto__.getContext.apply(this, arguments);
                                        ctx.fillRect = function() { 
                                            ctx.fillStyle = 'rgba(' + Math.random() * 255 + ',' + Math.random() * 255 + ',' + Math.random() * 255 + ',0.1)';
                                            ctx.fillRect.apply(this, arguments);
                                        };
                                        return ctx;
                                    };
                                ");
                            }

                            // Fake Audio
                            if (audio != "Default")
                            {
                                js.ExecuteScript("AudioContext.prototype.createOscillator = function() { return null; };");
                            }

                            // Fake Font
                            js.ExecuteScript($@"
                                Object.defineProperty(document, 'fonts', {{
                                    get: function() {{ return ['{font}']; }}
                                }});
                            ");

                            // Fake CPU
                            js.ExecuteScript($@"
                                Object.defineProperty(navigator, 'hardwareConcurrency', {{
                                    get: function() {{ return {(cpu.Contains("i7") ? 8 : cpu.Contains("i5") ? 4 : 6)}; }}
                                }});
                            ");

                            // Fake RAM
                            js.ExecuteScript($@"
                                Object.defineProperty(navigator, 'deviceMemory', {{
                                    get: function() {{ return {ram}; }}
                                }});
                            ");

                            // Lưu cấu hình profile vào file JSON
                            //sửa lưu json
                            Dictionary<string, string> config = new Dictionary<string, string>();

                            this.Invoke(new Action(() =>
                            {
                                string screenResolution = res[0] + "x" + res[1];
                                config = new Dictionary<string, string>
                                    {
                                        {"DriverInfo", driver_info},
                                        {"UserAgent", ua},
                                        {"ScreenResolution", screenResolution},
                                        {"Timezone", timezone},
                                        {"WebGL", webGL},
                                        {"Audio", audio},
                                        {"Font", font},
                                        {"WebRTC", webRTC},
                                        {"Canvas", canvas},
                                        {"CPU", cpu},
                                        {"RAM", ram},
                                        {"Ext", ext.ToString()},
                                        {"Port", port}
                                    };
                            }));

                            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                            File.WriteAllText(Path.Combine(profilePath, "config.json"), json);


                            int rowIndex = -1; // Khởi tạo rowIndex trước khi sử dụng

                            // Cập nhật DataGridView trên thread UI
                            this.Invoke(new Action(() =>
                            {
                                rowIndex = dataGridViewDrivers.Rows.Add(profileName, port, "Đang chạy");
                                DataGridViewButtonCell startButton = new DataGridViewButtonCell { Value = "Khởi tạo" };
                                DataGridViewButtonCell stopButton = new DataGridViewButtonCell { Value = "Tắt" };
                                DataGridViewButtonCell deleteButton = new DataGridViewButtonCell { Value = "Xóa" };
                                dataGridViewDrivers.Rows[rowIndex].Cells["Start"] = startButton;
                                dataGridViewDrivers.Rows[rowIndex].Cells["Stop"] = stopButton;
                                dataGridViewDrivers.Rows[rowIndex].Cells["Delete"] = deleteButton;
                            }));

                            driver.Navigate().GoToUrl("https://browserleaks.com/");

                            Thread.Sleep(3000);
                            driver.Close();
                            driver.Quit();

                            // Cập nhật trạng thái sau khi driver đóng
                            this.Invoke(new Action(() =>
                            {
                                if (rowIndex >= 0) // Kiểm tra rowIndex hợp lệ
                                {
                                    dataGridViewDrivers.Rows[rowIndex].Cells["Status"].Value = "Không hoạt động";
                                }
                            }));
                        }
                        else
                        {

                            this.BeginInvoke(new Action(() =>
                            {
                                string userAgent = cbUserAgent.SelectedItem.ToString();
                                string screenRes = cbScreenRes.SelectedItem.ToString();
                                string timezone = cbTimezone.SelectedItem.ToString();

                                string[] res;

                                if (screenRes == "Random")
                                {
                                    res = RandomRes(800, 600);
                                }
                                else
                                {
                                    res = screenRes.Split('x');
                                }
                                options.AddArgument($"--window-size={res[0]},{res[1]}");
                                options.AddArgument($"--user-agent={userAgent}");
                                options.AddArgument($"--timezone={timezone}");
                                options.AddExcludedArgument("enable-automation");
                                options.AddAdditionalOption("useAutomationExtension", false);
                                options.AddUserProfilePreference("credentials_enable_service", false); // Tắt lưu mật khẩu
                                options.AddUserProfilePreference("profile.password_manager_enabled", false); // Tắt quản lý mật khẩu

                                if (cbExtension.Checked)
                                {
                                    string path = Environment.CurrentDirectory + "\\ext\\Canvas_Fingerprint_Defender.crx";
                                    options.AddExtension(path);
                                    ext = true;
                                }

                                if (cbWebRTC.SelectedItem.ToString() == "Disabled")
                                {
                                    options.AddArgument("--disable-webrtc");
                                    options.AddArgument("--disable-blink-features=AutomationControlled");
                                    options.AddArgument("--disable-features=WebRTC-H264WithOpenH264FFmpeg");
                                    options.AddArgument("--disable-ipc-flooding-protection");
                                    options.AddArgument("--disable-webgl");
                                    options.AddArgument("--disable-client-side-phishing-detection");
                                    options.AddArgument("--disable-popup-blocking");
                                }
                                Random rnd = new Random();
                                string port = rnd.Next(9515, 9590).ToString();
                                //string port = (startPort + currentIndex).ToString();
                                string profileName = GetNextProfileName();
                                string profilePath = Path.Combine(profilesDir, profileName);
                                EnsureProfileDirectory(profilePath);

                                options.AddArgument($"--user-data-dir={profilePath}");
                                options.AddArgument($"--remote-debugging-port={port}");

                                IWebDriver driver = null;
                                try
                                {
                                    ChromeDriverService cService = ChromeDriverService.CreateDefaultService();
                                    cService.HideCommandPromptWindow = true;
                                    driver = new ChromeDriver(cService, options);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Lỗi khởi tạo driver trên port {port}: " + ex.Message);
                                    return;
                                }

                                lock (drivers)
                                {
                                    drivers.Add((driver, port, profilePath));
                                }

                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                                // Fake WebGL
                                js.ExecuteScript($@"
                                Object.defineProperty(window.WebGLRenderingContext.prototype, 'getParameter', {{
                                    value: function(param) {{
                                        if (param === 37445) return '{cbWebGL.SelectedItem}'; // VENDOR
                                        if (param === 37446) return 'Fake Renderer'; // RENDERER
                                        return null;
                                    }}
                                }});
                            ");

                                // Fake Canvas
                                if (cbCanvas.SelectedItem.ToString() != "Default")
                                {
                                    js.ExecuteScript(@"
                                    HTMLCanvasElement.prototype.getContext = function() {
                                        var ctx = this.__proto__.getContext.apply(this, arguments);
                                        ctx.fillRect = function() { 
                                            ctx.fillStyle = 'rgba(' + Math.random() * 255 + ',' + Math.random() * 255 + ',' + Math.random() * 255 + ',0.1)';
                                            ctx.fillRect.apply(this, arguments);
                                        };
                                        return ctx;
                                    };
                                ");
                                }

                                // Fake Audio
                                if (cbAudio.SelectedItem.ToString() != "Default")
                                {
                                    js.ExecuteScript("AudioContext.prototype.createOscillator = function() { return null; };");
                                }

                                // Fake Font
                                js.ExecuteScript($@"
                                Object.defineProperty(document, 'fonts', {{
                                    get: function() {{ return ['{cbFont.SelectedItem}']; }}
                                }});
                            ");

                                // Fake CPU
                                js.ExecuteScript($@"
                                Object.defineProperty(navigator, 'hardwareConcurrency', {{
                                    get: function() {{ return {(cbCPU.SelectedItem.ToString().Contains("i7") ? 8 : cbCPU.SelectedItem.ToString().Contains("i5") ? 4 : 6)}; }}
                                }});
                            ");

                                // Fake RAM
                                js.ExecuteScript($@"
                                Object.defineProperty(navigator, 'deviceMemory', {{
                                    get: function() {{ return {cbRAM.SelectedItem}; }}
                                }});
                            ");
                                string screenResolution = res[0] + "x" + res[1];
                                // Lưu cấu hình profile vào file JSON
                                var config = new Dictionary<string, string>
                                    {

                                        {"DriverInfo", cbDriverInfo.SelectedItem.ToString()},
                                        {"UserAgent", cbUserAgent.SelectedItem.ToString()},
                                        {"ScreenResolution", screenResolution},
                                        {"Timezone", cbTimezone.SelectedItem.ToString()},
                                        {"WebGL", cbWebGL.SelectedItem.ToString()},
                                        {"Audio", cbAudio.SelectedItem.ToString()},
                                        {"Font", cbFont.SelectedItem.ToString()},
                                        {"WebRTC", cbWebRTC.SelectedItem.ToString()},
                                        {"Canvas", cbCanvas.SelectedItem.ToString()},
                                        {"CPU", cbCPU.SelectedItem.ToString()},
                                        {"RAM", cbRAM.SelectedItem.ToString()},
                                        {"Ext", ext.ToString()},
                                        {"Port", port}
                                    };
                                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                                this.Invoke(new Action(() =>
                                {
                                    File.WriteAllText(Path.Combine(profilePath, "config.json"), json);
                                }));

                                int rowIndex = -1; // Khởi tạo rowIndex trước khi sử dụng

                                this.Invoke(new Action(() =>
                                {
                                    rowIndex = dataGridViewDrivers.Rows.Add(profileName, port, "Đang chạy");
                                    DataGridViewButtonCell startButton = new DataGridViewButtonCell { Value = "Khởi tạo" };
                                    DataGridViewButtonCell stopButton = new DataGridViewButtonCell { Value = "Tắt" };
                                    DataGridViewButtonCell deleteButton = new DataGridViewButtonCell { Value = "Xóa" };
                                    dataGridViewDrivers.Rows[rowIndex].Cells["Start"] = startButton;
                                    dataGridViewDrivers.Rows[rowIndex].Cells["Stop"] = stopButton;
                                    dataGridViewDrivers.Rows[rowIndex].Cells["Delete"] = deleteButton;
                                }));

                                driver.Navigate().GoToUrl("https://browserleaks.com/");

                                Thread.Sleep(3000);
                                driver.Close();
                                driver.Quit();

                                // Cập nhật trạng thái sau khi driver đóng
                                this.Invoke(new Action(() =>
                                {
                                    if (rowIndex >= 0) // Kiểm tra rowIndex hợp lệ
                                    {
                                        dataGridViewDrivers.Rows[rowIndex].Cells["Status"].Value = "Không hoạt động";
                                    }
                                }));
                            }));
                        }
                    }));
                }

                await Task.WhenAll(tasks).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        IEnumerable<Exception> exceptions = t.Exception?.InnerExceptions ?? Enumerable.Empty<Exception>();
                        foreach (var ex in exceptions)
                        {
                            MessageBox.Show($"Lỗi khi tạo driver: " + ex.Message);
                        }
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show($"{numToCreate} driver đã được khởi tạo cùng lúc!");
                        }));
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tổng quát: " + ex.Message);
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                var config = new Dictionary<string, string>
                {
                    {"DriverInfo", cbDriverInfo.SelectedItem.ToString()},
                    {"UserAgent", cbUserAgent.SelectedItem.ToString()},
                    {"ScreenResolution", cbScreenRes.SelectedItem.ToString()},
                    {"Timezone", cbTimezone.SelectedItem.ToString()},
                    {"WebGL", cbWebGL.SelectedItem.ToString()},
                    {"Audio", cbAudio.SelectedItem.ToString()},
                    {"Font", cbFont.SelectedItem.ToString()},
                    {"WebRTC", cbWebRTC.SelectedItem.ToString()},
                    {"Canvas", cbCanvas.SelectedItem.ToString()},
                    {"CPU", cbCPU.SelectedItem.ToString()},
                    {"RAM", cbRAM.SelectedItem.ToString()}
                };

                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText("config.json", json);
                MessageBox.Show("Đã lưu cấu hình vào config.json!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu cấu hình: " + ex.Message);
            }
        }

        private void btnStopDriver_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var (driver, _, profilePath) in drivers)
                {
                    if (driver != null)
                    {
                        driver.Quit(); // Dừng driver
                    }
                }
                drivers.Clear(); // Xóa danh sách driver
                dataGridViewDrivers.Rows.Clear(); // Xóa DataGridView
                MessageBox.Show("Đã tắt tất cả driver!");
                dataGridViewDrivers.Refresh();
                LoadExistingProfiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tắt driver: " + ex.Message);
            }
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is ComboBox cb && cb.Items.Count > 0)
                {
                    cb.SelectedIndex = 0;
                }
            }
        }

        private void dataGridViewDrivers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string port = dataGridViewDrivers.Rows[e.RowIndex].Cells["Port"].Value.ToString();
                int driverIndex = int.Parse(port) - startPort;

                if (e.ColumnIndex == dataGridViewDrivers.Columns["Start"].Index)
                {
                    if (driverIndex >= 0)
                    {
                        string name = dataGridViewDrivers.Rows[e.RowIndex].Cells["NameDriver"].Value.ToString();
                        string profilePath = Path.Combine(profilesDir, name);
                        string configPath = Path.Combine(profilePath, "config.json");

                        if (File.Exists(configPath))
                        {
                            try
                            {
                                string json = File.ReadAllText(configPath);
                                var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                                ChromeOptions options = new ChromeOptions();
                                options.AddArgument($"--user-agent={config["UserAgent"]}");
                                string[] resolution = config["ScreenResolution"].Split('x');
                                options.AddArgument($"--window-size={resolution[0]},{resolution[1]}");
                                options.AddArgument($"--timezone={config["Timezone"]}");
                                options.AddExcludedArgument("enable-automation");
                                options.AddAdditionalOption("useAutomationExtension", false);
                                options.AddUserProfilePreference("credentials_enable_service", false); // Tắt lưu mật khẩu
                                options.AddUserProfilePreference("profile.password_manager_enabled", false); // Tắt quản lý mật khẩu

                                if (config["WebRTC"] == "Disabled")
                                {
                                    options.AddArgument("--disable-webrtc");
                                    options.AddArgument("--disable-blink-features=AutomationControlled");
                                    options.AddArgument("--disable-features=WebRTC-H264WithOpenH264FFmpeg");
                                    options.AddArgument("--disable-ipc-flooding-protection");
                                    options.AddArgument("--disable-webgl");
                                    options.AddArgument("--disable-client-side-phishing-detection");
                                    options.AddArgument("--disable-popup-blocking");
                                }

                                if (config["Ext"] == "True")
                                {

                                    string path = Environment.CurrentDirectory + "\\ext\\Canvas_Fingerprint_Defender.crx";
                                    options.AddExtension(path);
                                }

                                options.AddArgument($"--user-data-dir={profilePath}");
                                options.AddArgument($"--remote-debugging-port={config["Port"]}");

                                ChromeDriverService cService = ChromeDriverService.CreateDefaultService();
                                cService.HideCommandPromptWindow = true;
                                IWebDriver driver = new ChromeDriver(cService, options);
                                //IWebDriver driver = new ChromeDriver(options);
                                lock (drivers)
                                {
                                    drivers.Add((driver, port, profilePath));
                                }

                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                                // Fake WebGL
                                js.ExecuteScript($@"
                                    Object.defineProperty(window.WebGLRenderingContext.prototype, 'getParameter', {{
                                        value: function(param) {{
                                            if (param === 37445) return '{config["WebGL"]}'; // VENDOR
                                            if (param === 37446) return 'Fake Renderer'; // RENDERER
                                            return null;
                                        }}
                                    }});
                                ");

                                // Fake Canvas
                                if (config["Canvas"] != "Default")
                                {
                                    js.ExecuteScript(@"
                                        HTMLCanvasElement.prototype.getContext = function() {
                                            var ctx = this.__proto__.getContext.apply(this, arguments);
                                            ctx.fillRect = function() { 
                                                ctx.fillStyle = 'rgba(' + Math.random() * 255 + ',' + Math.random() * 255 + ',' + Math.random() * 255 + ',0.1)';
                                                ctx.fillRect.apply(this, arguments);
                                            };
                                            return ctx;
                                        };
                                    ");
                                }

                                // Fake Audio
                                if (config["Audio"] != "Default")
                                {
                                    js.ExecuteScript("AudioContext.prototype.createOscillator = function() { return null; };");
                                }

                                // Fake Font
                                js.ExecuteScript($@"
                                    Object.defineProperty(document, 'fonts', {{
                                        get: function() {{ return ['{config["Font"]}']; }}
                                    }});
                                ");

                                // Fake CPU
                                int cpuCores = (config["CPU"].Contains("i7") ? 8 : config["CPU"].Contains("i5") ? 4 : 6);
                                js.ExecuteScript($@"
                                    Object.defineProperty(navigator, 'hardwareConcurrency', {{
                                        get: function() {{ return {cpuCores}; }}
                                    }});
                                ");

                                // Fake RAM
                                js.ExecuteScript($@"
                                    Object.defineProperty(navigator, 'deviceMemory', {{
                                        get: function() {{ return {config["RAM"]}; }}
                                    }});
                                ");

                                dataGridViewDrivers.Rows[e.RowIndex].Cells["Status"].Value = "Đang chạy";
                                dataGridViewDrivers.Rows[e.RowIndex].Cells["Stop"].Value = "Tắt"; // Đảm bảo nút "Tắt" hiển thị
                                MessageBox.Show($"Đã khởi tạo lại driver trên port {port}!");
                                driver.Navigate().GoToUrl("https://browserleaks.com/");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khởi tạo driver trên port {port}: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Không tìm thấy config.json cho profile trên port {port}!");
                        }
                    }
                }
                else if (e.ColumnIndex == dataGridViewDrivers.Columns["Stop"].Index)
                {

                    var driverInfo = drivers.FirstOrDefault(d => d.Port == port);

                    if (driverInfo.Driver != null)
                    {
                        try
                        {
                            driverInfo.Driver.Quit(); // Dừng driver
                            drivers.Remove(driverInfo); // Xóa khỏi danh sách

                            // Cập nhật UI
                            dataGridViewDrivers.Rows[e.RowIndex].Cells["Status"].Value = "Không hoạt động";
                            dataGridViewDrivers.Rows[e.RowIndex].Cells["Stop"].Value = null; // Xóa nút "Tắt"
                            MessageBox.Show($"Đã tắt driver trên port {port}!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi tắt driver trên port {port}: {ex.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy driver đang chạy trên port " + port);
                    }
                }
                else if (e.RowIndex >= 0 && dataGridViewDrivers.Columns[e.ColumnIndex].Name == "Delete")
                {
                    string profileName = dataGridViewDrivers.Rows[e.RowIndex].Cells["NameDriver"].Value.ToString();
                    string profilePath = Path.Combine(profilesDir, profileName);

                    if (Directory.Exists(profilePath))
                    {
                        DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa profile '{profileName}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                Directory.Delete(profilePath, true);
                                dataGridViewDrivers.Rows.RemoveAt(e.RowIndex);
                                MessageBox.Show($"Profile '{profileName}' đã được xóa thành công.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi xóa profile '{profileName}': {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Profile không tồn tại.");
                    }
                }
            }
        }
        public void StartSeleniumWithProxy(string proxyType, string proxy, string username = "", string password = "")
        {
            ChromeOptions options = new ChromeOptions();
            IWebDriver driver = null; // ✅ Khởi tạo driver là null để tránh lỗi CS0165

            try
            {
                switch (proxyType.ToLower())
                {
                    case "http":
                    case "https":
                        options.AddArgument($"--proxy-server={proxy}");
                        break;

                    case "socks5":
                        options.AddArgument($"--proxy-server=socks5://{proxy}");
                        break;

                    case "private":
                        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                        {
                            throw new ArgumentException("Username và Password không được để trống khi dùng proxy có xác thực.");
                        }
                        string extensionPath = CreateProxyAuthExtension(proxy, username, password);
                        options.AddExtension(extensionPath);
                        break;

                    default:
                        MessageBox.Show("Loại proxy không hợp lệ! Vui lòng chọn http, https, socks5 hoặc private.");
                        return; // ✅ Tránh tiếp tục chạy nếu proxy không hợp lệ
                }

                driver = new ChromeDriver(options);
                driver.Navigate().GoToUrl("https://whatismyipaddress.com/");
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            finally
            {
                if (driver != null) // ✅ Kiểm tra trước khi gọi Close() và Quit()
                {
                    driver.Close();
                    driver.Quit();
                }
            }
        }


        private static string CreateProxyAuthExtension(string proxy, string username, string password)
        {
            string extDir = Path.Combine(Directory.GetCurrentDirectory(), "proxy_extension");
            if (!Directory.Exists(extDir))
                Directory.CreateDirectory(extDir);

            string manifestJson = @"{
            ""manifest_version"": 2,
            ""name"": ""Proxy Authentication Plugin"",
            ""version"": ""1.0"",
            ""permissions"": [""proxy"", ""tabs"", ""unlimitedStorage"", ""storage"", ""<all_urls>"", ""webRequest"", ""webRequestBlocking""],
            ""background"": { ""scripts"": [""background.js""] }
        }";

            string backgroundJs = $@"
        var config = {{
            mode: 'fixed_servers',
            rules: {{
                singleProxy: {{
                    scheme: 'http',
                    host: '{proxy.Split(':')[0]}',
                    port: parseInt({proxy.Split(':')[1]})
                }},
                bypassList: ['localhost']
            }}
        }};

        chrome.proxy.settings.set({{ value: config, scope: 'regular' }}, function() {{}});
        chrome.webRequest.onAuthRequired.addListener(
            function(details) {{
                return {{
                    authCredentials: {{ username: '{username}', password: '{password}' }}
                }};
            }},
            {{ urls: ['<all_urls>'] }},
            ['blocking']
        );";

            File.WriteAllText(Path.Combine(extDir, "manifest.json"), manifestJson, Encoding.UTF8);
            File.WriteAllText(Path.Combine(extDir, "background.js"), backgroundJs, Encoding.UTF8);

            string zipPath = Path.Combine(Directory.GetCurrentDirectory(), "proxy_auth_plugin.zip");
            if (File.Exists(zipPath)) File.Delete(zipPath);

            ZipFile.CreateFromDirectory(extDir, zipPath);
            return zipPath;
        }


        void test()
        {
            var options = new ChromeOptions();

            // Thiết lập User-Agent để giả mạo phần cứng
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            // Ẩn navigator.webdriver để tránh bị phát hiện là bot
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);

            options.AddExtension(@"C:\Users\Dinh Bao Thanh\Downloads\Canvas_Fingerprint_Defender.crx");

            //options.AddArgument("--disable-blink-features=AutomationControlled");
            //options.AddArgument("--disable-features=WebRTC-H264WithOpenH264FFmpeg");
            //options.AddArgument("--disable-ipc-flooding-protection");
            //options.AddArgument("--disable-webgl");
            //options.AddArgument("--disable-client-side-phishing-detection");
            //options.AddArgument("--disable-popup-blocking");

            // Thiết lập Preferences bằng AddUserProfilePreference()
            //options.AddUserProfilePreference("profile.managed_default_content_settings.images", 2); // Tắt ảnh
            options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2); // Tắt thông báo
            options.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 2); // Chặn định vị
            options.AddUserProfilePreference("profile.default_content_setting_values.media_stream_mic", 2); // Chặn micro
            options.AddUserProfilePreference("profile.default_content_setting_values.media_stream_camera", 2); // Chặn camera
            options.AddUserProfilePreference("credentials_enable_service", false); // Tắt lưu mật khẩu
            options.AddUserProfilePreference("profile.password_manager_enabled", false); // Tắt quản lý mật khẩu

            // Khởi tạo ChromeDriver với các thiết lập trên
            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.whatismybrowser.com/");
        }

        public void ChangeGPU()
        {
            // Tạo ChromeOptions
            ChromeOptions options = new ChromeOptions();

            // Tắt GPU thực tế (tùy chọn)
            options.AddArgument("--disable-gpu");

            // Giả mạo User-Agent (tùy chọn)
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36");

            // Khởi tạo ChromeDriver
            IWebDriver driver = new ChromeDriver(options);

            try
            {
                // Mở trang kiểm tra WebGL
                driver.Navigate().GoToUrl("https://browserleaks.com/webgl");

                // Ép kiểu driver thành IJavaScriptExecutor để tiêm script
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // Script JavaScript để giả mạo thông tin GPU thành NVIDIA RTX 4090
                string fakeGpuScript = @"
                (function() {
                    // Ghi đè WebGLRenderingContext
                    const getParameterOriginal = WebGLRenderingContext.prototype.getParameter;
                    WebGLRenderingContext.prototype.getParameter = function(parameter) {
                        if (parameter === 0x1F00) { // GL_VENDOR
                            return 'NVIDIA'; // Nhà cung cấp giả: NVIDIA
                        }
                        if (parameter === 0x1F01) { // GL_RENDERER
                            return 'RTX 4090'; // Tên GPU giả: RTX 4090
                        }
                        return getParameterOriginal.apply(this, arguments);
                    };

                    // Ghi đè WEBGL_debug_renderer_info
                    const getExtensionOriginal = WebGLRenderingContext.prototype.getExtension;
                    WebGLRenderingContext.prototype.getExtension = function(name) {
                        if (name === 'WEBGL_debug_renderer_info') {
                            return {
                                UNMASKED_VENDOR_WEBGL: 0x9245,
                                UNMASKED_RENDERER_WEBGL: 0x9246,
                                getParameter: function(param) {
                                    if (param === 0x9245) return 'NVIDIA'; // Vendor: NVIDIA
                                    if (param === 0x9246) return 'RTX 4090'; // Renderer: RTX 4090
                                    return null;
                                }
                            };
                        }
                        return getExtensionOriginal.apply(this, arguments);
                    };
                })();
            ";

                // Thực thi script giả mạo
                js.ExecuteScript(fakeGpuScript);

                // Dừng lại để xem kết quả (5 giây)
                System.Threading.Thread.Sleep(1000);
                driver.Navigate().GoToUrl("https://browserleaks.com/webgl");
                System.Threading.Thread.Sleep(15000);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            finally
            {
                // Đóng trình duyệt
                driver.Quit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeGPU();
        }
    }
}
