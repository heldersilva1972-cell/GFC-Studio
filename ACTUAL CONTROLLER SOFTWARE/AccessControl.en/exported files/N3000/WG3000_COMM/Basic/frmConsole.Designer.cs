namespace WG3000_COMM.Basic
{
	// Token: 0x02000041 RID: 65
	public partial class frmConsole : global::System.Windows.Forms.Form
	{
		// Token: 0x06000558 RID: 1368 RVA: 0x00098ADC File Offset: 0x00097ADC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.control4btnServer != null)
			{
				this.control4btnServer.Dispose();
			}
			if (disposing && this.control4Check != null)
			{
				this.control4Check.Dispose();
			}
			if (disposing && this.control4Realtime != null)
			{
				this.control4Realtime.Dispose();
			}
			if (disposing && this.control4uploadPrivilege != null)
			{
				this.control4uploadPrivilege.Dispose();
			}
			if (disposing && this.control4getRecordsFromController != null)
			{
				this.control4getRecordsFromController.Dispose();
			}
			if (disposing && this.pr4uploadPrivilege != null)
			{
				this.pr4uploadPrivilege.Dispose();
			}
			if (disposing && this.swipe4GetRecords != null)
			{
				this.swipe4GetRecords.Dispose();
			}
			if (disposing && this.watching != null && global::WG3000_COMM.Core.wgTools.bUDPOnly64 <= 0)
			{
				this.watching.Dispose();
			}
			if (disposing && this.dfrmWait1 != null)
			{
				this.dfrmWait1.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00098BD4 File Offset: 0x00097BD4
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmConsole));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.cboUsers = new global::System.Windows.Forms.ComboBox();
			this.grpTool = new global::System.Windows.Forms.GroupBox();
			this.btnHideTools = new global::System.Windows.Forms.Button();
			this.chkDisplayNewestSwipe = new global::System.Windows.Forms.CheckBox();
			this.chkNeedCheckLosePacket = new global::System.Windows.Forms.CheckBox();
			this.lstDoors = new global::System.Windows.Forms.ListView();
			this.contextMenuStrip1Doors = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuCheck = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuWarnOutputReset = new global::System.Windows.Forms.ToolStripMenuItem();
			this.locateToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.personInsideToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.resetPersonInsideToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.lEDConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.lCDShowToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.allowAntiPassbackFirstExitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enableAllowAntiPassbackFirstExitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableAllowAntiPassbackFirstExitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setDoorControlToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new global::System.Windows.Forms.ToolStripSeparator();
			this.switchViewStyleToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultStyleToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.quickFormatToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disablePCControlToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnuMonitorToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuAdjustTimeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuUploadToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuGetRecordsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemoteOpenToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.multithreadToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.downloadMultithreadToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.uploadMultithreadToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.remoteOpenMultithreadToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setDoorControlToolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.adjustTimeMultithreadToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.driverUpdateToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayCloudControllersToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.otherToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.qRInfoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.watchingDogConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.watchingDogDeactiveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.watchingDog15minToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.watchingDog30minToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.watchingDogCustomToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.normalOpenTimeListToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.swipeDataCenterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer2 = new global::System.Windows.Forms.SplitContainer();
			this.dgvRunInfo = new global::System.Windows.Forms.DataGridView();
			this.f_Category = new global::System.Windows.Forms.DataGridViewImageColumn();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Time = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Info = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Detail = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MjRecStr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip2RunInfo = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.clearRunInfoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayMoreSwipesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.lblInfoID = new global::System.Windows.Forms.Label();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.grpDetail = new global::System.Windows.Forms.GroupBox();
			this.txtInfo = new global::System.Windows.Forms.TextBox();
			this.richTxtInfo = new global::System.Windows.Forms.RichTextBox();
			this.dataGridView2 = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.timerUpdateDoorInfo = new global::System.Windows.Forms.Timer(this.components);
			this.bkUploadAndGetRecords = new global::System.ComponentModel.BackgroundWorker();
			this.bkDispDoorStatus = new global::System.ComponentModel.BackgroundWorker();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnWarnExisted = new global::System.Windows.Forms.ToolStripButton();
			this.btnSelectAll = new global::System.Windows.Forms.ToolStripButton();
			this.btnServer = new global::System.Windows.Forms.ToolStripButton();
			this.btnStopOthers = new global::System.Windows.Forms.ToolStripButton();
			this.btnCheck = new global::System.Windows.Forms.ToolStripButton();
			this.btnSetTime = new global::System.Windows.Forms.ToolStripButton();
			this.btnUpload = new global::System.Windows.Forms.ToolStripButton();
			this.btnGetRecords = new global::System.Windows.Forms.ToolStripButton();
			this.btnRealtimeGetRecords = new global::System.Windows.Forms.ToolStripButton();
			this.btnDownloadPrivileges = new global::System.Windows.Forms.ToolStripButton();
			this.btnRemoteOpen = new global::System.Windows.Forms.ToolStripButton();
			this.btnClearRunInfo = new global::System.Windows.Forms.ToolStripButton();
			this.btnLocate = new global::System.Windows.Forms.ToolStripButton();
			this.btnPersonInside = new global::System.Windows.Forms.ToolStripButton();
			this.btnMaps = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.cboZone = new global::System.Windows.Forms.ToolStripComboBox();
			this.btnStopMonitor = new global::System.Windows.Forms.ToolStripButton();
			this.timerWarn = new global::System.Windows.Forms.Timer(this.components);
			this.bkRealtimeGetRecords = new global::System.ComponentModel.BackgroundWorker();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timerAutoLogin = new global::System.Windows.Forms.Timer(this.components);
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.tcpServer1 = new global::WG3000_COMM.ExtendFunc.TCPServer.TcpServer(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.grpTool.SuspendLayout();
			this.contextMenuStrip1Doors.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).BeginInit();
			this.contextMenuStrip2RunInfo.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			this.grpDetail.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView2).BeginInit();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.splitContainer1.Name = "splitContainer1";
			componentResourceManager.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
			this.splitContainer1.Panel1.Controls.Add(this.cboUsers);
			this.splitContainer1.Panel1.Controls.Add(this.grpTool);
			this.splitContainer1.Panel1.Controls.Add(this.lstDoors);
			this.toolTip1.SetToolTip(this.splitContainer1.Panel1, componentResourceManager.GetString("splitContainer1.Panel1.ToolTip"));
			componentResourceManager.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.toolTip1.SetToolTip(this.splitContainer1.Panel2, componentResourceManager.GetString("splitContainer1.Panel2.ToolTip"));
			this.toolTip1.SetToolTip(this.splitContainer1, componentResourceManager.GetString("splitContainer1.ToolTip"));
			componentResourceManager.ApplyResources(this.cboUsers, "cboUsers");
			this.cboUsers.DropDownWidth = 200;
			this.cboUsers.FormattingEnabled = true;
			this.cboUsers.Name = "cboUsers";
			this.toolTip1.SetToolTip(this.cboUsers, componentResourceManager.GetString("cboUsers.ToolTip"));
			componentResourceManager.ApplyResources(this.grpTool, "grpTool");
			this.grpTool.Controls.Add(this.btnHideTools);
			this.grpTool.Controls.Add(this.chkDisplayNewestSwipe);
			this.grpTool.Controls.Add(this.chkNeedCheckLosePacket);
			this.grpTool.ForeColor = global::System.Drawing.Color.White;
			this.grpTool.Name = "grpTool";
			this.grpTool.TabStop = false;
			this.toolTip1.SetToolTip(this.grpTool, componentResourceManager.GetString("grpTool.ToolTip"));
			componentResourceManager.ApplyResources(this.btnHideTools, "btnHideTools");
			this.btnHideTools.ForeColor = global::System.Drawing.Color.Black;
			this.btnHideTools.Name = "btnHideTools";
			this.toolTip1.SetToolTip(this.btnHideTools, componentResourceManager.GetString("btnHideTools.ToolTip"));
			this.btnHideTools.UseVisualStyleBackColor = true;
			this.btnHideTools.Click += new global::System.EventHandler(this.btnHideTools_Click);
			componentResourceManager.ApplyResources(this.chkDisplayNewestSwipe, "chkDisplayNewestSwipe");
			this.chkDisplayNewestSwipe.Name = "chkDisplayNewestSwipe";
			this.toolTip1.SetToolTip(this.chkDisplayNewestSwipe, componentResourceManager.GetString("chkDisplayNewestSwipe.ToolTip"));
			this.chkDisplayNewestSwipe.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkNeedCheckLosePacket, "chkNeedCheckLosePacket");
			this.chkNeedCheckLosePacket.Name = "chkNeedCheckLosePacket";
			this.toolTip1.SetToolTip(this.chkNeedCheckLosePacket, componentResourceManager.GetString("chkNeedCheckLosePacket.ToolTip"));
			this.chkNeedCheckLosePacket.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.lstDoors, "lstDoors");
			this.lstDoors.BackColor = global::System.Drawing.SystemColors.Window;
			this.lstDoors.BackgroundImageTiled = true;
			this.lstDoors.ContextMenuStrip = this.contextMenuStrip1Doors;
			this.lstDoors.ForeColor = global::System.Drawing.SystemColors.WindowText;
			this.lstDoors.Name = "lstDoors";
			this.toolTip1.SetToolTip(this.lstDoors, componentResourceManager.GetString("lstDoors.ToolTip"));
			this.lstDoors.UseCompatibleStateImageBehavior = false;
			this.lstDoors.SelectedIndexChanged += new global::System.EventHandler(this.lstDoors_SelectedIndexChanged);
			this.lstDoors.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmConsole_KeyDown);
			this.lstDoors.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.frmConsole_MouseClick);
			componentResourceManager.ApplyResources(this.contextMenuStrip1Doors, "contextMenuStrip1Doors");
			this.contextMenuStrip1Doors.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.mnuCheck, this.mnuWarnOutputReset, this.locateToolStripMenuItem, this.personInsideToolStripMenuItem, this.resetPersonInsideToolStripMenuItem, this.lEDConfigureToolStripMenuItem, this.lCDShowToolStripMenuItem, this.allowAntiPassbackFirstExitToolStripMenuItem, this.setDoorControlToolStripMenuItem, this.toolStripSeparator4,
				this.switchViewStyleToolStripMenuItem, this.restoreDefaultStyleToolStripMenuItem, this.quickFormatToolStripMenuItem, this.disablePCControlToolStripMenuItem, this.toolStripSeparator3, this.mnuMonitorToolStripMenuItem, this.mnuAdjustTimeToolStripMenuItem, this.mnuUploadToolStripMenuItem, this.mnuGetRecordsToolStripMenuItem, this.mnuRemoteOpenToolStripMenuItem,
				this.multithreadToolStripMenuItem, this.driverUpdateToolStripMenuItem, this.displayCloudControllersToolStripMenuItem, this.toolStripSeparator1, this.otherToolStripMenuItem
			});
			this.contextMenuStrip1Doors.Name = "contextMenuStrip1";
			this.toolTip1.SetToolTip(this.contextMenuStrip1Doors, componentResourceManager.GetString("contextMenuStrip1Doors.ToolTip"));
			componentResourceManager.ApplyResources(this.mnuCheck, "mnuCheck");
			this.mnuCheck.Name = "mnuCheck";
			this.mnuCheck.Click += new global::System.EventHandler(this.mnuCheck_Click);
			componentResourceManager.ApplyResources(this.mnuWarnOutputReset, "mnuWarnOutputReset");
			this.mnuWarnOutputReset.Name = "mnuWarnOutputReset";
			this.mnuWarnOutputReset.Click += new global::System.EventHandler(this.mnuWarnReset_Click);
			componentResourceManager.ApplyResources(this.locateToolStripMenuItem, "locateToolStripMenuItem");
			this.locateToolStripMenuItem.Name = "locateToolStripMenuItem";
			this.locateToolStripMenuItem.Click += new global::System.EventHandler(this.locateToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.personInsideToolStripMenuItem, "personInsideToolStripMenuItem");
			this.personInsideToolStripMenuItem.Name = "personInsideToolStripMenuItem";
			this.personInsideToolStripMenuItem.Click += new global::System.EventHandler(this.personInsideToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.resetPersonInsideToolStripMenuItem, "resetPersonInsideToolStripMenuItem");
			this.resetPersonInsideToolStripMenuItem.Name = "resetPersonInsideToolStripMenuItem";
			this.resetPersonInsideToolStripMenuItem.Click += new global::System.EventHandler(this.resetPersonInsideToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.lEDConfigureToolStripMenuItem, "lEDConfigureToolStripMenuItem");
			this.lEDConfigureToolStripMenuItem.Name = "lEDConfigureToolStripMenuItem";
			this.lEDConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.lEDConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.lCDShowToolStripMenuItem, "lCDShowToolStripMenuItem");
			this.lCDShowToolStripMenuItem.Name = "lCDShowToolStripMenuItem";
			this.lCDShowToolStripMenuItem.Click += new global::System.EventHandler(this.lCDShowToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.allowAntiPassbackFirstExitToolStripMenuItem, "allowAntiPassbackFirstExitToolStripMenuItem");
			this.allowAntiPassbackFirstExitToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.enableAllowAntiPassbackFirstExitToolStripMenuItem, this.disableAllowAntiPassbackFirstExitToolStripMenuItem });
			this.allowAntiPassbackFirstExitToolStripMenuItem.Name = "allowAntiPassbackFirstExitToolStripMenuItem";
			componentResourceManager.ApplyResources(this.enableAllowAntiPassbackFirstExitToolStripMenuItem, "enableAllowAntiPassbackFirstExitToolStripMenuItem");
			this.enableAllowAntiPassbackFirstExitToolStripMenuItem.Name = "enableAllowAntiPassbackFirstExitToolStripMenuItem";
			this.enableAllowAntiPassbackFirstExitToolStripMenuItem.Click += new global::System.EventHandler(this.allowAntiPassbackFirstExitToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.disableAllowAntiPassbackFirstExitToolStripMenuItem, "disableAllowAntiPassbackFirstExitToolStripMenuItem");
			this.disableAllowAntiPassbackFirstExitToolStripMenuItem.Name = "disableAllowAntiPassbackFirstExitToolStripMenuItem";
			this.disableAllowAntiPassbackFirstExitToolStripMenuItem.Click += new global::System.EventHandler(this.allowAntiPassbackFirstExitToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setDoorControlToolStripMenuItem, "setDoorControlToolStripMenuItem");
			this.setDoorControlToolStripMenuItem.Name = "setDoorControlToolStripMenuItem";
			this.setDoorControlToolStripMenuItem.Click += new global::System.EventHandler(this.setDoorControlToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			componentResourceManager.ApplyResources(this.switchViewStyleToolStripMenuItem, "switchViewStyleToolStripMenuItem");
			this.switchViewStyleToolStripMenuItem.Name = "switchViewStyleToolStripMenuItem";
			this.switchViewStyleToolStripMenuItem.Click += new global::System.EventHandler(this.switchViewStyleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultStyleToolStripMenuItem, "restoreDefaultStyleToolStripMenuItem");
			this.restoreDefaultStyleToolStripMenuItem.Name = "restoreDefaultStyleToolStripMenuItem";
			this.restoreDefaultStyleToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultStyleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.quickFormatToolStripMenuItem, "quickFormatToolStripMenuItem");
			this.quickFormatToolStripMenuItem.Name = "quickFormatToolStripMenuItem";
			this.quickFormatToolStripMenuItem.Click += new global::System.EventHandler(this.quickFormatToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.disablePCControlToolStripMenuItem, "disablePCControlToolStripMenuItem");
			this.disablePCControlToolStripMenuItem.Name = "disablePCControlToolStripMenuItem";
			this.disablePCControlToolStripMenuItem.Click += new global::System.EventHandler(this.disablePCControlToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			componentResourceManager.ApplyResources(this.mnuMonitorToolStripMenuItem, "mnuMonitorToolStripMenuItem");
			this.mnuMonitorToolStripMenuItem.Name = "mnuMonitorToolStripMenuItem";
			this.mnuMonitorToolStripMenuItem.Click += new global::System.EventHandler(this.mnuMonitorToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.mnuAdjustTimeToolStripMenuItem, "mnuAdjustTimeToolStripMenuItem");
			this.mnuAdjustTimeToolStripMenuItem.Name = "mnuAdjustTimeToolStripMenuItem";
			this.mnuAdjustTimeToolStripMenuItem.Click += new global::System.EventHandler(this.mnuAdjustTimeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.mnuUploadToolStripMenuItem, "mnuUploadToolStripMenuItem");
			this.mnuUploadToolStripMenuItem.Name = "mnuUploadToolStripMenuItem";
			this.mnuUploadToolStripMenuItem.Click += new global::System.EventHandler(this.mnuUploadToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.mnuGetRecordsToolStripMenuItem, "mnuGetRecordsToolStripMenuItem");
			this.mnuGetRecordsToolStripMenuItem.Name = "mnuGetRecordsToolStripMenuItem";
			this.mnuGetRecordsToolStripMenuItem.Click += new global::System.EventHandler(this.mnuGetRecordsToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.mnuRemoteOpenToolStripMenuItem, "mnuRemoteOpenToolStripMenuItem");
			this.mnuRemoteOpenToolStripMenuItem.Name = "mnuRemoteOpenToolStripMenuItem";
			this.mnuRemoteOpenToolStripMenuItem.Click += new global::System.EventHandler(this.remoteOpenToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.multithreadToolStripMenuItem, "multithreadToolStripMenuItem");
			this.multithreadToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.downloadMultithreadToolStripMenuItem, this.uploadMultithreadToolStripMenuItem, this.remoteOpenMultithreadToolStripMenuItem, this.setDoorControlToolStripMenuItem1, this.adjustTimeMultithreadToolStripMenuItem });
			this.multithreadToolStripMenuItem.Name = "multithreadToolStripMenuItem";
			componentResourceManager.ApplyResources(this.downloadMultithreadToolStripMenuItem, "downloadMultithreadToolStripMenuItem");
			this.downloadMultithreadToolStripMenuItem.Name = "downloadMultithreadToolStripMenuItem";
			this.downloadMultithreadToolStripMenuItem.Click += new global::System.EventHandler(this.downloadMultithreadToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.uploadMultithreadToolStripMenuItem, "uploadMultithreadToolStripMenuItem");
			this.uploadMultithreadToolStripMenuItem.Name = "uploadMultithreadToolStripMenuItem";
			this.uploadMultithreadToolStripMenuItem.Click += new global::System.EventHandler(this.uploadMultithreadToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.remoteOpenMultithreadToolStripMenuItem, "remoteOpenMultithreadToolStripMenuItem");
			this.remoteOpenMultithreadToolStripMenuItem.Name = "remoteOpenMultithreadToolStripMenuItem";
			this.remoteOpenMultithreadToolStripMenuItem.Click += new global::System.EventHandler(this.remoteOpenMultithreadToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setDoorControlToolStripMenuItem1, "setDoorControlToolStripMenuItem1");
			this.setDoorControlToolStripMenuItem1.Name = "setDoorControlToolStripMenuItem1";
			this.setDoorControlToolStripMenuItem1.Click += new global::System.EventHandler(this.setDoorControlToolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.adjustTimeMultithreadToolStripMenuItem, "adjustTimeMultithreadToolStripMenuItem");
			this.adjustTimeMultithreadToolStripMenuItem.Name = "adjustTimeMultithreadToolStripMenuItem";
			this.adjustTimeMultithreadToolStripMenuItem.Click += new global::System.EventHandler(this.adjustTimeMultithreadToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.driverUpdateToolStripMenuItem, "driverUpdateToolStripMenuItem");
			this.driverUpdateToolStripMenuItem.Name = "driverUpdateToolStripMenuItem";
			this.driverUpdateToolStripMenuItem.Click += new global::System.EventHandler(this.upDrv_Click);
			componentResourceManager.ApplyResources(this.displayCloudControllersToolStripMenuItem, "displayCloudControllersToolStripMenuItem");
			this.displayCloudControllersToolStripMenuItem.Name = "displayCloudControllersToolStripMenuItem";
			this.displayCloudControllersToolStripMenuItem.Click += new global::System.EventHandler(this.displayCloudControllersToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.otherToolStripMenuItem, "otherToolStripMenuItem");
			this.otherToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.qRInfoToolStripMenuItem, this.watchingDogConfigureToolStripMenuItem, this.normalOpenTimeListToolStripMenuItem, this.swipeDataCenterToolStripMenuItem });
			this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
			componentResourceManager.ApplyResources(this.qRInfoToolStripMenuItem, "qRInfoToolStripMenuItem");
			this.qRInfoToolStripMenuItem.Name = "qRInfoToolStripMenuItem";
			this.qRInfoToolStripMenuItem.Click += new global::System.EventHandler(this.qRInfoToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.watchingDogConfigureToolStripMenuItem, "watchingDogConfigureToolStripMenuItem");
			this.watchingDogConfigureToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.watchingDogDeactiveToolStripMenuItem, this.watchingDog15minToolStripMenuItem, this.watchingDog30minToolStripMenuItem, this.watchingDogCustomToolStripMenuItem });
			this.watchingDogConfigureToolStripMenuItem.Name = "watchingDogConfigureToolStripMenuItem";
			this.watchingDogConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.watchingDogConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.watchingDogDeactiveToolStripMenuItem, "watchingDogDeactiveToolStripMenuItem");
			this.watchingDogDeactiveToolStripMenuItem.Name = "watchingDogDeactiveToolStripMenuItem";
			this.watchingDogDeactiveToolStripMenuItem.Click += new global::System.EventHandler(this.watchingDogConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.watchingDog15minToolStripMenuItem, "watchingDog15minToolStripMenuItem");
			this.watchingDog15minToolStripMenuItem.Name = "watchingDog15minToolStripMenuItem";
			this.watchingDog15minToolStripMenuItem.Click += new global::System.EventHandler(this.watchingDogConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.watchingDog30minToolStripMenuItem, "watchingDog30minToolStripMenuItem");
			this.watchingDog30minToolStripMenuItem.Name = "watchingDog30minToolStripMenuItem";
			this.watchingDog30minToolStripMenuItem.Click += new global::System.EventHandler(this.watchingDogConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.watchingDogCustomToolStripMenuItem, "watchingDogCustomToolStripMenuItem");
			this.watchingDogCustomToolStripMenuItem.Name = "watchingDogCustomToolStripMenuItem";
			this.watchingDogCustomToolStripMenuItem.Click += new global::System.EventHandler(this.watchingDogConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.normalOpenTimeListToolStripMenuItem, "normalOpenTimeListToolStripMenuItem");
			this.normalOpenTimeListToolStripMenuItem.Name = "normalOpenTimeListToolStripMenuItem";
			this.normalOpenTimeListToolStripMenuItem.Click += new global::System.EventHandler(this.normalOpenTimeListToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.swipeDataCenterToolStripMenuItem, "swipeDataCenterToolStripMenuItem");
			this.swipeDataCenterToolStripMenuItem.Name = "swipeDataCenterToolStripMenuItem";
			this.swipeDataCenterToolStripMenuItem.Click += new global::System.EventHandler(this.swipeDataCenterToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.splitContainer2, "splitContainer2");
			this.splitContainer2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.splitContainer2.Name = "splitContainer2";
			componentResourceManager.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
			this.splitContainer2.Panel1.Controls.Add(this.dgvRunInfo);
			this.toolTip1.SetToolTip(this.splitContainer2.Panel1, componentResourceManager.GetString("splitContainer2.Panel1.ToolTip"));
			componentResourceManager.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
			this.splitContainer2.Panel2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.splitContainer2.Panel2.Controls.Add(this.lblInfoID);
			this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
			this.splitContainer2.Panel2.Controls.Add(this.grpDetail);
			this.splitContainer2.Panel2.Controls.Add(this.richTxtInfo);
			this.splitContainer2.Panel2.Controls.Add(this.dataGridView2);
			this.toolTip1.SetToolTip(this.splitContainer2.Panel2, componentResourceManager.GetString("splitContainer2.Panel2.ToolTip"));
			this.toolTip1.SetToolTip(this.splitContainer2, componentResourceManager.GetString("splitContainer2.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvRunInfo, "dgvRunInfo");
			this.dgvRunInfo.AllowUserToAddRows = false;
			this.dgvRunInfo.AllowUserToDeleteRows = false;
			this.dgvRunInfo.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Padding = new global::System.Windows.Forms.Padding(0, 0, 0, 2);
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvRunInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvRunInfo.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvRunInfo.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_Category, this.f_RecID, this.f_Time, this.f_Desc, this.f_Info, this.f_Detail, this.f_MjRecStr });
			this.dgvRunInfo.ContextMenuStrip = this.contextMenuStrip2RunInfo;
			this.dgvRunInfo.EditMode = global::System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dgvRunInfo.EnableHeadersVisualStyles = false;
			this.dgvRunInfo.MultiSelect = false;
			this.dgvRunInfo.Name = "dgvRunInfo";
			this.dgvRunInfo.ReadOnly = true;
			this.dgvRunInfo.RowHeadersVisible = false;
			this.dgvRunInfo.RowTemplate.Height = 23;
			this.dgvRunInfo.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvRunInfo.ShowCellErrors = false;
			this.dgvRunInfo.ShowCellToolTips = false;
			this.dgvRunInfo.ShowEditingIcon = false;
			this.dgvRunInfo.ShowRowErrors = false;
			this.toolTip1.SetToolTip(this.dgvRunInfo, componentResourceManager.GetString("dgvRunInfo.ToolTip"));
			this.dgvRunInfo.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRunInfo_CellFormatting);
			this.dgvRunInfo.DataError += new global::System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvRunInfo_DataError);
			this.dgvRunInfo.SelectionChanged += new global::System.EventHandler(this.dgvRunInfo_SelectionChanged);
			this.dgvRunInfo.Click += new global::System.EventHandler(this.dgvRunInfo_Click);
			componentResourceManager.ApplyResources(this.f_Category, "f_Category");
			this.f_Category.Name = "f_Category";
			this.f_Category.ReadOnly = true;
			this.f_Category.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_Category.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Time, "f_Time");
			this.f_Time.Name = "f_Time";
			this.f_Time.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc, "f_Desc");
			this.f_Desc.Name = "f_Desc";
			this.f_Desc.ReadOnly = true;
			this.f_Info.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Info, "f_Info");
			this.f_Info.Name = "f_Info";
			this.f_Info.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Detail, "f_Detail");
			this.f_Detail.Name = "f_Detail";
			this.f_Detail.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MjRecStr, "f_MjRecStr");
			this.f_MjRecStr.Name = "f_MjRecStr";
			this.f_MjRecStr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip2RunInfo, "contextMenuStrip2RunInfo");
			this.contextMenuStrip2RunInfo.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.clearRunInfoToolStripMenuItem, this.displayMoreSwipesToolStripMenuItem });
			this.contextMenuStrip2RunInfo.Name = "contextMenuStrip2RunInfo";
			this.toolTip1.SetToolTip(this.contextMenuStrip2RunInfo, componentResourceManager.GetString("contextMenuStrip2RunInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.clearRunInfoToolStripMenuItem, "clearRunInfoToolStripMenuItem");
			this.clearRunInfoToolStripMenuItem.Name = "clearRunInfoToolStripMenuItem";
			this.clearRunInfoToolStripMenuItem.Click += new global::System.EventHandler(this.clearRunInfoToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayMoreSwipesToolStripMenuItem, "displayMoreSwipesToolStripMenuItem");
			this.displayMoreSwipesToolStripMenuItem.Name = "displayMoreSwipesToolStripMenuItem";
			this.displayMoreSwipesToolStripMenuItem.Click += new global::System.EventHandler(this.displayMoreSwipesToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.lblInfoID, "lblInfoID");
			this.lblInfoID.BackColor = global::System.Drawing.Color.White;
			this.lblInfoID.ForeColor = global::System.Drawing.Color.Black;
			this.lblInfoID.Name = "lblInfoID";
			this.toolTip1.SetToolTip(this.lblInfoID, componentResourceManager.GetString("lblInfoID.ToolTip"));
			componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.BackColor = global::System.Drawing.Color.White;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox1, componentResourceManager.GetString("pictureBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.grpDetail, "grpDetail");
			this.grpDetail.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			this.grpDetail.Controls.Add(this.txtInfo);
			this.grpDetail.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.grpDetail.ForeColor = global::System.Drawing.Color.Transparent;
			this.grpDetail.Name = "grpDetail";
			this.grpDetail.TabStop = false;
			this.toolTip1.SetToolTip(this.grpDetail, componentResourceManager.GetString("grpDetail.ToolTip"));
			componentResourceManager.ApplyResources(this.txtInfo, "txtInfo");
			this.txtInfo.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.txtInfo.ForeColor = global::System.Drawing.SystemColors.WindowText;
			this.txtInfo.Name = "txtInfo";
			this.toolTip1.SetToolTip(this.txtInfo, componentResourceManager.GetString("txtInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.richTxtInfo, "richTxtInfo");
			this.richTxtInfo.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.richTxtInfo.Name = "richTxtInfo";
			this.toolTip1.SetToolTip(this.richTxtInfo, componentResourceManager.GetString("richTxtInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.dataGridView2, "dataGridView2");
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.BackgroundColor = global::System.Drawing.SystemColors.Window;
			this.dataGridView2.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.Padding = new global::System.Windows.Forms.Padding(0, 0, 0, 2);
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView2.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView2.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1 });
			this.dataGridView2.EnableHeadersVisualStyles = false;
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.ReadOnly = true;
			this.dataGridView2.RowHeadersVisible = false;
			this.dataGridView2.RowTemplate.Height = 23;
			this.toolTip1.SetToolTip(this.dataGridView2, componentResourceManager.GetString("dataGridView2.ToolTip"));
			this.dataGridViewTextBoxColumn1.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.timerUpdateDoorInfo.Interval = 200;
			this.timerUpdateDoorInfo.Tick += new global::System.EventHandler(this.timerUpdateDoorInfo_Tick);
			this.bkUploadAndGetRecords.WorkerSupportsCancellation = true;
			this.bkUploadAndGetRecords.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.bkUploadAndGetRecords_DoWork);
			this.bkUploadAndGetRecords.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.bkUploadAndGetRecords_RunWorkerCompleted);
			this.bkDispDoorStatus.WorkerSupportsCancellation = true;
			this.bkDispDoorStatus.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.bkDispDoorStatus_DoWork);
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.btnWarnExisted, this.btnSelectAll, this.btnServer, this.btnStopOthers, this.btnCheck, this.btnSetTime, this.btnUpload, this.btnGetRecords, this.btnRealtimeGetRecords, this.btnDownloadPrivileges,
				this.btnRemoteOpen, this.btnClearRunInfo, this.btnLocate, this.btnPersonInside, this.btnMaps, this.btnFind, this.cboZone, this.btnStopMonitor
			});
			this.toolStrip1.Name = "toolStrip1";
			this.toolTip1.SetToolTip(this.toolStrip1, componentResourceManager.GetString("toolStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnWarnExisted, "btnWarnExisted");
			this.btnWarnExisted.BackColor = global::System.Drawing.Color.Red;
			this.btnWarnExisted.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnWarnExisted.ForeColor = global::System.Drawing.Color.White;
			this.btnWarnExisted.Name = "btnWarnExisted";
			this.btnWarnExisted.Click += new global::System.EventHandler(this.btnWarnExisted_Click);
			componentResourceManager.ApplyResources(this.btnSelectAll, "btnSelectAll");
			this.btnSelectAll.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectAll.Image = global::WG3000_COMM.Properties.Resources.pConsole_SelectAll;
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Click += new global::System.EventHandler(this.btnSelectAll_Click);
			componentResourceManager.ApplyResources(this.btnServer, "btnServer");
			this.btnServer.BackColor = global::System.Drawing.Color.Transparent;
			this.btnServer.ForeColor = global::System.Drawing.Color.White;
			this.btnServer.Image = global::WG3000_COMM.Properties.Resources.pConsole_Monitor;
			this.btnServer.Name = "btnServer";
			this.btnServer.Click += new global::System.EventHandler(this.btnServer_Click);
			componentResourceManager.ApplyResources(this.btnStopOthers, "btnStopOthers");
			this.btnStopOthers.ForeColor = global::System.Drawing.Color.White;
			this.btnStopOthers.Image = global::WG3000_COMM.Properties.Resources.pConsole_Stop;
			this.btnStopOthers.Name = "btnStopOthers";
			this.btnStopOthers.Click += new global::System.EventHandler(this.btnStopOthers_Click);
			componentResourceManager.ApplyResources(this.btnCheck, "btnCheck");
			this.btnCheck.ForeColor = global::System.Drawing.Color.White;
			this.btnCheck.Image = global::WG3000_COMM.Properties.Resources.pConsole_CheckController;
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Click += new global::System.EventHandler(this.btnCheck_Click);
			componentResourceManager.ApplyResources(this.btnSetTime, "btnSetTime");
			this.btnSetTime.ForeColor = global::System.Drawing.Color.White;
			this.btnSetTime.Image = global::WG3000_COMM.Properties.Resources.pChild_AdjustTime;
			this.btnSetTime.Name = "btnSetTime";
			this.btnSetTime.Click += new global::System.EventHandler(this.btnSetTime_Click);
			componentResourceManager.ApplyResources(this.btnUpload, "btnUpload");
			this.btnUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnUpload.Image = global::WG3000_COMM.Properties.Resources.pConsole_Upload;
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Click += new global::System.EventHandler(this.btnUpload_Click);
			componentResourceManager.ApplyResources(this.btnGetRecords, "btnGetRecords");
			this.btnGetRecords.ForeColor = global::System.Drawing.Color.White;
			this.btnGetRecords.Image = global::WG3000_COMM.Properties.Resources.pConsole_GetRecords;
			this.btnGetRecords.Name = "btnGetRecords";
			this.btnGetRecords.Click += new global::System.EventHandler(this.btnGetRecords_Click);
			componentResourceManager.ApplyResources(this.btnRealtimeGetRecords, "btnRealtimeGetRecords");
			this.btnRealtimeGetRecords.ForeColor = global::System.Drawing.Color.White;
			this.btnRealtimeGetRecords.Image = global::WG3000_COMM.Properties.Resources.pConsole_RealtimeGetRecords;
			this.btnRealtimeGetRecords.Name = "btnRealtimeGetRecords";
			this.btnRealtimeGetRecords.Click += new global::System.EventHandler(this.btnRealtimeGetRecords_Click);
			componentResourceManager.ApplyResources(this.btnDownloadPrivileges, "btnDownloadPrivileges");
			this.btnDownloadPrivileges.ForeColor = global::System.Drawing.Color.White;
			this.btnDownloadPrivileges.Image = global::WG3000_COMM.Properties.Resources.pTools_Add_Auto;
			this.btnDownloadPrivileges.Name = "btnDownloadPrivileges";
			this.btnDownloadPrivileges.Click += new global::System.EventHandler(this.downloadPrivilegesToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.btnRemoteOpen, "btnRemoteOpen");
			this.btnRemoteOpen.ForeColor = global::System.Drawing.Color.White;
			this.btnRemoteOpen.Image = global::WG3000_COMM.Properties.Resources.pConsole_OpenDoor;
			this.btnRemoteOpen.Name = "btnRemoteOpen";
			this.btnRemoteOpen.Click += new global::System.EventHandler(this.btnRemoteOpen_Click);
			componentResourceManager.ApplyResources(this.btnClearRunInfo, "btnClearRunInfo");
			this.btnClearRunInfo.ForeColor = global::System.Drawing.Color.White;
			this.btnClearRunInfo.Image = global::WG3000_COMM.Properties.Resources.pTools_Clear_Condition;
			this.btnClearRunInfo.Name = "btnClearRunInfo";
			this.btnClearRunInfo.Click += new global::System.EventHandler(this.btnClearRunInfo_Click);
			componentResourceManager.ApplyResources(this.btnLocate, "btnLocate");
			this.btnLocate.ForeColor = global::System.Drawing.Color.White;
			this.btnLocate.Image = global::WG3000_COMM.Properties.Resources.pTools_TypeSetup;
			this.btnLocate.Name = "btnLocate";
			this.btnLocate.Click += new global::System.EventHandler(this.locateToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.btnPersonInside, "btnPersonInside");
			this.btnPersonInside.ForeColor = global::System.Drawing.Color.White;
			this.btnPersonInside.Image = global::WG3000_COMM.Properties.Resources.pTools_SetPwd;
			this.btnPersonInside.Name = "btnPersonInside";
			this.btnPersonInside.Click += new global::System.EventHandler(this.personInsideToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.btnMaps, "btnMaps");
			this.btnMaps.BackColor = global::System.Drawing.Color.Transparent;
			this.btnMaps.ForeColor = global::System.Drawing.Color.White;
			this.btnMaps.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps;
			this.btnMaps.Name = "btnMaps";
			this.btnMaps.Click += new global::System.EventHandler(this.btnMaps_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.cboZone, "cboZone");
			this.cboZone.AutoToolTip = true;
			this.cboZone.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboZone.Name = "cboZone";
			this.cboZone.DropDown += new global::System.EventHandler(this.cboZone_DropDown);
			this.cboZone.SelectedIndexChanged += new global::System.EventHandler(this.cboZone_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.btnStopMonitor, "btnStopMonitor");
			this.btnStopMonitor.ForeColor = global::System.Drawing.Color.White;
			this.btnStopMonitor.Image = global::WG3000_COMM.Properties.Resources.pConsole_Stop;
			this.btnStopMonitor.Name = "btnStopMonitor";
			this.btnStopMonitor.Click += new global::System.EventHandler(this.btnStopMonitor_Click);
			this.timerWarn.Interval = 500;
			this.timerWarn.Tick += new global::System.EventHandler(this.timerWarn_Tick);
			this.bkRealtimeGetRecords.WorkerSupportsCancellation = true;
			this.bkRealtimeGetRecords.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.bkRealtimeGetRecords_DoWork);
			this.bkRealtimeGetRecords.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.bkRealtimeGetRecords_RunWorkerCompleted);
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timerAutoLogin.Interval = 500;
			this.timerAutoLogin.Tick += new global::System.EventHandler(this.timerAutoLogin_Tick);
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			this.tcpServer1.Encoding = (global::System.Text.Encoding)componentResourceManager.GetObject("tcpServer1.Encoding");
			this.tcpServer1.IdleTime = 50;
			this.tcpServer1.IsOpen = false;
			this.tcpServer1.MaxCallbackThreads = 100;
			this.tcpServer1.MaxSendAttempts = 3;
			this.tcpServer1.Port = -1;
			this.tcpServer1.VerifyConnectionInterval = 100;
			this.tcpServer1.OnConnect += new global::WG3000_COMM.ExtendFunc.TCPServer.tcpServerConnectionChanged(this.tcpServer1_OnConnect);
			this.tcpServer1.OnDataAvailable += new global::WG3000_COMM.ExtendFunc.TCPServer.tcpServerConnectionChanged(this.tcpServer1_OnDataAvailable);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.White;
			base.Controls.Add(this.splitContainer1);
			base.Controls.Add(this.toolStrip1);
			this.DoubleBuffered = true;
			base.KeyPreview = true;
			base.Name = "frmConsole";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmConsole_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.frmConsole_FormClosed);
			base.Load += new global::System.EventHandler(this.frmConsole_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmConsole_KeyDown);
			base.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.frmConsole_MouseClick);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.grpTool.ResumeLayout(false);
			this.grpTool.PerformLayout();
			this.contextMenuStrip1Doors.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).EndInit();
			this.contextMenuStrip2RunInfo.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			this.grpDetail.ResumeLayout(false);
			this.grpDetail.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView2).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400096D RID: 2413
		private global::WG3000_COMM.DataOper.icController control4btnServer;

		// Token: 0x0400096E RID: 2414
		private global::WG3000_COMM.DataOper.icController control4Check;

		// Token: 0x0400096F RID: 2415
		private global::WG3000_COMM.DataOper.icController control4getRecordsFromController;

		// Token: 0x04000970 RID: 2416
		private global::WG3000_COMM.DataOper.icController control4Realtime;

		// Token: 0x04000993 RID: 2451
		private global::WG3000_COMM.Core.WatchingService watching;

		// Token: 0x040009B8 RID: 2488
		private global::WG3000_COMM.DataOper.icController control4uploadPrivilege = new global::WG3000_COMM.DataOper.icController();

		// Token: 0x040009BF RID: 2495
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x040009D5 RID: 2517
		private global::WG3000_COMM.DataOper.icPrivilege pr4uploadPrivilege = new global::WG3000_COMM.DataOper.icPrivilege();

		// Token: 0x040009E3 RID: 2531
		private global::WG3000_COMM.DataOper.icSwipeRecord swipe4GetRecords = new global::WG3000_COMM.DataOper.icSwipeRecord();

		// Token: 0x040009E8 RID: 2536
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040009E9 RID: 2537
		private global::System.Windows.Forms.ToolStripMenuItem adjustTimeMultithreadToolStripMenuItem;

		// Token: 0x040009EA RID: 2538
		private global::System.Windows.Forms.ToolStripMenuItem allowAntiPassbackFirstExitToolStripMenuItem;

		// Token: 0x040009EB RID: 2539
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040009EC RID: 2540
		private global::System.ComponentModel.BackgroundWorker bkDispDoorStatus;

		// Token: 0x040009ED RID: 2541
		private global::System.ComponentModel.BackgroundWorker bkRealtimeGetRecords;

		// Token: 0x040009EE RID: 2542
		private global::System.ComponentModel.BackgroundWorker bkUploadAndGetRecords;

		// Token: 0x040009EF RID: 2543
		private global::System.Windows.Forms.ToolStripButton btnClearRunInfo;

		// Token: 0x040009F0 RID: 2544
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x040009F1 RID: 2545
		private global::System.Windows.Forms.Button btnHideTools;

		// Token: 0x040009F2 RID: 2546
		private global::System.Windows.Forms.ToolStripButton btnSelectAll;

		// Token: 0x040009F3 RID: 2547
		private global::System.Windows.Forms.ToolStripButton btnStopMonitor;

		// Token: 0x040009F4 RID: 2548
		private global::System.Windows.Forms.ToolStripButton btnWarnExisted;

		// Token: 0x040009F5 RID: 2549
		private global::System.Windows.Forms.ComboBox cboUsers;

		// Token: 0x040009F6 RID: 2550
		private global::System.Windows.Forms.ToolStripComboBox cboZone;

		// Token: 0x040009F7 RID: 2551
		private global::System.Windows.Forms.CheckBox chkDisplayNewestSwipe;

		// Token: 0x040009F8 RID: 2552
		private global::System.Windows.Forms.CheckBox chkNeedCheckLosePacket;

		// Token: 0x040009F9 RID: 2553
		private global::System.Windows.Forms.ToolStripMenuItem clearRunInfoToolStripMenuItem;

		// Token: 0x040009FA RID: 2554
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1Doors;

		// Token: 0x040009FB RID: 2555
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip2RunInfo;

		// Token: 0x040009FC RID: 2556
		private global::System.Windows.Forms.DataGridView dataGridView2;

		// Token: 0x040009FD RID: 2557
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x040009FE RID: 2558
		private global::System.Windows.Forms.DataGridView dgvRunInfo;

		// Token: 0x040009FF RID: 2559
		private global::System.Windows.Forms.ToolStripMenuItem disableAllowAntiPassbackFirstExitToolStripMenuItem;

		// Token: 0x04000A00 RID: 2560
		private global::System.Windows.Forms.ToolStripMenuItem disablePCControlToolStripMenuItem;

		// Token: 0x04000A01 RID: 2561
		private global::System.Windows.Forms.ToolStripMenuItem displayCloudControllersToolStripMenuItem;

		// Token: 0x04000A02 RID: 2562
		private global::System.Windows.Forms.ToolStripMenuItem displayMoreSwipesToolStripMenuItem;

		// Token: 0x04000A03 RID: 2563
		private global::System.Windows.Forms.ToolStripMenuItem downloadMultithreadToolStripMenuItem;

		// Token: 0x04000A04 RID: 2564
		private global::System.Windows.Forms.ToolStripMenuItem driverUpdateToolStripMenuItem;

		// Token: 0x04000A05 RID: 2565
		private global::System.Windows.Forms.ToolStripMenuItem enableAllowAntiPassbackFirstExitToolStripMenuItem;

		// Token: 0x04000A06 RID: 2566
		private global::System.Windows.Forms.DataGridViewImageColumn f_Category;

		// Token: 0x04000A07 RID: 2567
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc;

		// Token: 0x04000A08 RID: 2568
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Detail;

		// Token: 0x04000A09 RID: 2569
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Info;

		// Token: 0x04000A0A RID: 2570
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MjRecStr;

		// Token: 0x04000A0B RID: 2571
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x04000A0C RID: 2572
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Time;

		// Token: 0x04000A0D RID: 2573
		private global::System.Windows.Forms.GroupBox grpDetail;

		// Token: 0x04000A0E RID: 2574
		private global::System.Windows.Forms.GroupBox grpTool;

		// Token: 0x04000A0F RID: 2575
		private global::System.Windows.Forms.Label lblInfoID;

		// Token: 0x04000A10 RID: 2576
		private global::System.Windows.Forms.ToolStripMenuItem lCDShowToolStripMenuItem;

		// Token: 0x04000A11 RID: 2577
		private global::System.Windows.Forms.ToolStripMenuItem lEDConfigureToolStripMenuItem;

		// Token: 0x04000A12 RID: 2578
		private global::System.Windows.Forms.ToolStripMenuItem locateToolStripMenuItem;

		// Token: 0x04000A13 RID: 2579
		private global::System.Windows.Forms.ToolStripMenuItem mnuAdjustTimeToolStripMenuItem;

		// Token: 0x04000A14 RID: 2580
		private global::System.Windows.Forms.ToolStripMenuItem mnuCheck;

		// Token: 0x04000A15 RID: 2581
		private global::System.Windows.Forms.ToolStripMenuItem mnuGetRecordsToolStripMenuItem;

		// Token: 0x04000A16 RID: 2582
		private global::System.Windows.Forms.ToolStripMenuItem mnuMonitorToolStripMenuItem;

		// Token: 0x04000A17 RID: 2583
		private global::System.Windows.Forms.ToolStripMenuItem mnuRemoteOpenToolStripMenuItem;

		// Token: 0x04000A18 RID: 2584
		private global::System.Windows.Forms.ToolStripMenuItem mnuUploadToolStripMenuItem;

		// Token: 0x04000A19 RID: 2585
		private global::System.Windows.Forms.ToolStripMenuItem multithreadToolStripMenuItem;

		// Token: 0x04000A1A RID: 2586
		private global::System.Windows.Forms.ToolStripMenuItem normalOpenTimeListToolStripMenuItem;

		// Token: 0x04000A1B RID: 2587
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04000A1C RID: 2588
		private global::System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;

		// Token: 0x04000A1D RID: 2589
		private global::System.Windows.Forms.ToolStripMenuItem personInsideToolStripMenuItem;

		// Token: 0x04000A1E RID: 2590
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000A1F RID: 2591
		private global::System.Windows.Forms.ToolStripMenuItem qRInfoToolStripMenuItem;

		// Token: 0x04000A20 RID: 2592
		private global::System.Windows.Forms.ToolStripMenuItem quickFormatToolStripMenuItem;

		// Token: 0x04000A21 RID: 2593
		private global::System.Windows.Forms.ToolStripMenuItem remoteOpenMultithreadToolStripMenuItem;

		// Token: 0x04000A22 RID: 2594
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultStyleToolStripMenuItem;

		// Token: 0x04000A23 RID: 2595
		private global::System.Windows.Forms.RichTextBox richTxtInfo;

		// Token: 0x04000A24 RID: 2596
		private global::System.Windows.Forms.ToolStripMenuItem setDoorControlToolStripMenuItem1;

		// Token: 0x04000A25 RID: 2597
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x04000A26 RID: 2598
		private global::System.Windows.Forms.SplitContainer splitContainer2;

		// Token: 0x04000A28 RID: 2600
		private global::System.Windows.Forms.ToolStripMenuItem swipeDataCenterToolStripMenuItem;

		// Token: 0x04000A29 RID: 2601
		private global::System.Windows.Forms.ToolStripMenuItem switchViewStyleToolStripMenuItem;

		// Token: 0x04000A2A RID: 2602
		private global::System.Windows.Forms.Timer timerAutoLogin;

		// Token: 0x04000A2B RID: 2603
		private global::System.Windows.Forms.Timer timerUpdateDoorInfo;

		// Token: 0x04000A2C RID: 2604
		private global::System.Windows.Forms.Timer timerWarn;

		// Token: 0x04000A2D RID: 2605
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04000A2E RID: 2606
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04000A2F RID: 2607
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x04000A30 RID: 2608
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

		// Token: 0x04000A31 RID: 2609
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04000A32 RID: 2610
		private global::System.Windows.Forms.TextBox txtInfo;

		// Token: 0x04000A33 RID: 2611
		private global::System.Windows.Forms.ToolStripMenuItem uploadMultithreadToolStripMenuItem;

		// Token: 0x04000A34 RID: 2612
		private global::System.Windows.Forms.ToolStripMenuItem watchingDog15minToolStripMenuItem;

		// Token: 0x04000A35 RID: 2613
		private global::System.Windows.Forms.ToolStripMenuItem watchingDog30minToolStripMenuItem;

		// Token: 0x04000A36 RID: 2614
		private global::System.Windows.Forms.ToolStripMenuItem watchingDogConfigureToolStripMenuItem;

		// Token: 0x04000A37 RID: 2615
		private global::System.Windows.Forms.ToolStripMenuItem watchingDogCustomToolStripMenuItem;

		// Token: 0x04000A38 RID: 2616
		private global::System.Windows.Forms.ToolStripMenuItem watchingDogDeactiveToolStripMenuItem;

		// Token: 0x04000A39 RID: 2617
		public global::System.Windows.Forms.ToolStripButton btnCheck;

		// Token: 0x04000A3A RID: 2618
		public global::System.Windows.Forms.ToolStripButton btnDownloadPrivileges;

		// Token: 0x04000A3B RID: 2619
		public global::System.Windows.Forms.ToolStripButton btnGetRecords;

		// Token: 0x04000A3C RID: 2620
		public global::System.Windows.Forms.ToolStripButton btnLocate;

		// Token: 0x04000A3D RID: 2621
		public global::System.Windows.Forms.ToolStripButton btnMaps;

		// Token: 0x04000A3E RID: 2622
		public global::System.Windows.Forms.ToolStripButton btnPersonInside;

		// Token: 0x04000A3F RID: 2623
		public global::System.Windows.Forms.ToolStripButton btnRealtimeGetRecords;

		// Token: 0x04000A40 RID: 2624
		public global::System.Windows.Forms.ToolStripButton btnRemoteOpen;

		// Token: 0x04000A41 RID: 2625
		public global::System.Windows.Forms.ToolStripButton btnServer;

		// Token: 0x04000A42 RID: 2626
		public global::System.Windows.Forms.ToolStripButton btnSetTime;

		// Token: 0x04000A43 RID: 2627
		public global::System.Windows.Forms.ToolStripButton btnStopOthers;

		// Token: 0x04000A44 RID: 2628
		public global::System.Windows.Forms.ToolStripButton btnUpload;

		// Token: 0x04000A45 RID: 2629
		private global::WG3000_COMM.ExtendFunc.TCPServer.TcpServer tcpServer1;

		// Token: 0x04000A46 RID: 2630
		public global::System.Windows.Forms.ListView lstDoors;

		// Token: 0x04000A47 RID: 2631
		public global::System.Windows.Forms.ToolStripMenuItem mnuWarnOutputReset;

		// Token: 0x04000A48 RID: 2632
		public global::System.Windows.Forms.ToolStripMenuItem resetPersonInsideToolStripMenuItem;

		// Token: 0x04000A49 RID: 2633
		public global::System.Windows.Forms.ToolStripMenuItem setDoorControlToolStripMenuItem;
	}
}
