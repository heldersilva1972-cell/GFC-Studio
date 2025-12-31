namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002EC RID: 748
	public partial class dfrmNetControllerConfig4FingerPrint : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600159E RID: 5534 RVA: 0x001B1836 File Offset: 0x001B0836
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

		// Token: 0x0600159F RID: 5535 RVA: 0x001B186C File Offset: 0x001B086C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Finger.dfrmNetControllerConfig4FingerPrint));
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
			this.otherToolsToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.enableMobileOpenDoorToolStripMenuItem, this.multicardSwipeGapToolStripMenuItem, this.masterClientControllerSetToolStripMenuItem, this.pCControlSwipeToolStripMenuItem, this.swipeInOrderToolStripMenuItem, this.dHCPToolStripMenuItem });
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
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.btnIPAndWebConfigure);
			base.Controls.Add(this.lblCount);
			base.Controls.Add(this.lblSearchNow);
			base.Controls.Add(this.chkSearchAgain);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnAddToSystem);
			base.Controls.Add(this.dgvFoundControllers);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnConfigure);
			base.Controls.Add(this.btnDefault);
			base.Controls.Add(this.btnSearch);
			base.Name = "dfrmNetControllerConfig4FingerPrint";
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

		// Token: 0x04002C5E RID: 11358
		private global::WG3000_COMM.Core.wgUdpComm wgudp;

		// Token: 0x04002C75 RID: 11381
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002C76 RID: 11382
		private global::System.Windows.Forms.ToolStripMenuItem addSelectedToSystemToolStripMenuItem;

		// Token: 0x04002C77 RID: 11383
		private global::System.Windows.Forms.ToolStripMenuItem autoSetIPNotCheckDuplicateToolStripMenuItem;

		// Token: 0x04002C78 RID: 11384
		private global::System.Windows.Forms.ToolStripMenuItem autoSetIPToolStripMenuItem;

		// Token: 0x04002C79 RID: 11385
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectedIPInDBToolStripMenuItem;

		// Token: 0x04002C7A RID: 11386
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectToolStripMenuItem;

		// Token: 0x04002C7B RID: 11387
		private global::System.Windows.Forms.Button btnConfigure;

		// Token: 0x04002C7C RID: 11388
		private global::System.Windows.Forms.Button btnDefault;

		// Token: 0x04002C7D RID: 11389
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04002C7E RID: 11390
		private global::System.Windows.Forms.Button btnSearch;

		// Token: 0x04002C7F RID: 11391
		private global::System.Windows.Forms.CheckBox chkSearchAgain;

		// Token: 0x04002C80 RID: 11392
		private global::System.Windows.Forms.ToolStripMenuItem clearSwipesToolStripMenuItem;

		// Token: 0x04002C81 RID: 11393
		private global::System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;

		// Token: 0x04002C82 RID: 11394
		private global::System.Windows.Forms.ToolStripMenuItem communicationTestToolStripMenuItem;

		// Token: 0x04002C83 RID: 11395
		private global::System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;

		// Token: 0x04002C84 RID: 11396
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04002C85 RID: 11397
		private global::System.Windows.Forms.DataGridView dgvFoundControllers;

		// Token: 0x04002C86 RID: 11398
		private global::System.Windows.Forms.ToolStripMenuItem dHCPToolStripMenuItem;

		// Token: 0x04002C87 RID: 11399
		private global::System.Windows.Forms.ToolStripMenuItem disableDHCPDefaultToolStripMenuItem;

		// Token: 0x04002C88 RID: 11400
		private global::System.Windows.Forms.ToolStripMenuItem disableMasterClientToolStripMenuItem;

		// Token: 0x04002C89 RID: 11401
		private global::System.Windows.Forms.ToolStripMenuItem disableMobileOpenDoorToolStripMenuItem;

		// Token: 0x04002C8A RID: 11402
		private global::System.Windows.Forms.ToolStripMenuItem disablePCControlSwipeToolStripMenuItem;

		// Token: 0x04002C8B RID: 11403
		private global::System.Windows.Forms.ToolStripMenuItem disableSwipeInOrderToolStripMenuItem;

		// Token: 0x04002C8C RID: 11404
		private global::System.Windows.Forms.ToolStripMenuItem enableDHCPToolStripMenuItem;

		// Token: 0x04002C8D RID: 11405
		private global::System.Windows.Forms.ToolStripMenuItem enabledPCControlSwipeToolStripMenuItem;

		// Token: 0x04002C8E RID: 11406
		private global::System.Windows.Forms.ToolStripMenuItem enableMobileOpenDoorToolStripMenuItem;

		// Token: 0x04002C8F RID: 11407
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04002C90 RID: 11408
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Gateway;

		// Token: 0x04002C91 RID: 11409
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ID;

		// Token: 0x04002C92 RID: 11410
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x04002C93 RID: 11411
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MACAddr;

		// Token: 0x04002C94 RID: 11412
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Mask;

		// Token: 0x04002C95 RID: 11413
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x04002C96 RID: 11414
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PCIPAddr;

		// Token: 0x04002C97 RID: 11415
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;

		// Token: 0x04002C98 RID: 11416
		private global::System.Windows.Forms.ToolStripMenuItem findF3ToolStripMenuItem;

		// Token: 0x04002C99 RID: 11417
		private global::System.Windows.Forms.ToolStripMenuItem force100MV552AboveToolStripMenuItem;

		// Token: 0x04002C9A RID: 11418
		private global::System.Windows.Forms.ToolStripMenuItem force10MHalfDuplexToolStripMenuItem;

		// Token: 0x04002C9B RID: 11419
		private global::System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;

		// Token: 0x04002C9D RID: 11421
		private global::System.Windows.Forms.ToolStripMenuItem hideControllerToolStripMenuItem;

		// Token: 0x04002C9E RID: 11422
		private global::System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;

		// Token: 0x04002C9F RID: 11423
		private global::System.Windows.Forms.ToolStripMenuItem iPFilterToolStripMenuItem;

		// Token: 0x04002CA0 RID: 11424
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002CA1 RID: 11425
		private global::System.Windows.Forms.Label lblCount;

		// Token: 0x04002CA2 RID: 11426
		private global::System.Windows.Forms.Label lblSearchNow;

		// Token: 0x04002CA3 RID: 11427
		private global::System.Windows.Forms.ToolStripMenuItem masterClientControllerSetToolStripMenuItem;

		// Token: 0x04002CA4 RID: 11428
		private global::System.Windows.Forms.ToolStripMenuItem mConfigureToolStripMenuItem;

		// Token: 0x04002CA5 RID: 11429
		private global::System.Windows.Forms.ToolStripMenuItem multicardSwipeGapToolStripMenuItem;

		// Token: 0x04002CA6 RID: 11430
		private global::System.Windows.Forms.ToolStripMenuItem otherToolsToolStripMenuItem;

		// Token: 0x04002CA7 RID: 11431
		private global::System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;

		// Token: 0x04002CA8 RID: 11432
		private global::System.Windows.Forms.ToolStripMenuItem pCControlSwipeToolStripMenuItem;

		// Token: 0x04002CA9 RID: 11433
		private global::System.Windows.Forms.DataGridViewTextBoxColumn PCMacAddr;

		// Token: 0x04002CAA RID: 11434
		private global::System.Windows.Forms.ToolStripMenuItem quickFormatToolStripMenuItem;

		// Token: 0x04002CAB RID: 11435
		private global::System.Windows.Forms.ToolStripMenuItem readerInputFormatToolStripMenuItem;

		// Token: 0x04002CAC RID: 11436
		private global::System.Windows.Forms.ToolStripMenuItem restore100M10MAutoToolStripMenuItem;

		// Token: 0x04002CAD RID: 11437
		private global::System.Windows.Forms.ToolStripMenuItem restoreAllSwipesToolStripMenuItem;

		// Token: 0x04002CAE RID: 11438
		private global::System.Windows.Forms.ToolStripMenuItem restoreAsDefaultToolStripMenuItem;

		// Token: 0x04002CAF RID: 11439
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultIPToolStripMenuItem;

		// Token: 0x04002CB0 RID: 11440
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultParamToolStripMenuItem;

		// Token: 0x04002CB1 RID: 11441
		private global::System.Windows.Forms.ToolStripMenuItem search100FromTheSpecialSNToolStripMenuItem;

		// Token: 0x04002CB2 RID: 11442
		private global::System.Windows.Forms.ToolStripMenuItem searchAdvancedToolStripMenuItem;

		// Token: 0x04002CB3 RID: 11443
		private global::System.Windows.Forms.ToolStripMenuItem searchSpecialSNToolStripMenuItem;

		// Token: 0x04002CB4 RID: 11444
		private global::System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;

		// Token: 0x04002CB5 RID: 11445
		private global::System.Windows.Forms.ToolStripMenuItem setAsClientToolStripMenuItem;

		// Token: 0x04002CB6 RID: 11446
		private global::System.Windows.Forms.ToolStripMenuItem setAsMasterToolStripMenuItem;

		// Token: 0x04002CB7 RID: 11447
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormat35BitsToolStripMenuItem;

		// Token: 0x04002CB8 RID: 11448
		private global::System.Windows.Forms.ToolStripMenuItem setReaderFormatWG26NoCheckToolStripMenuItem;

		// Token: 0x04002CB9 RID: 11449
		private global::System.Windows.Forms.ToolStripMenuItem setSpeacialToolStripMenuItem;

		// Token: 0x04002CBA RID: 11450
		private global::System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;

		// Token: 0x04002CBB RID: 11451
		private global::System.Windows.Forms.ToolStripMenuItem sMSConfigureToolStripMenuItem;

		// Token: 0x04002CBC RID: 11452
		private global::System.Windows.Forms.StatusStrip statusStrip1;

		// Token: 0x04002CBD RID: 11453
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderMode1Reader1212ToolStripMenuItem;

		// Token: 0x04002CBE RID: 11454
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderMode2Reader134213ToolStripMenuItem;

		// Token: 0x04002CBF RID: 11455
		private global::System.Windows.Forms.ToolStripMenuItem swipeInOrderToolStripMenuItem;

		// Token: 0x04002CC0 RID: 11456
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04002CC1 RID: 11457
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x04002CC2 RID: 11458
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x04002CC3 RID: 11459
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

		// Token: 0x04002CC4 RID: 11460
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;

		// Token: 0x04002CC5 RID: 11461
		public global::System.Windows.Forms.Button btnAddToSystem;

		// Token: 0x04002CC6 RID: 11462
		public global::System.Windows.Forms.Button btnIPAndWebConfigure;
	}
}
