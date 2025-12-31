using System;
using System.Collections;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic.MultiThread;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc;
using WG3000_COMM.ExtendFunc.CameraWatch;
using WG3000_COMM.ExtendFunc.Elevator;
using WG3000_COMM.ExtendFunc.FaceReader;
using WG3000_COMM.ExtendFunc.Finger;
using WG3000_COMM.ExtendFunc.GlobalAntiBack2015;
using WG3000_COMM.ExtendFunc.Meal;
using WG3000_COMM.ExtendFunc.Meeting;
using WG3000_COMM.ExtendFunc.Patrol;
using WG3000_COMM.ExtendFunc.PCCheck;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200003E RID: 62
	public partial class frmADCT3000 : Form
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x00075214 File Offset: 0x00074214
		public frmADCT3000()
		{
			string[,] array = new string[8, 3];
			array[0, 0] = "考勤报表";
			array[0, 1] = "mnuAttendenceData";
			array[0, 2] = "Reports.Shift.frmShiftAttReport";
			array[1, 0] = "正常班设置";
			array[1, 1] = "mnuShiftNormalConfigure";
			array[1, 2] = "Reports.Shift.dfrmShiftNormalParamSet";
			array[2, 0] = "倒班设置";
			array[2, 1] = "mnuShiftRule";
			array[2, 2] = "Reports.Shift.dfrmShiftOtherParamSet";
			array[3, 0] = "倒班班次";
			array[3, 1] = "mnuShiftSet";
			array[3, 2] = "Reports.Shift.frmShiftOtherTypes";
			array[4, 0] = "倒班排班";
			array[4, 1] = "mnuShiftArrange";
			array[4, 2] = "Reports.Shift.frmShiftOtherData";
			array[5, 0] = "正常班节假日";
			array[5, 1] = "mnuHolidaySet";
			array[5, 2] = "Reports.Shift.dfrmHolidaySet";
			array[6, 0] = "请假出差";
			array[6, 1] = "mnuLeave";
			array[6, 2] = "Reports.Shift.frmLeave";
			array[7, 0] = "签到";
			array[7, 1] = "mnuManualCardRecord";
			array[7, 2] = "Reports.Shift.frmManualSwipeRecords";
			this.functionNameAttendence = array;
			string[,] array2 = new string[12, 3];
			array2[0, 0] = "控制器";
			array2[0, 1] = "mnuControllers";
			array2[0, 2] = "Basic.frmControllers";
			array2[1, 0] = "部门班组";
			array2[1, 1] = "mnuGroups";
			array2[1, 2] = "Basic.frmDepartments";
			array2[2, 0] = "用户";
			array2[2, 1] = "mnuConsumers";
			array2[2, 2] = "Basic.frmUsers";
			array2[3, 0] = "权限";
			array2[3, 1] = "mnuPrivilege";
			array2[3, 2] = "Basic.frmPrivileges";
			array2[4, 0] = "时段";
			array2[4, 1] = "mnuControlSeg";
			array2[4, 2] = "Basic.frmControlSegs";
			array2[5, 0] = "报警.消防.防盗.联动";
			array2[5, 1] = "mnuPeripheral";
			array2[5, 2] = "ExtendFunc.dfrmControllerWarnSet";
			array2[6, 0] = "密码管理";
			array2[6, 1] = "mnuPasswordManagement";
			array2[6, 2] = "ExtendFunc.dfrmControllerExtendFuncPasswordManage";
			array2[7, 0] = "反潜回";
			array2[7, 1] = "mnuAntiBack";
			array2[7, 2] = "ExtendFunc.dfrmControllerAntiPassback";
			array2[8, 0] = "多门互锁";
			array2[8, 1] = "mnuInterLock";
			array2[8, 2] = "ExtendFunc.dfrmControllerInterLock";
			array2[9, 0] = "多卡开门";
			array2[9, 1] = "mnuMoreCards";
			array2[9, 2] = "ExtendFunc.dfrmControllerMultiCards";
			array2[10, 0] = "首卡开门";
			array2[10, 1] = "mnuFirstCard";
			array2[10, 2] = "ExtendFunc.dfrmControllerFirstCard";
			array2[11, 0] = "定时任务";
			array2[11, 1] = "mnuTaskList";
			array2[11, 2] = "ExtendFunc.dfrmControllerTaskList";
			this.functionNameBasicConfigure = array2;
			string[,] array3 = new string[2, 3];
			array3[0, 0] = "总控制台";
			array3[0, 1] = "mnuTotalControl";
			array3[0, 2] = "Basic.frmConsole";
			array3[1, 0] = "查询原始记录";
			array3[1, 1] = "mnuCardRecords";
			array3[1, 2] = "Basic.frmSwipeRecords";
			this.functionNameBasicOperate = array3;
			string[,] array4 = new string[14, 3];
			array4[0, 0] = "工具";
			array4[0, 1] = "mnu1Tool";
			array4[0, 2] = "";
			array4[1, 0] = "修改密码";
			array4[1, 1] = "cmdChangePasswor";
			array4[1, 2] = "Basic.dfrmSetPassword";
			array4[2, 0] = "操作员管理";
			array4[2, 1] = "cmdOperatorManage";
			array4[2, 2] = "Basic.dfrmOperator";
			array4[3, 0] = "数据库备份";
			array4[3, 1] = "mnuDBBackup";
			array4[3, 2] = "Basic.dfrmDbCompact";
			array4[4, 0] = "控制器通信密码";
			array4[4, 1] = "mnuControllerCommPasswordSet";
			array4[4, 2] = "Basic.";
			array4[5, 0] = "扩展功能";
			array4[5, 1] = "mnuExtendedFunction";
			array4[5, 2] = "Basic.dfrmExtendedFunctions";
			array4[6, 0] = "选项";
			array4[6, 1] = "mnuOption";
			array4[6, 2] = "";
			array4[7, 0] = "操作日志";
			array4[7, 1] = "mnuLogQuery";
			array4[7, 2] = "Basic.dfrmLogQuery";
			array4[8, 0] = "帮助";
			array4[8, 1] = "mnu1Help";
			array4[8, 2] = "";
			array4[9, 0] = "关于";
			array4[9, 1] = "mnuAbout";
			array4[9, 2] = "Basic.dfrmAbout";
			array4[10, 0] = "入门指南";
			array4[10, 1] = "mnuBeginner";
			array4[10, 2] = "Basic.";
			array4[11, 0] = "使用说明书";
			array4[11, 1] = "mnuManual";
			array4[11, 2] = "";
			array4[12, 0] = "系统特性";
			array4[12, 1] = "mnuSystemCharacteristic";
			array4[12, 2] = "";
			array4[13, 0] = "远程开门";
			array4[13, 1] = "TotalControl_RemoteOpen";
			array4[13, 2] = "";
			this.functionNameTool = array4;
			this.iLastTaskMin = -1;
			this.oldTitle = "";
			this.tmLasttaskShedule4Controller = DateTime.Now.AddMinutes(-2.0);
			this.tmStart = DateTime.Now;
			base..ctor();
			this.InitializeComponent();
			frmADCT3000.MyMessager myMessager = new frmADCT3000.MyMessager();
			Application.AddMessageFilter(myMessager);
			MdiClient mdiClient = new MdiClient();
			base.Controls.Add(mdiClient);
			Color color = Color.Empty;
			color = wgAppConfig.GetKeyColor("KeyWindows_Backcolor1", "91, 92, 120");
			this.panel2Content.BackColor = color;
			this.BackColor = color;
			this.panel4Form.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			color = wgAppConfig.GetKeyColor("KeyWindows_Backcolor4", "117, 121, 155");
			this.btnHideGettingStarted.BackColor = color;
			this.btnAddPrivilege.BackColor = color;
			this.btnAutoAddCardBySwiping.BackColor = color;
			this.btnAddController.BackColor = color;
			color = wgAppConfig.GetKeyColor("KeyWindows_Backcolor5", "147, 150, 177");
			this.grpGettingStarted.BackColor = color;
			this.btnConstMeal.BackColor = color;
			this.btnIconBasicOperate.BackColor = color;
			this.btnIconAttendance.BackColor = color;
			this.btnPatrol.BackColor = color;
			this.btnMeeting.BackColor = color;
			this.btnElevator.BackColor = color;
			this.btnFaceManagement.BackColor = color;
			color = wgAppConfig.GetKeyColor("KeyWindows_Backcolor15", "165, 191, 218");
			this.mnuMain.BackColor = color;
			if (wgTools.bUDPOnly64 > 0 && frmADCT3000.watchingP64 == null)
			{
				frmADCT3000.watchingP64 = new WatchingService();
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00075A00 File Offset: 0x00074A00
		private void btnAddController_Click(object sender, EventArgs e)
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (dfrmNetControllerConfig dfrmNetControllerConfig = new dfrmNetControllerConfig())
				{
					dfrmNetControllerConfig.ShowDialog(this);
				}
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00075A54 File Offset: 0x00074A54
		private void btnAddPrivilege_Click(object sender, EventArgs e)
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				if (wgTools.bUDPCloud > 0)
				{
					Cursor.Current = Cursors.WaitCursor;
					Thread.Sleep(3000);
					Cursor.Current = Cursors.Default;
				}
				using (dfrmPrivilege dfrmPrivilege = new dfrmPrivilege())
				{
					if (dfrmPrivilege.ShowDialog(this) == DialogResult.OK && dfrmPrivilege.bLoadConsole)
					{
						this.shortcutConsole_Click(null, null);
					}
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00075AE0 File Offset: 0x00074AE0
		private void btnAutoAddCardBySwiping_Click(object sender, EventArgs e)
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				if (wgTools.bUDPCloud > 0)
				{
					Cursor.Current = Cursors.WaitCursor;
					Thread.Sleep(3000);
					Cursor.Current = Cursors.Default;
				}
				try
				{
					using (dfrmUserAutoAdd dfrmUserAutoAdd = new dfrmUserAutoAdd())
					{
						dfrmUserAutoAdd.bAutoAddBySwiping = true;
						dfrmUserAutoAdd.ShowDialog(this);
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00075B80 File Offset: 0x00074B80
		private void btnFaceManagement_Click(object sender, EventArgs e)
		{
			switch (int.Parse("0" + wgAppConfig.getSystemParamByNO(209)))
			{
			case 1:
				this.dispInPanel4(new dfrmFaceManagement4uniubi());
				return;
			case 2:
				this.dispInPanel4(new dfrmFaceManagement4Syd());
				return;
			case 3:
				this.dispInPanel4(new dfrmFaceManagement4Hikvision());
				return;
			default:
				this.dispInPanel4(new dfrmFaceManagement4Hanvon());
				return;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00075BEC File Offset: 0x00074BEC
		private void btnFingerPrint_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new dfrmFingerManagement());
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00075BFC File Offset: 0x00074BFC
		private void btnHideGettingStarted_Click(object sender, EventArgs e)
		{
			this.grpGettingStarted.Visible = false;
			this.flowLayoutPanel1ICon.Size = new Size(this.flowLayoutPanel1ICon.Size.Width, 0);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00075C3C File Offset: 0x00074C3C
		private void btnIconBasicConfig_Click(object sender, EventArgs e)
		{
			Button button = sender as Button;
			if (this.btnIconSelected != null)
			{
				if (this.toolStripButtonBookmark1.Height > 30)
				{
					foreach (object obj in this.toolStrip1BookMark.Items)
					{
						ToolStripButton toolStripButton = (ToolStripButton)obj;
						toolStripButton.TextAlign = ContentAlignment.MiddleCenter;
					}
				}
				if (button == this.btnIconSelected)
				{
					return;
				}
			}
			if (this.panel4Form.Controls.Count > 0)
			{
				int count = this.panel4Form.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					(this.panel4Form.Controls[count - i - 1] as Form).Close();
				}
			}
			if (this.panel4Form.Controls.Count <= 0)
			{
				this.btnIconSelected = button;
				foreach (object obj2 in this.flowLayoutPanel1ICon.Controls)
				{
					if (obj2 is Button)
					{
						(obj2 as Button).BackgroundImage = null;
						(obj2 as Button).BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor5", "147, 150, 177");
					}
				}
				button.BackgroundImage = Resources.pMain_icon_focus02;
				button.BackColor = Color.Transparent;
				this.closeChildForm();
				foreach (object obj3 in this.toolStrip1BookMark.Items)
				{
					ToolStripButton toolStripButton2 = (ToolStripButton)obj3;
					toolStripButton2.BackgroundImage = Resources.pMain_Bookmark_normal;
				}
				if (icOperator.PCSysInfo(false).IndexOf(": \r\nMicrosoft Windows 7 ") <= 0)
				{
					foreach (object obj4 in this.toolStrip1BookMark.Items)
					{
						ToolStripButton toolStripButton3 = (ToolStripButton)obj4;
						toolStripButton3.TextAlign = ContentAlignment.MiddleCenter;
					}
				}
				this.btnBookmarkSelected = null;
				string[,] array = null;
				if (wgTools.SetObjToStr(button.Tag) == "BasciConfig")
				{
					array = this.functionNameBasicConfigure;
				}
				if (wgTools.SetObjToStr(button.Tag) == "BasicOperate")
				{
					array = this.functionNameBasicOperate;
				}
				if (button.Tag.ToString() == "Attendance")
				{
					array = this.functionNameAttendence;
				}
				foreach (object obj5 in this.toolStrip1BookMark.Items)
				{
					(obj5 as ToolStripButton).Visible = false;
				}
				if (array != null)
				{
					int num = 0;
					int num2 = 0;
					while (num2 < array.Length / 3 && num2 < this.toolStrip1BookMark.Items.Count)
					{
						if (!string.IsNullOrEmpty(array[num2, 1]))
						{
							this.toolStrip1BookMark.Items[num].Text = CommonStr.ResourceManager.GetString("strFunctionDisplayName_" + array[num2, 1]);
							this.toolStrip1BookMark.Items[num].Text = wgAppConfig.ReplaceFloorRoom(this.toolStrip1BookMark.Items[num].Text);
							this.toolStrip1BookMark.Items[num].Tag = "WG3000_COMM." + array[num2, 2];
							this.toolStrip1BookMark.Items[num].Visible = true;
							num++;
						}
						num2++;
					}
				}
				if (wgTools.SetObjToStr(button.Tag) == "BasicOperate" && this.shortcutConsole.Enabled)
				{
					this.shortcutConsole.PerformClick();
				}
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00076080 File Offset: 0x00075080
		private void buttonClick(Button btn, string funName)
		{
			try
			{
				btn.PerformClick();
				foreach (object obj in this.toolStrip1BookMark.Items)
				{
					ToolStripButton toolStripButton = (ToolStripButton)obj;
					if (string.Compare(toolStripButton.Tag.ToString(), "WG3000_COMM." + funName) == 0)
					{
						toolStripButton.PerformClick();
						break;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00076114 File Offset: 0x00075114
		private void checkPCDiskspace()
		{
			if (this.bcheckDiskSpace)
			{
				try
				{
					bool flag = false;
					double num = 2.0;
					if (wgTools.GetHardDiskFreeSpace(this.disknameOfApp) < num)
					{
						flag = true;
					}
					if (!flag && !this.disknameOfSql.Equals(this.disknameOfApp) && wgTools.GetHardDiskFreeSpace(this.disknameOfSql) < num)
					{
						flag = true;
					}
					if (flag)
					{
						this.count4checkPCDiskspace = 95L;
						this.lblDiskSpace.Visible = !this.lblDiskSpace.Visible;
						this.lblDiskSpace.Text = CommonStr.strDiskSpaceNotEnough;
					}
					else
					{
						this.lblDiskSpace.Visible = false;
						this.lblDiskSpace.Text = "";
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLogWithoutDB(ex.ToString());
				}
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000761E4 File Offset: 0x000751E4
		private void chkHideLogin_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000761E8 File Offset: 0x000751E8
		private void closeChildForm()
		{
			if (this.panel4Form.Controls.Count > 0)
			{
				int count = this.panel4Form.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					(this.panel4Form.Controls[count - i - 1] as Form).Close();
				}
			}
			this.statRunInfo_Num_Update("");
			this.statRunInfo_CommStatus_Update("");
			this.statRunInfo_Monitor_Update("");
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00076265 File Offset: 0x00075265
		public void closeChildForm4All()
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				this.btnIconBasicConfig.PerformClick();
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0007628C File Offset: 0x0007528C
		private void cmdChangePasswor_Click(object sender, EventArgs e)
		{
			this.dfrmSetPassword1 = new dfrmSetPassword();
			this.dfrmSetPassword1.Text = this.cmdChangePasswor.Text.Replace('&', ' ');
			this.dfrmSetPassword1.operatorID = icOperator.OperatorID;
			this.dispDfrm(this.dfrmSetPassword1);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000762E0 File Offset: 0x000752E0
		private void cmdEditOperator_Click(object sender, EventArgs e)
		{
			using (dfrmOperatorUpdate dfrmOperatorUpdate = new dfrmOperatorUpdate())
			{
				dfrmOperatorUpdate.operateMode = 1;
				dfrmOperatorUpdate.operatorID = icOperator.OperatorID;
				dfrmOperatorUpdate.operatorName = icOperator.OperatorName;
				dfrmOperatorUpdate.ShowDialog(this);
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00076334 File Offset: 0x00075334
		private void cmdOperatorManage_Click(object sender, EventArgs e)
		{
			this.dfrmOperator1 = new dfrmOperator();
			this.dispDfrm(this.dfrmOperator1);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00076350 File Offset: 0x00075350
		private void dispDfrm(Form dfrm)
		{
			if (this.panel4Form.Controls.Count > 0)
			{
				int count = this.panel4Form.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					(this.panel4Form.Controls[count - i - 1] as Form).Close();
				}
			}
			if (this.panel4Form.Controls.Count <= 0)
			{
				this.closeChildForm();
				foreach (object obj in this.flowLayoutPanel1ICon.Controls)
				{
					if (obj is Button)
					{
						(obj as Button).BackgroundImage = null;
						(obj as Button).BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor5", "147, 150, 177");
					}
				}
				foreach (object obj2 in this.toolStrip1BookMark.Items)
				{
					ToolStripButton toolStripButton = (ToolStripButton)obj2;
					toolStripButton.BackgroundImage = Resources.pMain_Bookmark_normal;
					toolStripButton.Visible = false;
				}
				this.btnIconSelected = null;
				this.btnBookmarkSelected = null;
				wgAppRunInfo.ClearAllDisplayedInfo();
				if (dfrm != null)
				{
					dfrm.ShowDialog(this);
				}
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000764C0 File Offset: 0x000754C0
		private void dispInPanel4(Form frm)
		{
			this.closeChildForm();
			if (this.panel4Form.Controls.Count <= 0)
			{
				foreach (object obj in this.flowLayoutPanel1ICon.Controls)
				{
					if (obj is Button)
					{
						(obj as Button).BackgroundImage = null;
						(obj as Button).BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor5", "147, 150, 177");
					}
				}
				foreach (object obj2 in this.toolStrip1BookMark.Items)
				{
					ToolStripButton toolStripButton = (ToolStripButton)obj2;
					toolStripButton.BackgroundImage = Resources.pMain_Bookmark_normal;
					toolStripButton.Visible = false;
				}
				this.btnIconSelected = null;
				this.btnBookmarkSelected = this.toolStripButtonBookmark1;
				frm.ShowDialog();
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000765D8 File Offset: 0x000755D8
		private void displayHideMenuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.displayHideMenuToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				this.mnuMain.Visible = !this.mnuMain.Visible;
				wgAppConfig.UpdateKeyVal("KEY_HideMainMenu", this.mnuMain.Visible ? "0" : "1");
				this.loadMainMenu();
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00076658 File Offset: 0x00075658
		private void displayHideStarterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.displayHideStarterToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				if (this.grpGettingStarted.Visible)
				{
					this.grpGettingStarted.Visible = false;
					this.flowLayoutPanel1ICon.Size = new Size(this.flowLayoutPanel1ICon.Size.Width, 0);
					wgAppConfig.UpdateKeyVal("HideGettingStartedWhenLogin", this.grpGettingStarted.Visible ? "0" : "1");
					return;
				}
				this.grpGettingStarted.Visible = true;
				this.flowLayoutPanel1ICon.Size = new Size(this.flowLayoutPanel1ICon.Size.Width, 0);
				wgAppConfig.UpdateKeyVal("HideGettingStartedWhenLogin", this.grpGettingStarted.Visible ? "0" : "1");
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0007674C File Offset: 0x0007574C
		private void displayHideStatusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.displayHideStatusBarToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				this.stbRunInfo.Visible = !this.stbRunInfo.Visible;
				wgAppConfig.UpdateKeyVal("KEY_HideStbRunInfo", this.stbRunInfo.Visible ? "0" : "1");
				this.loadMainMenu();
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000767CC File Offset: 0x000757CC
		private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmFeedback dfrmFeedback = new dfrmFeedback())
			{
				dfrmFeedback.ShowDialog();
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00076804 File Offset: 0x00075804
		private void fingerEnrollSingleUpdate()
		{
			if (frmADCT3000.qfingerEnrollInfo.Count > 0)
			{
				lock (frmADCT3000.qfingerEnrollInfo.SyncRoot)
				{
					new dfrmMultiThreadOperation
					{
						arrSelectedDoorsOnConsole = (ArrayList)frmADCT3000.qfingerEnrollarrController.Dequeue(),
						fingerDataParam = (byte[])frmADCT3000.qfingerEnrollInfo.Dequeue()
					}.startFingerSingleUpdate();
				}
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00076880 File Offset: 0x00075880
		private void frmADCT3000_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!frmADCT3000.bConfirmClose)
			{
				if (XMessageBox.Show(CommonStr.strExit + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}
				if (wgTools.bUDPOnly64 > 0 && frmADCT3000.watchingP64 != null)
				{
					frmADCT3000.watchingP64.StopWatchByForce();
				}
				frmADCT3000.bConfirmClose = true;
				this.closeChildForm();
				wgAppConfig.wgLog(this.mnuExit.Text, EventLogEntryType.Information, null);
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000768F0 File Offset: 0x000758F0
		private void frmADCT3000_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 68)
			{
				wgTools.strLogIntoFileName = Application.StartupPath + "\\n3k_log.log";
			}
			if (e.Control && !e.Shift && e.KeyValue == 84)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.frmTestController1 = new frmTestController();
				this.frmTestController1.Owner = this;
				this.frmTestController1.Show();
			}
			if (!e.Control && !e.Shift && e.KeyValue == 112)
			{
				this.mnuManual.PerformClick();
			}
			if (e.Control && e.Shift && e.KeyValue == 78)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.dfrmNetControllerConfig1 = new dfrmNetControllerConfig();
				this.dfrmNetControllerConfig1.Show();
			}
			if (!e.Control && e.Shift && e.KeyValue == 118)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.mnuDeleteOldRecords_Click(null, null);
			}
			if (!e.Control && e.Shift && e.KeyValue == 119)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.systemParamsToolStripMenuItem_Click(null, null);
			}
			if (!e.Control && e.Shift && e.KeyValue == 123)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.systemParamsCustomTitle();
			}
			if (((e.Control && e.KeyValue == 70) || e.KeyValue == 114) && this.panel4Form.Controls.Count > 0)
			{
				try
				{
					Form form = this.panel4Form.Controls[0] as Form;
					if (this.btnBookmarkSelected != null)
					{
						if (this.btnBookmarkSelected.Tag.ToString().IndexOf(".frmControllers") > 0)
						{
							(form as frmControllers).frmControllers_KeyDown(form, e);
						}
						if (this.btnBookmarkSelected.Tag.ToString().IndexOf(".frmConsole") > 0)
						{
							(form as frmConsole).frmConsole_KeyDown(form, e);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			if (e.Control && e.Shift && e.KeyValue == 69)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.toolStripMenuItem29a_Click(null, null);
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00076BAC File Offset: 0x00075BAC
		private void frmADCT3000_Load(object sender, EventArgs e)
		{
			this.iLoginCnt = wgAppConfig.GetKeyVal("RunTimes").Length;
			this.toolStripDropDownButton1.Visible = false;
			this.mnu1Help.Visible = false;
			this.getDiskname();
			wgAppConfig.bFloorRoomManager = wgAppConfig.getParamValBoolByNO(145);
			wgTools.gbInputKeyPasswordControl = wgAppConfig.getParamValBoolByNO(218);
			wgTools.gbHideCardNO = wgAppConfig.getParamValBoolByNO(219);
			if (wgAppConfig.getParamValBoolByNO(220))
			{
				wgTools.gbInputKeyPasswordControl = true;
				wgTools.gbHideCardNO = true;
				wgTools.gbAutoLockInterface = true;
			}
			this.toolStripButtonBookmark2.Text = wgAppConfig.ReplaceFloorRoom(this.toolStripButtonBookmark2.Text);
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
			base.KeyPreview = true;
			Application.ThreadException += Program.GlobalExceptionHandler;
			UserControlFind.blogin = true;
			UserControlFind4Shift.blogin = true;
			UserControlFindSecond.blogin = true;
			UserControlFindSecond4Shift.blogin = true;
			Application.ThreadException += Program.GlobalExceptionHandler;
			this.hideMenuBySystemConfig();
			this.hideMenuByUserPrivilege();
			this.loadOperatorPrivilegeOfConsole();
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.mnu2Camera.Visible = false;
				this.btnElevator.Visible = false;
				this.mnu2OneToMore.Visible = false;
				this.mnu2GlobalAntipassback.Visible = false;
				this.btnFaceManagement.Visible = false;
			}
			bool flag = true;
			foreach (object obj in this.flowLayoutPanel1ICon.Controls)
			{
				if (obj is Button && (obj as Button).Visible)
				{
					flag = false;
					if (obj != this.btnIconBasicConfig && obj != this.btnIconBasicOperate && obj != this.btnIconAttendance)
					{
						break;
					}
					break;
				}
			}
			if (flag)
			{
				XMessageBox.Show(CommonStr.strOperatorHaveNoPrivilege);
				frmADCT3000.bConfirmClose = true;
				this.mnuExit.PerformClick();
				return;
			}
			UserControlFind.blogin = true;
			UserControlFind4Shift.blogin = true;
			UserControlFindSecond.blogin = true;
			UserControlFindSecond4Shift.blogin = true;
			this.mnu2CheckAccess.Visible = false;
			if (wgAppConfig.getParamValBoolByNO(137) && icOperator.OperatorID == 1)
			{
				this.mnuPCCheckAccessConfigure.Visible = true;
				this.mnu2CheckAccess.Visible = true;
			}
			if (!wgAppConfig.getParamValBoolByNO(144))
			{
				this.mnuElevator.Visible = false;
				this.mnu2OneToMore.Visible = false;
				this.btnElevator.Visible = false;
			}
			else
			{
				using (dfrmOneToMoreSetup dfrmOneToMoreSetup = new dfrmOneToMoreSetup())
				{
					if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 2)
					{
						this.mnuElevator.Text = dfrmOneToMoreSetup.radioButton1.Text;
						this.mnu2OneToMore.Text = dfrmOneToMoreSetup.radioButton1.Text;
						this.btnElevator.Text = dfrmOneToMoreSetup.radioButton1.Text;
					}
					else if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 3)
					{
						this.mnuElevator.Text = dfrmOneToMoreSetup.radioButton2.Text;
						this.mnu2OneToMore.Text = dfrmOneToMoreSetup.radioButton2.Text;
						this.btnElevator.Text = dfrmOneToMoreSetup.radioButton2.Text;
					}
					else
					{
						this.mnuElevator.Text = dfrmOneToMoreSetup.radioButton0.Text;
						this.mnu2OneToMore.Text = dfrmOneToMoreSetup.radioButton0.Text;
						this.btnElevator.Text = dfrmOneToMoreSetup.radioButton0.Text;
					}
				}
			}
			if (!wgAppConfig.getParamValBoolByNO(186))
			{
				this.btnFaceManagement.Visible = false;
			}
			if (!wgAppConfig.getParamValBoolByNO(188))
			{
				this.btnFingerPrint.Visible = false;
			}
			if (!wgAppConfig.getParamValBoolByNO(149))
			{
				this.mnuMeetingSign.Visible = false;
				this.mnu2Meeting.Visible = false;
				this.btnMeeting.Visible = false;
			}
			else
			{
				this.mnuMeetingSign.Text = wgAppConfig.ReplaceMeeting(this.mnuMeetingSign.Text);
				this.mnu2Meeting.Text = wgAppConfig.ReplaceMeeting(this.mnu2Meeting.Text);
				this.btnMeeting.Text = wgAppConfig.ReplaceMeeting(this.btnMeeting.Text);
			}
			if (!wgAppConfig.getParamValBoolByNO(150))
			{
				this.mnuMeal.Visible = false;
				this.btnConstMeal.Visible = false;
				this.mnu2ConstMeal.Visible = false;
			}
			if (!wgAppConfig.getParamValBoolByNO(151))
			{
				this.mnuPatrol.Visible = false;
				this.mnu2Patrol.Visible = false;
				this.btnPatrol.Visible = false;
			}
			if (!wgAppConfig.IsActivateCameraManage)
			{
				this.mnuCameraManage.Visible = false;
				this.mnu2Camera.Visible = false;
			}
			else
			{
				try
				{
					string text = wgAppConfig.Path4AviJpgDefault();
					if (!wgAppConfig.DirectoryIsExisted(text))
					{
						Directory.CreateDirectory(text);
					}
					if (!wgAppConfig.DirectoryIsExisted(text))
					{
						wgAppConfig.wgLog(text + " " + CommonStr.strFileDirectoryNotVisited);
					}
				}
				catch (Exception)
				{
				}
			}
			this.mnu2Zones.Visible = false;
			bool flag2 = false;
			string text2 = "btnZoneManage";
			if (icOperator.OperatePrivilegeVisible(text2, ref flag2) && !flag2)
			{
				this.mnu2Zones.Visible = true;
			}
			this.mnu2CardLost.Visible = this.mnu2Personnel.Enabled;
			if (!this.mnu2Personnel.Enabled)
			{
				this.btnFaceManagement.Visible = false;
				this.btnFingerPrint.Visible = false;
			}
			if (this.mnu2Personnel.Enabled)
			{
				text2 = "mnuCardLost";
				if (icOperator.OperatePrivilegeVisible(text2, ref flag2))
				{
					this.mnu2CardLost.Visible = !flag2;
				}
				else
				{
					this.mnu2CardLost.Visible = false;
				}
				text2 = "mnuConsumers";
				if (icOperator.OperatePrivilegeVisible(text2, ref flag2) && flag2)
				{
					this.btnFaceManagement.Visible = false;
					this.btnFingerPrint.Visible = false;
				}
			}
			this.mnu2LimitedAccessTimes.Visible = this.mnu2TimeProfile.Enabled && wgAppConfig.getParamValBoolByNO(136);
			this.mnu2AccessHolidayControl.Visible = this.mnu2TimeProfile.Enabled;
			this.mnu2DoorAsSwitch.Visible = false;
			if (wgAppConfig.getParamValBoolByNO(146) && icOperator.OperatorID == 1)
			{
				this.mnuDoorAsSwitch.Visible = true;
				this.mnu2DoorAsSwitch.Visible = true;
			}
			if (!wgAppConfig.getParamValBoolByNO(148))
			{
				this.cmdOperatorManage.Visible = false;
				this.mnu2OperatorManagement.Visible = false;
			}
			if (wgAppConfig.getParamValBoolByNO(111))
			{
				this.mnu2DoorAsSwitch.Visible = false;
				this.mnu2Camera.Visible = false;
				this.mnu2CheckAccess.Visible = false;
				this.toolStripSeparator9.Visible = false;
				this.toolStripSeparator13.Visible = false;
				this.toolStripSeparator14.Visible = false;
			}
			if (wgAppConfig.getParamValBoolByNO(217))
			{
				this.mnu1MultiFunc.Visible = false;
			}
			this.toolStripSeparator9.Visible = false;
			this.toolStripSeparator14.Visible = false;
			if (!wgAppConfig.getParamValBoolByNO(181))
			{
				this.mnu2GlobalAntipassback.Visible = false;
			}
			if (wgAppConfig.IsAccessDB)
			{
				this.mnu2GlobalAntipassback.Visible = false;
			}
			if (wgAppConfig.GetKeyVal("KEY_InterfaceLock") == "1")
			{
				this.interfaceLockToolStripMenuItem.Visible = true;
			}
			this.Text = wgAppConfig.LoginTitle;
			this.loadStbRunInfo();
			wgAppRunInfo.evAppRunInfoLoadNum += this.statRunInfo_Num_Update;
			wgAppRunInfo.evAppRunInfoCommStatus += this.statRunInfo_CommStatus_Update;
			wgAppRunInfo.evAppRunInfoMonitor += this.statRunInfo_Monitor_Update;
			base.WindowState = FormWindowState.Maximized;
			this.dtglngReceiveAutoUploadCount = DateTime.Now.AddMinutes(5.0);
			this.dtglngReceiveCount = DateTime.Now.AddMinutes(5.0);
			this.timer1.Enabled = true;
			if (icOperator.OperatorID != 1 || wgAppConfig.GetKeyVal("HideGettingStartedWhenLogin") == "1")
			{
				this.grpGettingStarted.Visible = false;
				if (icOperator.OperatorID != 1)
				{
					this.toolStripMenuItem20.Visible = false;
					this.mnu2Beginner.Visible = false;
					this.mnu2hideGettingStartedToolStripMenuItem.Visible = false;
				}
			}
			string text3 = "SELECT COUNT(*) from t_s_Operator ";
			if (int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getValBySql(text3))) > 1)
			{
				this.cmdEditOperator.Visible = false;
			}
			else
			{
				this.cmdChangePasswor.Visible = false;
				this.cmdEditOperator.Visible = true;
			}
			this.flowLayoutPanel1ICon.Size = new Size(this.flowLayoutPanel1ICon.Size.Width, 0);
			this.loadMainMenu();
			this.Refresh();
			if (!wgAppConfig.getParamValBoolByNO(64) && this.shortcutConsole.Enabled)
			{
				Thread.Sleep(200);
				this.shortcutConsole.PerformClick();
			}
			if (wgAppConfig.getParamValBoolByNO(188))
			{
				new Thread(new ThreadStart(this.getPortNotUSB))
				{
					IsBackground = true,
					CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false)
				}.Start();
			}
			if (icOperator.OperatorID != 1)
			{
				this.displayHideMenuToolStripMenuItem.Visible = false;
				this.displayHideStarterToolStripMenuItem.Visible = false;
			}
			this.bcheckDiskSpace = wgAppConfig.getParamValBoolByNO(204);
			if (wgAppConfig.GetKeyVal("AutoLoginMinimize").Equals("1"))
			{
				base.WindowState = FormWindowState.Minimized;
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00077558 File Offset: 0x00076558
		private Form getCurrentForm(string formName)
		{
			if (this.panel4Form.Controls.Count > 0)
			{
				try
				{
					Form form = this.panel4Form.Controls[0] as Form;
					if (this.btnBookmarkSelected != null && this.btnBookmarkSelected.Tag.ToString().IndexOf(formName) > 0)
					{
						return form;
					}
				}
				catch (Exception)
				{
				}
			}
			return null;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000775CC File Offset: 0x000765CC
		private void getDiskname()
		{
			try
			{
				this.disknameOfApp = Application.ExecutablePath.Substring(0, 1);
				this.disknameOfSql = this.disknameOfApp;
				if (!wgAppConfig.IsAccessDB)
				{
					string text = "";
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						text = sqlConnection.Database;
					}
					char c = char.Parse(wgAppConfig.getValStringBySql(string.Format(" SELECT FileName FROM master.dbo.sysdatabases WHERE name = {0}", wgTools.PrepareStrNUnicode(text))).Substring(0, 1));
					if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
					{
						this.disknameOfSql = c.ToString();
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00077688 File Offset: 0x00076688
		private void getPortNotUSB()
		{
			frmADCT3000.portsNotUSB = frmADCT3000.GetSerialPort();
			wgAppConfig.wgLog("portsNotUSB = " + frmADCT3000.portsNotUSB);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000776A8 File Offset: 0x000766A8
		public static string GetSerialPort()
		{
			try
			{
				return frmADCT3000.MulGetHardwareInfo(frmADCT3000.HardwareEnum.Win32_SerialPort, "Name");
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return "";
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000776EC File Offset: 0x000766EC
		private void hideFuncItem(ref string[,] func, string funcName, bool bNotHide)
		{
			if (!bNotHide)
			{
				for (int i = 0; i < func.Length / 3; i++)
				{
					if (!string.IsNullOrEmpty(func[i, 1]) && func[i, 1] == funcName)
					{
						func[i, 1] = null;
						return;
					}
				}
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0007773C File Offset: 0x0007673C
		private void hideMenuBySystemConfig()
		{
			this.mnuLogQuery.Visible = wgAppConfig.getParamValBoolByNO(103);
			this.mnu2LogQuery.Visible = wgAppConfig.getParamValBoolByNO(103);
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuControlSeg", wgAppConfig.getParamValBoolByNO(121));
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuPasswordManagement", wgAppConfig.getParamValBoolByNO(123));
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuPeripheral", wgAppConfig.getParamValBoolByNO(124));
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuAntiBack", wgAppConfig.getParamValBoolByNO(132));
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuInterLock", wgAppConfig.getParamValBoolByNO(133));
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuMoreCards", wgAppConfig.getParamValBoolByNO(134));
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuFirstCard", wgAppConfig.getParamValBoolByNO(135));
			this.hideFuncItem(ref this.functionNameBasicConfigure, "mnuTaskList", wgAppConfig.getParamValBoolByNO(131));
			this.btnIconAttendance.Visible = !wgAppConfig.getParamValBoolByNO(112);
			if (!wgAppConfig.getParamValBoolByNO(112))
			{
				this.hideFuncItem(ref this.functionNameAttendence, "mnuShiftArrange", wgAppConfig.getParamValBoolByNO(113));
				this.hideFuncItem(ref this.functionNameAttendence, "mnuShiftRule", wgAppConfig.getParamValBoolByNO(113));
				this.hideFuncItem(ref this.functionNameAttendence, "mnuShiftSet", wgAppConfig.getParamValBoolByNO(113));
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000778A4 File Offset: 0x000768A4
		private void hideMenuByUserPrivilege()
		{
			icOperator.OperatePrivilegeLoad(ref this.functionNameBasicConfigure, 3, 1);
			if (wgAppConfig.getParamValBoolByNO(111))
			{
				for (int i = 3; i <= 11; i++)
				{
					this.functionNameBasicConfigure[i, 1] = null;
				}
			}
			icOperator.OperatePrivilegeLoad(ref this.functionNameBasicOperate, 3, 1);
			icOperator.OperatePrivilegeLoad(ref this.functionNameAttendence, 3, 1);
			using (ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem())
			{
				foreach (object obj in this.contextMenuStrip1Tools.Items)
				{
					if (object.ReferenceEquals(obj.GetType(), toolStripMenuItem.GetType()) && !icOperator.OperatePrivilegeVisible((obj as ToolStripMenuItem).Name))
					{
						(obj as ToolStripMenuItem).Visible = false;
					}
				}
				foreach (object obj2 in this.contextMenuStrip2Help.Items)
				{
					if (object.ReferenceEquals(obj2.GetType(), toolStripMenuItem.GetType()) && !icOperator.OperatePrivilegeVisible((obj2 as ToolStripMenuItem).Name))
					{
						(obj2 as ToolStripMenuItem).Visible = false;
					}
				}
			}
			this.mnuLogQuery.Visible = icOperator.OperatePrivilegeFullControl("mnuLogQuery") && wgAppConfig.getParamValBoolByNO(103);
			this.mnu2LogQuery.Visible = icOperator.OperatePrivilegeFullControl("mnuLogQuery") && wgAppConfig.getParamValBoolByNO(103);
			this.mnuDBBackup.Visible = icOperator.OperatePrivilegeFullControl("mnuDBBackup");
			this.mnu2DBBackup.Visible = icOperator.OperatePrivilegeFullControl("mnuDBBackup");
			if (!icOperator.OperatePrivilegeVisible("mnu1BasicConfigure"))
			{
				this.btnIconBasicConfig.Visible = false;
			}
			if (!icOperator.OperatePrivilegeVisible("mnu1BasicOperate"))
			{
				this.btnIconBasicOperate.Visible = false;
			}
			if (!icOperator.OperatePrivilegeVisible("mnu1Attendence"))
			{
				this.btnIconAttendance.Visible = false;
			}
			if (!this.btnIconBasicConfig.Visible)
			{
				this.flowLayoutPanel1ICon.Controls.Remove(this.btnIconBasicConfig);
				this.btnIconBasicConfig.Dispose();
				this.shortcutControllers.Visible = false;
				this.shortcutPersonnel.Visible = false;
				this.shortcutPrivilege.Visible = false;
				this.mnu1Configure.Visible = false;
			}
			this.mnuCameraManage.Visible = icOperator.OperatePrivilegeVisible("mnuCameraManage");
			this.mnu2Camera.Visible = icOperator.OperatePrivilegeVisible("mnuCameraManage");
			if (!this.btnIconBasicOperate.Visible)
			{
				this.flowLayoutPanel1ICon.Controls.Remove(this.btnIconBasicOperate);
				this.btnIconBasicOperate.Dispose();
				this.shortcutConsole.Visible = false;
				this.shortcutSwipe.Visible = false;
				this.mnu2QuerySwipeRecords.Visible = false;
				this.mnu1Operate.Visible = false;
			}
			if (!this.btnIconAttendance.Visible)
			{
				this.flowLayoutPanel1ICon.Controls.Remove(this.btnIconAttendance);
				this.btnIconAttendance.Dispose();
				this.shortcutAttendance.Visible = false;
				this.mnu2Attendence.Visible = false;
			}
			double num = 0.0;
			foreach (object obj3 in this.flowLayoutPanel1ICon.Controls)
			{
				Control control = (Control)obj3;
				if (control is Button)
				{
					if ((control as Button).Image != null)
					{
						num += 1.0;
					}
					else
					{
						num += 0.5;
					}
				}
			}
			this.flowLayoutPanel1ICon.Size = new Size(this.flowLayoutPanel1ICon.Width, (int)(88.0 * num));
			this.mnuPatrol.Visible = icOperator.OperatePrivilegeVisible("mnuPatrolDetailData");
			this.mnu2Patrol.Visible = icOperator.OperatePrivilegeVisible("mnuPatrolDetailData");
			this.btnPatrol.Visible = icOperator.OperatePrivilegeVisible("mnuPatrolDetailData");
			this.mnuMeal.Visible = icOperator.OperatePrivilegeVisible("mnuConstMeal");
			this.mnu2ConstMeal.Visible = icOperator.OperatePrivilegeVisible("mnuConstMeal");
			this.btnConstMeal.Visible = icOperator.OperatePrivilegeVisible("mnuConstMeal");
			this.mnuMeetingSign.Visible = icOperator.OperatePrivilegeVisible("mnuMeeting");
			this.mnu2Meeting.Visible = icOperator.OperatePrivilegeVisible("mnuMeeting");
			this.btnMeeting.Visible = icOperator.OperatePrivilegeVisible("mnuMeeting");
			this.mnuElevator.Visible = icOperator.OperatePrivilegeVisible("mnuElevator");
			this.mnu2OneToMore.Visible = icOperator.OperatePrivilegeVisible("mnuElevator");
			this.btnElevator.Visible = icOperator.OperatePrivilegeVisible("mnuElevator");
			this.mnu1Tool.Visible = icOperator.OperatePrivilegeFullControl("mnu1Tool");
			this.mnu1Tools.Visible = icOperator.OperatePrivilegeFullControl("mnu1Tool");
			this.cmdChangePasswor.Visible = icOperator.OperatePrivilegeFullControl("cmdChangePasswor");
			this.mnu2EditOperator.Visible = icOperator.OperatePrivilegeFullControl("cmdChangePasswor");
			this.cmdOperatorManage.Visible = icOperator.OperatePrivilegeFullControl("cmdOperatorManage");
			this.mnu2OperatorManagement.Visible = icOperator.OperatePrivilegeFullControl("cmdOperatorManage");
			this.mnuExtendedFunction.Visible = icOperator.OperatePrivilegeFullControl("mnuExtendedFunction");
			this.mnu2ExtendedFunction.Visible = icOperator.OperatePrivilegeFullControl("mnuExtendedFunction");
			this.mnuOption.Visible = icOperator.OperatePrivilegeFullControl("mnuOption");
			this.mnu2Language.Visible = icOperator.OperatePrivilegeFullControl("mnuOption");
			this.mnu2InterfaceTitle.Visible = icOperator.OperatePrivilegeFullControl("mnuOption");
			this.mnu2AutoLogin.Visible = icOperator.OperatePrivilegeFullControl("mnuOption");
			this.mnu1Help.Visible = icOperator.OperatePrivilegeVisible("mnu1Help");
			this.mnu1Help.Visible = false;
			this.mnu1HelpA.Visible = icOperator.OperatePrivilegeVisible("mnu1Help");
			this.mnuAbout.Visible = icOperator.OperatePrivilegeVisible("mnuAbout");
			this.mnu2About.Visible = icOperator.OperatePrivilegeVisible("mnuAbout");
			this.mnuManual.Visible = icOperator.OperatePrivilegeVisible("mnuManual");
			this.mnu2Manual.Visible = icOperator.OperatePrivilegeVisible("mnuManual");
			this.mnuSystemCharacteristic.Visible = icOperator.OperatePrivilegeVisible("mnuSystemCharacteristic");
			this.mnu2SystemCharacteristic.Visible = icOperator.OperatePrivilegeVisible("mnuSystemCharacteristic");
			if (this.functionNameBasicConfigure[0, 1] == null)
			{
				this.shortcutControllers.Visible = false;
				this.mnu2Controller.Visible = false;
			}
			if (this.functionNameBasicConfigure[1, 1] == null)
			{
				this.mnu2Departments.Visible = false;
			}
			if (this.functionNameBasicConfigure[2, 1] == null)
			{
				this.shortcutPersonnel.Visible = false;
				this.mnu2Personnel.Visible = false;
				this.mnu2Personnel.Enabled = false;
			}
			if (this.functionNameBasicConfigure[3, 1] == null)
			{
				this.shortcutPrivilege.Visible = false;
				this.mnu2AccessPrivilege.Visible = false;
				this.mnu2privilegeTypesManagementToolStripMenuItem.Visible = false;
			}
			else if (wgAppConfig.IsPrivilegeTypeManagementModeActive)
			{
				this.shortcutPrivilege.Visible = false;
				this.mnu2AccessPrivilege.Visible = false;
				this.functionNameBasicConfigure[3, 1] = "mnuPrivilegeTypeManagement";
				this.functionNameBasicConfigure[3, 2] = "ExtendFunc.PrivilegeType.frmPrivilegeTypeManagement";
			}
			else
			{
				this.mnu2privilegeTypesManagementToolStripMenuItem.Visible = false;
			}
			if (this.functionNameBasicConfigure[4, 1] == null)
			{
				this.mnu2TimeProfile.Visible = false;
				this.mnu2TimeProfile.Enabled = false;
			}
			if (this.functionNameBasicConfigure[5, 1] == null)
			{
				this.mnu2PeripheralControl.Visible = false;
			}
			if (this.functionNameBasicConfigure[6, 1] == null)
			{
				this.mnu2PasswordManagement.Visible = false;
			}
			if (this.functionNameBasicConfigure[7, 1] == null)
			{
				this.mnu2AntiPassback.Visible = false;
				if (!wgAppConfig.getParamValBoolByNO(181))
				{
					this.mnu2GlobalAntipassback.Visible = false;
				}
				else if (!icOperator.OperatePrivilegeFullControl("mnuAntiBack"))
				{
					this.mnu2GlobalAntipassback.Visible = false;
				}
			}
			if (this.functionNameBasicConfigure[8, 1] == null)
			{
				this.mnu2InterLock.Visible = false;
			}
			if (this.functionNameBasicConfigure[9, 1] == null)
			{
				this.mnu2MultiCard.Visible = false;
			}
			if (this.functionNameBasicConfigure[10, 1] == null)
			{
				this.mnu2FirstCardOpen.Visible = false;
			}
			if (this.functionNameBasicConfigure[11, 1] == null)
			{
				this.mnu2TaskList.Visible = false;
			}
			if (this.functionNameBasicOperate[0, 1] == null)
			{
				this.shortcutConsole.Visible = false;
				this.mnu2Console.Visible = false;
				this.mnu2Console.Enabled = false;
			}
			if (this.functionNameBasicOperate[1, 1] == null)
			{
				this.shortcutSwipe.Visible = false;
				this.mnu2QuerySwipeRecords.Visible = false;
			}
			if (this.functionNameAttendence[0, 1] == null)
			{
				this.shortcutAttendance.Visible = false;
			}
			if (this.functionNameAttendence[0, 1] == null)
			{
				this.mnuAttendenceData.Visible = false;
			}
			if (this.functionNameAttendence[1, 1] == null)
			{
				this.mnuShiftNormalConfigure.Visible = false;
			}
			if (this.functionNameAttendence[2, 1] == null)
			{
				this.mnuShiftRule.Visible = false;
			}
			if (this.functionNameAttendence[3, 1] == null)
			{
				this.mnuShiftSet.Visible = false;
			}
			if (this.functionNameAttendence[4, 1] == null)
			{
				this.mnuShiftArrange.Visible = false;
			}
			if (this.functionNameAttendence[5, 1] == null)
			{
				this.mnuHolidaySet.Visible = false;
			}
			if (this.functionNameAttendence[6, 1] == null)
			{
				this.mnuLeave.Visible = false;
			}
			if (this.functionNameAttendence[7, 1] == null)
			{
				this.mnuManualCardRecord.Visible = false;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000782B4 File Offset: 0x000772B4
		private void LanMenu_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000782B8 File Offset: 0x000772B8
		private void loadMainMenu()
		{
			if (wgAppConfig.GetKeyVal("KEY_HideMainMenu") == "1")
			{
				this.mnuMain.Visible = false;
			}
			else
			{
				this.mnuMain.Visible = true;
			}
			if (!this.mnuMain.Visible)
			{
				Size size = this.mnuMain.Size;
				if (this.toolStrip1BookMark.Location.Y == 28 || this.toolStrip1BookMark.Location.Y == 26)
				{
					this.toolStrip1BookMark.Location = new Point(this.toolStrip1BookMark.Location.X, this.toolStrip1BookMark.Location.Y - size.Height);
					this.panel4Form.Location = new Point(this.panel4Form.Location.X, this.panel4Form.Location.Y - size.Height);
					this.panel4Form.Size = new Size(this.panel4Form.Size.Width, this.panel4Form.Size.Height + size.Height);
					this.flowLayoutPanel1ICon.Location = new Point(this.flowLayoutPanel1ICon.Location.X, this.flowLayoutPanel1ICon.Location.Y - size.Height);
				}
			}
			else
			{
				Size size2 = this.mnuMain.Size;
				if (this.toolStrip1BookMark.Location.Y != 28 && this.toolStrip1BookMark.Location.Y != 26)
				{
					this.toolStrip1BookMark.Location = new Point(this.toolStrip1BookMark.Location.X, this.toolStrip1BookMark.Location.Y + size2.Height);
					this.panel4Form.Location = new Point(this.panel4Form.Location.X, this.panel4Form.Location.Y + size2.Height);
					this.panel4Form.Size = new Size(this.panel4Form.Size.Width, this.panel4Form.Size.Height - size2.Height);
					this.flowLayoutPanel1ICon.Location = new Point(this.flowLayoutPanel1ICon.Location.X, this.flowLayoutPanel1ICon.Location.Y + size2.Height);
				}
			}
			if (wgAppConfig.GetKeyVal("KEY_HideStbRunInfo") == "1")
			{
				this.stbRunInfo.Visible = false;
			}
			else
			{
				this.stbRunInfo.Visible = true;
			}
			if (!this.stbRunInfo.Visible)
			{
				if (!this.stbStatucModeChange)
				{
					this.stbStatucModeChange = true;
					Size size3 = this.stbRunInfo.Size;
					this.panel4Form.Size = new Size(this.panel4Form.Size.Width, this.panel4Form.Size.Height + size3.Height);
					return;
				}
			}
			else if (this.stbStatucModeChange)
			{
				Size size4 = this.stbRunInfo.Size;
				this.panel4Form.Size = new Size(this.panel4Form.Size.Width, this.panel4Form.Size.Height - size4.Height);
				this.stbStatucModeChange = false;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00078684 File Offset: 0x00077684
		private void loadOperatorPrivilegeOfConsole()
		{
			bool flag;
			bool flag2;
			icOperator.getFrmOperatorPrivilege("frmConsole", out flag, out flag2);
			if (!this.mnu2Console.Enabled)
			{
				flag2 = false;
				flag = false;
				this.mnu2Check.Visible = false;
				this.mnu2AdjustTime.Visible = false;
				this.mnu2Upload.Visible = false;
				this.mnu2Monitor.Visible = false;
				this.mnu2GetRecords.Visible = false;
				this.mnu2RemoteOpen.Visible = false;
				this.mnu2RealtimeGetRecords.Visible = false;
				this.mnu2Maps.Visible = false;
				this.mnu2WarnOutputReset.Visible = false;
				this.mnu2ResetPersonInside.Visible = false;
				this.mnu2SetDoorControl.Visible = false;
				this.mnu2Locate.Visible = false;
				this.mnu2PersonInside.Visible = false;
				return;
			}
			if (flag2)
			{
				icOperator.getFrmOperatorPrivilege("btnCheckController", out flag, out flag2);
				this.mnu2Check.Visible = flag || flag2;
				icOperator.getFrmOperatorPrivilege("btnAdjustTime", out flag, out flag2);
				this.mnu2AdjustTime.Visible = flag2;
				icOperator.getFrmOperatorPrivilege("btnUpload", out flag, out flag2);
				this.mnu2Upload.Visible = flag2;
				this.mnu2Upload.Enabled = flag2;
				icOperator.getFrmOperatorPrivilege("btnMonitor", out flag, out flag2);
				this.mnu2Monitor.Visible = flag || flag2;
				icOperator.getFrmOperatorPrivilege("btnGetRecords", out flag, out flag2);
				this.mnu2GetRecords.Visible = flag2;
				icOperator.getFrmOperatorPrivilege("btnRemoteOpen", out flag, out flag2);
				this.mnu2RemoteOpen.Visible = flag2;
				this.mnu2RemoteOpen.Enabled = flag2;
				this.mnu2RemoteOpen.Text = wgAppConfig.ReplaceSpecialWord(this.mnu2RemoteOpen.Text, "KEY_strRemoteOpenDoor");
				icOperator.getFrmOperatorPrivilege("btnRealtimeGetRecords", out flag, out flag2);
				this.mnu2RealtimeGetRecords.Visible = flag2;
				this.mnu2Maps.Visible = icOperator.OperatePrivilegeVisible("btnMaps");
			}
			else if (flag)
			{
				icOperator.getFrmOperatorPrivilege("btnCheckController", out flag, out flag2);
				this.mnu2Check.Visible = flag || flag2;
				this.mnu2AdjustTime.Visible = false;
				this.mnu2Upload.Visible = false;
				this.mnu2Upload.Enabled = false;
				icOperator.getFrmOperatorPrivilege("btnMonitor", out flag, out flag2);
				this.mnu2Monitor.Visible = flag || flag2;
				icOperator.getFrmOperatorPrivilege("btnMaps", out flag, out flag2);
				this.mnu2Maps.Visible = flag2 || flag;
				this.mnu2GetRecords.Visible = false;
				this.mnu2RemoteOpen.Visible = false;
				this.mnu2RemoteOpen.Enabled = false;
				this.mnu2RealtimeGetRecords.Visible = false;
			}
			else
			{
				this.mnu2Check.Visible = false;
				this.mnu2AdjustTime.Visible = false;
				this.mnu2Upload.Visible = false;
				this.mnu2Upload.Enabled = false;
				this.mnu2Monitor.Visible = false;
				this.mnu2GetRecords.Visible = false;
				this.mnu2RemoteOpen.Visible = false;
				this.mnu2RemoteOpen.Enabled = false;
				this.mnu2RealtimeGetRecords.Visible = false;
				this.mnu2Maps.Visible = false;
			}
			if (!wgAppConfig.getParamValBoolByNO(122))
			{
				this.mnu2RemoteOpen.Visible = false;
			}
			if (!wgAppConfig.getParamValBoolByNO(114))
			{
				this.mnu2Maps.Visible = false;
			}
			if (this.mnu2Console.Enabled)
			{
				this.mnu2WarnOutputReset.Visible = wgAppConfig.getParamValBoolByNO(124);
				this.mnu2ResetPersonInside.Visible = wgAppConfig.getParamValBoolByNO(132);
				this.mnu2SetDoorControl.Visible = this.mnu2Upload.Enabled || this.mnu2RemoteOpen.Enabled;
				this.mnu2Locate.Visible = wgAppConfig.getParamValBoolByNO(161);
				this.mnu2PersonInside.Visible = wgAppConfig.getParamValBoolByNO(162);
				return;
			}
			this.mnu2WarnOutputReset.Visible = false;
			this.mnu2ResetPersonInside.Visible = false;
			this.mnu2SetDoorControl.Visible = false;
			this.mnu2Locate.Visible = false;
			this.mnu2PersonInside.Visible = false;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00078A8C File Offset: 0x00077A8C
		private void loadStbRunInfo()
		{
			if (icOperator.OperatorID == 1)
			{
				this.statOperator.Text = string.Format("{0}:{1}", CommonStr.strSuper, icOperator.OperatorName);
			}
			else
			{
				this.statOperator.Text = string.Format("{0}", icOperator.OperatorName);
			}
			string text = Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
			text = text.Substring(0, text.LastIndexOf("."));
			if (wgAppConfig.IsAccessDB)
			{
				this.statSoftwareVer.Text = string.Format("{0} - Ver: {1}", "Access", text);
			}
			else
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					this.statSoftwareVer.Text = string.Format("SQL: {0} - Ver: {1}", sqlConnection.Database, text);
				}
			}
			string text2 = "BLUE";
			if (wgTools.gWGYTJ)
			{
				text2 = "AGYTJ";
			}
			string productTypeOfApp = wgAppConfig.ProductTypeOfApp;
			if (productTypeOfApp != null)
			{
				if (!(productTypeOfApp == "CGACCESS"))
				{
					if (productTypeOfApp == "AGACCESS")
					{
						text2 = "AG";
					}
					else if (productTypeOfApp == "XGACCESS")
					{
						text2 = "ADCT";
					}
				}
				else
				{
					text2 = "CG";
				}
			}
			this.statSoftwareVer.Text = this.statSoftwareVer.Text.Replace(" - Ver: ", string.Format(" -{0}- Ver: ", text2));
			string[] array = Application.ProductVersion.Split(new char[] { '.' });
			if (array.Length >= 4 && int.Parse(array[1]) % 2 == 0)
			{
				this.statSoftwareVer.Text = this.statSoftwareVer.Text + "." + array[3].ToString();
			}
			wgTools.CommPStr = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommPCurrent"));
			if (!string.IsNullOrEmpty(wgTools.CommPStr))
			{
				this.statSoftwareVer.Text = this.statSoftwareVer.Text + ":!s";
			}
			if (!string.IsNullOrEmpty(wgTools.gCustomProductType))
			{
				this.statSoftwareVer.Text = this.statSoftwareVer.Text + ":!jm";
			}
			this.statCOM.Text = "";
			this.statRuninfo1.Text = "";
			this.statRuninfo1.Spring = true;
			this.statRuninfo2.Text = "";
			this.statRuninfo3.Text = "";
			this.statRuninfo3.AutoSize = false;
			this.statRuninfo3.Width = 48;
			this.statRuninfoLoadedNum.Text = "";
			this.statRuninfoLoadedNum.AutoSize = false;
			this.statRuninfoLoadedNum.Width = 137;
			this.statTimeDate.Text = DateTime.Now.ToString(wgTools.YMDHMSFormat);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00078D6C File Offset: 0x00077D6C
		private void mnu2About_Click(object sender, EventArgs e)
		{
			this.dfrmAbout1 = new dfrmAbout();
			this.dfrmAbout1.Owner = this;
			this.dispDfrm(this.dfrmAbout1);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00078D94 File Offset: 0x00077D94
		private void mnu2AccessHolidayControl_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmControlSegs");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicConfig, "Basic.frmControlSegs");
				form = this.getCurrentForm(".frmControlSegs");
			}
			if (form != null)
			{
				(form as frmControlSegs).btnHolidayControl.PerformClick();
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00078DE0 File Offset: 0x00077DE0
		private void mnu2AccessPrivilege_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "Basic.frmPrivileges");
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00078DF4 File Offset: 0x00077DF4
		private void mnu2AdjustTime_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnSetTime.PerformClick();
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00078E40 File Offset: 0x00077E40
		private void mnu2AntiPassback_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.dfrmControllerAntiPassback");
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00078E53 File Offset: 0x00077E53
		private void mnu2Beginner_Click(object sender, EventArgs e)
		{
			this.grpGettingStarted.Visible = true;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00078E61 File Offset: 0x00077E61
		private void mnu2Camera_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new dfrmCameraManage());
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00078E70 File Offset: 0x00077E70
		private void mnu2CardLost_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmUsers");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicConfig, "Basic.frmUsers");
				form = this.getCurrentForm(".frmUsers");
			}
			if (form != null)
			{
				(form as frmUsers).btnRegisterLostCard.PerformClick();
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00078EBC File Offset: 0x00077EBC
		private void mnu2Check_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnCheck.PerformClick();
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00078F08 File Offset: 0x00077F08
		private void mnu2CheckAccess_Click(object sender, EventArgs e)
		{
			this.dfrmCheckAccessConfigure1 = new dfrmCheckAccessConfigure();
			this.dispDfrm(this.dfrmCheckAccessConfigure1);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00078F21 File Offset: 0x00077F21
		private void mnu2Console_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00078F34 File Offset: 0x00077F34
		private void mnu2ConstMeal_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new frmMeal());
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00078F41 File Offset: 0x00077F41
		private void mnu2Departments_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "Basic.frmDepartments");
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00078F54 File Offset: 0x00077F54
		private void mnu2DoorAsSwitch_Click(object sender, EventArgs e)
		{
			dfrmDoorAsSwitch dfrmDoorAsSwitch = new dfrmDoorAsSwitch();
			this.dispDfrm(dfrmDoorAsSwitch);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00078F70 File Offset: 0x00077F70
		private void mnu2EditOperator_Click(object sender, EventArgs e)
		{
			if (this.panel4Form.Controls.Count > 0)
			{
				int count = this.panel4Form.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					(this.panel4Form.Controls[count - i - 1] as Form).Close();
				}
			}
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (dfrmOperatorUpdate dfrmOperatorUpdate = new dfrmOperatorUpdate())
				{
					dfrmOperatorUpdate.operateMode = 1;
					dfrmOperatorUpdate.operatorID = icOperator.OperatorID;
					dfrmOperatorUpdate.operatorName = icOperator.OperatorName;
					dfrmOperatorUpdate.Text = string.Format("{0}--{1}", dfrmOperatorUpdate.Text, icOperator.OperatorName);
					if (dfrmOperatorUpdate.ShowDialog(this) == DialogResult.OK && XMessageBox.Show(this, CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
					{
						wgAppConfig.gRestart = true;
						this.mnuExit.PerformClick();
					}
				}
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0007906C File Offset: 0x0007806C
		private void mnu2FirstCardOpen_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.dfrmControllerFirstCard");
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00079080 File Offset: 0x00078080
		private void mnu2GetRecords_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnGetRecords.PerformClick();
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000790CC File Offset: 0x000780CC
		private void mnu2GlobalAntipassback_Click(object sender, EventArgs e)
		{
			dfrmGlobalAntiBackManagement dfrmGlobalAntiBackManagement = new dfrmGlobalAntiBackManagement();
			this.dispDfrm(dfrmGlobalAntiBackManagement);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000790E6 File Offset: 0x000780E6
		private void mnu2InterLock_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.dfrmControllerInterLock");
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000790FC File Offset: 0x000780FC
		private void mnu2LimitedAccessTimes_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmControlSegs");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicConfig, "Basic.frmControlSegs");
				form = this.getCurrentForm(".frmControlSegs");
			}
			if (form != null)
			{
				(form as frmControlSegs).btnEdit.PerformClick();
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00079148 File Offset: 0x00078148
		private void mnu2Locate_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnLocate.PerformClick();
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00079194 File Offset: 0x00078194
		private void mnu2Manual_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = Environment.CurrentDirectory + "\\Readme.doc",
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000791FC File Offset: 0x000781FC
		private void mnu2Maps_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnMaps.PerformClick();
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00079248 File Offset: 0x00078248
		private void mnu2Meeting_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new frmMeetings());
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00079258 File Offset: 0x00078258
		private void mnu2Monitor_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnServer.PerformClick();
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000792A4 File Offset: 0x000782A4
		private void mnu2MultiCard_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.dfrmControllerMultiCards");
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000792B8 File Offset: 0x000782B8
		private void mnu2OneToMore_Click(object sender, EventArgs e)
		{
			frmUsers4Elevator frmUsers4Elevator = new frmUsers4Elevator();
			frmUsers4Elevator.Text = this.mnuElevator.Text;
			this.dispInPanel4(frmUsers4Elevator);
			if (frmUsers4Elevator.bLoadConsole)
			{
				this.shortcutConsole_Click(null, null);
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000792F3 File Offset: 0x000782F3
		private void mnu2OperatorManagement_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new dfrmOperator());
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00079300 File Offset: 0x00078300
		private void mnu2Option_Click(object sender, EventArgs e)
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (dfrmOption dfrmOption = new dfrmOption())
				{
					dfrmOption.pageIndex = 1;
					if (sender == this.mnu2Language)
					{
						dfrmOption.pageIndex = 1;
					}
					else if (sender == this.mnu2InterfaceTitle)
					{
						dfrmOption.pageIndex = 2;
					}
					else if (sender == this.mnu2hideGettingStartedToolStripMenuItem)
					{
						dfrmOption.pageIndex = 2;
					}
					else if (sender == this.mnu2AutoLogin)
					{
						dfrmOption.pageIndex = 3;
					}
					else if (sender == this.mnu2ExtendedFunction)
					{
						dfrmOption.pageIndex = 4;
					}
					if (dfrmOption.ShowDialog(this) == DialogResult.OK)
					{
						wgAppConfig.gRestart = true;
						this.mnuExit.PerformClick();
					}
				}
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000793C8 File Offset: 0x000783C8
		private void mnu2PasswordManagement_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.dfrmControllerExtendFuncPasswordManage");
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000793DB File Offset: 0x000783DB
		private void mnu2Patrol_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new frmPatrolReport());
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000793E8 File Offset: 0x000783E8
		private void mnu2PeripheralControl_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.dfrmControllerWarnSet");
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000793FC File Offset: 0x000783FC
		private void mnu2PersonInside_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnPersonInside.PerformClick();
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00079448 File Offset: 0x00078448
		private void mnu2Personnel_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "Basic.frmUsers");
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0007945B File Offset: 0x0007845B
		private void mnu2privilegeTypesManagementToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.PrivilegeType.frmPrivilegeTypeManagement");
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0007946E File Offset: 0x0007846E
		private void mnu2QuerySwipeRecords_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicOperate, "Basic.frmSwipeRecords");
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00079484 File Offset: 0x00078484
		private void mnu2RealtimeGetRecords_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnRealtimeGetRecords.PerformClick();
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000794D0 File Offset: 0x000784D0
		private void mnu2RemoteOpen_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnRemoteOpen.PerformClick();
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0007951C File Offset: 0x0007851C
		private void mnu2ResetPersonInside_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).resetPersonInsideToolStripMenuItem.PerformClick();
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00079568 File Offset: 0x00078568
		private void mnu2SetDoorControl_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).setDoorControlToolStripMenuItem.PerformClick();
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000795B4 File Offset: 0x000785B4
		private void mnu2SystemCharacteristic_Click(object sender, EventArgs e)
		{
			try
			{
				this.dispDfrm(null);
				if (this.panel4Form.Controls.Count <= 0)
				{
					string text = icOperator.PCSysInfo(false);
					XMessageBox.Show(text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					string text2 = "";
					string text3 = "";
					wgAppConfig.setSystemParamValue(38, text2, text3, text);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0007963C File Offset: 0x0007863C
		private void mnu2TaskList_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "ExtendFunc.dfrmControllerTaskList");
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0007964F File Offset: 0x0007864F
		private void mnu2TimeProfile_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "Basic.frmControlSegs");
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00079664 File Offset: 0x00078664
		private void mnu2Upload_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).btnUpload.PerformClick();
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000796B0 File Offset: 0x000786B0
		private void mnu2WarnOutputReset_Click(object sender, EventArgs e)
		{
			Form form = this.getCurrentForm(".frmConsole");
			if (form == null)
			{
				this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
				form = this.getCurrentForm(".frmConsole");
			}
			if (form != null)
			{
				(form as frmConsole).mnuWarnOutputReset.PerformClick();
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000796FC File Offset: 0x000786FC
		private void mnu2Zones_Click(object sender, EventArgs e)
		{
			this.frmZones1 = new frmZones();
			this.dispDfrm(this.frmZones1);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00079715 File Offset: 0x00078715
		private void mnuAbout_Click(object sender, EventArgs e)
		{
			this.dfrmAbout1 = new dfrmAbout();
			this.dfrmAbout1.Owner = this;
			this.dispDfrm(this.dfrmAbout1);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0007973A File Offset: 0x0007873A
		private void mnuAttendenceData_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.frmShiftAttReport");
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0007974D File Offset: 0x0007874D
		private void mnuCameraManage_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new dfrmCameraManage());
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0007975A File Offset: 0x0007875A
		private void mnuDBBackup_Click(object sender, EventArgs e)
		{
			this.dfrmDbCompact1 = new dfrmDbCompact();
			this.dfrmDbCompact1.callForm = this;
			this.dispDfrm(this.dfrmDbCompact1);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00079780 File Offset: 0x00078780
		private void mnuDeleteOldRecords_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			using (dfrmDeleteRecords dfrmDeleteRecords = new dfrmDeleteRecords())
			{
				dfrmDeleteRecords.ShowDialog(this);
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00079818 File Offset: 0x00078818
		private void mnuDoorAsSwitch_Click(object sender, EventArgs e)
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (dfrmDoorAsSwitch dfrmDoorAsSwitch = new dfrmDoorAsSwitch())
				{
					dfrmDoorAsSwitch.ShowDialog();
				}
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00079868 File Offset: 0x00078868
		private void mnuElevator_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new frmUsers4Elevator
			{
				Text = this.mnuElevator.Text
			});
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00079893 File Offset: 0x00078893
		private void mnuExit_Click(object sender, EventArgs e)
		{
			frmADCT3000.bConfirmClose = true;
			wgAppConfig.wgLog(this.mnuExit.Text, EventLogEntryType.Information, null);
			base.Close();
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000798B4 File Offset: 0x000788B4
		private void mnuExtendedFunction_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.setPasswordChar('*');
				dfrmInputNewName.Text = CommonStr.strInputExtendFunctionPassword;
				dfrmInputNewName.label1.Text = CommonStr.strExtendFunctionPassword;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (dfrmExtendedFunctions dfrmExtendedFunctions = new dfrmExtendedFunctions())
				{
					if (dfrmExtendedFunctions.ShowDialog(this) == DialogResult.OK)
					{
						wgAppConfig.gRestart = true;
						this.mnuExit.PerformClick();
					}
				}
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00079978 File Offset: 0x00078978
		private void mnuHolidaySet_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.dfrmHolidaySet");
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0007998C File Offset: 0x0007898C
		private void mnuInterfaceLock_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_InterfaceLock", "1");
			this.interfaceLockToolStripMenuItem.Visible = true;
			using (dfrmInterfaceLock dfrmInterfaceLock = new dfrmInterfaceLock())
			{
				dfrmInterfaceLock.txtOperatorName.Text = icOperator.OperatorName;
				dfrmInterfaceLock.StartPosition = FormStartPosition.CenterScreen;
				dfrmInterfaceLock.ShowDialog(this);
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000799F8 File Offset: 0x000789F8
		private void mnuLeave_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.frmLeave");
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00079A0B File Offset: 0x00078A0B
		private void mnuLogQuery_Click(object sender, EventArgs e)
		{
			this.dfrmLogQuery1 = new dfrmLogQuery();
			this.dispDfrm(this.dfrmLogQuery1);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00079A24 File Offset: 0x00078A24
		private void mnuManual_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = Environment.CurrentDirectory + "\\Readme.doc",
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00079A8C File Offset: 0x00078A8C
		private void mnuManualCardRecord_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.frmManualSwipeRecords");
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00079A9F File Offset: 0x00078A9F
		private void mnuMeal_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new frmMeal());
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00079AAC File Offset: 0x00078AAC
		private void mnuMeetingSign_Click(object sender, EventArgs e)
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (frmMeetings frmMeetings = new frmMeetings())
				{
					frmMeetings.ShowDialog();
				}
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00079AFC File Offset: 0x00078AFC
		private void mnuOption_Click(object sender, EventArgs e)
		{
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (dfrmOption dfrmOption = new dfrmOption())
				{
					if (dfrmOption.ShowDialog(this) == DialogResult.OK)
					{
						wgAppConfig.gRestart = true;
						this.mnuExit.PerformClick();
					}
				}
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00079B60 File Offset: 0x00078B60
		private void mnuPatrol_Click(object sender, EventArgs e)
		{
			this.dispInPanel4(new frmPatrolReport());
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00079B6D File Offset: 0x00078B6D
		private void mnuPCCheckAccessConfigure_Click(object sender, EventArgs e)
		{
			this.dfrmCheckAccessConfigure1 = new dfrmCheckAccessConfigure();
			this.dispDfrm(this.dfrmCheckAccessConfigure1);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00079B86 File Offset: 0x00078B86
		private void mnuShiftArrange_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.frmShiftOtherData");
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00079B99 File Offset: 0x00078B99
		private void mnuShiftNormalConfigure_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.dfrmShiftNormalParamSet");
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00079BAC File Offset: 0x00078BAC
		private void mnuShiftRule_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.dfrmShiftOtherParamSet");
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00079BBF File Offset: 0x00078BBF
		private void mnuShiftSet_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.frmShiftOtherTypes");
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00079BD4 File Offset: 0x00078BD4
		private void mnuSystemCharacteristic_Click(object sender, EventArgs e)
		{
			try
			{
				this.dispDfrm(null);
				if (this.panel4Form.Controls.Count <= 0)
				{
					string text = icOperator.PCSysInfo(false);
					XMessageBox.Show(text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					string text2 = "";
					string text3 = "";
					wgAppConfig.setSystemParamValue(38, text2, text3, text);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00079C5C File Offset: 0x00078C5C
		private void mnuTaskList_Click(object sender, EventArgs e)
		{
			this.dfrmControllerTaskList1 = new dfrmControllerTaskList();
			this.dispDfrm(this.dfrmControllerTaskList1);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00079C78 File Offset: 0x00078C78
		public static string MulGetHardwareInfo(frmADCT3000.HardwareEnum hardType, string propKey)
		{
			string text = "";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from " + hardType);
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				if (managementBaseObject.Properties[propKey] != null)
				{
					text = text + "," + managementBaseObject.Properties[propKey].Value.ToString();
				}
			}
			return text;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00079D0C File Offset: 0x00078D0C
		private void Nbi_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00079D18 File Offset: 0x00078D18
		private void openContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				string startupPath = Application.StartupPath;
				Process.Start("explorer.exe", startupPath);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00079D58 File Offset: 0x00078D58
		private void produceUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmUpdateOnline dfrmUpdateOnline = new dfrmUpdateOnline())
			{
				dfrmUpdateOnline.ShowDialog(this);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00079D90 File Offset: 0x00078D90
		private void shortcutAttendance_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconAttendance, "Reports.Shift.frmShiftAttReport");
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00079DA3 File Offset: 0x00078DA3
		public void shortcutConsole_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicOperate, "Basic.frmConsole");
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00079DB6 File Offset: 0x00078DB6
		private void shortcutControllers_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "Basic.frmControllers");
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00079DC9 File Offset: 0x00078DC9
		private void shortcutPersonnel_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "Basic.frmUsers");
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00079DDC File Offset: 0x00078DDC
		private void shortcutPrivilege_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicConfig, "Basic.frmPrivileges");
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00079DEF File Offset: 0x00078DEF
		private void shortcutSwipe_Click(object sender, EventArgs e)
		{
			this.buttonClick(this.btnIconBasicOperate, "Basic.frmSwipeRecords");
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00079E04 File Offset: 0x00078E04
		private void sleep4TaskShedule()
		{
			DateTime now = DateTime.Now;
			if (this.tmStart.AddDays(1.0) >= DateTime.Now)
			{
				if (now.Millisecond > 0)
				{
					Thread.Sleep(1000 - now.Millisecond);
				}
				Thread.Sleep((this.iLoginCnt % 10 + 1) * 100);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00079E68 File Offset: 0x00078E68
		private void statRunInfo_CommStatus_Update(string strCommStatus)
		{
			try
			{
				this.statRuninfo1.Text = strCommStatus;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00079E98 File Offset: 0x00078E98
		private void statRunInfo_Monitor_Update(string strMonitor)
		{
			try
			{
				if (strMonitor == null)
				{
					this.statRuninfo2.Text = strMonitor;
				}
				else if (strMonitor == "0")
				{
					this.statRuninfo2.BackColor = Color.Transparent;
					this.statRuninfo2.Text = CommonStr.strMonitorStop;
				}
				else if (strMonitor == "1")
				{
					this.statRuninfo2.Text = CommonStr.strMonitoring;
				}
				else if (strMonitor == "2")
				{
					this.statRuninfo2.Text = CommonStr.strRealtimeGetting;
				}
				else
				{
					this.statRuninfo2.Text = strMonitor;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00079F44 File Offset: 0x00078F44
		private void statRunInfo_Num_Update(string strLoadNum)
		{
			try
			{
				this.statRuninfoLoadedNum.Text = strLoadNum;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00079F74 File Offset: 0x00078F74
		private void systemParamsCustomTitle()
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			using (dfrmInputNewName dfrmInputNewName2 = new dfrmInputNewName())
			{
				dfrmInputNewName2.Text = CommonStr.strNewTitle;
				dfrmInputNewName2.bNotAllowNull = false;
				if (dfrmInputNewName2.ShowDialog(this) == DialogResult.OK && wgAppConfig.setSystemParamValue(17, "", wgTools.SetObjToStr(dfrmInputNewName2.strNewName).Trim(), "") > 0)
				{
					if (wgAppConfig.getSystemParamByName("Custom Title") != "")
					{
						this.Text = wgAppConfig.getSystemParamByName("Custom Title");
						this.oldTitle = this.Text;
						wgAppConfig.UpdateKeyVal("Custom Title", this.Text);
						wgAppConfig.SaveNewXmlFile("Custom Title", this.Text);
					}
					else
					{
						this.Text = this.defaultTitle;
					}
				}
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0007A0AC File Offset: 0x000790AC
		private void systemParamsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			using (dfrmSystemParam dfrmSystemParam = new dfrmSystemParam())
			{
				if (dfrmSystemParam.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.gRestart = true;
					this.mnuExit.PerformClick();
				}
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0007A158 File Offset: 0x00079158
		private void taskShedule4Controller()
		{
			if (wgTools.bUDPCloud > 0)
			{
				this.taskShedule4Controller4Cloud();
				return;
			}
			DateTime now = DateTime.Now;
			if (this.tmLasttaskShedule4Controller > now)
			{
				wgAppConfig.UpdateKeyVal("AutoUpdateTimeLast", "");
				wgAppConfig.UpdateKeyVal("AutoGetSwipeRecordsLast", "");
				wgAppConfig.UpdateKeyVal("AutoUploadPrivilegesLast", "");
				wgAppConfig.UpdateKeyVal("AutoUploadConfigureLast", "");
				this.iLastTaskMin = -1;
			}
			this.tmLasttaskShedule4Controller = now;
			if (this.iLastTaskMin != now.Minute)
			{
				this.iLastTaskMin = now.Minute;
				try
				{
					string keyVal = wgAppConfig.GetKeyVal("AutoUpdateTime");
					if (!string.IsNullOrEmpty(keyVal) && keyVal.IndexOf(now.ToString("HH:mm")) >= 0)
					{
						this.sleep4TaskShedule();
						if (!wgAppConfig.GetKeyVal("AutoUpdateTimeLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
						{
							string text = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
							if (!string.IsNullOrEmpty(text))
							{
								text = Program.Dpt4Database(text);
							}
							try
							{
								Process.Start(new ProcessStartInfo
								{
									FileName = Application.StartupPath + "\\N3000.exe",
									Arguments = string.Format(" -USER {0} -PASSWORD '{1}'  -SetTime", wgTools.PrepareStr(icOperator.OperatorName), text),
									UseShellExecute = true
								});
								wgAppConfig.UpdateKeyVal("AutoUpdateTimeLast", now.ToString("yyyy-MM-dd HH:mm"));
								wgAppConfig.wgLog("AutoUpdateTime");
							}
							catch (Exception ex)
							{
								wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
								wgAppConfig.wgLog(ex.ToString());
							}
						}
					}
					string keyVal2 = wgAppConfig.GetKeyVal("AutoGetSwipeRecords");
					if (!string.IsNullOrEmpty(keyVal2) && keyVal2.IndexOf(now.ToString("HH:mm")) >= 0)
					{
						this.sleep4TaskShedule();
						if (!wgAppConfig.GetKeyVal("AutoGetSwipeRecordsLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
						{
							string text2 = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
							if (!string.IsNullOrEmpty(text2))
							{
								text2 = Program.Dpt4Database(text2);
							}
							try
							{
								Process.Start(new ProcessStartInfo
								{
									FileName = Application.StartupPath + "\\N3000.exe",
									Arguments = string.Format(" -USER {0} -PASSWORD '{1}'  -GetRecord", wgTools.PrepareStr(icOperator.OperatorName), text2),
									UseShellExecute = true
								});
								wgAppConfig.UpdateKeyVal("AutoGetSwipeRecordsLast", now.ToString("yyyy-MM-dd HH:mm"));
								wgAppConfig.wgLog("AutoGetSwipeRecords");
							}
							catch (Exception ex2)
							{
								wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
								wgAppConfig.wgLog(ex2.ToString());
							}
						}
					}
					string keyVal3 = wgAppConfig.GetKeyVal("AutoUploadPrivileges");
					if (!string.IsNullOrEmpty(keyVal3) && keyVal3.IndexOf(now.ToString("HH:mm")) >= 0)
					{
						this.sleep4TaskShedule();
						if (!wgAppConfig.GetKeyVal("AutoUploadPrivilegesLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
						{
							string text3 = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
							if (!string.IsNullOrEmpty(text3))
							{
								text3 = Program.Dpt4Database(text3);
							}
							try
							{
								Process.Start(new ProcessStartInfo
								{
									FileName = Application.StartupPath + "\\N3000.exe",
									Arguments = string.Format(" -USER {0} -PASSWORD '{1}'  -UploadPrivilege", wgTools.PrepareStr(icOperator.OperatorName), text3),
									UseShellExecute = true
								});
								wgAppConfig.UpdateKeyVal("AutoUploadPrivilegesLast", now.ToString("yyyy-MM-dd HH:mm"));
								wgAppConfig.wgLog("AutoUploadPrivileges");
							}
							catch (Exception ex3)
							{
								wgTools.WgDebugWrite(ex3.ToString(), new object[] { EventLogEntryType.Error });
								wgAppConfig.wgLog(ex3.ToString());
							}
						}
					}
					string keyVal4 = wgAppConfig.GetKeyVal("AutoUploadConfigure");
					if (!string.IsNullOrEmpty(keyVal4) && keyVal4.IndexOf(now.ToString("HH:mm")) >= 0)
					{
						this.sleep4TaskShedule();
						if (!wgAppConfig.GetKeyVal("AutoUploadConfigureLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
						{
							string text4 = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
							if (!string.IsNullOrEmpty(text4))
							{
								text4 = Program.Dpt4Database(text4);
							}
							try
							{
								Process.Start(new ProcessStartInfo
								{
									FileName = Application.StartupPath + "\\N3000.exe",
									Arguments = string.Format(" -USER {0} -PASSWORD '{1}'  -Configure", wgTools.PrepareStr(icOperator.OperatorName), text4),
									UseShellExecute = true
								});
								wgAppConfig.UpdateKeyVal("AutoUploadConfigureLast", now.ToString("yyyy-MM-dd HH:mm"));
								wgAppConfig.wgLog("AutoUploadConfigure");
							}
							catch (Exception ex4)
							{
								wgTools.WgDebugWrite(ex4.ToString(), new object[] { EventLogEntryType.Error });
								wgAppConfig.wgLog(ex4.ToString());
							}
						}
					}
				}
				catch (Exception ex5)
				{
					wgAppConfig.wgLog(ex5.ToString());
				}
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0007A700 File Offset: 0x00079700
		private void taskShedule4Controller4Cloud()
		{
			if (wgTools.arrSNReceived.Count > 0)
			{
				DateTime now = DateTime.Now;
				if (this.tmLasttaskShedule4Controller > now)
				{
					wgAppConfig.UpdateKeyVal("AutoUpdateTimeLast", "");
					wgAppConfig.UpdateKeyVal("AutoGetSwipeRecordsLast", "");
					wgAppConfig.UpdateKeyVal("AutoUploadPrivilegesLast", "");
					wgAppConfig.UpdateKeyVal("AutoUploadConfigureLast", "");
					this.iLastTaskMin = -1;
				}
				this.tmLasttaskShedule4Controller = now;
				if (this.iLastTaskMin != now.Minute)
				{
					this.iLastTaskMin = now.Minute;
					try
					{
						string keyVal = wgAppConfig.GetKeyVal("AutoUpdateTime");
						if (!string.IsNullOrEmpty(keyVal) && keyVal.IndexOf(now.ToString("HH:mm")) >= 0)
						{
							this.sleep4TaskShedule();
							if (!wgAppConfig.GetKeyVal("AutoUpdateTimeLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
							{
								string text = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
								if (!string.IsNullOrEmpty(text))
								{
									text = Program.Dpt4Database(text);
								}
								try
								{
									if (batchAutoRun.commandSpecialCall(new string[]
									{
										"-USER",
										wgTools.PrepareStr(icOperator.OperatorName),
										"-PASSWORD",
										wgTools.PrepareStr(text),
										"-SetTime"
									}) > 0)
									{
										wgAppConfig.UpdateKeyVal("AutoUpdateTimeLast", now.ToString("yyyy-MM-dd HH:mm"));
										wgAppConfig.wgLog("AutoUpdateTime");
									}
								}
								catch (Exception ex)
								{
									wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
									wgAppConfig.wgLog(ex.ToString());
								}
							}
						}
						string keyVal2 = wgAppConfig.GetKeyVal("AutoGetSwipeRecords");
						if (!string.IsNullOrEmpty(keyVal2) && keyVal2.IndexOf(now.ToString("HH:mm")) >= 0)
						{
							this.sleep4TaskShedule();
							if (!wgAppConfig.GetKeyVal("AutoGetSwipeRecordsLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
							{
								string text2 = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
								if (!string.IsNullOrEmpty(text2))
								{
									text2 = Program.Dpt4Database(text2);
								}
								try
								{
									if (batchAutoRun.commandSpecialCall(new string[]
									{
										"-USER",
										wgTools.PrepareStr(icOperator.OperatorName),
										"-PASSWORD",
										wgTools.PrepareStr(text2),
										"-GetRecord"
									}) > 0)
									{
										wgAppConfig.UpdateKeyVal("AutoGetSwipeRecordsLast", now.ToString("yyyy-MM-dd HH:mm"));
										wgAppConfig.wgLog("AutoGetSwipeRecords");
									}
								}
								catch (Exception ex2)
								{
									wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
									wgAppConfig.wgLog(ex2.ToString());
								}
							}
						}
						string keyVal3 = wgAppConfig.GetKeyVal("AutoUploadPrivileges");
						if (!string.IsNullOrEmpty(keyVal3) && keyVal3.IndexOf(now.ToString("HH:mm")) >= 0)
						{
							this.sleep4TaskShedule();
							if (!wgAppConfig.GetKeyVal("AutoUploadPrivilegesLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
							{
								string text3 = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
								if (!string.IsNullOrEmpty(text3))
								{
									text3 = Program.Dpt4Database(text3);
								}
								try
								{
									if (batchAutoRun.commandSpecialCall(new string[]
									{
										"-USER",
										wgTools.PrepareStr(icOperator.OperatorName),
										"-PASSWORD",
										wgTools.PrepareStr(text3),
										"-UploadPrivilege"
									}) > 0)
									{
										wgAppConfig.UpdateKeyVal("AutoUploadPrivilegesLast", now.ToString("yyyy-MM-dd HH:mm"));
										wgAppConfig.wgLog("AutoUploadPrivileges");
									}
								}
								catch (Exception ex3)
								{
									wgTools.WgDebugWrite(ex3.ToString(), new object[] { EventLogEntryType.Error });
									wgAppConfig.wgLog(ex3.ToString());
								}
							}
						}
						string keyVal4 = wgAppConfig.GetKeyVal("AutoUploadConfigure");
						if (!string.IsNullOrEmpty(keyVal4) && keyVal4.IndexOf(now.ToString("HH:mm")) >= 0)
						{
							this.sleep4TaskShedule();
							if (!wgAppConfig.GetKeyVal("AutoUploadConfigureLast").Equals(now.ToString("yyyy-MM-dd HH:mm")))
							{
								string text4 = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
								if (!string.IsNullOrEmpty(text4))
								{
									text4 = Program.Dpt4Database(text4);
								}
								try
								{
									if (batchAutoRun.commandSpecialCall(new string[]
									{
										"-USER",
										wgTools.PrepareStr(icOperator.OperatorName),
										"-PASSWORD",
										wgTools.PrepareStr(text4),
										"-Configure"
									}) > 0)
									{
										wgAppConfig.UpdateKeyVal("AutoUploadConfigureLast", now.ToString("yyyy-MM-dd HH:mm"));
										wgAppConfig.wgLog("AutoUploadConfigure");
									}
								}
								catch (Exception ex4)
								{
									wgTools.WgDebugWrite(ex4.ToString(), new object[] { EventLogEntryType.Error });
									wgAppConfig.wgLog(ex4.ToString());
								}
							}
						}
					}
					catch (Exception ex5)
					{
						wgAppConfig.wgLog(ex5.ToString());
					}
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0007AC98 File Offset: 0x00079C98
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				this.timer1.Enabled = false;
				this.statTimeDate.Text = DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
				if (wgTools.gbAutoLockInterface)
				{
					int num = 1000;
					if (frmADCT3000.iOperCount > num)
					{
						if (frmADCT3000.iOperCount == num + 1)
						{
							frmADCT3000.iOperCount++;
							this.mnuInterfaceLock_Click(null, null);
						}
					}
					else
					{
						frmADCT3000.iOperCount++;
					}
				}
				if (!wgAppConfig.IsAccessDB)
				{
					if (string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(30)))
					{
						this.cntDBNotConnect++;
					}
					else
					{
						this.cntDBNotConnect = 0;
					}
					if (this.cntDBNotConnect > 5)
					{
						wgAppConfig.DBIsConnected = false;
						this.lblFailedToConnectDB.Visible = (this.cntDBNotConnect & 8) > 0;
					}
					else
					{
						wgAppConfig.DBIsConnected = true;
						this.lblFailedToConnectDB.Visible = false;
					}
				}
				if (wgAppConfig.bForceRestart)
				{
					wgAppConfig.wgLog("Application Restart as Cloud app failed.");
					wgAppConfig.bForceRestart = false;
					Thread.Sleep(200);
					wgAppConfig.gRestart = true;
					this.mnuExit.PerformClick();
				}
				else if (wgTools.bUDPCloudSpecial > 0)
				{
					if (wgAppConfig.glngReceiveAutoUploadCount != this.lastglngReceiveAutoUploadCount)
					{
						this.lastglngReceiveAutoUploadCount = wgAppConfig.glngReceiveAutoUploadCount;
						this.dtglngReceiveAutoUploadCount = DateTime.Now.AddMinutes(5.0);
					}
					else if (DateTime.Now > this.dtglngReceiveAutoUploadCount)
					{
						wgAppConfig.bForceRestart = true;
					}
				}
				else if (wgTools.bUDPNeedCheckNetRunning > 0)
				{
					if (wgAppConfig.glngReceiveCount != this.lastglngReceiveCount)
					{
						this.lastglngReceiveCount = wgAppConfig.glngReceiveCount;
						this.dtglngReceiveCount = DateTime.Now.AddMinutes(5.0);
					}
					else if (DateTime.Now > this.dtglngReceiveCount)
					{
						wgAppConfig.wgLog("bUDPNeedCheckNetRunning  Application Restart as Cloud app failed.");
						wgAppConfig.bForceRestart = true;
					}
				}
				this.taskShedule4Controller();
				this.fingerEnrollSingleUpdate();
				this.count4checkPCDiskspace += 1L;
				if (this.count4checkPCDiskspace >= 80L)
				{
					this.count4checkPCDiskspace = 0L;
					this.checkPCDiskspace();
					if ((int)DateTime.Now.TimeOfDay.TotalMinutes == 180)
					{
						int num2 = 0;
						int.TryParse(wgAppConfig.GetKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH"), out num2);
						if (num2 > 0)
						{
							new Thread(new ThreadStart(Program.deleteOldAviPhoto)).Start();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				this.timer1.Enabled = true;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0007AF30 File Offset: 0x00079F30
		private void toolStrip1BookMark_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			ToolStripButton toolStripButton = e.ClickedItem as ToolStripButton;
			if (this.btnBookmarkSelected == null || toolStripButton != this.btnBookmarkSelected)
			{
				if (this.panel4Form.Controls.Count > 0)
				{
					int count = this.panel4Form.Controls.Count;
					for (int i = 0; i < count; i++)
					{
						(this.panel4Form.Controls[count - i - 1] as Form).Close();
					}
				}
				if (this.panel4Form.Controls.Count <= 0)
				{
					this.btnBookmarkSelected = toolStripButton;
					foreach (object obj in this.toolStrip1BookMark.Items)
					{
						ToolStripButton toolStripButton2 = (ToolStripButton)obj;
						toolStripButton2.BackgroundImage = Resources.pMain_Bookmark_normal;
					}
					toolStripButton.BackgroundImage = Resources.pMain_Bookmark_focus;
					this.closeChildForm();
					Form form = null;
					if (!string.IsNullOrEmpty(wgTools.SetObjToStr(toolStripButton.Tag)))
					{
						form = (Form)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType(toolStripButton.Tag.ToString()));
					}
					if (form != null)
					{
						if (toolStripButton.Tag.ToString().IndexOf(".dfrm") >= 0)
						{
							form.ShowDialog(this);
							this.btnBookmarkSelected = null;
							toolStripButton.BackgroundImage = Resources.pMain_Bookmark_normal;
							return;
						}
						form.Location = new Point(-4, -32);
						form.ControlBox = false;
						form.WindowState = FormWindowState.Normal;
						form.MdiParent = this;
						Cursor.Current = Cursors.WaitCursor;
						form.FormBorderStyle = FormBorderStyle.None;
						form.StartPosition = FormStartPosition.Manual;
						form.Show();
						form.Dock = DockStyle.Fill;
						Cursor.Current = Cursors.Default;
						this.panel4Form.Controls.Add(form);
						if (this.btnIconSelected != null)
						{
							this.btnIconSelected.Select();
						}
					}
				}
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0007B128 File Offset: 0x0007A128
		private void toolStripButtonBookmark1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0007B12A File Offset: 0x0007A12A
		private void toolStripMenuItem20_Click(object sender, EventArgs e)
		{
			this.grpGettingStarted.Visible = true;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0007B138 File Offset: 0x0007A138
		private void toolStripMenuItem29a_Click(object sender, EventArgs e)
		{
			if (sender != null && wgTools.gWGYTJ)
			{
				this.mnu2Option_Click(this.mnu2ExtendedFunction, null);
				return;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.setPasswordChar('*');
				dfrmInputNewName.Text = CommonStr.strInputExtendFunctionPassword;
				dfrmInputNewName.label1.Text = CommonStr.strExtendFunctionPassword;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				if (dfrmInputNewName.strNewName.ToUpper().IndexOf((567800 + DateTime.Now.Day).ToString()) >= 0 || dfrmInputNewName.strNewName.ToUpper().IndexOf((56780 + DateTime.Now.Day).ToString()) >= 0)
				{
					wgAppConfig.wgLog("Clear ConInfo");
					wgAppConfig.setSystemParamValue(48, "ConInfo", "", "------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
					wgAppConfig.setSystemParamValue(64, "special value 7.95", "0", wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
				}
				if (dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			this.dispDfrm(null);
			if (this.panel4Form.Controls.Count <= 0)
			{
				using (dfrmExtendedFunctions dfrmExtendedFunctions = new dfrmExtendedFunctions())
				{
					if (dfrmExtendedFunctions.ShowDialog(this) == DialogResult.OK)
					{
						wgAppConfig.gRestart = true;
						this.mnuExit.PerformClick();
					}
				}
			}
		}

		// Token: 0x040007F8 RID: 2040
		private const int funItemLen = 3;

		// Token: 0x040007F9 RID: 2041
		private const int funNameLoc = 1;

		// Token: 0x040007FA RID: 2042
		private const int funTagLoc = 2;

		// Token: 0x040007FB RID: 2043
		private bool bcheckDiskSpace;

		// Token: 0x040007FC RID: 2044
		private int cntDBNotConnect;

		// Token: 0x040007FD RID: 2045
		private dfrmAbout dfrmAbout1;

		// Token: 0x040007FE RID: 2046
		private dfrmCheckAccessConfigure dfrmCheckAccessConfigure1;

		// Token: 0x040007FF RID: 2047
		private dfrmControllerTaskList dfrmControllerTaskList1;

		// Token: 0x04000800 RID: 2048
		private dfrmDbCompact dfrmDbCompact1;

		// Token: 0x04000801 RID: 2049
		private dfrmLogQuery dfrmLogQuery1;

		// Token: 0x04000802 RID: 2050
		private dfrmNetControllerConfig dfrmNetControllerConfig1;

		// Token: 0x04000803 RID: 2051
		private dfrmOperator dfrmOperator1;

		// Token: 0x04000804 RID: 2052
		private dfrmSetPassword dfrmSetPassword1;

		// Token: 0x04000805 RID: 2053
		private string disknameOfApp;

		// Token: 0x04000806 RID: 2054
		private string disknameOfSql;

		// Token: 0x04000807 RID: 2055
		private DateTime dtglngReceiveAutoUploadCount;

		// Token: 0x04000808 RID: 2056
		private DateTime dtglngReceiveCount;

		// Token: 0x04000809 RID: 2057
		private frmTestController frmTestController1;

		// Token: 0x0400080A RID: 2058
		private frmZones frmZones1;

		// Token: 0x0400080B RID: 2059
		private int iLoginCnt;

		// Token: 0x0400080C RID: 2060
		private long lastglngReceiveAutoUploadCount;

		// Token: 0x0400080D RID: 2061
		private long lastglngReceiveCount;

		// Token: 0x0400080E RID: 2062
		private bool stbStatucModeChange;

		// Token: 0x0400080F RID: 2063
		private Button btnIconSelected;

		// Token: 0x04000810 RID: 2064
		private ToolStripButton btnBookmarkSelected;

		// Token: 0x04000811 RID: 2065
		public static bool bConfirmClose = false;

		// Token: 0x04000812 RID: 2066
		private long count4checkPCDiskspace = 1000L;

		// Token: 0x04000813 RID: 2067
		private string defaultTitle = "";

		// Token: 0x04000814 RID: 2068
		private string[,] functionNameAccessControl = new string[0, 0];

		// Token: 0x04000815 RID: 2069
		private string[,] functionNameAttendence;

		// Token: 0x04000816 RID: 2070
		private string[,] functionNameBasicConfigure;

		// Token: 0x04000817 RID: 2071
		private string[,] functionNameBasicOperate;

		// Token: 0x04000818 RID: 2072
		private string[,] functionNameTool;

		// Token: 0x04000819 RID: 2073
		private int iLastTaskMin;

		// Token: 0x0400081A RID: 2074
		public static int iOperCount = 0;

		// Token: 0x0400081B RID: 2075
		private string oldTitle;

		// Token: 0x0400081C RID: 2076
		public static string portsNotUSB = "";

		// Token: 0x0400081D RID: 2077
		public static Queue qfingerEnrollarrController = new Queue();

		// Token: 0x0400081E RID: 2078
		public static Queue qfingerEnrollInfo = new Queue();

		// Token: 0x0400081F RID: 2079
		private DateTime tmLasttaskShedule4Controller;

		// Token: 0x04000820 RID: 2080
		private DateTime tmStart;

		// Token: 0x0200003F RID: 63
		public enum HardwareEnum
		{
			// Token: 0x040008E8 RID: 2280
			Win32_Processor,
			// Token: 0x040008E9 RID: 2281
			Win32_PhysicalMemory,
			// Token: 0x040008EA RID: 2282
			Win32_Keyboard,
			// Token: 0x040008EB RID: 2283
			Win32_PointingDevice,
			// Token: 0x040008EC RID: 2284
			Win32_FloppyDrive,
			// Token: 0x040008ED RID: 2285
			Win32_DiskDrive,
			// Token: 0x040008EE RID: 2286
			Win32_CDROMDrive,
			// Token: 0x040008EF RID: 2287
			Win32_BaseBoard,
			// Token: 0x040008F0 RID: 2288
			Win32_BIOS,
			// Token: 0x040008F1 RID: 2289
			Win32_ParallelPort,
			// Token: 0x040008F2 RID: 2290
			Win32_SerialPort,
			// Token: 0x040008F3 RID: 2291
			Win32_SerialPortConfiguration,
			// Token: 0x040008F4 RID: 2292
			Win32_SoundDevice,
			// Token: 0x040008F5 RID: 2293
			Win32_SystemSlot,
			// Token: 0x040008F6 RID: 2294
			Win32_USBController,
			// Token: 0x040008F7 RID: 2295
			Win32_NetworkAdapter,
			// Token: 0x040008F8 RID: 2296
			Win32_NetworkAdapterConfiguration,
			// Token: 0x040008F9 RID: 2297
			Win32_Printer,
			// Token: 0x040008FA RID: 2298
			Win32_PrinterConfiguration,
			// Token: 0x040008FB RID: 2299
			Win32_PrintJob,
			// Token: 0x040008FC RID: 2300
			Win32_TCPIPPrinterPort,
			// Token: 0x040008FD RID: 2301
			Win32_POTSModem,
			// Token: 0x040008FE RID: 2302
			Win32_POTSModemToSerialPort,
			// Token: 0x040008FF RID: 2303
			Win32_DesktopMonitor,
			// Token: 0x04000900 RID: 2304
			Win32_DisplayConfiguration,
			// Token: 0x04000901 RID: 2305
			Win32_DisplayControllerConfiguration,
			// Token: 0x04000902 RID: 2306
			Win32_VideoController,
			// Token: 0x04000903 RID: 2307
			Win32_VideoSettings,
			// Token: 0x04000904 RID: 2308
			Win32_TimeZone,
			// Token: 0x04000905 RID: 2309
			Win32_SystemDriver,
			// Token: 0x04000906 RID: 2310
			Win32_DiskPartition,
			// Token: 0x04000907 RID: 2311
			Win32_LogicalDisk,
			// Token: 0x04000908 RID: 2312
			Win32_LogicalDiskToPartition,
			// Token: 0x04000909 RID: 2313
			Win32_LogicalMemoryConfiguration,
			// Token: 0x0400090A RID: 2314
			Win32_PageFile,
			// Token: 0x0400090B RID: 2315
			Win32_PageFileSetting,
			// Token: 0x0400090C RID: 2316
			Win32_BootConfiguration,
			// Token: 0x0400090D RID: 2317
			Win32_ComputerSystem,
			// Token: 0x0400090E RID: 2318
			Win32_OperatingSystem,
			// Token: 0x0400090F RID: 2319
			Win32_StartupCommand,
			// Token: 0x04000910 RID: 2320
			Win32_Service,
			// Token: 0x04000911 RID: 2321
			Win32_Group,
			// Token: 0x04000912 RID: 2322
			Win32_GroupUser,
			// Token: 0x04000913 RID: 2323
			Win32_UserAccount,
			// Token: 0x04000914 RID: 2324
			Win32_Process,
			// Token: 0x04000915 RID: 2325
			Win32_Thread,
			// Token: 0x04000916 RID: 2326
			Win32_Share,
			// Token: 0x04000917 RID: 2327
			Win32_NetworkClient,
			// Token: 0x04000918 RID: 2328
			Win32_NetworkProtocol,
			// Token: 0x04000919 RID: 2329
			Win32_PnPEntity
		}

		// Token: 0x02000040 RID: 64
		internal class MyMessager : IMessageFilter
		{
			// Token: 0x060004B8 RID: 1208 RVA: 0x0007FA95 File Offset: 0x0007EA95
			public bool PreFilterMessage(ref Message m)
			{
				if (m.Msg == 512 || m.Msg == 513 || m.Msg == 516 || m.Msg == 519)
				{
					frmADCT3000.iOperCount = 0;
				}
				return false;
			}
		}
	}
}
