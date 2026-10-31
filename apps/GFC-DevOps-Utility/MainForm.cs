using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Security.Principal;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.Administration;

namespace GFCDevOpsUtility
{
    public partial class MainForm : Form
    {
        private AppConfig _config = null!;
        private DataTable _dtRevisions = null!;
        private bool _isBusy = false;
        private bool _isUpdatingUi = false;
        private bool _isPublishAborted = false;
        private ProcessRunner? _activePublishRunner = null;

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

            // DevOps Execution Terminal
            rtbTerminal.BackColor = Color.FromArgb(15, 23, 42);
            rtbTerminal.ForeColor = Color.FromArgb(248, 250, 252);
            btnClearLogs.ForeColor = Color.FromArgb(226, 232, 240);
            btnClearLogs.BackColor = Color.FromArgb(30, 41, 59);
            btnCopyLog.ForeColor = Color.FromArgb(226, 232, 240);
            btnCopyLog.BackColor = Color.FromArgb(30, 41, 59);
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
            this.Text = DevOpsVersion.DisplayTitle;
            _config = AppConfig.Load();

            // Populate form text boxes from config
            txtPubWorkspace.Text = _config.WorkspacePath;
            txtPubOutput.Text = _config.PublishOutputPath;
            txtDepZip.Text = _config.ZipInputPath;
            
            chkAutoConfigWebConfig.Checked = _config.AutoConfigureWebConfig;
            chkPurgeFiles.Checked = _config.PurgeFiles;

            _isUpdatingUi = true;

            // Force all pipeline APK flags to default enabled if required
            foreach (var app in _config.AppPipelines)
            {
                app.PublishMobileApk = false;
            }
            _config.PosPublishMobileApk = false;
            _config.PosDeployMobileApk = true;
            _config.Save();

            _isUpdatingUi = false;

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

        private void MainForm_Shown(object sender, EventArgs e)
        {
            ResetAllSelections();
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
            _isUpdatingUi = true;
            try
            {
                if (clbPublishApps.SelectedIndex != -1)
                {
                    clbPublishApps.SelectedIndex = -1;
                }
                lblAppDesc.Text = "Click/hover an item to see its purpose.";

                if (clbDeployApps.SelectedIndex != -1)
                {
                    clbDeployApps.SelectedIndex = -1;
                }

                radDeployLatest.Checked = true;
                radDeployRollback.Checked = false;
                cmbDeployArchiveVersions.Enabled = false;
                cmbDeployArchiveVersions.Items.Clear();

                if (cmbRevAppSelect.SelectedIndex != -1)
                {
                    cmbRevAppSelect.SelectedIndex = -1;
                }

                dgvRevisions.ClearSelection();
                dgvRevisions.CurrentCell = null;
            }
            finally
            {
                _isUpdatingUi = false;
            }
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

            btnRunPublish.Visible = (panelToShow == pnlPublish);
            btnRunDeploy.Visible = (panelToShow == pnlDeploy);
            btnRunRevisionSync.Visible = (panelToShow == pnlRevisions);
        }

        private void SetActiveTabButton(Button activeBtn)
        {
            btnTabPublish.ForeColor = (activeBtn == btnTabPublish) ? ColorPrimary : ColorTextMuted;
            btnTabDeploy.ForeColor = (activeBtn == btnTabDeploy) ? ColorPrimary : ColorTextMuted;
            btnTabRevisions.ForeColor = (activeBtn == btnTabRevisions) ? ColorPrimary : ColorTextMuted;
        }
        #endregion

        #region Unified Dark Execution Console
        private readonly List<LogLine> _fullLogHistory = new List<LogLine>();

        public class LogLine
        {
            public string Message { get; set; } = "";
            public bool IsError { get; set; }
            public Color? CustomColor { get; set; }
        }

        private bool ShouldFilterLine(string message, bool isError)
        {
            bool showErrorsOnly = chkShowErrors.Checked;
            bool showWarningsOnly = chkShowWarnings.Checked;

            if (!showErrorsOnly && !showWarningsOnly)
            {
                return false;
            }

            string lower = message.ToLowerInvariant();
            bool isErrLine = isError || lower.Contains("error") || lower.Contains("[fail]") || lower.Contains("failed");
            bool isWarnLine = lower.Contains("warning") || lower.Contains("[warn]");

            if (showErrorsOnly && showWarningsOnly)
            {
                return !(isErrLine || isWarnLine);
            }
            if (showErrorsOnly)
            {
                return !isErrLine;
            }
            if (showWarningsOnly)
            {
                return !isWarnLine;
            }

            return false;
        }

        private void ChkShowFilters_CheckedChanged(object sender, EventArgs e)
        {
            rtbTerminal.Clear();
            foreach (var line in _fullLogHistory)
            {
                if (!ShouldFilterLine(line.Message, line.IsError))
                {
                    AppendToTerminal(line.Message, line.IsError, line.CustomColor);
                }
            }
        }

        private void Log(string message, bool isError = false, Color? customColor = null)
        {
            if (rtbTerminal.InvokeRequired)
            {
                rtbTerminal.Invoke(new Action(() => Log(message, isError, customColor)));
                return;
            }

            _fullLogHistory.Add(new LogLine { Message = message, IsError = isError, CustomColor = customColor });

            if (ShouldFilterLine(message, isError))
            {
                return;
            }

            AppendToTerminal(message, isError, customColor);
        }

        private void LogPublish(string message, bool isError = false, Color? customColor = null)
        {
            Log(message, isError, customColor);
        }

