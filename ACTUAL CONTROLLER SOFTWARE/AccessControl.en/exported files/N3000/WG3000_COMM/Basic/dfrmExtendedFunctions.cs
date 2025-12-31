using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc.Elevator;
using WG3000_COMM.ExtendFunc.Normal;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000014 RID: 20
	public partial class dfrmExtendedFunctions : frmN3000
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x000230EC File Offset: 0x000220EC
		public dfrmExtendedFunctions()
		{
			this.InitializeComponent();
			this.tabPageFile.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPageConfigure.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPageOperate.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPageOneCardMultiFunc.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPageTools.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPageOther.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000231A1 File Offset: 0x000221A1
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000231B0 File Offset: 0x000221B0
		private void btnFaceDeviceType_Click(object sender, EventArgs e)
		{
			using (dfrmFaceDeviceTypeSetup dfrmFaceDeviceTypeSetup = new dfrmFaceDeviceTypeSetup())
			{
				dfrmFaceDeviceTypeSetup.ShowDialog();
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000231E8 File Offset: 0x000221E8
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessControlBlue && (this.chkActivateCamera.Checked || this.chkActivateAccelerator.Checked || this.chkActivityPrivilegeTypeManagementMode.Checked || this.chkActivatePCCheckMealOpen.Checked || this.chkActivateGlobalAntiBack.Checked || this.chkActivateElevator.Checked))
			{
				XMessageBox.Show(CommonStr.strDontSupportThoseFunction2015, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (wgAppConfig.IsAccessDB && this.chkActivateGlobalAntiBack.Checked)
			{
				XMessageBox.Show(CommonStr.strGlobalAntiPassbackOnlySupportSqlServer2015, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			bool isPrivilegeTypeManagementModeActive = wgAppConfig.IsPrivilegeTypeManagementModeActive;
			this.btnOK.Enabled = false;
			wgAppConfig.setSystemParamValueBool(102, this.chkRecordDoorStatusEvent.Checked);
			wgAppConfig.setSystemParamValueBool(101, this.chkRecordDoorStatusEvent.Checked);
			wgAppConfig.setSystemParamValueBool(103, this.chkActiveLogQuery.Checked);
			wgAppConfig.setSystemParamValueBool(111, this.chkActivateDontDisplayAccessControl.Checked);
			wgAppConfig.setSystemParamValueBool(112, this.chkActivateDontDisplayAttendance.Checked);
			wgAppConfig.setSystemParamValueBool(113, this.chkActivateOtherShiftSchedule.Checked);
			wgAppConfig.setSystemParamValueBool(114, this.chkActivateMaps.Checked);
			wgAppConfig.setSystemParamValueBool(121, this.chkActivateTimeProfile.Checked);
			wgAppConfig.setSystemParamValueBool(122, this.chkActivateRemoteOpenDoor.Checked);
			wgAppConfig.setSystemParamValueBool(123, this.chkActivateAccessKeypad.Checked);
			wgAppConfig.setSystemParamValueBool(124, this.chkActivatePeripheralControl.Checked);
			wgAppConfig.setSystemParamValueBool(148, this.chkActivateOperatorManagement.Checked);
			wgAppConfig.setSystemParamValueBool(131, this.chkActivateControllerTaskList.Checked);
			wgAppConfig.setSystemParamValueBool(132, this.chkActivateAntiPassBack.Checked);
			wgAppConfig.setSystemParamValueBool(133, this.chkActivateInterLock.Checked);
			wgAppConfig.setSystemParamValueBool(134, this.chkActivateMultiCardAccess.Checked);
			wgAppConfig.setSystemParamValueBool(135, this.chkActivateFirstCardOpen.Checked);
			wgAppConfig.setSystemParamValueBool(137, this.chkActivatePCCheckAccess.Checked);
			wgAppConfig.setSystemParamValueBool(136, this.chkActivateTimeSegLimittedAccess.Checked);
			wgAppConfig.setSystemParamValueBool(146, this.chkActivateDoorAsSwitch.Checked);
			wgAppConfig.setSystemParamValueBool(141, this.chkActivateWarnForceWithCard.Checked);
			wgAppConfig.setSystemParamValueBool(142, this.chkActivateDontAutoLoadPrivileges.Checked);
			wgAppConfig.setSystemParamValueBool(143, this.chkActivateDontAutoLoadSwipeRecords.Checked);
			if (!this.chkActivateElevator.Checked)
			{
				wgAppConfig.setSystemParamValueBool(144, false);
			}
			else
			{
				if (this.OneToMoreSelect == 0)
				{
					this.OneToMoreSelect = 1;
				}
				wgAppConfig.setSystemParamValue(144, this.OneToMoreSelect.ToString());
			}
			wgAppConfig.setSystemParamValueBool(149, this.chkActivateMeeting.Checked);
			wgAppConfig.setSystemParamValueBool(150, this.chkActivateMeal.Checked);
			wgAppConfig.setSystemParamValueBool(151, this.chkActivatePatrol.Checked);
			wgAppConfig.setSystemParamValue(152, "Activate Mobile As Card Input", this.chkActivateMobileAsCardInput.Checked ? "1" : "0", " Driver Version V5.40 above - 20130508");
			wgAppConfig.setSystemParamValue(165, "Disable Reader LEDBEEPER Output When InvalidCard", this.chkDisableReaderLEDBEEPEROutput.Checked ? "1" : "0", " Driver Version V5.48 above - 20140318");
			wgAppConfig.setSystemParamValue(168, "Activate SFZ Reader", this.chkActivateSFZReader.Checked ? "1" : "0", "DEVICE: CVR-100U, 20140417");
			wgAppConfig.setSystemParamValue(194, "Activate SFZ Reader ID as Card NO", this.chkSFZIDAsCardNO.Checked ? "1" : "0", "DEVICE: CVR-100U, 2017-06-30 15:09:49");
			wgAppConfig.setSystemParamValue(186, "Activate Face Management", this.chkActivateFaceManagement.Checked ? "1" : "0", "DEVICE: E356A, 2016-04-17");
			wgAppConfig.setSystemParamValue(188, "Activate Fingerprint Management", this.chkActivateFingerprintManagement.Checked ? "1" : "0", "Finger print, 2016-05-09");
			wgAppConfig.setSystemParamValue(195, "ActivateCreateQRCode", this.chkActivateCreateQRCode.Checked ? "1" : "0", "Driver 8.80, 2017-10-10 17:29:17");
			wgAppConfig.setSystemParamValue(163, "Activate Do not Display DoorStatus Records", this.chkActivateDontDisplayDoorStatusRecords.Checked ? "1" : "0", "2014-02-14 12:40:23");
			wgAppConfig.setSystemParamValue(164, "Activate Do not Display Reboot Records", this.chkActivateDontDisplayRebootRecords.Checked ? "1" : "0", "2014-02-14 12:40:23");
			wgAppConfig.setSystemParamValue(173, "Activate Display Yellow When Door Is Open", this.chkActivateDisplayYellowWhenDoorOpen.Checked ? "1" : "0", "2014-12-31 08:09:23");
			if (!wgAppConfig.IsAccessControlBlue)
			{
				wgAppConfig.setSystemParamValue(156, "Activate Camera Manage", this.chkActivateCamera.Checked ? "1" : "0", "Camera View-20130927");
				wgAppConfig.setSystemParamValue(166, "Activate Accelerator", this.chkActivateAccelerator.Checked ? "1" : "0", "Commucation Accelerator-20140403");
				wgAppConfig.setSystemParamValue(167, "Activate Privliege Type Management Mode", this.chkActivityPrivilegeTypeManagementMode.Checked ? "1" : "0", "Activity Privilege Type Management Mode-20140406");
				wgAppConfig.setSystemParamValue(169, "Activate PC Check Meal Open", this.chkActivatePCCheckMealOpen.Checked ? this.nudf_LimitedTimesOfDay.Value.ToString() : "0", "Activate PC Check Meal Open Once-20140420");
				wgAppConfig.setSystemParamValue(181, "ActivateGlobalAntiBackOpen", this.chkActivateGlobalAntiBack.Checked ? "1" : "0", "2015-11-15 11:20:42");
				wgAppConfig.setSystemParamValue(191, "ActivateTimeSecond", this.chkActivateTimeSecond.Checked ? "1" : "0", "2016-09-02 10:12:00");
				icConsumer.gTimeSecondEnabled = wgAppConfig.getParamValBoolByNO(191);
				wgAppConfig.IsActivateCard19 = this.chkActivate19Card.Checked;
				wgAppConfig.IsPhotoNameFromConsumerNO = this.chkPhotoNameFromConsumerNO.Checked;
				wgAppConfig.setSystemParamValue(156, "Activate Camera Manage", this.chkActivateCamera.Checked ? "1" : "0", "Camera View-20130927");
				wgAppConfig.setSystemParamValue(214, "DoorAsSwitch_Option", this.chkSpeacialOption.Checked ? "1" : "0", "2019-02-28 16:48:32");
			}
			wgAppConfig.setSystemParamValue(217, "DontDisplayOneCardMultifunction", this.chkActivateDontDisplayOneCardMultifunction.Checked ? "1" : "0", "2019-07-03 17:14:35");
			wgAppConfig.setSystemParamValue(161, "Activate Locate", this.chkActivateLocate.Checked ? "1" : "0", "2014-02-12");
			wgAppConfig.setSystemParamValue(162, "Activate Person Inside", this.chkActivatePersonInside.Checked ? "1" : "0", "2014-02-12");
			string text = "";
			string text2 = "";
			string text3 = "";
			wgAppConfig.getSystemParamValue(170, out text, out text2, out text3);
			wgAppConfig.setSystemParamValue(170, "Activate Swipe Four Times Set NormalOpen", this.chkActivateSwipeFourTimeSetNormalOpen.Checked ? "1" : "0", this.chkActivateSwipeFourTimeSetNormalOpen.Checked ? text3 : "0");
			wgAppConfig.setSystemParamValue(177, "Get_Swipe_Records_Continuously", this.chkGetSwipeRecordsContinuously.Checked ? "1" : "0", "2015-05-27 10:49:17");
			wgAppConfig.setSystemParamValue(180, "ActivateOpenTooLongWarn V6.58", this.chkActivateOpenTooLongWarn.Checked ? "1" : "0", "2015-09-20 20:36:16");
			wgAppConfig.IsActivateOpenTooLongWarn = this.chkActivateOpenTooLongWarn.Checked;
			wgAppConfig.setSystemParamValue(211, "ActivateOpenInvalidCardMoreTimesWarn V8.96", this.chkActivateInvalidCardMoreTimesWarn.Checked ? "3" : "0", "2018-12-28 19:52:29");
			wgAppConfig.IsActivateInvalidCardMoreTimesWarn = this.chkActivateInvalidCardMoreTimesWarn.Checked;
			bool isPrivilegeTypeManagementModeActive2 = wgAppConfig.IsPrivilegeTypeManagementModeActive;
			if (this.chkActivateGlobalAntiBack.Checked && this.chkActivateTimeProfile.Checked)
			{
				XMessageBox.Show(CommonStr.strGlobalAntiPassbackDontSupportTimeProfile2015, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			wgAppConfig.setSystemParamValue(187, "Activate Display Cert. ID", this.chkActivateDisplayCertID.Checked ? "1" : "0", "2016-04-19 16:03:37");
			wgAppConfig.setSystemParamValue(197, "ActivateDisplayCertAndTel", this.chkActivateDisplayCertAndTel.Checked ? "1" : "0", "2017-12-19 21:13:13");
			wgAppConfig.setSystemParamValue(198, "ActivateImportExtern", this.chkImportExtern.Checked ? "1" : "0", "2017-12-19 21:13:13");
			wgAppConfig.setSystemParamValue(208, "Deactive ElevatorNO", this.bDeactiveNO.ToString(), "2018-09-12 18:45:24");
			int num = 0;
			if (this.chkValidSwipeGap.Checked)
			{
				num = (int)this.nudValidSwipeGap.Value;
			}
			if ((num & 1) > 0)
			{
				num++;
			}
			wgAppConfig.setSystemParamValue(147, num.ToString());
			if (icConsumer.gTimeSecondEnabled)
			{
				string text4 = "SELECT f_ControllerSN FROM t_b_Controller WHERE  f_ControllerSN >400000000 and f_ControllerSN< 499999999";
				if (wgAppConfig.getValBySql(text4) > 0)
				{
					wgAppConfig.setSystemParamValue(191, "ActivateTimeSecond", "0", "2016-09-02 10:12:00");
					icConsumer.gTimeSecondEnabled = wgAppConfig.getParamValBoolByNO(191);
					XMessageBox.Show(CommonStr.strTimeSecondEnableWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			wgAppConfig.setSystemParamValue(220, "ActivateSecondLevelSecurity", this.chkActivateSecondLevelSecurity.Checked ? "1" : "0", "2019-09-05 11:56:40");
			base.DialogResult = DialogResult.Cancel;
			if (XMessageBox.Show(this, CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00023BDC File Offset: 0x00022BDC
		private void btnSetup_Click(object sender, EventArgs e)
		{
			using (dfrmOneToMoreSetup dfrmOneToMoreSetup = new dfrmOneToMoreSetup())
			{
				dfrmOneToMoreSetup.radioButton0.Checked = true;
				dfrmOneToMoreSetup.radioButton1.Checked = (this.OneToMoreSelect & 255) == 2;
				dfrmOneToMoreSetup.radioButton2.Checked = (this.OneToMoreSelect & 255) == 3;
				dfrmOneToMoreSetup.chkDeactiveNO.Checked = this.bDeactiveNO > 0;
				if (this.OneToMoreSelect > 3)
				{
					try
					{
						dfrmOneToMoreSetup.numericUpDown21.Value = ((this.OneToMoreSelect >> 8) & 255) / 10m;
						dfrmOneToMoreSetup.numericUpDown1.Value = ((this.OneToMoreSelect >> 16) & 255) % 10;
						dfrmOneToMoreSetup.numericUpDown20.Value = (((this.OneToMoreSelect >> 16) & 255) - ((this.OneToMoreSelect >> 16) & 255) % 10) / 10m;
						dfrmOneToMoreSetup.Size = new Size(554, 259);
					}
					catch (Exception)
					{
					}
				}
				if (dfrmOneToMoreSetup.ShowDialog() == DialogResult.OK)
				{
					this.OneToMoreSelect = 1;
					if (dfrmOneToMoreSetup.radioButton1.Checked)
					{
						this.OneToMoreSelect = 2;
					}
					if (dfrmOneToMoreSetup.radioButton2.Checked)
					{
						this.OneToMoreSelect = 3;
					}
					if (!(dfrmOneToMoreSetup.numericUpDown21.Value == 0.4m) || !(dfrmOneToMoreSetup.numericUpDown20.Value == 5m) || !(dfrmOneToMoreSetup.numericUpDown1.Value == 0m))
					{
						this.OneToMoreSelect += (int)(dfrmOneToMoreSetup.numericUpDown21.Value * 10m) << 8;
						this.OneToMoreSelect += (int)(dfrmOneToMoreSetup.numericUpDown20.Value * 10m) << 16;
						this.OneToMoreSelect += (int)dfrmOneToMoreSetup.numericUpDown1.Value << 16;
					}
					this.bDeactiveNO = (dfrmOneToMoreSetup.chkDeactiveNO.Checked ? 1 : 0);
				}
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00023E48 File Offset: 0x00022E48
		private void btnSetupNormalOpen_Click(object sender, EventArgs e)
		{
			using (dfrmReader4SwitchNormalOpen dfrmReader4SwitchNormalOpen = new dfrmReader4SwitchNormalOpen())
			{
				string text = "";
				string text2 = "";
				string text3 = "";
				wgAppConfig.getSystemParamValue(170, out text, out text2, out text3);
				if (!string.IsNullOrEmpty(wgTools.SetObjToStr(text3)))
				{
					string[] array = text3.Split(new char[] { ';' });
					if (array.Length >= 2)
					{
						int num = 0;
						int.TryParse(array[0], out num);
						dfrmReader4SwitchNormalOpen.timeProfile = num;
						dfrmReader4SwitchNormalOpen.strEnabledReaders = array[1];
					}
				}
				if (dfrmReader4SwitchNormalOpen.ShowDialog() == DialogResult.OK)
				{
					wgAppConfig.setSystemParamValue(170, "Activate Swipe Four Times Set NormalOpen", "1", dfrmReader4SwitchNormalOpen.strEnabledReaders);
				}
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00023F0C File Offset: 0x00022F0C
		private void chkActivateElevator_CheckedChanged(object sender, EventArgs e)
		{
			this.btnSetup.Visible = this.chkActivateElevator.Checked;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00023F24 File Offset: 0x00022F24
		private void chkActivateFaceManagement_CheckedChanged(object sender, EventArgs e)
		{
			this.btnFaceDeviceType.Visible = this.chkActivateFaceManagement.Checked;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00023F3C File Offset: 0x00022F3C
		private void chkActivateSFZReader_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActivateSFZReader.Checked && this.bIsLoaded)
			{
				if (!this.chkSFZIDAsCardNO.Visible)
				{
					this.chkSFZIDAsCardNO.Visible = true;
					this.chkSFZIDAsCardNO.Checked = true;
				}
				XMessageBox.Show(CommonStr.strSFZSupport);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00023F8E File Offset: 0x00022F8E
		private void chkActivateSwipeFourTimeSetNormalOpen_CheckedChanged(object sender, EventArgs e)
		{
			this.btnSetupNormalOpen.Visible = this.chkActivateSwipeFourTimeSetNormalOpen.Checked;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00023FA6 File Offset: 0x00022FA6
		private void dfrmExtendedFunctions_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.funcCtrlShiftQ();
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00023FE8 File Offset: 0x00022FE8
		private void dfrmExtendedFunctions_Load(object sender, EventArgs e)
		{
			this.chkRecordButtonEvent.Checked = wgAppConfig.getParamValBoolByNO(101);
			this.chkRecordDoorStatusEvent.Checked = wgAppConfig.getParamValBoolByNO(102);
			this.chkActiveLogQuery.Checked = wgAppConfig.getParamValBoolByNO(103);
			this.chkActivateDontDisplayAccessControl.Checked = wgAppConfig.getParamValBoolByNO(111);
			this.chkActivateDontDisplayAttendance.Checked = wgAppConfig.getParamValBoolByNO(112);
			this.chkActivateOtherShiftSchedule.Checked = wgAppConfig.getParamValBoolByNO(113);
			this.chkActivateMaps.Checked = wgAppConfig.getParamValBoolByNO(114);
			this.chkActivateTimeProfile.Checked = wgAppConfig.getParamValBoolByNO(121);
			this.chkActivateRemoteOpenDoor.Checked = wgAppConfig.getParamValBoolByNO(122);
			this.chkActivateAccessKeypad.Checked = wgAppConfig.getParamValBoolByNO(123);
			this.chkActivatePeripheralControl.Checked = wgAppConfig.getParamValBoolByNO(124);
			this.chkActivateOpenTooLongWarn.Checked = wgAppConfig.getParamValBoolByNO(180);
			this.chkActivateInvalidCardMoreTimesWarn.Checked = wgAppConfig.getParamValBoolByNO(211);
			this.chkActivateOperatorManagement.Checked = wgAppConfig.getParamValBoolByNO(148);
			this.chkActivateControllerTaskList.Checked = wgAppConfig.getParamValBoolByNO(131);
			this.chkActivateAntiPassBack.Checked = wgAppConfig.getParamValBoolByNO(132);
			this.chkActivateInterLock.Checked = wgAppConfig.getParamValBoolByNO(133);
			this.chkActivateMultiCardAccess.Checked = wgAppConfig.getParamValBoolByNO(134);
			this.chkActivateFirstCardOpen.Checked = wgAppConfig.getParamValBoolByNO(135);
			this.chkActivatePCCheckAccess.Checked = wgAppConfig.getParamValBoolByNO(137);
			this.chkActivateTimeSegLimittedAccess.Checked = wgAppConfig.getParamValBoolByNO(136);
			this.chkActivateDoorAsSwitch.Checked = wgAppConfig.getParamValBoolByNO(146);
			this.chkActivateWarnForceWithCard.Checked = wgAppConfig.getParamValBoolByNO(141);
			this.chkActivateDontAutoLoadPrivileges.Checked = wgAppConfig.getParamValBoolByNO(142);
			this.chkActivateDontAutoLoadSwipeRecords.Checked = wgAppConfig.getParamValBoolByNO(143);
			this.chkActivateElevator.Checked = wgAppConfig.getParamValBoolByNO(144);
			this.OneToMoreSelect = int.Parse("0" + wgAppConfig.getSystemParamByNO(144));
			this.bDeactiveNO = int.Parse("0" + wgAppConfig.getSystemParamByNO(208));
			this.chkActivateMeeting.Checked = wgAppConfig.getParamValBoolByNO(149);
			this.chkActivateMeal.Checked = wgAppConfig.getParamValBoolByNO(150);
			this.chkActivatePatrol.Checked = wgAppConfig.getParamValBoolByNO(151);
			this.chkActivatePCCheckMealOpen.Checked = wgAppConfig.getParamValBoolByNO(169);
			if (int.Parse("0" + wgAppConfig.getSystemParamByNO(169)) > 0)
			{
				this.nudf_LimitedTimesOfDay.Value = int.Parse("0" + wgAppConfig.getSystemParamByNO(169));
			}
			this.chkActivateMobileAsCardInput.Checked = wgAppConfig.getParamValBoolByNO(152);
			this.chkDisableReaderLEDBEEPEROutput.Checked = wgAppConfig.getParamValBoolByNO(165);
			this.chkActivateSFZReader.Checked = wgAppConfig.getParamValBoolByNO(168);
			this.chkActivateCreateQRCode.Checked = wgAppConfig.getParamValBoolByNO(195);
			this.chkSFZIDAsCardNO.Checked = wgAppConfig.getParamValBoolByNO(194);
			this.chkActivateFaceManagement.Checked = wgAppConfig.getParamValBoolByNO(186);
			this.btnFaceDeviceType.Visible = this.chkActivateFaceManagement.Checked;
			this.chkActivateFingerprintManagement.Checked = wgAppConfig.getParamValBoolByNO(188);
			this.chkActivateSwipeFourTimeSetNormalOpen.Checked = wgAppConfig.getParamValBoolByNO(170);
			this.chkActivateDisplayCertAndTel.Checked = wgAppConfig.getParamValBoolByNO(197);
			this.chkImportExtern.Checked = wgAppConfig.getParamValBoolByNO(198);
			if (!this.chkActivateMobileAsCardInput.Checked)
			{
				this.chkActivateMobileAsCardInput.Visible = false;
			}
			if (!this.chkDisableReaderLEDBEEPEROutput.Checked)
			{
				this.chkDisableReaderLEDBEEPEROutput.Visible = false;
			}
			if (!this.chkActivateSFZReader.Checked)
			{
				this.chkActivateSFZReader.Visible = false;
				this.chkSFZIDAsCardNO.Visible = false;
			}
			if (!this.chkActivateSwipeFourTimeSetNormalOpen.Checked)
			{
				this.chkActivateSwipeFourTimeSetNormalOpen.Visible = false;
				this.btnSetupNormalOpen.Visible = false;
			}
			if (!wgAppConfig.IsAccessControlBlue)
			{
				this.chkActivateCamera.Visible = true;
				this.chkActivateCamera.Checked = wgAppConfig.IsActivateCameraManage;
				this.chkActivateAccelerator.Visible = true;
				this.chkActivateAccelerator.Checked = wgAppConfig.IsAcceleratorActive;
				this.chkActivityPrivilegeTypeManagementMode.Checked = wgAppConfig.IsPrivilegeTypeManagementModeActive;
				if (this.chkActivatePCCheckMealOpen.Checked)
				{
					this.chkActivatePCCheckMealOpen.Visible = true;
					this.lblOneDayLimit.Visible = true;
					this.nudf_LimitedTimesOfDay.Visible = true;
				}
				this.chkActivateGlobalAntiBack.Checked = wgAppConfig.getParamValBoolByNO(181);
				this.chkActivateTimeSecond.Visible = true;
				this.chkActivateTimeSecond.Checked = wgAppConfig.getParamValBoolByNO(191);
				this.chkActivate19Card.Visible = true;
				this.chkActivate19Card.Checked = wgAppConfig.IsActivateCard19;
				this.chkPhotoNameFromConsumerNO.Visible = true;
				this.chkPhotoNameFromConsumerNO.Checked = wgAppConfig.IsPhotoNameFromConsumerNO;
				this.chkActivateCreateQRCode.Visible = true;
				if (wgAppConfig.getParamValBoolByNO(214))
				{
					this.chkSpeacialOption.Visible = true;
					this.chkSpeacialOption.Checked = true;
				}
			}
			this.chkActivateDontDisplayOneCardMultifunction.Checked = wgAppConfig.getParamValBoolByNO(217);
			this.chkActivateLocate.Checked = wgAppConfig.getParamValBoolByNO(161);
			this.chkActivatePersonInside.Checked = wgAppConfig.getParamValBoolByNO(162);
			this.chkActivateDontDisplayDoorStatusRecords.Checked = wgAppConfig.getParamValBoolByNO(163);
			this.chkActivateDontDisplayRebootRecords.Checked = wgAppConfig.getParamValBoolByNO(164);
			this.chkActivateDisplayYellowWhenDoorOpen.Checked = wgAppConfig.getParamValBoolByNO(173);
			this.chkGetSwipeRecordsContinuously.Checked = wgAppConfig.getParamValBoolByNO(177);
			this.chkActivateDisplayCertID.Checked = wgAppConfig.getParamValBoolByNO(187);
			this.chkValidSwipeGap.Checked = wgAppConfig.getParamValBoolByNO(147);
			if (int.Parse(wgAppConfig.getSystemParamByNO(147)) > 0)
			{
				try
				{
					this.nudValidSwipeGap.Value = int.Parse(wgAppConfig.getSystemParamByNO(147));
				}
				catch
				{
				}
			}
			this.chkActivateRemoteOpenDoor.Text = wgAppConfig.ReplaceRemoteOpenDoor(this.chkActivateRemoteOpenDoor.Text);
			this.chkActivateSecondLevelSecurity.Checked = wgAppConfig.getParamValBoolByNO(220);
			this.bIsLoaded = true;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0002468C File Offset: 0x0002368C
		private void funcCtrlShiftQ()
		{
			this.chkActivateTimeSegLimittedAccess.Visible = true;
			this.chkActivateDoorAsSwitch.Visible = true;
			this.chkActivateMobileAsCardInput.Visible = true;
			this.chkDisableReaderLEDBEEPEROutput.Visible = true;
			this.chkActivateSFZReader.Visible = true;
			this.chkActivateFaceManagement.Visible = true;
			this.chkActivateSwipeFourTimeSetNormalOpen.Visible = true;
			if (!wgAppConfig.IsAccessControlBlue)
			{
				if (this.bShiftQMoreTime)
				{
					this.chkActivatePCCheckMealOpen.Visible = true;
					this.lblOneDayLimit.Visible = true;
					this.nudf_LimitedTimesOfDay.Visible = true;
				}
				this.chkSpeacialOption.Visible = true;
			}
			this.bShiftQMoreTime = true;
		}

		// Token: 0x04000264 RID: 612
		private int bDeactiveNO;

		// Token: 0x04000265 RID: 613
		private bool bIsLoaded;

		// Token: 0x04000266 RID: 614
		private bool bShiftQMoreTime;

		// Token: 0x04000267 RID: 615
		private int OneToMoreSelect;
	}
}
