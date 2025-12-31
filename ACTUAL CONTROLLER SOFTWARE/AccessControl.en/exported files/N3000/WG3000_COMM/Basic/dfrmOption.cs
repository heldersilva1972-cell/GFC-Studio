using System;
using System.Collections;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using WindowsStartup.Utils;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000023 RID: 35
	public partial class dfrmOption : frmN3000
	{
		// Token: 0x060001EE RID: 494 RVA: 0x000407DC File Offset: 0x0003F7DC
		public dfrmOption()
		{
			this.InitializeComponent();
			this.tabPage3.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage4.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage5.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage6.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0004085D File Offset: 0x0003F85D
		private void absentEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("AbsentEdit");
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0004086A File Offset: 0x0003F86A
		private void absentRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("AbsentRestore");
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00040878 File Offset: 0x0003F878
		private void activeStartupWhenUserLoginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "WindowsStartupWG";
			string executablePath = Application.ExecutablePath;
			string text2 = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run";
			if (RegisterTool.SetValue(text2, text, executablePath))
			{
				XMessageBox.Show(this.activeStartupWhenUserLoginToolStripMenuItem.Text + " OK");
				return;
			}
			XMessageBox.Show(this.activeStartupWhenUserLoginToolStripMenuItem.Text + " " + CommonStr.strFailed);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000408DD File Offset: 0x0003F8DD
		private void auditEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("AuditEdit");
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000408EA File Offset: 0x0003F8EA
		private void auditRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("AuditRestore");
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000408F7 File Offset: 0x0003F8F7
		private void autoRestartWhenNetFailedDisableDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoRestartWhenNetFailed", "0");
			this.updateAutoLoginMode();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0004090E File Offset: 0x0003F90E
		private void autoRestartWhenNetFailedEnabledToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoRestartWhenNetFailed", "1");
			this.updateAutoLoginMode();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00040925 File Offset: 0x0003F925
		private void autoRestartWhenNetFailedToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00040928 File Offset: 0x0003F928
		private void btnOption_Click(object sender, EventArgs e)
		{
			using (dfrmOptionAdvanced dfrmOptionAdvanced = new dfrmOptionAdvanced())
			{
				dfrmOptionAdvanced.ShowDialog();
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00040960 File Offset: 0x0003F960
		private void btnRefreshDateTime_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.cboDateTime.Text))
				{
					this.txtDateTime.Text = "";
				}
				else
				{
					this.txtDateTime.Text = DateTime.Now.ToString(this.cboDateTime.Text);
					DateTime dateTime;
					if (!DateTime.TryParse(this.txtDateTime.Text, out dateTime))
					{
						this.txtDateTime.Text = CommonStr.strDateTimeFormatErr;
					}
				}
			}
			catch (Exception)
			{
				this.txtDateTime.Text = CommonStr.strDateTimeFormatErr;
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00040A00 File Offset: 0x0003FA00
		private void btnRefreshDateTimeWeek_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.cboDateTimeWeek.Text))
				{
					this.txtDateTimeWeek.Text = "";
				}
				else
				{
					this.txtDateTimeWeek.Text = DateTime.Now.ToString(this.cboDateTimeWeek.Text);
					DateTime dateTime;
					if (!DateTime.TryParse(this.txtDateTimeWeek.Text, out dateTime))
					{
						this.txtDateTimeWeek.Text = CommonStr.strDateTimeFormatErr;
					}
				}
			}
			catch (Exception)
			{
				this.txtDateTimeWeek.Text = CommonStr.strDateTimeFormatErr;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00040AA0 File Offset: 0x0003FAA0
		private void btnRefreshDateWeek_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.cboDateWeek.Text))
				{
					this.txtDateWeek.Text = "";
				}
				else
				{
					this.txtDateWeek.Text = DateTime.Now.ToString(this.cboDateWeek.Text);
					DateTime dateTime;
					if (!DateTime.TryParse(this.txtDateWeek.Text, out dateTime))
					{
						this.txtDateWeek.Text = CommonStr.strDateTimeFormatErr;
					}
				}
			}
			catch (Exception)
			{
				this.txtDateWeek.Text = CommonStr.strDateTimeFormatErr;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00040B40 File Offset: 0x0003FB40
		private void btnRefreshOnlyDate_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.cboOnlyDate.Text))
				{
					this.txtOnlyDate.Text = "";
				}
				else
				{
					this.txtOnlyDate.Text = DateTime.Now.ToString(this.cboOnlyDate.Text);
					DateTime dateTime;
					if (!DateTime.TryParse(this.txtOnlyDate.Text, out dateTime))
					{
						this.txtOnlyDate.Text = CommonStr.strDateTimeFormatErr;
					}
				}
			}
			catch (Exception)
			{
				this.txtOnlyDate.Text = CommonStr.strDateTimeFormatErr;
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00040BE0 File Offset: 0x0003FBE0
		private void checkDiskSpaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(204))
			{
				wgAppConfig.setSystemParamValue(204, "0");
			}
			else
			{
				wgAppConfig.setSystemParamValue(204, "ActivateCheckDiskSpace", "1", "2018-06-27 17:21:33");
			}
			this.checkDiskSpaceToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(204);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00040C3C File Offset: 0x0003FC3C
		private bool checkValidAutoTime(string item, string NewVal)
		{
			try
			{
				string text = wgAppConfig.GetKeyVal("AutoUpdateTime");
				string text2 = wgAppConfig.GetKeyVal("AutoGetSwipeRecords");
				string text3 = wgAppConfig.GetKeyVal("AutoUploadPrivileges");
				string text4 = wgAppConfig.GetKeyVal("AutoUploadConfigure");
				string text5 = "";
				if (item != null)
				{
					if (!(item == "AutoUpdateTime"))
					{
						if (!(item == "AutoGetSwipeRecords"))
						{
							if (!(item == "AutoUploadPrivileges"))
							{
								if (item == "AutoUploadConfigure")
								{
									text4 = NewVal;
									text5 = this.setTimeToAutoUploadConfigureToolStripMenuItem.Text;
								}
							}
							else
							{
								text3 = NewVal;
								text5 = this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Text;
							}
						}
						else
						{
							text2 = NewVal;
							text5 = this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Text;
						}
					}
					else
					{
						text = NewVal;
						text5 = this.setCustomTimeToolStripMenuItem.Text;
					}
				}
				if (text5.IndexOf("[") > 0)
				{
					text5 = text5.Substring(0, text5.IndexOf("["));
				}
				string[] array = new string[] { text, text2, text3, text4 };
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						string[] array2 = array[i].Split(new char[] { ',' });
						for (int j = 0; j < array2.Length; j++)
						{
							DateTime dateTime;
							if (DateTime.TryParse(string.Format("2016-03-31 {0}", array2[j].Trim()), out dateTime))
							{
								arrayList.Add(dateTime);
							}
						}
					}
				}
				arrayList.Sort();
				if (arrayList.Count > 1)
				{
					for (int k = 1; k < arrayList.Count; k++)
					{
						if (arrayList[k - 1] == arrayList[k])
						{
							DateTime dateTime2 = (DateTime)arrayList[k - 1];
							XMessageBox.Show(string.Format(CommonStr.strFailedToSetTaskTimeMoreTask + " {0}", dateTime2.ToString("HH:mm")), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}
						if (((DateTime)arrayList[k - 1]).AddMinutes(10.0) > (DateTime)arrayList[k])
						{
							DateTime dateTime3 = (DateTime)arrayList[k - 1];
							DateTime dateTime4 = (DateTime)arrayList[k];
							XMessageBox.Show(string.Format(CommonStr.strFailedToSetTaskTimeTwoTask + " {0},{1}", dateTime3.ToString("HH:mm"), dateTime4.ToString("HH:mm")), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}
					}
					DateTime dateTime5 = (DateTime)arrayList[arrayList.Count - 1];
					DateTime dateTime6 = (DateTime)arrayList[0];
					if (dateTime5.AddMinutes(10.0) > dateTime6.AddDays(1.0))
					{
						DateTime dateTime7 = (DateTime)arrayList[arrayList.Count - 1];
						DateTime dateTime8 = (DateTime)arrayList[0];
						XMessageBox.Show(string.Format(CommonStr.strFailedToSetTaskTimeTwoTask + " {0},{1}", dateTime7.ToString("HH:mm"), dateTime8.ToString("HH:mm")), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
				wgAppConfig.UpdateKeyVal(item, NewVal);
				wgAppConfig.wgLog(string.Format("{0}  Task schedule: {1}={2}", text5, item, NewVal));
				this.updateTaskSchedule();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return true;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00040FF0 File Offset: 0x0003FFF0
		private void cloudServerIP4RemoteControl_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00040FF2 File Offset: 0x0003FFF2
		private void cloudServerIP4RemoteControl_KeyUp(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00040FF4 File Offset: 0x0003FFF4
		private void cloudServerIP4RemoteControl_Leave(object sender, EventArgs e)
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00040FF6 File Offset: 0x0003FFF6
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00041008 File Offset: 0x00040008
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (this.cboLanguage.SelectedIndex == 0)
			{
				wgAppConfig.UpdateKeyVal("Language", "");
			}
			else if (this.cboLanguage.SelectedIndex == 1)
			{
				wgAppConfig.UpdateKeyVal("Language", "zh-CHS");
			}
			else if (this.cboLanguage.SelectedIndex >= 2)
			{
				if (this.cboLanguage.Items[this.cboLanguage.SelectedIndex].ToString().IndexOf("zh-CHT") >= 0)
				{
					wgAppConfig.UpdateKeyVal("Language", "zh-CHT");
				}
				else
				{
					wgAppConfig.UpdateKeyVal("Language", this.cboLanguage.Items[this.cboLanguage.SelectedIndex].ToString());
				}
			}
			if (this.chkAutoLoginOnly.Checked)
			{
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM t_s_Operator WHERE f_OperatorID= " + icOperator.OperatorID, oleDbConnection))
						{
							oleDbConnection.Open();
							OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
							if (oleDbDataReader.Read())
							{
								wgAppConfig.UpdateKeyVal("autologinName", wgTools.SetObjToStr(oleDbDataReader["f_OperatorName"]));
								wgAppConfig.UpdateKeyVal("autologinPassword", wgTools.SetObjToStr(oleDbDataReader["f_Password"]));
							}
							oleDbDataReader.Close();
						}
						goto IL_0214;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM t_s_Operator WHERE f_OperatorID= " + icOperator.OperatorID, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							wgAppConfig.UpdateKeyVal("autologinName", wgTools.SetObjToStr(sqlDataReader["f_OperatorName"]));
							wgAppConfig.UpdateKeyVal("autologinPassword", wgTools.SetObjToStr(sqlDataReader["f_Password"]));
						}
						sqlDataReader.Close();
					}
					goto IL_0214;
				}
			}
			wgAppConfig.UpdateKeyVal("autologinName", "");
			wgAppConfig.UpdateKeyVal("autologinPassword", "");
			IL_0214:
			wgAppConfig.setSystemParamValueBool(145, this.chkHouse.Checked);
			wgAppConfig.UpdateKeyVal("HideGettingStartedWhenLogin", this.chkHideLogin.Checked ? "0" : "1");
			wgAppConfig.setSystemParamValue(178, "Activate Renting House", this.rentingHouseEnabledToolStripMenuItem.Checked ? "1" : "0", "2015-05-28 13:28:01");
			if (this.tabControl1.Visible)
			{
				if (wgTools.IsValidDateTimeFormat(this.cboOnlyDate.Text))
				{
					wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMD", this.cboOnlyDate.Text);
				}
				if (wgTools.IsValidDateTimeFormat(this.cboDateWeek.Text))
				{
					wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMDWeek", this.cboDateWeek.Text);
				}
				if (wgTools.IsValidDateTimeFormat(this.cboDateTime.Text))
				{
					wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMDHMS", this.cboDateTime.Text);
				}
				if (wgTools.IsValidDateTimeFormat(this.cboDateTimeWeek.Text))
				{
					wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMDHMSWeek", this.cboDateTimeWeek.Text);
				}
			}
			if (wgTools.gWGYTJ && icOperator.OperatorID == 1)
			{
				wgAppConfig.setSystemParamValueBool(102, this.chkRecordDoorStatusEvent.Checked);
				wgAppConfig.setSystemParamValueBool(101, this.chkRecordDoorStatusEvent.Checked);
				wgAppConfig.setSystemParamValueBool(113, this.chkActivateOtherShiftSchedule.Checked);
				wgAppConfig.setSystemParamValueBool(121, this.chkActivateTimeProfile.Checked);
				wgAppConfig.setSystemParamValueBool(122, this.chkActivateRemoteOpenDoor.Checked);
				wgAppConfig.setSystemParamValueBool(123, this.chkActivateAccessKeypad.Checked);
			}
			string text = "0.0.0.0";
			if (!string.IsNullOrEmpty(this.cloudServerIP4RemoteControl.Text) && !this.cloudServerIP4RemoteControl.Text.Equals("0.0.0.0"))
			{
				text = this.cloudServerIP4RemoteControl.Text;
			}
			wgAppConfig.setSystemParamValue(205, "Cloud IP Remote Control By Client ", text, "2018-07-13 17:41:19 ");
			wgAppConfig.setSystemParamValue(206, "InvalidSwipeWarnNeedMoreSwipe", this.needMoreTimesText.Text, "2018-07-14 14:12:28 ");
			wgAppConfig.setSystemParamValue(207, "CapturePhotoDelaySeconds", this.capturePhotoDelayText.Text, "2018-07-20 16:13:06 ");
			if (XMessageBox.Show(CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				base.DialogResult = DialogResult.OK;
			}
			else
			{
				base.DialogResult = DialogResult.Cancel;
			}
			base.Close();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000414B0 File Offset: 0x000404B0
		private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoLoginMode", "1");
			this.updateAutoLoginMode();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000414C8 File Offset: 0x000404C8
		private void customXmlAAToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			bool flag = false;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = this.customXmlAAToolStripMenuItem.Text;
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				if (dfrmInputNewName.strNewName.ToUpper() == "778899LOGO")
				{
					flag = true;
				}
				else if (dfrmInputNewName.strNewName != "778899")
				{
					return;
				}
			}
			using (dfrmCreateCustomConfigure dfrmCreateCustomConfigure = new dfrmCreateCustomConfigure())
			{
				dfrmCreateCustomConfigure.onlyLogo = flag;
				dfrmCreateCustomConfigure.ShowDialog(this);
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00041598 File Offset: 0x00040598
		private void dateTimeDisplayStyleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.tabControl1.Visible = true;
			base.Size = new Size(this.button1.Location.X + 10, this.button1.Location.Y + 2 * this.cmdOK.Size.Height);
			try
			{
				wgAppConfig.GetKeyVal("Language");
				this.cboOnlyDate.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMD");
				this.cboDateWeek.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek");
				this.cboDateTime.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS");
				this.cboDateTimeWeek.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek");
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00041670 File Offset: 0x00040670
		private void delegateEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("DelegateEdit");
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0004167D File Offset: 0x0004067D
		private void delegateRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("DelegateRestore");
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0004168C File Offset: 0x0004068C
		private void departmentAndZoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.departmentAndZoneToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.wgLog(this.departmentAndZoneToolStripMenuItem.Text);
				this.Cursor = Cursors.WaitCursor;
				wgAppConfig.optimizeDepartment();
				wgAppConfig.optimizeZone();
				this.Cursor = Cursors.Default;
				XMessageBox.Show("OK");
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00041721 File Offset: 0x00040721
		private void dfrmOption_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x0600020A RID: 522 RVA: 0x00041760 File Offset: 0x00040760
		private void dfrmOption_Load(object sender, EventArgs e)
		{
			this.wait60SecondsWhenInputWrongPasswords5TimesToolStripMenuItem.Visible = false;
			this.hideCardNOToolStripMenuItem.Visible = false;
			this.chkHideLogin.Checked = wgAppConfig.GetKeyVal("HideGettingStartedWhenLogin") != "1";
			this.chkAutoLoginOnly.Checked = wgAppConfig.GetKeyVal("autologinName") != "";
			this.nBIoTDriverV886ToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(203);
			this.wait60SecondsWhenInputWrongPasswords5TimesToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(218);
			this.hideCardNOToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(219);
			this.hideRestoreDatabaseToolMenuItem.Checked = wgAppConfig.getParamValBoolByNO(224);
			this.checkDiskSpaceToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(204);
			this.softwareWarnAutoResetWhenAllDoorAreClosedToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(216);
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.softwareWarnAutoResetWhenAllDoorAreClosedToolStripMenuItem.Visible = false;
			}
			this.software_language_check();
			this.tabPage1.BackColor = this.BackColor;
			this.tabPage2.BackColor = this.BackColor;
			this.chkHouse.Checked = wgAppConfig.bFloorRoomManager;
			switch (this.pageIndex)
			{
			case 1:
				this.tabControl2.SelectedTab = this.tabPage3;
				break;
			case 2:
				this.tabControl2.SelectedTab = this.tabPage4;
				break;
			case 3:
				this.tabControl2.SelectedTab = this.tabPage5;
				break;
			case 4:
				this.tabControl2.SelectedTab = this.tabPage6;
				break;
			}
			if (icOperator.OperatorID == 1 && (wgAppConfig.GetKeyVal("AllowUploadUserName") == "1" || !string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(41)) || wgAppConfig.getParamValBoolByNO(147)))
			{
				this.btnOption.Visible = true;
			}
			this.cboDateTime.Items.Clear();
			this.cboDateTime.Items.AddRange(new string[]
			{
				"yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-M-d HH:mm:ss", "yy-M-d HH:mm:ss", "yy-MM-dd HH:mm:ss", "HH:mm:ss dd-MM-yy", "HH:mm:ss d/M/yyyy", "HH:mm:ss d/M/yy", "HH:mm:ss dd/MM/yy", "yy/M/d HH:mm:ss",
				"yy/MM/dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "HH:mm:ss M/d/yyyy", "HH:mm:ss M/d/yy", "HH:mm:ss MM/dd/yyyy"
			});
			this.cboDateTimeWeek.Items.Clear();
			this.cboDateTimeWeek.Items.AddRange(new string[]
			{
				"yyyy-MM-dd HH:mm:ss dddd", "yyyy-MM-dd HH:mm:ss ddd", "yyyy-M-d HH:mm:ss ddd", "yy-M-d HH:mm:ss ddd", "yy-MM-dd HH:mm:ss ddd", "HH:mm:ss dd-MM-yy ddd", "HH:mm:ss d/M/yyyy ddd", "HH:mm:ss d/M/yy ddd", "HH:mm:ss dd/MM/yy ddd", "yy/M/d HH:mm:ss ddd",
				"yy/MM/dd HH:mm:ss ddd", "yyyy/MM/dd HH:mm:ss ddd", "HH:mm:ss M/d/yyyy ddd", "HH:mm:ss M/d/yy ddd", "HH:mm:ss MM/dd/yyyy ddd"
			});
			this.cboDateWeek.Items.Clear();
			this.cboDateWeek.Items.AddRange(new string[]
			{
				"yyyy-MM-dd dddd", "yyyy-MM-dd ddd", "yyyy-M-d ddd", "yy-M-d ddd", "yy-MM-dd ddd", "dd-MM-yy ddd", "d/M/yyyy ddd", "d/M/yy ddd", "dd/MM/yy ddd", "yy/M/d ddd",
				"yy/MM/dd ddd", "yyyy/MM/dd ddd", "M/d/yyyy ddd", "M/d/yy ddd", "MM/dd/yyyy ddd"
			});
			this.cboOnlyDate.Items.Clear();
			this.cboOnlyDate.Items.AddRange(new string[]
			{
				"yyyy-MM-dd", "yyyy-M-d", "yy-M-d", "yy-MM-dd", "dd-MM-yy", "d/M/yyyy", "d/M/yy", "dd/MM/yy", "yy/M/d", "yy/MM/dd",
				"yyyy/MM/dd", "M/d/yyyy", "M/d/yy", "MM/dd/yyyy"
			});
			if (wgTools.gWGYTJ && icOperator.OperatorID == 1)
			{
				this.chkRecordDoorStatusEvent.Checked = wgAppConfig.getParamValBoolByNO(102);
				this.chkActivateOtherShiftSchedule.Checked = wgAppConfig.getParamValBoolByNO(113);
				this.chkActivateTimeProfile.Checked = wgAppConfig.getParamValBoolByNO(121);
				this.chkActivateRemoteOpenDoor.Checked = wgAppConfig.getParamValBoolByNO(122);
				this.chkActivateAccessKeypad.Checked = wgAppConfig.getParamValBoolByNO(123);
			}
			else
			{
				this.tabPage6.Parent = null;
			}
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.rentingHouseManagementToolStripMenuItem.Visible = false;
			}
			this.updateRecEventModeCheck();
			this.updateAutoLoginMode();
			this.updateRentingHouseMode();
			this.updateTCPServerConfig();
			this.updateTaskSchedule();
			string keyVal = wgAppConfig.GetKeyVal("KEY_EXCELMODE");
			if (string.IsNullOrEmpty(keyVal))
			{
				this.excelexportModeDefault.Checked = true;
			}
			else if (keyVal.Equals("0"))
			{
				this.excelexportMode0.Checked = true;
			}
			else if (keyVal.Equals("1"))
			{
				this.excelexportMode1.Checked = true;
			}
			else if (keyVal.Equals("2"))
			{
				this.excelexportMode2.Checked = true;
			}
			else
			{
				this.excelexportModeDefault.Checked = true;
			}
			this.twoCardCheckToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(200);
			this.faceIDCheckInputToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(201);
			this.cloudServerIP4RemoteControl.Text = wgAppConfig.getSystemParamByNO(205);
			this.needMoreTimesText.Text = wgAppConfig.getSystemParamByNO(206);
			this.capturePhotoDelayText.Text = wgAppConfig.getSystemParamByNO(207);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00041D7E File Offset: 0x00040D7E
		private void disableAutoUploadNewInformationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_TCPServerAutoUpload", "0");
			this.updateTCPServerConfig();
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00041D98 File Offset: 0x00040D98
		private void disabledAutoGetSwipeRecordsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", (sender as ToolStripMenuItem).Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.UpdateKeyVal("AutoGetSwipeRecords", "");
				wgAppConfig.wgLog(string.Format("Task schedule: {0}", (sender as ToolStripMenuItem).Text));
				this.updateTaskSchedule();
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00041E24 File Offset: 0x00040E24
		private void disabledAutoUploadConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", (sender as ToolStripMenuItem).Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.UpdateKeyVal("AutoUploadConfigure", "");
				wgAppConfig.wgLog(string.Format("Task schedule: {0}", (sender as ToolStripMenuItem).Text));
				this.updateTaskSchedule();
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00041EB0 File Offset: 0x00040EB0
		private void disabledAutoUploadPrivilegesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", (sender as ToolStripMenuItem).Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.UpdateKeyVal("AutoUploadPrivileges", "");
				wgAppConfig.wgLog(string.Format("Task schedule: {0}", (sender as ToolStripMenuItem).Text));
				this.updateTaskSchedule();
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00041F3C File Offset: 0x00040F3C
		private void disabledUpdateTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", (sender as ToolStripMenuItem).Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.UpdateKeyVal("AutoUpdateTime", "");
				wgAppConfig.wgLog(string.Format("Task schedule: {0}", (sender as ToolStripMenuItem).Text));
				this.updateTaskSchedule();
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00041FC8 File Offset: 0x00040FC8
		private void disableStartupWhenUserLoginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "WindowsStartupWG";
			string executablePath = Application.ExecutablePath;
			string text2 = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run";
			if (RegisterTool.DeleteValue(text2, text))
			{
				XMessageBox.Show(this.disableStartupWhenUserLoginToolStripMenuItem.Text + " OK");
				return;
			}
			XMessageBox.Show(this.disableStartupWhenUserLoginToolStripMenuItem.Text + " " + CommonStr.strFailed);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0004202C File Offset: 0x0004102C
		private void disableTCPServerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_TCPServerConfigActive", "0");
			this.updateTCPServerConfig();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00042044 File Offset: 0x00041044
		private void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.strNewName = wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO);
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.UpdateKeyVal("KEY_strReplaceWorkNO", dfrmInputNewName.strNewName);
				}
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000420A0 File Offset: 0x000410A0
		private void editToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.strNewName = wgAppConfig.ReplaceFloorRoom(CommonStr.strReplaceDepartment);
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.UpdateKeyVal("KEY_strReplaceDepartment", dfrmInputNewName.strNewName);
				}
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000420FC File Offset: 0x000410FC
		private void enableAutoUploadNewInformationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_TCPServerAutoUpload", "1");
			this.updateTCPServerConfig();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00042113 File Offset: 0x00041113
		private void enableTCPServerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_TCPServerConfigActive", "1");
			this.updateTCPServerConfig();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0004212A File Offset: 0x0004112A
		private void excelexportMode0_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_EXCELMODE", "0");
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0004213B File Offset: 0x0004113B
		private void excelexportMode1_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_EXCELMODE", "1");
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0004214C File Offset: 0x0004114C
		private void excelexportMode2_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_EXCELMODE", "2");
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0004215D File Offset: 0x0004115D
		private void excelexportModeDefault_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_EXCELMODE", "");
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00042170 File Offset: 0x00041170
		private void faceIDCheckInputToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(201))
			{
				wgAppConfig.setSystemParamValue(201, "0");
			}
			else
			{
				if (wgAppConfig.IsAccessDB)
				{
					XMessageBox.Show(CommonStr.strOnlySupportSqlServer2015);
					return;
				}
				try
				{
					string text = "CREATE TABLE [tRecognize](\r\n\t[id] [BIGINT] IDENTITY(1,1) NOT NULL,\r\n\t[success] [VARCHAR](50) NULL,\r\n\t[idCardCode] [VARCHAR](50) NULL,\r\n\t[realName] [VARCHAR](50) NULL,\r\n\t[sex] [VARCHAR](50) NULL,\r\n\t[birthday] [VARCHAR](50) NULL,\r\n\t[address] [VARCHAR](200) NULL,\r\n\t[phone] [VARCHAR](50) NULL,\r\n\t[bizType] [BIGINT] NULL,\r\n\t[recognizeTimeStr] [VARCHAR](50) NULL,\r\n\t[requestTimeStr] [VARCHAR](50) NULL,\r\n\t[recognizeImage] [IMAGE] NULL,\r\n\t[identifyImage] [IMAGE] NULL\r\n)";
					wgAppConfig.runSql(text);
				}
				catch
				{
				}
				wgAppConfig.setSystemParamValue(201, "ActivateFaceIDCheckInput", "1", "2018-05-10 07:53:57 ");
			}
			this.faceIDCheckInputToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(201);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00042200 File Offset: 0x00041200
		private void funcCtrlShiftQ()
		{
			this.rentingHouseManagementToolStripMenuItem.Visible = true;
			this.setMaxThreadNumToolStripMenuItem.Visible = true;
			this.queryAutosLogToolStripMenuItem.Visible = true;
			if (!this.btnOption.Visible)
			{
				this.btnOption.Visible = true;
				return;
			}
			this.tabControl1.Visible = true;
			base.Size = new Size(this.button1.Location.X + 10, this.button1.Location.Y + 2 * this.cmdOK.Size.Height);
			try
			{
				wgAppConfig.GetKeyVal("Language");
				this.cboOnlyDate.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMD");
				this.cboDateWeek.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek");
				this.cboDateTime.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS");
				this.cboDateTimeWeek.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek");
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00042318 File Offset: 0x00041318
		private void gB3212EncodingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_TCPServerConfigEncoding", "GB2312");
			this.updateTCPServerConfig();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00042330 File Offset: 0x00041330
		private void hideCardNOToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(219))
			{
				wgAppConfig.setSystemParamValue(219, "0");
			}
			else
			{
				wgAppConfig.setSystemParamValue(219, "ActivateHideCardNO", "1", "2019-09-03 11:25:12");
			}
			this.hideCardNOToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(219);
			wgTools.gbHideCardNO = wgAppConfig.getParamValBoolByNO(219);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0004239C File Offset: 0x0004139C
		private void hideRestoreDatabaseToolMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(224))
			{
				wgAppConfig.setSystemParamValue(224, "0");
			}
			else
			{
				wgAppConfig.setSystemParamValue(224, "Activate_HIDE_RESTOREDATABASE", "1", "2019-11-14 22:59:45");
			}
			this.hideRestoreDatabaseToolMenuItem.Checked = wgAppConfig.getParamValBoolByNO(224);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000423F6 File Offset: 0x000413F6
		private void invitationalEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("InvitationalEdit");
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00042403 File Offset: 0x00041403
		private void invitationalRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("InvitationalRestore");
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00042410 File Offset: 0x00041410
		private void noneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("logRecEventMode", "0");
			this.updateRecEventModeCheck();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00042427 File Offset: 0x00041427
		private void allRecordsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("logRecEventMode", "1");
			this.updateRecEventModeCheck();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0004243E File Offset: 0x0004143E
		private void swipeRecordsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("logRecEventMode", "2");
			this.updateRecEventModeCheck();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00042455 File Offset: 0x00041455
		private void nonSwipeRecordsegWarnPushButtonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("logRecEventMode", "3");
			this.updateRecEventModeCheck();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0004246C File Offset: 0x0004146C
		private void swipeRecordsn3krec1logAndNonSwipeN3krec2logToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("logRecEventMode", "4");
			this.updateRecEventModeCheck();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00042483 File Offset: 0x00041483
		private void logRecCommFailAndWarnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.logRecCommFailAndWarnToolStripMenuItem.Checked)
			{
				wgAppConfig.UpdateKeyVal("logRecCommFail", "0");
			}
			else
			{
				wgAppConfig.UpdateKeyVal("logRecCommFail", "2");
			}
			this.updateRecEventModeCheck();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000424B8 File Offset: 0x000414B8
		private void logRecCommFailToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.logRecCommFailToolStripMenuItem.Checked)
			{
				wgAppConfig.UpdateKeyVal("logRecCommFail", "0");
			}
			else
			{
				wgAppConfig.UpdateKeyVal("logRecCommFail", "1");
			}
			this.updateRecEventModeCheck();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000424F0 File Offset: 0x000414F0
		private void updateRecEventModeCheck()
		{
			this.allRecordsToolStripMenuItem.Checked = false;
			this.swipeRecordsToolStripMenuItem.Checked = false;
			this.nonSwipeRecordsegWarnPushButtonToolStripMenuItem.Checked = false;
			this.noneToolStripMenuItem.Checked = false;
			this.swipeRecordsn3krec1logAndNonSwipeN3krec2logToolStripMenuItem.Checked = false;
			string keyVal = wgAppConfig.GetKeyVal("logRecEventMode");
			if (keyVal != null)
			{
				if (!(keyVal == "1"))
				{
					if (!(keyVal == "2"))
					{
						if (!(keyVal == "3"))
						{
							if (!(keyVal == "4"))
							{
								this.noneToolStripMenuItem.Checked = true;
							}
							else
							{
								this.swipeRecordsn3krec1logAndNonSwipeN3krec2logToolStripMenuItem.Checked = true;
							}
						}
						else
						{
							this.nonSwipeRecordsegWarnPushButtonToolStripMenuItem.Checked = true;
						}
					}
					else
					{
						this.swipeRecordsToolStripMenuItem.Checked = true;
					}
				}
				else
				{
					this.allRecordsToolStripMenuItem.Checked = true;
				}
			}
			else
			{
				this.noneToolStripMenuItem.Checked = true;
			}
			this.logRecCommFailToolStripMenuItem.Checked = false;
			this.logRecCommFailToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("logRecCommFail") == "1";
			this.logRecCommFailAndWarnToolStripMenuItem.Checked = false;
			this.logRecCommFailAndWarnToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("logRecCommFail") == "2";
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00042623 File Offset: 0x00041623
		private void meetingSignEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("MeetingSignEdit");
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00042630 File Offset: 0x00041630
		private void meetingSignRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("MeetingSignRestore");
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0004263D File Offset: 0x0004163D
		private void minimizeAfterAutologinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.GetKeyVal("AutoLoginMinimize").Equals("1"))
			{
				wgAppConfig.UpdateKeyVal("AutoLoginMinimize", "0");
			}
			else
			{
				wgAppConfig.UpdateKeyVal("AutoLoginMinimize", "1");
			}
			this.updateAutoLoginMode();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0004267C File Offset: 0x0004167C
		private void nBIoTDriverV886ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(203))
			{
				wgAppConfig.setSystemParamValue(203, "0");
			}
			else
			{
				wgAppConfig.setSystemParamValue(203, "ActivateNBIOT_P64", "1", "2018-06-10 10:22:11");
			}
			this.nBIoTDriverV886ToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(203);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000426D6 File Offset: 0x000416D6
		private void nonvotingDelegateEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("NonvotingDelegateEdit");
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000426E3 File Offset: 0x000416E3
		private void nonvotingDelegateRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("NonvotingDelegateRestore");
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000426F0 File Offset: 0x000416F0
		private void otherToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000426F4 File Offset: 0x000416F4
		private void queryAutosLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = wgAppConfig.Path4Doc() + "n3k_autorun.log",
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00042778 File Offset: 0x00041778
		private void realtimeGetRecordsAndLockInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoLoginMode", "4");
			this.updateAutoLoginMode();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0004278F File Offset: 0x0004178F
		private void realtimeGetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoLoginMode", "3");
			this.updateAutoLoginMode();
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000427A6 File Offset: 0x000417A6
		private void realtimeMonitoerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoLoginMode", "2");
			this.updateAutoLoginMode();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000427BD File Offset: 0x000417BD
		private void recordWarn24HourEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("RecordWarn24HourEdit");
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000427CA File Offset: 0x000417CA
		private void recordWarn24HourRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("RecordWarn24HourRestore");
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000427D7 File Offset: 0x000417D7
		private void recordWarnEmergencyCallEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("RecordWarnEmergencyCallEdit");
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000427E4 File Offset: 0x000417E4
		private void recordWarnEmergencyCallRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("RecordWarnEmergencyCallRestore");
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000427F1 File Offset: 0x000417F1
		private void recordWarnGuardAgainstTheftEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("RecordWarnGuardAgainstTheftEdit");
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000427FE File Offset: 0x000417FE
		private void recordWarnGuardAgainstTheftRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("RecordWarnGuardAgainstTheftRestore");
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0004280B File Offset: 0x0004180B
		private void remoteOpenDoorEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("RemoteOpenDoor");
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00042818 File Offset: 0x00041818
		private void remoteOpenDoorRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("RemoteOpenDoor");
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00042825 File Offset: 0x00041825
		private void rentingHouseDisableToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.setSystemParamValue(178, "Activate Renting House", "0", "2015-05-28 13:28:01");
			this.updateRentingHouseMode();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00042847 File Offset: 0x00041847
		private void rentingHouseEnabledToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.setSystemParamValue(178, "Activate Renting House", "1", "2015-05-28 13:28:01");
			this.updateRentingHouseMode();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00042869 File Offset: 0x00041869
		private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_strReplaceWorkNO", "");
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0004287A File Offset: 0x0004187A
		private void restoreToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_strReplaceDepartment", "");
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0004288C File Offset: 0x0004188C
		private void setCustomTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmDateTimeSelect dfrmDateTimeSelect = new dfrmDateTimeSelect())
			{
				DateTime now = DateTime.Now;
				dfrmDateTimeSelect.timeInfo = wgAppConfig.GetKeyVal("AutoUpdateTime");
				dfrmDateTimeSelect.dateBeginHMS1.Visible = true;
				dfrmDateTimeSelect.Text = (sender as ToolStripMenuItem).Text;
				if (dfrmDateTimeSelect.Text.IndexOf("[") > 0)
				{
					dfrmDateTimeSelect.Text = dfrmDateTimeSelect.Text.Substring(0, dfrmDateTimeSelect.Text.IndexOf("["));
				}
				dfrmDateTimeSelect.taskSheuleCommand = "AutoUpdateTime";
				if (dfrmDateTimeSelect.ShowDialog() == DialogResult.OK)
				{
					this.checkValidAutoTime("AutoUpdateTime", dfrmDateTimeSelect.timeInfo);
				}
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00042968 File Offset: 0x00041968
		private void setMaxThreadNumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, sender.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				int num = 10;
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.Text = (sender as ToolStripItem).Text;
					dfrmInputNewName.strNewName = "10";
					int num2 = 0;
					int.TryParse(wgAppConfig.GetKeyVal("KEY_MaxThreadNum"), out num2);
					if (num2 > 0)
					{
						dfrmInputNewName.strNewName = num2.ToString();
					}
					if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
					{
						if (string.IsNullOrEmpty(dfrmInputNewName.strNewName))
						{
							wgAppConfig.UpdateKeyVal("KEY_MaxThreadNum", "");
						}
						else
						{
							bool flag = true;
							if (!int.TryParse(dfrmInputNewName.strNewName.Trim(), out num))
							{
								flag = false;
							}
							if (num < 1)
							{
								num = 1;
							}
							if (!flag)
							{
								XMessageBox.Show(this, CommonStr.strInvalidValue, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							}
							else
							{
								wgAppConfig.UpdateKeyVal("KEY_MaxThreadNum", num.ToString());
							}
						}
					}
				}
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00042A70 File Offset: 0x00041A70
		private void setTimeToAutoGetSwipeRecordsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmDateTimeSelect dfrmDateTimeSelect = new dfrmDateTimeSelect())
			{
				DateTime now = DateTime.Now;
				dfrmDateTimeSelect.timeInfo = wgAppConfig.GetKeyVal("AutoGetSwipeRecords");
				dfrmDateTimeSelect.dateBeginHMS1.Visible = true;
				dfrmDateTimeSelect.Text = (sender as ToolStripMenuItem).Text;
				if (dfrmDateTimeSelect.Text.IndexOf("[") > 0)
				{
					dfrmDateTimeSelect.Text = dfrmDateTimeSelect.Text.Substring(0, dfrmDateTimeSelect.Text.IndexOf("["));
				}
				dfrmDateTimeSelect.taskSheuleCommand = "AutoGetSwipeRecords";
				if (dfrmDateTimeSelect.ShowDialog() == DialogResult.OK)
				{
					this.checkValidAutoTime("AutoGetSwipeRecords", dfrmDateTimeSelect.timeInfo);
				}
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00042B4C File Offset: 0x00041B4C
		private void setTimeToAutoUploadConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.label1.Text = CommonStr.strInputExtendFunctionPassword;
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			using (dfrmDateTimeSelect dfrmDateTimeSelect = new dfrmDateTimeSelect())
			{
				DateTime now = DateTime.Now;
				dfrmDateTimeSelect.timeInfo = wgAppConfig.GetKeyVal("AutoUploadConfigure");
				dfrmDateTimeSelect.dateBeginHMS1.Visible = true;
				dfrmDateTimeSelect.Text = (sender as ToolStripMenuItem).Text;
				if (dfrmDateTimeSelect.Text.IndexOf("[") > 0)
				{
					dfrmDateTimeSelect.Text = dfrmDateTimeSelect.Text.Substring(0, dfrmDateTimeSelect.Text.IndexOf("["));
				}
				dfrmDateTimeSelect.taskSheuleCommand = "AutoUploadConfigure";
				if (dfrmDateTimeSelect.ShowDialog() == DialogResult.OK)
				{
					this.checkValidAutoTime("AutoUploadConfigure", dfrmDateTimeSelect.timeInfo);
				}
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00042C80 File Offset: 0x00041C80
		private void setTimeToAutoUploadPrivilegesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.label1.Text = CommonStr.strInputExtendFunctionPassword;
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			using (dfrmDateTimeSelect dfrmDateTimeSelect = new dfrmDateTimeSelect())
			{
				DateTime now = DateTime.Now;
				dfrmDateTimeSelect.timeInfo = wgAppConfig.GetKeyVal("AutoUploadPrivileges");
				dfrmDateTimeSelect.dateBeginHMS1.Visible = true;
				dfrmDateTimeSelect.Text = (sender as ToolStripMenuItem).Text;
				if (dfrmDateTimeSelect.Text.IndexOf("[") > 0)
				{
					dfrmDateTimeSelect.Text = dfrmDateTimeSelect.Text.Substring(0, dfrmDateTimeSelect.Text.IndexOf("["));
				}
				dfrmDateTimeSelect.taskSheuleCommand = "AutoUploadPrivileges";
				if (dfrmDateTimeSelect.ShowDialog() == DialogResult.OK)
				{
					this.checkValidAutoTime("AutoUploadPrivileges", dfrmDateTimeSelect.timeInfo);
				}
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00042DB4 File Offset: 0x00041DB4
		private void software_language_check()
		{
			this.cboLanguage.Items.Clear();
			this.cboLanguage.Items.Add("English");
			this.cboLanguage.SelectedIndex = 0;
			this.cboLanguage.Items.Add("简体中文[zh-CHS]");
			this.cboLanguage.Items.Add("繁體中文[zh-CHT]");
			if (wgAppConfig.GetKeyVal("Language") == "zh-CHS")
			{
				this.cboLanguage.SelectedIndex = 1;
			}
			if (wgAppConfig.GetKeyVal("Language") == "zh-CHT")
			{
				this.cboLanguage.SelectedIndex = 2;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath);
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
				{
					if (fileInfo.Name == "N3000.resources.dll")
					{
						wgTools.WriteLine(fileInfo.FullName);
						if (directoryInfo2.Name != "zh-CHS" && directoryInfo2.Name != "zh-CHT")
						{
							this.cboLanguage.Items.Add(directoryInfo2.Name);
							if (wgAppConfig.GetKeyVal("Language") == directoryInfo2.Name)
							{
								this.cboLanguage.SelectedIndex = this.cboLanguage.Items.Count - 1;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00042F48 File Offset: 0x00041F48
		private void softwareWarnAutoResetWhenAllDoorAreClosedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(216))
			{
				wgAppConfig.setSystemParamValue(216, "0");
			}
			else
			{
				wgAppConfig.setSystemParamValue(216, "ActivateSoftwareWarnAutoResetWhenAllDoorAreClosed", "1", "2019-04-17 17:01:35");
			}
			this.softwareWarnAutoResetWhenAllDoorAreClosedToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(216);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00042FA4 File Offset: 0x00041FA4
		private void specialWordEditToolStripMenuItem_Click(string sender)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				string text;
				switch (text = sender.ToString())
				{
				case "RecordWarnGuardAgainstTheftEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strRecordWarnGuardAgainstTheft");
					break;
				case "RecordWarn24HourEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strRecordWarn24Hour");
					break;
				case "RecordWarnEmergencyCallEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strRecordWarnEmergencyCall");
					break;
				case "DelegateEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strDelegate");
					break;
				case "NonvotingDelegateEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strNonvotingDelegate");
					break;
				case "InvitationalEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strInvitational");
					break;
				case "AuditEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strAudit");
					break;
				case "MeetingSignEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strMeetingSign");
					break;
				case "totalUsersIndoorsEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strtotalUsersIndoors");
					break;
				case "AbsentEdit":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strAbsent");
					break;
				case "RemoteOpenDoor":
					dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_strRemoteOpenDoor");
					break;
				}
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					string text2;
					switch (text2 = sender.ToString())
					{
					case "RecordWarnGuardAgainstTheftEdit":
						wgAppConfig.UpdateKeyVal("KEY_strRecordWarnGuardAgainstTheft", dfrmInputNewName.strNewName);
						break;
					case "RecordWarn24HourEdit":
						wgAppConfig.UpdateKeyVal("KEY_strRecordWarn24Hour", dfrmInputNewName.strNewName);
						break;
					case "RecordWarnEmergencyCallEdit":
						wgAppConfig.UpdateKeyVal("KEY_strRecordWarnEmergencyCall", dfrmInputNewName.strNewName);
						break;
					case "DelegateEdit":
						wgAppConfig.UpdateKeyVal("KEY_strDelegate", dfrmInputNewName.strNewName);
						break;
					case "NonvotingDelegateEdit":
						wgAppConfig.UpdateKeyVal("KEY_strNonvotingDelegate", dfrmInputNewName.strNewName);
						break;
					case "InvitationalEdit":
						wgAppConfig.UpdateKeyVal("KEY_strInvitational", dfrmInputNewName.strNewName);
						break;
					case "AuditEdit":
						wgAppConfig.UpdateKeyVal("KEY_strAudit", dfrmInputNewName.strNewName);
						break;
					case "MeetingSignEdit":
						wgAppConfig.UpdateKeyVal("KEY_strMeetingSign", dfrmInputNewName.strNewName);
						break;
					case "totalUsersIndoorsEdit":
						wgAppConfig.UpdateKeyVal("KEY_strtotalUsersIndoors", dfrmInputNewName.strNewName);
						break;
					case "AbsentEdit":
						wgAppConfig.UpdateKeyVal("KEY_strAbsent", dfrmInputNewName.strNewName);
						break;
					case "RemoteOpenDoor":
						wgAppConfig.UpdateKeyVal("KEY_strRemoteOpenDoor", dfrmInputNewName.strNewName);
						break;
					}
					wgTools.gReaderBrokenWarnActive = ((wgAppConfig.GetKeyVal("KEY_strRecordWarnEmergencyCall").IndexOf("防拆") >= 0) ? 1 : 0);
				}
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00043398 File Offset: 0x00042398
		private void specialWordRestoreToolStripMenuItem_Click(string sender)
		{
			string text;
			switch (text = sender.ToString())
			{
			case "RecordWarnGuardAgainstTheftRestore":
				wgAppConfig.UpdateKeyVal("KEY_strRecordWarnGuardAgainstTheft", "");
				break;
			case "RecordWarn24HourRestore":
				wgAppConfig.UpdateKeyVal("KEY_strRecordWarn24Hour", "");
				break;
			case "RecordWarnEmergencyCallRestore":
				wgAppConfig.UpdateKeyVal("KEY_strRecordWarnEmergencyCall", "");
				break;
			case "DelegateResotre":
				wgAppConfig.UpdateKeyVal("KEY_strDelegate", "");
				break;
			case "NonvotingDelegateResotre":
				wgAppConfig.UpdateKeyVal("KEY_strNonvotingDelegate", "");
				break;
			case "InvitationalResotre":
				wgAppConfig.UpdateKeyVal("KEY_strInvitational", "");
				break;
			case "AuditResotre":
				wgAppConfig.UpdateKeyVal("KEY_strAudit", "");
				break;
			case "MeetingSignResotre":
				wgAppConfig.UpdateKeyVal("KEY_strMeetingSign", "");
				break;
			case "totalUsersIndoorsRestore":
				wgAppConfig.UpdateKeyVal("KEY_strtotalUsersIndoors", "");
				break;
			case "AbsentRestore":
				wgAppConfig.UpdateKeyVal("KEY_strAbsent", "");
				break;
			case "RemoteOpenDoor":
				wgAppConfig.UpdateKeyVal("KEY_strRemoteOpenDoor", "");
				break;
			}
			wgTools.gReaderBrokenWarnActive = ((wgAppConfig.GetKeyVal("KEY_strRecordWarnEmergencyCall").IndexOf("防拆") >= 0) ? 1 : 0);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0004357F File Offset: 0x0004257F
		private void tcpPorttoolStripTextBox1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00043584 File Offset: 0x00042584
		private void tcpPorttoolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
		{
			int num = 60006;
			if (int.TryParse(this.tcpPorttoolStripTextBox1.Text, out num) && num > 0 && num < 65535)
			{
				wgAppConfig.UpdateKeyVal("KEY_TCPServerConfigPort", num.ToString());
			}
			this.updateTCPServerConfig();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000435CE File Offset: 0x000425CE
		private void toolStripComboBox1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000435D0 File Offset: 0x000425D0
		private void totalUsersIndoorsEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordEditToolStripMenuItem_Click("totalUsersIndoorsEdit");
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000435DD File Offset: 0x000425DD
		private void totalUsersIndoorsRestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.specialWordRestoreToolStripMenuItem_Click("totalUsersIndoorsRestore");
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000435EC File Offset: 0x000425EC
		private void twoCardCheckToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(200))
			{
				wgAppConfig.setSystemParamValue(200, "0");
			}
			else
			{
				wgAppConfig.setSystemParamValue(200, "ActivateTwoCardCheck", "1", "2018-05-09 23:44:10");
			}
			this.twoCardCheckToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(200);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00043648 File Offset: 0x00042648
		private void updateAutoLoginMode()
		{
			this.realtimeMonitoerToolStripMenuItem.Checked = false;
			this.realtimeGetToolStripMenuItem.Checked = false;
			this.consoleToolStripMenuItem.Checked = false;
			this.realtimeGetRecordsAndLockInterfaceToolStripMenuItem.Checked = false;
			this.watchingLCDToolStripMenuItem.Checked = false;
			this.watchingLEDToolStripMenuItem.Checked = false;
			if (wgAppConfig.GetKeyVal("AutoLoginMode") == "2")
			{
				this.realtimeMonitoerToolStripMenuItem.Checked = true;
			}
			else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "3")
			{
				this.realtimeGetToolStripMenuItem.Checked = true;
			}
			else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "4")
			{
				this.realtimeGetRecordsAndLockInterfaceToolStripMenuItem.Checked = true;
			}
			else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "5")
			{
				this.watchingLCDToolStripMenuItem.Checked = true;
			}
			else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "6")
			{
				this.watchingLEDToolStripMenuItem.Checked = true;
			}
			else
			{
				this.consoleToolStripMenuItem.Checked = true;
			}
			if (wgAppConfig.GetKeyVal("AutoRestartWhenNetFailed").Equals("1"))
			{
				this.autoRestartWhenNetFailedDisableDefaultToolStripMenuItem.Checked = false;
				this.autoRestartWhenNetFailedEnabledToolStripMenuItem.Checked = true;
			}
			else
			{
				this.autoRestartWhenNetFailedDisableDefaultToolStripMenuItem.Checked = true;
				this.autoRestartWhenNetFailedEnabledToolStripMenuItem.Checked = false;
			}
			this.minimizeAfterAutologinToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("AutoLoginMinimize").Equals("1");
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000437C7 File Offset: 0x000427C7
		private void updateRentingHouseMode()
		{
			this.rentingHouseDisableToolStripMenuItem.Checked = false;
			this.rentingHouseEnabledToolStripMenuItem.Checked = false;
			if (wgAppConfig.getParamValBoolByNO(178))
			{
				this.rentingHouseEnabledToolStripMenuItem.Checked = true;
				return;
			}
			this.rentingHouseDisableToolStripMenuItem.Checked = true;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00043808 File Offset: 0x00042808
		private void updateTaskSchedule()
		{
			try
			{
				DateTime now = DateTime.Now;
				string text = wgAppConfig.GetKeyVal("AutoUpdateTime");
				this.disabledUpdateTimeToolStripMenuItem.Checked = true;
				this.setCustomTimeToolStripMenuItem.Checked = false;
				if (this.setCustomTimeToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.setCustomTimeToolStripMenuItem.Text = this.setCustomTimeToolStripMenuItem.Text.Substring(0, this.setCustomTimeToolStripMenuItem.Text.IndexOf("["));
				}
				if (this.autoUpdateTimeToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.autoUpdateTimeToolStripMenuItem.Text = this.autoUpdateTimeToolStripMenuItem.Text.Substring(0, this.autoUpdateTimeToolStripMenuItem.Text.IndexOf("["));
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.disabledUpdateTimeToolStripMenuItem.Checked = false;
					this.setCustomTimeToolStripMenuItem.Checked = true;
					this.setCustomTimeToolStripMenuItem.Text = this.setCustomTimeToolStripMenuItem.Text + string.Format("[{0}]", text);
					this.autoUpdateTimeToolStripMenuItem.Text = this.autoUpdateTimeToolStripMenuItem.Text + string.Format("[{0}]", text);
				}
				text = wgAppConfig.GetKeyVal("AutoGetSwipeRecords");
				this.disabledAutoGetSwipeRecordsToolStripMenuItem.Checked = true;
				this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Checked = false;
				if (this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Text = this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Text.Substring(0, this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Text.IndexOf("["));
				}
				if (this.autoGetSwipeRecordsToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.autoGetSwipeRecordsToolStripMenuItem.Text = this.autoGetSwipeRecordsToolStripMenuItem.Text.Substring(0, this.autoGetSwipeRecordsToolStripMenuItem.Text.IndexOf("["));
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.disabledAutoGetSwipeRecordsToolStripMenuItem.Checked = false;
					this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Checked = true;
					this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Text = this.setTimeToAutoGetSwipeRecordsToolStripMenuItem.Text + string.Format("[{0}]", text);
					this.autoGetSwipeRecordsToolStripMenuItem.Text = this.autoGetSwipeRecordsToolStripMenuItem.Text + string.Format("[{0}]", text);
				}
				text = wgAppConfig.GetKeyVal("AutoUploadPrivileges");
				this.disabledAutoUploadPrivilegesToolStripMenuItem.Checked = true;
				this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Checked = false;
				if (this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Text = this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Text.Substring(0, this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Text.IndexOf("["));
				}
				if (this.autoUploadPrivilegesToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.autoUploadPrivilegesToolStripMenuItem.Text = this.autoUploadPrivilegesToolStripMenuItem.Text.Substring(0, this.autoUploadPrivilegesToolStripMenuItem.Text.IndexOf("["));
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.disabledAutoUploadPrivilegesToolStripMenuItem.Checked = false;
					this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Checked = true;
					this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Text = this.setTimeToAutoUploadPrivilegesToolStripMenuItem.Text + string.Format("[{0}]", text);
					this.autoUploadPrivilegesToolStripMenuItem.Text = this.autoUploadPrivilegesToolStripMenuItem.Text + string.Format("[{0}]", text);
				}
				text = wgAppConfig.GetKeyVal("AutoUploadConfigure");
				this.disabledAutoUploadConfigureToolStripMenuItem.Checked = true;
				this.setTimeToAutoUploadConfigureToolStripMenuItem.Checked = false;
				if (this.setTimeToAutoUploadConfigureToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.setTimeToAutoUploadConfigureToolStripMenuItem.Text = this.setTimeToAutoUploadConfigureToolStripMenuItem.Text.Substring(0, this.setTimeToAutoUploadConfigureToolStripMenuItem.Text.IndexOf("["));
				}
				if (this.autoUploadConfigureToolStripMenuItem.Text.IndexOf("[") > 0)
				{
					this.autoUploadConfigureToolStripMenuItem.Text = this.autoUploadConfigureToolStripMenuItem.Text.Substring(0, this.autoUploadConfigureToolStripMenuItem.Text.IndexOf("["));
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.disabledAutoUploadConfigureToolStripMenuItem.Checked = false;
					this.setTimeToAutoUploadConfigureToolStripMenuItem.Checked = true;
					this.setTimeToAutoUploadConfigureToolStripMenuItem.Text = this.setTimeToAutoUploadConfigureToolStripMenuItem.Text + string.Format("[{0}]", text);
					this.autoUploadConfigureToolStripMenuItem.Text = this.autoUploadConfigureToolStripMenuItem.Text + string.Format("[{0}]", text);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00043CCC File Offset: 0x00042CCC
		private void updateTCPServerConfig()
		{
			this.enableTCPServerToolStripMenuItem.Checked = false;
			this.disableTCPServerToolStripMenuItem.Checked = false;
			this.GB2312EncodingToolStripMenuItem.Checked = false;
			this.uTF8EncodingDefaultToolStripMenuItem.Checked = false;
			this.enableAutoUploadNewInformationToolStripMenuItem.Checked = false;
			this.disableAutoUploadNewInformationToolStripMenuItem.Checked = false;
			this.tcpPorttoolStripTextBox1.Text = "60006";
			if (wgAppConfig.GetKeyVal("KEY_TCPServerConfigActive") == "1")
			{
				this.enableTCPServerToolStripMenuItem.Checked = true;
			}
			else
			{
				this.disableTCPServerToolStripMenuItem.Checked = true;
			}
			if (wgAppConfig.GetKeyVal("KEY_TCPServerConfigEncoding") == "UTF-8")
			{
				this.uTF8EncodingDefaultToolStripMenuItem.Checked = true;
			}
			else
			{
				this.GB2312EncodingToolStripMenuItem.Checked = true;
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_TCPServerConfigPort")))
			{
				this.tcpPorttoolStripTextBox1.Text = wgAppConfig.GetKeyVal("KEY_TCPServerConfigPort");
			}
			if (wgAppConfig.GetKeyVal("KEY_TCPServerAutoUpload") == "0")
			{
				this.disableAutoUploadNewInformationToolStripMenuItem.Checked = true;
				return;
			}
			this.enableAutoUploadNewInformationToolStripMenuItem.Checked = true;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00043DE6 File Offset: 0x00042DE6
		private void uTF8EncodingDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_TCPServerConfigEncoding", "UTF-8");
			this.updateTCPServerConfig();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00043E00 File Offset: 0x00042E00
		private void wait60SecondsWhenInputWrongPasswords5TimesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(218))
			{
				wgAppConfig.setSystemParamValue(218, "0");
			}
			else
			{
				wgAppConfig.setSystemParamValue(218, "Activatewait60Seconds", "1", "2019-09-02 22:49:56");
			}
			this.wait60SecondsWhenInputWrongPasswords5TimesToolStripMenuItem.Checked = wgAppConfig.getParamValBoolByNO(218);
			wgTools.gbInputKeyPasswordControl = wgAppConfig.getParamValBoolByNO(218);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00043E69 File Offset: 0x00042E69
		private void watchingLCDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoLoginMode", "5");
			this.updateAutoLoginMode();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00043E80 File Offset: 0x00042E80
		private void watchingLEDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("AutoLoginMode", "6");
			this.updateAutoLoginMode();
		}

		// Token: 0x04000419 RID: 1049
		public int pageIndex;
	}
}
