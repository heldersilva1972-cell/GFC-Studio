using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GFCDevOpsUtility
{
    public partial class MainForm : Form
    {
        private AppConfig _config = null!;
        private DataTable _dtRevisions = null!;
        private bool _isBusy = false;

        // Custom theme colors (Light Mode)
        private static readonly Color ColorBg = Color.FromArgb(248, 250, 252);        // Slate 50
        private static readonly Color ColorCard = Color.FromArgb(255, 255, 255);      // Pure White
        private static readonly Color ColorText = Color.FromArgb(15, 23, 42);         // Slate 900
        private static readonly Color ColorTextMuted = Color.FromArgb(71, 85, 105);   // Slate 600
        private static readonly Color ColorBorder = Color.FromArgb(226, 232, 240);     // Slate 200
        
        // Brand/Status Accent Colors (Non-Red)
        private static readonly Color ColorPrimary = Color.FromArgb(37, 99, 235);     // Royal Blue
        private static readonly Color ColorSuccess = Color.FromArgb(22, 101, 52);      // Forest Green
        private static readonly Color ColorWarning = Color.FromArgb(217, 119, 6);      // Dark Orange / Amber
        private static readonly Color ColorError = Color.FromArgb(181, 96, 0);         // Warm Amber for Errors
        private static readonly Color ColorWait = Color.FromArgb(29, 78, 216);         // Dark Blue for Steps

        public MainForm()
        {
            InitializeComponent();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            this.BackColor = ColorBg;
            
            // Styled Groupboxes
            StyleGroupBox(grpPublishOptions);
            StyleGroupBox(grpDeployOptions);
            StyleGroupBox(grpRevisionGrid);
            StyleGroupBox(grpRevisionSyncOptions);

            // Styled Flat buttons
            StylePrimaryButton(btnRunPublish);
            StylePrimaryButton(btnRunRevisionSync);
            btnRunDeploy.BackColor = Color.FromArgb(22, 101, 52); // Forest Green for deployment
            StylePrimaryButton(btnRunDeploy);

            // Terminal
            rtbTerminal.BackColor = Color.FromArgb(248, 250, 252);
            rtbTerminal.ForeColor = ColorText;
            btnClearLogs.ForeColor = ColorTextMuted;
            btnClearLogs.BackColor = ColorCard;
        }

        private void StyleGroupBox(GroupBox gb)
        {
            gb.BackColor = ColorCard;
            gb.ForeColor = ColorText;
        }

        private void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _config = AppConfig.Load();

            // Populate form text boxes from config
            txtPubWorkspace.Text = _config.WorkspacePath;
            txtPubOutput.Text = _config.PublishOutputPath;
            txtDepZip.Text = _config.ZipInputPath;
            
            chkAutoConfigWebConfig.Checked = _config.AutoConfigureWebConfig;
            chkPurgeFiles.Checked = _config.PurgeFiles;
            chkPublishMobileApk.Checked = _config.PosPublishMobileApk;

            cmbPublishApp.SelectedIndex = -1;
            cmbDeployApp.SelectedIndex = -1;
            cmbRevAppSelect.SelectedIndex = -1;

            RestoreDeployPaths();
            InitializeRevisionsTable();
            RefreshVersionDashboard();

            Log(">>> GFC DevOps Utility loaded successfully.", false, ColorPrimary);
            CheckAdministratorPrivileges();
        }

        private void CheckAdministratorPrivileges()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                if (!isAdmin)
                {
                    Log("==========================================================================", false, ColorWarning);
                    Log("WARNING: Running without ADMINISTRATOR privileges.", false, ColorWarning);
                    Log("IIS site control and staging deployments will fail if paths require admin.", false, ColorWarning);
                    Log("To deploy, please restart this application as Administrator.", false, ColorWarning);
                    Log("==========================================================================", false, ColorWarning);
                    lblStatus.Text = "Status: Warning - Not running as Administrator";
                    lblStatus.ForeColor = ColorWarning;
                }
            }
        }

        #region Navigation Tabs
        private void ResetAllSelections()
        {
            cmbPublishApp.SelectedIndex = -1;
            cmbDeployApp.SelectedIndex = -1;
            cmbRevAppSelect.SelectedIndex = -1;
        }

        private void BtnTabPublish_Click(object sender, EventArgs e)
        {
            ResetAllSelections();
            ShowPanel(pnlPublish);
            SetActiveTabButton(btnTabPublish);
        }

        private void BtnTabDeploy_Click(object sender, EventArgs e)
        {
            ResetAllSelections();
            ShowPanel(pnlDeploy);
            SetActiveTabButton(btnTabDeploy);
        }

        private void BtnTabRevisions_Click(object sender, EventArgs e)
        {
            ResetAllSelections();
            ShowPanel(pnlRevisions);
            SetActiveTabButton(btnTabRevisions);
            RefreshVersionDashboard();
        }

        private void ShowPanel(Panel panelToShow)
        {
            pnlPublish.Visible = (panelToShow == pnlPublish);
            pnlDeploy.Visible = (panelToShow == pnlDeploy);
            pnlRevisions.Visible = (panelToShow == pnlRevisions);
        }

        private void SetActiveTabButton(Button activeBtn)
        {
            btnTabPublish.ForeColor = (activeBtn == btnTabPublish) ? ColorPrimary : ColorTextMuted;
            btnTabDeploy.ForeColor = (activeBtn == btnTabDeploy) ? ColorPrimary : ColorTextMuted;
            btnTabRevisions.ForeColor = (activeBtn == btnTabRevisions) ? ColorPrimary : ColorTextMuted;
        }
        #endregion

        #region Logging Console (Amber for Errors, No Red)
        private void Log(string message, bool isError = false, Color? customColor = null)
        {
            if (rtbTerminal.InvokeRequired)
            {
                rtbTerminal.Invoke(new Action(() => Log(message, isError, customColor)));
                return;
            }

            Color logColor = ColorText;

            if (isError)
            {
                logColor = ColorError; // Warm Amber / Dark Orange for errors
            }
            else if (customColor.HasValue)
            {
                logColor = customColor.Value;
            }
            else
            {
                // Auto-detect status keywords
                if (message.Contains("[OK]") || message.Contains("SUCCESS") || message.Contains("Successful"))
                {
                    logColor = ColorSuccess;
                }
                else if (message.Contains("[DRY]") || message.Contains("WARNING") || message.Contains("Would "))
                {
                    logColor = ColorWarning;
                }
                else if (message.Contains("[WAIT]") || message.Contains(">>>") || message.Contains("Stopping") || message.Contains("Starting"))
                {
                    logColor = ColorWait;
                }
            }

            rtbTerminal.SelectionStart = rtbTerminal.TextLength;
            rtbTerminal.SelectionLength = 0;
            rtbTerminal.SelectionColor = logColor;
            
            if (isError || message.Contains("CRITICAL ERROR") || message.Contains("FAILED") || message.Contains("!!!"))
            {
                rtbTerminal.SelectionFont = new Font(rtbTerminal.Font, FontStyle.Bold);
            }
            else
            {
                rtbTerminal.SelectionFont = new Font(rtbTerminal.Font, FontStyle.Regular);
            }

            rtbTerminal.AppendText(message + Environment.NewLine);
            rtbTerminal.SelectionColor = rtbTerminal.ForeColor;
            rtbTerminal.ScrollToCaret();
        }

        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            rtbTerminal.Clear();
            rtbBuildOutput.Clear();
        }

        private void LogPublish(string message, bool isError = false, Color? customColor = null)
        {
            if (rtbBuildOutput.InvokeRequired)
            {
                rtbBuildOutput.Invoke(new Action(() => LogPublish(message, isError, customColor)));
                return;
            }

            Color logColor = ColorText;

            if (isError)
            {
                logColor = ColorError; // Warm Amber for errors
            }
            else if (customColor.HasValue)
            {
                logColor = customColor.Value;
            }
            else
            {
                // Auto-detect status keywords
                if (message.Contains("[OK]") || message.Contains("SUCCESS") || message.Contains("Successful"))
                {
                    logColor = ColorSuccess;
                }
                else if (message.Contains("[DRY]") || message.Contains("WARNING") || message.Contains("Would "))
                {
                    logColor = ColorWarning;
                }
                else if (message.Contains("[WAIT]") || message.Contains(">>>") || message.Contains("Stopping") || message.Contains("Starting"))
                {
                    logColor = ColorWait;
                }
            }

            rtbBuildOutput.SelectionStart = rtbBuildOutput.TextLength;
            rtbBuildOutput.SelectionLength = 0;
            rtbBuildOutput.SelectionColor = logColor;
            
            if (isError || message.Contains("CRITICAL ERROR") || message.Contains("FAILED") || message.Contains("!!!"))
            {
                rtbBuildOutput.SelectionFont = new Font(rtbBuildOutput.Font, FontStyle.Bold);
            }
            else
            {
                rtbBuildOutput.SelectionFont = new Font(rtbBuildOutput.Font, FontStyle.Regular);
            }

            rtbBuildOutput.AppendText(message + Environment.NewLine);
            rtbBuildOutput.SelectionColor = rtbBuildOutput.ForeColor;
            rtbBuildOutput.SelectionStart = rtbBuildOutput.Text.Length;
            rtbBuildOutput.ScrollToCaret();
        }

        private void BtnCopyLog_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rtbBuildOutput.Text))
            {
                Clipboard.SetText(rtbBuildOutput.Text);
                MessageBox.Show("Build logs successfully copied to clipboard!", "Logs Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No build logs to copy.", "Empty Logs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Revisions Dashboard
        private void InitializeRevisionsTable()
        {
            _dtRevisions = new DataTable();
            _dtRevisions.Columns.Add("App Project", typeof(string));
            _dtRevisions.Columns.Add("Current Version", typeof(string));
            _dtRevisions.Columns.Add("Proposed Version", typeof(string));

            _dtRevisions.Rows.Add("📱 Mobile App", "Loading...", "---");
            _dtRevisions.Rows.Add("🖥️ POS App", "Loading...", "---");
            _dtRevisions.Rows.Add("🌐 Web App", "Loading...", "---");

            dgvRevisions.DataSource = _dtRevisions;
            dgvRevisions.Columns[0].Width = 140;
            dgvRevisions.Columns[1].Width = 140;
            dgvRevisions.Columns[2].Width = 180;
            dgvRevisions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dgvRevisions.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        }

        private void RefreshVersionDashboard()
        {
            string workspace = txtPubWorkspace.Text.Trim();
            if (string.IsNullOrEmpty(workspace) || !Directory.Exists(workspace))
            {
                return;
            }

            Task.Run(() =>
            {
                string mobVer = VersionScanner.GetMobileVersion(workspace);
                string posVer = VersionScanner.GetPosVersion(workspace);
                string webVer = VersionScanner.GetWebAppVersion(workspace);

                this.Invoke(new Action(() =>
                {
                    _dtRevisions.Rows[0]["Current Version"] = mobVer;
                    _dtRevisions.Rows[1]["Current Version"] = posVer;
                    _dtRevisions.Rows[2]["Current Version"] = webVer;

                    UpdateProposedVersions();
                }));
            });
        }

        private void UpdateProposedVersions()
        {
            if (_dtRevisions == null) return;

            for (int i = 0; i < 3; i++)
            {
                string current = _dtRevisions.Rows[i]["Current Version"].ToString() ?? "Unknown";
                string proposed = "---";

                string selectedTarget = cmbRevAppSelect.SelectedItem?.ToString() ?? "";
                string rowTarget = i == 0 ? "Mobile" : (i == 1 ? "POS" : "WebApp");

                if (selectedTarget.Equals(rowTarget, StringComparison.OrdinalIgnoreCase))
                {
                    if (current != "Unknown" && current != "Loading...")
                    {
                        if (radRevNext.Checked)
                        {
                            var parts = current.Split('.');
                            if (parts.Length >= 2 && int.TryParse(parts[parts.Length - 1], out int last))
                            {
                                parts[parts.Length - 1] = (last + 1).ToString();
                                proposed = string.Join(".", parts);
                            }
                            else
                            {
                                proposed = current + ".1"; // fallback
                            }
                        }
                        else
                        {
                            proposed = txtRevCustomValue.Text.Trim();
                            if (string.IsNullOrEmpty(proposed)) proposed = "Specify Version...";
                        }
                    }
                }

                _dtRevisions.Rows[i]["Proposed Version"] = proposed;
            }
        }

        private void CmbRevAppSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProposedVersions();
        }

        private void CmbPublishApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkPublishMobileApk.Visible = cmbPublishApp.SelectedIndex == 1;
        }

        private void RadRevOption_CheckedChanged(object sender, EventArgs e)
        {
            txtRevCustomValue.Enabled = radRevCustom.Checked;
            UpdateProposedVersions();
        }

        private void TxtRevCustomValue_TextChanged(object sender, EventArgs e)
        {
            UpdateProposedVersions();
        }
        #endregion

        #region Path Browsers & Persistence
        private void BtnPubWorkspaceBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select GFC-Studio Workspace Root Folder";
                fbd.SelectedPath = txtPubWorkspace.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPubWorkspace.Text = fbd.SelectedPath;
                    _config.WorkspacePath = fbd.SelectedPath;
                    _config.Save();
                    RefreshVersionDashboard();
                }
            }
        }

        private void BtnPubOutputBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Folder to Save Published ZIP Packages";
                fbd.SelectedPath = txtPubOutput.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPubOutput.Text = fbd.SelectedPath;
                    _config.PublishOutputPath = fbd.SelectedPath;
                    _config.Save();
                }
            }
        }

        private void BtnDepZipBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Directory containing Deployment ZIP packages";
                fbd.SelectedPath = txtDepZip.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepZip.Text = fbd.SelectedPath;
                    _config.ZipInputPath = fbd.SelectedPath;
                    _config.Save();
                }
            }
        }

        private void BtnDepStagingBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepStaging.Text = fbd.SelectedPath;
                    SaveDeployPaths();
                }
            }
        }

        private void BtnDepLiveBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepLive.Text = fbd.SelectedPath;
                    SaveDeployPaths();
                }
            }
        }

        private void BtnDepBackupBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepBackup.Text = fbd.SelectedPath;
                    SaveDeployPaths();
                }
            }
        }

        private void CmbDeployApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            RestoreDeployPaths();
        }

        private void RestoreDeployPaths()
        {
            if (_config == null) return;

            int idx = cmbDeployApp.SelectedIndex;
            if (idx < 0) return; // No selection yet
            if (idx == 0) // Mobile
            {
                txtDepLive.Text = _config.MobileLivePath;
                txtDepStaging.Text = _config.MobileStagingPath;
                txtDepBackup.Text = _config.MobileBackupPath;
                txtDepIisSite.Text = _config.MobileIisSite;
                txtDepIisAppPool.Text = _config.MobileIisAppPool;
                txtDepLive.Enabled = txtDepStaging.Enabled = txtDepBackup.Enabled = txtDepIisSite.Enabled = txtDepIisAppPool.Enabled = true;
                btnDepLiveBrowse.Enabled = btnDepStagingBrowse.Enabled = btnDepBackupBrowse.Enabled = true;
                
                chkDepMobileApk.Visible = lblDepApkDist.Visible = txtDepApkDist.Visible = btnDepApkDistBrowse.Visible = false;
            }
            else if (idx == 1) // POS
            {
                txtDepLive.Text = _config.PosLivePath;
                txtDepStaging.Text = _config.PosStagingPath;
                txtDepBackup.Text = _config.PosBackupPath;
                txtDepIisSite.Text = _config.PosIisSite;
                txtDepIisAppPool.Text = _config.PosIisAppPool;
                
                chkDepMobileApk.Checked = _config.PosDeployMobileApk;
                txtDepApkDist.Text = _config.PosApkDistFolder;
                
                txtDepLive.Enabled = txtDepStaging.Enabled = txtDepBackup.Enabled = txtDepIisSite.Enabled = txtDepIisAppPool.Enabled = true;
                btnDepLiveBrowse.Enabled = btnDepStagingBrowse.Enabled = btnDepBackupBrowse.Enabled = true;
                
                chkDepMobileApk.Visible = lblDepApkDist.Visible = txtDepApkDist.Visible = btnDepApkDistBrowse.Visible = true;
                txtDepApkDist.Enabled = btnDepApkDistBrowse.Enabled = chkDepMobileApk.Checked;
            }
            else if (idx == 2) // WebApp
            {
                txtDepLive.Text = _config.WebAppLivePath;
                txtDepStaging.Text = _config.WebAppStagingPath;
                txtDepBackup.Text = _config.WebAppBackupPath;
                txtDepIisSite.Text = _config.WebAppIisSite;
                txtDepIisAppPool.Text = _config.WebAppIisAppPool;
                txtDepLive.Enabled = txtDepStaging.Enabled = txtDepBackup.Enabled = txtDepIisSite.Enabled = txtDepIisAppPool.Enabled = true;
                btnDepLiveBrowse.Enabled = btnDepStagingBrowse.Enabled = btnDepBackupBrowse.Enabled = true;
                
                chkDepMobileApk.Visible = lblDepApkDist.Visible = txtDepApkDist.Visible = btnDepApkDistBrowse.Visible = false;
            }
            else // Full Suite
            {
                txtDepLive.Text = "Configured in full deployment suite settings";
                txtDepStaging.Text = @"C:\inetpub\PublishFullSuiteStaging";
                txtDepBackup.Text = "Managed individually in backup directories";
                txtDepIisSite.Text = "Managed individually";
                txtDepIisAppPool.Text = "Managed individually";
                txtDepLive.Enabled = txtDepBackup.Enabled = txtDepIisSite.Enabled = txtDepIisAppPool.Enabled = false;
                btnDepLiveBrowse.Enabled = btnDepBackupBrowse.Enabled = false;
                
                chkDepMobileApk.Visible = lblDepApkDist.Visible = txtDepApkDist.Visible = btnDepApkDistBrowse.Visible = false;
            }
        }

        private void SaveDeployPaths()
        {
            if (_config == null) return;

            int idx = cmbDeployApp.SelectedIndex;
            if (idx < 0) return; // No selection yet
            if (idx == 0) // Mobile
            {
                _config.MobileLivePath = txtDepLive.Text.Trim();
                _config.MobileStagingPath = txtDepStaging.Text.Trim();
                _config.MobileBackupPath = txtDepBackup.Text.Trim();
                _config.MobileIisSite = txtDepIisSite.Text.Trim();
                _config.MobileIisAppPool = txtDepIisAppPool.Text.Trim();
            }
            else if (idx == 1) // POS
            {
                _config.PosLivePath = txtDepLive.Text.Trim();
                _config.PosStagingPath = txtDepStaging.Text.Trim();
                _config.PosBackupPath = txtDepBackup.Text.Trim();
                _config.PosIisSite = txtDepIisSite.Text.Trim();
                _config.PosIisAppPool = txtDepIisAppPool.Text.Trim();
                
                _config.PosDeployMobileApk = chkDepMobileApk.Checked;
                _config.PosApkDistFolder = txtDepApkDist.Text.Trim();
            }
            else if (idx == 2) // WebApp
            {
                _config.WebAppLivePath = txtDepLive.Text.Trim();
                _config.WebAppStagingPath = txtDepStaging.Text.Trim();
                _config.WebAppBackupPath = txtDepBackup.Text.Trim();
                _config.WebAppIisSite = txtDepIisSite.Text.Trim();
                _config.WebAppIisAppPool = txtDepIisAppPool.Text.Trim();
            }

            _config.AutoConfigureWebConfig = chkAutoConfigWebConfig.Checked;
            _config.PurgeFiles = chkPurgeFiles.Checked;
            _config.Save();
        }

        private void ChkDepMobileApk_CheckedChanged(object sender, EventArgs e)
        {
            txtDepApkDist.Enabled = btnDepApkDistBrowse.Enabled = chkDepMobileApk.Checked;
            SaveDeployPaths();
        }

        private void BtnDepApkDistBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select APK Distribution Folder on Server";
                fbd.SelectedPath = txtDepApkDist.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepApkDist.Text = fbd.SelectedPath;
                    SaveDeployPaths();
                }
            }
        }
        #endregion

        #region Operations Blocker Helper
        private void SetBusy(bool busy, string statusText)
        {
            _isBusy = busy;
            btnRunPublish.Enabled = !busy;
            btnRunDeploy.Enabled = !busy;
            btnRunRevisionSync.Enabled = !busy;

            btnTabPublish.Enabled = !busy;
            btnTabDeploy.Enabled = !busy;
            btnTabRevisions.Enabled = !busy;

            lblStatus.Text = "Status: " + statusText;
            lblStatus.ForeColor = busy ? ColorPrimary : ColorSuccess;

            if (!busy)
            {
                pbProgress.Style = ProgressBarStyle.Blocks;
                pbProgress.Value = 0;
            }
            else
            {
                pbProgress.Style = ProgressBarStyle.Marquee;
            }
        }
        #endregion

        #region Core Publish Operations (In-App Compression & Silent CLI)
        private async void BtnRunPublish_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            int selection = cmbPublishApp.SelectedIndex;
            if (selection < 0)
            {
                MessageBox.Show("Please select an application to publish.", "No App Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string workspace = txtPubWorkspace.Text.Trim();
            string outputDir = txtPubOutput.Text.Trim();

            if (string.IsNullOrEmpty(workspace) || !Directory.Exists(workspace))
            {
                MessageBox.Show("Please select a valid GFC-Studio workspace path.", "Missing Workspace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir))
            {
                MessageBox.Show("Please select a valid ZIP output directory.", "Missing Output Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save settings
            _config.WorkspacePath = workspace;
            _config.PublishOutputPath = outputDir;
            _config.PosPublishMobileApk = chkPublishMobileApk.Checked;
            _config.Save();

            SetBusy(true, "Publishing project...");
            rtbTerminal.Clear();
            rtbBuildOutput.Clear();

            bool success = false;

            try
            {
                if (selection == 0) // Mobile
                {
                    success = await PublishMobileAppAsync(workspace, outputDir);
                }
                else if (selection == 1) // POS
                {
                    success = await PublishPosAppAsync(workspace, outputDir);
                }
                else if (selection == 2) // WebApp
                {
                    success = await PublishWebAppAsync(workspace, outputDir);
                }
                else // Full Suite
                {
                    success = await PublishFullSuiteAsync(workspace, outputDir);
                }

                if (success)
                {
                    LogPublish(">>> PUBLISH OPERATION COMPLETED SUCCESSFULLY!", false, ColorSuccess);
                    LogPublish("Selected application has been successfully published and archived!", false, ColorSuccess);
                    SetBusy(false, "Ready");
                }
                else
                {
                    LogPublish("!!! PUBLISH PIPELINE ENCOUNTERED AN ERROR", true);
                    LogPublish("The build process encountered an error and failed (Exit Code != 0). Please check the build logs above for details.", true);
                    SetBusy(false, "Error: Build Failed!");
                }
            }
            catch (Exception ex)
            {
                Log($"CRITICAL PIPELINE EXCEPTION: {ex.Message}", true);
                SetBusy(false, "Exception Failure");
                MessageBox.Show($"Pipeline execution failed: {ex.Message}", "Critical Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> PublishMobileAppAsync(string workspace, string outputDir)
        {
            LogPublish(">>> [WAIT] Starting Mobile App Publish Pipeline...", false, ColorWait);
            
            // Stop process if active
            LogPublish(">>> Stopping GFC.Mobile process if running...", false, ColorWait);
            await KillProcessAsync("GFC.Mobile");

            // Compile
            string projectPath = Path.Combine("apps", "GFC-Mobile-Standalone", "GFC.Mobile", "GFC.Mobile.csproj");
            string tempOut = Path.Combine(workspace, "publish_mobile_temp");
            
            if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);

            LogPublish(">>> Executing dotnet publish...", false, ColorWait);
            var runner = new ProcessRunner((line, err) => LogPublish(line, err));
            int exit = await runner.RunAsync("dotnet", $"publish \"{projectPath}\" -c Release -o \"{tempOut}\" /p:TreatWarningsAsErrors=false", workspace);

            if (exit != 0)
            {
                LogPublish($"!!! dotnet publish failed with code {exit}", true);
                if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);
                return false;
            }

            // In-app ZIP
            string zipPath = Path.Combine(outputDir, "GFC_Mobile_Standalone.zip");
            LogPublish($">>> Compressing output files directly to: {zipPath}", false, ColorWait);
            
            await CreateZipFromDirectoryAsync(tempOut, zipPath);
            
            Directory.Delete(tempOut, true);
            LogPublish($">>> [OK] Mobile package successfully compressed.", false, ColorSuccess);
            return true;
        }

        private async Task<bool> PublishPosAppAsync(string workspace, string outputDir)
        {
            LogPublish(">>> [WAIT] Starting POS App Publish Pipeline...", false, ColorWait);

            // Sync POS versions automatically from Props before build
            LogPublish(">>> Reading PosVersion.props to sync versions...", false, ColorWait);
            string propsPath = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "PosVersion.props");
            if (!File.Exists(propsPath))
            {
                LogPublish("!!! Critical error: PosVersion.props not found at: " + propsPath, true);
                return false;
            }

            string xml = File.ReadAllText(propsPath);
            Match m = Regex.Match(xml, @"<PosVersion>(.*?)</PosVersion>");
            string version = m.Success ? m.Groups[1].Value.Trim() : "";

            if (string.IsNullOrEmpty(version))
            {
                LogPublish("!!! Could not read <PosVersion> in PosVersion.props.", true);
                return false;
            }

            LogPublish($">>> Triggering version synchronization to version {version}...", false, ColorWait);
            var syncRunner = new ProcessRunner((line, err) => LogPublish($"[SYNC] {line}", err));
            int syncExit = await syncRunner.RunAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -File \".\\sync-version.ps1\" -Project POS -Version \"{version}\"", workspace);
            
            if (syncExit != 0)
            {
                LogPublish("!!! Version synchronization execution failed.", true);
                return false;
            }

            bool webBuildSuccess = false;
            bool mobileBuildSuccess = true; // default true if not selected

            // STEP 1: Web Terminal Build
            LogPublish("\n=======================================================", false, ColorPrimary);
            LogPublish(">>> STEP 1/2: Publishing POS Web Terminal", false, ColorPrimary);
            LogPublish("=======================================================", false, ColorPrimary);

            string projectPath = Path.Combine("apps", "GFC-Pos-Standalone", "GFC.Pos.Terminal", "GFC.Pos.Terminal.csproj");
            string tempOut = Path.Combine(workspace, "publish_pos_temp");

            if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);

            LogPublish(">>> Executing dotnet publish for POS Terminal...", false, ColorWait);
            var runner = new ProcessRunner((line, err) => LogPublish(line, err));
            int exit = await runner.RunAsync("dotnet", $"publish \"{projectPath}\" -c Release -o \"{tempOut}\" /p:TreatWarningsAsErrors=false", workspace);

            if (exit == 0)
            {
                // Write version.txt
                string wwwrootPath = Path.Combine(tempOut, "wwwroot");
                File.WriteAllText(Path.Combine(wwwrootPath, "version.txt"), version);

                // In-app ZIP (zips wwwroot files)
                string zipPath = Path.Combine(outputDir, "GFC_POS_Standalone.zip");
                LogPublish($">>> Compressing output wwwroot directly to: {zipPath}", false, ColorWait);

                await CreateZipFromDirectoryAsync(wwwrootPath, zipPath);
                
                Directory.Delete(tempOut, true);
                LogPublish(">>> [OK] POS Web Terminal published and compressed successfully.", false, ColorSuccess);
                webBuildSuccess = true;
            }
            else
            {
                LogPublish($"!!! dotnet publish failed for POS Terminal with code {exit}", true);
                if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);
                webBuildSuccess = false;
            }

            // STEP 2: Native Android Mobile Build
            if (chkPublishMobileApk.Checked)
            {
                LogPublish("\n=======================================================", false, ColorPrimary);
                LogPublish(">>> STEP 2/2: Publishing POS Native Android Mobile", false, ColorPrimary);
                LogPublish("=======================================================", false, ColorPrimary);

                string mobileProjectPath = Path.Combine("apps", "GFC-Pos-Standalone", "GFC.Pos.Mobile", "GFC.Pos.Mobile.csproj");
                string mobileTempOut = Path.Combine(workspace, "publish_pos_mobile_temp");

                if (Directory.Exists(mobileTempOut)) Directory.Delete(mobileTempOut, true);

                LogPublish(">>> Executing dotnet publish for Native Android project...", false, ColorWait);
                var mobileRunner = new ProcessRunner((line, err) => LogPublish(line, err));
                int mobileExit = await mobileRunner.RunAsync("dotnet", $"publish \"{mobileProjectPath}\" -f net10.0-android -c Release -o \"{mobileTempOut}\" /p:TreatWarningsAsErrors=false", workspace);

                if (mobileExit == 0)
                {
                    LogPublish(">>> Locating generated APK installer file...", false, ColorWait);
                    string foundApk = null;
                    if (Directory.Exists(mobileTempOut))
                    {
                        var apks = Directory.GetFiles(mobileTempOut, "*.apk");
                        if (apks.Length > 0) foundApk = apks[0];
                    }
                    
                    // Fallback to bin folder
                    if (foundApk == null)
                    {
                        string fallbackDir = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Mobile", "bin", "Release", "net10.0-android");
                        if (Directory.Exists(fallbackDir))
                        {
                            var apks = Directory.GetFiles(fallbackDir, "*.apk");
                            if (apks.Length > 0) foundApk = apks[0];
                        }
                    }

                    if (foundApk != null)
                    {
                        string destApk = Path.Combine(outputDir, "GFC_POS_Mobile.apk");
                        LogPublish($">>> Copying APK to output directory: {destApk}", false, ColorWait);
                        File.Copy(foundApk, destApk, true);
                        LogPublish(">>> [OK] Native Android Mobile build and package completed successfully.", false, ColorSuccess);
                        mobileBuildSuccess = true;
                    }
                    else
                    {
                        LogPublish("!!! Failed to locate the generated .apk installer file in the output directories.", true);
                        mobileBuildSuccess = false;
                    }
                }
                else
                {
                    LogPublish($"!!! dotnet publish failed for POS Mobile with code {mobileExit}", true);
                    mobileBuildSuccess = false;
                }

                if (Directory.Exists(mobileTempOut)) Directory.Delete(mobileTempOut, true);
            }

            return webBuildSuccess && mobileBuildSuccess;
        }

        private async Task<bool> PublishWebAppAsync(string workspace, string outputDir)
        {
            LogPublish(">>> [WAIT] Starting Web App Publish Pipeline...", false, ColorWait);

            LogPublish(">>> Stopping GFC.BlazorServer process if running...", false, ColorWait);
            await KillProcessAsync("GFC.BlazorServer");

            string projectPath = Path.Combine("apps", "webapp", "GFC.BlazorServer", "GFC.BlazorServer.csproj");
            string tempOut = Path.Combine(workspace, "publish_webapp_temp");

            if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);

            LogPublish(">>> Executing dotnet publish for Blazor Server...", false, ColorWait);
            var runner = new ProcessRunner((line, err) => LogPublish(line, err));
            int exit = await runner.RunAsync("dotnet", $"publish \"{projectPath}\" -c Release -o \"{tempOut}\" /p:TreatWarningsAsErrors=false", workspace);

            if (exit != 0)
            {
                LogPublish($"!!! dotnet publish failed with code {exit}", true);
                if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);
                return false;
            }

            // In-app ZIP
            string zipPath = Path.Combine(outputDir, "PublishGFCWebApp.zip");
            LogPublish($">>> Compressing output files directly to: {zipPath}", false, ColorWait);

            await CreateZipFromDirectoryAsync(tempOut, zipPath);

            Directory.Delete(tempOut, true);
            LogPublish($">>> [OK] Web App package successfully compressed.", false, ColorSuccess);
            return true;
        }

        private async Task<bool> PublishFullSuiteAsync(string workspace, string outputDir)
        {
            LogPublish(">>> [WAIT] STARTING FULL SUITE INTEGRATED PUBLISH...", false, ColorWait);

            string tempOut = Path.Combine(workspace, "publish_suite_temp");
            if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);
            Directory.CreateDirectory(tempOut);

            // 1. WebApp
            string webOut = Path.Combine(tempOut, "webapp");
            LogPublish(">>> [1/3] Compiling Web App...", false, ColorWait);
            var runner = new ProcessRunner((line, err) => LogPublish(line, err));
            int exit1 = await runner.RunAsync("dotnet", $"publish \"apps\\webapp\\GFC.BlazorServer\\GFC.BlazorServer.csproj\" -c Release -o \"{webOut}\" /p:TreatWarningsAsErrors=false", workspace);
            if (exit1 != 0) { LogPublish("!!! Web App compile failed.", true); return false; }

            // 2. Mobile
            string mobOut = Path.Combine(tempOut, "mobile");
            LogPublish(">>> [2/3] Compiling Mobile App...", false, ColorWait);
            int exit2 = await runner.RunAsync("dotnet", $"publish \"apps\\GFC-Mobile-Standalone\\GFC.Mobile\\GFC.Mobile.csproj\" -c Release -o \"{mobOut}\" /p:TreatWarningsAsErrors=false", workspace);
            if (exit2 != 0) { LogPublish("!!! Mobile App compile failed.", true); return false; }

            // 3. POS
            string posOut = Path.Combine(tempOut, "pos");
            LogPublish(">>> [3/3] Compiling POS App...", false, ColorWait);
            int exit3 = await runner.RunAsync("dotnet", $"publish \"apps\\GFC-Pos-Standalone\\GFC.Pos.Terminal\\GFC.Pos.Terminal.csproj\" -c Release -o \"{posOut}\" /p:TreatWarningsAsErrors=false", workspace);
            if (exit3 != 0) { LogPublish("!!! POS App compile failed.", true); return false; }

            // Zip Suite
            string zipPath = Path.Combine(outputDir, "GFC_Full_Suite_Update.zip");
            LogPublish($">>> Compressing combined suite directly to: {zipPath}", false, ColorWait);

            await CreateZipFromDirectoryAsync(tempOut, zipPath);

            Directory.Delete(tempOut, true);
            LogPublish($">>> [OK] Full DevOps suite package successfully compressed.", false, ColorSuccess);
            return true;
        }

        private async Task KillProcessAsync(string name)
        {
            await Task.Run(() =>
            {
                foreach (var process in Process.GetProcessesByName(name))
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(3000);
                    }
                    catch
                    {
                        // ignore
                    }
                }
            });
        }

        private async Task CreateZipFromDirectoryAsync(string sourceDir, string destinationZip)
        {
            await Task.Run(() =>
            {
                if (File.Exists(destinationZip))
                {
                    File.Delete(destinationZip);
                }

                // In-app zipping streams updates of files
                using (var zip = ZipFile.Open(destinationZip, ZipArchiveMode.Create))
                {
                    var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
                    int total = files.Length;
                    int count = 0;

                    foreach (var file in files)
                    {
                        string entryName = file.Substring(sourceDir.Length).TrimStart('\\', '/');
                        Log($"[ZIP] Compressing: {entryName}", false, ColorTextMuted);
                        zip.CreateEntryFromFile(file, entryName);
                        count++;
                        
                        // Async progress indicator
                        if (count % 20 == 0 || count == total)
                        {
                            this.Invoke(new Action(() => {
                                lblStatus.Text = $"Status: Zipping ({count}/{total} files)...";
                            }));
                        }
                    }
                }
            });
        }
        #endregion

        #region Core Deployment Operations (Recursive Copy & IIS Control)
        private async void BtnRunDeploy_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            string zipInputPath = txtDepZip.Text.Trim();
            if (string.IsNullOrEmpty(zipInputPath) || !Directory.Exists(zipInputPath))
            {
                MessageBox.Show("Please select a valid directory containing deployment ZIPs.", "Invalid ZIP Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selection = cmbDeployApp.SelectedIndex;
            if (selection < 0)
            {
                MessageBox.Show("Please select an application to deploy.", "No App Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveDeployPaths();

            SetBusy(true, "Deploying package...");
            rtbTerminal.Clear();

            try
            {
                bool success = false;
                if (selection == 0) // Mobile
                {
                    success = await DeploySingleAppAsync(
                        "GFC_Mobile_Standalone.zip",
                        txtDepStaging.Text,
                        txtDepLive.Text,
                        txtDepBackup.Text,
                        txtDepIisSite.Text,
                        txtDepIisAppPool.Text,
                        true, // isWasm SPA routing
                        "wwwroot" // nested path inside staging
                    );
                }
                else if (selection == 1) // POS
                {
                    success = await DeploySingleAppAsync(
                        "GFC_POS_Standalone.zip",
                        txtDepStaging.Text,
                        txtDepLive.Text,
                        txtDepBackup.Text,
                        txtDepIisSite.Text,
                        txtDepIisAppPool.Text,
                        true, // isWasm SPA routing
                        "", // direct extract to root
                        chkDepMobileApk.Checked,
                        txtDepApkDist.Text
                    );
                }
                else if (selection == 2) // WebApp
                {
                    success = await DeploySingleAppAsync(
                        "PublishGFCWebApp.zip",
                        txtDepStaging.Text,
                        txtDepLive.Text,
                        txtDepBackup.Text,
                        txtDepIisSite.Text,
                        txtDepIisAppPool.Text,
                        false, // IIS standard .NET Core executable
                        "" // direct extract
                    );
                }
                else // Full Suite
                {
                    success = await DeployFullSuiteAsync(zipInputPath);
                }

                if (success)
                {
                    Log(">>> DEPLOYMENT PIPELINE SUCCESSFUL!", false, ColorSuccess);
                    SetBusy(false, "Ready");
                    MessageBox.Show("Deployment has completed successfully! IIS services are online.", "Deployment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Log("!!! DEPLOYMENT PIPELINE ENCOUNTERED ERRORS", true);
                    SetBusy(false, "Error: Deploy Failed!");
                    MessageBox.Show("Deployment failed. Review the terminal logs and ensure you are running as Administrator.", "Deployment Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Log($"CRITICAL DEPLOY EXCEPTION: {ex.Message}", true);
                SetBusy(false, "Exception Failure");
                MessageBox.Show($"Pipeline execution failed: {ex.Message}", "Critical Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> DeploySingleAppAsync(
            string zipName, 
            string staging, 
            string live, 
            string backup, 
            string siteName, 
            string appPool, 
            bool isWasm, 
            string nestedStagingPath,
            bool deployApk = false,
            string apkDistFolder = "")
        {
            Log($">>> [WAIT] Starting Deployment for: {zipName}...", false, ColorWait);

            // Locate zip file
            string zipPath = Path.Combine(txtDepZip.Text.Trim(), zipName);
            if (!File.Exists(zipPath))
            {
                Log($"!!! ZIP file not found at: {zipPath}", true);
                return false;
            }

            // Stop IIS Pool & Site
            Log($">>> Stopping IIS site and application pool for '{siteName}'...", false, ColorWait);
            await ToggleIisAsync(siteName, appPool, false);

            try
            {
                // Unzip Staging
                Log(">>> Preparing Staging...", false, ColorWait);
                if (Directory.Exists(staging)) Directory.Delete(staging, true);
                Directory.CreateDirectory(staging);

                Log($">>> Unzipping {zipName} directly into staging...", false, ColorWait);
                await ExtractZipAsync(zipPath, staging);

                // Site Backup
                Log(">>> Preparing site backup...", false, ColorWait);
                if (!Directory.Exists(backup)) Directory.CreateDirectory(backup);
                string ts = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFolder = Path.Combine(backup, "Backup_" + ts);
                
                if (Directory.Exists(live))
                {
                    Log($">>> Copying live files to backup: {backupFolder}", false, ColorWait);
                    await CopyDirectoryAsync(live, backupFolder, false, null); // don't filter backup
                }

                // Deploy files staging -> live
                Log($">>> Deploying new staging files to live: {live}...", false, ColorWait);
                string sourceCopy = string.IsNullOrEmpty(nestedStagingPath) ? staging : Path.Combine(staging, nestedStagingPath);

                if (!Directory.Exists(live)) Directory.CreateDirectory(live);

                // Exclude Production appsettings or web.config from purge if checked
                var exclusions = new string[] { "appsettings.Production.json", "web.config" };
                await CopyDirectoryAsync(sourceCopy, live, chkPurgeFiles.Checked, exclusions);

                // Config web.config
                if (isWasm && chkAutoConfigWebConfig.Checked)
                {
                    Log(">>> Writing flat optimized web.config for Blazor WASM SPA...", false, ColorWait);
                    WriteWasmWebConfig(live, deployApk);
                }

                // Deploy APK if dual-track is active
                if (deployApk && !string.IsNullOrEmpty(apkDistFolder))
                {
                    Log(">>> [WAIT] Deploying Native Mobile APK...", false, ColorWait);
                    string foundApkPath = null;
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    
                    // Search in desktop and deployment ZIP folder
                    string[] searchFiles = new string[]
                    {
                        Path.Combine(txtDepZip.Text.Trim(), "GFC_POS_Mobile.apk"),
                        Path.Combine(desktop, "GFC_POS_Mobile.apk"),
                        Path.Combine(txtDepZip.Text.Trim(), "com.gfc.pos.mobile-Signed.apk"),
                        Path.Combine(desktop, "com.gfc.pos.mobile-Signed.apk")
                    };

                    foreach (var path in searchFiles)
                    {
                        if (File.Exists(path))
                        {
                            foundApkPath = path;
                            break;
                        }
                    }

                    // Fallback to searching for any *.apk file in the directories
                    if (foundApkPath == null)
                    {
                        if (Directory.Exists(txtDepZip.Text.Trim()))
                        {
                            var apks = Directory.GetFiles(txtDepZip.Text.Trim(), "*.apk");
                            if (apks.Length > 0) foundApkPath = apks[0];
                        }
                        if (foundApkPath == null && Directory.Exists(desktop))
                        {
                            var apks = Directory.GetFiles(desktop, "*.apk");
                            if (apks.Length > 0) foundApkPath = apks[0];
                        }
                    }

                    if (foundApkPath != null)
                    {
                        Log($">>> Found APK installer at: {foundApkPath}", false, ColorWait);
                        if (!Directory.Exists(apkDistFolder))
                        {
                            Directory.CreateDirectory(apkDistFolder);
                        }
                        string destApk = Path.Combine(apkDistFolder, "GFC_POS_Mobile.apk");
                        Log($">>> Copying APK to distribution path: {destApk}", false, ColorWait);
                        File.Copy(foundApkPath, destApk, true);
                        Log($">>> [OK] APK successfully copied to server.", false, ColorSuccess);

                        // Safe source cleanup matching dynamic scripting behavior
                        try 
                        { 
                            File.Delete(foundApkPath); 
                            Log($">>> Cleanup: Source APK '{Path.GetFileName(foundApkPath)}' deleted.", false, ColorTextMuted); 
                        } 
                        catch 
                        { 
                        }
                    }
                    else
                    {
                        Log("!!! Warning: Dual-track APK deployment was selected, but no .apk installer file was found in search paths or on Desktop.", true);
                    }
                }

                // Delete ZIP if completed
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                    Log($">>> Cleanup: Source ZIP '{zipName}' purged from directory.", false, ColorTextMuted);
                }

                Log($">>> Starting IIS site and application pool for '{siteName}'...", false, ColorWait);
                await ToggleIisAsync(siteName, appPool, true);

                // Clean staging
                if (Directory.Exists(staging)) Directory.Delete(staging, true);

                Log($">>> [OK] Deployment for {zipName} finished successfully.", false, ColorSuccess);
                return true;
            }
            catch (Exception ex)
            {
                Log($"!!! Deployment of {zipName} failed: {ex.Message}", true);
                
                // Attempt restore toggle
                Log(">>> Attempting to restart IIS on fallback...", false, ColorWarning);
                await ToggleIisAsync(siteName, appPool, true);
                return false;
            }
        }

        private async Task<bool> DeployFullSuiteAsync(string zipDir)
        {
            Log(">>> [WAIT] STARTING FULL SUITE INTEGRATED DEPLOYMENT...", false, ColorWait);

            string zipName = "GFC_Full_Suite_Update.zip";
            string zipPath = Path.Combine(zipDir, zipName);
            if (!File.Exists(zipPath))
            {
                Log($"!!! Combined Full Suite Zip not found at: {zipPath}", true);
                return false;
            }

            string staging = @"C:\inetpub\PublishFullSuiteStaging";
            Log(">>> Preparing Suite Staging...", false, ColorWait);
            if (Directory.Exists(staging)) Directory.Delete(staging, true);
            Directory.CreateDirectory(staging);

            Log(">>> Extracting full suite ZIP package...", false, ColorWait);
            await ExtractZipAsync(zipPath, staging);

            // Deploy individuals
            bool ok1 = await DeploySingleAppAsync(
                "PublishGFCWebApp.zip",
                _config.WebAppStagingPath,
                _config.WebAppLivePath,
                _config.WebAppBackupPath,
                _config.WebAppIisSite,
                _config.WebAppIisAppPool,
                false,
                ""
            );

            bool ok2 = await DeploySingleAppAsync(
                "GFC_Mobile_Standalone.zip",
                _config.MobileStagingPath,
                _config.MobileLivePath,
                _config.MobileBackupPath,
                _config.MobileIisSite,
                _config.MobileIisAppPool,
                true,
                "wwwroot"
            );

            bool ok3 = await DeploySingleAppAsync(
                "GFC_POS_Standalone.zip",
                _config.PosStagingPath,
                _config.PosLivePath,
                _config.PosBackupPath,
                _config.PosIisSite,
                _config.PosIisAppPool,
                true,
                ""
            );

            // Clean combined staging
            if (Directory.Exists(staging)) Directory.Delete(staging, true);

            if (File.Exists(zipPath)) File.Delete(zipPath);

            return ok1 && ok2 && ok3;
        }

        private async Task ToggleIisAsync(string site, string pool, bool start)
        {
            string cmd = start ? "Start" : "Stop";
            var iisRunner = new ProcessRunner((line, err) => Log($"[IIS] {line}", err));

            // Stop/Start Pool
            await iisRunner.RunAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"Import-Module WebAdministration; {cmd}-WebAppPool -Name '{pool}'\"", Environment.SystemDirectory);
            // Stop/Start Site
            await iisRunner.RunAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"Import-Module WebAdministration; {cmd}-Website -Name '{site}'\"", Environment.SystemDirectory);

            await Task.Delay(1500); // cooldown sleep
        }

        private async Task ExtractZipAsync(string zipPath, string destFolder)
        {
            await Task.Run(() =>
            {
                using (var archive = ZipFile.OpenRead(zipPath))
                {
                    int total = archive.Entries.Count;
                    int count = 0;

                    foreach (var entry in archive.Entries)
                    {
                        if (string.IsNullOrEmpty(entry.Name)) continue; // skip directories

                        string fullDest = Path.Combine(destFolder, entry.FullName);
                        string parent = Path.GetDirectoryName(fullDest) ?? "";
                        if (!Directory.Exists(parent)) Directory.CreateDirectory(parent);

                        entry.ExtractToFile(fullDest, true);
                        count++;

                        if (count % 25 == 0 || count == total)
                        {
                            this.Invoke(new Action(() => {
                                lblStatus.Text = $"Status: Extracting ({count}/{total} files)...";
                            }));
                        }
                    }
                }
            });
        }

        private async Task CopyDirectoryAsync(string source, string dest, bool purgeDest, string[]? exclusions)
        {
            await Task.Run(() =>
            {
                if (purgeDest && Directory.Exists(dest))
                {
                    // Purge target but preserve files in exclusions
                    foreach (var file in Directory.GetFiles(dest, "*", SearchOption.AllDirectories))
                    {
                        bool isExcluded = false;
                        if (exclusions != null)
                        {
                            foreach (var exc in exclusions)
                            {
                                if (Path.GetFileName(file).Equals(exc, StringComparison.OrdinalIgnoreCase))
                                {
                                    isExcluded = true;
                                    break;
                                }
                            }
                        }

                        if (!isExcluded)
                        {
                            try { File.Delete(file); } catch { /* ignore locked files */ }
                        }
                    }
                }

                // Copy files recursively
                var files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
                int total = files.Length;
                int count = 0;

                foreach (var file in files)
                {
                    string rel = file.Substring(source.Length).TrimStart('\\', '/');
                    string destFile = Path.Combine(dest, rel);

                    // Check exclusions
                    bool skip = false;
                    if (exclusions != null && File.Exists(destFile))
                    {
                        foreach (var exc in exclusions)
                        {
                            if (Path.GetFileName(file).Equals(exc, StringComparison.OrdinalIgnoreCase))
                            {
                                skip = true;
                                break;
                            }
                        }
                    }

                    if (!skip)
                    {
                        string parent = Path.GetDirectoryName(destFile) ?? "";
                        if (!Directory.Exists(parent)) Directory.CreateDirectory(parent);

                        File.Copy(file, destFile, true);
                    }

                    count++;
                    if (count % 25 == 0 || count == total)
                    {
                        this.Invoke(new Action(() => {
                            lblStatus.Text = $"Status: Copying files ({count}/{total})...";
                        }));
                    }
                }
            });
        }

        private void WriteWasmWebConfig(string livePath, bool deployApk)
        {
            string apkMime = deployApk 
                ? "\r\n      <remove fileExtension=\".apk\" />\r\n      <mimeMap fileExtension=\".apk\" mimeType=\"application/vnd.android.package-archive\" />" 
                : "";

            string cleanWebConfig = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<configuration>
  <system.webServer>
    <defaultDocument>
      <files>
        <clear />
        <add value=""index.html"" />
      </files>
    </defaultDocument>
    <staticContent>
      <clientCache cacheControlMode=""DisableCache"" />
      <remove fileExtension="".blat"" />
      <remove fileExtension="".dat"" />
      <remove fileExtension="".dll"" />
      <remove fileExtension="".webcil"" />
      <remove fileExtension="".json"" />
      <remove fileExtension="".txt"" />
      <remove fileExtension="".wasm"" />
      <remove fileExtension="".woff"" />
      <remove fileExtension="".woff2"" />{apkMime}
      <mimeMap fileExtension="".blat"" mimeType=""application/octet-stream"" />
      <mimeMap fileExtension="".dll"" mimeType=""application/octet-stream"" />
      <mimeMap fileExtension="".webcil"" mimeType=""application/octet-stream"" />
      <mimeMap fileExtension="".dat"" mimeType=""application/octet-stream"" />
      <mimeMap fileExtension="".json"" mimeType=""application/json"" />
      <mimeMap fileExtension="".txt"" mimeType=""text/plain"" />
      <mimeMap fileExtension="".wasm"" mimeType=""application/wasm"" />
      <mimeMap fileExtension="".woff"" mimeType=""application/font-woff"" />
      <mimeMap fileExtension="".woff2"" mimeType=""application/font-woff"" />
      <remove fileExtension="".webmanifest"" />
      <mimeMap fileExtension="".webmanifest"" mimeType=""application/manifest+json"" />
    </staticContent>
    <httpCompression>
      <dynamicTypes>
        <add mimeType=""application/octet-stream"" enabled=""true"" />
        <add mimeType=""application/wasm"" enabled=""true"" />
      </dynamicTypes>
    </httpCompression>
    <rewrite>
      <rules>
        <rule name=""SPA fallback routing"" stopProcessing=""true"">
          <match url="".*"" />
          <conditions logicalGrouping=""MatchAll"">
            <add input=""{{REQUEST_FILENAME}}"" matchType=""IsFile"" negate=""true"" />
          </conditions>
          <action type=""Rewrite"" url=""index.html"" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>";

            string path = Path.Combine(livePath, "web.config");
            File.WriteAllText(path, cleanWebConfig, System.Text.Encoding.UTF8);
        }
        #endregion

        #region Core Revision Operations
        private async void BtnRunRevisionSync_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            string workspace = txtPubWorkspace.Text.Trim();
            if (string.IsNullOrEmpty(workspace) || !Directory.Exists(workspace))
            {
                MessageBox.Show("Please select a valid GFC-Studio workspace path under the Publish tab to synchronize version revisions.", "Missing Workspace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string project = cmbRevAppSelect.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(project))
            {
                MessageBox.Show("Please select a target project before running the revision sync.", "No Project Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Build parameters
            string args = $"-Project \"{project}\"";

            if (radRevNext.Checked)
            {
                args += " -Next";
            }
            else
            {
                string custVer = txtRevCustomValue.Text.Trim();
                if (string.IsNullOrEmpty(custVer))
                {
                    MessageBox.Show("Please specify a valid custom version number (e.g. 2.4.26).", "Invalid Version", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                args += $" -Version \"{custVer}\"";
            }

            if (chkRevDryRun.Checked)
            {
                args += " -DryRun";
            }

            SetBusy(true, "Syncing version revisions...");
            rtbTerminal.Clear();
            Log($">>> [WAIT] Triggering Version Revision Sync for project: {project}...", false, ColorWait);
            Log($">>> Executing command: powershell.exe -File .\\sync-version.ps1 {args}", false, ColorTextMuted);

            try
            {
                var runner = new ProcessRunner((line, err) => Log($"[SYNC] {line}", err));
                int exit = await runner.RunAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -File \".\\sync-version.ps1\" {args}", workspace);

                if (exit == 0)
                {
                    Log(">>> REVISION SYNCHRONIZATION PIPELINE SUCCESSFUL!", false, ColorSuccess);
                    SetBusy(false, "Ready");
                    MessageBox.Show("Version sync completed successfully!", "Sync Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Rescan and update UI dashboard
                    RefreshVersionDashboard();
                }
                else
                {
                    Log($"!!! sync-version.ps1 execution failed with code {exit}", true);
                    SetBusy(false, "Error: Sync Failed!");
                    MessageBox.Show("Version sync script execution failed. Check errors in logs.", "Sync Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Log($"CRITICAL SYNC EXCEPTION: {ex.Message}", true);
                SetBusy(false, "Exception Failure");
                MessageBox.Show($"Pipeline execution failed: {ex.Message}", "Critical Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
