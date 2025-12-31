namespace WG3000_COMM.Basic
{
	// Token: 0x0200001A RID: 26
	public partial class dfrmNetControllerConfig : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00035DD4 File Offset: 0x00034DD4
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

		// Token: 0x06000185 RID: 389 RVA: 0x00035E0C File Offset: 0x00034E0C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmNetControllerConfig));
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
			this.wGQRReaderConfigureV892AboveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dHCPToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableDHCPDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enableDHCPToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.invalidSwipeOpenDoorV896AboveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableInvalidSwipeOpenDoorToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enableInvalidSwipeOpenDoorToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.rS232ConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.option19 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.wIFIToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setAlarmOffDelayToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setAlarmOffDelay30SecDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setAlarmOffDelay60SecToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.limit2HourOutForSwimmingPoolToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.activeElevatorAsSwitchV899AboveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.get656InfoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.inputOldDeviceInformationToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.sMSConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.iPFilterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableMobileOpenDoorToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.readerInputFormatToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreAsDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setReaderFormatWG26NoCheckToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setSpeacialToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setReaderFormat35BitsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setReaderFormatHID36BitsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setReaderFormatWG26FromWG34ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
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
			this.custTypeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.twoCardCheckV885AboveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.twoCardCheckRestoreDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.twoCardCheckOneByOneToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.twoCardCheckMoreToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.twoCardCheckCustomToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pwd468CheckV890above = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pwd468CheckV890aboveDeactive = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pwd468CheckV890above4 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pwd468CheckV890above6 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pwd468CheckV890above8 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pwd468CheckV890aboveCustom = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new global::System.Windows.Forms.ToolStripSeparator();
			this.cloudServerConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.hTTPServerConfigureV882AboveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
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
			this.f_Gateway = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PORT = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MACAddr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PCIPAddr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Note = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PCMacAddr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnConfigure = new global::System.Windows.Forms.Button();
			this.btnDefault = new global::System.Windows.Forms.Button();
			this.btnSearch = new global::System.Windows.Forms.Button();
			this.btnFormatSpecial = new global::System.Windows.Forms.Button();
			this.txtFormatSpecial = new global::System.Windows.Forms.TextBox();
			this.btnSetProductType = new global::System.Windows.Forms.Button();
			this.btnGetTypeInfo = new global::System.Windows.Forms.Button();
			this.btnTestNewCustTypeController = new global::System.Windows.Forms.Button();
			this.btnResetProductType = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.contextMenuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvFoundControllers).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.configureToolStripMenuItem, this.findF3ToolStripMenuItem, this.addSelectedToSystemToolStripMenuItem, this.clearToolStripMenuItem, this.toolStripSeparator1, this.searchToolStripMenuItem, this.searchAdvancedToolStripMenuItem, this.communicationTestToolStripMenuItem, this.toolStripSeparator2, this.restoreDefaultIPToolStripMenuItem,
				this.restoreDefaultParamToolStripMenuItem, this.restoreAllSwipesToolStripMenuItem, this.clearSwipesToolStripMenuItem, this.formatToolStripMenuItem, this.quickFormatToolStripMenuItem, this.toolStripSeparator3, this.otherToolsToolStripMenuItem, this.option19, this.sMSConfigureToolStripMenuItem, this.iPFilterToolStripMenuItem,
				this.disableMobileOpenDoorToolStripMenuItem, this.readerInputFormatToolStripMenuItem, this.hideControllerToolStripMenuItem, this.otherToolStripMenuItem, this.mConfigureToolStripMenuItem, this.custTypeToolStripMenuItem, this.twoCardCheckV885AboveToolStripMenuItem, this.pwd468CheckV890above, this.toolStripSeparator4, this.cloudServerConfigureToolStripMenuItem,
				this.hTTPServerConfigureV882AboveToolStripMenuItem
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
			this.otherToolsToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.enableMobileOpenDoorToolStripMenuItem, this.multicardSwipeGapToolStripMenuItem, this.masterClientControllerSetToolStripMenuItem, this.pCControlSwipeToolStripMenuItem, this.swipeInOrderToolStripMenuItem, this.wGQRReaderConfigureV892AboveToolStripMenuItem, this.dHCPToolStripMenuItem, this.invalidSwipeOpenDoorV896AboveToolStripMenuItem, this.rS232ConfigureToolStripMenuItem });
			this.otherToolsToolStripMenuItem.Name = "otherToolsToolStripMenuItem";
			this.otherToolsToolStripMenuItem.Click += new global::System.EventHandler(this.otherToolsToolStripMenuItem_Click);
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
			componentResourceManager.ApplyResources(this.wGQRReaderConfigureV892AboveToolStripMenuItem, "wGQRReaderConfigureV892AboveToolStripMenuItem");
			this.wGQRReaderConfigureV892AboveToolStripMenuItem.Name = "wGQRReaderConfigureV892AboveToolStripMenuItem";
			this.wGQRReaderConfigureV892AboveToolStripMenuItem.Click += new global::System.EventHandler(this.wGQRReaderConfigureV892AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.dHCPToolStripMenuItem, "dHCPToolStripMenuItem");
			this.dHCPToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.disableDHCPDefaultToolStripMenuItem, this.enableDHCPToolStripMenuItem });
			this.dHCPToolStripMenuItem.Name = "dHCPToolStripMenuItem";
			componentResourceManager.ApplyResources(this.disableDHCPDefaultToolStripMenuItem, "disableDHCPDefaultToolStripMenuItem");
			this.disableDHCPDefaultToolStripMenuItem.Name = "disableDHCPDefaultToolStripMenuItem";
			this.disableDHCPDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.enableDHCPToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.enableDHCPToolStripMenuItem, "enableDHCPToolStripMenuItem");
			this.enableDHCPToolStripMenuItem.Name = "enableDHCPToolStripMenuItem";
			this.enableDHCPToolStripMenuItem.Click += new global::System.EventHandler(this.enableDHCPToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.invalidSwipeOpenDoorV896AboveToolStripMenuItem, "invalidSwipeOpenDoorV896AboveToolStripMenuItem");
			this.invalidSwipeOpenDoorV896AboveToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.disableInvalidSwipeOpenDoorToolStripMenuItem, this.enableInvalidSwipeOpenDoorToolStripMenuItem });
			this.invalidSwipeOpenDoorV896AboveToolStripMenuItem.Name = "invalidSwipeOpenDoorV896AboveToolStripMenuItem";
			componentResourceManager.ApplyResources(this.disableInvalidSwipeOpenDoorToolStripMenuItem, "disableInvalidSwipeOpenDoorToolStripMenuItem");
			this.disableInvalidSwipeOpenDoorToolStripMenuItem.Name = "disableInvalidSwipeOpenDoorToolStripMenuItem";
			this.disableInvalidSwipeOpenDoorToolStripMenuItem.Click += new global::System.EventHandler(this.enableInvalidSwipeOpenDoorToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.enableInvalidSwipeOpenDoorToolStripMenuItem, "enableInvalidSwipeOpenDoorToolStripMenuItem");
			this.enableInvalidSwipeOpenDoorToolStripMenuItem.Name = "enableInvalidSwipeOpenDoorToolStripMenuItem";
			this.enableInvalidSwipeOpenDoorToolStripMenuItem.Click += new global::System.EventHandler(this.enableInvalidSwipeOpenDoorToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.rS232ConfigureToolStripMenuItem, "rS232ConfigureToolStripMenuItem");
			this.rS232ConfigureToolStripMenuItem.Name = "rS232ConfigureToolStripMenuItem";
			this.rS232ConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.rS232ConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.option19, "option19");
			this.option19.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.wIFIToolStripMenuItem, this.setAlarmOffDelayToolStripMenuItem, this.limit2HourOutForSwimmingPoolToolStripMenuItem, this.deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem, this.activeElevatorAsSwitchV899AboveToolStripMenuItem, this.get656InfoToolStripMenuItem, this.inputOldDeviceInformationToolStripMenuItem });
			this.option19.Name = "option19";
			componentResourceManager.ApplyResources(this.wIFIToolStripMenuItem, "wIFIToolStripMenuItem");
			this.wIFIToolStripMenuItem.Name = "wIFIToolStripMenuItem";
			this.wIFIToolStripMenuItem.Click += new global::System.EventHandler(this.wIFIToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setAlarmOffDelayToolStripMenuItem, "setAlarmOffDelayToolStripMenuItem");
			this.setAlarmOffDelayToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.setAlarmOffDelay30SecDefaultToolStripMenuItem, this.setAlarmOffDelay60SecToolStripMenuItem });
			this.setAlarmOffDelayToolStripMenuItem.Name = "setAlarmOffDelayToolStripMenuItem";
			componentResourceManager.ApplyResources(this.setAlarmOffDelay30SecDefaultToolStripMenuItem, "setAlarmOffDelay30SecDefaultToolStripMenuItem");
			this.setAlarmOffDelay30SecDefaultToolStripMenuItem.Name = "setAlarmOffDelay30SecDefaultToolStripMenuItem";
			this.setAlarmOffDelay30SecDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.setAlarmOffDelay30SecDefaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setAlarmOffDelay60SecToolStripMenuItem, "setAlarmOffDelay60SecToolStripMenuItem");
			this.setAlarmOffDelay60SecToolStripMenuItem.Name = "setAlarmOffDelay60SecToolStripMenuItem";
			this.setAlarmOffDelay60SecToolStripMenuItem.Click += new global::System.EventHandler(this.setAlarmOffDelay30SecDefaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.limit2HourOutForSwimmingPoolToolStripMenuItem, "limit2HourOutForSwimmingPoolToolStripMenuItem");
			this.limit2HourOutForSwimmingPoolToolStripMenuItem.Name = "limit2HourOutForSwimmingPoolToolStripMenuItem";
			this.limit2HourOutForSwimmingPoolToolStripMenuItem.Click += new global::System.EventHandler(this.limit2HourOutForSwimmingPoolToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem, "deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem");
			this.deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem.Name = "deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem";
			this.deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem.Click += new global::System.EventHandler(this.deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.activeElevatorAsSwitchV899AboveToolStripMenuItem, "activeElevatorAsSwitchV899AboveToolStripMenuItem");
			this.activeElevatorAsSwitchV899AboveToolStripMenuItem.Name = "activeElevatorAsSwitchV899AboveToolStripMenuItem";
			this.activeElevatorAsSwitchV899AboveToolStripMenuItem.Click += new global::System.EventHandler(this.activeElevatorAsSwitchV899AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.get656InfoToolStripMenuItem, "get656InfoToolStripMenuItem");
			this.get656InfoToolStripMenuItem.Name = "get656InfoToolStripMenuItem";
			this.get656InfoToolStripMenuItem.Click += new global::System.EventHandler(this.get656InfoToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.inputOldDeviceInformationToolStripMenuItem, "inputOldDeviceInformationToolStripMenuItem");
			this.inputOldDeviceInformationToolStripMenuItem.Name = "inputOldDeviceInformationToolStripMenuItem";
			this.inputOldDeviceInformationToolStripMenuItem.Click += new global::System.EventHandler(this.inputOldDeviceInformationToolStripMenuItem_Click);
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
			this.readerInputFormatToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.restoreAsDefaultToolStripMenuItem, this.setReaderFormatWG26NoCheckToolStripMenuItem, this.setSpeacialToolStripMenuItem, this.setReaderFormat35BitsToolStripMenuItem, this.setReaderFormatHID36BitsToolStripMenuItem, this.setReaderFormatWG26FromWG34ToolStripMenuItem });
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
			componentResourceManager.ApplyResources(this.setReaderFormatHID36BitsToolStripMenuItem, "setReaderFormatHID36BitsToolStripMenuItem");
			this.setReaderFormatHID36BitsToolStripMenuItem.Name = "setReaderFormatHID36BitsToolStripMenuItem";
			this.setReaderFormatHID36BitsToolStripMenuItem.Click += new global::System.EventHandler(this.setSpeacialToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setReaderFormatWG26FromWG34ToolStripMenuItem, "setReaderFormatWG26FromWG34ToolStripMenuItem");
			this.setReaderFormatWG26FromWG34ToolStripMenuItem.Name = "setReaderFormatWG26FromWG34ToolStripMenuItem";
			this.setReaderFormatWG26FromWG34ToolStripMenuItem.Click += new global::System.EventHandler(this.setSpeacialToolStripMenuItem_Click);
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
			componentResourceManager.ApplyResources(this.custTypeToolStripMenuItem, "custTypeToolStripMenuItem");
			this.custTypeToolStripMenuItem.Name = "custTypeToolStripMenuItem";
			componentResourceManager.ApplyResources(this.twoCardCheckV885AboveToolStripMenuItem, "twoCardCheckV885AboveToolStripMenuItem");
			this.twoCardCheckV885AboveToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.twoCardCheckRestoreDefaultToolStripMenuItem, this.twoCardCheckOneByOneToolStripMenuItem, this.twoCardCheckMoreToolStripMenuItem, this.twoCardCheckCustomToolStripMenuItem });
			this.twoCardCheckV885AboveToolStripMenuItem.Name = "twoCardCheckV885AboveToolStripMenuItem";
			this.twoCardCheckV885AboveToolStripMenuItem.Click += new global::System.EventHandler(this.twoCardCheckV885AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.twoCardCheckRestoreDefaultToolStripMenuItem, "twoCardCheckRestoreDefaultToolStripMenuItem");
			this.twoCardCheckRestoreDefaultToolStripMenuItem.Name = "twoCardCheckRestoreDefaultToolStripMenuItem";
			this.twoCardCheckRestoreDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.twoCardCheckV885AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.twoCardCheckOneByOneToolStripMenuItem, "twoCardCheckOneByOneToolStripMenuItem");
			this.twoCardCheckOneByOneToolStripMenuItem.Name = "twoCardCheckOneByOneToolStripMenuItem";
			this.twoCardCheckOneByOneToolStripMenuItem.Click += new global::System.EventHandler(this.twoCardCheckV885AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.twoCardCheckMoreToolStripMenuItem, "twoCardCheckMoreToolStripMenuItem");
			this.twoCardCheckMoreToolStripMenuItem.Name = "twoCardCheckMoreToolStripMenuItem";
			this.twoCardCheckMoreToolStripMenuItem.Click += new global::System.EventHandler(this.twoCardCheckV885AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.twoCardCheckCustomToolStripMenuItem, "twoCardCheckCustomToolStripMenuItem");
			this.twoCardCheckCustomToolStripMenuItem.Name = "twoCardCheckCustomToolStripMenuItem";
			this.twoCardCheckCustomToolStripMenuItem.Click += new global::System.EventHandler(this.twoCardCheckV885AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.pwd468CheckV890above, "pwd468CheckV890above");
			this.pwd468CheckV890above.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.pwd468CheckV890aboveDeactive, this.pwd468CheckV890above4, this.pwd468CheckV890above6, this.pwd468CheckV890above8, this.pwd468CheckV890aboveCustom });
			this.pwd468CheckV890above.Name = "pwd468CheckV890above";
			this.pwd468CheckV890above.Click += new global::System.EventHandler(this.pwd468CheckV890above_Click);
			componentResourceManager.ApplyResources(this.pwd468CheckV890aboveDeactive, "pwd468CheckV890aboveDeactive");
			this.pwd468CheckV890aboveDeactive.Name = "pwd468CheckV890aboveDeactive";
			this.pwd468CheckV890aboveDeactive.Click += new global::System.EventHandler(this.pwd468CheckV890above_Click);
			componentResourceManager.ApplyResources(this.pwd468CheckV890above4, "pwd468CheckV890above4");
			this.pwd468CheckV890above4.Name = "pwd468CheckV890above4";
			this.pwd468CheckV890above4.Click += new global::System.EventHandler(this.pwd468CheckV890above_Click);
			componentResourceManager.ApplyResources(this.pwd468CheckV890above6, "pwd468CheckV890above6");
			this.pwd468CheckV890above6.Name = "pwd468CheckV890above6";
			this.pwd468CheckV890above6.Click += new global::System.EventHandler(this.pwd468CheckV890above_Click);
			componentResourceManager.ApplyResources(this.pwd468CheckV890above8, "pwd468CheckV890above8");
			this.pwd468CheckV890above8.Name = "pwd468CheckV890above8";
			this.pwd468CheckV890above8.Click += new global::System.EventHandler(this.pwd468CheckV890above_Click);
			componentResourceManager.ApplyResources(this.pwd468CheckV890aboveCustom, "pwd468CheckV890aboveCustom");
			this.pwd468CheckV890aboveCustom.Name = "pwd468CheckV890aboveCustom";
			this.pwd468CheckV890aboveCustom.Click += new global::System.EventHandler(this.pwd468CheckV890above_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			componentResourceManager.ApplyResources(this.cloudServerConfigureToolStripMenuItem, "cloudServerConfigureToolStripMenuItem");
			this.cloudServerConfigureToolStripMenuItem.Name = "cloudServerConfigureToolStripMenuItem";
			this.cloudServerConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.cloudServerConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.hTTPServerConfigureV882AboveToolStripMenuItem, "hTTPServerConfigureV882AboveToolStripMenuItem");
			this.hTTPServerConfigureV882AboveToolStripMenuItem.Name = "hTTPServerConfigureV882AboveToolStripMenuItem";
			this.hTTPServerConfigureV882AboveToolStripMenuItem.Click += new global::System.EventHandler(this.hTTPServerConfigureV882AboveToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.statusStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_bottom;
			this.statusStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripStatusLabel1, this.toolStripStatusLabel2 });
			this.statusStrip1.Name = "statusStrip1";
			componentResourceManager.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
			this.toolStripStatusLabel1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripStatusLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
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
			componentResourceManager.ApplyResources(this.btnFormatSpecial, "btnFormatSpecial");
			this.btnFormatSpecial.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFormatSpecial.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFormatSpecial.ForeColor = global::System.Drawing.Color.White;
			this.btnFormatSpecial.Name = "btnFormatSpecial";
			this.btnFormatSpecial.UseVisualStyleBackColor = false;
			this.btnFormatSpecial.Click += new global::System.EventHandler(this.btnFormatSpecial_Click);
			componentResourceManager.ApplyResources(this.txtFormatSpecial, "txtFormatSpecial");
			this.txtFormatSpecial.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 128);
			this.txtFormatSpecial.Name = "txtFormatSpecial";
			componentResourceManager.ApplyResources(this.btnSetProductType, "btnSetProductType");
			this.btnSetProductType.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSetProductType.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSetProductType.ForeColor = global::System.Drawing.Color.White;
			this.btnSetProductType.Name = "btnSetProductType";
			this.btnSetProductType.UseVisualStyleBackColor = false;
			this.btnSetProductType.Click += new global::System.EventHandler(this.btnSetProductType_Click);
			componentResourceManager.ApplyResources(this.btnGetTypeInfo, "btnGetTypeInfo");
			this.btnGetTypeInfo.BackColor = global::System.Drawing.Color.Transparent;
			this.btnGetTypeInfo.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnGetTypeInfo.ForeColor = global::System.Drawing.Color.White;
			this.btnGetTypeInfo.Name = "btnGetTypeInfo";
			this.btnGetTypeInfo.UseVisualStyleBackColor = false;
			this.btnGetTypeInfo.Click += new global::System.EventHandler(this.btnGetTypeInfo_Click);
			componentResourceManager.ApplyResources(this.btnTestNewCustTypeController, "btnTestNewCustTypeController");
			this.btnTestNewCustTypeController.BackColor = global::System.Drawing.Color.Transparent;
			this.btnTestNewCustTypeController.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnTestNewCustTypeController.ForeColor = global::System.Drawing.Color.White;
			this.btnTestNewCustTypeController.Name = "btnTestNewCustTypeController";
			this.btnTestNewCustTypeController.UseVisualStyleBackColor = false;
			this.btnTestNewCustTypeController.Click += new global::System.EventHandler(this.btnTestNewCustTypeController_Click);
			componentResourceManager.ApplyResources(this.btnResetProductType, "btnResetProductType");
			this.btnResetProductType.BackColor = global::System.Drawing.Color.Transparent;
			this.btnResetProductType.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnResetProductType.ForeColor = global::System.Drawing.Color.White;
			this.btnResetProductType.Name = "btnResetProductType";
			this.btnResetProductType.UseVisualStyleBackColor = false;
			this.btnResetProductType.Click += new global::System.EventHandler(this.btnSetProductType_Click);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.btnResetProductType);
			base.Controls.Add(this.btnTestNewCustTypeController);
			base.Controls.Add(this.btnGetTypeInfo);
			base.Controls.Add(this.btnSetProductType);
			base.Controls.Add(this.txtFormatSpecial);
			base.Controls.Add(this.btnFormatSpecial);
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
			base.Controls.Add(this.dgvFoundControllers);
			base.Name = "dfrmNetControllerConfig";
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

		// Token: 0x04000309 RID: 777
		private global::WG3000_COMM.Core.wgUdpComm wgudp;

		// Token: 0x04000321 RID: 801
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000322 RID: 802
		private global::System.Windows.Forms.ToolStripMenuItem activeElevatorAsSwitchV899AboveToolStripMenuItem;

		// Token: 0x04000323 RID: 803
		private global::System.Windows.Forms.ToolStripMenuItem addSelectedToSystemToolStripMenuItem;

		// Token: 0x04000324 RID: 804
		private global::System.Windows.Forms.ToolStripMenuItem autoSetIPNotCheckDuplicateToolStripMenuItem;

		// Token: 0x04000325 RID: 805
		private global::System.Windows.Forms.ToolStripMenuItem autoSetIPToolStripMenuItem;

		// Token: 0x04000326 RID: 806
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectedIPInDBToolStripMenuItem;

		// Token: 0x04000327 RID: 807
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectToolStripMenuItem;

		// Token: 0x04000328 RID: 808
		public global::System.Windows.Forms.Button btnAddToSystem;

		// Token: 0x04000329 RID: 809
		public global::System.Windows.Forms.Button btnFormatSpecial;

		// Token: 0x0400032A RID: 810
		public global::System.Windows.Forms.Button btnGetTypeInfo;

		// Token: 0x0400032B RID: 811
		public global::System.Windows.Forms.Button btnIPAndWebConfigure;

		// Token: 0x0400032C RID: 812
		public global::System.Windows.Forms.Button btnResetProductType;

		// Token: 0x0400032D RID: 813
		public global::System.Windows.Forms.Button btnSetProductType;

		// Token: 0x0400032E RID: 814
		public global::System.Windows.Forms.Button btnTestNewCustTypeController;

		// Token: 0x0400032F RID: 815
		private global::System.Windows.Forms.Button btnConfigure;

		// Token: 0x04000330 RID: 816
		private global::System.Windows.Forms.Button btnDefault;

		// Token: 0x04000331 RID: 817
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04000332 RID: 818
		private global::System.Windows.Forms.Button btnSearch;

		// Token: 0x04000333 RID: 819
		private global::System.Windows.Forms.CheckBox chkSearchAgain;

		// Token: 0x04000334 RID: 820
		private global::System.Windows.Forms.ToolStripMenuItem clearSwipesToolStripMenuItem;

		// Token: 0x04000335 RID: 821
		private global::System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;

		// Token: 0x04000336 RID: 822
		private global::System.Windows.Forms.ToolStripMenuItem cloudServerConfigureToolStripMenuItem;

		// Token: 0x04000337 RID: 823
		private global::System.Windows.Forms.ToolStripMenuItem communicationTestToolStripMenuItem;

		// Token: 0x04000338 RID: 824
		private global::System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;

		// Token: 0x04000339 RID: 825
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x0400033A RID: 826
		private global::System.Windows.Forms.ToolStripMenuItem custTypeToolStripMenuItem;

		// Token: 0x0400033B RID: 827
		private global::System.Windows.Forms.ToolStripMenuItem deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem;

		// Token: 0x0400033C RID: 828
		private global::System.Windows.Forms.DataGridView dgvFoundControllers;

		// Token: 0x0400033D RID: 829
		private global::System.Windows.Forms.ToolStripMenuItem dHCPToolStripMenuItem;

		// Token: 0x0400033E RID: 830
		private global::System.Windows.Forms.ToolStripMenuItem disableDHCPDefaultToolStripMenuItem;

		// Token: 0x0400033F RID: 831
		private global::System.Windows.Forms.ToolStripMenuItem disableInvalidSwipeOpenDoorToolStripMenuItem;

		// Token: 0x04000340 RID: 832
		private global::System.Windows.Forms.ToolStripMenuItem disableMasterClientToolStripMenuItem;

		// Token: 0x04000341 RID: 833
		private global::System.Windows.Forms.ToolStripMenuItem disableMobileOpenDoorToolStripMenuItem;

		// Token: 0x04000342 RID: 834
		private global::System.Windows.Forms.ToolStripMenuItem disablePCControlSwipeToolStripMenuItem;

		// Token: 0x04000343 RID: 835
		private global::System.Windows.Forms.ToolStripMenuItem disableSwipeInOrderToolStripMenuItem;

		// Token: 0x04000345 RID: 837
		private global::System.Windows.Forms.ToolStripMenuItem enableDHCPToolStripMenuItem;

		// Token: 0x04000346 RID: 838
		private global::System.Windows.Forms.ToolStripMenuItem enabledPCControlSwipeToolStripMenuItem;

		// Token: 0x04000347 RID: 839
		private global::System.Windows.Forms.ToolStripMenuItem enableInvalidSwipeOpenDoorToolStripMenuItem;

		// Token: 0x04000348 RID: 840
		private global::System.Windows.Forms.ToolStripMenuItem enableMobileOpenDoorToolStripMenuItem;

		// Token: 0x04000349 RID: 841
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x0400034A RID: 842
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Gateway;

		// Token: 0x0400034B RID: 843
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ID;

		// Token: 0x0400034C RID: 844
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x0400034D RID: 845
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MACAddr;

		// Token: 0x0400034E RID: 846
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Mask;

		// Token: 0x0400034F RID: 847
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x04000350 RID: 848
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PCIPAddr;

		// Token: 0x04000351 RID: 849
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;

		// Token: 0x04000352 RID: 850
		private global::System.Windows.Forms.ToolStripMenuItem findF3ToolStripMenuItem;

		// Token: 0x04000353 RID: 851
		private global::System.Windows.Forms.ToolStripMenuItem force100MV552AboveToolStripMenuItem;

		// Token: 0x04000354 RID: 852
		private global::System.Windows.Forms.ToolStripMenuItem force10MHalfDuplexToolStripMenuItem;

		// Token: 0x04000355 RID: 853
		private global::System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;

		// Token: 0x04000356 RID: 854
		private global::System.Windows.Forms.ToolStripMenuItem get656InfoToolStripMenuItem;

		// Token: 0x04000357 RID: 855
		private global::System.Windows.Forms.ToolStripMenuItem hideControllerToolStripMenuItem;

		// Token: 0x04000358 RID: 856
		private global::System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;

		// Token: 0x04000359 RID: 857
		private global::System.Windows.Forms.ToolStripMenuItem hTTPServerConfigureV882AboveToolStripMenuItem;

		// Token: 0x0400035A RID: 858
		private global::System.Windows.Forms.ToolStripMenuItem inputOldDeviceInformationToolStripMenuItem;

		// Token: 0x0400035B RID: 859
		private global::System.Windows.Forms.ToolStripMenuItem invalidSwipeOpenDoorV896AboveToolStripMenuItem;

		// Token: 0x0400035C RID: 860
		private global::System.Windows.Forms.ToolStripMenuItem iPFilterToolStripMenuItem;

		// Token: 0x0400035D RID: 861
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400035E RID: 862
		private global::System.Windows.Forms.Label lblCount;

		// Token: 0x0400035F RID: 863
		private global::System.Windows.Forms.Label lblSearchNow;

		// Token: 0x04000360 RID: 864
		private global::System.Windows.Forms.ToolStripMenuItem limit2HourOutForSwimmingPoolToolStripMenuItem;

		// Token: 0x04000361 RID: 865
		private global::System.Windows.Forms.ToolStripMenuItem masterClientControllerSetToolStripMenuItem;

		// Token: 0x04000362 RID: 866
		private global::System.Windows.Forms.ToolStripMenuItem mConfigureToolStripMenuItem;

		// Token: 0x04000363 RID: 867
		private global::System.Windows.Forms.ToolStripMenuItem multicardSwipeGapToolStripMenuItem;

		// Token: 0x04000364 RID: 868
		private global::System.Windows.Forms.ToolStripMenuItem option19;

		// Token: 0x04000365 RID: 869
		private global::System.Windows.Forms.ToolStripMenuItem otherToolsToolStripMenuItem;

		// Token: 0x04000366 RID: 870
		private global::System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;

		// Token: 0x04000367 RID: 871
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000368 RID: 872
		private global::System.Windows.Forms.ToolStripMenuItem pCControlSwipeToolStripMenuItem;

		// Token: 0x04000369 RID: 873
		private global::System.Windows.Forms.DataGridViewTextBoxColumn PCMacAddr;

		// Token: 0x0400036A RID: 874
		private global::System.Windows.Forms.ToolStripMenuItem pwd468CheckV890above;

		// Token: 0x0400036B RID: 875
		private global::System.Windows.Forms.ToolStripMenuItem pwd468CheckV890above4;

		// Token: 0x0400036C RID: 876
		private global::System.Windows.Forms.ToolStripMenuItem pwd468CheckV890above6;

		// Token: 0x0400036D RID: 877
		private global::System.Windows.Forms.ToolStripMenuItem pwd468CheckV890above8;

		// Token: 0x0400036E RID: 878
		private global::System.Windows.Forms.ToolStripMenuItem pwd468CheckV890aboveCustom;

		// Token: 0x0400036F RID: 879
		private global::System.Windows.Forms.ToolStripMenuItem pwd468CheckV890aboveDeactive;

		// Token: 0x04000370 RID: 880
		private global::System.Windows.Forms.ToolStripMenuItem quickFormatToolStripMenuItem;

		// Token: 0x04000371 RID: 881
		private global::System.Windows.Forms.ToolStripMenuItem readerInputFormatToolStripMenuItem;

		// Token: 0x04000372 RID: 882
		private global::System.Windows.Forms.ToolStripMenuItem restore100M10MAutoToolStripMenuItem;

		// Token: 0x04000373 RID: 883
		private global::System.Windows.Forms.ToolStripMenuItem restoreAllSwipesToolStripMenuItem;

		// Token: 0x04000374 RID: 884
		private global::System.Windows.Forms.ToolStripMenuItem restoreAsDefaultToolStripMenuItem;

		// Token: 0x04000375 RID: 885
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultIPToolStripMenuItem;

		// Token: 0x04000376 RID: 886
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultParamToolStripMenuItem;

		// Token: 0x04000377 RID: 887
		private global::System.Windows.Forms.ToolStripMenuItem rS232ConfigureToolStripMenuItem;

		// Token: 0x04000378 RID: 888
		private global::System.Windows.Forms.ToolStripMenuItem search100FromTheSpecialSNToolStripMenuItem;

		// Token: 0x04000379 RID: 889
		private global::System.Windows.Forms.ToolStripMenuItem searchAdvancedToolStripMenuItem;

		// Token: 0x0400037A RID: 890
		private global::System.Windows.Forms.ToolStripMenuItem searchSpecialSNToolStripMenuItem;

		// Token: 0x0400037B RID: 891
		private global::System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;

		// Token: 0x0400037C RID: 892
		private global::System.Windows.Forms.ToolStripMenuItem setAlarmOffDelay30SecDefaultToolStripMenuItem;

		// Token: 0x0400037D RID: 893
		private global::System.Windows.Forms.ToolStripMenuItem setAlarmOffDelay60SecToolStripMenuItem;

		// Token: 0x0400037E RID: 894
		private global::System.Windows.Forms.ToolStripMenuItem setAlarmOffDelayToolStripMenuItem;

		// Token: 0x0400037F RID: 895
		private global::System.Windows.Forms.ToolStripMenuItem setAsClientToolStripMenuItem;

		// Token: 0x04000380 RID: 896
		private global::System.Windows.Forms.ToolStripMenuItem setAsMasterToolStripMenuItem;

		// Token: 0x04000381 RID: 897
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormat35BitsToolStripMenuItem;

		// Token: 0x04000382 RID: 898
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormatHID36BitsToolStripMenuItem;

		// Token: 0x04000383 RID: 899
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormatWG26FromWG34ToolStripMenuItem;

		// Token: 0x04000384 RID: 900
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormatWG26NoCheckToolStripMenuItem;

		// Token: 0x04000385 RID: 901
		private global::System.Windows.Forms.ToolStripMenuItem setSpeacialToolStripMenuItem;

		// Token: 0x04000386 RID: 902
		private global::System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;

		// Token: 0x04000387 RID: 903
		private global::System.Windows.Forms.ToolStripMenuItem sMSConfigureToolStripMenuItem;

		// Token: 0x04000388 RID: 904
		private global::System.Windows.Forms.StatusStrip statusStrip1;

		// Token: 0x04000389 RID: 905
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderMode1Reader1212ToolStripMenuItem;

		// Token: 0x0400038A RID: 906
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderMode2Reader134213ToolStripMenuItem;

		// Token: 0x0400038B RID: 907
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderToolStripMenuItem;

		// Token: 0x0400038C RID: 908
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x0400038D RID: 909
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x0400038E RID: 910
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x0400038F RID: 911
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

		// Token: 0x04000390 RID: 912
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

		// Token: 0x04000391 RID: 913
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;

		// Token: 0x04000392 RID: 914
		private global::System.Windows.Forms.ToolStripMenuItem twoCardCheckCustomToolStripMenuItem;

		// Token: 0x04000393 RID: 915
		private global::System.Windows.Forms.ToolStripMenuItem twoCardCheckMoreToolStripMenuItem;

		// Token: 0x04000394 RID: 916
		private global::System.Windows.Forms.ToolStripMenuItem twoCardCheckOneByOneToolStripMenuItem;

		// Token: 0x04000395 RID: 917
		private global::System.Windows.Forms.ToolStripMenuItem twoCardCheckRestoreDefaultToolStripMenuItem;

		// Token: 0x04000396 RID: 918
		private global::System.Windows.Forms.ToolStripMenuItem twoCardCheckV885AboveToolStripMenuItem;

		// Token: 0x04000397 RID: 919
		private global::System.Windows.Forms.TextBox txtFormatSpecial;

		// Token: 0x04000398 RID: 920
		private global::System.Windows.Forms.ToolStripMenuItem wGQRReaderConfigureV892AboveToolStripMenuItem;

		// Token: 0x04000399 RID: 921
		private global::System.Windows.Forms.ToolStripMenuItem wIFIToolStripMenuItem;
	}
}
