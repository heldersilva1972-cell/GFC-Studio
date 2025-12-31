namespace WG3000_COMM.Basic
{
	// Token: 0x0200003E RID: 62
	public partial class frmADCT3000 : global::System.Windows.Forms.Form
	{
		// Token: 0x060004B5 RID: 1205 RVA: 0x0007B2C4 File Offset: 0x0007A2C4
		protected override void Dispose(bool disposing)
		{
			if (disposing && global::WG3000_COMM.Basic.frmADCT3000.watchingP64 != null && global::WG3000_COMM.Core.wgTools.bUDPOnly64 > 0)
			{
				global::WG3000_COMM.Basic.frmADCT3000.watchingP64.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0007B300 File Offset: 0x0007A300
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmADCT3000));
			this.contextMenuStrip1Tools = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmdChangePasswor = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdEditOperator = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdOperatorManage = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuDBBackup = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuOption = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnuExtendedFunction = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuElevator = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuMeetingSign = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuMeal = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuPatrol = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuCameraManage = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuPCCheckAccessConfigure = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuTaskList = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuDoorAsSwitch = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuLogQuery = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnuInterfaceLock = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton1 = new global::System.Windows.Forms.ToolStripDropDownButton();
			this.mnu1Tool = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteOldRecords = new global::System.Windows.Forms.ToolStripMenuItem();
			this.systemParamsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem19 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuAbout = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuManual = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuSystemCharacteristic = new global::System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip2Help = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnu1Help = new global::System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new global::System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem8 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolsFormToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolStrip1BookMark = new global::System.Windows.Forms.ToolStrip();
			this.contextMenuStrip3Normal = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.shortcutControllers = new global::System.Windows.Forms.ToolStripMenuItem();
			this.shortcutPersonnel = new global::System.Windows.Forms.ToolStripMenuItem();
			this.shortcutPrivilege = new global::System.Windows.Forms.ToolStripMenuItem();
			this.shortcutConsole = new global::System.Windows.Forms.ToolStripMenuItem();
			this.shortcutSwipe = new global::System.Windows.Forms.ToolStripMenuItem();
			this.shortcutAttendance = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator17 = new global::System.Windows.Forms.ToolStripSeparator();
			this.displayHideMenuToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayHideStatusBarToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayHideStarterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButtonBookmark1 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButtonBookmark2 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton5 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton7 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton6 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton8 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButtonBookmark3 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton9 = new global::System.Windows.Forms.ToolStripButton();
			this.flowLayoutPanel1ICon = new global::System.Windows.Forms.FlowLayoutPanel();
			this.grpGettingStarted = new global::System.Windows.Forms.GroupBox();
			this.btnHideGettingStarted = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnAddPrivilege = new global::System.Windows.Forms.Button();
			this.btnAutoAddCardBySwiping = new global::System.Windows.Forms.Button();
			this.btnAddController = new global::System.Windows.Forms.Button();
			this.btnIconBasicConfig = new global::System.Windows.Forms.Button();
			this.btnIconBasicOperate = new global::System.Windows.Forms.Button();
			this.btnIconAttendance = new global::System.Windows.Forms.Button();
			this.btnConstMeal = new global::System.Windows.Forms.Button();
			this.btnPatrol = new global::System.Windows.Forms.Button();
			this.btnMeeting = new global::System.Windows.Forms.Button();
			this.btnElevator = new global::System.Windows.Forms.Button();
			this.btnFingerPrint = new global::System.Windows.Forms.Button();
			this.btnFaceManagement = new global::System.Windows.Forms.Button();
			this.panel2Content = new global::System.Windows.Forms.Panel();
			this.panel4Form = new global::System.Windows.Forms.PictureBox();
			this.mnuMain = new global::System.Windows.Forms.MenuStrip();
			this.mnu1File = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2LogQuery = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2DBBackup = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2Exit = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu1Configure = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Controller = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Zones = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Departments = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Personnel = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2CardLost = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2AccessPrivilege = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2privilegeTypesManagementToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2TimeProfile = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2LimitedAccessTimes = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2AccessHolidayControl = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2PeripheralControl = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2PasswordManagement = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2AntiPassback = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2InterLock = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2MultiCard = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2FirstCardOpen = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2TaskList = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2DoorAsSwitch = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2GlobalAntipassback = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2Camera = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2CheckAccess = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu1Operate = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Console = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Monitor = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Check = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2AdjustTime = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Upload = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2GetRecords = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2RealtimeGetRecords = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2RemoteOpen = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Locate = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2PersonInside = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Maps = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2WarnOutputReset = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2ResetPersonInside = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2SetDoorControl = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2QuerySwipeRecords = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2InterfaceLock = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu1MultiFunc = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Attendence = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuAttendenceData = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnuShiftNormalConfigure = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuShiftRule = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuShiftSet = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuShiftArrange = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnuHolidaySet = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuLeave = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuManualCardRecord = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2ConstMeal = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Patrol = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Meeting = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2OneToMore = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu1Tools = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2OperatorManagement = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2EditOperator = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2ExtendedFunction = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2hideGettingStartedToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Language = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2InterfaceTitle = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2AutoLogin = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu1HelpA = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Beginner = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnu2Manual = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator15 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2SystemCharacteristic = new global::System.Windows.Forms.ToolStripMenuItem();
			this.produceUpdatesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.openContainingFolderToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.feedbackToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator16 = new global::System.Windows.Forms.ToolStripSeparator();
			this.mnu2About = new global::System.Windows.Forms.ToolStripMenuItem();
			this.interfaceLockToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.stbRunInfo = new global::System.Windows.Forms.StatusStrip();
			this.statOperator = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statSoftwareVer = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statCOM = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfo1 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfo2 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfo3 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfoLoadedNum = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statTimeDate = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.lblDiskSpace = new global::System.Windows.Forms.Label();
			this.lblFailedToConnectDB = new global::System.Windows.Forms.Label();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1Tools.SuspendLayout();
			this.contextMenuStrip2Help.SuspendLayout();
			this.toolStrip1BookMark.SuspendLayout();
			this.contextMenuStrip3Normal.SuspendLayout();
			this.flowLayoutPanel1ICon.SuspendLayout();
			this.grpGettingStarted.SuspendLayout();
			this.panel2Content.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.panel4Form).BeginInit();
			this.mnuMain.SuspendLayout();
			this.stbRunInfo.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1Tools, "contextMenuStrip1Tools");
			this.contextMenuStrip1Tools.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.cmdChangePasswor, this.cmdEditOperator, this.cmdOperatorManage, this.mnuDBBackup, this.mnuOption, this.toolStripSeparator1, this.mnuExtendedFunction, this.toolStripMenuItem23, this.mnuElevator, this.mnuMeetingSign,
				this.mnuMeal, this.mnuPatrol, this.mnuCameraManage, this.mnuPCCheckAccessConfigure, this.mnuTaskList, this.mnuDoorAsSwitch, this.mnuLogQuery, this.toolStripSeparator4, this.mnuInterfaceLock, this.toolStripSeparator2,
				this.mnuExit
			});
			this.contextMenuStrip1Tools.Name = "contextMenuStrip1";
			this.contextMenuStrip1Tools.OwnerItem = this.toolStripDropDownButton1;
			this.toolTip1.SetToolTip(this.contextMenuStrip1Tools, componentResourceManager.GetString("contextMenuStrip1Tools.ToolTip"));
			componentResourceManager.ApplyResources(this.cmdChangePasswor, "cmdChangePasswor");
			this.cmdChangePasswor.Name = "cmdChangePasswor";
			this.cmdChangePasswor.Click += new global::System.EventHandler(this.cmdChangePasswor_Click);
			componentResourceManager.ApplyResources(this.cmdEditOperator, "cmdEditOperator");
			this.cmdEditOperator.Name = "cmdEditOperator";
			this.cmdEditOperator.Click += new global::System.EventHandler(this.cmdEditOperator_Click);
			componentResourceManager.ApplyResources(this.cmdOperatorManage, "cmdOperatorManage");
			this.cmdOperatorManage.Name = "cmdOperatorManage";
			this.cmdOperatorManage.Click += new global::System.EventHandler(this.cmdOperatorManage_Click);
			componentResourceManager.ApplyResources(this.mnuDBBackup, "mnuDBBackup");
			this.mnuDBBackup.Name = "mnuDBBackup";
			this.mnuDBBackup.Click += new global::System.EventHandler(this.mnuDBBackup_Click);
			componentResourceManager.ApplyResources(this.mnuOption, "mnuOption");
			this.mnuOption.Name = "mnuOption";
			this.mnuOption.Click += new global::System.EventHandler(this.mnuOption_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.mnuExtendedFunction, "mnuExtendedFunction");
			this.mnuExtendedFunction.Name = "mnuExtendedFunction";
			this.mnuExtendedFunction.Click += new global::System.EventHandler(this.mnuExtendedFunction_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem23, "toolStripMenuItem23");
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			componentResourceManager.ApplyResources(this.mnuElevator, "mnuElevator");
			this.mnuElevator.Name = "mnuElevator";
			this.mnuElevator.Click += new global::System.EventHandler(this.mnuElevator_Click);
			componentResourceManager.ApplyResources(this.mnuMeetingSign, "mnuMeetingSign");
			this.mnuMeetingSign.Name = "mnuMeetingSign";
			this.mnuMeetingSign.Click += new global::System.EventHandler(this.mnuMeetingSign_Click);
			componentResourceManager.ApplyResources(this.mnuMeal, "mnuMeal");
			this.mnuMeal.Name = "mnuMeal";
			this.mnuMeal.Click += new global::System.EventHandler(this.mnuMeal_Click);
			componentResourceManager.ApplyResources(this.mnuPatrol, "mnuPatrol");
			this.mnuPatrol.Name = "mnuPatrol";
			this.mnuPatrol.Click += new global::System.EventHandler(this.mnuPatrol_Click);
			componentResourceManager.ApplyResources(this.mnuCameraManage, "mnuCameraManage");
			this.mnuCameraManage.Name = "mnuCameraManage";
			this.mnuCameraManage.Click += new global::System.EventHandler(this.mnuCameraManage_Click);
			componentResourceManager.ApplyResources(this.mnuPCCheckAccessConfigure, "mnuPCCheckAccessConfigure");
			this.mnuPCCheckAccessConfigure.Name = "mnuPCCheckAccessConfigure";
			this.mnuPCCheckAccessConfigure.Click += new global::System.EventHandler(this.mnuPCCheckAccessConfigure_Click);
			componentResourceManager.ApplyResources(this.mnuTaskList, "mnuTaskList");
			this.mnuTaskList.Name = "mnuTaskList";
			this.mnuTaskList.Click += new global::System.EventHandler(this.mnuTaskList_Click);
			componentResourceManager.ApplyResources(this.mnuDoorAsSwitch, "mnuDoorAsSwitch");
			this.mnuDoorAsSwitch.Name = "mnuDoorAsSwitch";
			this.mnuDoorAsSwitch.Click += new global::System.EventHandler(this.mnuDoorAsSwitch_Click);
			componentResourceManager.ApplyResources(this.mnuLogQuery, "mnuLogQuery");
			this.mnuLogQuery.Name = "mnuLogQuery";
			this.mnuLogQuery.Click += new global::System.EventHandler(this.mnuLogQuery_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			componentResourceManager.ApplyResources(this.mnuInterfaceLock, "mnuInterfaceLock");
			this.mnuInterfaceLock.Name = "mnuInterfaceLock";
			this.mnuInterfaceLock.Click += new global::System.EventHandler(this.mnuInterfaceLock_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			componentResourceManager.ApplyResources(this.mnuExit, "mnuExit");
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Click += new global::System.EventHandler(this.mnuExit_Click);
			componentResourceManager.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
			this.toolStripDropDownButton1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripDropDownButton1.DropDown = this.contextMenuStrip1Tools;
			this.toolStripDropDownButton1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripDropDownButton1.Image = global::WG3000_COMM.Properties.Resources.pMain_tool;
			this.toolStripDropDownButton1.Margin = new global::System.Windows.Forms.Padding(21, 2, 0, 0);
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Padding = new global::System.Windows.Forms.Padding(15, 0, 0, 0);
			componentResourceManager.ApplyResources(this.mnu1Tool, "mnu1Tool");
			this.mnu1Tool.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnuDeleteOldRecords, this.systemParamsToolStripMenuItem });
			this.mnu1Tool.Name = "mnu1Tool";
			componentResourceManager.ApplyResources(this.mnuDeleteOldRecords, "mnuDeleteOldRecords");
			this.mnuDeleteOldRecords.Name = "mnuDeleteOldRecords";
			this.mnuDeleteOldRecords.Click += new global::System.EventHandler(this.mnuDeleteOldRecords_Click);
			componentResourceManager.ApplyResources(this.systemParamsToolStripMenuItem, "systemParamsToolStripMenuItem");
			this.systemParamsToolStripMenuItem.Name = "systemParamsToolStripMenuItem";
			this.systemParamsToolStripMenuItem.Click += new global::System.EventHandler(this.systemParamsToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem20, "toolStripMenuItem20");
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			this.toolStripMenuItem20.Click += new global::System.EventHandler(this.toolStripMenuItem20_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem19, "toolStripMenuItem19");
			this.toolStripMenuItem19.Name = "toolStripMenuItem19";
			componentResourceManager.ApplyResources(this.mnuAbout, "mnuAbout");
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Click += new global::System.EventHandler(this.mnuAbout_Click);
			componentResourceManager.ApplyResources(this.mnuManual, "mnuManual");
			this.mnuManual.Name = "mnuManual";
			this.mnuManual.Click += new global::System.EventHandler(this.mnuManual_Click);
			componentResourceManager.ApplyResources(this.mnuSystemCharacteristic, "mnuSystemCharacteristic");
			this.mnuSystemCharacteristic.Name = "mnuSystemCharacteristic";
			this.mnuSystemCharacteristic.Click += new global::System.EventHandler(this.mnuSystemCharacteristic_Click);
			componentResourceManager.ApplyResources(this.contextMenuStrip2Help, "contextMenuStrip2Help");
			this.contextMenuStrip2Help.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnuAbout, this.mnuManual, this.mnuSystemCharacteristic, this.toolStripMenuItem20 });
			this.contextMenuStrip2Help.Name = "contextMenuStrip2Help";
			this.contextMenuStrip2Help.OwnerItem = this.mnu1Help;
			this.toolTip1.SetToolTip(this.contextMenuStrip2Help, componentResourceManager.GetString("contextMenuStrip2Help.ToolTip"));
			componentResourceManager.ApplyResources(this.mnu1Help, "mnu1Help");
			this.mnu1Help.BackColor = global::System.Drawing.Color.Transparent;
			this.mnu1Help.DropDown = this.contextMenuStrip2Help;
			this.mnu1Help.ForeColor = global::System.Drawing.Color.White;
			this.mnu1Help.Image = global::WG3000_COMM.Properties.Resources.pMain_help;
			this.mnu1Help.Name = "mnu1Help";
			componentResourceManager.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem1.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.toolStripMenuItem2, this.toolStripMenuItem3, this.toolStripMenuItem4, this.toolStripMenuItem5, this.toolStripMenuItem6, this.toolStripMenuItem7, this.toolStripSeparator5, this.toolStripMenuItem8, this.toolStripMenuItem9, this.toolStripMenuItem10,
				this.toolStripMenuItem11, this.toolStripMenuItem12, this.toolStripMenuItem13
			});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			componentResourceManager.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			componentResourceManager.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			componentResourceManager.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			componentResourceManager.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			componentResourceManager.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			componentResourceManager.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			componentResourceManager.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			componentResourceManager.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			componentResourceManager.ApplyResources(this.toolStripMenuItem9, "toolStripMenuItem9");
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			componentResourceManager.ApplyResources(this.toolStripMenuItem10, "toolStripMenuItem10");
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			componentResourceManager.ApplyResources(this.toolStripMenuItem11, "toolStripMenuItem11");
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			componentResourceManager.ApplyResources(this.toolStripMenuItem12, "toolStripMenuItem12");
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			componentResourceManager.ApplyResources(this.toolStripMenuItem13, "toolStripMenuItem13");
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			componentResourceManager.ApplyResources(this.toolStripMenuItem14, "toolStripMenuItem14");
			this.toolStripMenuItem14.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripMenuItem15, this.toolStripMenuItem16, this.toolStripMenuItem17, this.toolStripMenuItem18, this.toolsFormToolStripMenuItem });
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			componentResourceManager.ApplyResources(this.toolStripMenuItem15, "toolStripMenuItem15");
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			componentResourceManager.ApplyResources(this.toolStripMenuItem16, "toolStripMenuItem16");
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			componentResourceManager.ApplyResources(this.toolStripMenuItem17, "toolStripMenuItem17");
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			componentResourceManager.ApplyResources(this.toolStripMenuItem18, "toolStripMenuItem18");
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			componentResourceManager.ApplyResources(this.toolsFormToolStripMenuItem, "toolsFormToolStripMenuItem");
			this.toolsFormToolStripMenuItem.Name = "toolsFormToolStripMenuItem";
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.toolStrip1BookMark, "toolStrip1BookMark");
			this.toolStrip1BookMark.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1BookMark.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_bkg;
			this.toolStrip1BookMark.ContextMenuStrip = this.contextMenuStrip3Normal;
			this.toolStrip1BookMark.GripStyle = global::System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1BookMark.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.toolStripButtonBookmark1, this.toolStripButtonBookmark2, this.toolStripButton4, this.toolStripButton3, this.toolStripButton2, this.toolStripButton1, this.toolStripButton5, this.toolStripButton7, this.toolStripButton6, this.toolStripButton8,
				this.toolStripButtonBookmark3, this.toolStripButton9
			});
			this.toolStrip1BookMark.Name = "toolStrip1BookMark";
			this.toolStrip1BookMark.RenderMode = global::System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolTip1.SetToolTip(this.toolStrip1BookMark, componentResourceManager.GetString("toolStrip1BookMark.ToolTip"));
			this.toolStrip1BookMark.ItemClicked += new global::System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1BookMark_ItemClicked);
			componentResourceManager.ApplyResources(this.contextMenuStrip3Normal, "contextMenuStrip3Normal");
			this.contextMenuStrip3Normal.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.shortcutControllers, this.shortcutPersonnel, this.shortcutPrivilege, this.shortcutConsole, this.shortcutSwipe, this.shortcutAttendance, this.toolStripSeparator17, this.displayHideMenuToolStripMenuItem, this.displayHideStatusBarToolStripMenuItem, this.displayHideStarterToolStripMenuItem });
			this.contextMenuStrip3Normal.Name = "contextMenuStrip3Normal";
			this.toolTip1.SetToolTip(this.contextMenuStrip3Normal, componentResourceManager.GetString("contextMenuStrip3Normal.ToolTip"));
			componentResourceManager.ApplyResources(this.shortcutControllers, "shortcutControllers");
			this.shortcutControllers.Name = "shortcutControllers";
			this.shortcutControllers.Click += new global::System.EventHandler(this.shortcutControllers_Click);
			componentResourceManager.ApplyResources(this.shortcutPersonnel, "shortcutPersonnel");
			this.shortcutPersonnel.Name = "shortcutPersonnel";
			this.shortcutPersonnel.Click += new global::System.EventHandler(this.shortcutPersonnel_Click);
			componentResourceManager.ApplyResources(this.shortcutPrivilege, "shortcutPrivilege");
			this.shortcutPrivilege.Name = "shortcutPrivilege";
			this.shortcutPrivilege.Click += new global::System.EventHandler(this.shortcutPrivilege_Click);
			componentResourceManager.ApplyResources(this.shortcutConsole, "shortcutConsole");
			this.shortcutConsole.Name = "shortcutConsole";
			this.shortcutConsole.Click += new global::System.EventHandler(this.shortcutConsole_Click);
			componentResourceManager.ApplyResources(this.shortcutSwipe, "shortcutSwipe");
			this.shortcutSwipe.Name = "shortcutSwipe";
			this.shortcutSwipe.Click += new global::System.EventHandler(this.shortcutSwipe_Click);
			componentResourceManager.ApplyResources(this.shortcutAttendance, "shortcutAttendance");
			this.shortcutAttendance.Name = "shortcutAttendance";
			this.shortcutAttendance.Click += new global::System.EventHandler(this.shortcutAttendance_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator17, "toolStripSeparator17");
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			componentResourceManager.ApplyResources(this.displayHideMenuToolStripMenuItem, "displayHideMenuToolStripMenuItem");
			this.displayHideMenuToolStripMenuItem.Name = "displayHideMenuToolStripMenuItem";
			this.displayHideMenuToolStripMenuItem.Click += new global::System.EventHandler(this.displayHideMenuToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayHideStatusBarToolStripMenuItem, "displayHideStatusBarToolStripMenuItem");
			this.displayHideStatusBarToolStripMenuItem.Name = "displayHideStatusBarToolStripMenuItem";
			this.displayHideStatusBarToolStripMenuItem.Click += new global::System.EventHandler(this.displayHideStatusBarToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayHideStarterToolStripMenuItem, "displayHideStarterToolStripMenuItem");
			this.displayHideStarterToolStripMenuItem.Name = "displayHideStarterToolStripMenuItem";
			this.displayHideStarterToolStripMenuItem.Click += new global::System.EventHandler(this.displayHideStarterToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripButtonBookmark1, "toolStripButtonBookmark1");
			this.toolStripButtonBookmark1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButtonBookmark1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButtonBookmark1.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonBookmark1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButtonBookmark1.Margin = new global::System.Windows.Forms.Padding(10, 6, 0, 6);
			this.toolStripButtonBookmark1.Name = "toolStripButtonBookmark1";
			this.toolStripButtonBookmark1.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButtonBookmark1.Tag = "WG3000_COMM.Basic.frmControllers";
			this.toolStripButtonBookmark1.Click += new global::System.EventHandler(this.toolStripButtonBookmark1_Click);
			componentResourceManager.ApplyResources(this.toolStripButtonBookmark2, "toolStripButtonBookmark2");
			this.toolStripButtonBookmark2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButtonBookmark2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButtonBookmark2.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonBookmark2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButtonBookmark2.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButtonBookmark2.Name = "toolStripButtonBookmark2";
			this.toolStripButtonBookmark2.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButtonBookmark2.Tag = "WG3000_COMM.Basic.frmDepartments";
			componentResourceManager.ApplyResources(this.toolStripButton4, "toolStripButton4");
			this.toolStripButton4.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton4.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton4.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton4.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton4.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton4.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton3, "toolStripButton3");
			this.toolStripButton3.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton3.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton3.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton3.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton3.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton3.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton2, "toolStripButton2");
			this.toolStripButton2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton2.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton2.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton2.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton1, "toolStripButton1");
			this.toolStripButton1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton1.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton1.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton1.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton5, "toolStripButton5");
			this.toolStripButton5.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton5.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton5.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton5.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton5.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton5.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton7, "toolStripButton7");
			this.toolStripButton7.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton7.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton7.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton7.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton7.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton7.Name = "toolStripButton7";
			this.toolStripButton7.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton7.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton6, "toolStripButton6");
			this.toolStripButton6.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton6.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton6.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton6.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton6.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton6.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton8, "toolStripButton8");
			this.toolStripButton8.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton8.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton8.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton8.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton8.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton8.Name = "toolStripButton8";
			this.toolStripButton8.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton8.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButtonBookmark3, "toolStripButtonBookmark3");
			this.toolStripButtonBookmark3.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButtonBookmark3.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButtonBookmark3.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonBookmark3.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButtonBookmark3.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButtonBookmark3.Name = "toolStripButtonBookmark3";
			this.toolStripButtonBookmark3.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButtonBookmark3.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.toolStripButton9, "toolStripButton9");
			this.toolStripButton9.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripButton9.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_Bookmark_normal;
			this.toolStripButton9.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton9.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton9.Margin = new global::System.Windows.Forms.Padding(6, 6, 0, 6);
			this.toolStripButton9.Name = "toolStripButton9";
			this.toolStripButton9.Padding = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.toolStripButton9.Tag = "WG3000_COMM.Basic.frmUsers";
			componentResourceManager.ApplyResources(this.flowLayoutPanel1ICon, "flowLayoutPanel1ICon");
			this.flowLayoutPanel1ICon.BackColor = global::System.Drawing.Color.Transparent;
			this.flowLayoutPanel1ICon.Controls.Add(this.grpGettingStarted);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnIconBasicConfig);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnIconBasicOperate);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnIconAttendance);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnConstMeal);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnPatrol);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnMeeting);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnElevator);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnFingerPrint);
			this.flowLayoutPanel1ICon.Controls.Add(this.btnFaceManagement);
			this.flowLayoutPanel1ICon.Name = "flowLayoutPanel1ICon";
			this.toolTip1.SetToolTip(this.flowLayoutPanel1ICon, componentResourceManager.GetString("flowLayoutPanel1ICon.ToolTip"));
			componentResourceManager.ApplyResources(this.grpGettingStarted, "grpGettingStarted");
			this.grpGettingStarted.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.grpGettingStarted.Controls.Add(this.btnHideGettingStarted);
			this.grpGettingStarted.Controls.Add(this.label1);
			this.grpGettingStarted.Controls.Add(this.btnAddPrivilege);
			this.grpGettingStarted.Controls.Add(this.btnAutoAddCardBySwiping);
			this.grpGettingStarted.Controls.Add(this.btnAddController);
			this.grpGettingStarted.ForeColor = global::System.Drawing.Color.White;
			this.grpGettingStarted.Name = "grpGettingStarted";
			this.grpGettingStarted.TabStop = false;
			this.toolTip1.SetToolTip(this.grpGettingStarted, componentResourceManager.GetString("grpGettingStarted.ToolTip"));
			componentResourceManager.ApplyResources(this.btnHideGettingStarted, "btnHideGettingStarted");
			this.btnHideGettingStarted.BackColor = global::System.Drawing.Color.FromArgb(117, 121, 155);
			this.btnHideGettingStarted.Name = "btnHideGettingStarted";
			this.toolTip1.SetToolTip(this.btnHideGettingStarted, componentResourceManager.GetString("btnHideGettingStarted.ToolTip"));
			this.btnHideGettingStarted.UseVisualStyleBackColor = false;
			this.btnHideGettingStarted.Click += new global::System.EventHandler(this.btnHideGettingStarted_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnAddPrivilege, "btnAddPrivilege");
			this.btnAddPrivilege.BackColor = global::System.Drawing.Color.FromArgb(117, 121, 155);
			this.btnAddPrivilege.Name = "btnAddPrivilege";
			this.toolTip1.SetToolTip(this.btnAddPrivilege, componentResourceManager.GetString("btnAddPrivilege.ToolTip"));
			this.btnAddPrivilege.UseVisualStyleBackColor = false;
			this.btnAddPrivilege.Click += new global::System.EventHandler(this.btnAddPrivilege_Click);
			componentResourceManager.ApplyResources(this.btnAutoAddCardBySwiping, "btnAutoAddCardBySwiping");
			this.btnAutoAddCardBySwiping.BackColor = global::System.Drawing.Color.FromArgb(117, 121, 155);
			this.btnAutoAddCardBySwiping.Name = "btnAutoAddCardBySwiping";
			this.toolTip1.SetToolTip(this.btnAutoAddCardBySwiping, componentResourceManager.GetString("btnAutoAddCardBySwiping.ToolTip"));
			this.btnAutoAddCardBySwiping.UseVisualStyleBackColor = false;
			this.btnAutoAddCardBySwiping.Click += new global::System.EventHandler(this.btnAutoAddCardBySwiping_Click);
			componentResourceManager.ApplyResources(this.btnAddController, "btnAddController");
			this.btnAddController.BackColor = global::System.Drawing.Color.FromArgb(117, 121, 155);
			this.btnAddController.Name = "btnAddController";
			this.toolTip1.SetToolTip(this.btnAddController, componentResourceManager.GetString("btnAddController.ToolTip"));
			this.btnAddController.UseVisualStyleBackColor = false;
			this.btnAddController.Click += new global::System.EventHandler(this.btnAddController_Click);
			componentResourceManager.ApplyResources(this.btnIconBasicConfig, "btnIconBasicConfig");
			this.btnIconBasicConfig.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_icon_focus02;
			this.btnIconBasicConfig.ForeColor = global::System.Drawing.Color.White;
			this.btnIconBasicConfig.Image = global::WG3000_COMM.Properties.Resources.pMain_BasicConfigure;
			this.btnIconBasicConfig.Name = "btnIconBasicConfig";
			this.btnIconBasicConfig.Tag = "BasciConfig";
			this.toolTip1.SetToolTip(this.btnIconBasicConfig, componentResourceManager.GetString("btnIconBasicConfig.ToolTip"));
			this.btnIconBasicConfig.UseVisualStyleBackColor = false;
			this.btnIconBasicConfig.Click += new global::System.EventHandler(this.btnIconBasicConfig_Click);
			componentResourceManager.ApplyResources(this.btnIconBasicOperate, "btnIconBasicOperate");
			this.btnIconBasicOperate.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnIconBasicOperate.ForeColor = global::System.Drawing.Color.White;
			this.btnIconBasicOperate.Image = global::WG3000_COMM.Properties.Resources.pMain_BasicOperate;
			this.btnIconBasicOperate.Name = "btnIconBasicOperate";
			this.btnIconBasicOperate.Tag = "BasicOperate";
			this.toolTip1.SetToolTip(this.btnIconBasicOperate, componentResourceManager.GetString("btnIconBasicOperate.ToolTip"));
			this.btnIconBasicOperate.UseVisualStyleBackColor = false;
			this.btnIconBasicOperate.Click += new global::System.EventHandler(this.btnIconBasicConfig_Click);
			componentResourceManager.ApplyResources(this.btnIconAttendance, "btnIconAttendance");
			this.btnIconAttendance.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnIconAttendance.ForeColor = global::System.Drawing.Color.White;
			this.btnIconAttendance.Image = global::WG3000_COMM.Properties.Resources.pMain_Attendance;
			this.btnIconAttendance.Name = "btnIconAttendance";
			this.btnIconAttendance.Tag = "Attendance";
			this.toolTip1.SetToolTip(this.btnIconAttendance, componentResourceManager.GetString("btnIconAttendance.ToolTip"));
			this.btnIconAttendance.UseVisualStyleBackColor = false;
			this.btnIconAttendance.Click += new global::System.EventHandler(this.btnIconBasicConfig_Click);
			componentResourceManager.ApplyResources(this.btnConstMeal, "btnConstMeal");
			this.btnConstMeal.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnConstMeal.ForeColor = global::System.Drawing.Color.White;
			this.btnConstMeal.Name = "btnConstMeal";
			this.btnConstMeal.Tag = "ConstMeal";
			this.toolTip1.SetToolTip(this.btnConstMeal, componentResourceManager.GetString("btnConstMeal.ToolTip"));
			this.btnConstMeal.UseVisualStyleBackColor = false;
			this.btnConstMeal.Click += new global::System.EventHandler(this.mnu2ConstMeal_Click);
			componentResourceManager.ApplyResources(this.btnPatrol, "btnPatrol");
			this.btnPatrol.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnPatrol.ForeColor = global::System.Drawing.Color.White;
			this.btnPatrol.Name = "btnPatrol";
			this.btnPatrol.Tag = "Patrol";
			this.toolTip1.SetToolTip(this.btnPatrol, componentResourceManager.GetString("btnPatrol.ToolTip"));
			this.btnPatrol.UseVisualStyleBackColor = false;
			this.btnPatrol.Click += new global::System.EventHandler(this.mnu2Patrol_Click);
			componentResourceManager.ApplyResources(this.btnMeeting, "btnMeeting");
			this.btnMeeting.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnMeeting.ForeColor = global::System.Drawing.Color.White;
			this.btnMeeting.Name = "btnMeeting";
			this.btnMeeting.Tag = "MeetingSign";
			this.toolTip1.SetToolTip(this.btnMeeting, componentResourceManager.GetString("btnMeeting.ToolTip"));
			this.btnMeeting.UseVisualStyleBackColor = false;
			this.btnMeeting.Click += new global::System.EventHandler(this.mnu2Meeting_Click);
			componentResourceManager.ApplyResources(this.btnElevator, "btnElevator");
			this.btnElevator.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnElevator.ForeColor = global::System.Drawing.Color.White;
			this.btnElevator.Name = "btnElevator";
			this.btnElevator.Tag = "OneToMore";
			this.toolTip1.SetToolTip(this.btnElevator, componentResourceManager.GetString("btnElevator.ToolTip"));
			this.btnElevator.UseVisualStyleBackColor = false;
			this.btnElevator.Click += new global::System.EventHandler(this.mnu2OneToMore_Click);
			componentResourceManager.ApplyResources(this.btnFingerPrint, "btnFingerPrint");
			this.btnFingerPrint.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnFingerPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnFingerPrint.Name = "btnFingerPrint";
			this.btnFingerPrint.Tag = "指纹";
			this.toolTip1.SetToolTip(this.btnFingerPrint, componentResourceManager.GetString("btnFingerPrint.ToolTip"));
			this.btnFingerPrint.UseVisualStyleBackColor = false;
			this.btnFingerPrint.Click += new global::System.EventHandler(this.btnFingerPrint_Click);
			componentResourceManager.ApplyResources(this.btnFaceManagement, "btnFaceManagement");
			this.btnFaceManagement.BackColor = global::System.Drawing.Color.FromArgb(147, 150, 177);
			this.btnFaceManagement.ForeColor = global::System.Drawing.Color.White;
			this.btnFaceManagement.Name = "btnFaceManagement";
			this.btnFaceManagement.Tag = "FaceDevice";
			this.toolTip1.SetToolTip(this.btnFaceManagement, componentResourceManager.GetString("btnFaceManagement.ToolTip"));
			this.btnFaceManagement.UseVisualStyleBackColor = false;
			this.btnFaceManagement.Click += new global::System.EventHandler(this.btnFaceManagement_Click);
			componentResourceManager.ApplyResources(this.panel2Content, "panel2Content");
			this.panel2Content.BackColor = global::System.Drawing.Color.FromArgb(91, 92, 120);
			this.panel2Content.Controls.Add(this.panel4Form);
			this.panel2Content.Controls.Add(this.mnuMain);
			this.panel2Content.Controls.Add(this.stbRunInfo);
			this.panel2Content.Controls.Add(this.panel1);
			this.panel2Content.Name = "panel2Content";
			this.toolTip1.SetToolTip(this.panel2Content, componentResourceManager.GetString("panel2Content.ToolTip"));
			componentResourceManager.ApplyResources(this.panel4Form, "panel4Form");
			this.panel4Form.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.panel4Form.Name = "panel4Form";
			this.panel4Form.TabStop = false;
			this.toolTip1.SetToolTip(this.panel4Form, componentResourceManager.GetString("panel4Form.ToolTip"));
			componentResourceManager.ApplyResources(this.mnuMain, "mnuMain");
			this.mnuMain.BackColor = global::System.Drawing.Color.FromArgb(165, 191, 218);
			this.mnuMain.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnu1File, this.mnu1Configure, this.mnu1Operate, this.mnu1MultiFunc, this.mnu1Tools, this.mnu1HelpA, this.interfaceLockToolStripMenuItem });
			this.mnuMain.Name = "mnuMain";
			this.toolTip1.SetToolTip(this.mnuMain, componentResourceManager.GetString("mnuMain.ToolTip"));
			componentResourceManager.ApplyResources(this.mnu1File, "mnu1File");
			this.mnu1File.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.mnu1File.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnu2LogQuery, this.mnu2DBBackup, this.toolStripSeparator3, this.mnu2Exit });
			this.mnu1File.Name = "mnu1File";
			componentResourceManager.ApplyResources(this.mnu2LogQuery, "mnu2LogQuery");
			this.mnu2LogQuery.Name = "mnu2LogQuery";
			this.mnu2LogQuery.Click += new global::System.EventHandler(this.mnuLogQuery_Click);
			componentResourceManager.ApplyResources(this.mnu2DBBackup, "mnu2DBBackup");
			this.mnu2DBBackup.Name = "mnu2DBBackup";
			this.mnu2DBBackup.Click += new global::System.EventHandler(this.mnuDBBackup_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			componentResourceManager.ApplyResources(this.mnu2Exit, "mnu2Exit");
			this.mnu2Exit.Name = "mnu2Exit";
			this.mnu2Exit.Click += new global::System.EventHandler(this.mnuExit_Click);
			componentResourceManager.ApplyResources(this.mnu1Configure, "mnu1Configure");
			this.mnu1Configure.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.mnu1Configure.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.mnu2Controller, this.mnu2Zones, this.mnu2Departments, this.mnu2Personnel, this.mnu2CardLost, this.toolStripSeparator13, this.mnu2AccessPrivilege, this.mnu2privilegeTypesManagementToolStripMenuItem, this.mnu2TimeProfile, this.mnu2LimitedAccessTimes,
				this.mnu2AccessHolidayControl, this.toolStripSeparator14, this.mnu2PeripheralControl, this.mnu2PasswordManagement, this.mnu2AntiPassback, this.mnu2InterLock, this.mnu2MultiCard, this.mnu2FirstCardOpen, this.mnu2TaskList, this.mnu2DoorAsSwitch,
				this.mnu2GlobalAntipassback, this.toolStripSeparator9, this.mnu2Camera, this.mnu2CheckAccess
			});
			this.mnu1Configure.Name = "mnu1Configure";
			componentResourceManager.ApplyResources(this.mnu2Controller, "mnu2Controller");
			this.mnu2Controller.Name = "mnu2Controller";
			this.mnu2Controller.Click += new global::System.EventHandler(this.shortcutControllers_Click);
			componentResourceManager.ApplyResources(this.mnu2Zones, "mnu2Zones");
			this.mnu2Zones.Name = "mnu2Zones";
			this.mnu2Zones.Click += new global::System.EventHandler(this.mnu2Zones_Click);
			componentResourceManager.ApplyResources(this.mnu2Departments, "mnu2Departments");
			this.mnu2Departments.Name = "mnu2Departments";
			this.mnu2Departments.Click += new global::System.EventHandler(this.mnu2Departments_Click);
			componentResourceManager.ApplyResources(this.mnu2Personnel, "mnu2Personnel");
			this.mnu2Personnel.Name = "mnu2Personnel";
			this.mnu2Personnel.Click += new global::System.EventHandler(this.mnu2Personnel_Click);
			componentResourceManager.ApplyResources(this.mnu2CardLost, "mnu2CardLost");
			this.mnu2CardLost.Name = "mnu2CardLost";
			this.mnu2CardLost.Click += new global::System.EventHandler(this.mnu2CardLost_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			componentResourceManager.ApplyResources(this.mnu2AccessPrivilege, "mnu2AccessPrivilege");
			this.mnu2AccessPrivilege.Name = "mnu2AccessPrivilege";
			this.mnu2AccessPrivilege.Click += new global::System.EventHandler(this.mnu2AccessPrivilege_Click);
			componentResourceManager.ApplyResources(this.mnu2privilegeTypesManagementToolStripMenuItem, "mnu2privilegeTypesManagementToolStripMenuItem");
			this.mnu2privilegeTypesManagementToolStripMenuItem.Name = "mnu2privilegeTypesManagementToolStripMenuItem";
			this.mnu2privilegeTypesManagementToolStripMenuItem.Click += new global::System.EventHandler(this.mnu2privilegeTypesManagementToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.mnu2TimeProfile, "mnu2TimeProfile");
			this.mnu2TimeProfile.Name = "mnu2TimeProfile";
			this.mnu2TimeProfile.Click += new global::System.EventHandler(this.mnu2TimeProfile_Click);
			componentResourceManager.ApplyResources(this.mnu2LimitedAccessTimes, "mnu2LimitedAccessTimes");
			this.mnu2LimitedAccessTimes.Name = "mnu2LimitedAccessTimes";
			this.mnu2LimitedAccessTimes.Click += new global::System.EventHandler(this.mnu2LimitedAccessTimes_Click);
			componentResourceManager.ApplyResources(this.mnu2AccessHolidayControl, "mnu2AccessHolidayControl");
			this.mnu2AccessHolidayControl.Name = "mnu2AccessHolidayControl";
			this.mnu2AccessHolidayControl.Click += new global::System.EventHandler(this.mnu2AccessHolidayControl_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			componentResourceManager.ApplyResources(this.mnu2PeripheralControl, "mnu2PeripheralControl");
			this.mnu2PeripheralControl.Name = "mnu2PeripheralControl";
			this.mnu2PeripheralControl.Click += new global::System.EventHandler(this.mnu2PeripheralControl_Click);
			componentResourceManager.ApplyResources(this.mnu2PasswordManagement, "mnu2PasswordManagement");
			this.mnu2PasswordManagement.Name = "mnu2PasswordManagement";
			this.mnu2PasswordManagement.Click += new global::System.EventHandler(this.mnu2PasswordManagement_Click);
			componentResourceManager.ApplyResources(this.mnu2AntiPassback, "mnu2AntiPassback");
			this.mnu2AntiPassback.Name = "mnu2AntiPassback";
			this.mnu2AntiPassback.Click += new global::System.EventHandler(this.mnu2AntiPassback_Click);
			componentResourceManager.ApplyResources(this.mnu2InterLock, "mnu2InterLock");
			this.mnu2InterLock.Name = "mnu2InterLock";
			this.mnu2InterLock.Click += new global::System.EventHandler(this.mnu2InterLock_Click);
			componentResourceManager.ApplyResources(this.mnu2MultiCard, "mnu2MultiCard");
			this.mnu2MultiCard.Name = "mnu2MultiCard";
			this.mnu2MultiCard.Click += new global::System.EventHandler(this.mnu2MultiCard_Click);
			componentResourceManager.ApplyResources(this.mnu2FirstCardOpen, "mnu2FirstCardOpen");
			this.mnu2FirstCardOpen.Name = "mnu2FirstCardOpen";
			this.mnu2FirstCardOpen.Click += new global::System.EventHandler(this.mnu2FirstCardOpen_Click);
			componentResourceManager.ApplyResources(this.mnu2TaskList, "mnu2TaskList");
			this.mnu2TaskList.Name = "mnu2TaskList";
			this.mnu2TaskList.Click += new global::System.EventHandler(this.mnu2TaskList_Click);
			componentResourceManager.ApplyResources(this.mnu2DoorAsSwitch, "mnu2DoorAsSwitch");
			this.mnu2DoorAsSwitch.Name = "mnu2DoorAsSwitch";
			this.mnu2DoorAsSwitch.Click += new global::System.EventHandler(this.mnu2DoorAsSwitch_Click);
			componentResourceManager.ApplyResources(this.mnu2GlobalAntipassback, "mnu2GlobalAntipassback");
			this.mnu2GlobalAntipassback.Name = "mnu2GlobalAntipassback";
			this.mnu2GlobalAntipassback.Click += new global::System.EventHandler(this.mnu2GlobalAntipassback_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			componentResourceManager.ApplyResources(this.mnu2Camera, "mnu2Camera");
			this.mnu2Camera.Name = "mnu2Camera";
			this.mnu2Camera.Click += new global::System.EventHandler(this.mnu2Camera_Click);
			componentResourceManager.ApplyResources(this.mnu2CheckAccess, "mnu2CheckAccess");
			this.mnu2CheckAccess.Name = "mnu2CheckAccess";
			this.mnu2CheckAccess.Click += new global::System.EventHandler(this.mnu2CheckAccess_Click);
			componentResourceManager.ApplyResources(this.mnu1Operate, "mnu1Operate");
			this.mnu1Operate.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.mnu1Operate.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.mnu2Console, this.mnu2Monitor, this.mnu2Check, this.mnu2AdjustTime, this.mnu2Upload, this.mnu2GetRecords, this.mnu2RealtimeGetRecords, this.toolStripSeparator11, this.mnu2RemoteOpen, this.mnu2Locate,
				this.mnu2PersonInside, this.mnu2Maps, this.mnu2WarnOutputReset, this.mnu2ResetPersonInside, this.mnu2SetDoorControl, this.toolStripSeparator10, this.mnu2QuerySwipeRecords, this.toolStripSeparator12, this.mnu2InterfaceLock
			});
			this.mnu1Operate.Name = "mnu1Operate";
			componentResourceManager.ApplyResources(this.mnu2Console, "mnu2Console");
			this.mnu2Console.Name = "mnu2Console";
			this.mnu2Console.Click += new global::System.EventHandler(this.mnu2Console_Click);
			componentResourceManager.ApplyResources(this.mnu2Monitor, "mnu2Monitor");
			this.mnu2Monitor.Name = "mnu2Monitor";
			this.mnu2Monitor.Click += new global::System.EventHandler(this.mnu2Monitor_Click);
			componentResourceManager.ApplyResources(this.mnu2Check, "mnu2Check");
			this.mnu2Check.Name = "mnu2Check";
			this.mnu2Check.Click += new global::System.EventHandler(this.mnu2Check_Click);
			componentResourceManager.ApplyResources(this.mnu2AdjustTime, "mnu2AdjustTime");
			this.mnu2AdjustTime.Name = "mnu2AdjustTime";
			this.mnu2AdjustTime.Click += new global::System.EventHandler(this.mnu2AdjustTime_Click);
			componentResourceManager.ApplyResources(this.mnu2Upload, "mnu2Upload");
			this.mnu2Upload.Name = "mnu2Upload";
			this.mnu2Upload.Click += new global::System.EventHandler(this.mnu2Upload_Click);
			componentResourceManager.ApplyResources(this.mnu2GetRecords, "mnu2GetRecords");
			this.mnu2GetRecords.Name = "mnu2GetRecords";
			this.mnu2GetRecords.Click += new global::System.EventHandler(this.mnu2GetRecords_Click);
			componentResourceManager.ApplyResources(this.mnu2RealtimeGetRecords, "mnu2RealtimeGetRecords");
			this.mnu2RealtimeGetRecords.Name = "mnu2RealtimeGetRecords";
			this.mnu2RealtimeGetRecords.Click += new global::System.EventHandler(this.mnu2RealtimeGetRecords_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			componentResourceManager.ApplyResources(this.mnu2RemoteOpen, "mnu2RemoteOpen");
			this.mnu2RemoteOpen.Name = "mnu2RemoteOpen";
			this.mnu2RemoteOpen.Click += new global::System.EventHandler(this.mnu2RemoteOpen_Click);
			componentResourceManager.ApplyResources(this.mnu2Locate, "mnu2Locate");
			this.mnu2Locate.Name = "mnu2Locate";
			this.mnu2Locate.Click += new global::System.EventHandler(this.mnu2Locate_Click);
			componentResourceManager.ApplyResources(this.mnu2PersonInside, "mnu2PersonInside");
			this.mnu2PersonInside.Name = "mnu2PersonInside";
			this.mnu2PersonInside.Click += new global::System.EventHandler(this.mnu2PersonInside_Click);
			componentResourceManager.ApplyResources(this.mnu2Maps, "mnu2Maps");
			this.mnu2Maps.Name = "mnu2Maps";
			this.mnu2Maps.Click += new global::System.EventHandler(this.mnu2Maps_Click);
			componentResourceManager.ApplyResources(this.mnu2WarnOutputReset, "mnu2WarnOutputReset");
			this.mnu2WarnOutputReset.Name = "mnu2WarnOutputReset";
			this.mnu2WarnOutputReset.Click += new global::System.EventHandler(this.mnu2WarnOutputReset_Click);
			componentResourceManager.ApplyResources(this.mnu2ResetPersonInside, "mnu2ResetPersonInside");
			this.mnu2ResetPersonInside.Name = "mnu2ResetPersonInside";
			this.mnu2ResetPersonInside.Click += new global::System.EventHandler(this.mnu2ResetPersonInside_Click);
			componentResourceManager.ApplyResources(this.mnu2SetDoorControl, "mnu2SetDoorControl");
			this.mnu2SetDoorControl.Name = "mnu2SetDoorControl";
			this.mnu2SetDoorControl.Click += new global::System.EventHandler(this.mnu2SetDoorControl_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			componentResourceManager.ApplyResources(this.mnu2QuerySwipeRecords, "mnu2QuerySwipeRecords");
			this.mnu2QuerySwipeRecords.Name = "mnu2QuerySwipeRecords";
			this.mnu2QuerySwipeRecords.Click += new global::System.EventHandler(this.mnu2QuerySwipeRecords_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			componentResourceManager.ApplyResources(this.mnu2InterfaceLock, "mnu2InterfaceLock");
			this.mnu2InterfaceLock.Name = "mnu2InterfaceLock";
			this.mnu2InterfaceLock.Click += new global::System.EventHandler(this.mnuInterfaceLock_Click);
			componentResourceManager.ApplyResources(this.mnu1MultiFunc, "mnu1MultiFunc");
			this.mnu1MultiFunc.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnu2Attendence, this.mnu2ConstMeal, this.mnu2Patrol, this.mnu2Meeting, this.mnu2OneToMore });
			this.mnu1MultiFunc.Name = "mnu1MultiFunc";
			componentResourceManager.ApplyResources(this.mnu2Attendence, "mnu2Attendence");
			this.mnu2Attendence.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnuAttendenceData, this.toolStripSeparator7, this.mnuShiftNormalConfigure, this.mnuShiftRule, this.mnuShiftSet, this.mnuShiftArrange, this.toolStripSeparator6, this.mnuHolidaySet, this.mnuLeave, this.mnuManualCardRecord });
			this.mnu2Attendence.Name = "mnu2Attendence";
			componentResourceManager.ApplyResources(this.mnuAttendenceData, "mnuAttendenceData");
			this.mnuAttendenceData.Name = "mnuAttendenceData";
			this.mnuAttendenceData.Click += new global::System.EventHandler(this.mnuAttendenceData_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			componentResourceManager.ApplyResources(this.mnuShiftNormalConfigure, "mnuShiftNormalConfigure");
			this.mnuShiftNormalConfigure.Name = "mnuShiftNormalConfigure";
			this.mnuShiftNormalConfigure.Click += new global::System.EventHandler(this.mnuShiftNormalConfigure_Click);
			componentResourceManager.ApplyResources(this.mnuShiftRule, "mnuShiftRule");
			this.mnuShiftRule.Name = "mnuShiftRule";
			this.mnuShiftRule.Click += new global::System.EventHandler(this.mnuShiftRule_Click);
			componentResourceManager.ApplyResources(this.mnuShiftSet, "mnuShiftSet");
			this.mnuShiftSet.Name = "mnuShiftSet";
			this.mnuShiftSet.Click += new global::System.EventHandler(this.mnuShiftSet_Click);
			componentResourceManager.ApplyResources(this.mnuShiftArrange, "mnuShiftArrange");
			this.mnuShiftArrange.Name = "mnuShiftArrange";
			this.mnuShiftArrange.Click += new global::System.EventHandler(this.mnuShiftArrange_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			componentResourceManager.ApplyResources(this.mnuHolidaySet, "mnuHolidaySet");
			this.mnuHolidaySet.Name = "mnuHolidaySet";
			this.mnuHolidaySet.Click += new global::System.EventHandler(this.mnuHolidaySet_Click);
			componentResourceManager.ApplyResources(this.mnuLeave, "mnuLeave");
			this.mnuLeave.Name = "mnuLeave";
			this.mnuLeave.Click += new global::System.EventHandler(this.mnuLeave_Click);
			componentResourceManager.ApplyResources(this.mnuManualCardRecord, "mnuManualCardRecord");
			this.mnuManualCardRecord.Name = "mnuManualCardRecord";
			this.mnuManualCardRecord.Click += new global::System.EventHandler(this.mnuManualCardRecord_Click);
			componentResourceManager.ApplyResources(this.mnu2ConstMeal, "mnu2ConstMeal");
			this.mnu2ConstMeal.Name = "mnu2ConstMeal";
			this.mnu2ConstMeal.Click += new global::System.EventHandler(this.mnu2ConstMeal_Click);
			componentResourceManager.ApplyResources(this.mnu2Patrol, "mnu2Patrol");
			this.mnu2Patrol.Name = "mnu2Patrol";
			this.mnu2Patrol.Click += new global::System.EventHandler(this.mnu2Patrol_Click);
			componentResourceManager.ApplyResources(this.mnu2Meeting, "mnu2Meeting");
			this.mnu2Meeting.Name = "mnu2Meeting";
			this.mnu2Meeting.Click += new global::System.EventHandler(this.mnu2Meeting_Click);
			componentResourceManager.ApplyResources(this.mnu2OneToMore, "mnu2OneToMore");
			this.mnu2OneToMore.Name = "mnu2OneToMore";
			this.mnu2OneToMore.Click += new global::System.EventHandler(this.mnu2OneToMore_Click);
			componentResourceManager.ApplyResources(this.mnu1Tools, "mnu1Tools");
			this.mnu1Tools.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnu2OperatorManagement, this.mnu2EditOperator, this.mnu2ExtendedFunction, this.toolStripSeparator8, this.mnu2hideGettingStartedToolStripMenuItem, this.mnu2Language, this.mnu2InterfaceTitle, this.mnu2AutoLogin });
			this.mnu1Tools.Name = "mnu1Tools";
			componentResourceManager.ApplyResources(this.mnu2OperatorManagement, "mnu2OperatorManagement");
			this.mnu2OperatorManagement.Name = "mnu2OperatorManagement";
			this.mnu2OperatorManagement.Click += new global::System.EventHandler(this.mnu2OperatorManagement_Click);
			componentResourceManager.ApplyResources(this.mnu2EditOperator, "mnu2EditOperator");
			this.mnu2EditOperator.Name = "mnu2EditOperator";
			this.mnu2EditOperator.Click += new global::System.EventHandler(this.mnu2EditOperator_Click);
			componentResourceManager.ApplyResources(this.mnu2ExtendedFunction, "mnu2ExtendedFunction");
			this.mnu2ExtendedFunction.Name = "mnu2ExtendedFunction";
			this.mnu2ExtendedFunction.Click += new global::System.EventHandler(this.toolStripMenuItem29a_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			componentResourceManager.ApplyResources(this.mnu2hideGettingStartedToolStripMenuItem, "mnu2hideGettingStartedToolStripMenuItem");
			this.mnu2hideGettingStartedToolStripMenuItem.Name = "mnu2hideGettingStartedToolStripMenuItem";
			this.mnu2hideGettingStartedToolStripMenuItem.Click += new global::System.EventHandler(this.mnu2Option_Click);
			componentResourceManager.ApplyResources(this.mnu2Language, "mnu2Language");
			this.mnu2Language.Name = "mnu2Language";
			this.mnu2Language.Click += new global::System.EventHandler(this.mnu2Option_Click);
			componentResourceManager.ApplyResources(this.mnu2InterfaceTitle, "mnu2InterfaceTitle");
			this.mnu2InterfaceTitle.Name = "mnu2InterfaceTitle";
			this.mnu2InterfaceTitle.Click += new global::System.EventHandler(this.mnu2Option_Click);
			componentResourceManager.ApplyResources(this.mnu2AutoLogin, "mnu2AutoLogin");
			this.mnu2AutoLogin.Name = "mnu2AutoLogin";
			this.mnu2AutoLogin.Click += new global::System.EventHandler(this.mnu2Option_Click);
			componentResourceManager.ApplyResources(this.mnu1HelpA, "mnu1HelpA");
			this.mnu1HelpA.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnu2Beginner, this.mnu2Manual, this.toolStripSeparator15, this.mnu2SystemCharacteristic, this.produceUpdatesToolStripMenuItem, this.openContainingFolderToolStripMenuItem, this.feedbackToolStripMenuItem, this.toolStripSeparator16, this.mnu2About });
			this.mnu1HelpA.ForeColor = global::System.Drawing.Color.Black;
			this.mnu1HelpA.Name = "mnu1HelpA";
			componentResourceManager.ApplyResources(this.mnu2Beginner, "mnu2Beginner");
			this.mnu2Beginner.Name = "mnu2Beginner";
			this.mnu2Beginner.Click += new global::System.EventHandler(this.mnu2Beginner_Click);
			componentResourceManager.ApplyResources(this.mnu2Manual, "mnu2Manual");
			this.mnu2Manual.Name = "mnu2Manual";
			this.mnu2Manual.Click += new global::System.EventHandler(this.mnu2Manual_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			componentResourceManager.ApplyResources(this.mnu2SystemCharacteristic, "mnu2SystemCharacteristic");
			this.mnu2SystemCharacteristic.Name = "mnu2SystemCharacteristic";
			this.mnu2SystemCharacteristic.Click += new global::System.EventHandler(this.mnu2SystemCharacteristic_Click);
			componentResourceManager.ApplyResources(this.produceUpdatesToolStripMenuItem, "produceUpdatesToolStripMenuItem");
			this.produceUpdatesToolStripMenuItem.Name = "produceUpdatesToolStripMenuItem";
			this.produceUpdatesToolStripMenuItem.Click += new global::System.EventHandler(this.produceUpdatesToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.openContainingFolderToolStripMenuItem, "openContainingFolderToolStripMenuItem");
			this.openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
			this.openContainingFolderToolStripMenuItem.Click += new global::System.EventHandler(this.openContainingFolderToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.feedbackToolStripMenuItem, "feedbackToolStripMenuItem");
			this.feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
			this.feedbackToolStripMenuItem.Click += new global::System.EventHandler(this.feedbackToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator16, "toolStripSeparator16");
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			componentResourceManager.ApplyResources(this.mnu2About, "mnu2About");
			this.mnu2About.Name = "mnu2About";
			this.mnu2About.Click += new global::System.EventHandler(this.mnu2About_Click);
			componentResourceManager.ApplyResources(this.interfaceLockToolStripMenuItem, "interfaceLockToolStripMenuItem");
			this.interfaceLockToolStripMenuItem.Name = "interfaceLockToolStripMenuItem";
			this.interfaceLockToolStripMenuItem.Click += new global::System.EventHandler(this.mnuInterfaceLock_Click);
			componentResourceManager.ApplyResources(this.stbRunInfo, "stbRunInfo");
			this.stbRunInfo.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_bottom;
			this.stbRunInfo.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripDropDownButton1, this.mnu1Help, this.statOperator, this.statSoftwareVer, this.statCOM, this.statRuninfo1, this.statRuninfo2, this.statRuninfo3, this.statRuninfoLoadedNum, this.statTimeDate });
			this.stbRunInfo.Name = "stbRunInfo";
			this.toolTip1.SetToolTip(this.stbRunInfo, componentResourceManager.GetString("stbRunInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.statOperator, "statOperator");
			this.statOperator.BackColor = global::System.Drawing.Color.Transparent;
			this.statOperator.BorderSides = global::System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.statOperator.ForeColor = global::System.Drawing.Color.White;
			this.statOperator.Margin = new global::System.Windows.Forms.Padding(10, 3, 0, 2);
			this.statOperator.Name = "statOperator";
			componentResourceManager.ApplyResources(this.statSoftwareVer, "statSoftwareVer");
			this.statSoftwareVer.BackColor = global::System.Drawing.Color.Transparent;
			this.statSoftwareVer.ForeColor = global::System.Drawing.Color.White;
			this.statSoftwareVer.Name = "statSoftwareVer";
			componentResourceManager.ApplyResources(this.statCOM, "statCOM");
			this.statCOM.BackColor = global::System.Drawing.Color.Transparent;
			this.statCOM.ForeColor = global::System.Drawing.Color.White;
			this.statCOM.Name = "statCOM";
			componentResourceManager.ApplyResources(this.statRuninfo1, "statRuninfo1");
			this.statRuninfo1.BackColor = global::System.Drawing.Color.Transparent;
			this.statRuninfo1.ForeColor = global::System.Drawing.Color.White;
			this.statRuninfo1.Name = "statRuninfo1";
			this.statRuninfo1.Spring = true;
			componentResourceManager.ApplyResources(this.statRuninfo2, "statRuninfo2");
			this.statRuninfo2.BackColor = global::System.Drawing.Color.Transparent;
			this.statRuninfo2.ForeColor = global::System.Drawing.Color.White;
			this.statRuninfo2.Name = "statRuninfo2";
			componentResourceManager.ApplyResources(this.statRuninfo3, "statRuninfo3");
			this.statRuninfo3.BackColor = global::System.Drawing.Color.Transparent;
			this.statRuninfo3.ForeColor = global::System.Drawing.Color.White;
			this.statRuninfo3.Name = "statRuninfo3";
			componentResourceManager.ApplyResources(this.statRuninfoLoadedNum, "statRuninfoLoadedNum");
			this.statRuninfoLoadedNum.BackColor = global::System.Drawing.Color.Transparent;
			this.statRuninfoLoadedNum.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statRuninfoLoadedNum.ForeColor = global::System.Drawing.Color.White;
			this.statRuninfoLoadedNum.Name = "statRuninfoLoadedNum";
			componentResourceManager.ApplyResources(this.statTimeDate, "statTimeDate");
			this.statTimeDate.BackColor = global::System.Drawing.Color.Transparent;
			this.statTimeDate.ForeColor = global::System.Drawing.Color.White;
			this.statTimeDate.Image = global::WG3000_COMM.Properties.Resources.timequery;
			this.statTimeDate.Name = "statTimeDate";
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = global::System.Drawing.Color.Transparent;
			this.panel1.ContextMenuStrip = this.contextMenuStrip3Normal;
			this.panel1.Controls.Add(this.lblDiskSpace);
			this.panel1.Controls.Add(this.lblFailedToConnectDB);
			this.panel1.Name = "panel1";
			this.toolTip1.SetToolTip(this.panel1, componentResourceManager.GetString("panel1.ToolTip"));
			componentResourceManager.ApplyResources(this.lblDiskSpace, "lblDiskSpace");
			this.lblDiskSpace.BackColor = global::System.Drawing.Color.White;
			this.lblDiskSpace.ForeColor = global::System.Drawing.Color.Red;
			this.lblDiskSpace.Name = "lblDiskSpace";
			this.toolTip1.SetToolTip(this.lblDiskSpace, componentResourceManager.GetString("lblDiskSpace.ToolTip"));
			componentResourceManager.ApplyResources(this.lblFailedToConnectDB, "lblFailedToConnectDB");
			this.lblFailedToConnectDB.BackColor = global::System.Drawing.Color.White;
			this.lblFailedToConnectDB.ForeColor = global::System.Drawing.Color.Red;
			this.lblFailedToConnectDB.Name = "lblFailedToConnectDB";
			this.toolTip1.SetToolTip(this.lblFailedToConnectDB, componentResourceManager.GetString("lblFailedToConnectDB.ToolTip"));
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.FromArgb(91, 92, 120);
			base.Controls.Add(this.toolStrip1BookMark);
			base.Controls.Add(this.flowLayoutPanel1ICon);
			base.Controls.Add(this.panel2Content);
			base.Name = "frmADCT3000";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmADCT3000_FormClosing);
			base.Load += new global::System.EventHandler(this.frmADCT3000_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmADCT3000_KeyDown);
			this.contextMenuStrip1Tools.ResumeLayout(false);
			this.contextMenuStrip2Help.ResumeLayout(false);
			this.toolStrip1BookMark.ResumeLayout(false);
			this.toolStrip1BookMark.PerformLayout();
			this.contextMenuStrip3Normal.ResumeLayout(false);
			this.flowLayoutPanel1ICon.ResumeLayout(false);
			this.grpGettingStarted.ResumeLayout(false);
			this.grpGettingStarted.PerformLayout();
			this.panel2Content.ResumeLayout(false);
			this.panel2Content.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.panel4Form).EndInit();
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.stbRunInfo.ResumeLayout(false);
			this.stbRunInfo.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000821 RID: 2081
		public static global::WG3000_COMM.Core.WatchingService watchingP64;

		// Token: 0x04000822 RID: 2082
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000823 RID: 2083
		private global::System.Windows.Forms.Button btnAddController;

		// Token: 0x04000824 RID: 2084
		private global::System.Windows.Forms.Button btnAddPrivilege;

		// Token: 0x04000825 RID: 2085
		private global::System.Windows.Forms.Button btnAutoAddCardBySwiping;

		// Token: 0x04000826 RID: 2086
		private global::System.Windows.Forms.Button btnConstMeal;

		// Token: 0x04000827 RID: 2087
		private global::System.Windows.Forms.Button btnElevator;

		// Token: 0x04000828 RID: 2088
		private global::System.Windows.Forms.Button btnFaceManagement;

		// Token: 0x04000829 RID: 2089
		private global::System.Windows.Forms.Button btnFingerPrint;

		// Token: 0x0400082A RID: 2090
		private global::System.Windows.Forms.Button btnHideGettingStarted;

		// Token: 0x0400082B RID: 2091
		private global::System.Windows.Forms.Button btnIconAttendance;

		// Token: 0x0400082C RID: 2092
		private global::System.Windows.Forms.Button btnIconBasicConfig;

		// Token: 0x0400082D RID: 2093
		private global::System.Windows.Forms.Button btnIconBasicOperate;

		// Token: 0x0400082E RID: 2094
		private global::System.Windows.Forms.Button btnMeeting;

		// Token: 0x0400082F RID: 2095
		private global::System.Windows.Forms.Button btnPatrol;

		// Token: 0x04000830 RID: 2096
		private global::System.Windows.Forms.ToolStripMenuItem cmdChangePasswor;

		// Token: 0x04000831 RID: 2097
		private global::System.Windows.Forms.ToolStripMenuItem cmdEditOperator;

		// Token: 0x04000832 RID: 2098
		private global::System.Windows.Forms.ToolStripMenuItem cmdOperatorManage;

		// Token: 0x04000833 RID: 2099
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1Tools;

		// Token: 0x04000834 RID: 2100
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip2Help;

		// Token: 0x04000835 RID: 2101
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip3Normal;

		// Token: 0x04000836 RID: 2102
		private global::System.Windows.Forms.ToolStripMenuItem displayHideMenuToolStripMenuItem;

		// Token: 0x04000837 RID: 2103
		private global::System.Windows.Forms.ToolStripMenuItem displayHideStarterToolStripMenuItem;

		// Token: 0x04000838 RID: 2104
		private global::System.Windows.Forms.ToolStripMenuItem displayHideStatusBarToolStripMenuItem;

		// Token: 0x04000839 RID: 2105
		private global::System.Windows.Forms.ToolStripMenuItem feedbackToolStripMenuItem;

		// Token: 0x0400083A RID: 2106
		private global::System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1ICon;

		// Token: 0x0400083B RID: 2107
		private global::System.Windows.Forms.GroupBox grpGettingStarted;

		// Token: 0x0400083C RID: 2108
		private global::System.Windows.Forms.ToolStripMenuItem interfaceLockToolStripMenuItem;

		// Token: 0x0400083D RID: 2109
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400083E RID: 2110
		private global::System.Windows.Forms.Label lblDiskSpace;

		// Token: 0x0400083F RID: 2111
		private global::System.Windows.Forms.Label lblFailedToConnectDB;

		// Token: 0x04000840 RID: 2112
		private global::System.Windows.Forms.ToolStripMenuItem mnu1Configure;

		// Token: 0x04000841 RID: 2113
		private global::System.Windows.Forms.ToolStripMenuItem mnu1File;

		// Token: 0x04000842 RID: 2114
		private global::System.Windows.Forms.ToolStripDropDownButton mnu1Help;

		// Token: 0x04000843 RID: 2115
		private global::System.Windows.Forms.ToolStripMenuItem mnu1HelpA;

		// Token: 0x04000844 RID: 2116
		private global::System.Windows.Forms.ToolStripMenuItem mnu1MultiFunc;

		// Token: 0x04000845 RID: 2117
		private global::System.Windows.Forms.ToolStripMenuItem mnu1Operate;

		// Token: 0x04000846 RID: 2118
		private global::System.Windows.Forms.ToolStripMenuItem mnu1Tool;

		// Token: 0x04000847 RID: 2119
		private global::System.Windows.Forms.ToolStripMenuItem mnu1Tools;

		// Token: 0x04000848 RID: 2120
		private global::System.Windows.Forms.ToolStripMenuItem mnu2About;

		// Token: 0x04000849 RID: 2121
		private global::System.Windows.Forms.ToolStripMenuItem mnu2AccessHolidayControl;

		// Token: 0x0400084A RID: 2122
		private global::System.Windows.Forms.ToolStripMenuItem mnu2AccessPrivilege;

		// Token: 0x0400084B RID: 2123
		private global::System.Windows.Forms.ToolStripMenuItem mnu2AdjustTime;

		// Token: 0x0400084C RID: 2124
		private global::System.Windows.Forms.ToolStripMenuItem mnu2AntiPassback;

		// Token: 0x0400084D RID: 2125
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Attendence;

		// Token: 0x0400084E RID: 2126
		private global::System.Windows.Forms.ToolStripMenuItem mnu2AutoLogin;

		// Token: 0x0400084F RID: 2127
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Beginner;

		// Token: 0x04000850 RID: 2128
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Camera;

		// Token: 0x04000851 RID: 2129
		private global::System.Windows.Forms.ToolStripMenuItem mnu2CardLost;

		// Token: 0x04000852 RID: 2130
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Check;

		// Token: 0x04000853 RID: 2131
		private global::System.Windows.Forms.ToolStripMenuItem mnu2CheckAccess;

		// Token: 0x04000854 RID: 2132
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Console;

		// Token: 0x04000855 RID: 2133
		private global::System.Windows.Forms.ToolStripMenuItem mnu2ConstMeal;

		// Token: 0x04000856 RID: 2134
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Controller;

		// Token: 0x04000857 RID: 2135
		private global::System.Windows.Forms.ToolStripMenuItem mnu2DBBackup;

		// Token: 0x04000858 RID: 2136
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Departments;

		// Token: 0x04000859 RID: 2137
		private global::System.Windows.Forms.ToolStripMenuItem mnu2DoorAsSwitch;

		// Token: 0x0400085A RID: 2138
		private global::System.Windows.Forms.ToolStripMenuItem mnu2EditOperator;

		// Token: 0x0400085B RID: 2139
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Exit;

		// Token: 0x0400085C RID: 2140
		private global::System.Windows.Forms.ToolStripMenuItem mnu2ExtendedFunction;

		// Token: 0x0400085D RID: 2141
		private global::System.Windows.Forms.ToolStripMenuItem mnu2FirstCardOpen;

		// Token: 0x0400085E RID: 2142
		private global::System.Windows.Forms.ToolStripMenuItem mnu2GetRecords;

		// Token: 0x0400085F RID: 2143
		private global::System.Windows.Forms.ToolStripMenuItem mnu2GlobalAntipassback;

		// Token: 0x04000860 RID: 2144
		private global::System.Windows.Forms.ToolStripMenuItem mnu2hideGettingStartedToolStripMenuItem;

		// Token: 0x04000861 RID: 2145
		private global::System.Windows.Forms.ToolStripMenuItem mnu2InterfaceLock;

		// Token: 0x04000862 RID: 2146
		private global::System.Windows.Forms.ToolStripMenuItem mnu2InterfaceTitle;

		// Token: 0x04000863 RID: 2147
		private global::System.Windows.Forms.ToolStripMenuItem mnu2InterLock;

		// Token: 0x04000864 RID: 2148
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Language;

		// Token: 0x04000865 RID: 2149
		private global::System.Windows.Forms.ToolStripMenuItem mnu2LimitedAccessTimes;

		// Token: 0x04000866 RID: 2150
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Locate;

		// Token: 0x04000867 RID: 2151
		private global::System.Windows.Forms.ToolStripMenuItem mnu2LogQuery;

		// Token: 0x04000868 RID: 2152
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Manual;

		// Token: 0x04000869 RID: 2153
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Maps;

		// Token: 0x0400086A RID: 2154
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Meeting;

		// Token: 0x0400086B RID: 2155
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Monitor;

		// Token: 0x0400086C RID: 2156
		private global::System.Windows.Forms.ToolStripMenuItem mnu2MultiCard;

		// Token: 0x0400086D RID: 2157
		private global::System.Windows.Forms.ToolStripMenuItem mnu2OneToMore;

		// Token: 0x0400086E RID: 2158
		private global::System.Windows.Forms.ToolStripMenuItem mnu2OperatorManagement;

		// Token: 0x0400086F RID: 2159
		private global::System.Windows.Forms.ToolStripMenuItem mnu2PasswordManagement;

		// Token: 0x04000870 RID: 2160
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Patrol;

		// Token: 0x04000871 RID: 2161
		private global::System.Windows.Forms.ToolStripMenuItem mnu2PeripheralControl;

		// Token: 0x04000872 RID: 2162
		private global::System.Windows.Forms.ToolStripMenuItem mnu2PersonInside;

		// Token: 0x04000873 RID: 2163
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Personnel;

		// Token: 0x04000874 RID: 2164
		private global::System.Windows.Forms.ToolStripMenuItem mnu2privilegeTypesManagementToolStripMenuItem;

		// Token: 0x04000875 RID: 2165
		private global::System.Windows.Forms.ToolStripMenuItem mnu2QuerySwipeRecords;

		// Token: 0x04000876 RID: 2166
		private global::System.Windows.Forms.ToolStripMenuItem mnu2RealtimeGetRecords;

		// Token: 0x04000877 RID: 2167
		private global::System.Windows.Forms.ToolStripMenuItem mnu2RemoteOpen;

		// Token: 0x04000878 RID: 2168
		private global::System.Windows.Forms.ToolStripMenuItem mnu2ResetPersonInside;

		// Token: 0x04000879 RID: 2169
		private global::System.Windows.Forms.ToolStripMenuItem mnu2SetDoorControl;

		// Token: 0x0400087A RID: 2170
		private global::System.Windows.Forms.ToolStripMenuItem mnu2SystemCharacteristic;

		// Token: 0x0400087B RID: 2171
		private global::System.Windows.Forms.ToolStripMenuItem mnu2TaskList;

		// Token: 0x0400087C RID: 2172
		private global::System.Windows.Forms.ToolStripMenuItem mnu2TimeProfile;

		// Token: 0x0400087D RID: 2173
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Upload;

		// Token: 0x0400087E RID: 2174
		private global::System.Windows.Forms.ToolStripMenuItem mnu2WarnOutputReset;

		// Token: 0x0400087F RID: 2175
		private global::System.Windows.Forms.ToolStripMenuItem mnu2Zones;

		// Token: 0x04000880 RID: 2176
		private global::System.Windows.Forms.ToolStripMenuItem mnuAbout;

		// Token: 0x04000881 RID: 2177
		private global::System.Windows.Forms.ToolStripMenuItem mnuAttendenceData;

		// Token: 0x04000882 RID: 2178
		private global::System.Windows.Forms.ToolStripMenuItem mnuCameraManage;

		// Token: 0x04000883 RID: 2179
		private global::System.Windows.Forms.ToolStripMenuItem mnuDBBackup;

		// Token: 0x04000884 RID: 2180
		private global::System.Windows.Forms.ToolStripMenuItem mnuDeleteOldRecords;

		// Token: 0x04000885 RID: 2181
		private global::System.Windows.Forms.ToolStripMenuItem mnuDoorAsSwitch;

		// Token: 0x04000886 RID: 2182
		private global::System.Windows.Forms.ToolStripMenuItem mnuElevator;

		// Token: 0x04000887 RID: 2183
		private global::System.Windows.Forms.ToolStripMenuItem mnuExtendedFunction;

		// Token: 0x04000888 RID: 2184
		private global::System.Windows.Forms.ToolStripMenuItem mnuHolidaySet;

		// Token: 0x04000889 RID: 2185
		private global::System.Windows.Forms.ToolStripMenuItem mnuInterfaceLock;

		// Token: 0x0400088A RID: 2186
		private global::System.Windows.Forms.ToolStripMenuItem mnuLeave;

		// Token: 0x0400088B RID: 2187
		private global::System.Windows.Forms.ToolStripMenuItem mnuLogQuery;

		// Token: 0x0400088C RID: 2188
		private global::System.Windows.Forms.MenuStrip mnuMain;

		// Token: 0x0400088D RID: 2189
		private global::System.Windows.Forms.ToolStripMenuItem mnuManual;

		// Token: 0x0400088E RID: 2190
		private global::System.Windows.Forms.ToolStripMenuItem mnuManualCardRecord;

		// Token: 0x0400088F RID: 2191
		private global::System.Windows.Forms.ToolStripMenuItem mnuMeal;

		// Token: 0x04000890 RID: 2192
		private global::System.Windows.Forms.ToolStripMenuItem mnuMeetingSign;

		// Token: 0x04000891 RID: 2193
		private global::System.Windows.Forms.ToolStripMenuItem mnuOption;

		// Token: 0x04000892 RID: 2194
		private global::System.Windows.Forms.ToolStripMenuItem mnuPatrol;

		// Token: 0x04000893 RID: 2195
		private global::System.Windows.Forms.ToolStripMenuItem mnuPCCheckAccessConfigure;

		// Token: 0x04000894 RID: 2196
		private global::System.Windows.Forms.ToolStripMenuItem mnuShiftArrange;

		// Token: 0x04000895 RID: 2197
		private global::System.Windows.Forms.ToolStripMenuItem mnuShiftNormalConfigure;

		// Token: 0x04000896 RID: 2198
		private global::System.Windows.Forms.ToolStripMenuItem mnuShiftRule;

		// Token: 0x04000897 RID: 2199
		private global::System.Windows.Forms.ToolStripMenuItem mnuShiftSet;

		// Token: 0x04000898 RID: 2200
		private global::System.Windows.Forms.ToolStripMenuItem mnuSystemCharacteristic;

		// Token: 0x04000899 RID: 2201
		private global::System.Windows.Forms.ToolStripMenuItem mnuTaskList;

		// Token: 0x0400089A RID: 2202
		private global::System.Windows.Forms.ToolStripMenuItem openContainingFolderToolStripMenuItem;

		// Token: 0x0400089B RID: 2203
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400089C RID: 2204
		private global::System.Windows.Forms.Panel panel2Content;

		// Token: 0x0400089D RID: 2205
		private global::System.Windows.Forms.PictureBox panel4Form;

		// Token: 0x0400089E RID: 2206
		private global::System.Windows.Forms.ToolStripMenuItem produceUpdatesToolStripMenuItem;

		// Token: 0x0400089F RID: 2207
		private global::System.Windows.Forms.ToolStripMenuItem shortcutAttendance;

		// Token: 0x040008A0 RID: 2208
		private global::System.Windows.Forms.ToolStripMenuItem shortcutConsole;

		// Token: 0x040008A1 RID: 2209
		private global::System.Windows.Forms.ToolStripMenuItem shortcutControllers;

		// Token: 0x040008A2 RID: 2210
		private global::System.Windows.Forms.ToolStripMenuItem shortcutPersonnel;

		// Token: 0x040008A3 RID: 2211
		private global::System.Windows.Forms.ToolStripMenuItem shortcutPrivilege;

		// Token: 0x040008A4 RID: 2212
		private global::System.Windows.Forms.ToolStripMenuItem shortcutSwipe;

		// Token: 0x040008A5 RID: 2213
		private global::System.Windows.Forms.ToolStripStatusLabel statCOM;

		// Token: 0x040008A6 RID: 2214
		private global::System.Windows.Forms.ToolStripStatusLabel statOperator;

		// Token: 0x040008A7 RID: 2215
		private global::System.Windows.Forms.ToolStripStatusLabel statRuninfo1;

		// Token: 0x040008A8 RID: 2216
		private global::System.Windows.Forms.ToolStripStatusLabel statRuninfo2;

		// Token: 0x040008A9 RID: 2217
		private global::System.Windows.Forms.ToolStripStatusLabel statRuninfo3;

		// Token: 0x040008AA RID: 2218
		private global::System.Windows.Forms.ToolStripStatusLabel statRuninfoLoadedNum;

		// Token: 0x040008AB RID: 2219
		private global::System.Windows.Forms.ToolStripStatusLabel statSoftwareVer;

		// Token: 0x040008AC RID: 2220
		private global::System.Windows.Forms.ToolStripStatusLabel statTimeDate;

		// Token: 0x040008AD RID: 2221
		private global::System.Windows.Forms.StatusStrip stbRunInfo;

		// Token: 0x040008AE RID: 2222
		private global::System.Windows.Forms.ToolStripMenuItem systemParamsToolStripMenuItem;

		// Token: 0x040008AF RID: 2223
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040008B0 RID: 2224
		private global::System.Windows.Forms.ToolStripMenuItem toolsFormToolStripMenuItem;

		// Token: 0x040008B1 RID: 2225
		private global::System.Windows.Forms.ToolStrip toolStrip1BookMark;

		// Token: 0x040008B2 RID: 2226
		private global::System.Windows.Forms.ToolStripButton toolStripButton1;

		// Token: 0x040008B3 RID: 2227
		private global::System.Windows.Forms.ToolStripButton toolStripButton2;

		// Token: 0x040008B4 RID: 2228
		private global::System.Windows.Forms.ToolStripButton toolStripButton3;

		// Token: 0x040008B5 RID: 2229
		private global::System.Windows.Forms.ToolStripButton toolStripButton4;

		// Token: 0x040008B6 RID: 2230
		private global::System.Windows.Forms.ToolStripButton toolStripButton5;

		// Token: 0x040008B7 RID: 2231
		private global::System.Windows.Forms.ToolStripButton toolStripButton6;

		// Token: 0x040008B8 RID: 2232
		private global::System.Windows.Forms.ToolStripButton toolStripButton7;

		// Token: 0x040008B9 RID: 2233
		private global::System.Windows.Forms.ToolStripButton toolStripButton8;

		// Token: 0x040008BA RID: 2234
		private global::System.Windows.Forms.ToolStripButton toolStripButton9;

		// Token: 0x040008BB RID: 2235
		private global::System.Windows.Forms.ToolStripButton toolStripButtonBookmark1;

		// Token: 0x040008BC RID: 2236
		private global::System.Windows.Forms.ToolStripButton toolStripButtonBookmark2;

		// Token: 0x040008BD RID: 2237
		private global::System.Windows.Forms.ToolStripButton toolStripButtonBookmark3;

		// Token: 0x040008BE RID: 2238
		private global::System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;

		// Token: 0x040008BF RID: 2239
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

		// Token: 0x040008C0 RID: 2240
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;

		// Token: 0x040008C1 RID: 2241
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;

		// Token: 0x040008C2 RID: 2242
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;

		// Token: 0x040008C3 RID: 2243
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;

		// Token: 0x040008C4 RID: 2244
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;

		// Token: 0x040008C5 RID: 2245
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;

		// Token: 0x040008C6 RID: 2246
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;

		// Token: 0x040008C7 RID: 2247
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem17;

		// Token: 0x040008C8 RID: 2248
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem18;

		// Token: 0x040008C9 RID: 2249
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem19;

		// Token: 0x040008CA RID: 2250
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;

		// Token: 0x040008CB RID: 2251
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;

		// Token: 0x040008CC RID: 2252
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem23;

		// Token: 0x040008CD RID: 2253
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;

		// Token: 0x040008CE RID: 2254
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;

		// Token: 0x040008CF RID: 2255
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;

		// Token: 0x040008D0 RID: 2256
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;

		// Token: 0x040008D1 RID: 2257
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;

		// Token: 0x040008D2 RID: 2258
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;

		// Token: 0x040008D3 RID: 2259
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;

		// Token: 0x040008D4 RID: 2260
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x040008D5 RID: 2261
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator10;

		// Token: 0x040008D6 RID: 2262
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator11;

		// Token: 0x040008D7 RID: 2263
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator12;

		// Token: 0x040008D8 RID: 2264
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator13;

		// Token: 0x040008D9 RID: 2265
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator14;

		// Token: 0x040008DA RID: 2266
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator15;

		// Token: 0x040008DB RID: 2267
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator16;

		// Token: 0x040008DC RID: 2268
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator17;

		// Token: 0x040008DD RID: 2269
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x040008DE RID: 2270
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x040008DF RID: 2271
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

		// Token: 0x040008E0 RID: 2272
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator5;

		// Token: 0x040008E1 RID: 2273
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator6;

		// Token: 0x040008E2 RID: 2274
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator7;

		// Token: 0x040008E3 RID: 2275
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator8;

		// Token: 0x040008E4 RID: 2276
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator9;

		// Token: 0x040008E5 RID: 2277
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x040008E6 RID: 2278
		public global::System.Windows.Forms.ToolStripMenuItem mnuExit;
	}
}
