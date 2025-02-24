using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public int RandPort(int min, int max)
        {
            return random.Next(min, max + 1); // +1 để bao gồm cả max
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
            try
            {
                int numToCreate = (int)numDrivers.Value;
                //dataGridViewDrivers.Rows.Clear();

                var tasks = new List<Task>();
                for (int i = 0; i < numToCreate; i++)
                {
                    int currentIndex = i; // Capture the loop variable for the lambda
                    tasks.Add(Task.Run(async () =>
                    {
                        await Task.Delay(100 * currentIndex); // Thêm delay để giảm xung đột (100ms * index)
                        ChromeOptions options = new ChromeOptions();

                        // Random các thông số nếu checkbox được tick
                        if (chkRandomParams.Checked)
                        {
                            string driver_info = GetRandomItem(cbDriverInfo);

                            string ua = GetRandomItem(cbUserAgent);
                            options.AddArgument($"--user-agent={ua}");

                            string resolution = GetRandomItem(cbScreenRes);
                            string[] resParts = resolution.Split('x');
                            options.AddArgument($"--window-size={resParts[0]},{resParts[1]}");

                            string timezone = GetRandomItem(cbTimezone);
                            options.AddArgument($"--timezone={timezone}");

                            string webGL = GetRandomItem(cbWebGL);
                            string audio = GetRandomItem(cbAudio);
                            string font = GetRandomItem(cbFont);
                            string webRTC = GetRandomItem(cbWebRTC);
                            string canvas = GetRandomItem(cbCanvas);
                            string cpu = GetRandomItem(cbCPU);
                            string ram = GetRandomItem(cbRAM);


                            if (webRTC == "Disabled")
                            {
                                options.AddArgument("--disable-webrtc");
                            }

                            // Đảm bảo thư mục profile có quyền truy cập
                            string port = (startPort + currentIndex).ToString();
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
                                config = new Dictionary<string, string>
                                    {
                                        {"DriverInfo", driver_info},
                                        {"UserAgent", ua},
                                        {"ScreenResolution", resolution},
                                        {"Timezone", timezone},
                                        {"WebGL", webGL},
                                        {"Audio", audio},
                                        {"Font", font},
                                        {"WebRTC", webRTC},
                                        {"Canvas", canvas},
                                        {"CPU", cpu},
                                        {"RAM", ram},
                                        {"Port", port}
                                    };
                            }));

                            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                            File.WriteAllText(Path.Combine(profilePath, "config.json"), json);


                            // Cập nhật DataGridView trên thread UI
                            this.Invoke(new Action(() =>
                            {
                                int rowIndex = dataGridViewDrivers.Rows.Add(profileName, port, "Đang chạy");
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

                        }
                        else
                        {

                            this.BeginInvoke(new Action(() =>
                            {
                                string userAgent = cbUserAgent.SelectedItem?.ToString() ?? "";
                                string screenRes = cbScreenRes.SelectedItem?.ToString() ?? "1920x1080";
                                string timezone = cbTimezone.SelectedItem?.ToString() ?? "";

                                string[] resolution = screenRes.Split('x');

                                options.AddArgument($"--user-agent={userAgent}");
                                options.AddArgument($"--window-size={resolution[0]},{resolution[1]}");
                                options.AddArgument($"--timezone={timezone}");

                                if (cbWebRTC.SelectedItem.ToString() == "Disabled")
                                {
                                    options.AddArgument("--disable-webrtc");
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

                                // Lưu cấu hình profile vào file JSON
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
                                {"RAM", cbRAM.SelectedItem.ToString()},
                                {"Port", port}
                            };
                                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                                this.Invoke(new Action(() =>
                                {
                                    File.WriteAllText(Path.Combine(profilePath, "config.json"), json);
                                }));


                                // Cập nhật DataGridView trên thread UI
                                this.Invoke(new Action(() =>
                                {
                                    int rowIndex = dataGridViewDrivers.Rows.Add(profileName, port, "Đang chạy");
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
                            }));
                        }
                    }));
                }

                // Chờ tất cả task hoàn thành và xử lý exception
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

                                if (config["WebRTC"] == "Disabled")
                                {
                                    options.AddArgument("--disable-webrtc");
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
    }
}