        private void AppendToTerminal(string message, bool isError = false, Color? customColor = null)
        {
            Color logColor = Color.FromArgb(248, 250, 252); // Default Cascadia text (Slate 50)

            if (isError)
            {
                logColor = Color.FromArgb(248, 113, 113); // High-contrast Red / Coral for errors
            }
            else if (customColor.HasValue)
            {
                logColor = customColor.Value;
            }
            else
            {
                string lowerMsg = message.ToLowerInvariant();
                if (lowerMsg.Contains("error") || lowerMsg.Contains("[fail]") || lowerMsg.Contains("critical"))
                {
                    logColor = Color.FromArgb(248, 113, 113); // Coral Red
                }
                else if (lowerMsg.Contains("warning") || lowerMsg.Contains("[warn]"))
                {
                    logColor = Color.FromArgb(251, 191, 36); // Amber / Warm Yellow
                }
                else if (lowerMsg.Contains("[ok]") || lowerMsg.Contains("success") || lowerMsg.Contains("successful") || lowerMsg.Contains("completed successfully"))
                {
                    logColor = Color.FromArgb(52, 211, 153); // Emerald Green
                }
                else if (lowerMsg.Contains("[step") || lowerMsg.Contains(">>>") || lowerMsg.Contains("[wait]") || lowerMsg.Contains("starting"))
                {
                    logColor = Color.FromArgb(96, 165, 250); // Sky Blue
                }
                else if (lowerMsg.Contains("[dry]") || lowerMsg.Contains("would "))
                {
                    logColor = Color.FromArgb(251, 146, 60); // Orange
                }
            }

            rtbTerminal.SelectionStart = rtbTerminal.TextLength;
            rtbTerminal.SelectionLength = 0;
            rtbTerminal.SelectionColor = logColor;

            bool isBold = isError || message.Contains("CRITICAL") || message.Contains("FAILED") || message.Contains("!!!") || message.Contains("SUCCESS:") || message.Contains("==================");
            rtbTerminal.SelectionFont = new Font(rtbTerminal.Font.FontFamily, rtbTerminal.Font.Size, isBold ? FontStyle.Bold : FontStyle.Regular);

            rtbTerminal.AppendText(message + Environment.NewLine);
            rtbTerminal.SelectionColor = rtbTerminal.ForeColor;
            rtbTerminal.ScrollToCaret();
        }

        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            rtbTerminal.Clear();
            _fullLogHistory.Clear();
        }

