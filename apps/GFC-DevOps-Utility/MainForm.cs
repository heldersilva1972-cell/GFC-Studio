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
        private bool _isUpdatingUi = false;
        private bool _isSyncingChecks = false;

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

            _isUpdatingUi = true;

            // Bind APK checkbox states and wire events (force defaults to false as requested)
            foreach (var app in _config.AppPipelines)
            {
                app.PublishMobileApk = false;
            }
            _config.PosPublishMobileApk = false;
            _config.Save();

            chkApkMobile.Checked = false;
            chkApkPos.Checked = false;
            chkApkWebApp.Checked = false;

            _isUpdatingUi = false;

            chkApkMobile.CheckedChanged += ChkApkMobile_CheckedChanged;
            chkApkPos.CheckedChanged += ChkApkPos_CheckedChanged;
            chkApkWebApp.CheckedChanged += ChkApkWebApp_CheckedChanged;

            // Hide legacy comboboxes visually
            cmbPublishApp.Visible = false;
            cmbPublishApp.Enabled = false;
            cmbDeployApp.Visible = false;
            cmbDeployApp.Enabled = false;
            cmbRevAppSelect.SelectedIndex = -1;

            InitializeRevisionsTable();
            RefreshVersionDashboard();

            // Initialize CheckedListBox controls
            _isUpdatingUi = true;
            clbPublishApps.Items.Clear();
            clbDeployApps.Items.Clear();
            foreach (var app in _config.AppPipelines)
            {
                clbPublishApps.Items.Add(app.AppName);
                clbDeployApps.Items.Add(app.AppName);
            }

            // None checked by default as requested

            // No default highlighting on load
            if (clbPublishApps.Items.Count > 0) clbPublishApps.SelectedIndex = -1;
            if (clbDeployApps.Items.Count > 0) clbDeployApps.SelectedIndex = -1;

            // Wire text changes programmatically for detail updating
            txtPubOutput.TextChanged += TxtPubOutput_TextChanged;
            txtDepIisSite.TextChanged += TxtDepIisSite_TextChanged;
            txtDepIisAppPool.TextChanged += TxtDepIisAppPool_TextChanged;

            _isUpdatingUi = false;

            // Trigger manual master-detail updates
            ClbPublishApps_SelectedIndexChanged(clbPublishApps, EventArgs.Empty);
            ClbDeployApps_SelectedIndexChanged(clbDeployApps, EventArgs.Empty);

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
                    
                    // Update active highlighted app config too
                    if (clbPublishApps.SelectedIndex >= 0)
                    {
                        var selectedAppName = clbPublishApps.SelectedItem?.ToString();
                        var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
                        if (appConfig != null)
                        {
                            appConfig.SourceZipFolder = fbd.SelectedPath;
                        }
                    }
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

                    // Update active highlighted app config too
                    if (clbDeployApps.SelectedIndex >= 0)
                    {
                        var selectedAppName = clbDeployApps.SelectedItem?.ToString();
                        var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
                        if (appConfig != null)
                        {
                            appConfig.SourceZipFolder = fbd.SelectedPath;
                        }
                    }
                    _config.Save();
                }
            }
        }

        private void BtnDepStagingBrowse_Click(object sender, EventArgs e)
        {
            // Staging folder is hidden/unused, but preserved for reference compatibility
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepStaging.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnDepLiveBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = txtDepLive.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepLive.Text = fbd.SelectedPath;
                    if (clbDeployApps.SelectedIndex >= 0)
                    {
                        var selectedAppName = clbDeployApps.SelectedItem?.ToString();
                        var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
                        if (appConfig != null)
                        {
                            appConfig.LiveTargetFolder = fbd.SelectedPath;
                            _config.Save();
                        }
                    }
                }
            }
        }

        private void BtnDepBackupBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = txtDepBackup.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDepBackup.Text = fbd.SelectedPath;
                    if (clbDeployApps.SelectedIndex >= 0)
                    {
                        var selectedAppName = clbDeployApps.SelectedItem?.ToString();
                        var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
                        if (appConfig != null)
                        {
                            appConfig.ArchiveFolder = fbd.SelectedPath;
                            _config.Save();
                        }
                    }
                }
            }
        }

        private void ChkDepMobileApk_CheckedChanged(object sender, EventArgs e)
        {
            txtDepApkDist.Enabled = btnDepApkDistBrowse.Enabled = chkDepMobileApk.Checked;
            _config.PosDeployMobileApk = chkDepMobileApk.Checked;
            _config.Save();
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
                    _config.PosApkDistFolder = fbd.SelectedPath;
                    _config.Save();
                }
            }
        }

        private void RestoreDeployPaths() { }
        private void SaveDeployPaths() { }
        #endregion

        #region CheckedListBox Master-Detail & Tabs Sync
        private void ClbPublishApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi) return;

            if (clbPublishApps.SelectedIndex < 0)
            {
                lblAppDesc.Text = "Click/hover an item to see its purpose.";
                return;
            }

            var selectedAppName = clbPublishApps.SelectedItem?.ToString();
            if (selectedAppName == "Mobile")
            {
                lblAppDesc.Text = "Mobile: Publishes the Android / handheld client application package.";
            }
            else if (selectedAppName == "POS")
            {
                lblAppDesc.Text = "POS: Publishes the raw TCP-capable Point of Sale terminal.";
            }
            else if (selectedAppName == "WebApp")
            {
                lblAppDesc.Text = "WebApp: Publishes the Blazor Server administration web portal.";
            }
            else
            {
                lblAppDesc.Text = "Click/hover an item to see its purpose.";
            }

            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                _isUpdatingUi = true;
                txtPubOutput.Text = appConfig.SourceZipFolder;
                _isUpdatingUi = false;
            }
        }

        private void ClbDeployApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi || clbDeployApps.SelectedIndex < 0) return;

            var selectedAppName = clbDeployApps.SelectedItem?.ToString();
            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                _isUpdatingUi = true;
                txtDepZip.Text = appConfig.SourceZipFolder;
                txtDepLive.Text = appConfig.LiveTargetFolder;
                txtDepBackup.Text = appConfig.ArchiveFolder;
                txtDepIisSite.Text = appConfig.IisSiteName;
                txtDepIisAppPool.Text = appConfig.IisAppPool;

                bool isPos = appConfig.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase);
                chkDepMobileApk.Visible = isPos;
                lblDepApkDist.Visible = isPos;
                txtDepApkDist.Visible = isPos;
                btnDepApkDistBrowse.Visible = isPos;

                if (isPos)
                {
                    chkDepMobileApk.Checked = _config.PosDeployMobileApk;
                    txtDepApkDist.Text = _config.PosApkDistFolder;
                    txtDepApkDist.Enabled = btnDepApkDistBrowse.Enabled = chkDepMobileApk.Checked;
                }
                _isUpdatingUi = false;
            }
        }

        private void ClbPublishApps_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_isSyncingChecks) return;
            _isSyncingChecks = true;
            clbDeployApps.SetItemChecked(e.Index, e.NewValue == CheckState.Checked);
            _isSyncingChecks = false;
        }

        private void ClbDeployApps_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_isSyncingChecks) return;
            _isSyncingChecks = true;
            clbPublishApps.SetItemChecked(e.Index, e.NewValue == CheckState.Checked);
            _isSyncingChecks = false;
        }



        private void ChkApkMobile_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi) return;
            var appConfig = _config.AppPipelines.Find(a => a.AppName == "Mobile");
            if (appConfig != null)
            {
                appConfig.PublishMobileApk = chkApkMobile.Checked;
                _config.Save();
            }
        }

        private void ChkApkPos_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi) return;
            var appConfig = _config.AppPipelines.Find(a => a.AppName == "POS");
            if (appConfig != null)
            {
                appConfig.PublishMobileApk = chkApkPos.Checked;
                _config.Save();
            }
        }

        private void ChkApkWebApp_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi) return;
            var appConfig = _config.AppPipelines.Find(a => a.AppName == "WebApp");
            if (appConfig != null)
            {
                appConfig.PublishMobileApk = chkApkWebApp.Checked;
                _config.Save();
            }
        }

        private void TxtPubOutput_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi || clbPublishApps.SelectedIndex < 0) return;
            var selectedAppName = clbPublishApps.SelectedItem?.ToString();
            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                appConfig.SourceZipFolder = txtPubOutput.Text.Trim();
                _config.Save();
            }
        }

        private void TxtDepZip_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi || clbDeployApps.SelectedIndex < 0) return;
            var selectedAppName = clbDeployApps.SelectedItem?.ToString();
            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                appConfig.SourceZipFolder = txtDepZip.Text.Trim();
                _config.Save();
            }
        }

        private void TxtDepLive_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi || clbDeployApps.SelectedIndex < 0) return;
            var selectedAppName = clbDeployApps.SelectedItem?.ToString();
            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                appConfig.LiveTargetFolder = txtDepLive.Text.Trim();
                _config.Save();
            }
        }

        private void TxtDepBackup_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi || clbDeployApps.SelectedIndex < 0) return;
            var selectedAppName = clbDeployApps.SelectedItem?.ToString();
            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                appConfig.ArchiveFolder = txtDepBackup.Text.Trim();
                _config.Save();
            }
        }

        private void TxtDepIisSite_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi || clbDeployApps.SelectedIndex < 0) return;
            var selectedAppName = clbDeployApps.SelectedItem?.ToString();
            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                appConfig.IisSiteName = txtDepIisSite.Text.Trim();
                _config.Save();
            }
        }

        private void TxtDepIisAppPool_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUi || clbDeployApps.SelectedIndex < 0) return;
            var selectedAppName = clbDeployApps.SelectedItem?.ToString();
            var appConfig = _config.AppPipelines.Find(a => a.AppName == selectedAppName);
            if (appConfig != null)
            {
                appConfig.IisAppPool = txtDepIisAppPool.Text.Trim();
                _config.Save();
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
        private string GetAppRevision(string appName, string workspace)
        {
            string version = "1.0.0";
            if (appName.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
            {
                version = VersionScanner.GetMobileVersion(workspace);
            }
            else if (appName.Equals("POS", StringComparison.OrdinalIgnoreCase))
            {
                version = VersionScanner.GetPosVersion(workspace);
            }
            else if (appName.Equals("WebApp", StringComparison.OrdinalIgnoreCase))
            {
                version = VersionScanner.GetWebAppVersion(workspace);
            }
            
            if (string.IsNullOrEmpty(version) || version.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
            {
                version = "1.0.0";
            }
            
            if (!version.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                version = "v" + version;
            }
            
            return version;
        }

        private async void BtnRunPublish_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            var checkedApps = new List<AppPipelineConfig>();
            foreach (var item in clbPublishApps.CheckedItems)
            {
                var app = _config.AppPipelines.Find(a => a.AppName == item.ToString());
                if (app != null) checkedApps.Add(app);
            }

            if (checkedApps.Count == 0)
            {
                MessageBox.Show("Please check at least one application to publish.", "No App Checked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string workspace = txtPubWorkspace.Text.Trim();
            if (string.IsNullOrEmpty(workspace) || !Directory.Exists(workspace))
            {
                MessageBox.Show("Please select a valid GFC-Studio workspace path.", "Missing Workspace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save settings
            _config.WorkspacePath = workspace;
            _config.Save();

            SetBusy(true, "Publishing projects...");
            rtbTerminal.Clear();
            rtbBuildOutput.Clear();
            LogPublish($">>> STARTING BATCH PUBLISH FOR {checkedApps.Count} TARGET(S)...", false, ColorWait);

            // Initialize progress status indicators
            foreach (var appConfig in _config.AppPipelines)
            {
                bool isSelected = checkedApps.Exists(a => a.AppName == appConfig.AppName);
                if (isSelected)
                {
                    UpdateVisualStatus(appConfig.AppName, false, "Queued ⏳", Color.Orange);
                    if (appConfig.PublishMobileApk)
                    {
                        UpdateVisualStatus(appConfig.AppName, true, "Queued ⏳", Color.Orange);
                    }
                    else
                    {
                        UpdateVisualStatus(appConfig.AppName, true, "APK: -", Color.Gray);
                    }
                }
                else
                {
                    UpdateVisualStatus(appConfig.AppName, false, "Skipped ➔", Color.LightGray);
                    UpdateVisualStatus(appConfig.AppName, true, "APK: -", Color.Gray);
                }
            }

            var successes = new List<string>();
            var failures = new Dictionary<string, string>();

            foreach (var app in checkedApps)
            {
                if (app.PublishMobileApk)
                {
                    UpdateVisualStatus(app.AppName, false, "APK Only", Color.Gray);
                    UpdateVisualStatus(app.AppName, true, "Waiting ⏳", Color.Orange);
                }
                else
                {
                    UpdateVisualStatus(app.AppName, false, "Compiling... ⚙️", Color.DeepSkyBlue);
                }
                try
                {
                    LogPublish($"\n=======================================================", false, ColorPrimary);
                    LogPublish($">>> STARTING PUBLISH PIPELINE FOR: {app.AppName}", false, ColorPrimary);
                    LogPublish($"=======================================================", false, ColorPrimary);

                    string revision = GetAppRevision(app.AppName, workspace);
                    bool appSuccess = false;

                    if (app.PublishMobileApk)
                    {
                        appSuccess = await PublishApkHelperAsync(workspace, app, revision);
                    }
                    else
                    {
                        if (app.AppName.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
                        {
                            appSuccess = await PublishMobileAppAsync(workspace, app, revision);
                        }
                        else if (app.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase))
                        {
                            appSuccess = await PublishPosAppAsync(workspace, app, revision);
                        }
                        else if (app.AppName.Equals("WebApp", StringComparison.OrdinalIgnoreCase))
                        {
                            appSuccess = await PublishWebAppAsync(workspace, app, revision);
                        }
                    }

                    if (appSuccess)
                    {
                        successes.Add(app.AppName);
                        LogPublish($">>> [OK] Publish pipeline SUCCEEDED for: {app.AppName}", false, ColorSuccess);
                        if (!app.PublishMobileApk)
                        {
                            UpdateVisualStatus(app.AppName, false, "Success ✅", Color.Green);
                        }
                    }
                    else
                    {
                        failures.Add(app.AppName, "dotnet publish exited with compilation errors.");
                        LogPublish($"!!! [FAIL] Publish pipeline FAILED for: {app.AppName}", true);
                        UpdateVisualStatus(app.AppName, false, "Failed ❌", Color.Red);
                        if (app.PublishMobileApk)
                        {
                            UpdateVisualStatus(app.AppName, true, "Failed ❌", Color.Red);
                        }
                    }
                }
                catch (Exception ex)
                {
                    failures.Add(app.AppName, ex.Message);
                    LogPublish($"!!! [EXCEPTION] Publish pipeline failed for {app.AppName}: {ex.Message}", true);
                    UpdateVisualStatus(app.AppName, false, "Failed ❌", Color.Red);
                    if (app.PublishMobileApk)
                    {
                        UpdateVisualStatus(app.AppName, true, "Failed ❌", Color.Red);
                    }
                }
            }

            // Clear checked items and selection on completion
            _isSyncingChecks = true;
            for (int i = 0; i < clbPublishApps.Items.Count; i++)
            {
                clbPublishApps.SetItemChecked(i, false);
                clbDeployApps.SetItemChecked(i, false);
            }
            clbPublishApps.SelectedIndex = -1;
            clbDeployApps.SelectedIndex = -1;
            if (lblAppDesc != null) lblAppDesc.Text = "Click/hover an item to see its purpose.";
            _isSyncingChecks = false;

            SetBusy(false, failures.Count == 0 ? "Ready" : "Error: Build Failed!");

            string summaryMsg = $"Batch Publish Operation Completed.\n\n" +
                                $"Successful ({successes.Count}):\n" +
                                (successes.Count > 0 ? string.Join("\n", successes.ConvertAll(s => $" - {s}")) : " None") + "\n\n" +
                                $"Failed ({failures.Count}):\n" +
                                (failures.Count > 0 ? string.Join("\n", new List<string>(failures.Keys).ConvertAll(k => $" - {k}: {failures[k]}")) : " None");

            MessageBox.Show(summaryMsg, "Publish Batch Summary", MessageBoxButtons.OK, 
                            failures.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private async Task<bool> PublishMobileAppAsync(string workspace, AppPipelineConfig app, string revision)
        {
            LogPublish($">>> [WAIT] Starting Mobile App Publish Pipeline for revision {revision}...", false, ColorWait);
            
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

            // In-app ZIP of wwwroot (or entire temp if no wwwroot exists)
            string zipSrc = tempOut;
            if (Directory.Exists(Path.Combine(tempOut, "wwwroot")))
            {
                zipSrc = Path.Combine(tempOut, "wwwroot");
            }

            if (!Directory.Exists(app.SourceZipFolder))
            {
                Directory.CreateDirectory(app.SourceZipFolder);
            }
            string zipPath = Path.Combine(app.SourceZipFolder, $"{app.AppName}_{revision}.zip");
            LogPublish($">>> Compressing output files directly to: {zipPath}", false, ColorWait);
            
            await CreateZipFromDirectoryAsync(zipSrc, zipPath);
            
            Directory.Delete(tempOut, true);
            LogPublish($">>> [OK] Mobile package successfully compressed.", false, ColorSuccess);

            return true;
        }

        private async Task<bool> PublishPosAppAsync(string workspace, AppPipelineConfig app, string revision)
        {
            LogPublish($">>> [WAIT] Starting POS App Publish Pipeline for revision {revision}...", false, ColorWait);

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
                if (!Directory.Exists(wwwrootPath)) Directory.CreateDirectory(wwwrootPath);
                File.WriteAllText(Path.Combine(wwwrootPath, "version.txt"), version);

                if (!Directory.Exists(app.SourceZipFolder))
                {
                    Directory.CreateDirectory(app.SourceZipFolder);
                }
                string zipPath = Path.Combine(app.SourceZipFolder, $"{app.AppName}_{revision}.zip");
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
            return webBuildSuccess;
        }

        private async Task<bool> PublishWebAppAsync(string workspace, AppPipelineConfig app, string revision)
        {
            LogPublish($">>> [WAIT] Starting Web App Publish Pipeline for revision {revision}...", false, ColorWait);

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

            if (!Directory.Exists(app.SourceZipFolder))
            {
                Directory.CreateDirectory(app.SourceZipFolder);
            }
            string zipPath = Path.Combine(app.SourceZipFolder, $"{app.AppName}_{revision}.zip");
            LogPublish($">>> Compressing output files directly to: {zipPath}", false, ColorWait);

            await CreateZipFromDirectoryAsync(tempOut, zipPath);

            Directory.Delete(tempOut, true);
            LogPublish($">>> [OK] Web App package successfully compressed.", false, ColorSuccess);

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

        private async Task<bool> PublishApkHelperAsync(string workspace, AppPipelineConfig app, string revision)
        {
            UpdateVisualStatus(app.AppName, true, "Compiling APK... ⚙️", Color.DeepSkyBlue);
            LogPublish("\n=======================================================", false, ColorPrimary);
            LogPublish($">>> STEP: Publishing Native Android Mobile APK for {app.AppName}", false, ColorPrimary);
            LogPublish("=======================================================", false, ColorPrimary);

            string mobileProjectPath = Path.Combine("apps", "GFC-Pos-Standalone", "GFC.Pos.Mobile", "GFC.Pos.Mobile.csproj");
            string mobileTempOut = Path.Combine(workspace, "publish_pos_mobile_temp");

            if (Directory.Exists(mobileTempOut)) Directory.Delete(mobileTempOut, true);

            // Bulletproof cache clearing to resolve shadowing/stale asset issues
            LogPublish(">>> Cleaning up intermediate build caches (bin/obj) to prevent asset shadowing...", false, ColorWait);
            try
            {
                string mobileDir = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Mobile");
                string uiDir = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.UI");
                
                foreach (var dir in new[] { mobileDir, uiDir })
                {
                    if (Directory.Exists(dir))
                    {
                        var binPath = Path.Combine(dir, "bin");
                        var objPath = Path.Combine(dir, "obj");
                        if (Directory.Exists(binPath)) Directory.Delete(binPath, true);
                        if (Directory.Exists(objPath)) Directory.Delete(objPath, true);
                        LogPublish($">>> Cleaned: {Path.GetFileName(dir)} bin/obj", false, ColorSuccess);
                    }
                }
            }
            catch (Exception ex)
            {
                LogPublish($">>> [Warning] Failed to clear intermediate build directories: {ex.Message}", false, ColorWarning);
            }

            LogPublish(">>> Executing dotnet publish for Native Android project...", false, ColorWait);
            var mobileRunner = new ProcessRunner((line, err) => LogPublish(line, err));
            int mobileExit = await mobileRunner.RunAsync("dotnet", $"publish \"{mobileProjectPath}\" -f net10.0-android -c Release -o \"{mobileTempOut}\" /p:TreatWarningsAsErrors=false", workspace);

            bool success = false;
            if (mobileExit == 0)
            {
                // Inspect and print LastWriteTime of intermediate assets directly to log panel
                string intermediateAssets = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Mobile", "obj", "Release", "net10.0-android", "assets", "wwwroot");
                if (Directory.Exists(intermediateAssets))
                {
                    LogPublish("\n>>> [GFC ASSETS INSPECTION] Listing intermediate assets build times:", false, ColorSuccess);
                    foreach (var file in Directory.GetFiles(intermediateAssets, "*.*", SearchOption.AllDirectories))
                    {
                        var relativePath = file.Substring(intermediateAssets.Length).TrimStart('\\', '/');
                        var writeTime = File.GetLastWriteTime(file);
                        LogPublish($"  - {relativePath} | LastWriteTime: {writeTime:yyyy-MM-dd HH:mm:ss}", false, ColorTextMuted);
                    }
                    LogPublish(">>> [GFC ASSETS INSPECTION] Inspection complete.\n", false, ColorSuccess);
                }
                else
                {
                    LogPublish($"\n!!! Warning: Intermediate assets folder not found at: {intermediateAssets}\n", true);
                }

                LogPublish(">>> Locating generated APK installer file...", false, ColorWait);
                string foundApk = null;
                if (Directory.Exists(mobileTempOut))
                {
                    var apks = Directory.GetFiles(mobileTempOut, "*.apk");
                    if (apks.Length > 0) foundApk = apks[0];
                }
                
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
                    if (!Directory.Exists(app.SourceZipFolder))
                    {
                        Directory.CreateDirectory(app.SourceZipFolder);
                    }
                    string destApk = Path.Combine(app.SourceZipFolder, $"{app.AppName}_{revision}.apk");
                    LogPublish($">>> Copying APK to output directory: {destApk}", false, ColorWait);
                    File.Copy(foundApk, destApk, true);
                    LogPublish(">>> [OK] Native Android Mobile build and package completed successfully.", false, ColorSuccess);
                    success = true;
                }
                else
                {
                    LogPublish("!!! Failed to locate the generated .apk installer file in the output directories.", true);
                }
            }

            if (mobileExit == 0 && success)
            {
                UpdateVisualStatus(app.AppName, true, "Success ✅", Color.Green);
            }
            else
            {
                UpdateVisualStatus(app.AppName, true, "Failed ❌", Color.Red);
            }

            if (Directory.Exists(mobileTempOut)) Directory.Delete(mobileTempOut, true);
            return success;
        }

        private void UpdateVisualStatus(string appName, bool isApk, string status, Color color)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                this.Invoke(new Action(() =>
                {
                    Label targetLabel = null;
                    if (appName.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLabel = isApk ? lblStatusMobileApk : lblStatusMobilePublish;
                    }
                    else if (appName.Equals("POS", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLabel = isApk ? lblStatusPosApk : lblStatusPosPublish;
                    }
                    else if (appName.Equals("WebApp", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLabel = isApk ? lblStatusWebAppApk : lblStatusWebAppPublish;
                    }

                    if (targetLabel != null)
                    {
                        targetLabel.Text = status;
                        targetLabel.ForeColor = color;
                        targetLabel.Refresh();
                    }
                }));
            }
            catch
            {
                // ignore thread/handle exceptions on close
            }
        }

        private void UpdateDeployVisualStatus(string appName, string status, Color color)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                this.Invoke(new Action(() =>
                {
                    Label targetLabel = null;
                    if (appName.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLabel = lblStatusDeployMobileState;
                    }
                    else if (appName.Equals("POS", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLabel = lblStatusDeployPosState;
                    }
                    else if (appName.Equals("WebApp", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLabel = lblStatusDeployWebAppState;
                    }

                    if (targetLabel != null)
                    {
                        targetLabel.Text = status;
                        targetLabel.ForeColor = color;
                        targetLabel.Refresh();
                    }
                }));
            }
            catch
            {
                // ignore thread/handle exceptions on close
            }
        }
        #endregion

        #region Core Deployment Operations (Recursive Copy & IIS Control)
        private async Task<bool> DeploySingleAppGenericAsync(AppPipelineConfig app, string workspace)
        {
            Log($"\n=======================================================", false, ColorPrimary);
            Log($">>> STARTING DEPLOYMENT PIPELINE FOR: {app.AppName}", false, ColorPrimary);
            Log($"=======================================================", false, ColorPrimary);

            // Locate zip file dynamically: Scan Source ZIP Folder for files matching pattern AppName_v*.zip or AppName_*.zip
            string zipPath = string.Empty;
            string zipName = string.Empty;

            if (Directory.Exists(app.SourceZipFolder))
            {
                var matchedFiles = new List<string>(Directory.GetFiles(app.SourceZipFolder, $"{app.AppName}_v*.zip"));
                if (matchedFiles.Count == 0)
                {
                    matchedFiles.AddRange(Directory.GetFiles(app.SourceZipFolder, $"{app.AppName}_*.zip"));
                }

                if (matchedFiles.Count > 0)
                {
                    matchedFiles.Sort((a, b) => File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)));
                    zipPath = matchedFiles[0];
                    zipName = Path.GetFileName(zipPath);
                    Log($">>> Dynamically resolved latest package: {zipName}", false, ColorSuccess);
                }
            }

            if (string.IsNullOrEmpty(zipPath) || !File.Exists(zipPath))
            {
                if (app.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase) && _config.PosDeployMobileApk)
                {
                    Log(">>> No ZIP package found, but APK deployment is active. Attempting APK-Only deployment...", false, ColorWarning);
                    
                    string foundApkPath = null;
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    
                    string[] searchApkFiles = new string[]
                    {
                        Path.Combine(app.SourceZipFolder, "GFC_POS_Mobile.apk"),
                        Path.Combine(desktop, "GFC_POS_Mobile.apk"),
                        Path.Combine(app.SourceZipFolder, "com.gfc.pos.mobile-Signed.apk"),
                        Path.Combine(desktop, "com.gfc.pos.mobile-Signed.apk")
                    };

                    foreach (var path in searchApkFiles)
                    {
                        if (File.Exists(path))
                        {
                            foundApkPath = path;
                            break;
                        }
                    }

                    if (foundApkPath == null)
                    {
                        if (Directory.Exists(app.SourceZipFolder))
                        {
                            var apks = Directory.GetFiles(app.SourceZipFolder, "*.apk");
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
                        Log($">>> Located APK installer for APK-Only deployment: {foundApkPath}", false, ColorWait);
                        if (!Directory.Exists(_config.PosApkDistFolder))
                        {
                            Directory.CreateDirectory(_config.PosApkDistFolder);
                        }
                        string destApk = Path.Combine(_config.PosApkDistFolder, "GFC_POS_Mobile.apk");
                        Log($">>> Copying APK to distribution path: {destApk}", false, ColorWait);
                        File.Copy(foundApkPath, destApk, true);
                        Log($">>> [OK] APK successfully copied to server (APK-Only).", false, ColorSuccess);

                        try
                        {
                            File.Delete(foundApkPath);
                            Log($">>> Cleanup: Source APK deleted.", false, ColorTextMuted);
                        }
                        catch
                        {
                            // ignore
                        }

                        Log($">>> SUCCESS: APK-Only deployment completed successfully for POS.", false, ColorSuccess);
                        return true;
                    }
                }

                Log($"!!! Error: No build package found matching pattern '{app.AppName}_v*.zip' in Source ZIP Folder: {app.SourceZipFolder}", true);
                return false;
            }

            // Stop IIS Pool & Site
            Log($">>> Stopping IIS site '{app.IisSiteName}' and app pool '{app.IisAppPool}'...", false, ColorWait);
            await ToggleIisAsync(app.IisSiteName, app.IisAppPool, false);

            try
            {
                // Step A: Archive
                Log($"[Step A] Archiving ZIP package '{zipName}'...", false, ColorWait);
                if (!Directory.Exists(app.ArchiveFolder))
                {
                    Directory.CreateDirectory(app.ArchiveFolder);
                }
                string archiveDest = Path.Combine(app.ArchiveFolder, zipName);
                File.Copy(zipPath, archiveDest, true);
                Log($"[Step A] [OK] Package archived successfully: {archiveDest}", false, ColorSuccess);

                // Step B: Deploy
                Log($"[Step B] Clearing production folder: {app.LiveTargetFolder}...", false, ColorWait);
                if (!Directory.Exists(app.LiveTargetFolder))
                {
                    Directory.CreateDirectory(app.LiveTargetFolder);
                }
                await ClearDirectoryContentsAsync(app.LiveTargetFolder, chkPurgeFiles.Checked);

                Log($"[Step B] Extracting new flat ZIP contents directly to live folder...", false, ColorWait);
                await ExtractZipAsync(zipPath, app.LiveTargetFolder);
                Log($"[Step B] [OK] Extraction completed flat.", false, ColorSuccess);

                // Config web.config
                bool isWasm = app.AppName.Equals("Mobile", StringComparison.OrdinalIgnoreCase) || 
                              app.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase);
                if (isWasm && chkAutoConfigWebConfig.Checked)
                {
                    Log(">>> [Step B] Writing optimized web.config for Blazor WASM SPA SPA...", false, ColorWait);
                    WriteWasmWebConfig(app.LiveTargetFolder, app.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase) && _config.PosDeployMobileApk);
                }

                // Deploy APK if active
                if (app.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase) && _config.PosDeployMobileApk)
                {
                    Log(">>> [Step B] Deploying dual-track Native Mobile APK...", false, ColorWait);
                    await DeployPosApkHelperAsync(app.SourceZipFolder);
                }

                // Start IIS
                Log($">>> Starting IIS site '{app.IisSiteName}' and app pool '{app.IisAppPool}'...", false, ColorWait);
                await ToggleIisAsync(app.IisSiteName, app.IisAppPool, true);

                // Step C: Cleanup
                Log($"[Step C] Cleaning up source package from source folder...", false, ColorWait);
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                    Log($"[Step C] [OK] Source package '{zipName}' deleted successfully.", false, ColorSuccess);
                }

                Log($">>> SUCCESS: Deployment completed for {app.AppName}.", false, ColorSuccess);
                return true;
            }
            catch (Exception ex)
            {
                Log($"!!! Deployment of {app.AppName} failed: {ex.Message}", true);
                
                // Fallback restore
                Log(">>> Attempting to restart IIS on fallback...", false, ColorWarning);
                await ToggleIisAsync(app.IisSiteName, app.IisAppPool, true);
                return false;
            }
        }

        private async Task DeployPosApkHelperAsync(string sourceZipFolder)
        {
            string foundApkPath = null;
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            string[] searchFiles = new string[]
            {
                Path.Combine(sourceZipFolder, "GFC_POS_Mobile.apk"),
                Path.Combine(desktop, "GFC_POS_Mobile.apk"),
                Path.Combine(sourceZipFolder, "com.gfc.pos.mobile-Signed.apk"),
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

            if (foundApkPath == null)
            {
                if (Directory.Exists(sourceZipFolder))
                {
                    var apks = Directory.GetFiles(sourceZipFolder, "*.apk");
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
                Log($">>> Located APK installer at: {foundApkPath}", false, ColorWait);
                if (!Directory.Exists(_config.PosApkDistFolder))
                {
                    Directory.CreateDirectory(_config.PosApkDistFolder);
                }
                string destApk = Path.Combine(_config.PosApkDistFolder, "GFC_POS_Mobile.apk");
                Log($">>> Copying APK to distribution path: {destApk}", false, ColorWait);
                File.Copy(foundApkPath, destApk, true);
                Log($">>> [OK] APK successfully copied to server.", false, ColorSuccess);

                try
                {
                    File.Delete(foundApkPath);
                    Log($">>> Cleanup: Source APK deleted.", false, ColorTextMuted);
                }
                catch
                {
                    // ignore
                }
            }
            else
            {
                Log("!!! Warning: Dual-track APK deployment was selected, but no .apk installer file was found in search paths.", true);
            }
        }

        private async Task ClearDirectoryContentsAsync(string path, bool preserveExclusions = true)
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(path)) return;

                var exclusions = new string[] { "appsettings.Production.json", "web.config" };
                
                // Delete files
                foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                {
                    if (preserveExclusions)
                    {
                        bool isExcluded = false;
                        foreach (var exc in exclusions)
                        {
                            if (Path.GetFileName(file).Equals(exc, StringComparison.OrdinalIgnoreCase))
                            {
                                isExcluded = true;
                                break;
                            }
                        }
                        if (isExcluded) continue;
                    }

                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // ignore locked files
                    }
                }

                // Delete empty directories
                DeleteEmptySubdirectories(path);
            });
        }

        private void DeleteEmptySubdirectories(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteEmptySubdirectories(directory);
                try
                {
                    if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                    {
                        Directory.Delete(directory, false);
                    }
                }
                catch
                {
                    // ignore
                }
            }
        }

        private async void BtnRunDeploy_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            var checkedApps = new List<AppPipelineConfig>();
            foreach (var item in clbDeployApps.CheckedItems)
            {
                var app = _config.AppPipelines.Find(a => a.AppName == item.ToString());
                if (app != null) checkedApps.Add(app);
            }

            if (checkedApps.Count == 0)
            {
                MessageBox.Show("Please check at least one application to deploy.", "No App Checked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetBusy(true, "Deploying packages...");
            rtbTerminal.Clear();
            Log($">>> STARTING BATCH DEPLOYMENT FOR {checkedApps.Count} TARGET(S)...", false, ColorWait);

            // Initialize progress status indicators
            foreach (var appConfig in _config.AppPipelines)
            {
                bool isSelected = checkedApps.Exists(a => a.AppName == appConfig.AppName);
                if (isSelected)
                {
                    UpdateDeployVisualStatus(appConfig.AppName, "Queued ⏳", Color.Orange);
                }
                else
                {
                    UpdateDeployVisualStatus(appConfig.AppName, "Skipped ➔", Color.LightGray);
                }
            }

            var successes = new List<string>();
            var failures = new Dictionary<string, string>();

            string workspace = txtPubWorkspace.Text.Trim();

            foreach (var app in checkedApps)
            {
                UpdateDeployVisualStatus(app.AppName, "Deploying... ⚙️", Color.DeepSkyBlue);
                try
                {
                    bool appSuccess = await DeploySingleAppGenericAsync(app, workspace);

                    if (appSuccess)
                    {
                        successes.Add(app.AppName);
                        Log($"[OK] Deployment SUCCEEDED for: {app.AppName}", false, ColorSuccess);
                        UpdateDeployVisualStatus(app.AppName, "Success ✅", Color.Green);
                    }
                    else
                    {
                        failures.Add(app.AppName, "Pipeline step failed. Check terminal logs above.");
                        Log($"[FAIL] Deployment FAILED for: {app.AppName}", true);
                        UpdateDeployVisualStatus(app.AppName, "Failed ❌", Color.Red);
                    }
                }
                catch (Exception ex)
                {
                    failures.Add(app.AppName, ex.Message);
                    Log($"[EXCEPTION] Deployment failed for {app.AppName}: {ex.Message}", true);
                    UpdateDeployVisualStatus(app.AppName, "Failed ❌", Color.Red);
                }
            }

            SetBusy(false, failures.Count == 0 ? "Ready" : "Error: Deploy Failed!");

            string summaryMsg = $"Batch Deployment Operation Completed.\n\n" +
                                $"Successful ({successes.Count}):\n" +
                                (successes.Count > 0 ? string.Join("\n", successes.ConvertAll(s => $" - {s}")) : " None") + "\n\n" +
                                $"Failed ({failures.Count}):\n" +
                                (failures.Count > 0 ? string.Join("\n", new List<string>(failures.Keys).ConvertAll(k => $" - {k}: {failures[k]}")) : " None");

            MessageBox.Show(summaryMsg, "Deployment Batch Summary", MessageBoxButtons.OK, 
                            failures.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
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
