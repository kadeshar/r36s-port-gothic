namespace GothicPortPreparer.UI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpGame = new System.Windows.Forms.GroupBox();
            this.rdoGothic2 = new System.Windows.Forms.RadioButton();
            this.rdoGothic1 = new System.Windows.Forms.RadioButton();
            this.grpPaths = new System.Windows.Forms.GroupBox();
            this.lblPathValidation = new System.Windows.Forms.Label();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnAutoDetect = new System.Windows.Forms.Button();
            this.btnBrowseGame = new System.Windows.Forms.Button();
            this.txtGamePath = new System.Windows.Forms.TextBox();
            this.lblGamePath = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.chkCheckUpdates = new System.Windows.Forms.CheckBox();
            this.chkIncludeVideo = new System.Windows.Forms.CheckBox();
            this.numThreads = new System.Windows.Forms.NumericUpDown();
            this.lblThreads = new System.Windows.Forms.Label();
            this.cmbTexture = new System.Windows.Forms.ComboBox();
            this.lblTexture = new System.Windows.Forms.Label();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnOpenOutput = new System.Windows.Forms.Button();
            this.txtLogs = new System.Windows.Forms.TextBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.grpGame.SuspendLayout();
            this.grpPaths.SuspendLayout();
            this.grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).BeginInit();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(784, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(205)))));
            this.lblSubTitle.Location = new System.Drawing.Point(18, 40);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(567, 15);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Automated port preparation tool for Gothic 1 & Gothic 2 on R36S console (ArkOS / pocket-native)";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(288, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gothic Port Preparer for R36S";
            // 
            // grpGame
            // 
            this.grpGame.Controls.Add(this.rdoGothic2);
            this.grpGame.Controls.Add(this.rdoGothic1);
            this.grpGame.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpGame.Location = new System.Drawing.Point(16, 80);
            this.grpGame.Name = "grpGame";
            this.grpGame.Size = new System.Drawing.Size(752, 60);
            this.grpGame.TabIndex = 1;
            this.grpGame.TabStop = false;
            this.grpGame.Text = "Target Game";
            // 
            // rdoGothic2
            // 
            this.rdoGothic2.AutoSize = true;
            this.rdoGothic2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rdoGothic2.Location = new System.Drawing.Point(220, 24);
            this.rdoGothic2.Name = "rdoGothic2";
            this.rdoGothic2.Size = new System.Drawing.Size(209, 19);
            this.rdoGothic2.TabIndex = 1;
            this.rdoGothic2.Text = "Gothic 2 (Night of the Raven)";
            this.rdoGothic2.UseVisualStyleBackColor = true;
            this.rdoGothic2.CheckedChanged += new System.EventHandler(this.RdoGame_CheckedChanged);
            // 
            // rdoGothic1
            // 
            this.rdoGothic1.AutoSize = true;
            this.rdoGothic1.Checked = true;
            this.rdoGothic1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rdoGothic1.Location = new System.Drawing.Point(20, 24);
            this.rdoGothic1.Name = "rdoGothic1";
            this.rdoGothic1.Size = new System.Drawing.Size(127, 19);
            this.rdoGothic1.TabIndex = 0;
            this.rdoGothic1.TabStop = true;
            this.rdoGothic1.Text = "Gothic 1 (Classic)";
            this.rdoGothic1.UseVisualStyleBackColor = true;
            this.rdoGothic1.CheckedChanged += new System.EventHandler(this.RdoGame_CheckedChanged);
            // 
            // grpPaths
            // 
            this.grpPaths.Controls.Add(this.lblPathValidation);
            this.grpPaths.Controls.Add(this.btnBrowseOutput);
            this.grpPaths.Controls.Add(this.txtOutputPath);
            this.grpPaths.Controls.Add(this.lblOutput);
            this.grpPaths.Controls.Add(this.btnAutoDetect);
            this.grpPaths.Controls.Add(this.btnBrowseGame);
            this.grpPaths.Controls.Add(this.txtGamePath);
            this.grpPaths.Controls.Add(this.lblGamePath);
            this.grpPaths.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPaths.Location = new System.Drawing.Point(16, 146);
            this.grpPaths.Name = "grpPaths";
            this.grpPaths.Size = new System.Drawing.Size(752, 140);
            this.grpPaths.TabIndex = 2;
            this.grpPaths.TabStop = false;
            this.grpPaths.Text = "Directories";
            // 
            // lblPathValidation
            // 
            this.lblPathValidation.AutoSize = true;
            this.lblPathValidation.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPathValidation.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblPathValidation.Location = new System.Drawing.Point(18, 64);
            this.lblPathValidation.Name = "lblPathValidation";
            this.lblPathValidation.Size = new System.Drawing.Size(198, 13);
            this.lblPathValidation.TabIndex = 7;
            this.lblPathValidation.Text = "Select your Gothic installation directory.";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBrowseOutput.Location = new System.Drawing.Point(645, 100);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(95, 25);
            this.btnBrowseOutput.TabIndex = 6;
            this.btnBrowseOutput.Text = "Browse...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.BtnBrowseOutput_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOutputPath.Location = new System.Drawing.Point(20, 101);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(615, 23);
            this.txtOutputPath.TabIndex = 5;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOutput.Location = new System.Drawing.Point(18, 83);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(306, 15);
            this.lblOutput.TabIndex = 4;
            this.lblOutput.Text = "Port Output Folder (Destination Directory / SD card):";
            // 
            // btnAutoDetect
            // 
            this.btnAutoDetect.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAutoDetect.Location = new System.Drawing.Point(595, 37);
            this.btnAutoDetect.Name = "btnAutoDetect";
            this.btnAutoDetect.Size = new System.Drawing.Size(145, 25);
            this.btnAutoDetect.TabIndex = 3;
            this.btnAutoDetect.Text = "Auto-Detect";
            this.btnAutoDetect.UseVisualStyleBackColor = true;
            this.btnAutoDetect.Click += new System.EventHandler(this.BtnAutoDetect_Click);
            // 
            // btnBrowseGame
            // 
            this.btnBrowseGame.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBrowseGame.Location = new System.Drawing.Point(490, 37);
            this.btnBrowseGame.Name = "btnBrowseGame";
            this.btnBrowseGame.Size = new System.Drawing.Size(95, 25);
            this.btnBrowseGame.TabIndex = 2;
            this.btnBrowseGame.Text = "Browse...";
            this.btnBrowseGame.UseVisualStyleBackColor = true;
            this.btnBrowseGame.Click += new System.EventHandler(this.BtnBrowseGame_Click);
            // 
            // txtGamePath
            // 
            this.txtGamePath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGamePath.Location = new System.Drawing.Point(20, 38);
            this.txtGamePath.Name = "txtGamePath";
            this.txtGamePath.Size = new System.Drawing.Size(460, 23);
            this.txtGamePath.TabIndex = 1;
            this.txtGamePath.TextChanged += new System.EventHandler(this.TxtGamePath_TextChanged);
            // 
            // lblGamePath
            // 
            this.lblGamePath.AutoSize = true;
            this.lblGamePath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGamePath.Location = new System.Drawing.Point(18, 20);
            this.lblGamePath.Name = "lblGamePath";
            this.lblGamePath.Size = new System.Drawing.Size(262, 15);
            this.lblGamePath.TabIndex = 0;
            this.lblGamePath.Text = "Gothic Installation Folder (Steam / GOG / Custom):";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkCheckUpdates);
            this.grpOptions.Controls.Add(this.chkIncludeVideo);
            this.grpOptions.Controls.Add(this.numThreads);
            this.grpOptions.Controls.Add(this.lblThreads);
            this.grpOptions.Controls.Add(this.cmbTexture);
            this.grpOptions.Controls.Add(this.lblTexture);
            this.grpOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpOptions.Location = new System.Drawing.Point(16, 292);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(752, 65);
            this.grpOptions.TabIndex = 3;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Baking Options";
            // 
            // chkCheckUpdates
            // 
            this.chkCheckUpdates.AutoSize = true;
            this.chkCheckUpdates.Checked = true;
            this.chkCheckUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCheckUpdates.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkCheckUpdates.Location = new System.Drawing.Point(540, 28);
            this.chkCheckUpdates.Name = "chkCheckUpdates";
            this.chkCheckUpdates.Size = new System.Drawing.Size(188, 19);
            this.chkCheckUpdates.TabIndex = 5;
            this.chkCheckUpdates.Text = "Check for GitHub Updates";
            this.chkCheckUpdates.UseVisualStyleBackColor = true;
            // 
            // chkIncludeVideo
            // 
            this.chkIncludeVideo.AutoSize = true;
            this.chkIncludeVideo.Checked = true;
            this.chkIncludeVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeVideo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkIncludeVideo.Location = new System.Drawing.Point(390, 28);
            this.chkIncludeVideo.Name = "chkIncludeVideo";
            this.chkIncludeVideo.Size = new System.Drawing.Size(134, 19);
            this.chkIncludeVideo.TabIndex = 4;
            this.chkIncludeVideo.Text = "Video Cutscenes (.bik)";
            this.chkIncludeVideo.UseVisualStyleBackColor = true;
            // 
            // numThreads
            // 
            this.numThreads.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numThreads.Location = new System.Drawing.Point(300, 27);
            this.numThreads.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            this.numThreads.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numThreads.Name = "numThreads";
            this.numThreads.Size = new System.Drawing.Size(65, 23);
            this.numThreads.TabIndex = 3;
            this.numThreads.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // lblThreads
            // 
            this.lblThreads.AutoSize = true;
            this.lblThreads.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblThreads.Location = new System.Drawing.Point(220, 30);
            this.lblThreads.Name = "lblThreads";
            this.lblThreads.Size = new System.Drawing.Size(76, 15);
            this.lblThreads.TabIndex = 2;
            this.lblThreads.Text = "CPU Threads:";
            // 
            // cmbTexture
            // 
            this.cmbTexture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTexture.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbTexture.FormattingEnabled = true;
            this.cmbTexture.Items.AddRange(new object[] {
            "128 px (Recommended for R36S)",
            "256 px (Higher Quality)",
            "64 px (Max Performance)"});
            this.cmbTexture.Location = new System.Drawing.Point(75, 27);
            this.cmbTexture.Name = "cmbTexture";
            this.cmbTexture.Size = new System.Drawing.Size(135, 23);
            this.cmbTexture.TabIndex = 1;
            // 
            // lblTexture
            // 
            this.lblTexture.AutoSize = true;
            this.lblTexture.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTexture.Location = new System.Drawing.Point(18, 30);
            this.lblTexture.Name = "lblTexture";
            this.lblTexture.Size = new System.Drawing.Size(53, 15);
            this.lblTexture.TabIndex = 0;
            this.lblTexture.Text = "Textures:";
            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.btnClearLog);
            this.grpProgress.Controls.Add(this.btnOpenOutput);
            this.grpProgress.Controls.Add(this.txtLogs);
            this.grpProgress.Controls.Add(this.lblStep);
            this.grpProgress.Controls.Add(this.progressBar);
            this.grpProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpProgress.Location = new System.Drawing.Point(16, 420);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(752, 220);
            this.grpProgress.TabIndex = 4;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress & Logs";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btnClearLog.Location = new System.Drawing.Point(645, 45);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(95, 22);
            this.btnClearLog.TabIndex = 4;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.BtnClearLog_Click);
            // 
            // btnOpenOutput
            // 
            this.btnOpenOutput.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btnOpenOutput.Location = new System.Drawing.Point(510, 45);
            this.btnOpenOutput.Name = "btnOpenOutput";
            this.btnOpenOutput.Size = new System.Drawing.Size(125, 22);
            this.btnOpenOutput.TabIndex = 3;
            this.btnOpenOutput.Text = "Open Port Folder";
            this.btnOpenOutput.UseVisualStyleBackColor = true;
            this.btnOpenOutput.Click += new System.EventHandler(this.BtnOpenOutput_Click);
            // 
            // txtLogs
            // 
            this.txtLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(30)))));
            this.txtLogs.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.txtLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.txtLogs.Location = new System.Drawing.Point(15, 70);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.ReadOnly = true;
            this.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogs.Size = new System.Drawing.Size(725, 140);
            this.txtLogs.TabIndex = 2;
            this.txtLogs.WordWrap = false;
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStep.Location = new System.Drawing.Point(15, 49);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(83, 15);
            this.lblStep.TabIndex = 1;
            this.lblStep.Text = "Ready to start.";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 22);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(725, 20);
            this.progressBar.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(120)))), ((int)(((byte)(210)))));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(16, 365);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(590, 45);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "PREPARE PORT FOR R36S";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Enabled = false;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(620, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(148, 45);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(784, 655);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpPaths);
            this.Controls.Add(this.grpGame);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gothic Port Preparer for R36S";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpGame.ResumeLayout(false);
            this.grpGame.PerformLayout();
            this.grpPaths.ResumeLayout(false);
            this.grpPaths.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).EndInit();
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.GroupBox grpGame;
        private System.Windows.Forms.RadioButton rdoGothic2;
        private System.Windows.Forms.RadioButton rdoGothic1;
        private System.Windows.Forms.GroupBox grpPaths;
        private System.Windows.Forms.Label lblGamePath;
        private System.Windows.Forms.TextBox txtGamePath;
        private System.Windows.Forms.Button btnBrowseGame;
        private System.Windows.Forms.Button btnAutoDetect;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Label lblPathValidation;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Label lblTexture;
        private System.Windows.Forms.ComboBox cmbTexture;
        private System.Windows.Forms.Label lblThreads;
        private System.Windows.Forms.NumericUpDown numThreads;
        private System.Windows.Forms.CheckBox chkIncludeVideo;
        private System.Windows.Forms.CheckBox chkCheckUpdates;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.TextBox txtLogs;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnOpenOutput;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
    }
}
