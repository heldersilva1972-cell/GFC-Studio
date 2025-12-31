namespace WG3000_COMM.Basic
{
	// Token: 0x02000004 RID: 4
	public partial class dfrmBLELockSearch : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600005C RID: 92 RVA: 0x0000CB50 File Offset: 0x0000BB50
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.wgudp != null)
			{
				this.wgudp.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000CB88 File Offset: 0x0000BB88
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmBLELockSearch));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.configureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.findF3ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.addSelectedToSystemToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.searchToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.searchAdvancedToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.searchSpecialSNToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.search100FromTheSpecialSNToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.communicationTestToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.restoreDefaultIPToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultParamToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreAllSwipesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.clearSwipesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.formatToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.quickFormatToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.otherToolsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enableMobileOpenDoorToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.multicardSwipeGapToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.masterClientControllerSetToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableMasterClientToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setAsMasterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setAsClientToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pCControlSwipeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disablePCControlSwipeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enabledPCControlSwipeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.swipeInOrderToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableSwipeInOrderToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.swipeInOrderMode1Reader1212ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.swipeInOrderMode2Reader134213ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dHCPToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableDHCPDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enableDHCPToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cloudServerConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.sMSConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.iPFilterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableMobileOpenDoorToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.readerInputFormatToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreAsDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setReaderFormatWG26NoCheckToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setSpeacialToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setReaderFormat35BitsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.hideControllerToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.showToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.hideToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.otherToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.batchUpdateSelectToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.batchUpdateSelectedIPInDBToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.autoSetIPToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.autoSetIPNotCheckDuplicateToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restore100M10MAutoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.force100MV552AboveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.force10MHalfDuplexToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.serialPort1 = new global::System.IO.Ports.SerialPort(this.components);
			this.button148 = new global::System.Windows.Forms.Button();
			this.button150 = new global::System.Windows.Forms.Button();
			this.txtBaudrate = new global::System.Windows.Forms.TextBox();
			this.label211 = new global::System.Windows.Forms.Label();
			this.button149 = new global::System.Windows.Forms.Button();
			this.cboUART = new global::System.Windows.Forms.ComboBox();
			this.statusStrip1 = new global::System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.btnIPAndWebConfigure = new global::System.Windows.Forms.Button();
			this.lblCount = new global::System.Windows.Forms.Label();
			this.lblSearchNow = new global::System.Windows.Forms.Label();
			this.chkSearchAgain = new global::System.Windows.Forms.CheckBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnAddToSystem = new global::System.Windows.Forms.Button();
			this.dgvFoundControllers = new global::System.Windows.Forms.DataGridView();
			this.f_ID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Mask = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Gateway = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_PORT = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MACAddr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PCIPAddr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Note = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PCMacAddr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnConfigure = new global::System.Windows.Forms.Button();
			this.btnDefault = new global::System.Windows.Forms.Button();
			this.btnSearch = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.btnClearAllExclude = new global::System.Windows.Forms.Button();
			this.btnAddAllToExclude = new global::System.Windows.Forms.Button();
			this.listBox1 = new global::System.Windows.Forms.ListBox();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			this.chkLock = new global::System.Windows.Forms.CheckBox();
			this.chkReader = new global::System.Windows.Forms.CheckBox();
			this.contextMenuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvFoundControllers).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.configureToolStripMenuItem, this.findF3ToolStripMenuItem, this.addSelectedToSystemToolStripMenuItem, this.clearToolStripMenuItem, this.toolStripSeparator1, this.searchToolStripMenuItem, this.searchAdvancedToolStripMenuItem, this.communicationTestToolStripMenuItem, this.toolStripSeparator2, this.restoreDefaultIPToolStripMenuItem,
				this.restoreDefaultParamToolStripMenuItem, this.restoreAllSwipesToolStripMenuItem, this.clearSwipesToolStripMenuItem, this.formatToolStripMenuItem, this.quickFormatToolStripMenuItem, this.toolStripSeparator3, this.otherToolsToolStripMenuItem, this.sMSConfigureToolStripMenuItem, this.iPFilterToolStripMenuItem, this.disableMobileOpenDoorToolStripMenuItem,
				this.readerInputFormatToolStripMenuItem, this.hideControllerToolStripMenuItem, this.otherToolStripMenuItem, this.mConfigureToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.configureToolStripMenuItem, "configureToolStripMenuItem");
			this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
			this.configureToolStripMenuItem.Click += new global::System.EventHandler(this.btnConfigure_Click);
			componentResourceManager.ApplyResources(this.findF3ToolStripMenuItem, "findF3ToolStripMenuItem");
			this.findF3ToolStripMenuItem.Name = "findF3ToolStripMenuItem";
			this.findF3ToolStripMenuItem.Click += new global::System.EventHandler(this.findF3ToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.addSelectedToSystemToolStripMenuItem, "addSelectedToSystemToolStripMenuItem");
			this.addSelectedToSystemToolStripMenuItem.Name = "addSelectedToSystemToolStripMenuItem";
			this.addSelectedToSystemToolStripMenuItem.Click += new global::System.EventHandler(this.addSelectedToSystemToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Click += new global::System.EventHandler(this.clearToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.searchToolStripMenuItem, "searchToolStripMenuItem");
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Click += new global::System.EventHandler(this.btnSearch_Click);
			componentResourceManager.ApplyResources(this.searchAdvancedToolStripMenuItem, "searchAdvancedToolStripMenuItem");
			this.searchAdvancedToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.searchSpecialSNToolStripMenuItem, this.search100FromTheSpecialSNToolStripMenuItem });
			this.searchAdvancedToolStripMenuItem.Name = "searchAdvancedToolStripMenuItem";
			componentResourceManager.ApplyResources(this.searchSpecialSNToolStripMenuItem, "searchSpecialSNToolStripMenuItem");
			this.searchSpecialSNToolStripMenuItem.Name = "searchSpecialSNToolStripMenuItem";
			this.searchSpecialSNToolStripMenuItem.Click += new global::System.EventHandler(this.search100FromTheSpecialSNToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.search100FromTheSpecialSNToolStripMenuItem, "search100FromTheSpecialSNToolStripMenuItem");
			this.search100FromTheSpecialSNToolStripMenuItem.Name = "search100FromTheSpecialSNToolStripMenuItem";
			this.search100FromTheSpecialSNToolStripMenuItem.Click += new global::System.EventHandler(this.search100FromTheSpecialSNToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.communicationTestToolStripMenuItem, "communicationTestToolStripMenuItem");
			this.communicationTestToolStripMenuItem.Name = "communicationTestToolStripMenuItem";
			this.communicationTestToolStripMenuItem.Click += new global::System.EventHandler(this.communicationTestToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			componentResourceManager.ApplyResources(this.restoreDefaultIPToolStripMenuItem, "restoreDefaultIPToolStripMenuItem");
			this.restoreDefaultIPToolStripMenuItem.Name = "restoreDefaultIPToolStripMenuItem";
			this.restoreDefaultIPToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultIPToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultParamToolStripMenuItem, "restoreDefaultParamToolStripMenuItem");
			this.restoreDefaultParamToolStripMenuItem.Name = "restoreDefaultParamToolStripMenuItem";
			this.restoreDefaultParamToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultParamToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreAllSwipesToolStripMenuItem, "restoreAllSwipesToolStripMenuItem");
			this.restoreAllSwipesToolStripMenuItem.Name = "restoreAllSwipesToolStripMenuItem";
			this.restoreAllSwipesToolStripMenuItem.Click += new global::System.EventHandler(this.restoreAllSwipesToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.clearSwipesToolStripMenuItem, "clearSwipesToolStripMenuItem");
			this.clearSwipesToolStripMenuItem.Name = "clearSwipesToolStripMenuItem";
			this.clearSwipesToolStripMenuItem.Click += new global::System.EventHandler(this.clearSwipesToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.formatToolStripMenuItem, "formatToolStripMenuItem");
			this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
			this.formatToolStripMenuItem.Click += new global::System.EventHandler(this.formatToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.quickFormatToolStripMenuItem, "quickFormatToolStripMenuItem");
			this.quickFormatToolStripMenuItem.Name = "quickFormatToolStripMenuItem";
			this.quickFormatToolStripMenuItem.Click += new global::System.EventHandler(this.quickFormatToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			componentResourceManager.ApplyResources(this.otherToolsToolStripMenuItem, "otherToolsToolStripMenuItem");
			this.otherToolsToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.enableMobileOpenDoorToolStripMenuItem, this.multicardSwipeGapToolStripMenuItem, this.masterClientControllerSetToolStripMenuItem, this.pCControlSwipeToolStripMenuItem, this.swipeInOrderToolStripMenuItem, this.dHCPToolStripMenuItem, this.cloudServerConfigureToolStripMenuItem });
			this.otherToolsToolStripMenuItem.Name = "otherToolsToolStripMenuItem";
			componentResourceManager.ApplyResources(this.enableMobileOpenDoorToolStripMenuItem, "enableMobileOpenDoorToolStripMenuItem");
			this.enableMobileOpenDoorToolStripMenuItem.Name = "enableMobileOpenDoorToolStripMenuItem";
			this.enableMobileOpenDoorToolStripMenuItem.Click += new global::System.EventHandler(this.enableMobileOpenDoorToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.multicardSwipeGapToolStripMenuItem, "multicardSwipeGapToolStripMenuItem");
			this.multicardSwipeGapToolStripMenuItem.Name = "multicardSwipeGapToolStripMenuItem";
			this.multicardSwipeGapToolStripMenuItem.Click += new global::System.EventHandler(this.multicardSwipeGapToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.masterClientControllerSetToolStripMenuItem, "masterClientControllerSetToolStripMenuItem");
			this.masterClientControllerSetToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.disableMasterClientToolStripMenuItem, this.setAsMasterToolStripMenuItem, this.setAsClientToolStripMenuItem });
			this.masterClientControllerSetToolStripMenuItem.Name = "masterClientControllerSetToolStripMenuItem";
			componentResourceManager.ApplyResources(this.disableMasterClientToolStripMenuItem, "disableMasterClientToolStripMenuItem");
			this.disableMasterClientToolStripMenuItem.Name = "disableMasterClientToolStripMenuItem";
			this.disableMasterClientToolStripMenuItem.Click += new global::System.EventHandler(this.disableMasterClientToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setAsMasterToolStripMenuItem, "setAsMasterToolStripMenuItem");
			this.setAsMasterToolStripMenuItem.Name = "setAsMasterToolStripMenuItem";
			this.setAsMasterToolStripMenuItem.Click += new global::System.EventHandler(this.disableMasterClientToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setAsClientToolStripMenuItem, "setAsClientToolStripMenuItem");
			this.setAsClientToolStripMenuItem.Name = "setAsClientToolStripMenuItem";
			this.setAsClientToolStripMenuItem.Click += new global::System.EventHandler(this.disableMasterClientToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.pCControlSwipeToolStripMenuItem, "pCControlSwipeToolStripMenuItem");
			this.pCControlSwipeToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.disablePCControlSwipeToolStripMenuItem, this.enabledPCControlSwipeToolStripMenuItem });
			this.pCControlSwipeToolStripMenuItem.Name = "pCControlSwipeToolStripMenuItem";
			componentResourceManager.ApplyResources(this.disablePCControlSwipeToolStripMenuItem, "disablePCControlSwipeToolStripMenuItem");
			this.disablePCControlSwipeToolStripMenuItem.Name = "disablePCControlSwipeToolStripMenuItem";
			this.disablePCControlSwipeToolStripMenuItem.Click += new global::System.EventHandler(this.disablePCControlSwipeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.enabledPCControlSwipeToolStripMenuItem, "enabledPCControlSwipeToolStripMenuItem");
			this.enabledPCControlSwipeToolStripMenuItem.Name = "enabledPCControlSwipeToolStripMenuItem";
			this.enabledPCControlSwipeToolStripMenuItem.Click += new global::System.EventHandler(this.disablePCControlSwipeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.swipeInOrderToolStripMenuItem, "swipeInOrderToolStripMenuItem");
			this.swipeInOrderToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.disableSwipeInOrderToolStripMenuItem, this.swipeInOrderMode1Reader1212ToolStripMenuItem, this.swipeInOrderMode2Reader134213ToolStripMenuItem });
			this.swipeInOrderToolStripMenuItem.Name = "swipeInOrderToolStripMenuItem";
			componentResourceManager.ApplyResources(this.disableSwipeInOrderToolStripMenuItem, "disableSwipeInOrderToolStripMenuItem");
			this.disableSwipeInOrderToolStripMenuItem.Name = "disableSwipeInOrderToolStripMenuItem";
			this.disableSwipeInOrderToolStripMenuItem.Click += new global::System.EventHandler(this.disableSwipeInOrderToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.swipeInOrderMode1Reader1212ToolStripMenuItem, "swipeInOrderMode1Reader1212ToolStripMenuItem");
			this.swipeInOrderMode1Reader1212ToolStripMenuItem.Name = "swipeInOrderMode1Reader1212ToolStripMenuItem";
			this.swipeInOrderMode1Reader1212ToolStripMenuItem.Click += new global::System.EventHandler(this.disableSwipeInOrderToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.swipeInOrderMode2Reader134213ToolStripMenuItem, "swipeInOrderMode2Reader134213ToolStripMenuItem");
			this.swipeInOrderMode2Reader134213ToolStripMenuItem.Name = "swipeInOrderMode2Reader134213ToolStripMenuItem";
			this.swipeInOrderMode2Reader134213ToolStripMenuItem.Click += new global::System.EventHandler(this.disableSwipeInOrderToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.dHCPToolStripMenuItem, "dHCPToolStripMenuItem");
			this.dHCPToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.disableDHCPDefaultToolStripMenuItem, this.enableDHCPToolStripMenuItem });
			this.dHCPToolStripMenuItem.Name = "dHCPToolStripMenuItem";
			componentResourceManager.ApplyResources(this.disableDHCPDefaultToolStripMenuItem, "disableDHCPDefaultToolStripMenuItem");
			this.disableDHCPDefaultToolStripMenuItem.Name = "disableDHCPDefaultToolStripMenuItem";
			this.disableDHCPDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.enableDHCPToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.enableDHCPToolStripMenuItem, "enableDHCPToolStripMenuItem");
			this.enableDHCPToolStripMenuItem.Name = "enableDHCPToolStripMenuItem";
			this.enableDHCPToolStripMenuItem.Click += new global::System.EventHandler(this.enableDHCPToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.cloudServerConfigureToolStripMenuItem, "cloudServerConfigureToolStripMenuItem");
			this.cloudServerConfigureToolStripMenuItem.Name = "cloudServerConfigureToolStripMenuItem";
			this.cloudServerConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.cloudServerConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.sMSConfigureToolStripMenuItem, "sMSConfigureToolStripMenuItem");
			this.sMSConfigureToolStripMenuItem.Name = "sMSConfigureToolStripMenuItem";
			this.sMSConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.sMSConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.iPFilterToolStripMenuItem, "iPFilterToolStripMenuItem");
			this.iPFilterToolStripMenuItem.Name = "iPFilterToolStripMenuItem";
			this.iPFilterToolStripMenuItem.Click += new global::System.EventHandler(this.iPFilterToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.disableMobileOpenDoorToolStripMenuItem, "disableMobileOpenDoorToolStripMenuItem");
			this.disableMobileOpenDoorToolStripMenuItem.Name = "disableMobileOpenDoorToolStripMenuItem";
			this.disableMobileOpenDoorToolStripMenuItem.Click += new global::System.EventHandler(this.disableMobileOpenDoorToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.readerInputFormatToolStripMenuItem, "readerInputFormatToolStripMenuItem");
			this.readerInputFormatToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.restoreAsDefaultToolStripMenuItem, this.setReaderFormatWG26NoCheckToolStripMenuItem, this.setSpeacialToolStripMenuItem, this.setReaderFormat35BitsToolStripMenuItem });
			this.readerInputFormatToolStripMenuItem.Name = "readerInputFormatToolStripMenuItem";
			componentResourceManager.ApplyResources(this.restoreAsDefaultToolStripMenuItem, "restoreAsDefaultToolStripMenuItem");
			this.restoreAsDefaultToolStripMenuItem.Name = "restoreAsDefaultToolStripMenuItem";
			this.restoreAsDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.setSpeacialToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setReaderFormatWG26NoCheckToolStripMenuItem, "setReaderFormatWG26NoCheckToolStripMenuItem");
			this.setReaderFormatWG26NoCheckToolStripMenuItem.Name = "setReaderFormatWG26NoCheckToolStripMenuItem";
			this.setReaderFormatWG26NoCheckToolStripMenuItem.Click += new global::System.EventHandler(this.setSpeacialToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setSpeacialToolStripMenuItem, "setSpeacialToolStripMenuItem");
			this.setSpeacialToolStripMenuItem.Name = "setSpeacialToolStripMenuItem";
			this.setSpeacialToolStripMenuItem.Click += new global::System.EventHandler(this.setSpeacialToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setReaderFormat35BitsToolStripMenuItem, "setReaderFormat35BitsToolStripMenuItem");
			this.setReaderFormat35BitsToolStripMenuItem.Name = "setReaderFormat35BitsToolStripMenuItem";
			this.setReaderFormat35BitsToolStripMenuItem.Click += new global::System.EventHandler(this.setSpeacialToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.hideControllerToolStripMenuItem, "hideControllerToolStripMenuItem");
			this.hideControllerToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.showToolStripMenuItem, this.hideToolStripMenuItem });
			this.hideControllerToolStripMenuItem.Name = "hideControllerToolStripMenuItem";
			componentResourceManager.ApplyResources(this.showToolStripMenuItem, "showToolStripMenuItem");
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			this.showToolStripMenuItem.Click += new global::System.EventHandler(this.hideToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.hideToolStripMenuItem, "hideToolStripMenuItem");
			this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
			this.hideToolStripMenuItem.Click += new global::System.EventHandler(this.hideToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.otherToolStripMenuItem, "otherToolStripMenuItem");
			this.otherToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.batchUpdateSelectToolStripMenuItem, this.batchUpdateSelectedIPInDBToolStripMenuItem, this.autoSetIPToolStripMenuItem, this.autoSetIPNotCheckDuplicateToolStripMenuItem });
			this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
			componentResourceManager.ApplyResources(this.batchUpdateSelectToolStripMenuItem, "batchUpdateSelectToolStripMenuItem");
			this.batchUpdateSelectToolStripMenuItem.Name = "batchUpdateSelectToolStripMenuItem";
			this.batchUpdateSelectToolStripMenuItem.Click += new global::System.EventHandler(this.batchUpdateSelectToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.batchUpdateSelectedIPInDBToolStripMenuItem, "batchUpdateSelectedIPInDBToolStripMenuItem");
			this.batchUpdateSelectedIPInDBToolStripMenuItem.Name = "batchUpdateSelectedIPInDBToolStripMenuItem";
			this.batchUpdateSelectedIPInDBToolStripMenuItem.Click += new global::System.EventHandler(this.batchUpdateSelectedIPInDBToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.autoSetIPToolStripMenuItem, "autoSetIPToolStripMenuItem");
			this.autoSetIPToolStripMenuItem.Name = "autoSetIPToolStripMenuItem";
			this.autoSetIPToolStripMenuItem.Click += new global::System.EventHandler(this.autoSetIPToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.autoSetIPNotCheckDuplicateToolStripMenuItem, "autoSetIPNotCheckDuplicateToolStripMenuItem");
			this.autoSetIPNotCheckDuplicateToolStripMenuItem.Name = "autoSetIPNotCheckDuplicateToolStripMenuItem";
			this.autoSetIPNotCheckDuplicateToolStripMenuItem.Click += new global::System.EventHandler(this.autoSetIPToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.mConfigureToolStripMenuItem, "mConfigureToolStripMenuItem");
			this.mConfigureToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.restore100M10MAutoToolStripMenuItem, this.force100MV552AboveToolStripMenuItem, this.force10MHalfDuplexToolStripMenuItem });
			this.mConfigureToolStripMenuItem.Name = "mConfigureToolStripMenuItem";
			componentResourceManager.ApplyResources(this.restore100M10MAutoToolStripMenuItem, "restore100M10MAutoToolStripMenuItem");
			this.restore100M10MAutoToolStripMenuItem.Name = "restore100M10MAutoToolStripMenuItem";
			this.restore100M10MAutoToolStripMenuItem.Click += new global::System.EventHandler(this.force10MHalfDuplexToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.force100MV552AboveToolStripMenuItem, "force100MV552AboveToolStripMenuItem");
			this.force100MV552AboveToolStripMenuItem.Name = "force100MV552AboveToolStripMenuItem";
			this.force100MV552AboveToolStripMenuItem.Click += new global::System.EventHandler(this.force10MHalfDuplexToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.force10MHalfDuplexToolStripMenuItem, "force10MHalfDuplexToolStripMenuItem");
			this.force10MHalfDuplexToolStripMenuItem.Name = "force10MHalfDuplexToolStripMenuItem";
			this.force10MHalfDuplexToolStripMenuItem.Click += new global::System.EventHandler(this.force10MHalfDuplexToolStripMenuItem_Click);
			this.serialPort1.BaudRate = 115200;
			this.serialPort1.ErrorReceived += new global::System.IO.Ports.SerialErrorReceivedEventHandler(this.serialPort1_ErrorReceived);
			this.serialPort1.DataReceived += new global::System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
			componentResourceManager.ApplyResources(this.button148, "button148");
			this.button148.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button148.ForeColor = global::System.Drawing.Color.White;
			this.button148.Name = "button148";
			this.button148.UseVisualStyleBackColor = true;
			this.button148.Click += new global::System.EventHandler(this.button148_Click);
			componentResourceManager.ApplyResources(this.button150, "button150");
			this.button150.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button150.ForeColor = global::System.Drawing.Color.White;
			this.button150.Name = "button150";
			this.button150.UseVisualStyleBackColor = true;
			this.button150.Click += new global::System.EventHandler(this.button150_Click);
			componentResourceManager.ApplyResources(this.txtBaudrate, "txtBaudrate");
			this.txtBaudrate.Name = "txtBaudrate";
			componentResourceManager.ApplyResources(this.label211, "label211");
			this.label211.ForeColor = global::System.Drawing.Color.White;
			this.label211.Name = "label211";
			componentResourceManager.ApplyResources(this.button149, "button149");
			this.button149.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button149.ForeColor = global::System.Drawing.Color.White;
			this.button149.Name = "button149";
			this.button149.UseVisualStyleBackColor = true;
			this.button149.Click += new global::System.EventHandler(this.button149_Click);
			componentResourceManager.ApplyResources(this.cboUART, "cboUART");
			this.cboUART.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboUART.FormattingEnabled = true;
			this.cboUART.Name = "cboUART";
			this.cboUART.SelectedIndexChanged += new global::System.EventHandler(this.cboUART_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.statusStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_bottom;
			this.statusStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripStatusLabel1, this.toolStripStatusLabel2 });
			this.statusStrip1.Name = "statusStrip1";
			componentResourceManager.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
			this.toolStripStatusLabel1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripStatusLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Click += new global::System.EventHandler(this.toolStripStatusLabel1_Click);
			componentResourceManager.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
			this.toolStripStatusLabel2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripStatusLabel2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Spring = true;
			this.toolStripStatusLabel2.Click += new global::System.EventHandler(this.toolStripStatusLabel2_Click);
			componentResourceManager.ApplyResources(this.btnIPAndWebConfigure, "btnIPAndWebConfigure");
			this.btnIPAndWebConfigure.BackColor = global::System.Drawing.Color.Transparent;
			this.btnIPAndWebConfigure.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnIPAndWebConfigure.ForeColor = global::System.Drawing.Color.White;
			this.btnIPAndWebConfigure.Name = "btnIPAndWebConfigure";
			this.btnIPAndWebConfigure.UseVisualStyleBackColor = false;
			this.btnIPAndWebConfigure.Click += new global::System.EventHandler(this.btnIPAndWebConfigure_Click);
			componentResourceManager.ApplyResources(this.lblCount, "lblCount");
			this.lblCount.BackColor = global::System.Drawing.Color.Transparent;
			this.lblCount.ForeColor = global::System.Drawing.Color.White;
			this.lblCount.Name = "lblCount";
			componentResourceManager.ApplyResources(this.lblSearchNow, "lblSearchNow");
			this.lblSearchNow.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSearchNow.ForeColor = global::System.Drawing.Color.White;
			this.lblSearchNow.Name = "lblSearchNow";
			componentResourceManager.ApplyResources(this.chkSearchAgain, "chkSearchAgain");
			this.chkSearchAgain.BackColor = global::System.Drawing.Color.Transparent;
			this.chkSearchAgain.Checked = true;
			this.chkSearchAgain.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkSearchAgain.ForeColor = global::System.Drawing.Color.White;
			this.chkSearchAgain.Name = "chkSearchAgain";
			this.chkSearchAgain.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnAddToSystem, "btnAddToSystem");
			this.btnAddToSystem.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddToSystem.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddToSystem.ForeColor = global::System.Drawing.Color.White;
			this.btnAddToSystem.Name = "btnAddToSystem";
			this.btnAddToSystem.UseVisualStyleBackColor = false;
			this.btnAddToSystem.Click += new global::System.EventHandler(this.btnAddToSystem_Click);
			componentResourceManager.ApplyResources(this.dgvFoundControllers, "dgvFoundControllers");
			this.dgvFoundControllers.AllowUserToAddRows = false;
			this.dgvFoundControllers.AllowUserToDeleteRows = false;
			this.dgvFoundControllers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvFoundControllers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvFoundControllers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvFoundControllers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ID, this.f_ControllerSN, this.f_IP, this.f_Mask, this.f_Gateway, this.f_PORT, this.f_MACAddr, this.f_PCIPAddr, this.f_Note, this.PCMacAddr });
			this.dgvFoundControllers.ContextMenuStrip = this.contextMenuStrip1;
			this.dgvFoundControllers.EnableHeadersVisualStyles = false;
			this.dgvFoundControllers.Name = "dgvFoundControllers";
			this.dgvFoundControllers.ReadOnly = true;
			this.dgvFoundControllers.RowHeadersVisible = false;
			this.dgvFoundControllers.RowTemplate.Height = 23;
			this.dgvFoundControllers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvFoundControllers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvFoundControllers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.f_ID, "f_ID");
			this.f_ID.Name = "f_ID";
			this.f_ID.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_IP, "f_IP");
			this.f_IP.Name = "f_IP";
			this.f_IP.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Mask, "f_Mask");
			this.f_Mask.Name = "f_Mask";
			this.f_Mask.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Gateway, "f_Gateway");
			this.f_Gateway.Name = "f_Gateway";
			this.f_Gateway.ReadOnly = true;
			this.f_Gateway.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_Gateway.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_PORT.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_PORT, "f_PORT");
			this.f_PORT.Name = "f_PORT";
			this.f_PORT.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MACAddr, "f_MACAddr");
			this.f_MACAddr.Name = "f_MACAddr";
			this.f_MACAddr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_PCIPAddr, "f_PCIPAddr");
			this.f_PCIPAddr.Name = "f_PCIPAddr";
			this.f_PCIPAddr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Note, "f_Note");
			this.f_Note.Name = "f_Note";
			this.f_Note.ReadOnly = true;
			componentResourceManager.ApplyResources(this.PCMacAddr, "PCMacAddr");
			this.PCMacAddr.Name = "PCMacAddr";
			this.PCMacAddr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnConfigure, "btnConfigure");
			this.btnConfigure.BackColor = global::System.Drawing.Color.Transparent;
			this.btnConfigure.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnConfigure.ForeColor = global::System.Drawing.Color.White;
			this.btnConfigure.Name = "btnConfigure";
			this.btnConfigure.UseVisualStyleBackColor = false;
			this.btnConfigure.Click += new global::System.EventHandler(this.btnConfigure_Click);
			componentResourceManager.ApplyResources(this.btnDefault, "btnDefault");
			this.btnDefault.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDefault.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDefault.ForeColor = global::System.Drawing.Color.White;
			this.btnDefault.Name = "btnDefault";
			this.btnDefault.UseVisualStyleBackColor = false;
			this.btnDefault.Click += new global::System.EventHandler(this.btnDefault_Click);
			componentResourceManager.ApplyResources(this.btnSearch, "btnSearch");
			this.btnSearch.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSearch.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSearch.ForeColor = global::System.Drawing.Color.White;
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.UseVisualStyleBackColor = false;
			this.btnSearch.Click += new global::System.EventHandler(this.btnSearch_Click);
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			componentResourceManager.ApplyResources(this.btnClearAllExclude, "btnClearAllExclude");
			this.btnClearAllExclude.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClearAllExclude.ForeColor = global::System.Drawing.Color.White;
			this.btnClearAllExclude.Name = "btnClearAllExclude";
			this.btnClearAllExclude.UseVisualStyleBackColor = true;
			this.btnClearAllExclude.Click += new global::System.EventHandler(this.btnClearAllExclude_Click);
			componentResourceManager.ApplyResources(this.btnAddAllToExclude, "btnAddAllToExclude");
			this.btnAddAllToExclude.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllToExclude.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllToExclude.Name = "btnAddAllToExclude";
			this.btnAddAllToExclude.UseVisualStyleBackColor = true;
			this.btnAddAllToExclude.Click += new global::System.EventHandler(this.btnAddAllToExclude_Click);
			componentResourceManager.ApplyResources(this.listBox1, "listBox1");
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Name = "listBox1";
			componentResourceManager.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.ForeColor = global::System.Drawing.Color.White;
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkLock, "chkLock");
			this.chkLock.Checked = true;
			this.chkLock.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkLock.ForeColor = global::System.Drawing.Color.White;
			this.chkLock.Name = "chkLock";
			this.chkLock.UseVisualStyleBackColor = true;
			this.chkLock.CheckedChanged += new global::System.EventHandler(this.chkLock_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkReader, "chkReader");
			this.chkReader.Checked = true;
			this.chkReader.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkReader.ForeColor = global::System.Drawing.Color.White;
			this.chkReader.Name = "chkReader";
			this.chkReader.UseVisualStyleBackColor = true;
			this.chkReader.CheckedChanged += new global::System.EventHandler(this.chkReader_CheckedChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkReader);
			base.Controls.Add(this.chkLock);
			base.Controls.Add(this.checkBox1);
			base.Controls.Add(this.listBox1);
			base.Controls.Add(this.btnAddAllToExclude);
			base.Controls.Add(this.btnClearAllExclude);
			base.Controls.Add(this.dgvFoundControllers);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.button148);
			base.Controls.Add(this.button150);
			base.Controls.Add(this.txtBaudrate);
			base.Controls.Add(this.label211);
			base.Controls.Add(this.button149);
			base.Controls.Add(this.cboUART);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.btnIPAndWebConfigure);
			base.Controls.Add(this.lblCount);
			base.Controls.Add(this.lblSearchNow);
			base.Controls.Add(this.chkSearchAgain);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnAddToSystem);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnConfigure);
			base.Controls.Add(this.btnDefault);
			base.Controls.Add(this.btnSearch);
			base.Name = "dfrmBLELockSearch";
			base.WindowState = global::System.Windows.Forms.FormWindowState.Maximized;
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmNetControllerConfig_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmNetControllerConfig_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmNetControllerConfig_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvFoundControllers).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000026 RID: 38
		private global::WG3000_COMM.Core.wgUdpComm wgudp;

		// Token: 0x04000041 RID: 65
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.ToolStripMenuItem addSelectedToSystemToolStripMenuItem;

		// Token: 0x04000043 RID: 67
		private global::System.Windows.Forms.ToolStripMenuItem autoSetIPNotCheckDuplicateToolStripMenuItem;

		// Token: 0x04000044 RID: 68
		private global::System.Windows.Forms.ToolStripMenuItem autoSetIPToolStripMenuItem;

		// Token: 0x04000045 RID: 69
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectedIPInDBToolStripMenuItem;

		// Token: 0x04000046 RID: 70
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectToolStripMenuItem;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.Button btnAddAllToExclude;

		// Token: 0x04000048 RID: 72
		private global::System.Windows.Forms.Button btnClearAllExclude;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.Button btnConfigure;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.Button btnDefault;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.Button btnSearch;

		// Token: 0x0400004D RID: 77
		private global::System.Windows.Forms.Button button1;

		// Token: 0x0400004E RID: 78
		private global::System.Windows.Forms.Button button148;

		// Token: 0x0400004F RID: 79
		private global::System.Windows.Forms.Button button149;

		// Token: 0x04000050 RID: 80
		private global::System.Windows.Forms.Button button150;

		// Token: 0x04000051 RID: 81
		private global::System.Windows.Forms.ComboBox cboUART;

		// Token: 0x04000052 RID: 82
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x04000053 RID: 83
		private global::System.Windows.Forms.CheckBox chkLock;

		// Token: 0x04000054 RID: 84
		private global::System.Windows.Forms.CheckBox chkReader;

		// Token: 0x04000055 RID: 85
		private global::System.Windows.Forms.CheckBox chkSearchAgain;

		// Token: 0x04000056 RID: 86
		private global::System.Windows.Forms.ToolStripMenuItem clearSwipesToolStripMenuItem;

		// Token: 0x04000057 RID: 87
		private global::System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;

		// Token: 0x04000058 RID: 88
		private global::System.Windows.Forms.ToolStripMenuItem cloudServerConfigureToolStripMenuItem;

		// Token: 0x04000059 RID: 89
		private global::System.Windows.Forms.ToolStripMenuItem communicationTestToolStripMenuItem;

		// Token: 0x0400005A RID: 90
		private global::System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;

		// Token: 0x0400005B RID: 91
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x0400005C RID: 92
		private global::System.Windows.Forms.DataGridView dgvFoundControllers;

		// Token: 0x0400005D RID: 93
		private global::System.Windows.Forms.ToolStripMenuItem dHCPToolStripMenuItem;

		// Token: 0x0400005E RID: 94
		private global::System.Windows.Forms.ToolStripMenuItem disableDHCPDefaultToolStripMenuItem;

		// Token: 0x0400005F RID: 95
		private global::System.Windows.Forms.ToolStripMenuItem disableMasterClientToolStripMenuItem;

		// Token: 0x04000060 RID: 96
		private global::System.Windows.Forms.ToolStripMenuItem disableMobileOpenDoorToolStripMenuItem;

		// Token: 0x04000061 RID: 97
		private global::System.Windows.Forms.ToolStripMenuItem disablePCControlSwipeToolStripMenuItem;

		// Token: 0x04000062 RID: 98
		private global::System.Windows.Forms.ToolStripMenuItem disableSwipeInOrderToolStripMenuItem;

		// Token: 0x04000063 RID: 99
		private global::System.Windows.Forms.ToolStripMenuItem enableDHCPToolStripMenuItem;

		// Token: 0x04000064 RID: 100
		private global::System.Windows.Forms.ToolStripMenuItem enabledPCControlSwipeToolStripMenuItem;

		// Token: 0x04000065 RID: 101
		private global::System.Windows.Forms.ToolStripMenuItem enableMobileOpenDoorToolStripMenuItem;

		// Token: 0x04000066 RID: 102
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04000067 RID: 103
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Gateway;

		// Token: 0x04000068 RID: 104
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ID;

		// Token: 0x04000069 RID: 105
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x0400006A RID: 106
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MACAddr;

		// Token: 0x0400006B RID: 107
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Mask;

		// Token: 0x0400006C RID: 108
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x0400006D RID: 109
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PCIPAddr;

		// Token: 0x0400006E RID: 110
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;

		// Token: 0x0400006F RID: 111
		private global::System.Windows.Forms.ToolStripMenuItem findF3ToolStripMenuItem;

		// Token: 0x04000070 RID: 112
		private global::System.Windows.Forms.ToolStripMenuItem force100MV552AboveToolStripMenuItem;

		// Token: 0x04000071 RID: 113
		private global::System.Windows.Forms.ToolStripMenuItem force10MHalfDuplexToolStripMenuItem;

		// Token: 0x04000072 RID: 114
		private global::System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;

		// Token: 0x04000073 RID: 115
		private global::System.Windows.Forms.ToolStripMenuItem hideControllerToolStripMenuItem;

		// Token: 0x04000074 RID: 116
		private global::System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;

		// Token: 0x04000075 RID: 117
		private global::System.Windows.Forms.ToolStripMenuItem iPFilterToolStripMenuItem;

		// Token: 0x04000076 RID: 118
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000077 RID: 119
		private global::System.Windows.Forms.Label label211;

		// Token: 0x04000078 RID: 120
		private global::System.Windows.Forms.Label lblCount;

		// Token: 0x04000079 RID: 121
		private global::System.Windows.Forms.Label lblSearchNow;

		// Token: 0x0400007A RID: 122
		private global::System.Windows.Forms.ListBox listBox1;

		// Token: 0x0400007B RID: 123
		private global::System.Windows.Forms.ToolStripMenuItem masterClientControllerSetToolStripMenuItem;

		// Token: 0x0400007C RID: 124
		private global::System.Windows.Forms.ToolStripMenuItem mConfigureToolStripMenuItem;

		// Token: 0x0400007D RID: 125
		private global::System.Windows.Forms.ToolStripMenuItem multicardSwipeGapToolStripMenuItem;

		// Token: 0x0400007E RID: 126
		private global::System.Windows.Forms.ToolStripMenuItem otherToolsToolStripMenuItem;

		// Token: 0x0400007F RID: 127
		private global::System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;

		// Token: 0x04000080 RID: 128
		private global::System.Windows.Forms.ToolStripMenuItem pCControlSwipeToolStripMenuItem;

		// Token: 0x04000081 RID: 129
		private global::System.Windows.Forms.DataGridViewTextBoxColumn PCMacAddr;

		// Token: 0x04000082 RID: 130
		private global::System.Windows.Forms.ToolStripMenuItem quickFormatToolStripMenuItem;

		// Token: 0x04000083 RID: 131
		private global::System.Windows.Forms.ToolStripMenuItem readerInputFormatToolStripMenuItem;

		// Token: 0x04000084 RID: 132
		private global::System.Windows.Forms.ToolStripMenuItem restore100M10MAutoToolStripMenuItem;

		// Token: 0x04000085 RID: 133
		private global::System.Windows.Forms.ToolStripMenuItem restoreAllSwipesToolStripMenuItem;

		// Token: 0x04000086 RID: 134
		private global::System.Windows.Forms.ToolStripMenuItem restoreAsDefaultToolStripMenuItem;

		// Token: 0x04000087 RID: 135
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultIPToolStripMenuItem;

		// Token: 0x04000088 RID: 136
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultParamToolStripMenuItem;

		// Token: 0x04000089 RID: 137
		private global::System.Windows.Forms.ToolStripMenuItem search100FromTheSpecialSNToolStripMenuItem;

		// Token: 0x0400008A RID: 138
		private global::System.Windows.Forms.ToolStripMenuItem searchAdvancedToolStripMenuItem;

		// Token: 0x0400008B RID: 139
		private global::System.Windows.Forms.ToolStripMenuItem searchSpecialSNToolStripMenuItem;

		// Token: 0x0400008C RID: 140
		private global::System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;

		// Token: 0x0400008D RID: 141
		private global::System.IO.Ports.SerialPort serialPort1;

		// Token: 0x0400008E RID: 142
		private global::System.Windows.Forms.ToolStripMenuItem setAsClientToolStripMenuItem;

		// Token: 0x0400008F RID: 143
		private global::System.Windows.Forms.ToolStripMenuItem setAsMasterToolStripMenuItem;

		// Token: 0x04000090 RID: 144
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormat35BitsToolStripMenuItem;

		// Token: 0x04000091 RID: 145
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormatWG26NoCheckToolStripMenuItem;

		// Token: 0x04000092 RID: 146
		private global::System.Windows.Forms.ToolStripMenuItem setSpeacialToolStripMenuItem;

		// Token: 0x04000093 RID: 147
		private global::System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;

		// Token: 0x04000094 RID: 148
		private global::System.Windows.Forms.ToolStripMenuItem sMSConfigureToolStripMenuItem;

		// Token: 0x04000095 RID: 149
		private global::System.Windows.Forms.StatusStrip statusStrip1;

		// Token: 0x04000096 RID: 150
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderMode1Reader1212ToolStripMenuItem;

		// Token: 0x04000097 RID: 151
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderMode2Reader134213ToolStripMenuItem;

		// Token: 0x04000098 RID: 152
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderToolStripMenuItem;

		// Token: 0x04000099 RID: 153
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x0400009A RID: 154
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x0400009B RID: 155
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x0400009C RID: 156
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

		// Token: 0x0400009D RID: 157
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;

		// Token: 0x0400009E RID: 158
		private global::System.Windows.Forms.TextBox txtBaudrate;

		// Token: 0x0400009F RID: 159
		public global::System.Windows.Forms.Button btnAddToSystem;

		// Token: 0x040000A0 RID: 160
		public global::System.Windows.Forms.Button btnIPAndWebConfigure;
	}
}
