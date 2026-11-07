using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using GothicPortPreparer.Models;
using GothicPortPreparer.Services;

namespace GothicPortPreparer.UI
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource? _cts;
        private string _lastPreparedDir = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbTexture.SelectedIndex = 0; // 128 px
            numThreads.Value = Math.Max(1, Environment.ProcessorCount);

            // Default output directory in user documents or current app directory
            var defaultOut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "R36S_Ports");
            txtOutputPath.Text = defaultOut;

            // Run initial auto-detection
            AutoDetectGame();
        }

        private void AutoDetectGame()
        {
            var detected = GameDetector.FindInstalledGames();
            var targetType = rdoGothic2.Checked ? GameType.Gothic2 : GameType.Gothic1;

            DetectedGame? match = null;
            foreach (var g in detected)
            {
                if (g.GameType == targetType)
                {
                    match = g;
                    break;
                }
            }

            if (match != null)
            {
                txtGamePath.Text = match.Path;
                AppendLog($"[Detection] Found {match.Name} ({match.Source}): {match.Path}");
            }
            else if (detected.Count > 0)
            {
                // Select first available detected game
                var first = detected[0];
                if (first.GameType == GameType.Gothic2)
                    rdoGothic2.Checked = true;
                else
                    rdoGothic1.Checked = true;

                txtGamePath.Text = first.Path;
                AppendLog($"[Detection] Found {first.Name} ({first.Source}): {first.Path}");
            }
            else
            {
                AppendLog("[Detection] No Gothic installations automatically detected. Please browse manually.");
            }
        }

        private void RdoGame_CheckedChanged(object sender, EventArgs e)
        {
            var targetType = rdoGothic2.Checked ? GameType.Gothic2 : GameType.Gothic1;
            var currentVal = GameDetector.ValidatePath(txtGamePath.Text);
            if (currentVal.IsValid && currentVal.DetectedType != targetType)
            {
                // Try finding matching installation for the newly selected radio button
                var detected = GameDetector.FindInstalledGames();
                foreach (var g in detected)
                {
                    if (g.GameType == targetType)
                    {
                        txtGamePath.Text = g.Path;
                        break;
                    }
                }
            }
            ValidateGamePath();
        }

        private void TxtGamePath_TextChanged(object sender, EventArgs e)
        {
            ValidateGamePath();
        }

        private void ValidateGamePath()
        {
            var path = txtGamePath.Text.Trim();
            var val = GameDetector.ValidatePath(path);

            if (val.IsValid)
            {
                lblPathValidation.ForeColor = Color.ForestGreen;
                lblPathValidation.Text = "✔ " + val.Message;

                // Sync radio button if user pointed to Gothic 2 or Gothic 1
                if (val.DetectedType == GameType.Gothic2 && !rdoGothic2.Checked)
                {
                    rdoGothic2.Checked = true;
                }
                else if (val.DetectedType == GameType.Gothic1 && !rdoGothic1.Checked)
                {
                    rdoGothic1.Checked = true;
                }
            }
            else
            {
                lblPathValidation.ForeColor = Color.Crimson;
                lblPathValidation.Text = "✖ " + val.Message;
            }
        }

        private void BtnBrowseGame_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "Select your Gothic 1 or Gothic 2 installation folder:";
            if (!string.IsNullOrWhiteSpace(txtGamePath.Text) && Directory.Exists(txtGamePath.Text))
            {
                dlg.SelectedPath = txtGamePath.Text;
            }
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                txtGamePath.Text = dlg.SelectedPath;
            }
        }

        private void BtnAutoDetect_Click(object sender, EventArgs e)
        {
            AutoDetectGame();
        }

        private void BtnBrowseOutput_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "Select port output folder (can be your SD card roms/ports directory):";
            if (!string.IsNullOrWhiteSpace(txtOutputPath.Text) && Directory.Exists(txtOutputPath.Text))
            {
                dlg.SelectedPath = txtOutputPath.Text;
            }
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                txtOutputPath.Text = dlg.SelectedPath;
            }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            var gamePath = txtGamePath.Text.Trim();
            var outPath = txtOutputPath.Text.Trim();

            var val = GameDetector.ValidatePath(gamePath);
            if (!val.IsValid)
            {
                MessageBox.Show(this, "The specified game path is invalid:\n" + val.Message, "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(outPath))
            {
                MessageBox.Show(this, "Please choose an output directory.", "Missing Output Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maxTex = 128;
            if (cmbTexture.SelectedIndex == 1) maxTex = 256;
            else if (cmbTexture.SelectedIndex == 2) maxTex = 64;

            var options = new BakeOptions
            {
                GameType = rdoGothic2.Checked ? GameType.Gothic2 : GameType.Gothic1,
                GameDirectory = gamePath,
                OutputDirectory = outPath,
                MaxTextureSize = maxTex,
                Threads = (int)numThreads.Value,
                IncludeVideo = chkIncludeVideo.Checked,
                CheckForUpdates = chkCheckUpdates.Checked
            };

            SetUiRunning(true);
            _cts = new CancellationTokenSource();

            var progress = new Progress<PreparationProgress>(p =>
            {
                if (p.Percentage >= 0 && p.Percentage <= 100)
                {
                    progressBar.Value = p.Percentage;
                }
                if (!string.IsNullOrEmpty(p.StepTitle))
                {
                    lblStep.Text = $"[{p.Percentage}%] {p.StepTitle}";
                }
                if (!string.IsNullOrEmpty(p.LogMessage))
                {
                    AppendLog(p.LogMessage);
                }
            });

            try
            {
                var workflow = new PreparationWorkflow();
                _lastPreparedDir = await workflow.RunPreparationAsync(options, progress, _cts.Token);

                System.Media.SystemSounds.Asterisk.Play();
                MessageBox.Show(this,
                    $"Preparation of {(options.GameType == GameType.Gothic2 ? "Gothic 2" : "Gothic 1")} port completed successfully!\n\n" +
                    $"Output files are located in:\n{_lastPreparedDir}\n\n" +
                    "The folder has been opened in Windows Explorer. Copy it to your R36S SD card (in roms/ports/).",
                    "R36S Port Ready!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                lblStep.Text = "Cancelled by user.";
                AppendLog("[CANCELLED] Port preparation was cancelled by user.");
            }
            catch (Exception ex)
            {
                lblStep.Text = "An error occurred during preparation.";
                AppendLog("[ERROR] " + ex.ToString());
                MessageBox.Show(this, "An error occurred during port preparation:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUiRunning(false);
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                btnCancel.Enabled = false;
                AppendLog("[Action] Cancellation requested...");
                _cts.Cancel();
            }
        }

        private void SetUiRunning(bool isRunning)
        {
            btnStart.Enabled = !isRunning;
            btnCancel.Enabled = isRunning;
            grpGame.Enabled = !isRunning;
            grpPaths.Enabled = !isRunning;
            grpOptions.Enabled = !isRunning;
            if (isRunning)
            {
                progressBar.Value = 0;
            }
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendLog), message);
                return;
            }

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtLogs.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            txtLogs.Clear();
        }

        private void BtnOpenOutput_Click(object sender, EventArgs e)
        {
            var dir = !string.IsNullOrEmpty(_lastPreparedDir) && Directory.Exists(_lastPreparedDir)
                ? _lastPreparedDir
                : txtOutputPath.Text.Trim();

            if (Directory.Exists(dir))
            {
                Process.Start("explorer.exe", $"\"{dir}\"");
            }
            else
            {
                MessageBox.Show(this, "The specified output directory does not exist yet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