        private void BtnCopyLog_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rtbTerminal.Text))
            {
                Clipboard.SetText(rtbTerminal.Text);
                MessageBox.Show("Console logs successfully copied to clipboard!", "Logs Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No console logs to copy.", "Empty Logs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            _dtRevisions.Rows.Add("📲 Connect App", "Loading...", "---");

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
                string conVer = VersionScanner.GetConnectVersion(workspace);

                this.Invoke(new Action(() =>
                {
                    _dtRevisions.Rows[0]["Current Version"] = mobVer;
                    _dtRevisions.Rows[1]["Current Version"] = posVer;
                    _dtRevisions.Rows[2]["Current Version"] = webVer;
                    _dtRevisions.Rows[3]["Current Version"] = conVer;

                    UpdateProposedVersions();

                    if (cmbRevAppSelect.SelectedIndex == -1)
                    {
                        dgvRevisions.ClearSelection();
                        dgvRevisions.CurrentCell = null;
                    }
                }));
            });
        }

        private void UpdateProposedVersions()
        {
            if (_dtRevisions == null) return;

            for (int i = 0; i < 4; i++)
            {
                string current = _dtRevisions.Rows[i]["Current Version"].ToString() ?? "Unknown";
                string proposed = "---";

                string selectedTarget = cmbRevAppSelect.SelectedItem?.ToString() ?? "";
                string rowTarget = i == 0 ? "Mobile" : (i == 1 ? "POS" : (i == 2 ? "WebApp" : "Connect"));

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

        private void DgvRevisions_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvRevisions.Rows.Count)
            {
                string? targetApp = e.RowIndex switch
                {
                    0 => "Mobile",
                    1 => "POS",
                    2 => "WebApp",
                    3 => "Connect",
                    _ => null
                };

                if (targetApp != null && cmbRevAppSelect.SelectedItem?.ToString() != targetApp)
                {
                    cmbRevAppSelect.SelectedItem = targetApp;
                }
            }
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

                PopulateArchiveVersions(appConfig);

                bool isApkApp = appConfig.AppName.Equals("Connect", StringComparison.OrdinalIgnoreCase) ||
                                appConfig.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase);

                if (isApkApp)
                {
                    radDeployLatest.Checked = true;
                    radDeployRollback.Checked = false;
                    radDeployRollback.Enabled = false;
                    radDeployRollback.Text = "Rollback Not Supported (APK / Native Android)";
                    cmbDeployArchiveVersions.Enabled = false;
                }
                else
                {
                    radDeployRollback.Enabled = true;
                    radDeployRollback.Text = "Rollback Previous Release:";
                    cmbDeployArchiveVersions.Enabled = radDeployRollback.Checked;
                }

                _isUpdatingUi = false;
            }
        }

        private void RadDeployMode_CheckedChanged(object sender, EventArgs e)
        {
            var selectedAppName = clbDeployApps.SelectedItem?.ToString();
            bool isApkApp = "Connect".Equals(selectedAppName, StringComparison.OrdinalIgnoreCase) ||
                            "POS".Equals(selectedAppName, StringComparison.OrdinalIgnoreCase);

            if (isApkApp)
            {
                radDeployRollback.Enabled = false;
                cmbDeployArchiveVersions.Enabled = false;
                if (radDeployRollback.Checked)
                {
                    radDeployLatest.Checked = true;
                }
            }
            else
            {
                cmbDeployArchiveVersions.Enabled = radDeployRollback.Checked;
            }
        }

        private void PopulateArchiveVersions(AppPipelineConfig appConfig)
        {
            cmbDeployArchiveVersions.Items.Clear();

            if (appConfig == null || string.IsNullOrWhiteSpace(appConfig.ArchiveFolder) || !Directory.Exists(appConfig.ArchiveFolder))
            {
                return;
            }

            // Exclude APK apps from rollback archive selection
            if (appConfig.AppName.Equals("Connect", StringComparison.OrdinalIgnoreCase) ||
                appConfig.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                var files = new List<string>(Directory.GetFiles(appConfig.ArchiveFolder, $"{appConfig.AppName}_v*.zip"));
                if (files.Count == 0)
                {
                    files.AddRange(Directory.GetFiles(appConfig.ArchiveFolder, $"{appConfig.AppName}_*.zip"));
                }

                var fileInfos = new List<FileInfo>();
                foreach (var f in files)
                {
                    try
                    {
                        fileInfos.Add(new FileInfo(f));
                    }
                    catch
                    {
                        // ignore unreadable files
                    }
                }

                fileInfos.Sort((a, b) => b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc));

                foreach (var fi in fileInfos)
                {
                    cmbDeployArchiveVersions.Items.Add(fi.Name);
                }

                if (cmbDeployArchiveVersions.Items.Count > 0)
                {
                    cmbDeployArchiveVersions.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Log($"[ARCHIVE] Warning: Unable to inspect archive directory: {ex.Message}", false, ColorWarning);
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
                PopulateArchiveVersions(appConfig);
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

        private void SetBusy(bool busy, string statusText, bool isPublish = false)
        {
            _isBusy = busy;
            
            if (isPublish)
            {
                btnRunPublish.Enabled = true;
                if (busy)
                {
                    btnRunPublish.Text = "🛑 Abort Publish";
                    btnRunPublish.BackColor = Color.FromArgb(220, 38, 38); // Premium Red
                }
                else
                {
                    btnRunPublish.Text = "📤 Start Application Publish";
                    btnRunPublish.BackColor = Color.FromArgb(37, 99, 235); // Royal Blue
                }
            }
            else
            {
                btnRunPublish.Enabled = !busy;
                btnRunPublish.Text = "📤 Start Application Publish";
                btnRunPublish.BackColor = Color.FromArgb(37, 99, 235); // Royal Blue
            }

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
            else if (appName.Equals("Connect", StringComparison.OrdinalIgnoreCase))
            {
                version = VersionScanner.GetConnectVersion(workspace);
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
            if (_isBusy)
            {
                // Trigger Abort sequence
                _isPublishAborted = true;
                _activePublishRunner?.Abort();
                return;
            }

            _isPublishAborted = false;

            if (clbPublishApps.SelectedItem == null)
            {
                MessageBox.Show("Please select an application from the list to publish.", "No App Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedApp = _config.AppPipelines.Find(a => a.AppName == clbPublishApps.SelectedItem.ToString());
            if (selectedApp == null)
            {
                MessageBox.Show("Selected application configuration could not be found.", "App Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            SetBusy(true, $"Publishing {selectedApp.AppName}...", true);
            rtbTerminal.Clear();
            _fullLogHistory.Clear();
            LogPublish($">>> STARTING PUBLISH PIPELINE FOR TARGET: {selectedApp.AppName}...", false, ColorWait);

            // Initialize progress status indicators
            foreach (var appConfig in _config.AppPipelines)
            {
                if (appConfig.AppName == selectedApp.AppName)
                {
                    UpdateVisualStatus(appConfig.AppName, false, "Queued ⏳", Color.Orange);
                    UpdateVisualStatus(appConfig.AppName, true, "Queued ⏳", Color.Orange);
                }
                else
                {
                    UpdateVisualStatus(appConfig.AppName, false, "Skipped ➔", Color.LightGray);
                    UpdateVisualStatus(appConfig.AppName, true, "APK: -", Color.Gray);
                }
            }

            var checkedApps = new List<AppPipelineConfig> { selectedApp };
            var successes = new List<string>();
            var failures = new Dictionary<string, string>();

            foreach (var app in checkedApps)
            {
                if (_isPublishAborted)
                {
                    LogPublish($">>> [ABORT] Skipping publish pipeline for: {app.AppName} due to user abort.", false, ColorWarning);
                    UpdateVisualStatus(app.AppName, false, "Aborted 🛑", Color.Red);
                    if (app.PublishMobileApk)
                    {
                        UpdateVisualStatus(app.AppName, true, "Aborted 🛑", Color.Red);
                    }
                    continue;
                }

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
                        else if (app.AppName.Equals("Connect", StringComparison.OrdinalIgnoreCase))
                        {
                            appSuccess = await PublishConnectAppAsync(workspace, app, revision);
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
                        if (_isPublishAborted)
                        {
                            LogPublish($">>> [ABORT] Publish pipeline aborted for: {app.AppName}", false, ColorWarning);
                            UpdateVisualStatus(app.AppName, false, "Aborted 🛑", Color.Red);
                            if (app.PublishMobileApk)
                            {
                                UpdateVisualStatus(app.AppName, true, "Aborted 🛑", Color.Red);
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
                }
                catch (Exception ex)
                {
                    if (_isPublishAborted)
                    {
                        LogPublish($">>> [ABORT] Publish pipeline aborted for: {app.AppName}", false, ColorWarning);
                        UpdateVisualStatus(app.AppName, false, "Aborted 🛑", Color.Red);
                        if (app.PublishMobileApk)
                        {
                            UpdateVisualStatus(app.AppName, true, "Aborted 🛑", Color.Red);
                        }
                    }
                    else
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
            }

            if (lblAppDesc != null) lblAppDesc.Text = "Click an item to see its purpose.";

            if (_isPublishAborted)
            {
                SetBusy(false, "Publish Aborted!", true);
                MessageBox.Show("Publish operation was aborted by the user.", "Operation Aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SetBusy(false, failures.Count == 0 ? "Ready" : "Error: Build Failed!", true);

                string summaryMsg = $"Batch Publish Operation Completed.\n\n" +
                                    $"Successful ({successes.Count}):\n" +
                                    (successes.Count > 0 ? string.Join("\n", successes.ConvertAll(s => $" - {s}")) : " None") + "\n\n" +
                                    $"Failed ({failures.Count}):\n" +
                                    (failures.Count > 0 ? string.Join("\n", new List<string>(failures.Keys).ConvertAll(k => $" - {k}: {failures[k]}")) : " None");

                MessageBox.Show(summaryMsg, "Publish Batch Summary", MessageBoxButtons.OK, 
                                failures.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
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

            if (_isPublishAborted) return false;
            LogPublish(">>> Executing dotnet publish...", false, ColorWait);
            var runner = new ProcessRunner((line, err) => LogPublish(line, err));
            _activePublishRunner = runner;
            int exit = -1;
            try
            {
                exit = await runner.RunAsync("dotnet", $"publish \"{projectPath}\" -c Release -o \"{tempOut}\" /p:TreatWarningsAsErrors=false", workspace);
            }
            finally
            {
                _activePublishRunner = null;
            }

            if (exit != 0 || _isPublishAborted)
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

            if (_isPublishAborted) return false;
            LogPublish($">>> Triggering version synchronization to version {version}...", false, ColorWait);

            bool syncOk = await VersionSyncEngine.SyncVersionAsync(
                project: "POS",
                workspace: workspace,
                explicitVersion: version,
                useNext: false,
                dryRun: false,
                noSync: false,
                log: (msg, isErr) => LogPublish($"[SYNC] {msg}", isErr)
            );

            if (!syncOk || _isPublishAborted)
            {
                LogPublish("!!! Version synchronization failed or was aborted.", true);
                return false;
            }

            // Execute Native Android Mobile Build (APK)
            LogPublish("\n=======================================================", false, ColorPrimary);
            LogPublish($">>> STEP: Publishing Native Android Mobile APK for POS ({revision})", false, ColorPrimary);
            LogPublish("=======================================================", false, ColorPrimary);

            bool apkBuildSuccess = await PublishApkHelperAsync(workspace, app, revision);
            return apkBuildSuccess;
        }

        private async Task<bool> PublishWebAppAsync(string workspace, AppPipelineConfig app, string revision)
        {
            LogPublish($">>> [WAIT] Starting Web App Publish Pipeline for revision {revision}...", false, ColorWait);

            LogPublish(">>> Stopping GFC.BlazorServer process if running...", false, ColorWait);
            await KillProcessAsync("GFC.BlazorServer");

            string projectPath = Path.Combine("apps", "webapp", "GFC.BlazorServer", "GFC.BlazorServer.csproj");
            string tempOut = Path.Combine(workspace, "publish_webapp_temp");

            if (Directory.Exists(tempOut)) Directory.Delete(tempOut, true);

            if (_isPublishAborted) return false;
            LogPublish(">>> Executing dotnet publish for Blazor Server...", false, ColorWait);
            var runner = new ProcessRunner((line, err) => LogPublish(line, err));
            _activePublishRunner = runner;
            int exit = -1;
            try
            {
                exit = await runner.RunAsync("dotnet", $"publish \"{projectPath}\" -c Release -o \"{tempOut}\" /p:TreatWarningsAsErrors=false", workspace);
            }
            finally
            {
                _activePublishRunner = null;
            }

            if (exit != 0 || _isPublishAborted)
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

        private async Task<bool> PublishConnectAppAsync(string workspace, AppPipelineConfig app, string revision)
        {
            LogPublish($"\n=======================================================", false, ColorPrimary);
            LogPublish($">>> [WAIT] Starting GFC Connect Native Android Build for revision {revision}...", false, ColorWait);
            LogPublish($"=======================================================", false, ColorPrimary);

            string connectDir = Path.Combine(workspace, "apps", "GFC-Connect");
            string gradlew = Path.Combine(connectDir, "gradlew.bat");

            // --- 1. PREREQUISITE DIAGNOSTIC CHECK ---
            LogPublish(">>> Checking compilation prerequisites for GFC Connect...", false, ColorWait);

            bool javaOk = false;
            string javaPath = string.Empty;

            string javaHome = Environment.GetEnvironmentVariable("JAVA_HOME") ?? string.Empty;
            if (!string.IsNullOrEmpty(javaHome) && Directory.Exists(javaHome))
            {
                javaOk = true;
                javaPath = javaHome;
            }
            else
            {
                // Check Android Studio JBR defaults
                string[] studioJbrPaths = new[]
                {
                    @"C:\Program Files\Android\Android Studio\jbr",
                    @"C:\Program Files\Android\Android Studio\jre",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Android Studio\jbr")
                };
                foreach (var p in studioJbrPaths)
                {
                    if (Directory.Exists(p))
                    {
                        javaOk = true;
                        javaPath = p;
                        break;
                    }
                }
            }

            bool sdkOk = false;
            string sdkPath = string.Empty;
            string androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME") ?? Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT") ?? string.Empty;
            if (!string.IsNullOrEmpty(androidHome) && Directory.Exists(androidHome))
            {
                sdkOk = true;
                sdkPath = androidHome;
            }
            else
            {
                string localProps = Path.Combine(connectDir, "local.properties");
                if (File.Exists(localProps))
                {
                    var lines = File.ReadAllLines(localProps);
                    foreach (var line in lines)
                    {
                        if (line.Trim().StartsWith("sdk.dir", StringComparison.OrdinalIgnoreCase))
                        {
                            var sdkLineParts = line.Split('=');
                            if (sdkLineParts.Length > 1)
                            {
                                string candidate = sdkLineParts[1].Trim().Replace("\\\\", "\\").Replace("\\:", ":");
                                if (Directory.Exists(candidate))
                                {
                                    sdkOk = true;
                                    sdkPath = candidate;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!sdkOk)
                {
                    string defaultLocalSdk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Android\Sdk");
                    if (Directory.Exists(defaultLocalSdk))
                    {
                        sdkOk = true;
                        sdkPath = defaultLocalSdk;
                    }
                }
            }

            if (!javaOk || !sdkOk)
            {
                LogPublish("======================================================================", true);
                LogPublish("[PREREQUISITES REQUIRED TO BUILD GFC CONNECT ON THIS COMPUTER]", true);
                LogPublish("======================================================================", true);
                LogPublish($"1. Java JDK 17+:  {(javaOk ? $"[FOUND: {javaPath}]" : "[MISSING] -> Install Android Studio or OpenJDK 17.")}", !javaOk);
                LogPublish($"2. Android SDK:   {(sdkOk ? $"[FOUND: {sdkPath}]" : "[MISSING] -> Install Android Studio or Android SDK Command-line Tools.")}", !sdkOk);
                LogPublish("======================================================================", true);
                LogPublish(">>> GFC Connect build cannot proceed on this computer without the Android build toolchain.", true);
                return false;
            }

            LogPublish($"[OK] Java toolchain: {javaPath}", false, ColorSuccess);
            LogPublish($"[OK] Android SDK:    {sdkPath}", false, ColorSuccess);

            if (!File.Exists(gradlew))
            {
                LogPublish($"!!! Missing Gradle wrapper script at: {gradlew}", true);
                return false;
            }

            if (_isPublishAborted) return false;

            // --- 2. EXECUTE GRADLE ASSEMBLE RELEASE ---
            LogPublish(">>> Executing gradlew.bat assembleRelease...", false, ColorWait);
            var runner = new ProcessRunner((line, err) => LogPublish(line, err));
            _activePublishRunner = runner;
            int exit = -1;

            try
            {
                var envVars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["JAVA_HOME"] = javaPath,
                    ["ANDROID_HOME"] = sdkPath,
                    ["ANDROID_SDK_ROOT"] = sdkPath
                };

                string existingPath = Environment.GetEnvironmentVariable("PATH") ?? "";
                string javaBin = Path.Combine(javaPath, "bin");
                if (!existingPath.Contains(javaBin, StringComparison.OrdinalIgnoreCase))
                {
                    envVars["PATH"] = $"{javaBin};{existingPath}";
                }

                exit = await runner.RunAsync("cmd.exe", $"/c \"{gradlew}\" assembleRelease -x lint -x test --no-daemon", connectDir, envVars);
            }
            finally
            {
                _activePublishRunner = null;
            }

            if (exit != 0 || _isPublishAborted)
            {
                LogPublish($"!!! Gradle assembleRelease failed with exit code {exit}", true);
                return false;
            }

            // --- 3. LOCATE OUTPUT APK ---
            string expectedApk = Path.Combine(connectDir, "app", "build", "outputs", "apk", "release", "app-release.apk");
            if (!File.Exists(expectedApk))
            {
                // Fallback scan in release outputs
                string releaseDir = Path.Combine(connectDir, "app", "release");
                if (Directory.Exists(releaseDir))
                {
                    var found = Directory.GetFiles(releaseDir, "*.apk");
                    if (found.Length > 0) expectedApk = found[0];
                }
            }

            if (!File.Exists(expectedApk))
            {
                LogPublish($"!!! Gradle succeeded but output APK was not found at: {expectedApk}", true);
                return false;
            }

            // --- 4. STAGE gfc-connect.apk AND version.json ---
            if (!Directory.Exists(app.SourceZipFolder))
            {
                Directory.CreateDirectory(app.SourceZipFolder);
            }

            string targetApk = Path.Combine(app.SourceZipFolder, "gfc-connect.apk");
            File.Copy(expectedApk, targetApk, true);
            LogPublish($">>> [OK] Staged production APK: {targetApk}", false, ColorSuccess);

            // Generate version.json in output directory
            string targetVersionJson = Path.Combine(app.SourceZipFolder, "version.json");
            string cleanRevision = revision.TrimStart('v', 'V');
            int computedCode = 100;
            var parts = cleanRevision.Split('.');
            if (parts.Length >= 2 && int.TryParse(parts[0], out int major) && int.TryParse(parts[1], out int minor))
            {
                int patch = 0;
                if (parts.Length >= 3) int.TryParse(parts[2], out patch);
                computedCode = (major * 100) + (minor * 10) + patch;
            }

            var verObj = new
            {
                latestVersionCode = computedCode,
                latestVersionName = cleanRevision,
                downloadUrl = "/api/app/download/latest.apk",
                mandatoryUpdate = false,
                releaseNotes = $"GFC Connect Release v{cleanRevision}",
                releasedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
            File.WriteAllText(targetVersionJson, JsonSerializer.Serialize(verObj, new JsonSerializerOptions { WriteIndented = true }));
            LogPublish($">>> [OK] Generated release metadata: {targetVersionJson}", false, ColorSuccess);

            // Create a bundled zip as well so it conforms to the standard package format
            string zipPath = Path.Combine(app.SourceZipFolder, $"{app.AppName}_{revision}.zip");
            string tempPackDir = Path.Combine(workspace, "publish_connect_temp");
            if (Directory.Exists(tempPackDir)) Directory.Delete(tempPackDir, true);
            Directory.CreateDirectory(tempPackDir);

            File.Copy(targetApk, Path.Combine(tempPackDir, "gfc-connect.apk"), true);
            File.Copy(targetVersionJson, Path.Combine(tempPackDir, "version.json"), true);

            await CreateZipFromDirectoryAsync(tempPackDir, zipPath);
            Directory.Delete(tempPackDir, true);

            LogPublish($">>> [OK] Connect App package successfully created: {zipPath}", false, ColorSuccess);
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

            if (_isPublishAborted) return false;
            LogPublish(">>> Executing dotnet publish for Native Android project...", false, ColorWait);
            var mobileRunner = new ProcessRunner((line, err) => LogPublish(line, err));
            _activePublishRunner = mobileRunner;
            int mobileExit = -1;
            try
            {
                mobileExit = await mobileRunner.RunAsync("dotnet", $"publish \"{mobileProjectPath}\" -f net10.0-android -c Release -o \"{mobileTempOut}\" /p:TreatWarningsAsErrors=false", workspace);
            }
            finally
            {
                _activePublishRunner = null;
            }

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
                    var apks = Directory.GetFiles(mobileTempOut, "*Signed.apk");
                    if (apks.Length > 0) foundApk = apks[0];
                }
                
                if (foundApk == null)
                {
                    string fallbackDir = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Mobile", "bin", "Release", "net10.0-android");
                    if (Directory.Exists(fallbackDir))
                    {
                        var apks = Directory.GetFiles(fallbackDir, "*Signed.apk");
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

            if (mobileExit == 0 && success && !_isPublishAborted)
            {
                UpdateVisualStatus(app.AppName, true, "Success ✅", Color.Green);
            }
            else
            {
                if (_isPublishAborted)
                {
                    UpdateVisualStatus(app.AppName, true, "Aborted 🛑", Color.Red);
                }
                else
                {
                    UpdateVisualStatus(app.AppName, true, "Failed ❌", Color.Red);
                }
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
                    else if (appName.Equals("Connect", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLabel = lblStatusDeployConnectState;
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

            // Locate package file dynamically:
            string zipPath = string.Empty;
            string zipName = string.Empty;
            bool isRollback = radDeployRollback.Checked;

            if (isRollback)
            {
                if (cmbDeployArchiveVersions.SelectedItem == null)
                {
                    Log($"!!! Error: Rollback mode is active, but no archived version package is selected.", true);
                    return false;
                }

                string selectedArchiveFile = cmbDeployArchiveVersions.SelectedItem.ToString() ?? "";
                zipPath = Path.Combine(app.ArchiveFolder, selectedArchiveFile);
                zipName = selectedArchiveFile;

                if (!File.Exists(zipPath))
                {
                    Log($"!!! Error: Selected archive package does not exist at path: {zipPath}", true);
                    return false;
                }

                Log($">>> [ROLLBACK] Target archived package selected: {zipName}", false, ColorSuccess);
            }
            else
            {
                // Scan Source ZIP Folder for zip or apk
                if (Directory.Exists(app.SourceZipFolder))
                {
                    var matchedFiles = new List<string>(Directory.GetFiles(app.SourceZipFolder, $"{app.AppName}_v*.zip"));
                    if (matchedFiles.Count == 0)
                    {
                        matchedFiles.AddRange(Directory.GetFiles(app.SourceZipFolder, $"{app.AppName}_*.zip"));
                    }

                    // If Connect and no zip found, check for raw gfc-connect.apk
                    if (matchedFiles.Count == 0 && app.AppName.Equals("Connect", StringComparison.OrdinalIgnoreCase))
                    {
                        string rawApk = Path.Combine(app.SourceZipFolder, "gfc-connect.apk");
                        if (File.Exists(rawApk))
                        {
                            matchedFiles.Add(rawApk);
                        }
                    }

                    if (matchedFiles.Count > 0)
                    {
                        matchedFiles.Sort((a, b) => File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)));
                        zipPath = matchedFiles[0];
                        zipName = Path.GetFileName(zipPath);
                        Log($">>> Dynamically resolved latest package: {zipName}", false, ColorSuccess);
                    }
                }
            }

            if (string.IsNullOrEmpty(zipPath) || !File.Exists(zipPath))
            {
                if (!isRollback && app.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase) && _config.PosDeployMobileApk)
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
                            if (apks.Length > 0)
                            {
                                Array.Sort(apks, (a, b) => File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)));
                                foundApkPath = apks[0];
                            }
                        }
                        if (foundApkPath == null && Directory.Exists(desktop))
                        {
                            var apks = Directory.GetFiles(desktop, "*.apk");
                            if (apks.Length > 0)
                            {
                                Array.Sort(apks, (a, b) => File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)));
                                foundApkPath = apks[0];
                            }
                        }
                    }

                    if (foundApkPath != null)
                    {
                        Log($">>> Located APK installer for APK-Only deployment: {foundApkPath}", false, ColorWait);
                        
                        // Extract revision if present in filename: e.g. POS_2.43.196.apk
                        string apkFilename = Path.GetFileNameWithoutExtension(foundApkPath);
                        string apkRevision = apkFilename.Contains("_") ? apkFilename.Substring(apkFilename.IndexOf('_') + 1) : "";

                        // Ensure destination distribution directories exist (supports both Download and downloads paths)
                        var distFolders = new List<string>();
                        if (!string.IsNullOrEmpty(app.LiveTargetFolder))
                        {
                            distFolders.Add(Path.Combine(app.LiveTargetFolder, "Download"));
                            distFolders.Add(Path.Combine(app.LiveTargetFolder, "downloads"));
                            
                            // Write latest version.txt so the POS terminal knows this version is live
                            if (!string.IsNullOrEmpty(apkRevision))
                            {
                                try
                                {
                                    string versionTxtPath = Path.Combine(app.LiveTargetFolder, "version.txt");
                                    File.WriteAllText(versionTxtPath, apkRevision);
                                    Log($">>> Updated {versionTxtPath} to version {apkRevision}", false, ColorSuccess);
                                }
                                catch (Exception ex)
                                {
                                    Log($"!!! Warning: Could not write version.txt: {ex.Message}", false, ColorWarning);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(_config.PosApkDistFolder) && !distFolders.Contains(_config.PosApkDistFolder))
                        {
                            distFolders.Add(_config.PosApkDistFolder);
                        }

                        foreach (var folder in distFolders)
                        {
                            try
                            {
                                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                                string destApk = Path.Combine(folder, "GFC_POS_Mobile.apk");
                                Log($">>> Copying APK to distribution path: {destApk}", false, ColorWait);
                                File.Copy(foundApkPath, destApk, true);
                            }
                            catch (Exception ex)
                            {
                                Log($"!!! Warning: Error copying to {folder}: {ex.Message}", false, ColorWarning);
                            }
                        }

                        Log($">>> [OK] APK successfully copied to distribution server (APK-Only).", false, ColorSuccess);

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

            bool isConnectApp = app.AppName.Equals("Connect", StringComparison.OrdinalIgnoreCase);

            // Stop IIS Pool & Site (Skip for Connect App since it only updates a static APK file without downtime)
            if (!isConnectApp && !string.IsNullOrWhiteSpace(app.IisSiteName))
            {
                Log($">>> Stopping IIS site '{app.IisSiteName}' and app pool '{app.IisAppPool}'...", false, ColorWait);
                await ToggleIisAsync(app.IisSiteName, app.IisAppPool, false);
            }
            else if (isConnectApp)
            {
                Log(">>> Connect App deployment: Zero-downtime file deployment (IIS restart skipped).", false, ColorSuccess);
            }

            try
            {
                // Step A: Archive (only when deploying fresh packages, skip for rollback)
                if (isRollback)
                {
                    Log($"[Step A] Rollback mode active: Re-archiving skipped (package is already archived).", false, ColorWait);
                }
                else
                {
                    Log($"[Step A] Archiving ZIP package '{zipName}'...", false, ColorWait);
                    if (!Directory.Exists(app.ArchiveFolder))
                    {
                        Directory.CreateDirectory(app.ArchiveFolder);
                    }
                    string archiveDest = Path.Combine(app.ArchiveFolder, zipName);
                    File.Copy(zipPath, archiveDest, true);
                    Log($"[Step A] [OK] Package archived successfully: {archiveDest}", false, ColorSuccess);
                }

                // Step B: Deploy
                if (chkPurgeFiles.Checked)
                {
                    Log($"[Step B] Clearing production folder (preserving configurations/versions): {app.LiveTargetFolder}...", false, ColorWait);
                    if (!Directory.Exists(app.LiveTargetFolder))
                    {
                        Directory.CreateDirectory(app.LiveTargetFolder);
                    }
                    await ClearDirectoryContentsAsync(app.LiveTargetFolder, true);
                }
                else
                {
                    Log($"[Step B] Purge is disabled. Skipping production folder clearing step.", false, ColorWait);
                }

                Log($"[Step B] Deploying package contents directly to live folder...", false, ColorWait);
                if (zipPath.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
                {
                    string targetApk = Path.Combine(app.LiveTargetFolder, Path.GetFileName(zipPath));
                    File.Copy(zipPath, targetApk, true);
                    string companionJson = Path.Combine(Path.GetDirectoryName(zipPath) ?? "", "version.json");
                    if (File.Exists(companionJson))
                    {
                        File.Copy(companionJson, Path.Combine(app.LiveTargetFolder, "version.json"), true);
                    }
                }
                else
                {
                    await ExtractZipAsync(zipPath, app.LiveTargetFolder);
                }
                Log($"[Step B] [OK] Deployment completed to live folder.", false, ColorSuccess);

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

                // Start IIS (Skip for Connect App since it was not stopped)
                if (!isConnectApp && !string.IsNullOrWhiteSpace(app.IisSiteName))
                {
                    Log($">>> Starting IIS site '{app.IisSiteName}' and app pool '{app.IisAppPool}'...", false, ColorWait);
                    await ToggleIisAsync(app.IisSiteName, app.IisAppPool, true);
                }

                // Step C: Cleanup (skip deletion if rollback from archive)
                if (isRollback)
                {
                    Log($"[Step C] Rollback mode active: Preserving archived package '{zipName}'.", false, ColorSuccess);
                }
                else
                {
                    Log($"[Step C] Cleaning up source package from source folder...", false, ColorWait);
                    if (File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                        Log($"[Step C] [OK] Source package '{zipName}' deleted successfully.", false, ColorSuccess);
                    }
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
                    if (apks.Length > 0)
                    {
                        Array.Sort(apks, (a, b) => File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)));
                        foundApkPath = apks[0];
                    }
                }
                if (foundApkPath == null && Directory.Exists(desktop))
                {
                    var apks = Directory.GetFiles(desktop, "*.apk");
                    if (apks.Length > 0)
                    {
                        Array.Sort(apks, (a, b) => File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)));
                        foundApkPath = apks[0];
                    }
                }
            }

            if (foundApkPath != null)
            {
                Log($">>> Located APK installer at: {foundApkPath}", false, ColorWait);
                string distFolder = _config.PosApkDistFolder;
                if (string.IsNullOrEmpty(distFolder))
                {
                    var posApp = _config.AppPipelines.Find(a => a.AppName.Equals("POS", StringComparison.OrdinalIgnoreCase));
                    distFolder = (posApp != null && !string.IsNullOrEmpty(posApp.LiveTargetFolder))
                        ? Path.Combine(posApp.LiveTargetFolder, "Download")
                        : @"C:\inetpub\wwwroot\GFCPOS\Download";
                }

                if (!Directory.Exists(distFolder))
                {
                    Directory.CreateDirectory(distFolder);
                }
                string destApk = Path.Combine(distFolder, "GFC_POS_Mobile.apk");
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

                var exclusions = new string[] { "appsettings.Production.json", "web.config", "version.txt", "version.json" };
                
                // Delete files
                foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                {
                    if (preserveExclusions)
                    {
                        // Exclude any files residing inside Download, downloads, or App_Data\Packages directories
                        if (file.IndexOf("\\Download\\", StringComparison.OrdinalIgnoreCase) >= 0 || 
                            file.IndexOf("\\downloads\\", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            file.IndexOf("\\App_Data\\Packages", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            continue;
                        }

                        // Protect APK installer packages
                        if (file.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

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
                // Protect "Download", "downloads", and "App_Data" subdirectories from being traversed/deleted
                string dirName = Path.GetFileName(directory);
                if (dirName.Equals("Download", StringComparison.OrdinalIgnoreCase) || 
                    dirName.Equals("downloads", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("App_Data", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

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

            if (clbDeployApps.SelectedItem == null)
            {
                MessageBox.Show("Please select an application from the list to deploy.", "No App Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedApp = _config.AppPipelines.Find(a => a.AppName == clbDeployApps.SelectedItem.ToString());
            if (selectedApp == null)
            {
                MessageBox.Show("Selected application configuration could not be found.", "App Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetBusy(true, $"Deploying {selectedApp.AppName}...");
            rtbTerminal.Clear();
            _fullLogHistory.Clear();
            Log($">>> STARTING DEPLOYMENT FOR TARGET: {selectedApp.AppName}...", false, ColorWait);

            // Initialize progress status indicators
            foreach (var appConfig in _config.AppPipelines)
            {
                if (appConfig.AppName == selectedApp.AppName)
                {
                    UpdateDeployVisualStatus(appConfig.AppName, "Queued ⏳", Color.Orange);
                }
                else
                {
                    UpdateDeployVisualStatus(appConfig.AppName, "Skipped ➔", Color.LightGray);
                }
            }

            var checkedApps = new List<AppPipelineConfig> { selectedApp };
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
            await Task.Run(() =>
            {
                try
                {
                    using var manager = new ServerManager();

                    // Toggle Website
                    var website = manager.Sites[site];
                    if (website != null)
                    {
                        try
                        {
                            if (start)
                            {
                                if (website.State != ObjectState.Started && website.State != ObjectState.Starting)
                                    website.Start();
                            }
                            else
                            {
                                if (website.State != ObjectState.Stopped && website.State != ObjectState.Stopping)
                                    website.Stop();
                            }
                            Log($"[IIS] Website '{site}' {(start ? "started" : "stopped")}.", false);
                        }
                        catch (Exception exSite)
                        {
                            Log($"[IIS] Note on website '{site}': {exSite.Message}", false, ColorTextMuted);
                        }
                    }

                    // Toggle Application Pool
                    var appPool = manager.ApplicationPools[pool];
                    if (appPool != null)
                    {
                        if (start)
                        {
                            // Wait briefly if pool is still stopping before starting
                            for (int i = 0; i < 10; i++)
                            {
                                if (appPool.State == ObjectState.Stopping)
                                {
                                    Thread.Sleep(500);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (appPool.State != ObjectState.Started && appPool.State != ObjectState.Starting)
                            {
                                appPool.Start();
                            }
                            Log($"[IIS] App Pool '{pool}' started.", false);
                        }
                        else
                        {
                            if (appPool.State != ObjectState.Stopped && appPool.State != ObjectState.Stopping)
                            {
                                appPool.Stop();
                            }

                            // Wait for App Pool and worker process to actually stop (up to 10 seconds)
                            Log($"[IIS] Waiting for App Pool '{pool}' worker process to shut down and release file handles...", false, ColorWait);
                            int waitedMs = 0;
                            while (appPool.State != ObjectState.Stopped && waitedMs < 10000)
                            {
                                Thread.Sleep(500);
                                waitedMs += 500;
                            }
                            Log($"[IIS] App Pool '{pool}' stopped completely ({waitedMs}ms).", false);
                        }
                    }
                    else
                    {
                        Log($"[IIS] App Pool '{pool}' not found — skipping.", false);
                    }
                }
                catch (Exception ex)
                {
                    Log($"[IIS] Warning: Could not toggle IIS state for '{site}': {ex.Message}", true);
                }
            });

            // Brief extra cooldown for Windows file handle unlock
            await Task.Delay(1000);
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

                        // File extraction with automatic retry for momentary process locks
                        bool written = false;
                        int attempts = 0;
                        while (!written && attempts < 5)
                        {
                            attempts++;
                            try
                            {
                                entry.ExtractToFile(fullDest, true);
                                written = true;
                            }
                            catch (IOException) when (attempts < 5)
                            {
                                Thread.Sleep(600); // Wait for worker process to release DLL handle
                            }
                        }

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

            // Resolve version and flags from UI
            string? customVer = radRevNext.Checked ? null : txtRevCustomValue.Text.Trim();
            bool useNext = radRevNext.Checked;
            bool dryRun = chkRevDryRun.Checked;

            SetBusy(true, "Syncing version revisions...");
            rtbTerminal.Clear();
            Log($">>> [WAIT] Triggering Version Revision Sync for project: {project}...", false, ColorWait);

            try
            {
                bool success = await VersionSyncEngine.SyncVersionAsync(
                    project: project,
                    workspace: workspace,
                    explicitVersion: customVer,
                    useNext: useNext,
                    dryRun: dryRun,
                    noSync: false,
                    log: (msg, isErr) => Log($"[SYNC] {msg}", isErr)
                );

                if (success)
                {
                    Log(">>> REVISION SYNCHRONIZATION PIPELINE SUCCESSFUL!", false, ColorSuccess);
                    SetBusy(false, "Ready");
                    MessageBox.Show("Version sync completed successfully!", "Sync Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshVersionDashboard();
                }
                else
                {
                    Log("!!! Version synchronization failed. Check errors in logs above.", true);
                    SetBusy(false, "Error: Sync Failed!");
                    MessageBox.Show("Version sync failed. Check errors in logs.", "Sync Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Log($"CRITICAL SYNC EXCEPTION: {ex.Message}", true);
                SetBusy(false, "Exception Failure");
                MessageBox.Show($"Sync failed: {ex.Message}", "Critical Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
