namespace Antidetect_Selenium
{
    partial class FMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDriverInfo = new System.Windows.Forms.Label();
            this.cbDriverInfo = new System.Windows.Forms.ComboBox();
            this.lblUserAgent = new System.Windows.Forms.Label();
            this.cbUserAgent = new System.Windows.Forms.ComboBox();
            this.lblScreenRes = new System.Windows.Forms.Label();
            this.cbScreenRes = new System.Windows.Forms.ComboBox();
            this.lblTimezone = new System.Windows.Forms.Label();
            this.cbTimezone = new System.Windows.Forms.ComboBox();
            this.lblWebGL = new System.Windows.Forms.Label();
            this.cbWebGL = new System.Windows.Forms.ComboBox();
            this.lblAudio = new System.Windows.Forms.Label();
            this.cbAudio = new System.Windows.Forms.ComboBox();
            this.lblFont = new System.Windows.Forms.Label();
            this.cbFont = new System.Windows.Forms.ComboBox();
            this.lblWebRTC = new System.Windows.Forms.Label();
            this.cbWebRTC = new System.Windows.Forms.ComboBox();
            this.lblCanvas = new System.Windows.Forms.Label();
            this.cbCanvas = new System.Windows.Forms.ComboBox();
            this.lblCPU = new System.Windows.Forms.Label();
            this.cbCPU = new System.Windows.Forms.ComboBox();
            this.lblRAM = new System.Windows.Forms.Label();
            this.cbRAM = new System.Windows.Forms.ComboBox();
            this.chkRandomParams = new System.Windows.Forms.CheckBox();
            this.lblNumDrivers = new System.Windows.Forms.Label();
            this.numDrivers = new System.Windows.Forms.NumericUpDown();
            this.btnStartDriver = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnStopDriver = new System.Windows.Forms.Button();
            this.dataGridViewDrivers = new System.Windows.Forms.DataGridView();
            this.NameDriver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbExtension = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numDrivers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDriverInfo
            // 
            this.lblDriverInfo.Location = new System.Drawing.Point(20, 20);
            this.lblDriverInfo.Name = "lblDriverInfo";
            this.lblDriverInfo.Size = new System.Drawing.Size(150, 23);
            this.lblDriverInfo.TabIndex = 1;
            this.lblDriverInfo.Text = "Driver Info:";
            // 
            // cbDriverInfo
            // 
            this.cbDriverInfo.Items.AddRange(new object[] {
            "Chrome 114",
            "Chrome 115",
            "Firefox 102"});
            this.cbDriverInfo.Location = new System.Drawing.Point(170, 20);
            this.cbDriverInfo.Name = "cbDriverInfo";
            this.cbDriverInfo.Size = new System.Drawing.Size(437, 21);
            this.cbDriverInfo.TabIndex = 2;
            // 
            // lblUserAgent
            // 
            this.lblUserAgent.Location = new System.Drawing.Point(20, 50);
            this.lblUserAgent.Name = "lblUserAgent";
            this.lblUserAgent.Size = new System.Drawing.Size(150, 23);
            this.lblUserAgent.TabIndex = 3;
            this.lblUserAgent.Text = "User Agent:";
            // 
            // cbUserAgent
            // 
            this.cbUserAgent.Items.AddRange(new object[] {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/114.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/115.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Edge/114.0.1823.51 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Firefox/115.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like G" +
                "ecko) Chrome/115.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like G" +
                "ecko) Chrome/114.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_6) AppleWebKit/537.36 (KHTML, like Geck" +
                "o) Firefox/116.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 12_5) AppleWebKit/537.36 (KHTML, like Geck" +
                "o) Safari/605.1.15",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_0) AppleWebKit/537.36 (KHTML, like Geck" +
                "o) Chrome/117.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Linux; Android 10; SM-G960F) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/114.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Linux; Android 11; SM-A526B) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Linux; Android 12; Pixel 6 Pro) AppleWebKit/537.36 (KHTML, like Geck" +
                "o) Chrome/117.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Linux; Android 13; SM-S918B) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/118.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Linux; Android 9; Redmi Note 7) AppleWebKit/537.36 (KHTML, like Geck" +
                "o) Chrome/113.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/537.36 (KHTML," +
                " like Gecko) Version/15.0 Mobile/15E148 Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 16_1 like Mac OS X) AppleWebKit/537.36 (KHTML," +
                " like Gecko) Version/16.1 Mobile/16E232 Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/537.36 (KHTML," +
                " like Gecko) Version/17.0 Mobile/17E258 Safari/537.36",
            "Mozilla/5.0 (iPad; CPU OS 16_1 like Mac OS X) AppleWebKit/537.36 (KHTML, like Gec" +
                "ko) Version/16.1 Mobile/15E216 Safari/537.36",
            "Mozilla/5.0 (iPad; CPU OS 17_0 like Mac OS X) AppleWebKit/537.36 (KHTML, like Gec" +
                "ko) Version/17.0 Mobile/17E518 Safari/537.36"});
            this.cbUserAgent.Location = new System.Drawing.Point(170, 50);
            this.cbUserAgent.Name = "cbUserAgent";
            this.cbUserAgent.Size = new System.Drawing.Size(437, 21);
            this.cbUserAgent.TabIndex = 4;
            this.cbUserAgent.Text = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
    "Chrome/114.0.0.0 Safari/537.36";
            // 
            // lblScreenRes
            // 
            this.lblScreenRes.Location = new System.Drawing.Point(20, 80);
            this.lblScreenRes.Name = "lblScreenRes";
            this.lblScreenRes.Size = new System.Drawing.Size(150, 23);
            this.lblScreenRes.TabIndex = 5;
            this.lblScreenRes.Text = "Screen Resolution:";
            // 
            // cbScreenRes
            // 
            this.cbScreenRes.Items.AddRange(new object[] {
            "1920x1080",
            "1366x768",
            "1440x900",
            "2560x1440",
            "1280x720",
            "Random"});
            this.cbScreenRes.Location = new System.Drawing.Point(170, 80);
            this.cbScreenRes.Name = "cbScreenRes";
            this.cbScreenRes.Size = new System.Drawing.Size(437, 21);
            this.cbScreenRes.TabIndex = 6;
            // 
            // lblTimezone
            // 
            this.lblTimezone.Location = new System.Drawing.Point(20, 110);
            this.lblTimezone.Name = "lblTimezone";
            this.lblTimezone.Size = new System.Drawing.Size(150, 23);
            this.lblTimezone.TabIndex = 7;
            this.lblTimezone.Text = "Timezone:";
            // 
            // cbTimezone
            // 
            this.cbTimezone.Items.AddRange(new object[] {
            "Asia/Ho_Chi_Minh",
            "America/New_York",
            "Europe/London",
            "Asia/Tokyo",
            "Australia/Sydney"});
            this.cbTimezone.Location = new System.Drawing.Point(170, 110);
            this.cbTimezone.Name = "cbTimezone";
            this.cbTimezone.Size = new System.Drawing.Size(437, 21);
            this.cbTimezone.TabIndex = 8;
            this.cbTimezone.Text = "Asia/Ho_Chi_Minh";
            // 
            // lblWebGL
            // 
            this.lblWebGL.Location = new System.Drawing.Point(20, 140);
            this.lblWebGL.Name = "lblWebGL";
            this.lblWebGL.Size = new System.Drawing.Size(150, 23);
            this.lblWebGL.TabIndex = 9;
            this.lblWebGL.Text = "WebGL Vendor:";
            // 
            // cbWebGL
            // 
            this.cbWebGL.Items.AddRange(new object[] {
            "Intel Inc.",
            "NVIDIA Corporation",
            "AMD",
            "Qualcomm",
            "Apple"});
            this.cbWebGL.Location = new System.Drawing.Point(170, 140);
            this.cbWebGL.Name = "cbWebGL";
            this.cbWebGL.Size = new System.Drawing.Size(437, 21);
            this.cbWebGL.TabIndex = 10;
            this.cbWebGL.Text = "Intel Inc.";
            // 
            // lblAudio
            // 
            this.lblAudio.Location = new System.Drawing.Point(20, 170);
            this.lblAudio.Name = "lblAudio";
            this.lblAudio.Size = new System.Drawing.Size(150, 23);
            this.lblAudio.TabIndex = 11;
            this.lblAudio.Text = "Audio Context:";
            // 
            // cbAudio
            // 
            this.cbAudio.Items.AddRange(new object[] {
            "Default",
            "Fake Noise 1",
            "Fake Noise 2"});
            this.cbAudio.Location = new System.Drawing.Point(170, 170);
            this.cbAudio.Name = "cbAudio";
            this.cbAudio.Size = new System.Drawing.Size(437, 21);
            this.cbAudio.TabIndex = 12;
            // 
            // lblFont
            // 
            this.lblFont.Location = new System.Drawing.Point(20, 200);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(150, 23);
            this.lblFont.TabIndex = 13;
            this.lblFont.Text = "Font:";
            // 
            // cbFont
            // 
            this.cbFont.Items.AddRange(new object[] {
            "Arial",
            "Times New Roman",
            "Roboto",
            "Courier New",
            "Verdana"});
            this.cbFont.Location = new System.Drawing.Point(170, 200);
            this.cbFont.Name = "cbFont";
            this.cbFont.Size = new System.Drawing.Size(437, 21);
            this.cbFont.TabIndex = 14;
            this.cbFont.Text = "Arial";
            // 
            // lblWebRTC
            // 
            this.lblWebRTC.Location = new System.Drawing.Point(20, 230);
            this.lblWebRTC.Name = "lblWebRTC";
            this.lblWebRTC.Size = new System.Drawing.Size(150, 23);
            this.lblWebRTC.TabIndex = 15;
            this.lblWebRTC.Text = "WebRTC:";
            // 
            // cbWebRTC
            // 
            this.cbWebRTC.Items.AddRange(new object[] {
            "Enabled",
            "Disabled",
            "Fake IP"});
            this.cbWebRTC.Location = new System.Drawing.Point(170, 230);
            this.cbWebRTC.Name = "cbWebRTC";
            this.cbWebRTC.Size = new System.Drawing.Size(437, 21);
            this.cbWebRTC.TabIndex = 16;
            // 
            // lblCanvas
            // 
            this.lblCanvas.Location = new System.Drawing.Point(20, 260);
            this.lblCanvas.Name = "lblCanvas";
            this.lblCanvas.Size = new System.Drawing.Size(150, 23);
            this.lblCanvas.TabIndex = 17;
            this.lblCanvas.Text = "Canvas Fingerprint:";
            // 
            // cbCanvas
            // 
            this.cbCanvas.Items.AddRange(new object[] {
            "Default",
            "Random Noise 1",
            "Random Noise 2"});
            this.cbCanvas.Location = new System.Drawing.Point(170, 260);
            this.cbCanvas.Name = "cbCanvas";
            this.cbCanvas.Size = new System.Drawing.Size(437, 21);
            this.cbCanvas.TabIndex = 18;
            // 
            // lblCPU
            // 
            this.lblCPU.Location = new System.Drawing.Point(20, 290);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(150, 23);
            this.lblCPU.TabIndex = 19;
            this.lblCPU.Text = "CPU Info:";
            // 
            // cbCPU
            // 
            this.cbCPU.Items.AddRange(new object[] {
            "Intel i7-9700K",
            "Intel i5-8600K",
            "AMD Ryzen 7 5800X",
            "Apple M1",
            "Intel Xeon E5-2670"});
            this.cbCPU.Location = new System.Drawing.Point(170, 290);
            this.cbCPU.Name = "cbCPU";
            this.cbCPU.Size = new System.Drawing.Size(437, 21);
            this.cbCPU.TabIndex = 20;
            this.cbCPU.Text = "Intel i7-9700K";
            // 
            // lblRAM
            // 
            this.lblRAM.Location = new System.Drawing.Point(20, 320);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(150, 23);
            this.lblRAM.TabIndex = 21;
            this.lblRAM.Text = "RAM (GB):";
            // 
            // cbRAM
            // 
            this.cbRAM.Items.AddRange(new object[] {
            "4",
            "8",
            "16",
            "32",
            "64"});
            this.cbRAM.Location = new System.Drawing.Point(170, 320);
            this.cbRAM.Name = "cbRAM";
            this.cbRAM.Size = new System.Drawing.Size(437, 21);
            this.cbRAM.TabIndex = 22;
            this.cbRAM.Text = "8";
            // 
            // chkRandomParams
            // 
            this.chkRandomParams.Location = new System.Drawing.Point(20, 350);
            this.chkRandomParams.Name = "chkRandomParams";
            this.chkRandomParams.Size = new System.Drawing.Size(150, 24);
            this.chkRandomParams.TabIndex = 23;
            this.chkRandomParams.Text = "Random các thông số";
            // 
            // lblNumDrivers
            // 
            this.lblNumDrivers.Location = new System.Drawing.Point(20, 380);
            this.lblNumDrivers.Name = "lblNumDrivers";
            this.lblNumDrivers.Size = new System.Drawing.Size(150, 23);
            this.lblNumDrivers.TabIndex = 24;
            this.lblNumDrivers.Text = "Số Driver:";
            // 
            // numDrivers
            // 
            this.numDrivers.Location = new System.Drawing.Point(170, 380);
            this.numDrivers.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numDrivers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDrivers.Name = "numDrivers";
            this.numDrivers.Size = new System.Drawing.Size(200, 20);
            this.numDrivers.TabIndex = 25;
            this.numDrivers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnStartDriver
            // 
            this.btnStartDriver.Location = new System.Drawing.Point(170, 410);
            this.btnStartDriver.Name = "btnStartDriver";
            this.btnStartDriver.Size = new System.Drawing.Size(150, 23);
            this.btnStartDriver.TabIndex = 26;
            this.btnStartDriver.Text = "Khởi tạo Driver";
            this.btnStartDriver.Click += new System.EventHandler(this.btnStartDriver_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(330, 410);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(150, 23);
            this.btnSaveConfig.TabIndex = 27;
            this.btnSaveConfig.Text = "Lưu Cấu Hình";
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnStopDriver
            // 
            this.btnStopDriver.Location = new System.Drawing.Point(490, 410);
            this.btnStopDriver.Name = "btnStopDriver";
            this.btnStopDriver.Size = new System.Drawing.Size(150, 23);
            this.btnStopDriver.TabIndex = 28;
            this.btnStopDriver.Text = "Tắt Tất Cả Driver";
            this.btnStopDriver.Click += new System.EventHandler(this.btnStopDriver_Click);
            // 
            // dataGridViewDrivers
            // 
            this.dataGridViewDrivers.AllowUserToAddRows = false;
            this.dataGridViewDrivers.AllowUserToDeleteRows = false;
            this.dataGridViewDrivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDrivers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameDriver,
            this.Port,
            this.Status,
            this.Start,
            this.Stop,
            this.Delete});
            this.dataGridViewDrivers.Location = new System.Drawing.Point(5, 449);
            this.dataGridViewDrivers.Name = "dataGridViewDrivers";
            this.dataGridViewDrivers.Size = new System.Drawing.Size(643, 201);
            this.dataGridViewDrivers.TabIndex = 29;
            this.dataGridViewDrivers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDrivers_CellContentClick);
            // 
            // NameDriver
            // 
            this.NameDriver.HeaderText = "Name";
            this.NameDriver.Name = "NameDriver";
            // 
            // Port
            // 
            this.Port.HeaderText = "Port";
            this.Port.Name = "Port";
            // 
            // Status
            // 
            this.Status.HeaderText = "Trạng thái";
            this.Status.Name = "Status";
            // 
            // Start
            // 
            this.Start.HeaderText = "Khởi tạo";
            this.Start.Name = "Start";
            // 
            // Stop
            // 
            this.Stop.HeaderText = "Dừng";
            this.Stop.Name = "Stop";
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Xóa";
            this.Delete.Name = "Delete";
            // 
            // cbExtension
            // 
            this.cbExtension.Location = new System.Drawing.Point(170, 350);
            this.cbExtension.Name = "cbExtension";
            this.cbExtension.Size = new System.Drawing.Size(81, 24);
            this.cbExtension.TabIndex = 31;
            this.cbExtension.Text = "Extension";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(64, 409);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 32;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FMain
            // 
            this.ClientSize = new System.Drawing.Size(660, 650);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbExtension);
            this.Controls.Add(this.dataGridViewDrivers);
            this.Controls.Add(this.lblDriverInfo);
            this.Controls.Add(this.cbDriverInfo);
            this.Controls.Add(this.lblUserAgent);
            this.Controls.Add(this.cbUserAgent);
            this.Controls.Add(this.lblScreenRes);
            this.Controls.Add(this.cbScreenRes);
            this.Controls.Add(this.lblTimezone);
            this.Controls.Add(this.cbTimezone);
            this.Controls.Add(this.lblWebGL);
            this.Controls.Add(this.cbWebGL);
            this.Controls.Add(this.lblAudio);
            this.Controls.Add(this.cbAudio);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.cbFont);
            this.Controls.Add(this.lblWebRTC);
            this.Controls.Add(this.cbWebRTC);
            this.Controls.Add(this.lblCanvas);
            this.Controls.Add(this.cbCanvas);
            this.Controls.Add(this.lblCPU);
            this.Controls.Add(this.cbCPU);
            this.Controls.Add(this.lblRAM);
            this.Controls.Add(this.cbRAM);
            this.Controls.Add(this.chkRandomParams);
            this.Controls.Add(this.lblNumDrivers);
            this.Controls.Add(this.numDrivers);
            this.Controls.Add(this.btnStartDriver);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.btnStopDriver);
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fake Driver Tool";
            this.Load += new System.EventHandler(this.FMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numDrivers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDriverInfo;
        private System.Windows.Forms.ComboBox cbDriverInfo;
        private System.Windows.Forms.Label lblUserAgent;
        private System.Windows.Forms.ComboBox cbUserAgent;
        private System.Windows.Forms.Label lblScreenRes;
        private System.Windows.Forms.ComboBox cbScreenRes;
        private System.Windows.Forms.Label lblTimezone;
        private System.Windows.Forms.ComboBox cbTimezone;
        private System.Windows.Forms.Label lblWebGL;
        private System.Windows.Forms.ComboBox cbWebGL;
        private System.Windows.Forms.Label lblAudio;
        private System.Windows.Forms.ComboBox cbAudio;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.ComboBox cbFont;
        private System.Windows.Forms.Label lblWebRTC;
        private System.Windows.Forms.ComboBox cbWebRTC;
        private System.Windows.Forms.Label lblCanvas;
        private System.Windows.Forms.ComboBox cbCanvas;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.ComboBox cbCPU;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.ComboBox cbRAM;
        private System.Windows.Forms.CheckBox chkRandomParams;
        private System.Windows.Forms.Label lblNumDrivers;
        private System.Windows.Forms.NumericUpDown numDrivers;
        private System.Windows.Forms.Button btnStartDriver;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnStopDriver;
        private System.Windows.Forms.DataGridView dataGridViewDrivers;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameDriver;
        private System.Windows.Forms.DataGridViewTextBoxColumn Port;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stop;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delete;
        private System.Windows.Forms.CheckBox cbExtension;
        private System.Windows.Forms.Button button1;
    }
}