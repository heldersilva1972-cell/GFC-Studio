namespace GFCDevOpsUtility
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlNavBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnTabPublish = new System.Windows.Forms.Button();
            this.btnTabDeploy = new System.Windows.Forms.Button();
            this.btnTabRevisions = new System.Windows.Forms.Button();
            this.pnlMainContainer = new System.Windows.Forms.Panel();
            
            // Publish Panel controls
            this.pnlPublish = new System.Windows.Forms.Panel();
            this.grpPublishOptions = new System.Windows.Forms.GroupBox();
            this.lblPubApp = new System.Windows.Forms.Label();
            this.cmbPublishApp = new System.Windows.Forms.ComboBox();
            this.lblPubWorkspace = new System.Windows.Forms.Label();
            this.txtPubWorkspace = new System.Windows.Forms.TextBox();
            this.btnPubWorkspaceBrowse = new System.Windows.Forms.Button();
            this.lblPubOutput = new System.Windows.Forms.Label();
            this.txtPubOutput = new System.Windows.Forms.TextBox();
            this.btnPubOutputBrowse = new System.Windows.Forms.Button();
            this.btnRunPublish = new System.Windows.Forms.Button();

            // Deploy Panel controls
            this.pnlDeploy = new System.Windows.Forms.Panel();
            this.grpDeployOptions = new System.Windows.Forms.GroupBox();
            this.lblDepApp = new System.Windows.Forms.Label();
            this.cmbDeployApp = new System.Windows.Forms.ComboBox();
            this.lblDepZip = new System.Windows.Forms.Label();
            this.txtDepZip = new System.Windows.Forms.TextBox();
            this.btnDepZipBrowse = new System.Windows.Forms.Button();
            this.lblDepStaging = new System.Windows.Forms.Label();
            this.txtDepStaging = new System.Windows.Forms.TextBox();
            this.btnDepStagingBrowse = new System.Windows.Forms.Button();
            this.lblDepLive = new System.Windows.Forms.Label();
            this.txtDepLive = new System.Windows.Forms.TextBox();
            this.btnDepLiveBrowse = new System.Windows.Forms.Button();
            this.lblDepBackup = new System.Windows.Forms.Label();
            this.txtDepBackup = new System.Windows.Forms.TextBox();
            this.btnDepBackupBrowse = new System.Windows.Forms.Button();
            this.lblDepIisSite = new System.Windows.Forms.Label();
            this.txtDepIisSite = new System.Windows.Forms.TextBox();
            this.lblDepIisAppPool = new System.Windows.Forms.Label();
            this.txtDepIisAppPool = new System.Windows.Forms.TextBox();
            this.chkAutoConfigWebConfig = new System.Windows.Forms.CheckBox();
            this.chkPurgeFiles = new System.Windows.Forms.CheckBox();
            this.lblAutoConfigDesc = new System.Windows.Forms.Label();
            this.lblPurgeDesc = new System.Windows.Forms.Label();
            this.btnRunDeploy = new System.Windows.Forms.Button();

            // Revisions Panel controls
            this.pnlRevisions = new System.Windows.Forms.Panel();
            this.grpRevisionGrid = new System.Windows.Forms.GroupBox();
            this.dgvRevisions = new System.Windows.Forms.DataGridView();
            this.grpRevisionSyncOptions = new System.Windows.Forms.GroupBox();
            this.lblRevAppSelect = new System.Windows.Forms.Label();
            this.cmbRevAppSelect = new System.Windows.Forms.ComboBox();
            this.radRevNext = new System.Windows.Forms.RadioButton();
            this.radRevCustom = new System.Windows.Forms.RadioButton();
            this.txtRevCustomValue = new System.Windows.Forms.TextBox();
            this.chkRevDryRun = new System.Windows.Forms.CheckBox();
            this.btnRunRevisionSync = new System.Windows.Forms.Button();

            // Bottom elements
            this.pnlLogs = new System.Windows.Forms.Panel();
            this.lblTerminalHeader = new System.Windows.Forms.Label();
            this.rtbTerminal = new System.Windows.Forms.RichTextBox();
            this.btnClearLogs = new System.Windows.Forms.Button();
            
            this.pnlStatusStrip = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();

            this.pnlNavBar.SuspendLayout();
            this.pnlMainContainer.SuspendLayout();
            this.pnlPublish.SuspendLayout();
            this.grpPublishOptions.SuspendLayout();
            this.pnlDeploy.SuspendLayout();
            this.grpDeployOptions.SuspendLayout();
            this.pnlRevisions.SuspendLayout();
            this.grpRevisionGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevisions)).BeginInit();
            this.grpRevisionSyncOptions.SuspendLayout();
            this.pnlLogs.SuspendLayout();
            this.pnlStatusStrip.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlNavBar
            // 
            this.pnlNavBar.BackColor = System.Drawing.Color.White;
            this.pnlNavBar.Controls.Add(this.lblTitle);
            this.pnlNavBar.Controls.Add(this.btnTabPublish);
            this.pnlNavBar.Controls.Add(this.btnTabDeploy);
            this.pnlNavBar.Controls.Add(this.btnTabRevisions);
            this.pnlNavBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavBar.Location = new System.Drawing.Point(0, 0);
            this.pnlNavBar.Name = "pnlNavBar";
            this.pnlNavBar.Size = new System.Drawing.Size(960, 65);
            this.pnlNavBar.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblTitle.Location = new System.Drawing.Point(20, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(262, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "GFC STUDIO DEVOPS";
            // 
            // btnTabPublish
            // 
            this.btnTabPublish.BackColor = System.Drawing.Color.White;
            this.btnTabPublish.FlatAppearance.BorderSize = 0;
            this.btnTabPublish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabPublish.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnTabPublish.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnTabPublish.Location = new System.Drawing.Point(540, 12);
            this.btnTabPublish.Name = "btnTabPublish";
            this.btnTabPublish.Size = new System.Drawing.Size(120, 42);
            this.btnTabPublish.TabIndex = 1;
            this.btnTabPublish.Text = "📤 Publish App";
            this.btnTabPublish.UseVisualStyleBackColor = false;
            this.btnTabPublish.Click += new System.EventHandler(this.BtnTabPublish_Click);
            // 
            // btnTabDeploy
            // 
            this.btnTabDeploy.BackColor = System.Drawing.Color.White;
            this.btnTabDeploy.FlatAppearance.BorderSize = 0;
            this.btnTabDeploy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabDeploy.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnTabDeploy.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnTabDeploy.Location = new System.Drawing.Point(670, 12);
            this.btnTabDeploy.Name = "btnTabDeploy";
            this.btnTabDeploy.Size = new System.Drawing.Size(120, 42);
            this.btnTabDeploy.TabIndex = 2;
            this.btnTabDeploy.Text = "🚀 Deploy App";
            this.btnTabDeploy.UseVisualStyleBackColor = false;
            this.btnTabDeploy.Click += new System.EventHandler(this.BtnTabDeploy_Click);
            // 
            // btnTabRevisions
            // 
            this.btnTabRevisions.BackColor = System.Drawing.Color.White;
            this.btnTabRevisions.FlatAppearance.BorderSize = 0;
            this.btnTabRevisions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabRevisions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnTabRevisions.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnTabRevisions.Location = new System.Drawing.Point(800, 12);
            this.btnTabRevisions.Name = "btnTabRevisions";
            this.btnTabRevisions.Size = new System.Drawing.Size(140, 42);
            this.btnTabRevisions.TabIndex = 3;
            this.btnTabRevisions.Text = "🔄 Revisions";
            this.btnTabRevisions.UseVisualStyleBackColor = false;
            this.btnTabRevisions.Click += new System.EventHandler(this.BtnTabRevisions_Click);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlMainContainer.Controls.Add(this.pnlPublish);
            this.pnlMainContainer.Controls.Add(this.pnlDeploy);
            this.pnlMainContainer.Controls.Add(this.pnlRevisions);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 65);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Padding = new System.Windows.Forms.Padding(15);
            this.pnlMainContainer.Size = new System.Drawing.Size(960, 390);
            this.pnlMainContainer.TabIndex = 1;
            // 
            // pnlPublish
            // 
            this.pnlPublish.Controls.Add(this.grpPublishOptions);
            this.pnlPublish.Controls.Add(this.btnRunPublish);
            this.pnlPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPublish.Location = new System.Drawing.Point(15, 15);
            this.pnlPublish.Name = "pnlPublish";
            this.pnlPublish.Size = new System.Drawing.Size(930, 360);
            this.pnlPublish.TabIndex = 0;
            // 
            // grpPublishOptions
            // 
            this.grpPublishOptions.BackColor = System.Drawing.Color.White;
            this.grpPublishOptions.Controls.Add(this.lblPubApp);
            this.grpPublishOptions.Controls.Add(this.cmbPublishApp);
            this.grpPublishOptions.Controls.Add(this.lblPubWorkspace);
            this.grpPublishOptions.Controls.Add(this.txtPubWorkspace);
            this.grpPublishOptions.Controls.Add(this.btnPubWorkspaceBrowse);
            this.grpPublishOptions.Controls.Add(this.lblPubOutput);
            this.grpPublishOptions.Controls.Add(this.txtPubOutput);
            this.grpPublishOptions.Controls.Add(this.btnPubOutputBrowse);
            this.grpPublishOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpPublishOptions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpPublishOptions.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.grpPublishOptions.Location = new System.Drawing.Point(5, 5);
            this.grpPublishOptions.Name = "grpPublishOptions";
            this.grpPublishOptions.Size = new System.Drawing.Size(915, 260);
            this.grpPublishOptions.TabIndex = 0;
            this.grpPublishOptions.TabStop = false;
            this.grpPublishOptions.Text = "Publish Project Parameters";
            // 
            // lblPubApp
            // 
            this.lblPubApp.AutoSize = true;
            this.lblPubApp.Location = new System.Drawing.Point(20, 35);
            this.lblPubApp.Name = "lblPubApp";
            this.lblPubApp.Size = new System.Drawing.Size(126, 19);
            this.lblPubApp.TabIndex = 0;
            this.lblPubApp.Text = "Select App Project:";
            // 
            // cmbPublishApp
            // 
            this.cmbPublishApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPublishApp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbPublishApp.FormattingEnabled = true;
            this.cmbPublishApp.Items.AddRange(new object[] {
            "📱 Mobile App (Standalone)",
            "🖥️ POS App (Web Terminal)",
            "🌐 Web App (Blazor Server)",
            "📦 Full Suite (All Applications)"});
            this.cmbPublishApp.Location = new System.Drawing.Point(200, 32);
            this.cmbPublishApp.Name = "cmbPublishApp";
            this.cmbPublishApp.Size = new System.Drawing.Size(350, 25);
            this.cmbPublishApp.TabIndex = 1;
            // 
            // lblPubWorkspace
            // 
            this.lblPubWorkspace.AutoSize = true;
            this.lblPubWorkspace.Location = new System.Drawing.Point(20, 95);
            this.lblPubWorkspace.Name = "lblPubWorkspace";
            this.lblPubWorkspace.Size = new System.Drawing.Size(161, 19);
            this.lblPubWorkspace.TabIndex = 2;
            this.lblPubWorkspace.Text = "GFC-Studio Workspace:";
            // 
            // txtPubWorkspace
            // 
            this.txtPubWorkspace.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPubWorkspace.Location = new System.Drawing.Point(200, 92);
            this.txtPubWorkspace.Name = "txtPubWorkspace";
            this.txtPubWorkspace.Size = new System.Drawing.Size(580, 25);
            this.txtPubWorkspace.TabIndex = 3;
            // 
            // btnPubWorkspaceBrowse
            // 
            this.btnPubWorkspaceBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPubWorkspaceBrowse.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPubWorkspaceBrowse.Location = new System.Drawing.Point(795, 91);
            this.btnPubWorkspaceBrowse.Name = "btnPubWorkspaceBrowse";
            this.btnPubWorkspaceBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnPubWorkspaceBrowse.TabIndex = 4;
            this.btnPubWorkspaceBrowse.Text = "Browse...";
            this.btnPubWorkspaceBrowse.UseVisualStyleBackColor = true;
            this.btnPubWorkspaceBrowse.Click += new System.EventHandler(this.BtnPubWorkspaceBrowse_Click);
            // 
            // lblPubOutput
            // 
            this.lblPubOutput.AutoSize = true;
            this.lblPubOutput.Location = new System.Drawing.Point(20, 155);
            this.lblPubOutput.Name = "lblPubOutput";
            this.lblPubOutput.Size = new System.Drawing.Size(147, 19);
            this.lblPubOutput.TabIndex = 5;
            this.lblPubOutput.Text = "ZIP Output Directory:";
            // 
            // txtPubOutput
            // 
            this.txtPubOutput.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPubOutput.Location = new System.Drawing.Point(200, 152);
            this.txtPubOutput.Name = "txtPubOutput";
            this.txtPubOutput.Size = new System.Drawing.Size(580, 25);
            this.txtPubOutput.TabIndex = 6;
            // 
            // btnPubOutputBrowse
            // 
            this.btnPubOutputBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPubOutputBrowse.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPubOutputBrowse.Location = new System.Drawing.Point(795, 151);
            this.btnPubOutputBrowse.Name = "btnPubOutputBrowse";
            this.btnPubOutputBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnPubOutputBrowse.TabIndex = 7;
            this.btnPubOutputBrowse.Text = "Browse...";
            this.btnPubOutputBrowse.UseVisualStyleBackColor = true;
            this.btnPubOutputBrowse.Click += new System.EventHandler(this.BtnPubOutputBrowse_Click);
            // 
            // btnRunPublish
            // 
            this.btnRunPublish.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnRunPublish.FlatAppearance.BorderSize = 0;
            this.btnRunPublish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunPublish.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRunPublish.ForeColor = System.Drawing.Color.White;
            this.btnRunPublish.Location = new System.Drawing.Point(5, 285);
            this.btnRunPublish.Name = "btnRunPublish";
            this.btnRunPublish.Size = new System.Drawing.Size(250, 45);
            this.btnRunPublish.TabIndex = 1;
            this.btnRunPublish.Text = "📤 Start Application Publish";
            this.btnRunPublish.UseVisualStyleBackColor = false;
            this.btnRunPublish.Click += new System.EventHandler(this.BtnRunPublish_Click);
            // 
            // pnlDeploy
            // 
            this.pnlDeploy.Controls.Add(this.grpDeployOptions);
            this.pnlDeploy.Controls.Add(this.btnRunDeploy);
            this.pnlDeploy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDeploy.Location = new System.Drawing.Point(15, 15);
            this.pnlDeploy.Name = "pnlDeploy";
            this.pnlDeploy.Size = new System.Drawing.Size(930, 360);
            this.pnlDeploy.TabIndex = 0;
            this.pnlDeploy.Visible = false;
            // 
            // grpDeployOptions
            // 
            this.grpDeployOptions.BackColor = System.Drawing.Color.White;
            this.grpDeployOptions.Controls.Add(this.lblDepApp);
            this.grpDeployOptions.Controls.Add(this.cmbDeployApp);
            this.grpDeployOptions.Controls.Add(this.lblDepZip);
            this.grpDeployOptions.Controls.Add(this.txtDepZip);
            this.grpDeployOptions.Controls.Add(this.btnDepZipBrowse);
            this.grpDeployOptions.Controls.Add(this.lblDepStaging);
            this.grpDeployOptions.Controls.Add(this.txtDepStaging);
            this.grpDeployOptions.Controls.Add(this.btnDepStagingBrowse);
            this.grpDeployOptions.Controls.Add(this.lblDepLive);
            this.grpDeployOptions.Controls.Add(this.txtDepLive);
            this.grpDeployOptions.Controls.Add(this.btnDepLiveBrowse);
            this.grpDeployOptions.Controls.Add(this.lblDepBackup);
            this.grpDeployOptions.Controls.Add(this.txtDepBackup);
            this.grpDeployOptions.Controls.Add(this.btnDepBackupBrowse);
            this.grpDeployOptions.Controls.Add(this.lblDepIisSite);
            this.grpDeployOptions.Controls.Add(this.txtDepIisSite);
            this.grpDeployOptions.Controls.Add(this.lblDepIisAppPool);
            this.grpDeployOptions.Controls.Add(this.txtDepIisAppPool);
            this.grpDeployOptions.Controls.Add(this.chkAutoConfigWebConfig);
            this.grpDeployOptions.Controls.Add(this.lblAutoConfigDesc);
            this.grpDeployOptions.Controls.Add(this.chkPurgeFiles);
            this.grpDeployOptions.Controls.Add(this.lblPurgeDesc);
            this.grpDeployOptions.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpDeployOptions.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.grpDeployOptions.Location = new System.Drawing.Point(5, 5);
            this.grpDeployOptions.Name = "grpDeployOptions";
            this.grpDeployOptions.Size = new System.Drawing.Size(915, 260);
            this.grpDeployOptions.TabIndex = 0;
            this.grpDeployOptions.TabStop = false;
            this.grpDeployOptions.Text = "Deployment Target Parameters";
            // 
            // lblDepApp
            // 
            this.lblDepApp.AutoSize = true;
            this.lblDepApp.Location = new System.Drawing.Point(15, 25);
            this.lblDepApp.Name = "lblDepApp";
            this.lblDepApp.Size = new System.Drawing.Size(120, 17);
            this.lblDepApp.TabIndex = 0;
            this.lblDepApp.Text = "Select App Deploy:";
            // 
            // cmbDeployApp
            // 
            this.cmbDeployApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeployApp.FormattingEnabled = true;
            this.cmbDeployApp.Items.AddRange(new object[] {
            "📱 Mobile App (Standalone)",
            "🖥️ POS App (Web Terminal)",
            "🌐 Web App (Blazor Server)",
            "📦 Full Suite (All Applications)"});
            this.cmbDeployApp.Location = new System.Drawing.Point(150, 22);
            this.cmbDeployApp.Name = "cmbDeployApp";
            this.cmbDeployApp.Size = new System.Drawing.Size(250, 25);
            this.cmbDeployApp.TabIndex = 1;
            this.cmbDeployApp.SelectedIndexChanged += new System.EventHandler(this.CmbDeployApp_SelectedIndexChanged);
            // 
            // lblDepZip
            // 
            this.lblDepZip.AutoSize = true;
            this.lblDepZip.Location = new System.Drawing.Point(15, 62);
            this.lblDepZip.Name = "lblDepZip";
            this.lblDepZip.Size = new System.Drawing.Size(117, 17);
            this.lblDepZip.TabIndex = 2;
            this.lblDepZip.Text = "Source ZIP Folder:";
            // 
            // txtDepZip
            // 
            this.txtDepZip.Location = new System.Drawing.Point(150, 59);
            this.txtDepZip.Name = "txtDepZip";
            this.txtDepZip.Size = new System.Drawing.Size(250, 24);
            this.txtDepZip.TabIndex = 3;
            // 
            // btnDepZipBrowse
            // 
            this.btnDepZipBrowse.Location = new System.Drawing.Point(408, 57);
            this.btnDepZipBrowse.Name = "btnDepZipBrowse";
            this.btnDepZipBrowse.Size = new System.Drawing.Size(35, 28);
            this.btnDepZipBrowse.TabIndex = 4;
            this.btnDepZipBrowse.Text = "...";
            this.btnDepZipBrowse.UseVisualStyleBackColor = true;
            this.btnDepZipBrowse.Click += new System.EventHandler(this.BtnDepZipBrowse_Click);
            // 
            // lblDepStaging
            // 
            this.lblDepStaging.AutoSize = true;
            this.lblDepStaging.Location = new System.Drawing.Point(15, 102);
            this.lblDepStaging.Name = "lblDepStaging";
            this.lblDepStaging.Size = new System.Drawing.Size(99, 17);
            this.lblDepStaging.TabIndex = 5;
            this.lblDepStaging.Text = "Staging Folder:";
            // 
            // txtDepStaging
            // 
            this.txtDepStaging.Location = new System.Drawing.Point(150, 99);
            this.txtDepStaging.Name = "txtDepStaging";
            this.txtDepStaging.Size = new System.Drawing.Size(250, 24);
            this.txtDepStaging.TabIndex = 6;
            // 
            // btnDepStagingBrowse
            // 
            this.btnDepStagingBrowse.Location = new System.Drawing.Point(408, 97);
            this.btnDepStagingBrowse.Name = "btnDepStagingBrowse";
            this.btnDepStagingBrowse.Size = new System.Drawing.Size(35, 28);
            this.btnDepStagingBrowse.TabIndex = 7;
            this.btnDepStagingBrowse.Text = "...";
            this.btnDepStagingBrowse.UseVisualStyleBackColor = true;
            this.btnDepStagingBrowse.Click += new System.EventHandler(this.BtnDepStagingBrowse_Click);
            // 
            // lblDepLive
            // 
            this.lblDepLive.AutoSize = true;
            this.lblDepLive.Location = new System.Drawing.Point(15, 142);
            this.lblDepLive.Name = "lblDepLive";
            this.lblDepLive.Size = new System.Drawing.Size(119, 17);
            this.lblDepLive.TabIndex = 8;
            this.lblDepLive.Text = "Live Target Folder:";
            // 
            // txtDepLive
            // 
            this.txtDepLive.Location = new System.Drawing.Point(150, 139);
            this.txtDepLive.Name = "txtDepLive";
            this.txtDepLive.Size = new System.Drawing.Size(250, 24);
            this.txtDepLive.TabIndex = 9;
            // 
            // btnDepLiveBrowse
            // 
            this.btnDepLiveBrowse.Location = new System.Drawing.Point(408, 137);
            this.btnDepLiveBrowse.Name = "btnDepLiveBrowse";
            this.btnDepLiveBrowse.Size = new System.Drawing.Size(35, 28);
            this.btnDepLiveBrowse.TabIndex = 10;
            this.btnDepLiveBrowse.Text = "...";
            this.btnDepLiveBrowse.UseVisualStyleBackColor = true;
            this.btnDepLiveBrowse.Click += new System.EventHandler(this.BtnDepLiveBrowse_Click);
            // 
            // lblDepBackup
            // 
            this.lblDepBackup.AutoSize = true;
            this.lblDepBackup.Location = new System.Drawing.Point(15, 182);
            this.lblDepBackup.Name = "lblDepBackup";
            this.lblDepBackup.Size = new System.Drawing.Size(97, 17);
            this.lblDepBackup.TabIndex = 11;
            this.lblDepBackup.Text = "Backup Folder:";
            // 
            // txtDepBackup
            // 
            this.txtDepBackup.Location = new System.Drawing.Point(150, 179);
            this.txtDepBackup.Name = "txtDepBackup";
            this.txtDepBackup.Size = new System.Drawing.Size(250, 24);
            this.txtDepBackup.TabIndex = 12;
            // 
            // btnDepBackupBrowse
            // 
            this.btnDepBackupBrowse.Location = new System.Drawing.Point(408, 177);
            this.btnDepBackupBrowse.Name = "btnDepBackupBrowse";
            this.btnDepBackupBrowse.Size = new System.Drawing.Size(35, 28);
            this.btnDepBackupBrowse.TabIndex = 13;
            this.btnDepBackupBrowse.Text = "...";
            this.btnDepBackupBrowse.UseVisualStyleBackColor = true;
            this.btnDepBackupBrowse.Click += new System.EventHandler(this.BtnDepBackupBrowse_Click);
            // 
            // 
            // lblDepIisSite
            // 
            this.lblDepIisSite.AutoSize = true;
            this.lblDepIisSite.Location = new System.Drawing.Point(470, 25);
            this.lblDepIisSite.Name = "lblDepIisSite";
            this.lblDepIisSite.Size = new System.Drawing.Size(91, 17);
            this.lblDepIisSite.TabIndex = 14;
            this.lblDepIisSite.Text = "IIS Site Name:";
            // 
            // txtDepIisSite
            // 
            this.txtDepIisSite.Location = new System.Drawing.Point(620, 22);
            this.txtDepIisSite.Name = "txtDepIisSite";
            this.txtDepIisSite.Size = new System.Drawing.Size(250, 24);
            this.txtDepIisSite.TabIndex = 15;
            // 
            // lblDepIisAppPool
            // 
            this.lblDepIisAppPool.AutoSize = true;
            this.lblDepIisAppPool.Location = new System.Drawing.Point(470, 62);
            this.lblDepIisAppPool.Name = "lblDepIisAppPool";
            this.lblDepIisAppPool.Size = new System.Drawing.Size(126, 17);
            this.lblDepIisAppPool.TabIndex = 16;
            this.lblDepIisAppPool.Text = "IIS Application Pool:";
            // 
            // txtDepIisAppPool
            // 
            this.txtDepIisAppPool.Location = new System.Drawing.Point(620, 59);
            this.txtDepIisAppPool.Name = "txtDepIisAppPool";
            this.txtDepIisAppPool.Size = new System.Drawing.Size(250, 24);
            this.txtDepIisAppPool.TabIndex = 17;
            // 
            // chkAutoConfigWebConfig
            // 
            this.chkAutoConfigWebConfig.AutoSize = true;
            this.chkAutoConfigWebConfig.Checked = true;
            this.chkAutoConfigWebConfig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoConfigWebConfig.Location = new System.Drawing.Point(470, 102);
            this.chkAutoConfigWebConfig.Name = "chkAutoConfigWebConfig";
            this.chkAutoConfigWebConfig.Size = new System.Drawing.Size(325, 21);
            this.chkAutoConfigWebConfig.TabIndex = 18;
            this.chkAutoConfigWebConfig.Text = "Auto-configure web.config (for WASM / SPA routing)";
            this.chkAutoConfigWebConfig.UseVisualStyleBackColor = true;
            // 
            // lblAutoConfigDesc
            // 
            this.lblAutoConfigDesc.AutoSize = false;
            this.lblAutoConfigDesc.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblAutoConfigDesc.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblAutoConfigDesc.Location = new System.Drawing.Point(492, 124);
            this.lblAutoConfigDesc.Name = "lblAutoConfigDesc";
            this.lblAutoConfigDesc.Size = new System.Drawing.Size(380, 32);
            this.lblAutoConfigDesc.TabIndex = 20;
            this.lblAutoConfigDesc.Text = "Writes an optimized web.config to IIS so WASM mime-types are served and client-side SPA routing refreshes work.";
            // 
            // chkPurgeFiles
            // 
            this.chkPurgeFiles.AutoSize = true;
            this.chkPurgeFiles.Checked = true;
            this.chkPurgeFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPurgeFiles.Location = new System.Drawing.Point(470, 160);
            this.chkPurgeFiles.Name = "chkPurgeFiles";
            this.chkPurgeFiles.Size = new System.Drawing.Size(378, 21);
            this.chkPurgeFiles.TabIndex = 19;
            this.chkPurgeFiles.Text = "Purge existing files on live target (exclude appsettings / config)";
            this.chkPurgeFiles.UseVisualStyleBackColor = true;
            // 
            // lblPurgeDesc
            // 
            this.lblPurgeDesc.AutoSize = false;
            this.lblPurgeDesc.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblPurgeDesc.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblPurgeDesc.Location = new System.Drawing.Point(492, 182);
            this.lblPurgeDesc.Name = "lblPurgeDesc";
            this.lblPurgeDesc.Size = new System.Drawing.Size(380, 32);
            this.lblPurgeDesc.TabIndex = 21;
            this.lblPurgeDesc.Text = "Removes old files in live folder first during deployments, keeping your production appsettings configurations safe.";
            // 
            // btnRunDeploy
            // 
            this.btnRunDeploy.BackColor = System.Drawing.Color.FromArgb(22, 101, 52);
            this.btnRunDeploy.FlatAppearance.BorderSize = 0;
            this.btnRunDeploy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunDeploy.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRunDeploy.ForeColor = System.Drawing.Color.White;
            this.btnRunDeploy.Location = new System.Drawing.Point(5, 285);
            this.btnRunDeploy.Name = "btnRunDeploy";
            this.btnRunDeploy.Size = new System.Drawing.Size(250, 45);
            this.btnRunDeploy.TabIndex = 1;
            this.btnRunDeploy.Text = "🚀 Run Deployment Pipeline";
            this.btnRunDeploy.UseVisualStyleBackColor = false;
            this.btnRunDeploy.Click += new System.EventHandler(this.BtnRunDeploy_Click);
            // 
            // pnlRevisions
            // 
            this.pnlRevisions.Controls.Add(this.grpRevisionGrid);
            this.pnlRevisions.Controls.Add(this.grpRevisionSyncOptions);
            this.pnlRevisions.Controls.Add(this.btnRunRevisionSync);
            this.pnlRevisions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRevisions.Location = new System.Drawing.Point(15, 15);
            this.pnlRevisions.Name = "pnlRevisions";
            this.pnlRevisions.Size = new System.Drawing.Size(930, 360);
            this.pnlRevisions.TabIndex = 0;
            this.pnlRevisions.Visible = false;
            // 
            // grpRevisionGrid
            // 
            this.grpRevisionGrid.BackColor = System.Drawing.Color.White;
            this.grpRevisionGrid.Controls.Add(this.dgvRevisions);
            this.grpRevisionGrid.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpRevisionGrid.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.grpRevisionGrid.Location = new System.Drawing.Point(5, 5);
            this.grpRevisionGrid.Name = "grpRevisionGrid";
            this.grpRevisionGrid.Size = new System.Drawing.Size(490, 260);
            this.grpRevisionGrid.TabIndex = 0;
            this.grpRevisionGrid.TabStop = false;
            this.grpRevisionGrid.Text = "Version Revision Live Status";
            // 
            // dgvRevisions
            // 
            this.dgvRevisions.AllowUserToAddRows = false;
            this.dgvRevisions.AllowUserToDeleteRows = false;
            this.dgvRevisions.AllowUserToResizeRows = false;
            this.dgvRevisions.BackgroundColor = System.Drawing.Color.White;
            this.dgvRevisions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRevisions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRevisions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRevisions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRevisions.Location = new System.Drawing.Point(3, 21);
            this.dgvRevisions.MultiSelect = false;
            this.dgvRevisions.Name = "dgvRevisions";
            this.dgvRevisions.ReadOnly = true;
            this.dgvRevisions.RowHeadersVisible = false;
            this.dgvRevisions.RowTemplate.Height = 35;
            this.dgvRevisions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRevisions.Size = new System.Drawing.Size(484, 236);
            this.dgvRevisions.TabIndex = 0;
            // 
            // grpRevisionSyncOptions
            // 
            this.grpRevisionSyncOptions.BackColor = System.Drawing.Color.White;
            this.grpRevisionSyncOptions.Controls.Add(this.lblRevAppSelect);
            this.grpRevisionSyncOptions.Controls.Add(this.cmbRevAppSelect);
            this.grpRevisionSyncOptions.Controls.Add(this.radRevNext);
            this.grpRevisionSyncOptions.Controls.Add(this.radRevCustom);
            this.grpRevisionSyncOptions.Controls.Add(this.txtRevCustomValue);
            this.grpRevisionSyncOptions.Controls.Add(this.chkRevDryRun);
            this.grpRevisionSyncOptions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpRevisionSyncOptions.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.grpRevisionSyncOptions.Location = new System.Drawing.Point(510, 5);
            this.grpRevisionSyncOptions.Name = "grpRevisionSyncOptions";
            this.grpRevisionSyncOptions.Size = new System.Drawing.Size(410, 260);
            this.grpRevisionSyncOptions.TabIndex = 1;
            this.grpRevisionSyncOptions.TabStop = false;
            this.grpRevisionSyncOptions.Text = "Revision Synchronization Parameters";
            // 
            // lblRevAppSelect
            // 
            this.lblRevAppSelect.AutoSize = true;
            this.lblRevAppSelect.Location = new System.Drawing.Point(20, 35);
            this.lblRevAppSelect.Name = "lblRevAppSelect";
            this.lblRevAppSelect.Size = new System.Drawing.Size(103, 19);
            this.lblRevAppSelect.TabIndex = 0;
            this.lblRevAppSelect.Text = "Target Project:";
            // 
            // cmbRevAppSelect
            // 
            this.cmbRevAppSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRevAppSelect.FormattingEnabled = true;
            this.cmbRevAppSelect.Items.AddRange(new object[] {
            "Mobile",
            "POS",
            "WebApp"});
            this.cmbRevAppSelect.Location = new System.Drawing.Point(150, 32);
            this.cmbRevAppSelect.Name = "cmbRevAppSelect";
            this.cmbRevAppSelect.Size = new System.Drawing.Size(230, 25);
            this.cmbRevAppSelect.TabIndex = 1;
            this.cmbRevAppSelect.SelectedIndexChanged += new System.EventHandler(this.CmbRevAppSelect_SelectedIndexChanged);
            // 
            // radRevNext
            // 
            this.radRevNext.AutoSize = true;
            this.radRevNext.Checked = true;
            this.radRevNext.Location = new System.Drawing.Point(20, 85);
            this.radRevNext.Name = "radRevNext";
            this.radRevNext.Size = new System.Drawing.Size(229, 23);
            this.radRevNext.TabIndex = 2;
            this.radRevNext.TabStop = true;
            this.radRevNext.Text = "Auto-Increment Version (-Next)";
            this.radRevNext.UseVisualStyleBackColor = true;
            this.radRevNext.CheckedChanged += new System.EventHandler(this.RadRevOption_CheckedChanged);
            // 
            // radRevCustom
            // 
            this.radRevCustom.AutoSize = true;
            this.radRevCustom.Location = new System.Drawing.Point(20, 130);
            this.radRevCustom.Name = "radRevCustom";
            this.radRevCustom.Size = new System.Drawing.Size(183, 23);
            this.radRevCustom.TabIndex = 3;
            this.radRevCustom.Text = "Specify Custom Version:";
            this.radRevCustom.UseVisualStyleBackColor = true;
            this.radRevCustom.CheckedChanged += new System.EventHandler(this.RadRevOption_CheckedChanged);
            // 
            // txtRevCustomValue
            // 
            this.txtRevCustomValue.Enabled = false;
            this.txtRevCustomValue.Location = new System.Drawing.Point(215, 129);
            this.txtRevCustomValue.Name = "txtRevCustomValue";
            this.txtRevCustomValue.Size = new System.Drawing.Size(165, 25);
            this.txtRevCustomValue.TabIndex = 4;
            this.txtRevCustomValue.TextChanged += new System.EventHandler(this.TxtRevCustomValue_TextChanged);
            // 
            // chkRevDryRun
            // 
            this.chkRevDryRun.AutoSize = true;
            this.chkRevDryRun.Checked = false;
            this.chkRevDryRun.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.chkRevDryRun.Location = new System.Drawing.Point(20, 185);
            this.chkRevDryRun.Name = "chkRevDryRun";
            this.chkRevDryRun.Size = new System.Drawing.Size(232, 23);
            this.chkRevDryRun.TabIndex = 5;
            this.chkRevDryRun.Text = "Run Dry Run (Test-only Simulation)";
            this.chkRevDryRun.UseVisualStyleBackColor = true;
            // 
            // btnRunRevisionSync
            // 
            this.btnRunRevisionSync.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnRunRevisionSync.FlatAppearance.BorderSize = 0;
            this.btnRunRevisionSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunRevisionSync.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRunRevisionSync.ForeColor = System.Drawing.Color.White;
            this.btnRunRevisionSync.Location = new System.Drawing.Point(5, 285);
            this.btnRunRevisionSync.Name = "btnRunRevisionSync";
            this.btnRunRevisionSync.Size = new System.Drawing.Size(250, 45);
            this.btnRunRevisionSync.TabIndex = 2;
            this.btnRunRevisionSync.Text = "🔄 Run Version Revision Sync";
            this.btnRunRevisionSync.UseVisualStyleBackColor = false;
            this.btnRunRevisionSync.Click += new System.EventHandler(this.BtnRunRevisionSync_Click);
            // 
            // pnlLogs
            // 
            this.pnlLogs.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.pnlLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLogs.Controls.Add(this.lblTerminalHeader);
            this.pnlLogs.Controls.Add(this.rtbTerminal);
            this.pnlLogs.Controls.Add(this.btnClearLogs);
            this.pnlLogs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLogs.Location = new System.Drawing.Point(0, 455);
            this.pnlLogs.Name = "pnlLogs";
            this.pnlLogs.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
            this.pnlLogs.Size = new System.Drawing.Size(960, 210);
            this.pnlLogs.TabIndex = 2;
            // 
            // lblTerminalHeader
            // 
            this.lblTerminalHeader.AutoSize = true;
            this.lblTerminalHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTerminalHeader.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblTerminalHeader.Location = new System.Drawing.Point(10, 8);
            this.lblTerminalHeader.Name = "lblTerminalHeader";
            this.lblTerminalHeader.Size = new System.Drawing.Size(126, 15);
            this.lblTerminalHeader.TabIndex = 0;
            this.lblTerminalHeader.Text = "INTEGRATED LOGS LOG";
            // 
            // rtbTerminal
            // 
            this.rtbTerminal.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.rtbTerminal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTerminal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbTerminal.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbTerminal.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.rtbTerminal.Location = new System.Drawing.Point(5, 30);
            this.rtbTerminal.Name = "rtbTerminal";
            this.rtbTerminal.ReadOnly = true;
            this.rtbTerminal.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbTerminal.Size = new System.Drawing.Size(948, 173);
            this.rtbTerminal.TabIndex = 1;
            this.rtbTerminal.Text = "";
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClearLogs.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnClearLogs.Location = new System.Drawing.Point(875, 4);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(75, 22);
            this.btnClearLogs.TabIndex = 2;
            this.btnClearLogs.Text = "Clear Log";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // pnlStatusStrip
            // 
            this.pnlStatusStrip.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.pnlStatusStrip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStatusStrip.Controls.Add(this.lblStatus);
            this.pnlStatusStrip.Controls.Add(this.pbProgress);
            this.pnlStatusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatusStrip.Location = new System.Drawing.Point(0, 665);
            this.pnlStatusStrip.Name = "pnlStatusStrip";
            this.pnlStatusStrip.Size = new System.Drawing.Size(960, 35);
            this.pnlStatusStrip.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblStatus.Location = new System.Drawing.Point(10, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status: Ready";
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(700, 7);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(248, 20);
            this.pbProgress.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(960, 700);
            this.Controls.Add(this.pnlMainContainer);
            this.Controls.Add(this.pnlLogs);
            this.Controls.Add(this.pnlStatusStrip);
            this.Controls.Add(this.pnlNavBar);
            this.MinimumSize = new System.Drawing.Size(975, 740);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GFC Studio DevOps Utility";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlNavBar.ResumeLayout(false);
            this.pnlNavBar.PerformLayout();
            this.pnlMainContainer.ResumeLayout(false);
            this.pnlPublish.ResumeLayout(false);
            this.grpPublishOptions.ResumeLayout(false);
            this.grpPublishOptions.PerformLayout();
            this.pnlDeploy.ResumeLayout(false);
            this.grpDeployOptions.ResumeLayout(false);
            this.grpDeployOptions.PerformLayout();
            this.pnlRevisions.ResumeLayout(false);
            this.grpRevisionGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevisions)).EndInit();
            this.grpRevisionSyncOptions.ResumeLayout(false);
            this.grpRevisionSyncOptions.PerformLayout();
            this.pnlLogs.ResumeLayout(false);
            this.pnlLogs.PerformLayout();
            this.pnlStatusStrip.ResumeLayout(false);
            this.pnlStatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNavBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnTabPublish;
        private System.Windows.Forms.Button btnTabDeploy;
        private System.Windows.Forms.Button btnTabRevisions;
        
        private System.Windows.Forms.Panel pnlMainContainer;
        
        // Publish Tab Panel
        private System.Windows.Forms.Panel pnlPublish;
        private System.Windows.Forms.GroupBox grpPublishOptions;
        private System.Windows.Forms.Label lblPubApp;
        private System.Windows.Forms.ComboBox cmbPublishApp;
        private System.Windows.Forms.Label lblPubWorkspace;
        private System.Windows.Forms.TextBox txtPubWorkspace;
        private System.Windows.Forms.Button btnPubWorkspaceBrowse;
        private System.Windows.Forms.Label lblPubOutput;
        private System.Windows.Forms.TextBox txtPubOutput;
        private System.Windows.Forms.Button btnPubOutputBrowse;
        private System.Windows.Forms.Button btnRunPublish;

        // Deploy Tab Panel
        private System.Windows.Forms.Panel pnlDeploy;
        private System.Windows.Forms.GroupBox grpDeployOptions;
        private System.Windows.Forms.Label lblDepApp;
        private System.Windows.Forms.ComboBox cmbDeployApp;
        private System.Windows.Forms.Label lblDepZip;
        private System.Windows.Forms.TextBox txtDepZip;
        private System.Windows.Forms.Button btnDepZipBrowse;
        private System.Windows.Forms.Label lblDepStaging;
        private System.Windows.Forms.TextBox txtDepStaging;
        private System.Windows.Forms.Button btnDepStagingBrowse;
        private System.Windows.Forms.Label lblDepLive;
        private System.Windows.Forms.TextBox txtDepLive;
        private System.Windows.Forms.Button btnDepLiveBrowse;
        private System.Windows.Forms.Label lblDepBackup;
        private System.Windows.Forms.TextBox txtDepBackup;
        private System.Windows.Forms.Button btnDepBackupBrowse;
        private System.Windows.Forms.Label lblDepIisSite;
        private System.Windows.Forms.TextBox txtDepIisSite;
        private System.Windows.Forms.Label lblDepIisAppPool;
        private System.Windows.Forms.TextBox txtDepIisAppPool;
        private System.Windows.Forms.CheckBox chkAutoConfigWebConfig;
        private System.Windows.Forms.CheckBox chkPurgeFiles;
        private System.Windows.Forms.Button btnRunDeploy;

        // Revisions Tab Panel
        private System.Windows.Forms.Panel pnlRevisions;
        private System.Windows.Forms.GroupBox grpRevisionGrid;
        private System.Windows.Forms.DataGridView dgvRevisions;
        private System.Windows.Forms.GroupBox grpRevisionSyncOptions;
        private System.Windows.Forms.Label lblRevAppSelect;
        private System.Windows.Forms.ComboBox cmbRevAppSelect;
        private System.Windows.Forms.RadioButton radRevNext;
        private System.Windows.Forms.RadioButton radRevCustom;
        private System.Windows.Forms.TextBox txtRevCustomValue;
        private System.Windows.Forms.CheckBox chkRevDryRun;
        private System.Windows.Forms.Button btnRunRevisionSync;

        // Logs and Status elements
        private System.Windows.Forms.Panel pnlLogs;
        private System.Windows.Forms.Label lblTerminalHeader;
        private System.Windows.Forms.RichTextBox rtbTerminal;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Panel pnlStatusStrip;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label lblAutoConfigDesc;
        private System.Windows.Forms.Label lblPurgeDesc;
    }
}
