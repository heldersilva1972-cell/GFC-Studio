namespace WG3000_COMM.Basic
{
	// Token: 0x02000014 RID: 20
	public partial class dfrmExtendedFunctions : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00024733 File Offset: 0x00023733
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00024754 File Offset: 0x00023754
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmExtendedFunctions));
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPageFile = new global::System.Windows.Forms.TabPage();
			this.chkActiveLogQuery = new global::System.Windows.Forms.CheckBox();
			this.tabPageConfigure = new global::System.Windows.Forms.TabPage();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.chkActivateTimeSegLimittedAccess = new global::System.Windows.Forms.CheckBox();
			this.chkActivateTimeProfile = new global::System.Windows.Forms.CheckBox();
			this.chkRecordButtonEvent = new global::System.Windows.Forms.CheckBox();
			this.chkRecordDoorStatusEvent = new global::System.Windows.Forms.CheckBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.nudValidSwipeGap = new global::System.Windows.Forms.NumericUpDown();
			this.chkValidSwipeGap = new global::System.Windows.Forms.CheckBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.chkActivateWarnForceWithCard = new global::System.Windows.Forms.CheckBox();
			this.chkActivateOpenTooLongWarn = new global::System.Windows.Forms.CheckBox();
			this.chkActivateInvalidCardMoreTimesWarn = new global::System.Windows.Forms.CheckBox();
			this.chkSpeacialOption = new global::System.Windows.Forms.CheckBox();
			this.chkActivate19Card = new global::System.Windows.Forms.CheckBox();
			this.chkActivateTimeSecond = new global::System.Windows.Forms.CheckBox();
			this.chkActivateGlobalAntiBack = new global::System.Windows.Forms.CheckBox();
			this.chkActivityPrivilegeTypeManagementMode = new global::System.Windows.Forms.CheckBox();
			this.chkActivatePeripheralControl = new global::System.Windows.Forms.CheckBox();
			this.chkActivateCamera = new global::System.Windows.Forms.CheckBox();
			this.chkActivateControllerTaskList = new global::System.Windows.Forms.CheckBox();
			this.chkActivateFirstCardOpen = new global::System.Windows.Forms.CheckBox();
			this.chkActivateAccessKeypad = new global::System.Windows.Forms.CheckBox();
			this.chkActivateMultiCardAccess = new global::System.Windows.Forms.CheckBox();
			this.chkActivateAntiPassBack = new global::System.Windows.Forms.CheckBox();
			this.chkActivatePCCheckAccess = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDoorAsSwitch = new global::System.Windows.Forms.CheckBox();
			this.chkActivateInterLock = new global::System.Windows.Forms.CheckBox();
			this.tabPageOperate = new global::System.Windows.Forms.TabPage();
			this.chkActivatePersonInside = new global::System.Windows.Forms.CheckBox();
			this.chkActivateLocate = new global::System.Windows.Forms.CheckBox();
			this.chkActivateMaps = new global::System.Windows.Forms.CheckBox();
			this.chkActivateRemoteOpenDoor = new global::System.Windows.Forms.CheckBox();
			this.tabPageOneCardMultiFunc = new global::System.Windows.Forms.TabPage();
			this.chkActivateDontDisplayOneCardMultifunction = new global::System.Windows.Forms.CheckBox();
			this.chkImportExtern = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDisplayCertAndTel = new global::System.Windows.Forms.CheckBox();
			this.chkActivateFingerprintManagement = new global::System.Windows.Forms.CheckBox();
			this.nudf_LimitedTimesOfDay = new global::System.Windows.Forms.NumericUpDown();
			this.lblOneDayLimit = new global::System.Windows.Forms.Label();
			this.chkActivatePCCheckMealOpen = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDontDisplayAccessControl = new global::System.Windows.Forms.CheckBox();
			this.btnSetup = new global::System.Windows.Forms.Button();
			this.chkActivateDontDisplayAttendance = new global::System.Windows.Forms.CheckBox();
			this.chkActivatePatrol = new global::System.Windows.Forms.CheckBox();
			this.chkActivateOtherShiftSchedule = new global::System.Windows.Forms.CheckBox();
			this.chkActivateMeal = new global::System.Windows.Forms.CheckBox();
			this.chkActivateMeeting = new global::System.Windows.Forms.CheckBox();
			this.chkActivateElevator = new global::System.Windows.Forms.CheckBox();
			this.tabPageTools = new global::System.Windows.Forms.TabPage();
			this.chkActivateSecondLevelSecurity = new global::System.Windows.Forms.CheckBox();
			this.chkActivateOperatorManagement = new global::System.Windows.Forms.CheckBox();
			this.tabPageOther = new global::System.Windows.Forms.TabPage();
			this.btnFaceDeviceType = new global::System.Windows.Forms.Button();
			this.chkActivateCreateQRCode = new global::System.Windows.Forms.CheckBox();
			this.chkSFZIDAsCardNO = new global::System.Windows.Forms.CheckBox();
			this.chkPhotoNameFromConsumerNO = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDisplayCertID = new global::System.Windows.Forms.CheckBox();
			this.chkActivateFaceManagement = new global::System.Windows.Forms.CheckBox();
			this.chkGetSwipeRecordsContinuously = new global::System.Windows.Forms.CheckBox();
			this.btnSetupNormalOpen = new global::System.Windows.Forms.Button();
			this.chkActivateSFZReader = new global::System.Windows.Forms.CheckBox();
			this.chkActivateAccelerator = new global::System.Windows.Forms.CheckBox();
			this.chkDisableReaderLEDBEEPEROutput = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDontAutoLoadSwipeRecords = new global::System.Windows.Forms.CheckBox();
			this.chkActivateSwipeFourTimeSetNormalOpen = new global::System.Windows.Forms.CheckBox();
			this.chkActivateMobileAsCardInput = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDisplayYellowWhenDoorOpen = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDontDisplayRebootRecords = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDontDisplayDoorStatusRecords = new global::System.Windows.Forms.CheckBox();
			this.chkActivateDontAutoLoadPrivileges = new global::System.Windows.Forms.CheckBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPageFile.SuspendLayout();
			this.tabPageConfigure.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudValidSwipeGap).BeginInit();
			this.groupBox3.SuspendLayout();
			this.tabPageOperate.SuspendLayout();
			this.tabPageOneCardMultiFunc.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfDay).BeginInit();
			this.tabPageTools.SuspendLayout();
			this.tabPageOther.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPageFile);
			this.tabControl1.Controls.Add(this.tabPageConfigure);
			this.tabControl1.Controls.Add(this.tabPageOperate);
			this.tabControl1.Controls.Add(this.tabPageOneCardMultiFunc);
			this.tabControl1.Controls.Add(this.tabPageTools);
			this.tabControl1.Controls.Add(this.tabPageOther);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPageFile, "tabPageFile");
			this.tabPageFile.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPageFile.Controls.Add(this.chkActiveLogQuery);
			this.tabPageFile.ForeColor = global::System.Drawing.Color.White;
			this.tabPageFile.Name = "tabPageFile";
			componentResourceManager.ApplyResources(this.chkActiveLogQuery, "chkActiveLogQuery");
			this.chkActiveLogQuery.Name = "chkActiveLogQuery";
			this.chkActiveLogQuery.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPageConfigure, "tabPageConfigure");
			this.tabPageConfigure.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPageConfigure.Controls.Add(this.groupBox2);
			this.tabPageConfigure.Controls.Add(this.groupBox1);
			this.tabPageConfigure.ForeColor = global::System.Drawing.Color.White;
			this.tabPageConfigure.Name = "tabPageConfigure";
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.chkActivateTimeSegLimittedAccess);
			this.groupBox2.Controls.Add(this.chkActivateTimeProfile);
			this.groupBox2.Controls.Add(this.chkRecordButtonEvent);
			this.groupBox2.Controls.Add(this.chkRecordDoorStatusEvent);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.chkActivateTimeSegLimittedAccess, "chkActivateTimeSegLimittedAccess");
			this.chkActivateTimeSegLimittedAccess.ForeColor = global::System.Drawing.Color.White;
			this.chkActivateTimeSegLimittedAccess.Name = "chkActivateTimeSegLimittedAccess";
			this.chkActivateTimeSegLimittedAccess.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkActivateTimeProfile, "chkActivateTimeProfile");
			this.chkActivateTimeProfile.Name = "chkActivateTimeProfile";
			this.chkActivateTimeProfile.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkRecordButtonEvent, "chkRecordButtonEvent");
			this.chkRecordButtonEvent.ForeColor = global::System.Drawing.Color.White;
			this.chkRecordButtonEvent.Name = "chkRecordButtonEvent";
			this.chkRecordButtonEvent.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkRecordDoorStatusEvent, "chkRecordDoorStatusEvent");
			this.chkRecordDoorStatusEvent.ForeColor = global::System.Drawing.Color.White;
			this.chkRecordDoorStatusEvent.Name = "chkRecordDoorStatusEvent";
			this.chkRecordDoorStatusEvent.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.nudValidSwipeGap);
			this.groupBox1.Controls.Add(this.chkValidSwipeGap);
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.chkSpeacialOption);
			this.groupBox1.Controls.Add(this.chkActivate19Card);
			this.groupBox1.Controls.Add(this.chkActivateTimeSecond);
			this.groupBox1.Controls.Add(this.chkActivateGlobalAntiBack);
			this.groupBox1.Controls.Add(this.chkActivityPrivilegeTypeManagementMode);
			this.groupBox1.Controls.Add(this.chkActivatePeripheralControl);
			this.groupBox1.Controls.Add(this.chkActivateCamera);
			this.groupBox1.Controls.Add(this.chkActivateControllerTaskList);
			this.groupBox1.Controls.Add(this.chkActivateFirstCardOpen);
			this.groupBox1.Controls.Add(this.chkActivateAccessKeypad);
			this.groupBox1.Controls.Add(this.chkActivateMultiCardAccess);
			this.groupBox1.Controls.Add(this.chkActivateAntiPassBack);
			this.groupBox1.Controls.Add(this.chkActivatePCCheckAccess);
			this.groupBox1.Controls.Add(this.chkActivateDoorAsSwitch);
			this.groupBox1.Controls.Add(this.chkActivateInterLock);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.nudValidSwipeGap, "nudValidSwipeGap");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudValidSwipeGap;
			int[] array = new int[4];
			array[0] = 2;
			numericUpDown.Increment = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudValidSwipeGap;
			int[] array2 = new int[4];
			array2[0] = 86400;
			numericUpDown2.Maximum = new decimal(array2);
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudValidSwipeGap;
			int[] array3 = new int[4];
			array3[0] = 6;
			numericUpDown3.Minimum = new decimal(array3);
			this.nudValidSwipeGap.Name = "nudValidSwipeGap";
			this.nudValidSwipeGap.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudValidSwipeGap;
			int[] array4 = new int[4];
			array4[0] = 30;
			numericUpDown4.Value = new decimal(array4);
			componentResourceManager.ApplyResources(this.chkValidSwipeGap, "chkValidSwipeGap");
			this.chkValidSwipeGap.BackColor = global::System.Drawing.Color.Transparent;
			this.chkValidSwipeGap.ForeColor = global::System.Drawing.Color.White;
			this.chkValidSwipeGap.Name = "chkValidSwipeGap";
			this.chkValidSwipeGap.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.chkActivateWarnForceWithCard);
			this.groupBox3.Controls.Add(this.chkActivateOpenTooLongWarn);
			this.groupBox3.Controls.Add(this.chkActivateInvalidCardMoreTimesWarn);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.chkActivateWarnForceWithCard, "chkActivateWarnForceWithCard");
			this.chkActivateWarnForceWithCard.Name = "chkActivateWarnForceWithCard";
			this.chkActivateWarnForceWithCard.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateOpenTooLongWarn, "chkActivateOpenTooLongWarn");
			this.chkActivateOpenTooLongWarn.Name = "chkActivateOpenTooLongWarn";
			this.chkActivateOpenTooLongWarn.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateInvalidCardMoreTimesWarn, "chkActivateInvalidCardMoreTimesWarn");
			this.chkActivateInvalidCardMoreTimesWarn.Name = "chkActivateInvalidCardMoreTimesWarn";
			this.chkActivateInvalidCardMoreTimesWarn.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpeacialOption, "chkSpeacialOption");
			this.chkSpeacialOption.Name = "chkSpeacialOption";
			this.chkSpeacialOption.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkActivate19Card, "chkActivate19Card");
			this.chkActivate19Card.Name = "chkActivate19Card";
			this.chkActivate19Card.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateTimeSecond, "chkActivateTimeSecond");
			this.chkActivateTimeSecond.Name = "chkActivateTimeSecond";
			this.chkActivateTimeSecond.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateGlobalAntiBack, "chkActivateGlobalAntiBack");
			this.chkActivateGlobalAntiBack.Name = "chkActivateGlobalAntiBack";
			this.chkActivateGlobalAntiBack.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivityPrivilegeTypeManagementMode, "chkActivityPrivilegeTypeManagementMode");
			this.chkActivityPrivilegeTypeManagementMode.Name = "chkActivityPrivilegeTypeManagementMode";
			this.chkActivityPrivilegeTypeManagementMode.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivatePeripheralControl, "chkActivatePeripheralControl");
			this.chkActivatePeripheralControl.Name = "chkActivatePeripheralControl";
			this.chkActivatePeripheralControl.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateCamera, "chkActivateCamera");
			this.chkActivateCamera.Name = "chkActivateCamera";
			this.chkActivateCamera.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateControllerTaskList, "chkActivateControllerTaskList");
			this.chkActivateControllerTaskList.Name = "chkActivateControllerTaskList";
			this.chkActivateControllerTaskList.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateFirstCardOpen, "chkActivateFirstCardOpen");
			this.chkActivateFirstCardOpen.Name = "chkActivateFirstCardOpen";
			this.chkActivateFirstCardOpen.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateAccessKeypad, "chkActivateAccessKeypad");
			this.chkActivateAccessKeypad.Name = "chkActivateAccessKeypad";
			this.chkActivateAccessKeypad.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateMultiCardAccess, "chkActivateMultiCardAccess");
			this.chkActivateMultiCardAccess.Name = "chkActivateMultiCardAccess";
			this.chkActivateMultiCardAccess.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateAntiPassBack, "chkActivateAntiPassBack");
			this.chkActivateAntiPassBack.Name = "chkActivateAntiPassBack";
			this.chkActivateAntiPassBack.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivatePCCheckAccess, "chkActivatePCCheckAccess");
			this.chkActivatePCCheckAccess.Name = "chkActivatePCCheckAccess";
			this.chkActivatePCCheckAccess.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateDoorAsSwitch, "chkActivateDoorAsSwitch");
			this.chkActivateDoorAsSwitch.Name = "chkActivateDoorAsSwitch";
			this.chkActivateDoorAsSwitch.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkActivateInterLock, "chkActivateInterLock");
			this.chkActivateInterLock.Name = "chkActivateInterLock";
			this.chkActivateInterLock.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPageOperate, "tabPageOperate");
			this.tabPageOperate.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPageOperate.Controls.Add(this.chkActivatePersonInside);
			this.tabPageOperate.Controls.Add(this.chkActivateLocate);
			this.tabPageOperate.Controls.Add(this.chkActivateMaps);
			this.tabPageOperate.Controls.Add(this.chkActivateRemoteOpenDoor);
			this.tabPageOperate.ForeColor = global::System.Drawing.Color.White;
			this.tabPageOperate.Name = "tabPageOperate";
			componentResourceManager.ApplyResources(this.chkActivatePersonInside, "chkActivatePersonInside");
			this.chkActivatePersonInside.Name = "chkActivatePersonInside";
			this.chkActivatePersonInside.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateLocate, "chkActivateLocate");
			this.chkActivateLocate.Name = "chkActivateLocate";
			this.chkActivateLocate.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateMaps, "chkActivateMaps");
			this.chkActivateMaps.Name = "chkActivateMaps";
			this.chkActivateMaps.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateRemoteOpenDoor, "chkActivateRemoteOpenDoor");
			this.chkActivateRemoteOpenDoor.Name = "chkActivateRemoteOpenDoor";
			this.chkActivateRemoteOpenDoor.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPageOneCardMultiFunc, "tabPageOneCardMultiFunc");
			this.tabPageOneCardMultiFunc.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateDontDisplayOneCardMultifunction);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkImportExtern);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateDisplayCertAndTel);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateFingerprintManagement);
			this.tabPageOneCardMultiFunc.Controls.Add(this.nudf_LimitedTimesOfDay);
			this.tabPageOneCardMultiFunc.Controls.Add(this.lblOneDayLimit);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivatePCCheckMealOpen);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateDontDisplayAccessControl);
			this.tabPageOneCardMultiFunc.Controls.Add(this.btnSetup);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateDontDisplayAttendance);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivatePatrol);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateOtherShiftSchedule);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateMeal);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateMeeting);
			this.tabPageOneCardMultiFunc.Controls.Add(this.chkActivateElevator);
			this.tabPageOneCardMultiFunc.ForeColor = global::System.Drawing.Color.White;
			this.tabPageOneCardMultiFunc.Name = "tabPageOneCardMultiFunc";
			componentResourceManager.ApplyResources(this.chkActivateDontDisplayOneCardMultifunction, "chkActivateDontDisplayOneCardMultifunction");
			this.chkActivateDontDisplayOneCardMultifunction.Name = "chkActivateDontDisplayOneCardMultifunction";
			this.chkActivateDontDisplayOneCardMultifunction.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkImportExtern, "chkImportExtern");
			this.chkImportExtern.Name = "chkImportExtern";
			this.chkImportExtern.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateDisplayCertAndTel, "chkActivateDisplayCertAndTel");
			this.chkActivateDisplayCertAndTel.Name = "chkActivateDisplayCertAndTel";
			this.chkActivateDisplayCertAndTel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateFingerprintManagement, "chkActivateFingerprintManagement");
			this.chkActivateFingerprintManagement.Name = "chkActivateFingerprintManagement";
			this.chkActivateFingerprintManagement.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.nudf_LimitedTimesOfDay, "nudf_LimitedTimesOfDay");
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudf_LimitedTimesOfDay;
			int[] array5 = new int[4];
			array5[0] = 4;
			numericUpDown5.Maximum = new decimal(array5);
			global::System.Windows.Forms.NumericUpDown numericUpDown6 = this.nudf_LimitedTimesOfDay;
			int[] array6 = new int[4];
			array6[0] = 1;
			numericUpDown6.Minimum = new decimal(array6);
			this.nudf_LimitedTimesOfDay.Name = "nudf_LimitedTimesOfDay";
			this.nudf_LimitedTimesOfDay.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown7 = this.nudf_LimitedTimesOfDay;
			int[] array7 = new int[4];
			array7[0] = 3;
			numericUpDown7.Value = new decimal(array7);
			componentResourceManager.ApplyResources(this.lblOneDayLimit, "lblOneDayLimit");
			this.lblOneDayLimit.BackColor = global::System.Drawing.Color.Red;
			this.lblOneDayLimit.Name = "lblOneDayLimit";
			componentResourceManager.ApplyResources(this.chkActivatePCCheckMealOpen, "chkActivatePCCheckMealOpen");
			this.chkActivatePCCheckMealOpen.BackColor = global::System.Drawing.Color.Red;
			this.chkActivatePCCheckMealOpen.Name = "chkActivatePCCheckMealOpen";
			this.chkActivatePCCheckMealOpen.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkActivateDontDisplayAccessControl, "chkActivateDontDisplayAccessControl");
			this.chkActivateDontDisplayAccessControl.Name = "chkActivateDontDisplayAccessControl";
			this.chkActivateDontDisplayAccessControl.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnSetup, "btnSetup");
			this.btnSetup.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSetup.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSetup.ForeColor = global::System.Drawing.Color.White;
			this.btnSetup.Name = "btnSetup";
			this.btnSetup.UseVisualStyleBackColor = false;
			this.btnSetup.Click += new global::System.EventHandler(this.btnSetup_Click);
			componentResourceManager.ApplyResources(this.chkActivateDontDisplayAttendance, "chkActivateDontDisplayAttendance");
			this.chkActivateDontDisplayAttendance.Name = "chkActivateDontDisplayAttendance";
			this.chkActivateDontDisplayAttendance.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivatePatrol, "chkActivatePatrol");
			this.chkActivatePatrol.Name = "chkActivatePatrol";
			this.chkActivatePatrol.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateOtherShiftSchedule, "chkActivateOtherShiftSchedule");
			this.chkActivateOtherShiftSchedule.Name = "chkActivateOtherShiftSchedule";
			this.chkActivateOtherShiftSchedule.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateMeal, "chkActivateMeal");
			this.chkActivateMeal.Name = "chkActivateMeal";
			this.chkActivateMeal.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateMeeting, "chkActivateMeeting");
			this.chkActivateMeeting.Name = "chkActivateMeeting";
			this.chkActivateMeeting.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateElevator, "chkActivateElevator");
			this.chkActivateElevator.Name = "chkActivateElevator";
			this.chkActivateElevator.UseVisualStyleBackColor = true;
			this.chkActivateElevator.CheckedChanged += new global::System.EventHandler(this.chkActivateElevator_CheckedChanged);
			componentResourceManager.ApplyResources(this.tabPageTools, "tabPageTools");
			this.tabPageTools.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPageTools.Controls.Add(this.chkActivateSecondLevelSecurity);
			this.tabPageTools.Controls.Add(this.chkActivateOperatorManagement);
			this.tabPageTools.ForeColor = global::System.Drawing.Color.White;
			this.tabPageTools.Name = "tabPageTools";
			componentResourceManager.ApplyResources(this.chkActivateSecondLevelSecurity, "chkActivateSecondLevelSecurity");
			this.chkActivateSecondLevelSecurity.Name = "chkActivateSecondLevelSecurity";
			this.chkActivateSecondLevelSecurity.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateOperatorManagement, "chkActivateOperatorManagement");
			this.chkActivateOperatorManagement.Name = "chkActivateOperatorManagement";
			this.chkActivateOperatorManagement.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPageOther, "tabPageOther");
			this.tabPageOther.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPageOther.Controls.Add(this.btnFaceDeviceType);
			this.tabPageOther.Controls.Add(this.chkActivateCreateQRCode);
			this.tabPageOther.Controls.Add(this.chkSFZIDAsCardNO);
			this.tabPageOther.Controls.Add(this.chkPhotoNameFromConsumerNO);
			this.tabPageOther.Controls.Add(this.chkActivateDisplayCertID);
			this.tabPageOther.Controls.Add(this.chkActivateFaceManagement);
			this.tabPageOther.Controls.Add(this.chkGetSwipeRecordsContinuously);
			this.tabPageOther.Controls.Add(this.btnSetupNormalOpen);
			this.tabPageOther.Controls.Add(this.chkActivateSFZReader);
			this.tabPageOther.Controls.Add(this.chkActivateAccelerator);
			this.tabPageOther.Controls.Add(this.chkDisableReaderLEDBEEPEROutput);
			this.tabPageOther.Controls.Add(this.chkActivateDontAutoLoadSwipeRecords);
			this.tabPageOther.Controls.Add(this.chkActivateSwipeFourTimeSetNormalOpen);
			this.tabPageOther.Controls.Add(this.chkActivateMobileAsCardInput);
			this.tabPageOther.Controls.Add(this.chkActivateDisplayYellowWhenDoorOpen);
			this.tabPageOther.Controls.Add(this.chkActivateDontDisplayRebootRecords);
			this.tabPageOther.Controls.Add(this.chkActivateDontDisplayDoorStatusRecords);
			this.tabPageOther.Controls.Add(this.chkActivateDontAutoLoadPrivileges);
			this.tabPageOther.ForeColor = global::System.Drawing.Color.White;
			this.tabPageOther.Name = "tabPageOther";
			componentResourceManager.ApplyResources(this.btnFaceDeviceType, "btnFaceDeviceType");
			this.btnFaceDeviceType.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFaceDeviceType.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFaceDeviceType.ForeColor = global::System.Drawing.Color.White;
			this.btnFaceDeviceType.Name = "btnFaceDeviceType";
			this.btnFaceDeviceType.UseVisualStyleBackColor = false;
			this.btnFaceDeviceType.Click += new global::System.EventHandler(this.btnFaceDeviceType_Click);
			componentResourceManager.ApplyResources(this.chkActivateCreateQRCode, "chkActivateCreateQRCode");
			this.chkActivateCreateQRCode.Name = "chkActivateCreateQRCode";
			this.chkActivateCreateQRCode.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSFZIDAsCardNO, "chkSFZIDAsCardNO");
			this.chkSFZIDAsCardNO.BackColor = global::System.Drawing.Color.Red;
			this.chkSFZIDAsCardNO.Checked = true;
			this.chkSFZIDAsCardNO.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkSFZIDAsCardNO.Name = "chkSFZIDAsCardNO";
			this.chkSFZIDAsCardNO.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkPhotoNameFromConsumerNO, "chkPhotoNameFromConsumerNO");
			this.chkPhotoNameFromConsumerNO.Name = "chkPhotoNameFromConsumerNO";
			this.chkPhotoNameFromConsumerNO.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateDisplayCertID, "chkActivateDisplayCertID");
			this.chkActivateDisplayCertID.Name = "chkActivateDisplayCertID";
			this.chkActivateDisplayCertID.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateFaceManagement, "chkActivateFaceManagement");
			this.chkActivateFaceManagement.Name = "chkActivateFaceManagement";
			this.chkActivateFaceManagement.UseVisualStyleBackColor = false;
			this.chkActivateFaceManagement.CheckedChanged += new global::System.EventHandler(this.chkActivateFaceManagement_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkGetSwipeRecordsContinuously, "chkGetSwipeRecordsContinuously");
			this.chkGetSwipeRecordsContinuously.Name = "chkGetSwipeRecordsContinuously";
			this.chkGetSwipeRecordsContinuously.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnSetupNormalOpen, "btnSetupNormalOpen");
			this.btnSetupNormalOpen.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSetupNormalOpen.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSetupNormalOpen.ForeColor = global::System.Drawing.Color.White;
			this.btnSetupNormalOpen.Name = "btnSetupNormalOpen";
			this.btnSetupNormalOpen.UseVisualStyleBackColor = false;
			this.btnSetupNormalOpen.Click += new global::System.EventHandler(this.btnSetupNormalOpen_Click);
			componentResourceManager.ApplyResources(this.chkActivateSFZReader, "chkActivateSFZReader");
			this.chkActivateSFZReader.BackColor = global::System.Drawing.Color.Red;
			this.chkActivateSFZReader.Name = "chkActivateSFZReader";
			this.chkActivateSFZReader.UseVisualStyleBackColor = false;
			this.chkActivateSFZReader.CheckedChanged += new global::System.EventHandler(this.chkActivateSFZReader_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkActivateAccelerator, "chkActivateAccelerator");
			this.chkActivateAccelerator.Name = "chkActivateAccelerator";
			this.chkActivateAccelerator.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkDisableReaderLEDBEEPEROutput, "chkDisableReaderLEDBEEPEROutput");
			this.chkDisableReaderLEDBEEPEROutput.BackColor = global::System.Drawing.Color.Red;
			this.chkDisableReaderLEDBEEPEROutput.Name = "chkDisableReaderLEDBEEPEROutput";
			this.chkDisableReaderLEDBEEPEROutput.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkActivateDontAutoLoadSwipeRecords, "chkActivateDontAutoLoadSwipeRecords");
			this.chkActivateDontAutoLoadSwipeRecords.Name = "chkActivateDontAutoLoadSwipeRecords";
			this.chkActivateDontAutoLoadSwipeRecords.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateSwipeFourTimeSetNormalOpen, "chkActivateSwipeFourTimeSetNormalOpen");
			this.chkActivateSwipeFourTimeSetNormalOpen.BackColor = global::System.Drawing.Color.Red;
			this.chkActivateSwipeFourTimeSetNormalOpen.Name = "chkActivateSwipeFourTimeSetNormalOpen";
			this.chkActivateSwipeFourTimeSetNormalOpen.UseVisualStyleBackColor = false;
			this.chkActivateSwipeFourTimeSetNormalOpen.CheckedChanged += new global::System.EventHandler(this.chkActivateSwipeFourTimeSetNormalOpen_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkActivateMobileAsCardInput, "chkActivateMobileAsCardInput");
			this.chkActivateMobileAsCardInput.BackColor = global::System.Drawing.Color.Red;
			this.chkActivateMobileAsCardInput.Name = "chkActivateMobileAsCardInput";
			this.chkActivateMobileAsCardInput.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkActivateDisplayYellowWhenDoorOpen, "chkActivateDisplayYellowWhenDoorOpen");
			this.chkActivateDisplayYellowWhenDoorOpen.Name = "chkActivateDisplayYellowWhenDoorOpen";
			this.chkActivateDisplayYellowWhenDoorOpen.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateDontDisplayRebootRecords, "chkActivateDontDisplayRebootRecords");
			this.chkActivateDontDisplayRebootRecords.Name = "chkActivateDontDisplayRebootRecords";
			this.chkActivateDontDisplayRebootRecords.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateDontDisplayDoorStatusRecords, "chkActivateDontDisplayDoorStatusRecords");
			this.chkActivateDontDisplayDoorStatusRecords.Name = "chkActivateDontDisplayDoorStatusRecords";
			this.chkActivateDontDisplayDoorStatusRecords.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkActivateDontAutoLoadPrivileges, "chkActivateDontAutoLoadPrivileges");
			this.chkActivateDontAutoLoadPrivileges.Name = "chkActivateDontAutoLoadPrivileges";
			this.chkActivateDontAutoLoadPrivileges.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmExtendedFunctions";
			base.Load += new global::System.EventHandler(this.dfrmExtendedFunctions_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmExtendedFunctions_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.tabPageFile.ResumeLayout(false);
			this.tabPageFile.PerformLayout();
			this.tabPageConfigure.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudValidSwipeGap).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPageOperate.ResumeLayout(false);
			this.tabPageOperate.PerformLayout();
			this.tabPageOneCardMultiFunc.ResumeLayout(false);
			this.tabPageOneCardMultiFunc.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfDay).EndInit();
			this.tabPageTools.ResumeLayout(false);
			this.tabPageTools.PerformLayout();
			this.tabPageOther.ResumeLayout(false);
			this.tabPageOther.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000268 RID: 616
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000269 RID: 617
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400026A RID: 618
		private global::System.Windows.Forms.Button btnFaceDeviceType;

		// Token: 0x0400026B RID: 619
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x0400026C RID: 620
		private global::System.Windows.Forms.Button btnSetup;

		// Token: 0x0400026D RID: 621
		private global::System.Windows.Forms.Button btnSetupNormalOpen;

		// Token: 0x0400026E RID: 622
		private global::System.Windows.Forms.CheckBox chkActivate19Card;

		// Token: 0x0400026F RID: 623
		private global::System.Windows.Forms.CheckBox chkActivateAccelerator;

		// Token: 0x04000270 RID: 624
		private global::System.Windows.Forms.CheckBox chkActivateAccessKeypad;

		// Token: 0x04000271 RID: 625
		private global::System.Windows.Forms.CheckBox chkActivateAntiPassBack;

		// Token: 0x04000272 RID: 626
		private global::System.Windows.Forms.CheckBox chkActivateCamera;

		// Token: 0x04000273 RID: 627
		private global::System.Windows.Forms.CheckBox chkActivateControllerTaskList;

		// Token: 0x04000274 RID: 628
		private global::System.Windows.Forms.CheckBox chkActivateCreateQRCode;

		// Token: 0x04000275 RID: 629
		private global::System.Windows.Forms.CheckBox chkActivateDisplayCertAndTel;

		// Token: 0x04000276 RID: 630
		private global::System.Windows.Forms.CheckBox chkActivateDisplayCertID;

		// Token: 0x04000277 RID: 631
		private global::System.Windows.Forms.CheckBox chkActivateDisplayYellowWhenDoorOpen;

		// Token: 0x04000278 RID: 632
		private global::System.Windows.Forms.CheckBox chkActivateDontAutoLoadPrivileges;

		// Token: 0x04000279 RID: 633
		private global::System.Windows.Forms.CheckBox chkActivateDontAutoLoadSwipeRecords;

		// Token: 0x0400027A RID: 634
		private global::System.Windows.Forms.CheckBox chkActivateDontDisplayAccessControl;

		// Token: 0x0400027B RID: 635
		private global::System.Windows.Forms.CheckBox chkActivateDontDisplayAttendance;

		// Token: 0x0400027C RID: 636
		private global::System.Windows.Forms.CheckBox chkActivateDontDisplayDoorStatusRecords;

		// Token: 0x0400027D RID: 637
		private global::System.Windows.Forms.CheckBox chkActivateDontDisplayOneCardMultifunction;

		// Token: 0x0400027E RID: 638
		private global::System.Windows.Forms.CheckBox chkActivateDontDisplayRebootRecords;

		// Token: 0x0400027F RID: 639
		private global::System.Windows.Forms.CheckBox chkActivateDoorAsSwitch;

		// Token: 0x04000280 RID: 640
		private global::System.Windows.Forms.CheckBox chkActivateElevator;

		// Token: 0x04000281 RID: 641
		private global::System.Windows.Forms.CheckBox chkActivateFaceManagement;

		// Token: 0x04000282 RID: 642
		private global::System.Windows.Forms.CheckBox chkActivateFingerprintManagement;

		// Token: 0x04000283 RID: 643
		private global::System.Windows.Forms.CheckBox chkActivateFirstCardOpen;

		// Token: 0x04000284 RID: 644
		private global::System.Windows.Forms.CheckBox chkActivateGlobalAntiBack;

		// Token: 0x04000285 RID: 645
		private global::System.Windows.Forms.CheckBox chkActivateInterLock;

		// Token: 0x04000286 RID: 646
		private global::System.Windows.Forms.CheckBox chkActivateInvalidCardMoreTimesWarn;

		// Token: 0x04000287 RID: 647
		private global::System.Windows.Forms.CheckBox chkActivateLocate;

		// Token: 0x04000288 RID: 648
		private global::System.Windows.Forms.CheckBox chkActivateMaps;

		// Token: 0x04000289 RID: 649
		private global::System.Windows.Forms.CheckBox chkActivateMeal;

		// Token: 0x0400028A RID: 650
		private global::System.Windows.Forms.CheckBox chkActivateMeeting;

		// Token: 0x0400028B RID: 651
		private global::System.Windows.Forms.CheckBox chkActivateMobileAsCardInput;

		// Token: 0x0400028C RID: 652
		private global::System.Windows.Forms.CheckBox chkActivateMultiCardAccess;

		// Token: 0x0400028D RID: 653
		private global::System.Windows.Forms.CheckBox chkActivateOpenTooLongWarn;

		// Token: 0x0400028E RID: 654
		private global::System.Windows.Forms.CheckBox chkActivateOperatorManagement;

		// Token: 0x0400028F RID: 655
		private global::System.Windows.Forms.CheckBox chkActivateOtherShiftSchedule;

		// Token: 0x04000290 RID: 656
		private global::System.Windows.Forms.CheckBox chkActivatePatrol;

		// Token: 0x04000291 RID: 657
		private global::System.Windows.Forms.CheckBox chkActivatePCCheckAccess;

		// Token: 0x04000292 RID: 658
		private global::System.Windows.Forms.CheckBox chkActivatePCCheckMealOpen;

		// Token: 0x04000293 RID: 659
		private global::System.Windows.Forms.CheckBox chkActivatePeripheralControl;

		// Token: 0x04000294 RID: 660
		private global::System.Windows.Forms.CheckBox chkActivatePersonInside;

		// Token: 0x04000295 RID: 661
		private global::System.Windows.Forms.CheckBox chkActivateRemoteOpenDoor;

		// Token: 0x04000296 RID: 662
		private global::System.Windows.Forms.CheckBox chkActivateSecondLevelSecurity;

		// Token: 0x04000297 RID: 663
		private global::System.Windows.Forms.CheckBox chkActivateSFZReader;

		// Token: 0x04000298 RID: 664
		private global::System.Windows.Forms.CheckBox chkActivateSwipeFourTimeSetNormalOpen;

		// Token: 0x04000299 RID: 665
		private global::System.Windows.Forms.CheckBox chkActivateTimeProfile;

		// Token: 0x0400029A RID: 666
		private global::System.Windows.Forms.CheckBox chkActivateTimeSecond;

		// Token: 0x0400029B RID: 667
		private global::System.Windows.Forms.CheckBox chkActivateTimeSegLimittedAccess;

		// Token: 0x0400029C RID: 668
		private global::System.Windows.Forms.CheckBox chkActivateWarnForceWithCard;

		// Token: 0x0400029D RID: 669
		private global::System.Windows.Forms.CheckBox chkActiveLogQuery;

		// Token: 0x0400029E RID: 670
		private global::System.Windows.Forms.CheckBox chkActivityPrivilegeTypeManagementMode;

		// Token: 0x0400029F RID: 671
		private global::System.Windows.Forms.CheckBox chkDisableReaderLEDBEEPEROutput;

		// Token: 0x040002A0 RID: 672
		private global::System.Windows.Forms.CheckBox chkGetSwipeRecordsContinuously;

		// Token: 0x040002A1 RID: 673
		private global::System.Windows.Forms.CheckBox chkImportExtern;

		// Token: 0x040002A2 RID: 674
		private global::System.Windows.Forms.CheckBox chkPhotoNameFromConsumerNO;

		// Token: 0x040002A3 RID: 675
		private global::System.Windows.Forms.CheckBox chkRecordButtonEvent;

		// Token: 0x040002A4 RID: 676
		private global::System.Windows.Forms.CheckBox chkRecordDoorStatusEvent;

		// Token: 0x040002A5 RID: 677
		private global::System.Windows.Forms.CheckBox chkSFZIDAsCardNO;

		// Token: 0x040002A6 RID: 678
		private global::System.Windows.Forms.CheckBox chkSpeacialOption;

		// Token: 0x040002A7 RID: 679
		private global::System.Windows.Forms.CheckBox chkValidSwipeGap;

		// Token: 0x040002A8 RID: 680
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040002A9 RID: 681
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040002AA RID: 682
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x040002AB RID: 683
		private global::System.Windows.Forms.Label lblOneDayLimit;

		// Token: 0x040002AC RID: 684
		private global::System.Windows.Forms.NumericUpDown nudf_LimitedTimesOfDay;

		// Token: 0x040002AD RID: 685
		private global::System.Windows.Forms.NumericUpDown nudValidSwipeGap;

		// Token: 0x040002AE RID: 686
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x040002AF RID: 687
		private global::System.Windows.Forms.TabPage tabPageConfigure;

		// Token: 0x040002B0 RID: 688
		private global::System.Windows.Forms.TabPage tabPageFile;

		// Token: 0x040002B1 RID: 689
		private global::System.Windows.Forms.TabPage tabPageOneCardMultiFunc;

		// Token: 0x040002B2 RID: 690
		private global::System.Windows.Forms.TabPage tabPageOperate;

		// Token: 0x040002B3 RID: 691
		private global::System.Windows.Forms.TabPage tabPageOther;

		// Token: 0x040002B4 RID: 692
		private global::System.Windows.Forms.TabPage tabPageTools;
	}
}
