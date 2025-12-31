using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic.MultiThread;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc;
using WG3000_COMM.ExtendFunc.Cloud2017;
using WG3000_COMM.ExtendFunc.Map;
using WG3000_COMM.ExtendFunc.PCCheck;
using WG3000_COMM.ExtendFunc.QR2017;
using WG3000_COMM.ExtendFunc.TCPServer;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000041 RID: 65
	public partial class frmConsole : Form
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x0007FD38 File Offset: 0x0007ED38
		public frmConsole()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvRunInfo);
			wgAppConfig.custDataGridview(ref this.dataGridView2);
			this.splitContainer1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.splitContainer2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.splitContainer2.Panel2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000801CC File Offset: 0x0007F1CC
		private void frmConsole_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.watching != null)
				{
					this.watching.StopWatch();
				}
				if (this.dfrmFind1 != null)
				{
					this.dfrmFind1.ReallyCloseForm();
				}
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
				if (this.frmMoreRecords != null)
				{
					this.frmMoreRecords.ReallyCloseForm();
				}
				if (this.frmWatchingLCD != null)
				{
					this.frmWatchingLCD.ReallyCloseForm();
				}
				if (this.frmMaps1 != null)
				{
					try
					{
						this.frmMaps1.Dispose();
						this.frmMaps1 = null;
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
				}
				if (this.frm4ShowLocate != null)
				{
					try
					{
						this.frm4ShowLocate.Dispose();
						this.frm4ShowLocate = null;
					}
					catch (Exception ex2)
					{
						wgAppConfig.wgLog(ex2.ToString());
					}
				}
				if (this.frm4ShowPersonsInside != null)
				{
					try
					{
						this.frm4ShowPersonsInside.Dispose();
						this.frm4ShowPersonsInside = null;
					}
					catch (Exception ex3)
					{
						wgAppConfig.wgLog(ex3.ToString());
					}
				}
				if (this.frm4PCCheckAccess != null)
				{
					try
					{
						this.frm4PCCheckAccess.Dispose();
						this.frm4PCCheckAccess = null;
					}
					catch (Exception ex4)
					{
						wgAppConfig.wgLog(ex4.ToString());
					}
				}
				if (this.threadpcCheckMealOpen_DealNewRecord != null)
				{
					try
					{
						this.threadpcCheckMealOpen_DealNewRecord.Interrupt();
					}
					catch
					{
					}
				}
				this.control4uploadPrivilege = null;
				this.controlConfigure4uploadPrivilege = null;
				this.controlTaskList4uploadPrivilege = null;
				this.swipe4GetRecords = null;
				wgAppConfig.DisposeImage(this.pictureBox1.Image);
				wgAppRunInfo.raiseAppRunInfoMonitors("");
				this.closeTcpPort();
			}
			catch (Exception ex5)
			{
				wgAppConfig.wgLog(ex5.ToString());
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000803DC File Offset: 0x0007F3DC
		private void frmConsole_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.bMainWindowDisplay && !this.btnRealtimeGetRecords.Enabled && this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.Stop && !frmADCT3000.bConfirmClose && XMessageBox.Show(CommonStr.strSureStopRealtimeGetting, wgTools.MSGTITLE, MessageBoxButtons.OK) == DialogResult.OK)
				{
					e.Cancel = true;
					return;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.bMealStop = true;
			this.bPCCheckMealOpen = false;
			this.bWatchingDoorOpenWarnStop = true;
			this.bWatchingDoorOpenWarn = false;
			this.bPCCheckGlobalAntiBackOpen = false;
			this.bGlobalAntiBackStop = true;
			try
			{
				if (this.photoavi != null)
				{
					if (!this.photoavi.IsDisposed)
					{
						this.photoavi.bWatching = false;
						this.photoavi.stopVideo();
						this.photoavi.Close();
					}
					this.photoavi = null;
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			try
			{
				if (this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.Stop)
				{
					this.btnStopOthers.PerformClick();
				}
				long num = DateTime.Now.Ticks + 150000000L;
				while (DateTime.Now.Ticks < num && this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.Stop)
				{
					Application.DoEvents();
				}
				this.btnStopOthers.PerformClick();
			}
			catch (Exception ex3)
			{
				wgAppConfig.wgLog(ex3.ToString());
			}
			try
			{
				if (this.photoaviSingle != null)
				{
					if (!this.photoaviSingle.IsDisposed)
					{
						this.photoaviSingle.bWatching = false;
						this.photoaviSingle.stopVideo();
						this.photoaviSingle.Close();
					}
					this.photoaviSingle = null;
				}
			}
			catch (Exception ex4)
			{
				wgAppConfig.wgLog(ex4.ToString());
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0008059C File Offset: 0x0007F59C
		public void frmConsole_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (this.frm4PCCheckAccess != null && this.frm4PCCheckAccess.bDealing)
				{
					this.frm4PCCheckAccess.Focus();
				}
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
						this.dfrmFind1.StartPosition = FormStartPosition.Manual;
						this.dfrmFind1.Location = new Point(600, 8);
					}
					this.dfrmFind1.setObjtoFind(this.lstDoors, null);
				}
				if (e.Control && e.KeyValue == 65)
				{
					this.btnSelectAll.PerformClick();
				}
				if (e.Control)
				{
					int keyValue = e.KeyValue;
				}
				if (e.Control && e.KeyValue == 48)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					if (this.btnRemoteOpen.Visible)
					{
						this.btnDirectSetDoorControl();
					}
				}
				if (e.Control && !e.Shift && e.KeyValue == 121)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
					{
						dfrmInputNewName.setPasswordChar('*');
						if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
						{
							return;
						}
						if (dfrmInputNewName.strNewName.ToLower() == "upgrade")
						{
							this.driverUpdateToolStripMenuItem.Visible = true;
							return;
						}
						if (dfrmInputNewName.strNewName != "778899")
						{
							return;
						}
					}
					using (dfrmCommPSet dfrmCommPSet = new dfrmCommPSet())
					{
						dfrmCommPSet.Text = CommonStr.strSaveAsConfigureFile;
						if (dfrmCommPSet.ShowDialog(this) == DialogResult.OK)
						{
							if (string.IsNullOrEmpty(dfrmCommPSet.CurrentPwd))
							{
								wgAppConfig.UpdateKeyVal("CommPCurrent", "");
							}
							else
							{
								wgAppConfig.wgLog(".pCurr_10_" + WGPacket.Ept(dfrmCommPSet.CurrentPwd));
								wgAppConfig.UpdateKeyVal("CommPCurrent", WGPacket.Ept(WGPacket.Ept(dfrmCommPSet.CurrentPwd)));
								wgAppConfig.SaveNewXmlFile("CommPCurrent", WGPacket.Ept(WGPacket.Ept(dfrmCommPSet.CurrentPwd)));
							}
							wgAppConfig.wgLog(".pCurr_" + wgAppConfig.GetKeyVal("CommPCurrent"));
							if (XMessageBox.Show(CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
							{
								this.btnStopOthers_Click(null, null);
								Thread.Sleep(1000);
								wgAppConfig.gRestart = true;
								((frmADCT3000)base.ParentForm).mnuExit.PerformClick();
							}
						}
					}
				}
				if (e.Control && !e.Shift && e.KeyValue == 120)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					using (dfrmCommPSet dfrmCommPSet2 = new dfrmCommPSet())
					{
						dfrmCommPSet2.Text = CommonStr.strSetCommPassword;
						bool flag = false;
						if (dfrmCommPSet2.ShowDialog(this) == DialogResult.OK)
						{
							string keyVal = wgAppConfig.GetKeyVal("CommPCurrent");
							if (string.IsNullOrEmpty(dfrmCommPSet2.CurrentPwd))
							{
								if (string.IsNullOrEmpty(keyVal))
								{
									flag = true;
								}
							}
							else if (string.Compare(WGPacket.Ept(WGPacket.Ept(dfrmCommPSet2.CurrentPwd)), keyVal) == 0)
							{
								flag = true;
							}
							if (!flag)
							{
								XMessageBox.Show(this, CommonStr.strNewPwdNotAsSameInSystem, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
							if (flag)
							{
								Cursor.Current = Cursors.WaitCursor;
								this.UploadCommPassword(string.IsNullOrEmpty(dfrmCommPSet2.CurrentPwd) ? dfrmCommPSet2.CurrentPwd : WGPacket.Ept(dfrmCommPSet2.CurrentPwd), string.IsNullOrEmpty(dfrmCommPSet2.oldPwd) ? dfrmCommPSet2.oldPwd : WGPacket.Ept(dfrmCommPSet2.oldPwd));
								Cursor.Current = Cursors.Default;
							}
						}
					}
				}
				if (e.Control && e.Alt && e.KeyValue == 49)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					using (dfrmInputNewName dfrmInputNewName2 = new dfrmInputNewName())
					{
						dfrmInputNewName2.Text = CommonStr.strRestorLastSwipeRecords;
						dfrmInputNewName2.label1.Text = CommonStr.strInputExtendFunctionPassword;
						dfrmInputNewName2.setPasswordChar('*');
						if (dfrmInputNewName2.ShowDialog(this) != DialogResult.OK || dfrmInputNewName2.strNewName != "5678")
						{
							return;
						}
						this.RestoreAllSwipeInTheControllers();
					}
				}
				if (e.Control && e.Shift && e.KeyValue == 80)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					this.displayTools();
				}
				if (e.Control && e.Shift && e.KeyValue == 81)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					using (dfrmInputNewName dfrmInputNewName3 = new dfrmInputNewName())
					{
						dfrmInputNewName3.setPasswordChar('*');
						if (dfrmInputNewName3.ShowDialog(this) == DialogResult.OK && dfrmInputNewName3.strNewName.ToUpper() == "5678")
						{
							this.quickFormatToolStripMenuItem.Visible = true;
							this.watchingDogConfigureToolStripMenuItem.Visible = true;
						}
					}
				}
				if (e.Control && e.Shift && e.KeyValue == 76)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					if (DateTime.Now > this.dtlstDoorViewChange.AddSeconds(3.0))
					{
						this.dtlstDoorViewChange = DateTime.Now;
						if (this.lstDoors.View == View.Details)
						{
							this.lstDoors.View = View.LargeIcon;
						}
						else if (this.lstDoors.View == View.LargeIcon)
						{
							this.lstDoors.View = View.List;
						}
						else if (this.lstDoors.View == View.List)
						{
							this.lstDoors.View = View.SmallIcon;
						}
						else if (this.lstDoors.View == View.SmallIcon)
						{
							this.lstDoors.View = View.Tile;
						}
						else if (this.lstDoors.View == View.Tile)
						{
							this.lstDoors.View = View.LargeIcon;
						}
						else
						{
							this.lstDoors.View = View.LargeIcon;
						}
						wgTools.WgDebugWrite(this.lstDoors.View.ToString(), new object[0]);
						wgAppConfig.UpdateKeyVal("CONSOLE_DOORVIEW", this.lstDoors.View.ToString());
					}
				}
				if (e.Control && e.KeyValue == 67)
				{
					string text = "";
					for (int i = 0; i < this.dgvRunInfo.Rows.Count; i++)
					{
						for (int j = 0; j < this.dgvRunInfo.ColumnCount; j++)
						{
							text = text + this.dgvRunInfo.Rows[i].Cells[j].Value.ToString().Replace("\r\n", ",") + "\t";
						}
						text += "\r\n";
					}
					try
					{
						Clipboard.SetText(text);
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
						wgAppConfig.wgLogWithoutDB(text, EventLogEntryType.Information, null);
					}
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00080DC0 File Offset: 0x0007FDC0
		private void frmConsole_Load(object sender, EventArgs e)
		{
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
			wgRunInfoLog.eventRecID = 0;
			this.mnuRemoteOpenToolStripMenuItem.Text = wgAppConfig.ReplaceRemoteOpenDoor(this.mnuRemoteOpenToolStripMenuItem.Text);
			this.remoteOpenMultithreadToolStripMenuItem.Text = wgAppConfig.ReplaceRemoteOpenDoor(this.remoteOpenMultithreadToolStripMenuItem.Text);
			this.btnRemoteOpen.Text = wgAppConfig.ReplaceRemoteOpenDoor(this.btnRemoteOpen.Text);
			if (!frmConsole.bCheckRunningSingle4CloudServer && frmConsole.RunningInstance() != null && wgTools.bUDPCloud > 0)
			{
				frmConsole.bCheckRunningSingle4CloudServer = true;
				if (XMessageBox.Show(CommonStr.strCloudServerN3000MoreRunningA + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK && XMessageBox.Show(CommonStr.strCloudServerN3000MoreRunningB + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
				{
					try
					{
						frmConsole.RunningInstance().Kill();
						wgAppConfig.wgLog(CommonStr.strCloudServerN3000MoreRunningC);
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
						XMessageBox.Show(CommonStr.strCloudServerN3000MoreRunningD);
					}
				}
			}
			if (wgAppConfig.FileIsExisted(wgAppConfig.Path4PhotoDefault() + "invalidCard.WAV"))
			{
				this.player = new SoundPlayer();
				this.player.SoundLocation = wgAppConfig.Path4PhotoDefault() + "invalidCard.WAV";
			}
			wgTools.WriteLine("frmConsole_Load Start");
			this.bWarnExisted = false;
			this.btnWarnExisted.Visible = this.bWarnExisted;
			this.bsoftwareWarnAutoResetWhenAllDoorAreClosed = wgAppConfig.getParamValBoolByNO(216);
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.bsoftwareWarnAutoResetWhenAllDoorAreClosed = false;
			}
			if (this.totalConsoleMode == 0)
			{
				this.loadOperatorPrivilege();
			}
			else
			{
				this.btnCheck.Visible = false;
				this.btnSetTime.Visible = false;
				this.btnUpload.Visible = false;
				this.btnServer.Visible = false;
				this.btnGetRecords.Visible = false;
				this.btnRemoteOpen.Visible = false;
				this.mnuCheck.Visible = false;
				switch (this.totalConsoleMode)
				{
				case 1:
					this.btnCheck.Visible = true;
					break;
				case 2:
					this.btnSetTime.Visible = true;
					break;
				case 3:
					this.btnUpload.Visible = true;
					break;
				case 4:
					this.btnServer.Visible = true;
					break;
				case 5:
					this.btnGetRecords.Visible = true;
					break;
				case 6:
					this.btnRemoteOpen.Visible = true;
					break;
				}
			}
			this.mnuCheck.Visible = this.btnCheck.Visible;
			this.mnuRemoteOpenToolStripMenuItem.Visible = this.btnRemoteOpen.Visible;
			this.mnuAdjustTimeToolStripMenuItem.Visible = this.btnSetTime.Visible;
			this.mnuUploadToolStripMenuItem.Visible = this.btnUpload.Visible;
			this.mnuGetRecordsToolStripMenuItem.Visible = this.btnGetRecords.Visible;
			this.mnuMonitorToolStripMenuItem.Visible = this.btnServer.Visible;
			this.setDoorControlToolStripMenuItem.Visible = this.btnUpload.Visible || this.btnRemoteOpen.Visible;
			this.bPCCheckAccess = wgAppConfig.getParamValBoolByNO(137);
			this.loadDoorData();
			this.GetDoorInfoFromDB();
			this.txtInfo.Text = "";
			this.richTxtInfo.Text = "";
			this.lblInfoID.Text = "";
			wgRunInfoLog.init(out this.tbRunInfoLog);
			this.dv = new DataView(this.tbRunInfoLog);
			this.dgvRunInfo.AutoGenerateColumns = false;
			this.dgvRunInfo.DataSource = this.dv;
			this.dgvRunInfo.Columns[0].DataPropertyName = "f_Category";
			this.dgvRunInfo.Columns[1].DataPropertyName = "f_RecID";
			this.dgvRunInfo.Columns[2].DataPropertyName = "f_Time";
			this.dgvRunInfo.Columns[3].DataPropertyName = "f_Desc";
			this.dgvRunInfo.Columns[4].DataPropertyName = "f_Info";
			this.dgvRunInfo.Columns[5].DataPropertyName = "f_Detail";
			this.dgvRunInfo.Columns[6].DataPropertyName = "f_MjRecStr";
			for (int i = 0; i < this.dgvRunInfo.ColumnCount; i++)
			{
				this.dgvRunInfo.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
			}
			this.loadZoneInfo();
			if (!wgAppConfig.getParamValBoolByNO(122))
			{
				this.btnRemoteOpen.Visible = false;
			}
			this.mnuRemoteOpenToolStripMenuItem.Visible = this.btnRemoteOpen.Visible && wgAppConfig.getParamValBoolByNO(122);
			if (!wgAppConfig.getParamValBoolByNO(114))
			{
				this.btnMaps.Visible = false;
			}
			this.btnLocate.Visible = wgAppConfig.getParamValBoolByNO(161);
			this.btnPersonInside.Visible = wgAppConfig.getParamValBoolByNO(162);
			if (!wgAppConfig.IsAccessControlBlue)
			{
				this.btnDownloadPrivileges.Visible = wgAppConfig.getParamValBoolByNO(178);
			}
			this.mnuWarnOutputReset.Visible = wgAppConfig.getParamValBoolByNO(124);
			if (icOperator.OperatorID == 1)
			{
				this.resetPersonInsideToolStripMenuItem.Visible = wgAppConfig.getParamValBoolByNO(132);
				this.allowAntiPassbackFirstExitToolStripMenuItem.Visible = wgAppConfig.getParamValBoolByNO(132);
			}
			frmConsole.infoRowsCount = 0;
			this.strRealMonitor = this.btnServer.Text;
			this.oldInfoTitleString = this.dataGridView2.Columns[0].HeaderText;
			this.checkVideoCH();
			if (wgAppConfig.IsAcceleratorActive)
			{
				this.remoteOpenMultithreadToolStripMenuItem.Visible = this.btnRemoteOpen.Visible;
				this.uploadMultithreadToolStripMenuItem.Visible = this.btnGetRecords.Visible;
				this.downloadMultithreadToolStripMenuItem.Visible = this.btnUpload.Visible;
				this.setDoorControlToolStripMenuItem1.Visible = this.btnUpload.Visible || this.btnRemoteOpen.Visible;
			}
			else
			{
				this.multithreadToolStripMenuItem.Visible = false;
			}
			this.bPCCheckMealOpen = wgAppConfig.getParamValBoolByNO(169);
			this.bPCCheckGlobalAntiBackOpen = wgAppConfig.getParamValBoolByNO(181);
			if (wgAppConfig.getParamValBoolByNO(185))
			{
				wgMjControllerSwipeRecord.bRemoteOpenBiDirection = 1;
			}
			else if (wgMjControllerSwipeRecord.bRemoteOpenBiDirection > 0)
			{
				wgAppConfig.setSystemParamValue(185, "Activate Remote Open Direction", "1", "Remote Open Direction V6.62 2015-12-04 12:43:32");
			}
			this.disablePCControlToolStripMenuItem.Visible = this.bPCCheckMealOpen || this.bPCCheckGlobalAntiBackOpen;
			MjRec.bCertIDEnabled = this.bActivateDisplayCertID;
			this.loadConsumer4TooManyRecordOfAccess();
			this.timerAutoLogin.Enabled = true;
			wgTools.WriteLine("frmConsole_Load End");
			if (wgAppConfig.IsAccessControlBlue)
			{
				if (wgAppConfig.getParamValBoolByNO(64))
				{
					XMessageBox.Show(CommonStr.strSupposeUpgradeDriver);
					try
					{
						Thread.Sleep(500);
						Environment.Exit(0);
						Process.GetCurrentProcess().Kill();
					}
					catch (Exception)
					{
					}
				}
				this.normalOpenTimeListToolStripMenuItem.Visible = false;
			}
			else if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(64)).Equals("2"))
			{
				XMessageBox.Show(CommonStr.strSupposeUpgradeDriver);
				try
				{
					Thread.Sleep(500);
					Environment.Exit(0);
					Process.GetCurrentProcess().Kill();
				}
				catch (Exception)
				{
				}
			}
			frmWatchingLED.getLedConfig();
			this.activateWarnAvi = wgAppConfig.GetKeyVal("KEY_Display4WarnEvent") == "1";
			wgMjControllerRunInformation.activeInvalidCardTimeout = int.Parse("0" + wgAppConfig.getSystemParamByNO(206));
			if (wgTools.bUDPOnly64 > 0)
			{
				this.btnRealtimeGetRecords.Visible = false;
			}
			if (icOperator.OperatorID == 1)
			{
				this.displayCloudControllersToolStripMenuItem.Visible = wgTools.bUDPCloud > 0;
			}
			this.lCDShowToolStripMenuItem.Visible = wgAppConfig.GetKeyVal("Language") == "zh-CHS";
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000815A4 File Offset: 0x000805A4
		private void frmConsole_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.toolTip1.Active)
			{
				this.toolTip1.Active = false;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000815BF File Offset: 0x000805BF
		private void btnWarnExisted_Click(object sender, EventArgs e)
		{
			this.bWarnExisted = false;
			this.btnWarnExisted.Visible = this.bWarnExisted;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000815DC File Offset: 0x000805DC
		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			foreach (object obj in this.lstDoors.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				listViewItem.Selected = true;
			}
			this.lstDoors.Focus();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00081648 File Offset: 0x00080648
		private void btnServer_Click(object sender, EventArgs e)
		{
			if (this.btnServer.Enabled)
			{
				if (this.lstDoors.SelectedItems.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strSelectDoor);
					return;
				}
				if (DateTime.Now.Subtract(this.dtTimeStop).Milliseconds > 300)
				{
					lock (this.QueRecText.SyncRoot)
					{
						this.QueRecText.Clear();
					}
				}
				if (this.QueRecText.Count > 200)
				{
					lock (this.QueRecText.SyncRoot)
					{
						this.QueRecText.Clear();
					}
				}
				if (this.bPCCheckMealOpen)
				{
					lock (this.qMjRec4MealOpen.SyncRoot)
					{
						this.qMjRec4MealOpen.Clear();
					}
				}
				if (this.bPCCheckGlobalAntiBackOpen)
				{
					lock (this.qMjRec4GlobalAntiBack.SyncRoot)
					{
						this.qMjRec4GlobalAntiBack.Clear();
					}
				}
				wgAppConfig.GetKeyIntVal("KEY_P64_GPRS_REFRESHCYCLEMAX", ref wgTools.p64_gprs_refreshCycleMax);
				wgAppConfig.GetKeyIntVal("KEY_P64_GPRS_WATCHINGSENDCYCLE", ref wgTools.p64_gprs_watchingSendCycle);
				if (this.watching == null)
				{
					if (wgTools.bUDPOnly64 > 0)
					{
						this.watching = frmADCT3000.watchingP64;
					}
					else
					{
						this.watching = new WatchingService();
					}
					this.watching.EventHandler += this.evtNewInfoCallBack;
				}
				this.timerUpdateDoorInfo.Enabled = false;
				Cursor.Current = Cursors.WaitCursor;
				this.watchingStartTime = DateTime.Now;
				Dictionary<int, icController> dictionary = new Dictionary<int, icController>();
				this.arrSelectedControllers.Clear();
				foreach (object obj in this.lstDoors.Items)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					(listViewItem.Tag as frmConsole.DoorSetInfo).Selected = 0;
				}
				foreach (object obj2 in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem2 = (ListViewItem)obj2;
					(listViewItem2.Tag as frmConsole.DoorSetInfo).Selected = 1;
					if (!dictionary.ContainsKey((listViewItem2.Tag as frmConsole.DoorSetInfo).ControllerSN))
					{
						wgTools.WriteLine("!selectedControllers.ContainsKey(control.ControllerSN)");
						this.control4btnServer = new icController();
						this.control4btnServer.GetInfoFromDBByDoorNameSpecial(listViewItem2.Text, this.dsWatchingDoorInfo);
						dictionary.Add(this.control4btnServer.ControllerSN, this.control4btnServer);
						this.arrSelectedControllers.Add(this.control4btnServer.ControllerSN);
					}
				}
				if (dictionary.Count > 0)
				{
					wgTools.WriteLine("selectedControllers.Count=" + dictionary.Count.ToString());
					this.watching.WatchingController = dictionary;
					this.timerUpdateDoorInfo.Interval = 300;
					this.timerUpdateDoorInfo.Enabled = true;
					wgAppRunInfo.raiseAppRunInfoMonitors("1");
					if (this.lstDoors.SelectedItems.Count == this.lstDoors.Items.Count)
					{
						this.lstDoors.SelectedItems.Clear();
					}
				}
				else
				{
					wgTools.WriteLine("selectedControllers.Count=" + dictionary.Count.ToString());
					this.watching.WatchingController = null;
					this.timerUpdateDoorInfo.Enabled = false;
					wgAppRunInfo.raiseAppRunInfoMonitors("0");
				}
				(sender as ToolStripButton).BackColor = Color.Green;
				this.btnStopMonitor.BackColor = Color.Red;
				this.btnStopOthers.BackColor = Color.Red;
				(sender as ToolStripButton).Text = CommonStr.strMonitoring;
				if (wgAppConfig.IsActivateCameraManage && wgAppConfig.GetKeyVal("KEY_Video_DontCaputreOnThisPC") != "1")
				{
					this.selectedCameraID = this.selectDoorCamera();
					this.openCamera();
				}
				if (this.bPCCheckMealOpen)
				{
					this.pcCheckMealOpen_Init();
				}
				this.pcWatchingDoorOpenWarn_Init();
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00081AC4 File Offset: 0x00080AC4
		private void btnStopOthers_Click(object sender, EventArgs e)
		{
			if (this.watching != null)
			{
				this.watching.WatchingController = null;
				this.timerUpdateDoorInfo.Enabled = false;
				wgAppRunInfo.raiseAppRunInfoMonitors("0");
			}
			this.bStopComm = true;
			this.dtTimeStop = DateTime.Now;
			this.bMealStop = true;
			this.bWatchingDoorOpenWarnStop = true;
			this.bGlobalAntiBackStop = true;
			wgMjControllerPrivilege.StopUpload();
			wgMjControllerSwipeOperate.StopGetRecord();
			if (this.bkUploadAndGetRecords.IsBusy)
			{
				this.bkUploadAndGetRecords.CancelAsync();
			}
			lock (this.QueRecText.SyncRoot)
			{
				this.QueRecText.Clear();
			}
			if (this.bPCCheckMealOpen)
			{
				lock (this.qMjRec4MealOpen.SyncRoot)
				{
					this.qMjRec4MealOpen.Clear();
				}
			}
			if (this.bPCCheckGlobalAntiBackOpen)
			{
				lock (this.qMjRec4GlobalAntiBack.SyncRoot)
				{
					this.qMjRec4GlobalAntiBack.Clear();
				}
			}
			this.btnServer.BackColor = Color.Transparent;
			this.btnServer.Text = this.strRealMonitor;
			Interlocked.Exchange(ref frmConsole.dealingTxt, 0);
			this.btnRealtimeGetRecords.Enabled = true;
			this.btnStopOthers.BackColor = Color.Transparent;
			this.btnStopMonitor.BackColor = Color.Transparent;
			this.btnGetRecords.Enabled = true;
			this.mnuGetRecordsToolStripMenuItem.Enabled = true;
			this.btnUpload.Enabled = true;
			this.mnuUploadToolStripMenuItem.Enabled = true;
			this.btnServer.Enabled = true;
			this.mnuMonitorToolStripMenuItem.Enabled = true;
			this.btnStopOthers.BackColor = Color.Transparent;
			this.btnStopMonitor.BackColor = Color.Transparent;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00081D40 File Offset: 0x00080D40
		private void btnCheck_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			if (!wgTools.checkNetLinked())
			{
				XMessageBox.Show(CommonStr.strPCNotConnected);
				return;
			}
			bool flag = false;
			long commTimeoutMsMin = wgUdpComm.CommTimeoutMsMin;
			try
			{
				string text = this.lstDoors.SelectedItems[0].Text;
				int num = 0;
				int num2 = 0;
				wgAppConfig.getValBySql("SELECT MAX(t_b_Consumer_Fingerprint.f_FingerNO) From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer.f_CardNO>0 AND t_b_Consumer_Fingerprint.f_ConsumerID = t_b_Consumer.f_ConsumerID ");
				int valBySql = wgAppConfig.getValBySql("SELECT COUNT(t_b_Consumer_Fingerprint.f_FingerNO) From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer.f_CardNO>0 AND t_b_Consumer_Fingerprint.f_ConsumerID = t_b_Consumer.f_ConsumerID ");
				this.control4Check = new icController();
				this.bStopComm = false;
				byte[] array = new byte[2048];
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (this.bStopComm)
					{
						break;
					}
					this.control4Check.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					num = this.control4Check.ControllerID;
					DateTime now = DateTime.Now;
					num2 = 0;
					num2++;
					wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
					if (this.control4Check.GetControllerRunInformationIP(-1) <= 0)
					{
						wgRunInfoLog.addEventNotConnect(this.control4Check.ControllerSN, this.control4Check.IP, listViewItem);
						if (!string.IsNullOrEmpty(wgTools.CommPStr))
						{
							num2++;
							if (this.control4Check.GetControlDataNoPasswordIP(197504, 1427898384, ref array) > 0)
							{
								string strNetControlWrongPassword = CommonStr.strNetControlWrongPassword;
								wgAppConfig.wgLog(string.Format("{0}   {1}", this.control4Check.ControllerSN.ToString(), strNetControlWrongPassword));
								XMessageBox.Show(string.Format("{0}   {1}", this.control4Check.ControllerSN.ToString(), strNetControlWrongPassword));
								return;
							}
						}
						this.control4Check.Dispose();
						this.control4Check = null;
						this.control4Check = new icController();
					}
					else
					{
						if (this.control4Check.runinfo.netSpeedCode != 0)
						{
							wgUdpComm.CommTimeoutMsMin = 2000L;
						}
						flag = true;
						num2++;
						if (wgTools.bUDPOnly64 > 0)
						{
							InfoRow infoRow = new InfoRow();
							infoRow.desc = string.Format("{0}[{1:d}]", listViewItem.Text, this.control4Check.ControllerSN);
							infoRow.information = "";
							infoRow.detail = listViewItem.Text;
							infoRow.detail += string.Format("\r\n{0}:\t{1}", CommonStr.strDoorStatus, this.control4Check.runinfo.IsOpen(this.control4Check.GetDoorNO(listViewItem.Text)) ? CommonStr.strDoorStatus_Open : CommonStr.strDoorStatus_Closed);
							infoRow.information += string.Format("{0};", this.control4Check.runinfo.IsOpen(this.control4Check.GetDoorNO(listViewItem.Text)) ? CommonStr.strDoorStatus_Open : CommonStr.strDoorStatus_Closed);
							infoRow.detail += string.Format("\r\n{0}:\t{1:d}", CommonStr.strControllerSN, this.control4Check.ControllerSN);
							infoRow.detail += string.Format("\r\nIP:\t{0}", this.control4Check.IP);
							infoRow.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strSwipes, this.control4Check.runinfo.newRecordsNum);
							infoRow.information += string.Format("{0}:{1};", CommonStr.strSwipes, this.control4Check.runinfo.newRecordsNum);
							infoRow.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strPrivileges, this.control4Check.runinfo.registerCardNum);
							infoRow.information += string.Format("{0}:{1};", CommonStr.strPrivileges, this.control4Check.runinfo.registerCardNum);
							infoRow.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strRealClock, this.control4Check.runinfo.dtNow.ToString(wgTools.DisplayFormat_DateYMDHMSWeek));
							infoRow.information += string.Format("{0};", this.control4Check.runinfo.dtNow.ToString(wgTools.DisplayFormat_DateYMDHMSWeek));
							try
							{
								num2++;
								byte[] array2 = new byte[]
								{
									23, 50, 0, 0, 177, 152, 167, 25, 161, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0
								};
								Array.Copy(BitConverter.GetBytes(this.control4Check.ControllerSN), 0, array2, 4, 4);
								byte[] array3 = null;
								if (this.control4Check.ShortPacketSend(array2, ref array3) == 1 && array3 != null && array3[16] == 161 && array3[17] > 176)
								{
									infoRow.detail += string.Format("\r\n--WXT:  {0:X}", array3[17]);
									infoRow.information += string.Format("{0};", string.Format("WXT:{0:X}", array3[17]));
									if (array3[17] == 179 || array3[17] == 180)
									{
										byte[] array4 = new byte[20];
										for (int i = 0; i < 20; i++)
										{
											array4[i] = array3[18 + i];
										}
										string @string = Encoding.ASCII.GetString(array4);
										if (!string.IsNullOrEmpty(@string))
										{
											infoRow.detail += string.Format(" [{0}]", @string);
											infoRow.information += string.Format("[{0}];", @string);
										}
									}
								}
							}
							catch (Exception)
							{
							}
							wgRunInfoLog.addEvent(infoRow);
							listViewItem.ImageIndex = this.control4Check.runinfo.GetDoorImageIndex(this.control4Check.GetDoorNO(listViewItem.Text));
							this.displayNewestLog();
							continue;
						}
						if (this.control4Check.chkFalseControlIP() == 0)
						{
							InfoRow infoRow2 = new InfoRow();
							infoRow2.desc = string.Format("[{0}] {1}", this.control4Check.ControllerSN.ToString(), CommonStr.strNeedUpgradeDriver);
							infoRow2.information = string.Format("{0}: {1}", this.control4Check.ControllerSN.ToString(), CommonStr.strSupposeUpgradeDriver);
							infoRow2.detail = infoRow2.information;
							infoRow2.category = 5;
							wgRunInfoLog.addEvent(infoRow2);
						}
						int num3 = 0;
						int num4 = 0;
						wgTools.WriteLine("Start");
						wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
						wgMjControllerConfigure wgMjControllerConfigure2 = new wgMjControllerConfigure();
						wgMjControllerTaskList wgMjControllerTaskList = new wgMjControllerTaskList();
						wgMjControllerHolidaysList wgMjControllerHolidaysList = new wgMjControllerHolidaysList();
						icControllerConfigureFromDB.getControllerConfigureFromDBByControllerID(this.control4Check.ControllerID, ref wgMjControllerConfigure2, ref wgMjControllerTaskList, ref wgMjControllerHolidaysList);
						wgTools.WriteLine("getControllerConfigureFromDBByControllerID");
						num2++;
						if (this.control4Check.GetConfigureIP(ref wgMjControllerConfigure) <= 0)
						{
							wgRunInfoLog.addEventNotConnect(this.control4Check.ControllerSN, this.control4Check.IP, listViewItem);
							continue;
						}
						wgTools.WriteLine("getConfigureIP");
						wgMjControllerTaskList wgMjControllerTaskList2 = new wgMjControllerTaskList();
						if ((this.control4Check.runinfo.appError & 2) == 0)
						{
							num2++;
							if (this.control4Check.GetControlTaskListIP(ref wgMjControllerTaskList2) <= 0)
							{
								num3 = 1;
							}
							else
							{
								num4 = 1;
							}
						}
						wgTools.WriteLine("getControlTaskListIP");
						new wgMjControllerHolidaysList();
						if ((this.control4Check.runinfo.appError & 2) == 0 && num3 == 0)
						{
							byte[] array5 = null;
							num2++;
							if (this.control4Check.GetHolidayListIP(ref array5) <= 0)
							{
								num3 = 1;
							}
							else
							{
								num4 = 1;
								new wgMjControllerHolidaysList(array5);
							}
						}
						wgTools.WriteLine("GetHolidayListIP");
						InfoRow infoRow3 = new InfoRow();
						infoRow3.desc = string.Format("{0}[{1:d}]", listViewItem.Text, this.control4Check.ControllerSN);
						infoRow3.information = "";
						infoRow3.detail = listViewItem.Text;
						infoRow3.detail += string.Format("\r\n{0}:\t{1}", CommonStr.strDoorStatus, this.control4Check.runinfo.IsOpen(this.control4Check.GetDoorNO(listViewItem.Text)) ? CommonStr.strDoorStatus_Open : CommonStr.strDoorStatus_Closed);
						infoRow3.information += string.Format("{0};", this.control4Check.runinfo.IsOpen(this.control4Check.GetDoorNO(listViewItem.Text)) ? CommonStr.strDoorStatus_Open : CommonStr.strDoorStatus_Closed);
						infoRow3.detail += string.Format("\r\n{0}:\t{1}", CommonStr.strDoorControl, icDesc.doorControlDesc(wgMjControllerConfigure.DoorControlGet(this.control4Check.GetDoorNO(listViewItem.Text))));
						infoRow3.information += string.Format("{0};", icDesc.doorControlDesc(wgMjControllerConfigure.DoorControlGet(this.control4Check.GetDoorNO(listViewItem.Text))));
						infoRow3.detail += string.Format("\r\n{0}:\t{1:d}", CommonStr.strDoorDelay, wgMjControllerConfigure.DoorDelayGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString());
						infoRow3.information += string.Format("{0}:{1:d};", CommonStr.strDoorDelay, wgMjControllerConfigure.DoorDelayGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString());
						infoRow3.detail += string.Format("\r\n{0}:\t{1:d}", CommonStr.strControllerSN, this.control4Check.ControllerSN);
						infoRow3.detail += string.Format("\r\nIP:\t{0}", this.control4Check.IP);
						infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strSwipes, this.control4Check.runinfo.newRecordsNum);
						infoRow3.information += string.Format("{0}:{1};", CommonStr.strSwipes, this.control4Check.runinfo.newRecordsNum);
						infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strPrivileges, this.control4Check.runinfo.registerCardNum);
						infoRow3.information += string.Format("{0}:{1};", CommonStr.strPrivileges, this.control4Check.runinfo.registerCardNum);
						infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strRealClock, this.control4Check.runinfo.dtNow.ToString(wgTools.DisplayFormat_DateYMDHMSWeek));
						infoRow3.information += string.Format("{0};", this.control4Check.runinfo.dtNow.ToString(wgTools.DisplayFormat_DateYMDHMSWeek));
						if (wgMjController.IsFingerController(this.control4Check.ControllerSN))
						{
							infoRow3.desc = string.Format("{0}[{1:d}]", listViewItem.Text, this.control4Check.ControllerSN);
							infoRow3.information = "";
							infoRow3.detail = listViewItem.Text;
							infoRow3.detail += string.Format("\r\n{0}:\t{1:d}", CommonStr.strControllerSN, this.control4Check.ControllerSN);
							infoRow3.detail += string.Format("\r\nIP:\t{0}", this.control4Check.IP);
						}
						if (wgMjController.IsElevator(this.control4Check.ControllerSN) && this.control4Check.runinfo.mutliInput40 != 0UL)
						{
							string text2 = "";
							string text3 = "";
							int num5 = 0;
							int num6 = 0;
							int num7 = (int)((this.control4Check.runinfo.mutliInput40 >> 24) & 16777215UL);
							if (num7 != 0 && (num7 & 8388608) > 0)
							{
								for (int j = 0; j < 20; j++)
								{
									if ((num7 & (1 << j)) > 0)
									{
										text3 = text3 + (j + 1).ToString() + ",";
										num6++;
									}
									else
									{
										text2 = text2 + (j + 1).ToString() + ",";
										num5++;
									}
								}
							}
							num7 = (int)(this.control4Check.runinfo.mutliInput40 & 16777215UL);
							if (num7 != 0 && (num7 & 8388608) > 0)
							{
								for (int k = 0; k < 20; k++)
								{
									if ((num7 & (1 << k)) > 0)
									{
										text3 = text3 + (20 + k + 1).ToString() + ",";
										num6++;
									}
									else
									{
										text2 = text2 + (20 + k + 1).ToString() + ",";
										num5++;
									}
								}
							}
							infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strMultiInputUnconnected, text3);
							infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strMultiInputConnected, text2);
							infoRow3.information += string.Format("{0:X};", this.control4Check.runinfo.mutliInput40);
						}
						if (this.control4Check.runinfo.appError > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strErr, icDesc.ErrorDetail((int)this.control4Check.runinfo.appError));
							infoRow3.information += string.Format("{0};", icDesc.ErrorDetail((int)this.control4Check.runinfo.appError));
						}
						if (this.control4Check.runinfo.netSpeedCode < 2 && num3 > 0)
						{
							if (num4 == 0)
							{
								num2++;
								if (this.control4Check.UpdateFRamIP(127U, 0U) > 0)
								{
									num4 = 1;
								}
							}
							if (num4 == 0)
							{
								infoRow3.detail += string.Format("\r\n--{0}:\t{1}", "?", CommonStr.strPCAccessLimited);
								infoRow3.information += string.Format("{0};", CommonStr.strPCAccessLimited);
							}
							else
							{
								infoRow3.detail += string.Format("\r\n--{0}:\t{1}", "?", CommonStr.strCommLose);
								infoRow3.information += string.Format("{0};", CommonStr.strCommLose);
							}
						}
						if (this.control4Check.runinfo.WarnInfo(this.control4Check.GetDoorNO(listViewItem.Text)) > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strWarnDesc, icDesc.WarnDetail((int)this.control4Check.runinfo.WarnInfo(this.control4Check.GetDoorNO(listViewItem.Text))));
							infoRow3.information += string.Format("{0};", icDesc.WarnDetail((int)this.control4Check.runinfo.WarnInfo(this.control4Check.GetDoorNO(listViewItem.Text))));
						}
						if (this.control4Check.runinfo.FireIsActive)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strFire);
							infoRow3.information += string.Format("{0};", CommonStr.strFire);
						}
						if (this.control4Check.runinfo.ForceLockIsActive)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strCloseByForce);
							infoRow3.information += string.Format("{0};", CommonStr.strCloseByForce);
						}
						infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strFirmware, this.control4Check.runinfo.driverVersion);
						infoRow3.information += string.Format("{0};", this.control4Check.runinfo.driverVersion);
						try
						{
							string text4 = "";
							string text5 = "";
							num2++;
							if (!string.IsNullOrEmpty(this.control4Check.GetProductInfoIP(ref text4, ref text5, -1)))
							{
								infoRow3.detail += string.Format(" [{0}]", text4.Substring(text4.IndexOf("DATE=") + 5, 10));
							}
						}
						catch (Exception)
						{
						}
						if (wgTools.doubleParse(this.control4Check.runinfo.driverVersion.Substring(1)) >= 9.41)
						{
							try
							{
								num2++;
								byte[] array6 = new byte[]
								{
									23, 50, 0, 0, 177, 152, 167, 25, 161, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0
								};
								Array.Copy(BitConverter.GetBytes(this.control4Check.ControllerSN), 0, array6, 4, 4);
								byte[] array7 = null;
								if (this.control4Check.ShortPacketSend(array6, ref array7) == 1 && array7 != null && array7[16] == 161 && array7[17] > 176)
								{
									infoRow3.detail += string.Format("\r\n--WXT:  {0:X}", array7[17]);
									if (array7[17] == 179 || array7[17] == 180 || array7[17] == 181)
									{
										byte[] array8 = new byte[20];
										for (int l = 0; l < 20; l++)
										{
											array8[l] = array7[18 + l];
										}
										string string2 = Encoding.ASCII.GetString(array8);
										if (!string.IsNullOrEmpty(string2))
										{
											infoRow3.detail += string.Format(" [{0}]", string2);
										}
									}
								}
							}
							catch (Exception)
							{
							}
						}
						if (this.control4Check.runinfo.netSpeedCode != 1)
						{
							if (this.control4Check.runinfo.netSpeedCode != 2)
							{
								if (this.control4Check.runinfo.netSpeedCode == 3)
								{
								}
							}
						}
						string text6 = ((this.control4Check.runinfo.netSpeedCode < 2) ? "100M" : "10M");
						infoRow3.detail += string.Format("\r\n--{0}:\t{1} [{2}]", "MAC", wgMjControllerConfigure.MACAddr, text6);
						infoRow3.information += string.Format("{0}[{1}];", wgMjControllerConfigure.MACAddr, text6);
						if (wgMjController.IsFingerController(this.control4Check.ControllerSN))
						{
							if (this.control4Check.FingerGetCountIP() > 0)
							{
								this.checkParam(valBySql.ToString(), this.control4Check.fingerTotalValid.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strFingerprintCount, listViewItem.Text, false);
								infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strFingerprintCount, this.control4Check.fingerTotalValid);
								infoRow3.information += string.Format("{0}:{1};", CommonStr.strFingerprintCount, this.control4Check.fingerTotalValid);
							}
							wgRunInfoLog.addEvent(infoRow3);
							listViewItem.ImageIndex = this.control4Check.runinfo.GetDoorImageIndex(this.control4Check.GetDoorNO(listViewItem.Text));
							continue;
						}
						infoRow3.detail = infoRow3.detail + "\r\n---- " + CommonStr.strEnabled + " ----";
						listViewItem.ImageIndex = this.control4Check.runinfo.GetDoorImageIndex(this.control4Check.GetDoorNO(listViewItem.Text));
						if (!wgMjController.IsFingerController(this.control4Check.ControllerSN) && (DateTime.Now.AddMinutes(-30.0) > this.control4Check.runinfo.dtNow || DateTime.Now.AddMinutes(30.0) < this.control4Check.runinfo.dtNow))
						{
							this.checkParam(DateTime.Now.ToString(wgTools.YMDHMSFormat), this.control4Check.runinfo.dtNow.ToString(wgTools.YMDHMSFormat), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRealClock, listViewItem.Text + " " + CommonStr.strNeedAdjustTime, false);
						}
						wgTools.WriteLine("icPrivilege.getPrivilegeNumInDBByID");
						this.checkParamPrivileges(listViewItem.Text, this.control4Check, (int)this.control4Check.runinfo.registerCardNum);
						if (wgMjControllerConfigure.controlTaskList_enabled > 0)
						{
							if (wgMjControllerTaskList2.taskCount > 0)
							{
								infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strControlTaskList);
								infoRow3.information += string.Format("{0};", CommonStr.strControlTaskList);
							}
						}
						else
						{
							wgMjControllerTaskList2.Clear();
						}
						this.checkParam(wgMjControllerTaskList.taskCount.ToString(), wgMjControllerTaskList2.taskCount.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strControlTaskList, listViewItem.Text, false);
						if (wgMjControllerTaskList2.taskCount == 0)
						{
							this.checkParam(icDesc.doorControlDesc(wgMjControllerConfigure2.DoorControlGet(this.control4Check.GetDoorNO(listViewItem.Text))), icDesc.doorControlDesc(wgMjControllerConfigure.DoorControlGet(this.control4Check.GetDoorNO(listViewItem.Text))), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strDoorControl, listViewItem.Text, false);
						}
						this.checkParam(wgMjControllerConfigure2.DoorDelayGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), wgMjControllerConfigure.DoorDelayGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strDoorDelay, listViewItem.Text, false);
						if (wgMjControllerConfigure.DoorInterlockGet(this.control4Check.GetDoorNO(listViewItem.Text)) > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strInterLock);
							infoRow3.information += string.Format("{0};", CommonStr.strInterLock);
						}
						this.checkParam(wgMjControllerConfigure2.DoorInterlockGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), wgMjControllerConfigure.DoorInterlockGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strInterLock, listViewItem.Text, false);
						if (wgMjController.GetControllerType(this.control4Check.ControllerSN) == 4)
						{
							if (wgMjControllerConfigure.ReaderPasswordGet(this.control4Check.GetDoorNO(listViewItem.Text)) > 0)
							{
								infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strPasswordKeypad);
								infoRow3.information += string.Format("{0};", CommonStr.strPasswordKeypad);
							}
							this.checkParam(wgMjControllerConfigure2.ReaderPasswordGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), wgMjControllerConfigure.ReaderPasswordGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strPasswordKeypad, listViewItem.Text, false);
						}
						else
						{
							if (wgMjControllerConfigure.ReaderPasswordGet((this.control4Check.GetDoorNO(listViewItem.Text) - 1) * 2 + 1) > 0)
							{
								infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strInDoor, CommonStr.strPasswordKeypad);
								infoRow3.information += string.Format("{0}:{1};", CommonStr.strInDoor, CommonStr.strPasswordKeypad);
							}
							if (wgMjControllerConfigure.ReaderPasswordGet((this.control4Check.GetDoorNO(listViewItem.Text) - 1) * 2 + 2) > 0)
							{
								infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strExitDoor, CommonStr.strPasswordKeypad);
								infoRow3.information += string.Format("{0}:{1};", CommonStr.strExitDoor, CommonStr.strPasswordKeypad);
							}
							this.checkParam(wgMjControllerConfigure2.ReaderPasswordGet((this.control4Check.GetDoorNO(listViewItem.Text) - 1) * 2 + 1).ToString(), wgMjControllerConfigure.ReaderPasswordGet((this.control4Check.GetDoorNO(listViewItem.Text) - 1) * 2 + 1).ToString(), string.Concat(new string[]
							{
								"[",
								this.control4Check.ControllerSN.ToString(),
								"]",
								CommonStr.strInDoor,
								" ",
								CommonStr.strPasswordKeypad
							}), listViewItem.Text, false);
							this.checkParam(wgMjControllerConfigure2.ReaderPasswordGet((this.control4Check.GetDoorNO(listViewItem.Text) - 1) * 2 + 2).ToString(), wgMjControllerConfigure.ReaderPasswordGet((this.control4Check.GetDoorNO(listViewItem.Text) - 1) * 2 + 2).ToString(), string.Concat(new string[]
							{
								"[",
								this.control4Check.ControllerSN.ToString(),
								"]",
								CommonStr.strExitDoor,
								" ",
								CommonStr.strPasswordKeypad
							}), listViewItem.Text, false);
						}
						if (wgMjControllerConfigure.receventPB > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strRecordButtonEvent);
							infoRow3.information += string.Format("{0};", CommonStr.strRecordButtonEvent);
						}
						this.checkParam(wgMjControllerConfigure2.receventPB.ToString(), wgMjControllerConfigure.receventPB.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRecordButtonEvent, listViewItem.Text, true);
						if (wgMjControllerConfigure.receventDS > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strRecordDoorStatusEvent);
							infoRow3.information += string.Format("{0};", CommonStr.strRecordDoorStatusEvent);
						}
						this.checkParam(wgMjControllerConfigure2.receventDS.ToString(), wgMjControllerConfigure.receventDS.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRecordDoorStatusEvent, listViewItem.Text, true);
						if (wgMjControllerConfigure.receventWarn > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strRecordWarnEvent);
							infoRow3.information += string.Format("{0};", CommonStr.strRecordWarnEvent);
						}
						this.checkParam(wgMjControllerConfigure2.receventWarn.ToString(), wgMjControllerConfigure.receventWarn.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRecordWarnEvent, listViewItem.Text, true);
						if (wgMjControllerConfigure.antiback > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strAntiBack);
							infoRow3.information += string.Format("{0};", CommonStr.strAntiBack);
						}
						this.checkParam(wgMjControllerConfigure2.antiback.ToString(), wgMjControllerConfigure.antiback.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strAntiBack, listViewItem.Text, true);
						if (wgMjControllerConfigure.DoorDisableTimesegMinGet(this.control4Check.GetDoorNO(listViewItem.Text)) > 1)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strDisableControlSeg);
							infoRow3.information += string.Format("{0};", CommonStr.strDisableControlSeg);
						}
						this.checkParam(wgMjControllerConfigure2.DoorDisableTimesegMinGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), wgMjControllerConfigure.DoorDisableTimesegMinGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strDisableControlSeg, listViewItem.Text, true);
						if (wgMjControllerConfigure.indoorPersonsMax > 0)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strIndoorPersonsMax);
							infoRow3.information += string.Format("{0};", CommonStr.strIndoorPersonsMax);
						}
						this.checkParam(wgMjControllerConfigure2.indoorPersonsMax.ToString(), wgMjControllerConfigure.indoorPersonsMax.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strIndoorPersonsMax, listViewItem.Text, true);
						if ((wgMjControllerConfigure.warnSetup & -41) > 1)
						{
							infoRow3.detail += string.Format("\r\n--{0}", icDesc.WarnDetail(wgMjControllerConfigure.warnSetup & -41));
							infoRow3.information += string.Format("{0};", icDesc.WarnDetail(wgMjControllerConfigure.warnSetup & -41));
						}
						this.checkParam(wgMjControllerConfigure2.warnSetup.ToString(), wgMjControllerConfigure.warnSetup.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strWarn, listViewItem.Text, true);
						if (wgMjControllerConfigure.MorecardNeedCardsGet(this.control4Check.GetDoorNO(listViewItem.Text)) > 1)
						{
							infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strMoreCards);
							infoRow3.information += string.Format("{0};", CommonStr.strMoreCards);
						}
						this.checkParam(wgMjControllerConfigure2.MorecardNeedCardsGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), wgMjControllerConfigure.MorecardNeedCardsGet(this.control4Check.GetDoorNO(listViewItem.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strMoreCards, listViewItem.Text, true);
						if (wgMjControllerConfigure.lockSwitchOption >= 1)
						{
							string text7 = "";
							for (int m = 0; m < 4; m++)
							{
								if ((wgMjControllerConfigure.lockSwitchOption & (1 << m)) > 0)
								{
									if (text7 != "")
									{
										text7 += ",";
									}
									text7 = text7 + "#" + (m + 1).ToString();
								}
							}
							infoRow3.detail += string.Format("\r\n--{0}[{1:X2}]({2})", CommonStr.strLockSwitch, wgMjControllerConfigure.lockSwitchOption, text7);
							infoRow3.information += string.Format("{0}[{1:X2}]({2});", CommonStr.strLockSwitch, wgMjControllerConfigure.lockSwitchOption, text7);
						}
						this.checkParam(wgMjControllerConfigure2.lockSwitchOption.ToString(), wgMjControllerConfigure.lockSwitchOption.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strLockSwitch, listViewItem.Text, true);
						if (wgMjControllerConfigure.swipeGap >= 1)
						{
							infoRow3.detail += string.Format("\r\n--{0}({1}s)", CommonStr.strSwipeGap, wgMjControllerConfigure.swipeGap);
							infoRow3.information += string.Format("{0}({1}s);", CommonStr.strSwipeGap, wgMjControllerConfigure.swipeGap);
						}
						this.checkParam(wgMjControllerConfigure2.swipeGap.ToString(), wgMjControllerConfigure.swipeGap.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strSwipeGap, listViewItem.Text, true);
						if ((this.control4Check.runinfo.reserved1 & 4) > 0)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strFormatSpecialActive);
							infoRow3.information += string.Format("***{0};", CommonStr.strFormatSpecialActive);
						}
						if (wgMjControllerConfigure.webPort != 0 && wgMjControllerConfigure.webPort != 65535)
						{
							string text8 = "";
							text8 += string.Format("{0},{1},{2}", wgMjControllerConfigure.webLanguage, (wgAppConfig.CultureInfoStr == "zh-CHS") ? wgMjControllerConfigure.webDateDisplayFormatCHS : wgMjControllerConfigure.webDateDisplayFormat, wgMjControllerConfigure.webPort.ToString());
							infoRow3.detail += string.Format("\r\n--{0}({1})", CommonStr.strWEBEnabled, text8);
							infoRow3.information += string.Format("{0}({1});", CommonStr.strWEBEnabled, text8);
						}
						if (wgMjControllerConfigure.SpecialCard_Mother1 != 0L && wgMjControllerConfigure.SpecialCard_Mother1 != (long)((ulong)(-1)))
						{
							infoRow3.detail += string.Format("\r\n--***{0}1({1})", CommonStr.strSpecialCardMother, wgMjControllerConfigure.SpecialCard_Mother1.ToString());
							infoRow3.information += string.Format("***{0}1({1});", CommonStr.strSpecialCardMother, wgMjControllerConfigure.SpecialCard_Mother1.ToString());
						}
						if (wgMjControllerConfigure.SpecialCard_Mother2 != 0L && wgMjControllerConfigure.SpecialCard_Mother2 != (long)((ulong)(-1)))
						{
							infoRow3.detail += string.Format("\r\n--***{0}2({1})", CommonStr.strSpecialCardMother, wgMjControllerConfigure.SpecialCard_Mother2.ToString());
							infoRow3.information += string.Format("***{0}2({1});", CommonStr.strSpecialCardMother, wgMjControllerConfigure.SpecialCard_Mother2.ToString());
						}
						if (wgMjControllerConfigure.SpecialCard_OnlyOpen1 != 0L && wgMjControllerConfigure.SpecialCard_OnlyOpen1 != (long)((ulong)(-1)))
						{
							infoRow3.detail += string.Format("\r\n--***{0}1({1})", CommonStr.strSpecialCardSuper, wgMjControllerConfigure.SpecialCard_OnlyOpen1.ToString());
							infoRow3.information += string.Format("***{0}1({1});", CommonStr.strSpecialCardSuper, wgMjControllerConfigure.SpecialCard_OnlyOpen1.ToString());
						}
						if (wgMjControllerConfigure.SpecialCard_OnlyOpen2 != 0L && wgMjControllerConfigure.SpecialCard_OnlyOpen2 != (long)((ulong)(-1)))
						{
							infoRow3.detail += string.Format("\r\n--***{0}2({1})", CommonStr.strSpecialCardSuper, wgMjControllerConfigure.SpecialCard_OnlyOpen2.ToString());
							infoRow3.information += string.Format("***{0}2({1});", CommonStr.strSpecialCardSuper, wgMjControllerConfigure.SpecialCard_OnlyOpen2.ToString());
						}
						if ((wgMjControllerConfigure.fire_broadcast_receive != 0 && (long)wgMjControllerConfigure.fire_broadcast_receive != 255L) || (wgMjControllerConfigure.fire_broadcast_send != 0 && (long)wgMjControllerConfigure.fire_broadcast_send != 255L))
						{
							infoRow3.detail += string.Format("\r\n--***{0}({1}s,#{2})", CommonStr.strFireSignalShare, wgMjControllerConfigure.fire_broadcast_receive.ToString(), wgMjControllerConfigure.fire_broadcast_send.ToString());
							infoRow3.information += string.Format("***{0}({1}s,#{2});", CommonStr.strFireSignalShare, wgMjControllerConfigure.fire_broadcast_receive.ToString(), wgMjControllerConfigure.fire_broadcast_send.ToString());
						}
						this.checkParam((string.Format("({0}s,{1})", wgMjControllerConfigure2.fire_broadcast_receive.ToString(), wgMjControllerConfigure2.fire_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", (string.Format("({0}s,{1})", wgMjControllerConfigure.fire_broadcast_receive.ToString(), wgMjControllerConfigure.fire_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strFireSignalShare, listViewItem.Text, true);
						if ((wgMjControllerConfigure.interlock_broadcast_receive != 0 && (long)wgMjControllerConfigure.interlock_broadcast_receive != 255L) || (wgMjControllerConfigure.interlock_broadcast_send != 0 && (long)wgMjControllerConfigure.interlock_broadcast_send != 255L))
						{
							infoRow3.detail += string.Format("\r\n--***{0}({1}s,#{2})", CommonStr.strInterLockShare, wgMjControllerConfigure.interlock_broadcast_receive.ToString(), wgMjControllerConfigure.interlock_broadcast_send.ToString());
							infoRow3.information += string.Format("***{0}({1}s,#{2});", CommonStr.strInterLockShare, wgMjControllerConfigure.interlock_broadcast_receive.ToString(), wgMjControllerConfigure.interlock_broadcast_send.ToString());
						}
						this.checkParam((string.Format("({0}s,{1})", wgMjControllerConfigure2.interlock_broadcast_receive.ToString(), wgMjControllerConfigure2.interlock_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", (string.Format("({0}s,{1})", wgMjControllerConfigure.interlock_broadcast_receive.ToString(), wgMjControllerConfigure.interlock_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strInterLockShare, listViewItem.Text, true);
						if (wgMjControllerConfigure.antiback_broadcast_send != 0 && (long)wgMjControllerConfigure.antiback_broadcast_send != 255L)
						{
							infoRow3.detail += string.Format("\r\n--***{0}(#{1})", CommonStr.strAntibackShare, wgMjControllerConfigure.antiback_broadcast_send.ToString());
							infoRow3.information += string.Format("***{0}(#{1});", CommonStr.strAntibackShare, wgMjControllerConfigure.antiback_broadcast_send.ToString());
						}
						this.checkParam((string.Format("({0}s,{1})", wgMjControllerConfigure2.antiback_broadcast_send.ToString(), wgMjControllerConfigure2.antiback_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", (string.Format("({0}s,{1})", wgMjControllerConfigure.antiback_broadcast_send.ToString(), wgMjControllerConfigure.antiback_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strAntibackShare, listViewItem.Text, true);
						if (wgMjControllerConfigure.indoorPersonsMax > 0 || (wgMjControllerConfigure.antiback_broadcast_send != 0 && (long)wgMjControllerConfigure.antiback_broadcast_send != 255L))
						{
							infoRow3.detail += string.Format("\r\n--***{0}({1})", CommonStr.strtotalPerson4AntibackShare, this.control4Check.runinfo.totalPerson4AntibackShare.ToString());
							infoRow3.information += string.Format("***{0}({1});", CommonStr.strtotalPerson4AntibackShare, this.control4Check.runinfo.totalPerson4AntibackShare.ToString());
						}
						if ((wgMjControllerConfigure.IPListControl & 242) == 194)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strIPListEnable);
							infoRow3.information += string.Format("***{0};", CommonStr.strIPListEnable);
						}
						if (wgMjControllerConfigure.mobile_as_card_input == 165)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strMobileAsCardInputEnabled);
							infoRow3.information += string.Format("***{0};", CommonStr.strMobileAsCardInputEnabled);
						}
						if (wgMjControllerConfigure.invalidCard_ledbeep_output_disable == 165)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strDisableLEDBEEPWhileInvalidCard);
							infoRow3.information += string.Format("***{0};", CommonStr.strDisableLEDBEEPWhileInvalidCard);
						}
						if (wgMjControllerConfigure.check_controller_online_timeout != 0 && wgMjControllerConfigure.check_controller_online_timeout != 65535)
						{
							infoRow3.detail += string.Format("\r\n--{0}:\t{1}", CommonStr.strWatchingDogTime, wgMjControllerConfigure.check_controller_online_timeout);
							infoRow3.information += string.Format("{0}={1};", CommonStr.strWatchingDogTime, wgMjControllerConfigure.check_controller_online_timeout);
						}
						if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DISPLAY_NEWEST_SWIPE")) && this.control4Check.runinfo.newRecordsNum > 0U)
						{
							MjRec mjRec = this.control4Check.runinfo.newSwipes[0];
							if (mjRec != null)
							{
								if (mjRec.addressIsReader)
								{
									if (this.ReaderName.ContainsKey(string.Format("{0}-{1}", mjRec.ControllerSN.ToString(), mjRec.ReaderNo.ToString())))
									{
										wgTools.WriteLine("ReaderName.ContainsKey(string.Format(");
										mjRec.address = this.ReaderName[string.Format("{0}-{1}", mjRec.ControllerSN.ToString(), mjRec.ReaderNo.ToString())];
									}
								}
								else
								{
									this.dvDoors4Check.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjRec.ControllerSN.ToString(), mjRec.DoorNo.ToString());
									if (this.dvDoors4Check.Count > 0)
									{
										infoRow3.desc = this.dvDoors4Check[0]["f_DoorName"].ToString();
										mjRec.address = this.dvDoors4Check[0]["f_DoorName"] as string;
									}
								}
							}
							if (wgRunInfoLog.tcpServerEnabled > 0)
							{
								this.dvDoors4Check.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjRec.ControllerSN.ToString(), mjRec.DoorNo.ToString());
								if (this.dvDoors4Check.Count > 0)
								{
									mjRec.doorName = this.dvDoors4Check[0]["f_DoorName"] as string;
								}
							}
							string text9 = mjRec.ToDisplayInfo();
							int num8 = text9.LastIndexOf("-");
							text9 = text9.Substring(0, num8) + "\r\n  " + text9.Substring(num8);
							infoRow3.detail += string.Format("\r\n\r\n--{0}", text9);
						}
						if (this.bNeedCheckLosePacket)
						{
							int num9 = 0;
							int num10 = 0;
							int num11 = 0;
							for (int n = 0; n < 200; n++)
							{
								num9++;
								num2++;
								if (this.control4Check.SpecialPingIP() == 1)
								{
									num10++;
								}
								else
								{
									num11++;
								}
							}
							if (num11 == 0)
							{
								wgUdpComm.triesTotal = 0L;
								wgTools.WriteLine("control.Test1024 Start");
								int num12 = 0;
								string text10 = "";
								if (this.control4Check.test1024Write() < 0)
								{
									text10 = text10 + CommonStr.strCommLargePacketWriteFailed + "\r\n";
								}
								int num13 = this.control4Check.test1024Read(100U, ref num12);
								if (num13 < 0)
								{
									text10 = text10 + CommonStr.strCommLargePacketReadFailed + num13.ToString() + "\r\n";
								}
								if (wgUdpComm.triesTotal > 0L)
								{
									string text11 = text10;
									text10 = string.Concat(new string[]
									{
										text11,
										CommonStr.strCommLargePacketTryTimes,
										" = ",
										wgUdpComm.triesTotal.ToString(),
										"\r\n"
									});
								}
								wgTools.WriteLine("control.Test1024 End");
								if (text10 != "")
								{
									string text12 = text10;
									infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strCommLose);
									infoRow3.information += string.Format("{0};", CommonStr.strCommLose);
									infoRow3.detail += string.Format("\r\n--{0}", text12);
									infoRow3.information += string.Format("{0};", text12);
									wgRunInfoLog.addEvent(new InfoRow
									{
										desc = "[" + listViewItem.Text + "]" + CommonStr.strCommLose,
										information = string.Concat(new string[]
										{
											"[",
											this.control4Check.ControllerSN.ToString(),
											"]",
											CommonStr.strCommLose,
											": ",
											text12
										}),
										category = 501
									});
								}
								else
								{
									infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strCommOK);
									infoRow3.information += string.Format("{0};", CommonStr.strCommOK);
								}
							}
							else
							{
								string text13 = string.Format(" {0}: {1}={2}, {3}={4}, {5} = {6}", new object[]
								{
									CommonStr.strCommPacket,
									CommonStr.strCommPacketSent,
									num9,
									CommonStr.strCommPacketReceived,
									num10,
									CommonStr.strCommPacketLost,
									num11
								}) + "\r\n";
								infoRow3.detail += string.Format("\r\n--{0}", CommonStr.strCommLose);
								infoRow3.information += string.Format("{0};", CommonStr.strCommLose);
								infoRow3.detail += string.Format("\r\n--{0}", text13);
								infoRow3.information += string.Format("{0};", text13);
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = "[" + listViewItem.Text + "]" + CommonStr.strCommLose,
									information = string.Concat(new string[]
									{
										"[",
										this.control4Check.ControllerSN.ToString(),
										"]",
										CommonStr.strCommLose,
										": ",
										text13
									}),
									category = 501
								});
							}
						}
						double totalSeconds = DateTime.Now.Subtract(now).TotalSeconds;
						if (this.control4Check.runinfo.netSpeedCode >= 2 && totalSeconds > 2.0)
						{
							InfoRow infoRow4 = new InfoRow();
							infoRow4.desc = string.Format("{0}: {1}", listViewItem.Text, CommonStr.strNetException10M);
							infoRow4.information = string.Format("[{0}]: {1}", this.control4Check.ControllerSN.ToString(), CommonStr.strNetException10MSuggest);
							infoRow4.detail = infoRow4.information;
							infoRow4.category = 501;
							wgRunInfoLog.addEvent(infoRow4);
						}
						if (wgMjControllerConfigure.dhcpEnable == 165)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strDHCPEnabled);
							infoRow3.information += string.Format("***{0};", CommonStr.strDHCPEnabled);
						}
						if (wgMjControllerConfigure.mobile_web_autoip_disable == 165)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strMoobile_web_autoip_disable);
							infoRow3.information += string.Format("***{0};", CommonStr.strMoobile_web_autoip_disable);
						}
						if (wgMjControllerConfigure.auto_try10M_disable == 166)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.str10MForced);
							infoRow3.information += string.Format("***{0};", CommonStr.str10MForced);
						}
						if (wgMjControllerConfigure.auto_try10M_disable == 165)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.str100MForced);
							infoRow3.information += string.Format("***{0};", CommonStr.str100MForced);
						}
						if (wgMjControllerConfigure.pcControlSwipeTimeout > 0)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strGlobalAntiPassbackPCControlAccess2015);
							infoRow3.information += string.Format("***{0};", CommonStr.strGlobalAntiPassbackPCControlAccess2015);
						}
						if (wgMjControllerConfigure.autiback_allow_firstout_enable == 165)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strAllowAntiPassbackFirstExit2015);
							infoRow3.information += string.Format("***{0};", CommonStr.strAllowAntiPassbackFirstExit2015);
						}
						if (wgMjControllerConfigure.invalid_swipe_opendoor == 165)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strInvalidSwipeOpenDoor2018);
							infoRow3.information += string.Format("***{0};", CommonStr.strInvalidSwipeOpenDoor2018);
						}
						if (wgMjControllerConfigure.invalid_swipe_warntimeout > 0)
						{
							infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strRecordInvalidSwipeWarn);
							infoRow3.information += string.Format("***{0};", CommonStr.strRecordInvalidSwipeWarn);
						}
						if (this.control4Check.GetConfigureInFlashIP(ref wgMjControllerConfigure) > 0)
						{
							if (wgMjControllerConfigure.controllerServer != 0 && wgMjControllerConfigure.controllerServer != -1)
							{
								if (wgMjControllerConfigure.controllerServer == 1)
								{
									infoRow3.detail += string.Format("\r\n--***{0}", CommonStr.strControllerMaster);
									infoRow3.information += string.Format("***{0};", CommonStr.strControllerMaster);
								}
								else
								{
									infoRow3.detail += string.Format("\r\n--***{0}({1})", CommonStr.strControllerClient, wgMjControllerConfigure.controllerServer.ToString());
									infoRow3.information += string.Format("***{0}({1});", CommonStr.strControllerClient, wgMjControllerConfigure.controllerServer.ToString());
								}
							}
							if (wgMjControllerConfigure.rs232_1_option != 0 && wgMjControllerConfigure.rs232_1_option != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{0}({1:X2},{2:X2})", CommonStr.strActivateRS232_1, wgMjControllerConfigure.rs232_1_option, wgMjControllerConfigure.rs232_1_extern);
								infoRow3.information += string.Format("***{0}({1:X2},{2:X2});", CommonStr.strActivateRS232_1, wgMjControllerConfigure.rs232_1_option, wgMjControllerConfigure.rs232_1_extern);
							}
							if (wgMjControllerConfigure.rs232_2_option != 0 && wgMjControllerConfigure.rs232_2_option != 255)
							{
								infoRow3.detail += string.Format("***{0}({1:X2},{2:X2})", CommonStr.strActivateRS232_2, wgMjControllerConfigure.rs232_2_option, wgMjControllerConfigure.rs232_2_extern);
								infoRow3.information += string.Format("***{0}({1:X2},{2:X2});", CommonStr.strActivateRS232_2, wgMjControllerConfigure.rs232_2_option, wgMjControllerConfigure.rs232_2_extern);
							}
							if ((wgMjControllerConfigure.wgqr_option != 0 && wgMjControllerConfigure.wgqr_option != 255) || wgMjControllerConfigure.wgqr_extern != 0)
							{
								if (wgMjControllerConfigure.wgqr_option == 192)
								{
									infoRow3.detail += string.Format("\r\n--***{0}({1:X2},{2:X2})", CommonStr.strDeActivateWGQR, wgMjControllerConfigure.wgqr_option, wgMjControllerConfigure.wgqr_extern);
									infoRow3.information += string.Format("***{0}({1:X2},{2:X2});", CommonStr.strDeActivateWGQR, wgMjControllerConfigure.wgqr_option, wgMjControllerConfigure.wgqr_extern);
								}
								else
								{
									infoRow3.detail += string.Format("\r\n--***{0}({1:X2},{2:X2})", CommonStr.strActivateWGQR, wgMjControllerConfigure.wgqr_option, wgMjControllerConfigure.wgqr_extern);
									infoRow3.information += string.Format("***{0}({1:X2},{2:X2});", CommonStr.strActivateWGQR, wgMjControllerConfigure.wgqr_option, wgMjControllerConfigure.wgqr_extern);
								}
							}
							if (!wgMjControllerConfigure.dataServerIP.ToString().Equals("0.0.0.0") && !wgMjControllerConfigure.dataServerIP.ToString().Equals("255.255.255.255"))
							{
								infoRow3.detail += string.Format("\r\n--***{0} {1}:{2}  [{3}={4}]", new object[]
								{
									CommonStr.strCloudServer + "1: ",
									wgMjControllerConfigure.dataServerIP,
									wgMjControllerConfigure.dataServerPort,
									CommonStr.strCloudServerCycle,
									wgMjControllerConfigure.dataServerCycle
								});
								infoRow3.information += string.Format("***{0};", CommonStr.strCloudServer + " 1");
							}
							if (!wgMjControllerConfigure.dataServerShortIP.ToString().Equals("0.0.0.0") && !wgMjControllerConfigure.dataServerShortIP.ToString().Equals("255.255.255.255"))
							{
								infoRow3.detail += string.Format("\r\n--***{0} {1}:{2}  [{3}={4}]", new object[]
								{
									CommonStr.strCloudServer + "2: ",
									wgMjControllerConfigure.dataServerShortIP,
									wgMjControllerConfigure.dataServerShortPort,
									CommonStr.strCloudServerCycle,
									wgMjControllerConfigure.dataServerShortCycle
								});
								infoRow3.information += string.Format("***{0};", CommonStr.strCloudServer + " 2");
							}
							if (!wgMjControllerConfigure.dataServer3ShortIP.ToString().Equals("0.0.0.0") && !wgMjControllerConfigure.dataServer3ShortIP.ToString().Equals("255.255.255.255"))
							{
								infoRow3.detail += string.Format("\r\n--***{0} {1}:{2}  [{3}={4}]", new object[]
								{
									CommonStr.strCloudServer + "3: ",
									wgMjControllerConfigure.dataServer3ShortIP,
									wgMjControllerConfigure.dataServer3ShortPort,
									CommonStr.strCloudServerCycle,
									wgMjControllerConfigure.dataServerShortCycle
								});
								infoRow3.information += string.Format("***{0};", CommonStr.strCloudServer + " 3");
							}
							if (!wgMjControllerConfigure.dataServerShort2IP.ToString().Equals("0.0.0.0") && !wgMjControllerConfigure.dataServerShort2IP.ToString().Equals("255.255.255.255"))
							{
								infoRow3.detail += string.Format("\r\n--***{0}{1}:{2}  [{3:X2}]", new object[] { "HTTP://", wgMjControllerConfigure.dataServerShort2IP, wgMjControllerConfigure.dataServerShort2Port, wgMjControllerConfigure.dataServerShort2Option });
								infoRow3.information += string.Format("***{0};", " HTTP UPLOAD");
							}
							if (wgMjControllerConfigure.rs232_qr_validtime_max != 0 && wgMjControllerConfigure.rs232_qr_validtime_max != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{0}({1:X2})", CommonStr.strrs232_qr_validtime_max, wgMjControllerConfigure.rs232_qr_validtime_max);
								infoRow3.information += string.Format("***{0} ={1:X};", CommonStr.strrs232_qr_validtime_max, wgMjControllerConfigure.rs232_qr_validtime_max);
							}
							if (wgMjControllerConfigure.not_comm_doorno != 0 && wgMjControllerConfigure.not_comm_doorno != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{0}({1:X2})", CommonStr.strnot_comm_doorno, wgMjControllerConfigure.not_comm_doorno);
								infoRow3.information += string.Format("***{0} ={1:X};", CommonStr.strnot_comm_doorno, wgMjControllerConfigure.not_comm_doorno);
							}
							if (wgMjControllerConfigure.check_two_cards != 0 && wgMjControllerConfigure.check_two_cards != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{0}({1:X2})", CommonStr.strcheck_two_cards, wgMjControllerConfigure.check_two_cards);
								infoRow3.information += string.Format("***{0} ={1:X};", CommonStr.strcheck_two_cards, wgMjControllerConfigure.check_two_cards);
							}
							if (wgMjControllerConfigure.pwd468_Check != 0 && wgMjControllerConfigure.pwd468_Check != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{2}_{0}({1:X2})", CommonStr.strpwd468_Check, wgMjControllerConfigure.pwd468_Check, (wgMjControllerConfigure.pwd468_Check & 48) >> 2);
								infoRow3.information += string.Format("***{2}_{0} ={1:X};", CommonStr.strpwd468_Check, wgMjControllerConfigure.pwd468_Check, (wgMjControllerConfigure.pwd468_Check & 48) >> 2);
							}
							if (wgMjControllerConfigure.antiback_validtime != 0 && wgMjControllerConfigure.antiback_validtime != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{0}({1},{2})", CommonStr.strantiback_validtime, ((wgMjControllerConfigure.antiback_validtime & 128) > 0) ? "*" : "--", wgMjControllerConfigure.antiback_validtime & 127);
								infoRow3.information += string.Format("***{0} ={1:X2};", CommonStr.strantiback_validtime, wgMjControllerConfigure.antiback_validtime);
							}
							if (wgMjControllerConfigure.uart1_baud != 0 && wgMjControllerConfigure.uart1_baud != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{0}({1:X2})_{2}", "uart1_baud", wgMjControllerConfigure.uart1_baud, wgMjControllerConfigure.uart1_baud * 1200);
								infoRow3.information += string.Format("***{0} ={1:X}_{2};", "uart1_baud", wgMjControllerConfigure.uart1_baud, wgMjControllerConfigure.uart1_baud * 1200);
							}
							if (wgMjControllerConfigure.uart2_baud != 0 && wgMjControllerConfigure.uart2_baud != 255)
							{
								infoRow3.detail += string.Format("\r\n--***{0}({1:X2})_{2}", "uart2_baud", wgMjControllerConfigure.uart2_baud, wgMjControllerConfigure.uart2_baud * 1200);
								infoRow3.information += string.Format("***{0} ={1:X}_{2};", "uart2_baud", wgMjControllerConfigure.uart2_baud, wgMjControllerConfigure.uart2_baud * 1200);
							}
						}
						wgRunInfoLog.addEvent(infoRow3);
					}
					this.displayNewestLog();
					wgTools.WriteLine("displayNewestLog");
				}
				if (!flag)
				{
					Application.DoEvents();
					using (dfrmNetControllerFaultDeal dfrmNetControllerFaultDeal = new dfrmNetControllerFaultDeal())
					{
						this.control4Check.GetInfoFromDBByDoorNameSpecial(text, this.dsWatchingDoorInfo);
						dfrmNetControllerFaultDeal.ControllerID = num;
						dfrmNetControllerFaultDeal.Text += string.Format(" [ {0} ] ", text);
						dfrmNetControllerFaultDeal.ShowDialog(this);
					}
				}
				this.control4Check.Dispose();
				this.control4Check = null;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00085E6C File Offset: 0x00084E6C
		private void btnSetTime_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			if (wgAppConfig.IsAcceleratorActive)
			{
				this.adjustTimeMultithreadToolStripMenuItem_Click(sender, e);
				return;
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				if (this.lstDoors.SelectedItems.Count == 1)
				{
					if (XMessageBox.Show(sender.ToString() + " " + this.lstDoors.SelectedItems[0].Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						return;
					}
				}
				else if (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return;
				}
			}
			this.btnStopOthers.BackColor = Color.Red;
			this.btnStopMonitor.BackColor = Color.Red;
			using (icController icController = new icController())
			{
				this.bStopComm = false;
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (this.bStopComm)
					{
						break;
					}
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					DateTime now = DateTime.Now;
					if (icController.AdjustTimeIP(now) <= 0)
					{
						wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0}:{1}", CommonStr.strAdjustTimeOK, now.ToString("yyyy-MM-dd HH:mm:ss"))
						});
						this.dispDoorStatusByIPComm(icController, listViewItem);
					}
					this.displayNewestLog();
				}
			}
			if (this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting && this.btnServer.Text != CommonStr.strMonitoring)
			{
				this.btnStopOthers.BackColor = Color.Transparent;
				this.btnStopMonitor.BackColor = Color.Transparent;
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000860B4 File Offset: 0x000850B4
		private void btnUpload_Click(object sender, EventArgs e)
		{
			if (!this.bkUploadAndGetRecords.IsBusy)
			{
				if (this.lstDoors.SelectedItems.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strSelectDoor);
					return;
				}
				if (wgAppConfig.IsAcceleratorActive)
				{
					this.downloadMultithreadToolStripMenuItem_Click(sender, e);
					return;
				}
				using (dfrmUploadOption dfrmUploadOption = new dfrmUploadOption())
				{
					dfrmUploadOption.TopMost = true;
					dfrmUploadOption.ShowDialog(this);
					if (dfrmUploadOption.checkVal == 0)
					{
						return;
					}
					this.CommOperateOption = dfrmUploadOption.checkVal;
				}
				this.btnRealtimeGetRecords.Enabled = false;
				this.btnStopOthers.BackColor = Color.Red;
				this.btnStopMonitor.BackColor = Color.Red;
				this.btnGetRecords.Enabled = false;
				this.mnuGetRecordsToolStripMenuItem.Enabled = false;
				this.btnUpload.Enabled = false;
				this.mnuUploadToolStripMenuItem.Enabled = false;
				this.arrSelectedDoors.Clear();
				this.arrSelectedDoorsItem.Clear();
				this.dealtDoorIndex = 0;
				this.arrDealtController.Clear();
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					this.arrSelectedDoors.Add(listViewItem.Text);
					this.arrSelectedDoorsItem.Add(listViewItem);
				}
				using (icController icController = new icController())
				{
					icController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
					wgRunInfoLog.addEvent(new InfoRow
					{
						desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), icController.ControllerSN),
						information = string.Format("{0}", CommonStr.strUploadStart)
					});
				}
				this.displayNewestLog();
				this.CommOperate = "UPLOAD";
				this.bkUploadAndGetRecords.RunWorkerAsync();
				if (!this.bkDispDoorStatus.IsBusy)
				{
					this.bkDispDoorStatus.RunWorkerAsync();
				}
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00086304 File Offset: 0x00085304
		private void btnGetRecords_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAcceleratorActive)
			{
				this.uploadMultithreadToolStripMenuItem_Click(sender, e);
				return;
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				if (this.lstDoors.SelectedItems.Count == 1)
				{
					if (XMessageBox.Show(sender.ToString() + " " + this.lstDoors.SelectedItems[0].Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						return;
					}
				}
				else if (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return;
				}
			}
			if (!this.bkUploadAndGetRecords.IsBusy)
			{
				if (this.lstDoors.SelectedItems.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strSelectDoor);
					return;
				}
				this.btnRealtimeGetRecords.Enabled = false;
				this.btnStopOthers.BackColor = Color.Red;
				this.btnStopMonitor.BackColor = Color.Red;
				this.btnGetRecords.Enabled = false;
				this.mnuGetRecordsToolStripMenuItem.Enabled = false;
				this.btnUpload.Enabled = false;
				this.mnuUploadToolStripMenuItem.Enabled = false;
				this.arrSelectedDoors.Clear();
				this.arrSelectedDoorsItem.Clear();
				this.dealtDoorIndex = 0;
				this.arrDealtController.Clear();
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					this.arrSelectedDoors.Add(listViewItem.Text);
					this.arrSelectedDoorsItem.Add(listViewItem);
				}
				using (icController icController = new icController())
				{
					icController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
					wgRunInfoLog.addEvent(new InfoRow
					{
						desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), icController.ControllerSN),
						information = string.Format("{0}", CommonStr.strGetSwipeRecordStart)
					});
				}
				this.displayNewestLog();
				this.CommOperate = "GETRECORDS";
				this.bkUploadAndGetRecords.RunWorkerAsync();
				if (!this.bkDispDoorStatus.IsBusy)
				{
					this.bkDispDoorStatus.RunWorkerAsync();
				}
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0008658C File Offset: 0x0008558C
		public void directToRealtimeGet()
		{
			Cursor.Current = Cursors.WaitCursor;
			Thread.Sleep(1000);
			if (this.arrSelectDoors4Sign.Count == 0)
			{
				this.btnSelectAll.PerformClick();
			}
			else
			{
				try
				{
					for (int i = 0; i <= this.lstDoors.Items.Count - 1; i++)
					{
						this.dvDoors.RowFilter = "f_DoorName =" + wgTools.PrepareStr(this.lstDoors.Items[i].Text);
						if (this.dvDoors.Count > 0 && this.arrSelectDoors4Sign.IndexOf(this.dvDoors[0]["f_ControllerID"]) >= 0)
						{
							this.lstDoors.Items[i].Selected = true;
						}
						else
						{
							this.lstDoors.Items[i].Selected = false;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			this.bMainWindowDisplay = false;
			this.bDirectToRealtimeGet = true;
			this.btnRealtimeGetRecords.PerformClick();
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000866B0 File Offset: 0x000856B0
		private void btnRealtimeGetRecords_Click(object sender, EventArgs e)
		{
			if (!this.bkRealtimeGetRecords.IsBusy && this.btnRealtimeGetRecords.Enabled)
			{
				this.btnStopOthers_Click(null, null);
				if (this.lstDoors.SelectedItems.Count <= 0)
				{
					if (!this.bDirectToRealtimeGet)
					{
						XMessageBox.Show(CommonStr.strSelectDoor);
						return;
					}
				}
				else
				{
					if (this.lstDoors.SelectedItems.Count > 0 && !this.bDirectToRealtimeGet)
					{
						if (this.lstDoors.SelectedItems.Count == 1)
						{
							if (XMessageBox.Show(sender.ToString() + " " + this.lstDoors.SelectedItems[0].Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
							{
								return;
							}
						}
						else if (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
						{
							return;
						}
					}
					this.bDirectToRealtimeGet = false;
					this.btnStopOthers_Click(null, null);
					this.btnRealtimeGetRecords.Enabled = false;
					this.btnGetRecords.Enabled = false;
					this.mnuGetRecordsToolStripMenuItem.Enabled = false;
					this.btnUpload.Enabled = false;
					this.mnuUploadToolStripMenuItem.Enabled = false;
					this.btnServer.Enabled = false;
					this.mnuMonitorToolStripMenuItem.Enabled = false;
					this.Refresh();
					this.arrSelectedDoors.Clear();
					this.arrSelectedDoorsItem.Clear();
					this.dealtDoorIndex = 0;
					this.arrDealtController.Clear();
					foreach (object obj in this.lstDoors.SelectedItems)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						this.arrSelectedDoors.Add(listViewItem.Text);
						this.arrSelectedDoorsItem.Add(listViewItem);
					}
					this.timerUpdateDoorInfo.Enabled = false;
					Dictionary<int, icController> dictionary = new Dictionary<int, icController>();
					this.needDelSwipeControllers = new Dictionary<int, int>();
					Cursor.Current = Cursors.WaitCursor;
					foreach (object obj2 in this.lstDoors.Items)
					{
						ListViewItem listViewItem2 = (ListViewItem)obj2;
						(listViewItem2.Tag as frmConsole.DoorSetInfo).Selected = 0;
					}
					this.realtimeGetRecordsSwipeIndexGot.Clear();
					this.realtimeGetRecordsSwipeRawStringGot.Clear();
					this.selectedControllersSNOfRealtimeGetRecords.Clear();
					this.arrSelectedControllers.Clear();
					foreach (object obj3 in this.lstDoors.SelectedItems)
					{
						ListViewItem listViewItem3 = (ListViewItem)obj3;
						(listViewItem3.Tag as frmConsole.DoorSetInfo).Selected = 1;
						if (!dictionary.ContainsKey((listViewItem3.Tag as frmConsole.DoorSetInfo).ControllerSN))
						{
							wgTools.WriteLine("!selectedControllers.ContainsKey(control.ControllerSN)");
							this.control4Realtime = new icController();
							this.control4Realtime.GetInfoFromDBByDoorNameSpecial(listViewItem3.Text, this.dsWatchingDoorInfo);
							dictionary.Add(this.control4Realtime.ControllerSN, this.control4Realtime);
							this.realtimeGetRecordsSwipeIndexGot.Add(this.control4Realtime.ControllerSN, -1);
							this.realtimeGetRecordsSwipeRawStringGot.Add(this.control4Realtime.ControllerSN, "");
							this.selectedControllersSNOfRealtimeGetRecords.Add(this.control4Realtime.ControllerSN);
							this.needDelSwipeControllers.Add(this.control4Realtime.ControllerSN, 0);
							this.arrSelectedControllers.Add(this.control4Realtime.ControllerSN);
						}
					}
					this.selectedControllersOfRealtimeGetRecords = dictionary;
					using (icController icController = new icController())
					{
						icController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = "",
							information = string.Format("{0}", CommonStr.strRealtimeGetSwipeRecordStart)
						});
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), icController.ControllerSN),
							information = string.Format("{0}", CommonStr.strGetSwipeRecordStart)
						});
					}
					this.displayNewestLog();
					this.bStopComm = false;
					this.stepOfRealtimeGetRecords = frmConsole.StepOfRealtimeGetReocrds.GetRecordFirst;
					this.dealtIndexOfDoorsNeedToGetRecords = -1;
					this.doorsNeedToGetRecords.Clear();
					this.bkRealtimeGetRecords.RunWorkerAsync();
					if (!this.bkDispDoorStatus.IsBusy)
					{
						this.bkDispDoorStatus.RunWorkerAsync();
					}
					(sender as ToolStripButton).BackColor = Color.LightGreen;
					(sender as ToolStripButton).Text = CommonStr.strRealtimeGetting;
					this.btnStopOthers.BackColor = Color.Red;
					this.btnStopMonitor.BackColor = Color.Red;
					if (wgAppConfig.IsActivateCameraManage && wgAppConfig.GetKeyVal("KEY_Video_DontCaputreOnThisPC") != "1")
					{
						this.selectedCameraID = this.selectDoorCamera();
						this.openCamera();
					}
					if (this.bPCCheckMealOpen)
					{
						this.pcCheckMealOpen_Init();
					}
					this.pcWatchingDoorOpenWarn_Init();
					if (this.bPCCheckGlobalAntiBackOpen)
					{
						this.GlobalAntiBackOpen_Init();
					}
					this.bStartRealtimeGetRecords = true;
					Cursor.Current = Cursors.Default;
				}
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00086C80 File Offset: 0x00085C80
		private void downloadPrivilegesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				if (this.lstDoors.SelectedItems.Count == 1)
				{
					if (XMessageBox.Show(sender.ToString() + " " + this.lstDoors.SelectedItems[0].Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						return;
					}
				}
				else if (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return;
				}
			}
			if (!this.bkUploadAndGetRecords.IsBusy)
			{
				if (this.lstDoors.SelectedItems.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strSelectDoor);
					return;
				}
				this.fileName = wgAppConfig.Path4Doc() + "Pri" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
				this.consumerNO = this.user.ConsumerNONext();
				if (this.consumerNO < long.Parse(DateTime.Now.ToString("yyyyMMdd000001")))
				{
					this.consumerNO = long.Parse(DateTime.Now.ToString("yyyyMMdd000001"));
				}
				this.addedPrivilegeCnt = 0L;
				this.btnRealtimeGetRecords.Enabled = false;
				this.btnStopOthers.BackColor = Color.Red;
				this.btnStopMonitor.BackColor = Color.Red;
				this.btnGetRecords.Enabled = false;
				this.mnuGetRecordsToolStripMenuItem.Enabled = false;
				this.btnUpload.Enabled = false;
				this.mnuUploadToolStripMenuItem.Enabled = false;
				this.arrSelectedDoors.Clear();
				this.arrSelectedDoorsItem.Clear();
				this.dealtDoorIndex = 0;
				this.arrDealtController.Clear();
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					this.arrSelectedDoors.Add(listViewItem.Text);
					this.arrSelectedDoorsItem.Add(listViewItem);
				}
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = string.Format(CommonStr.strPrivilegeExportFile, new object[0]),
					information = string.Format(this.fileName.Replace(Application.StartupPath + "\\", ""), new object[0])
				});
				using (icController icController = new icController())
				{
					icController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
					wgRunInfoLog.addEvent(new InfoRow
					{
						desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), icController.ControllerSN),
						information = string.Format("{0}", CommonStr.strGetPrivilegesOfRentingHouseStart)
					});
				}
				this.displayNewestLog();
				this.CommOperate = "GETPRIVILEGES_RENTING_HOUSE";
				this.bkUploadAndGetRecords.RunWorkerAsync();
				if (!this.bkDispDoorStatus.IsBusy)
				{
					this.bkDispDoorStatus.RunWorkerAsync();
				}
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00086FD0 File Offset: 0x00085FD0
		private void btnRemoteOpen_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			if (wgAppConfig.IsAcceleratorActive)
			{
				this.remoteOpenMultithreadToolStripMenuItem_Click(sender, e);
				return;
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				if (this.lstDoors.SelectedItems.Count == 1)
				{
					if (XMessageBox.Show(sender.ToString() + " " + this.lstDoors.SelectedItems[0].Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						return;
					}
				}
				else if (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return;
				}
			}
			using (icController icController = new icController())
			{
				this.bStopComm = false;
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (this.bStopComm)
					{
						break;
					}
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					if (icController.RemoteOpenDoorIP(listViewItem.Text) <= 0)
					{
						wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0}", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK))
						});
						this.dispDoorStatusByIPComm(icController, listViewItem);
					}
					this.displayNewestLog();
				}
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000871A0 File Offset: 0x000861A0
		private void btnClearRunInfo_Click(object sender, EventArgs e)
		{
			(this.dgvRunInfo.DataSource as DataView).Table.Clear();
			this.txtInfo.Text = "";
			this.richTxtInfo.Text = "";
			this.lblInfoID.Text = "";
			this.pictureBox1.Visible = false;
			if (!string.IsNullOrEmpty(this.oldInfoTitleString))
			{
				this.dataGridView2.Columns[0].HeaderText = this.oldInfoTitleString;
			}
			this.arrCapturedFilenameRecID.Clear();
			this.arrCapturedFilename.Clear();
			wgRunInfoLog.eventRecID = 0;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00087248 File Offset: 0x00086248
		private void locateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.frm4ShowLocate == null)
				{
					this.frm4ShowLocate = new dfrmLocate();
					this.frm4ShowLocate.Show(this);
				}
				else
				{
					try
					{
						if (this.frm4ShowLocate.WindowState == FormWindowState.Minimized)
						{
							this.frm4ShowLocate.WindowState = FormWindowState.Normal;
						}
						this.frm4ShowLocate.Show();
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
						this.frm4ShowLocate = null;
						this.frm4ShowLocate = new dfrmLocate();
						this.frm4ShowLocate.Show(this);
					}
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x000872F8 File Offset: 0x000862F8
		private void personInsideToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.frm4ShowPersonsInside == null)
				{
					this.frm4ShowPersonsInside = new dfrmPersonsInside();
					this.frm4ShowPersonsInside.frmCall = this;
					this.frm4ShowPersonsInside.Show(this);
				}
				else
				{
					try
					{
						if (this.frm4ShowPersonsInside.WindowState == FormWindowState.Minimized)
						{
							this.frm4ShowPersonsInside.WindowState = FormWindowState.Normal;
						}
						this.frm4ShowPersonsInside.frmCall = this;
						this.frm4ShowPersonsInside.Show(this);
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
						this.frm4ShowPersonsInside = null;
						this.frm4ShowPersonsInside = new dfrmPersonsInside();
						this.frm4ShowPersonsInside.frmCall = this;
						this.frm4ShowPersonsInside.Show(this);
					}
				}
				this.bPersonInsideStarted = true;
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x000873D4 File Offset: 0x000863D4
		private void btnMaps_Click(object sender, EventArgs e)
		{
			if (this.frmMaps1 != null)
			{
				try
				{
					this.frmMaps1.Dispose();
					this.frmMaps1 = null;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			this.lstDoors.SelectedItems.Clear();
			this.frmMaps1 = new frmMaps();
			this.frmMaps1.tbRunInfoLog = this.tbRunInfoLog;
			this.frmMaps1.lstDoors = this.lstDoors;
			this.frmMaps1.btnMonitor = this.btnServer;
			this.frmMaps1.contextMenuStrip1Doors = this.contextMenuStrip1Doors;
			this.frmMaps1.btnStop = this.btnStopOthers;
			this.frmMaps1.Show(this);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00087498 File Offset: 0x00086498
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.dfrmFind1 == null)
			{
				this.dfrmFind1 = new dfrmFind();
				this.dfrmFind1.StartPosition = FormStartPosition.Manual;
				this.dfrmFind1.Location = new Point(600, 8);
			}
			this.dfrmFind1.setObjtoFind(this.lstDoors, null);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000874EC File Offset: 0x000864EC
		private void cboZone_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cboZone, this.cboUsers);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00087500 File Offset: 0x00086500
		private void cboZone_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.cboZone.ToolTipText = this.cboZone.Text;
			if (this.oldZoneText == null || this.oldZoneText != this.cboZone.Text)
			{
				this.oldZoneText = this.cboZone.Text;
				string text = "";
				if (this.dvDoors != null)
				{
					this.lstDoors.SelectedItems.Clear();
					DataView dataView = this.dvDoors;
					if (this.cboZone.SelectedIndex < 0 || (this.cboZone.SelectedIndex == 0 && (int)this.arrZoneID[0] == 0))
					{
						dataView.RowFilter = "";
						text = "";
						this.lstDoors.BeginUpdate();
						this.listViewNotDisplay.BeginUpdate();
						foreach (object obj in this.lstDoors.Items)
						{
							ListViewItem listViewItem = (ListViewItem)obj;
							this.lstDoors.Items.Remove(listViewItem);
							this.listViewNotDisplay.Items.Add(listViewItem);
						}
						this.listViewNotDisplay.EndUpdate();
						this.lstDoors.EndUpdate();
						if (this.listViewNotDisplay.Items.Count == 0)
						{
							wgAppRunInfo.raiseAppRunInfoLoadNums(this.lstDoors.Items.Count.ToString());
							return;
						}
					}
					else
					{
						if (this.lstDoors.Items.Count + this.listViewNotDisplay.Items.Count > 100)
						{
							this.dfrmWait1.Show();
							this.dfrmWait1.Refresh();
						}
						this.lstDoors.BeginUpdate();
						this.listViewNotDisplay.BeginUpdate();
						foreach (object obj2 in this.lstDoors.Items)
						{
							ListViewItem listViewItem2 = (ListViewItem)obj2;
							this.lstDoors.Items.Remove(listViewItem2);
							this.listViewNotDisplay.Items.Add(listViewItem2);
						}
						this.listViewNotDisplay.EndUpdate();
						this.lstDoors.EndUpdate();
						dataView.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
						text = " f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
						int num = (int)this.arrZoneID[this.cboZone.SelectedIndex];
						int num2 = (int)this.arrZoneNO[this.cboZone.SelectedIndex];
						int zoneChildMaxNo = icControllerZone.getZoneChildMaxNo(this.cboZone.Text, this.arrZoneName, this.arrZoneNO);
						if (num2 > 0)
						{
							if (num2 >= zoneChildMaxNo)
							{
								dataView.RowFilter = string.Format(" f_ZoneID ={0:d} ", num);
								text = string.Format(" f_ZoneID ={0:d} ", num);
							}
							else
							{
								dataView.RowFilter = "";
								string zoneQuery = icGroup.getZoneQuery(num2, zoneChildMaxNo, this.arrZoneNO, this.arrZoneID);
								dataView.RowFilter = string.Format("  {0} ", zoneQuery);
								text = string.Format("  {0} ", zoneQuery);
							}
						}
						dataView.RowFilter = string.Format(" {0} ", text);
					}
					if (this.lstDoors.Items.Count + this.listViewNotDisplay.Items.Count > 100)
					{
						this.dfrmWait1.Show();
						this.dfrmWait1.Refresh();
					}
					this.lstDoors.BeginUpdate();
					this.listViewNotDisplay.BeginUpdate();
					foreach (object obj3 in this.listViewNotDisplay.Items)
					{
						ListViewItem listViewItem3 = (ListViewItem)obj3;
						if (text != "")
						{
							this.dvDoors.RowFilter = string.Format("({0}) AND (f_DoorName = {1})", text, wgTools.PrepareStr(listViewItem3.Text));
						}
						else
						{
							this.dvDoors.RowFilter = string.Format("f_DoorName = {0}", wgTools.PrepareStr(listViewItem3.Text));
						}
						if (this.dvDoors.Count > 0)
						{
							this.listViewNotDisplay.Items.Remove(listViewItem3);
							this.lstDoors.Items.Add(listViewItem3);
						}
					}
					this.dvDoors.RowFilter = "";
					this.listViewNotDisplay.EndUpdate();
					this.lstDoors.EndUpdate();
					this.dfrmWait1.Hide();
					wgTools.WriteLine("foreach (ListViewItem itm in listViewNotDisplay.Items)");
					wgAppRunInfo.raiseAppRunInfoLoadNums(this.lstDoors.Items.Count.ToString());
					return;
				}
				wgAppRunInfo.raiseAppRunInfoLoadNums("0");
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00087A30 File Offset: 0x00086A30
		private void btnStopMonitor_Click(object sender, EventArgs e)
		{
			if (this.watching != null)
			{
				this.watching.WatchingController = null;
				this.timerUpdateDoorInfo.Enabled = false;
				wgAppRunInfo.raiseAppRunInfoMonitors("0");
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00087A5C File Offset: 0x00086A5C
		private void mnuCheck_Click(object sender, EventArgs e)
		{
			this.btnCheck.PerformClick();
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00087A6C File Offset: 0x00086A6C
		private void mnuWarnReset_Click(object sender, EventArgs e)
		{
			using (icController icController = new icController())
			{
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					if (icController.WarnResetIP() <= 0)
					{
						wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0}", sender.ToString())
						});
					}
					this.displayNewestLog();
				}
				icController.Dispose();
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00087B6C File Offset: 0x00086B6C
		private void resetPersonInsideToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			if (this.lstDoors.SelectedItems.Count <= 0 || XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				using (icController icController = new icController())
				{
					ArrayList arrayList = new ArrayList();
					foreach (object obj in this.lstDoors.SelectedItems)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
						if (arrayList.IndexOf(icController.ControllerSN) < 0)
						{
							long commTimeoutMsMin = wgUdpComm.CommTimeoutMsMin;
							try
							{
								wgUdpComm.CommTimeoutMsMin = 2000L;
								if (icController.UpdateFRamIP(268435458U, 0U) <= 0)
								{
									wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
								}
								else
								{
									wgRunInfoLog.addEvent(new InfoRow
									{
										desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
										information = string.Format("{0}", sender.ToString())
									});
									arrayList.Add(icController.ControllerSN);
								}
							}
							catch (Exception ex)
							{
								wgAppConfig.wgLog(ex.ToString());
							}
							wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
						}
						else
						{
							wgRunInfoLog.addEvent(new InfoRow
							{
								desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
								information = string.Format("{0}", sender.ToString())
							});
						}
						this.displayNewestLog();
					}
					icController.Dispose();
				}
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00087DA0 File Offset: 0x00086DA0
		private void lEDConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (frmWatchingLED frmWatchingLED = new frmWatchingLED())
				{
					frmWatchingLED.frmCall = this;
					frmWatchingLED.ShowDialog(this);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00087DFC File Offset: 0x00086DFC
		private void lCDShowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (!this.bStartRealtimeGetRecords || !this.bPersonInsideStarted)
				{
					if (!this.bStartRealtimeGetRecords)
					{
						this.bDirectToRealtimeGet = true;
						this.btnRealtimeGetRecords.PerformClick();
					}
					if (!this.bPersonInsideStarted)
					{
						this.personInsideToolStripMenuItem_Click(null, null);
					}
				}
				if (this.frmWatchingLCD != null && !this.frmWatchingLCD.Visible)
				{
					this.frmWatchingLCD.Close();
					this.frmWatchingLCD = null;
				}
				if (this.frmWatchingLCD == null)
				{
					this.frmWatchingLCD = new frmWatchingLCD();
					this.frmWatchingLCD.frmCall = this;
					this.frmWatchingLCD.tbRunInfoLog = this.tbRunInfoLog;
					this.frmWatchingLCD.bDisplayCapturedPhoto = this.existedVideoCHDll > 0;
				}
				this.frmWatchingLCD.Show(this);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00087EDC File Offset: 0x00086EDC
		private void allowAntiPassbackFirstExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			if (this.lstDoors.SelectedItems.Count <= 0 || XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				using (icController icController = new icController())
				{
					ArrayList arrayList = new ArrayList();
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					if (sender == this.enableAllowAntiPassbackFirstExitToolStripMenuItem)
					{
						wgMjControllerConfigure.autiback_allow_firstout_enable = 165;
					}
					else
					{
						wgMjControllerConfigure.autiback_allow_firstout_enable = 0;
					}
					foreach (object obj in this.lstDoors.SelectedItems)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
						if (arrayList.IndexOf(icController.ControllerSN) < 0)
						{
							try
							{
								if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) <= 0)
								{
									wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
								}
								else
								{
									wgRunInfoLog.addEvent(new InfoRow
									{
										desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
										information = string.Format("{0}", sender.ToString())
									});
									arrayList.Add(icController.ControllerSN);
								}
								goto IL_01A3;
							}
							catch (Exception ex)
							{
								wgAppConfig.wgLog(ex.ToString());
								goto IL_01A3;
							}
							goto IL_015C;
						}
						goto IL_015C;
						IL_01A3:
						this.displayNewestLog();
						continue;
						IL_015C:
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0}", sender.ToString())
						});
						goto IL_01A3;
					}
					icController.Dispose();
				}
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00088114 File Offset: 0x00087114
		private void setDoorControlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnDirectSetDoorControl();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0008811C File Offset: 0x0008711C
		private void btnDirectSetDoorControl()
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				this.btnSelectAll_Click(null, null);
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				if (wgAppConfig.IsAcceleratorActive)
				{
					this.setDoorControlToolStripMenuItem1_Click(null, null);
					return;
				}
				int num = -1;
				int num2 = -1;
				using (dfrmControllerDoorControlSet dfrmControllerDoorControlSet = new dfrmControllerDoorControlSet())
				{
					if (this.lstDoors.SelectedItems.Count == 1)
					{
						dfrmControllerDoorControlSet.Text = dfrmControllerDoorControlSet.Text + "--" + this.lstDoors.SelectedItems[0].Text;
					}
					else
					{
						dfrmControllerDoorControlSet.Text = string.Concat(new string[]
						{
							dfrmControllerDoorControlSet.Text,
							"--",
							CommonStr.strDoorsNum,
							" = ",
							this.lstDoors.SelectedItems.Count.ToString()
						});
						if (this.lstDoors.Items.Count == this.lstDoors.SelectedItems.Count)
						{
							dfrmControllerDoorControlSet.Text += CommonStr.strAll;
						}
					}
					if (dfrmControllerDoorControlSet.ShowDialog(this) == DialogResult.OK)
					{
						num = dfrmControllerDoorControlSet.doorControl;
						num2 = dfrmControllerDoorControlSet.doorOpenDelay;
					}
					if (num < 0 && num2 < 0)
					{
						return;
					}
				}
				using (icController icController = new icController())
				{
					this.bStopComm = false;
					foreach (object obj in this.lstDoors.SelectedItems)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if (this.bStopComm)
						{
							break;
						}
						if (num >= 0)
						{
							icController.SetdoorControlDelalyInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo, num, -1);
							icController.UpdateIntoDB(false);
							if (icController.DirectSetDoorControlIP(listViewItem.Text, num) <= 0)
							{
								wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
							}
							else
							{
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
									information = string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(num))
								});
								this.dispDoorStatusByIPComm(icController, listViewItem);
							}
						}
						if (num2 >= 0)
						{
							icController.SetdoorControlDelalyInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo, -1, num2);
							icController.UpdateIntoDB(false);
							if (icController.DirectSetDoorOpenDelayIP(listViewItem.Text, num2) <= 0)
							{
								wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
							}
							else
							{
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
									information = string.Format("{0}{1}", CommonStr.strDirectSetDoorOpenDelay, num2.ToString())
								});
								this.dispDoorStatusByIPComm(icController, listViewItem);
							}
						}
						this.displayNewestLog();
					}
					icController.Dispose();
				}
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00088480 File Offset: 0x00087480
		private void switchViewStyleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.dtlstDoorViewChange = DateTime.Now;
				if (this.lstDoors.View == View.Details)
				{
					this.lstDoors.View = View.LargeIcon;
				}
				else if (this.lstDoors.View == View.LargeIcon)
				{
					this.lstDoors.View = View.List;
				}
				else if (this.lstDoors.View == View.List)
				{
					this.lstDoors.View = View.SmallIcon;
				}
				else if (this.lstDoors.View == View.SmallIcon)
				{
					this.lstDoors.View = View.Tile;
				}
				else if (this.lstDoors.View == View.Tile)
				{
					this.lstDoors.View = View.LargeIcon;
				}
				else
				{
					this.lstDoors.View = View.LargeIcon;
				}
				wgTools.WgDebugWrite(this.lstDoors.View.ToString(), new object[0]);
				wgAppConfig.UpdateKeyVal("CONSOLE_DOORVIEW", this.lstDoors.View.ToString());
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00088590 File Offset: 0x00087590
		private void restoreDefaultStyleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.lstDoors.View = View.LargeIcon;
				wgAppConfig.UpdateKeyVal("CONSOLE_DOORVIEW", "");
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000885D8 File Offset: 0x000875D8
		private void quickFormatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.quickFormat();
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000885E0 File Offset: 0x000875E0
		private void quickFormat()
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				if (this.lstDoors.SelectedItems.Count == 1)
				{
					if (XMessageBox.Show(CommonStr.strFormat + " " + this.lstDoors.SelectedItems[0].Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						return;
					}
				}
				else if (XMessageBox.Show(CommonStr.strFormat + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return;
				}
			}
			using (icController icController = new icController())
			{
				this.bStopComm = false;
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (this.bStopComm)
					{
						break;
					}
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					if (!arrayList.Contains(icController.ControllerSN))
					{
						arrayList.Add(icController.ControllerSN);
						if (icController.GetControllerRunInformationIP(-1) <= 0)
						{
							wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
						}
						else if (wgTools.doubleParse(icController.runinfo.driverVersion.Substring(1)) >= 5.48)
						{
							arrayList2.Add(icController.ControllerSN);
							this.dispDoorStatusByIPComm(icController, listViewItem);
						}
						else
						{
							wgRunInfoLog.addEvent(new InfoRow
							{
								desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
								information = string.Format("{0}:{1}", icController.runinfo.driverVersion, CommonStr.strFormatNotSupport)
							});
							this.dispDoorStatusByIPComm(icController, listViewItem);
						}
						this.displayNewestLog();
					}
				}
				foreach (object obj2 in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem2 = (ListViewItem)obj2;
					if (this.bStopComm)
					{
						break;
					}
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem2.Text, this.dsWatchingDoorInfo);
					if (arrayList2.Contains(icController.ControllerSN))
					{
						arrayList2.Remove(icController.ControllerSN);
						if (!WGPacket.bCommP)
						{
							byte[] array = new byte[1152];
							this.ipweb_webDisable(ref array);
							for (int i = 64; i < 80; i++)
							{
								array[i] = byte.MaxValue;
								array[1024 + (i >> 3)] = array[1024 + (i >> 3)] | (byte)(1 << (i & 7));
							}
							icController.UpdateConfigureCPUSuperIP(array, "");
						}
						byte[] array2 = new byte[1152];
						array2[1027] = 165;
						array2[1026] = 165;
						array2[1025] = 165;
						array2[1024] = 165;
						icController.UpdateConfigureSuperIP(array2);
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem2.Text, icController.ControllerSN),
							information = string.Format("{0}", CommonStr.strFormat)
						});
						this.displayNewestLog();
					}
				}
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000889DC File Offset: 0x000879DC
		private void disablePCControlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				if (this.lstDoors.SelectedItems.Count == 1)
				{
					if (XMessageBox.Show(sender.ToString() + " " + this.lstDoors.SelectedItems[0].Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						return;
					}
				}
				else if (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return;
				}
			}
			this.btnStopOthers.BackColor = Color.Red;
			this.btnStopMonitor.BackColor = Color.Red;
			using (icController icController = new icController())
			{
				this.bStopComm = false;
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure.pcControlSwipeTimeout = 0;
				if (this.lstDoors.SelectedItems.Count == this.lstDoors.Items.Count)
				{
					icController.ControllerSN = -1;
					icController.UpdateConfigureIP(wgMjControllerConfigure, -1);
				}
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (this.bStopComm)
					{
						break;
					}
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) <= 0)
					{
						wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0}:{1}", this.disablePCControlToolStripMenuItem.Text, CommonStr.strSuccessfully)
						});
						this.dispDoorStatusByIPComm(icController, listViewItem);
					}
					this.displayNewestLog();
				}
			}
			if (this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting && this.btnServer.Text != CommonStr.strMonitoring)
			{
				this.btnStopOthers.BackColor = Color.Transparent;
				this.btnStopMonitor.BackColor = Color.Transparent;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00088C64 File Offset: 0x00087C64
		private void mnuMonitorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnServer.PerformClick();
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00088C71 File Offset: 0x00087C71
		private void mnuAdjustTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnSetTime.PerformClick();
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00088C7E File Offset: 0x00087C7E
		private void mnuUploadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnUpload.PerformClick();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00088C8B File Offset: 0x00087C8B
		private void mnuGetRecordsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnGetRecords.PerformClick();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00088C98 File Offset: 0x00087C98
		private void remoteOpenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnRemoteOpen.PerformClick();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00088CA8 File Offset: 0x00087CA8
		private void upDrv_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			this.openFileDialog1.Filter = " (*.*)|*.*";
			this.openFileDialog1.RestoreDirectory = true;
			try
			{
				this.openFileDialog1.InitialDirectory = ".\\DOC";
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.openFileDialog1.FileName = "";
			if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				string text = this.openFileDialog1.FileName;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				Application.DoEvents();
				using (icController icController = new icController())
				{
					this.bStopComm = false;
					ArrayList arrayList = new ArrayList();
					foreach (object obj in this.lstDoors.SelectedItems)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if (this.bStopComm)
						{
							break;
						}
						icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
						if (arrayList.IndexOf(icController.ControllerSN) < 0)
						{
							arrayList.Add(icController.ControllerSN);
							DateTime dateTime = DateTime.Now.AddSeconds(3.0);
							DateTime dateTime2 = DateTime.Now;
							while (dateTime > dateTime2 && icController.GetControllerRunInformationIP(-1) <= 0)
							{
								Thread.Sleep(1000);
								Application.DoEvents();
							}
							if (icController.runinfo.wgcticks == 0U)
							{
								wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
							}
							else if (wgTools.doubleParse(icController.runinfo.driverVersion.Substring(1)) < 6.54)
							{
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
									information = string.Format("[{0:d}]{1}:{2} {3}", new object[]
									{
										icController.ControllerSN,
										CommonStr.strFirmware,
										icController.runinfo.driverVersion,
										CommonStr.strDriverVersionLessThan654
									})
								});
							}
							else
							{
								string driverVersion = icController.runinfo.driverVersion;
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
									information = string.Format("[{0:d}]{1}:{2} {3}", new object[]
									{
										icController.ControllerSN,
										CommonStr.strFirmware,
										icController.runinfo.driverVersion,
										CommonStr.strDriverUpdateStart
									})
								});
								this.displayNewestLog();
								if (icController.WriteDrvToDataFlash(text) <= 0)
								{
									wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
									this.displayNewestLog();
								}
								dateTime = DateTime.Now.AddSeconds(60.0);
								dateTime2 = DateTime.Now;
								while (dateTime > dateTime2)
								{
									if (icController.GetControllerRunInformationIP(-1) > 0)
									{
										wgRunInfoLog.addEvent(new InfoRow
										{
											desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
											information = string.Format("[{0:d}]{1}:{2}=>{3}", new object[]
											{
												icController.ControllerSN,
												CommonStr.strDriverUpdateOK,
												driverVersion,
												icController.runinfo.driverVersion
											})
										});
										this.displayNewestLog();
										break;
									}
									Thread.Sleep(1000);
									Application.DoEvents();
								}
							}
						}
					}
				}
				this.dfrmWait1.Hide();
				return;
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000890F8 File Offset: 0x000880F8
		private void displayCloudControllersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					if (wgTools.arrSNReceived.Count > 0)
					{
						using (dfrmCloudServers dfrmCloudServers = new dfrmCloudServers())
						{
							if (dfrmCloudServers.ShowDialog(this) == DialogResult.OK && XMessageBox.Show(CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
							{
								wgAppConfig.gRestart = true;
								((frmADCT3000)base.ParentForm).mnuExit.PerformClick();
							}
							return;
						}
					}
					XMessageBox.Show(CommonStr.strCloudControllerNotFound);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000891B4 File Offset: 0x000881B4
		private void qRInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			using (dfrmCreateQR4Door dfrmCreateQR4Door = new dfrmCreateQR4Door())
			{
				dfrmCreateQR4Door.doorName = "";
				using (icController icController = new icController())
				{
					using (IEnumerator enumerator = this.lstDoors.SelectedItems.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							ListViewItem listViewItem = (ListViewItem)enumerator.Current;
							icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
							dfrmCreateQR4Door.controllerSN = (long)icController.ControllerSN;
							dfrmCreateQR4Door.doorName = listViewItem.Text;
							dfrmCreateQR4Door.doorNO = (long)icController.GetDoorNO(listViewItem.Text);
							dfrmCreateQR4Door.ShowDialog(this);
						}
					}
				}
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000892C0 File Offset: 0x000882C0
		private void watchingDogConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			int num = 0;
			if (sender == this.watchingDogDeactiveToolStripMenuItem)
			{
				num = 0;
			}
			else if (sender == this.watchingDog15minToolStripMenuItem)
			{
				num = 15;
			}
			else
			{
				if (sender != this.watchingDog30minToolStripMenuItem)
				{
					if (sender == this.watchingDogCustomToolStripMenuItem)
					{
						num = 0;
						using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
						{
							dfrmInputNewName.Text = (sender as ToolStripItem).Text;
							if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(dfrmInputNewName.strNewName))
							{
								int num2 = 0;
								if (int.TryParse(dfrmInputNewName.strNewName, NumberStyles.AllowHexSpecifier, null, out num2))
								{
									if (true)
									{
										num = num2;
										goto IL_00C7;
									}
									XMessageBox.Show(CommonStr.strInvalidValue);
								}
							}
						}
					}
					return;
				}
				num = 30;
			}
			IL_00C7:
			if (this.lstDoors.SelectedItems.Count > 0 && XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
			{
				return;
			}
			using (icController icController = new icController())
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					if (arrayList.IndexOf(icController.ControllerSN) < 0)
					{
						long commTimeoutMsMin = wgUdpComm.CommTimeoutMsMin;
						try
						{
							wgUdpComm.CommTimeoutMsMin = 2000L;
							if (icController.UpdateConfigureIP(new wgMjControllerConfigure
							{
								check_controller_online_timeout = num
							}, -1) <= 0)
							{
								wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
							}
							else
							{
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
									information = string.Format("{0}", sender.ToString())
								});
								arrayList.Add(icController.ControllerSN);
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0}", sender.ToString())
						});
					}
					this.displayNewestLog();
				}
				icController.Dispose();
				if (num > 0 && !(wgAppConfig.GetKeyVal("logRecCommFail") == "1") && !(wgAppConfig.GetKeyVal("logRecCommFail") == "2"))
				{
					wgAppConfig.UpdateKeyVal("logRecCommFail", "1");
					int.TryParse(wgAppConfig.GetKeyVal("logRecCommFail"), out wgRunInfoLog.logRecCommFail);
				}
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00089628 File Offset: 0x00088628
		private void normalOpenTimeListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmControllerNormalOpenTimeList dfrmControllerNormalOpenTimeList = new dfrmControllerNormalOpenTimeList())
			{
				dfrmControllerNormalOpenTimeList.ShowDialog(this);
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00089660 File Offset: 0x00088660
		private void swipeDataCenterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				try
				{
					Process process = frmConsole.RunningInstance();
					if (process != null && process.MainWindowTitle.ToUpper().Equals("DATACENTER"))
					{
						return;
					}
				}
				catch
				{
				}
				try
				{
					string text = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
					if (!string.IsNullOrEmpty(text))
					{
						text = Program.Dpt4Database(text);
					}
					wgAppConfig.wgDebugWrite(Process.Start(new ProcessStartInfo
					{
						FileName = Application.StartupPath + "\\N3000.exe",
						Arguments = string.Format(" -USER {0} -PASSWORD '{1}'  -DATACENTER", wgTools.PrepareStr(icOperator.OperatorName), text),
						UseShellExecute = true
					}).ToString());
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00089778 File Offset: 0x00088778
		private void downloadMultithreadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			this.arrSelectedDoors.Clear();
			this.arrSelectedDoorsItem.Clear();
			this.dealtDoorIndex = 0;
			this.arrDealtController.Clear();
			foreach (object obj in this.lstDoors.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				this.arrSelectedDoors.Add(listViewItem.Text);
				this.arrSelectedDoorsItem.Add(listViewItem);
			}
			if (this.dfrmMultiThreadOperation1Privilege != null)
			{
				try
				{
					this.dfrmMultiThreadOperation1Privilege.Dispose();
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			this.dfrmMultiThreadOperation1Privilege = new dfrmMultiThreadOperation();
			this.dfrmMultiThreadOperation1Privilege.arrSelectedDoorsOnConsole = this.arrSelectedDoors;
			this.dfrmMultiThreadOperation1Privilege.arrSelectedDoorsItem = this.arrSelectedDoorsItem;
			this.dfrmMultiThreadOperation1Privilege.Show(this);
			this.dfrmMultiThreadOperation1Privilege.startDownload();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000898A8 File Offset: 0x000888A8
		private void uploadMultithreadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			this.arrSelectedDoors.Clear();
			this.arrSelectedDoorsItem.Clear();
			this.dealtDoorIndex = 0;
			this.arrDealtController.Clear();
			foreach (object obj in this.lstDoors.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				this.arrSelectedDoors.Add(listViewItem.Text);
				this.arrSelectedDoorsItem.Add(listViewItem);
			}
			dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
			dfrmMultiThreadOperation.arrSelectedDoorsOnConsole = this.arrSelectedDoors;
			dfrmMultiThreadOperation.arrSelectedDoorsItem = this.arrSelectedDoorsItem;
			dfrmMultiThreadOperation.Show(this);
			dfrmMultiThreadOperation.startUpload();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00089990 File Offset: 0x00088990
		private void remoteOpenMultithreadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectDoor);
				return;
			}
			this.arrSelectedDoors.Clear();
			this.arrSelectedDoorsItem.Clear();
			this.dealtDoorIndex = 0;
			this.arrDealtController.Clear();
			foreach (object obj in this.lstDoors.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				this.arrSelectedDoors.Add(listViewItem.Text);
				this.arrSelectedDoorsItem.Add(listViewItem);
			}
			dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
			dfrmMultiThreadOperation.arrSelectedDoorsOnConsole = this.arrSelectedDoors;
			dfrmMultiThreadOperation.arrSelectedDoorsItem = this.arrSelectedDoorsItem;
			dfrmMultiThreadOperation.Show(this);
			dfrmMultiThreadOperation.startRemoteOpen();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00089A78 File Offset: 0x00088A78
		private void setDoorControlToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				this.btnSelectAll_Click(null, null);
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				this.arrSelectedDoors.Clear();
				this.arrSelectedDoorsItem.Clear();
				this.dealtDoorIndex = 0;
				this.arrDealtController.Clear();
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					this.arrSelectedDoors.Add(listViewItem.Text);
					this.arrSelectedDoorsItem.Add(listViewItem);
				}
				dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
				dfrmMultiThreadOperation.arrSelectedDoorsOnConsole = this.arrSelectedDoors;
				dfrmMultiThreadOperation.arrSelectedDoorsItem = this.arrSelectedDoorsItem;
				dfrmMultiThreadOperation.Show(this);
				dfrmMultiThreadOperation.startSetDoorControl();
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00089B74 File Offset: 0x00088B74
		private void adjustTimeMultithreadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstDoors.SelectedItems.Count <= 0)
			{
				this.btnSelectAll_Click(null, null);
			}
			if (this.lstDoors.SelectedItems.Count > 0)
			{
				this.arrSelectedDoors.Clear();
				this.arrSelectedDoorsItem.Clear();
				this.dealtDoorIndex = 0;
				this.arrDealtController.Clear();
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					this.arrSelectedDoors.Add(listViewItem.Text);
					this.arrSelectedDoorsItem.Add(listViewItem);
				}
				dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
				dfrmMultiThreadOperation.arrSelectedDoorsOnConsole = this.arrSelectedDoors;
				dfrmMultiThreadOperation.arrSelectedDoorsItem = this.arrSelectedDoorsItem;
				dfrmMultiThreadOperation.Show(this);
				dfrmMultiThreadOperation.startAdjustTime();
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00089C70 File Offset: 0x00088C70
		private void clearRunInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnClearRunInfo.PerformClick();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00089C80 File Offset: 0x00088C80
		private void displayMoreSwipesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.frmMoreRecords != null && !this.frmMoreRecords.Visible)
				{
					this.frmMoreRecords.Close();
					this.frmMoreRecords = null;
				}
				if (this.frmMoreRecords == null)
				{
					this.frmMoreRecords = new frmWatchingMoreRecords();
					this.frmMoreRecords.tbRunInfoLog = this.tbRunInfoLog;
					this.frmMoreRecords.bDisplayCapturedPhoto = this.existedVideoCHDll > 0;
				}
				this.frmMoreRecords.Show(this);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00089D18 File Offset: 0x00088D18
		private void timerAutoLogin_Tick(object sender, EventArgs e)
		{
			this.timerAutoLogin.Enabled = false;
			if (wgAppConfig.IsAutoLogin)
			{
				try
				{
					wgAppConfig.IsAutoLogin = false;
					if (wgAppConfig.GetKeyVal("AutoLoginMode") == "2")
					{
						this.btnSelectAll_Click(null, null);
						this.btnServer.PerformClick();
					}
					else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "3")
					{
						if (frmConsole.RunningInstance() != null)
						{
							XMessageBox.Show(CommonStr.strOtherApplicationRunningRealtimeGet);
							return;
						}
						this.btnSelectAll_Click(null, null);
						this.bDirectToRealtimeGet = true;
						this.btnRealtimeGetRecords.PerformClick();
					}
					else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "4")
					{
						if (frmConsole.RunningInstance() != null)
						{
							XMessageBox.Show(CommonStr.strOtherApplicationRunningRealtimeGet);
							return;
						}
						this.btnSelectAll_Click(null, null);
						this.bDirectToRealtimeGet = true;
						this.btnRealtimeGetRecords.PerformClick();
						wgAppConfig.UpdateKeyVal("KEY_InterfaceLock", "1");
						using (dfrmInterfaceLock dfrmInterfaceLock = new dfrmInterfaceLock())
						{
							dfrmInterfaceLock.txtOperatorName.Text = icOperator.OperatorName;
							dfrmInterfaceLock.StartPosition = FormStartPosition.CenterScreen;
							dfrmInterfaceLock.ShowDialog(this);
						}
					}
					if (wgAppConfig.GetKeyVal("AutoLoginMode") == "5")
					{
						if (frmConsole.RunningInstance() != null)
						{
							XMessageBox.Show(CommonStr.strOtherApplicationRunningRealtimeGet);
							return;
						}
						this.btnSelectAll_Click(null, null);
						this.bDirectToRealtimeGet = true;
						this.btnRealtimeGetRecords.PerformClick();
						this.personInsideToolStripMenuItem_Click(null, null);
						this.lCDShowToolStripMenuItem_Click(null, null);
						this.frm4ShowPersonsInside.WindowState = FormWindowState.Minimized;
						this.frmWatchingLCD.WindowState = FormWindowState.Maximized;
					}
					else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "6")
					{
						if (frmConsole.RunningInstance() != null)
						{
							XMessageBox.Show(CommonStr.strOtherApplicationRunningRealtimeGet);
							return;
						}
						this.btnSelectAll_Click(null, null);
						this.bDirectToRealtimeGet = true;
						this.btnRealtimeGetRecords.PerformClick();
						this.personInsideToolStripMenuItem_Click(null, null);
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			wgRunInfoLog.tcpServerEnabled = 0;
			if (wgAppConfig.GetKeyVal("KEY_TCPServerConfigActive") == "1")
			{
				try
				{
					this.openTcpPort();
					wgRunInfoLog.addEvent(new InfoRow
					{
						desc = string.Format(CommonStr.strTCPServerStart, new object[0]),
						information = string.Format("TCP Server={0}, {1}", this.tcpServer1.Port, this.encodingOfTCP)
					});
					wgRunInfoLog.tcpServerEnabled = 1;
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLog(ex2.ToString());
				}
			}
			wgRunInfoLog.tcpServerAutoLoad = 1;
			if (wgAppConfig.GetKeyVal("KEY_TCPServerAutoUpload") == "0")
			{
				try
				{
					wgRunInfoLog.tcpServerAutoLoad = 0;
				}
				catch (Exception ex3)
				{
					wgAppConfig.wgLog(ex3.ToString());
				}
			}
			if (wgTools.bUDPCloud > 0)
			{
				wgAppConfig.wgLog(string.Format("UDP Cloud Start. Server IP ={0},  Port ={1}, Short={2}", wgTools.UDPCloudIP, wgTools.UDPCloudPort, wgTools.UDPCloudShortPort));
				if (this.watching == null)
				{
					if (wgTools.bUDPOnly64 > 0)
					{
						this.watching = frmADCT3000.watchingP64;
						InfoRow infoRow = new InfoRow();
						if (wgTools.bUDPOnly64 > 0)
						{
							infoRow.desc = string.Format("{0}[{1},{2}]", string.Format(CommonStr.strCloudServer + "-P64", new object[0]), wgTools.p64_gprs_watchingSendCycle, wgTools.p64_gprs_refreshCycleMax);
						}
						else
						{
							infoRow.desc = string.Format(CommonStr.strCloudServer + "-1!", new object[0]);
						}
						infoRow.information = string.Format("{0} ={1}:{2}", "IP", wgTools.UDPCloudIP, wgTools.UDPCloudPort);
						wgRunInfoLog.addEvent(infoRow);
					}
					else
					{
						this.watching = new WatchingService();
					}
					this.watching.EventHandler += this.evtNewInfoCallBack;
				}
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0008A140 File Offset: 0x00089140
		private void timerUpdateDoorInfo_Tick(object sender, EventArgs e)
		{
			try
			{
				this.timerUpdateDoorInfo.Enabled = false;
				if (!wgAppConfig.DBIsConnected)
				{
					this.bOccuredDBNotConnect = true;
				}
				if (this.watching != null)
				{
					if (this.bOccuredDBNotConnect && wgAppConfig.DBIsConnected)
					{
						if (this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.Stop && this.selectedControllersSNOfRealtimeGetRecords.Count > 0)
						{
							for (int i = 0; i < this.selectedControllersSNOfRealtimeGetRecords.Count; i++)
							{
								this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  ", this.selectedControllersSNOfRealtimeGetRecords[i].ToString());
								if (this.dvDoors4Watching.Count > 0 && this.doorsNeedToGetRecords.IndexOf(this.dvDoors4Watching[0]["f_DoorName"].ToString(), Math.Max(0, this.dealtIndexOfDoorsNeedToGetRecords + 1)) < 0)
								{
									this.doorsNeedToGetRecords.Add(this.dvDoors4Watching[0]["f_DoorName"].ToString());
								}
							}
						}
						this.bOccuredDBNotConnect = false;
					}
					if (this.Delay5SecUpdateDoor > 0)
					{
						this.Delay5SecUpdateDoor--;
					}
					else
					{
						this.updateSelectedDoorsStatus();
					}
					if (base.IsHandleCreated)
					{
						if (this.QueRecText.Count > 0)
						{
							base.Invoke(new frmConsole.txtInfoHaveNewInfo(this.txtInfoHaveNewInfoEntry));
						}
						wgAppRunInfo.raiseAppRunInfoLoadNums(frmConsole.infoRowsCount.ToString());
						Application.DoEvents();
						this.pcCheckAccess_DealOpen();
						this.refreshWarnAVI();
					}
					try
					{
						if (this.bCameraEnabled && this.photoavi != null)
						{
							DateTime dtShowByClick = this.photoavi.dtShowByClick;
							if (dtShowByClick.AddMilliseconds(3000.0) > DateTime.Now && this.lastPhotoCnt == 0)
							{
								this.dgvRunInfo_SelectionChanged(null, null);
							}
						}
					}
					catch
					{
					}
					this.timerUpdateDoorInfo.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			try
			{
				if (this.watching != null)
				{
					this.timerUpdateDoorInfo.Enabled = true;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0008A390 File Offset: 0x00089390
		private void updateSelectedDoorsStatus()
		{
			if (this.watching != null)
			{
				string text = "";
				foreach (object obj in this.lstDoors.Items)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if ((listViewItem.Tag as frmConsole.DoorSetInfo).Selected > 0 && this.watching.WatchingController != null && this.watching.WatchingController.ContainsKey((listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN))
					{
						ControllerRunInformation runInfo = this.watching.GetRunInfo((listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN);
						if (runInfo == null)
						{
							if (DateTime.Now > this.watchingStartTime.AddSeconds(3.0))
							{
								if (listViewItem.ImageIndex != 3)
								{
									listViewItem.ImageIndex = 3;
									if (wgRunInfoLog.logRecCommFail > 0)
									{
										wgRunInfoLog.addEvent(new InfoRow
										{
											desc = string.Format("{0}[{1:d}]", listViewItem.Text, (listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN),
											information = string.Format("{0}", CommonStr.strCommFail)
										});
										if (wgRunInfoLog.logRecCommFail > 1)
										{
											this.bWarnExisted = true;
											this.btnWarnExisted.Visible = this.bWarnExisted;
											this.btnWarnExisted.BackColor = Color.Red;
											this.timerWarn.Enabled = true;
										}
										this.displayNewestLog();
									}
								}
								if (wgRunInfoLog.logRecEventMode > 0 || wgRunInfoLog.tcpServerEnabled > 0)
								{
									text += string.Format("{0},{1},{2}\r\n", listViewItem.Text, CommonStr.strCommFail, 0);
								}
							}
						}
						else if (DateTime.Now > runInfo.refreshTime.AddSeconds((double)WatchingService.unconnect_timeout_sec))
						{
							if (this.watching.lastGetInfoDateTime.AddMilliseconds((double)WatchingService.Watching_Cycle_ms) > DateTime.Now)
							{
								if (listViewItem.ImageIndex != 3)
								{
									listViewItem.ImageIndex = 3;
									if (wgRunInfoLog.logRecCommFail > 0)
									{
										wgRunInfoLog.addEvent(new InfoRow
										{
											desc = string.Format("{0}[{1:d}]", listViewItem.Text, (listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN),
											information = string.Format("{0}", CommonStr.strCommFail)
										});
										if (wgRunInfoLog.logRecCommFail > 1)
										{
											this.bWarnExisted = true;
											this.btnWarnExisted.Visible = this.bWarnExisted;
											this.btnWarnExisted.BackColor = Color.Red;
											this.timerWarn.Enabled = true;
										}
										this.displayNewestLog();
									}
								}
								if (wgRunInfoLog.logRecEventMode > 0 || wgRunInfoLog.tcpServerEnabled > 0)
								{
									text += string.Format("{0},{1},{2}\r\n", listViewItem.Text, CommonStr.strCommFail, 0);
								}
							}
						}
						else
						{
							int imageIndex = listViewItem.ImageIndex;
							listViewItem.ImageIndex = runInfo.GetDoorImageIndex((listViewItem.Tag as frmConsole.DoorSetInfo).DoorNO);
							if (wgRunInfoLog.logRecEventMode > 0 || wgRunInfoLog.tcpServerEnabled > 0)
							{
								if (listViewItem.ImageIndex == 2 || listViewItem.ImageIndex == 5)
								{
									text += string.Format("{0},{1},{2}\r\n", listViewItem.Text, CommonStr.strDoorStatus_Open.Trim(), 1);
								}
								else if (listViewItem.ImageIndex == 1 || listViewItem.ImageIndex == 4)
								{
									text += string.Format("{0},{1},{2}\r\n", listViewItem.Text, CommonStr.strDoorStatus_Closed.Trim(), 2);
								}
							}
							if (listViewItem.ImageIndex > 2 && imageIndex != listViewItem.ImageIndex)
							{
								this.warnlastRecordCount = runInfo.newRecordsNum;
								this.bWarnExisted = true;
								this.btnWarnExisted.Visible = this.bWarnExisted;
								this.btnWarnExisted.BackColor = Color.Red;
								this.timerWarn.Enabled = true;
								try
								{
									if (string.IsNullOrEmpty(this.oldWarnInfo))
									{
										this.oldWarnInfo = this.btnWarnExisted.Text;
									}
									else
									{
										this.btnWarnExisted.Text = this.oldWarnInfo;
									}
									if (runInfo.WarnInfo((listViewItem.Tag as frmConsole.DoorSetInfo).DoorNO) > 0)
									{
										this.btnWarnExisted.Text = string.Format("{0} {1}", icDesc.WarnDetail((int)runInfo.WarnInfo((listViewItem.Tag as frmConsole.DoorSetInfo).DoorNO)), CommonStr.strClickConfirm);
									}
									else if (runInfo.appError > 0)
									{
										this.btnWarnExisted.Text = icDesc.ErrorDetail((int)runInfo.appError) + CommonStr.strErr + " " + CommonStr.strClickConfirm;
									}
									this.oldWarnInfoTips = string.Concat(new string[]
									{
										(listViewItem.Tag as frmConsole.DoorSetInfo).DoorName,
										" ",
										this.btnWarnExisted.Text,
										"\t",
										DateTime.Now.ToString("HH:mm:ss")
									});
									this.btnWarnExisted.ToolTipText = this.oldWarnInfoTips;
									this.btnWarnExisted.Text = (listViewItem.Tag as frmConsole.DoorSetInfo).DoorName + "\r\n" + this.btnWarnExisted.Text;
								}
								catch (Exception ex)
								{
									wgAppConfig.wgLog(ex.ToString());
								}
							}
							if (!this.bAllowOperateExtendPort && wgTools.doubleParse(runInfo.driverVersion.Substring(1)) >= 7.71 && (runInfo.pbdsStatusHigh & 128) > 0)
							{
								this.bAllowOperateExtendPort = true;
							}
						}
					}
				}
				if ((wgRunInfoLog.logRecEventMode > 0 || wgRunInfoLog.tcpServerEnabled > 0) && !this.runningStatus.Equals(text))
				{
					this.runningStatus = text;
					if (wgRunInfoLog.logRecEventMode > 0)
					{
						wgAppConfig.wgRunningStatusOfController(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff\r\n") + this.runningStatus);
					}
					if (wgRunInfoLog.tcpServerEnabled > 0 && wgRunInfoLog.tcpServerAutoLoad > 0)
					{
						this.sendTCPdata("DoorStatus:\r\n" + this.runningStatus);
					}
				}
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0008A9F0 File Offset: 0x000899F0
		private void timerWarn_Tick(object sender, EventArgs e)
		{
			this.timerWarn.Enabled = false;
			if (this.bWarnExisted)
			{
				if (this.btnWarnExisted.BackColor == Color.Red)
				{
					this.btnWarnExisted.BackColor = Color.Transparent;
				}
				else
				{
					this.btnWarnExisted.BackColor = Color.Red;
				}
				SystemSounds.Beep.Play();
				bool flag = true;
				if (this.bsoftwareWarnAutoResetWhenAllDoorAreClosed)
				{
					foreach (object obj in this.lstDoors.Items)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if ((listViewItem.Tag as frmConsole.DoorSetInfo).Selected > 0)
						{
							if (listViewItem.ImageIndex == 2 || listViewItem.ImageIndex == 5)
							{
								flag = false;
								break;
							}
							if (listViewItem.ImageIndex == 4)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						this.bWarnExisted = false;
						this.btnWarnExisted.Visible = this.bWarnExisted;
					}
				}
				this.timerWarn.Enabled = true;
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0008AB0C File Offset: 0x00089B0C
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.loadUserData4BackWork();
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0008AB50 File Offset: 0x00089B50
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0008AB70 File Offset: 0x00089B70
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
				return;
			}
			if (e.Error != null)
			{
				wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
				return;
			}
			this.loadUserData4BackWorkComplete(e.Result as DataTable);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0008ABD0 File Offset: 0x00089BD0
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dvConsumer4Access = new DataView(dtUser);
			DataSet dataSet = new DataSet();
			try
			{
				string text = " SELECT a.f_GroupID,a.f_GroupName  from t_b_Group a order by f_GroupName  + '\\' ASC";
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbDataAdapter.Fill(dataSet, "Groups");
							}
						}
						goto IL_00C5;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(dataSet, "Groups");
						}
					}
				}
				IL_00C5:
				this.dvGroups4Access = new DataView(dataSet.Tables["Groups"]);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				return;
			}
			this.bLoadTooManyRecords = true;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0008AD4C File Offset: 0x00089D4C
		private void itmDisplayStatusEntry(ListViewItem itm, int status)
		{
			try
			{
				if (itm != null)
				{
					itm.ImageIndex = status;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0008AD84 File Offset: 0x00089D84
		private void bkDispDoorStatus_DoWork(object sender, DoWorkEventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			using (icController icController = new icController())
			{
				this.watchingDealtDoorIndex = 0;
				Thread.Sleep(100);
				while (this.watchingDealtDoorIndex > -1 && this.watchingDealtDoorIndex < this.arrSelectedDoors.Count && !this.bStopComm)
				{
					if (this.watchingDealtDoorIndex <= this.dealtDoorIndex)
					{
						icController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.watchingDealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
						this.dispDoorStatusByIPComm(icController, (ListViewItem)this.arrSelectedDoorsItem[this.watchingDealtDoorIndex]);
						this.watchingDealtDoorIndex++;
					}
					else
					{
						Thread.Sleep(100);
					}
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0008AE64 File Offset: 0x00089E64
		private void dispDoorStatusByIPComm(icController control, ListViewItem itm)
		{
			try
			{
				if (wgTools.bUDPOnly64 <= 0 && base.IsHandleCreated)
				{
					if (control.GetControllerRunInformationIP(-1) <= 0)
					{
						base.Invoke(new frmConsole.itmDisplayStatus(this.itmDisplayStatusEntry), new object[] { itm, 3 });
					}
					else
					{
						base.Invoke(new frmConsole.itmDisplayStatus(this.itmDisplayStatusEntry), new object[]
						{
							itm,
							control.runinfo.GetDoorImageIndex(control.GetDoorNO(itm.Text))
						});
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0008AF14 File Offset: 0x00089F14
		private void bkRealtimeGetRecords_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				BackgroundWorker backgroundWorker = sender as BackgroundWorker;
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
				if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.GetRecordFirst)
				{
					this.swipe4GetRecords.Clear();
					int swipeRecordsByDoorName = this.swipe4GetRecords.GetSwipeRecordsByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
					if (swipeRecordsByDoorName >= 0 && this.realtimeGetRecordsSwipeIndexGot.ContainsKey(this.swipe4GetRecords.ControllerSN))
					{
						this.realtimeGetRecordsSwipeIndexGot[this.swipe4GetRecords.ControllerSN] = this.swipe4GetRecords.lastRecordFlashIndex;
					}
					e.Result = swipeRecordsByDoorName;
				}
				else if (this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.GetFinished && this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.StartMonitoring)
				{
					if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.WaitGetRecord)
					{
						while (!this.bStopComm)
						{
							if (this.doorsNeedToGetRecords.Count > 0)
							{
								if (this.dealtIndexOfDoorsNeedToGetRecords + 1 < this.doorsNeedToGetRecords.Count)
								{
									int controllerSN;
									using (icController icController = new icController())
									{
										icController.GetInfoFromDBByDoorNameSpecial(this.doorsNeedToGetRecords[this.dealtIndexOfDoorsNeedToGetRecords + 1].ToString(), this.dsWatchingDoorInfo);
										controllerSN = icController.ControllerSN;
									}
									if (this.realtimeGetRecordsSwipeIndexGot.ContainsKey(controllerSN) && this.realtimeGetRecordsSwipeIndexGot[controllerSN] > 0 && this.selectedControllersOfRealtimeGetRecords.ContainsKey(controllerSN) && this.selectedControllersOfRealtimeGetRecords[controllerSN].GetControllerRunInformationIP(-1) > 0 && (ulong)(this.selectedControllersOfRealtimeGetRecords[controllerSN].runinfo.lastGetRecordIndex + this.selectedControllersOfRealtimeGetRecords[controllerSN].runinfo.newRecordsNum) >= (ulong)((long)this.realtimeGetRecordsSwipeIndexGot[controllerSN]))
									{
										this.selectedControllersOfRealtimeGetRecords[controllerSN].UpdateLastGetRecordLocationIP((uint)this.realtimeGetRecordsSwipeIndexGot[controllerSN]);
										this.needDelSwipeControllers[controllerSN] = 0;
									}
									this.swipe4GetRecords.Clear();
									int swipeRecordsByDoorName2 = this.swipe4GetRecords.GetSwipeRecordsByDoorName(this.doorsNeedToGetRecords[this.dealtIndexOfDoorsNeedToGetRecords + 1].ToString());
									e.Result = swipeRecordsByDoorName2;
									if (swipeRecordsByDoorName2 >= 0 && this.realtimeGetRecordsSwipeIndexGot.ContainsKey(this.swipe4GetRecords.ControllerSN))
									{
										this.realtimeGetRecordsSwipeIndexGot[this.swipe4GetRecords.ControllerSN] = this.swipe4GetRecords.lastRecordFlashIndex;
										break;
									}
									break;
								}
								else if (this.doorsNeedToGetRecords.Count > 1000)
								{
									this.doorsNeedToGetRecords.Clear();
									this.dealtIndexOfDoorsNeedToGetRecords = -1;
								}
							}
							else
							{
								Thread.Sleep(1000);
							}
						}
					}
					else if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.DelSwipe && this.realtimeGetRecordsSwipeIndexGot.Count > 0)
					{
						foreach (object obj in this.selectedControllersSNOfRealtimeGetRecords)
						{
							int num = (int)obj;
							if (this.realtimeGetRecordsSwipeIndexGot.ContainsKey(num) && this.realtimeGetRecordsSwipeIndexGot[num] > 0 && this.selectedControllersOfRealtimeGetRecords.ContainsKey(num) && this.needDelSwipeControllers[num] == 1 && this.selectedControllersOfRealtimeGetRecords[num].GetControllerRunInformationIP(-1) > 0 && (ulong)(this.selectedControllersOfRealtimeGetRecords[num].runinfo.lastGetRecordIndex + this.selectedControllersOfRealtimeGetRecords[num].runinfo.newRecordsNum) >= (ulong)((long)this.realtimeGetRecordsSwipeIndexGot[num]))
							{
								this.selectedControllersOfRealtimeGetRecords[num].UpdateLastGetRecordLocationIP((uint)this.realtimeGetRecordsSwipeIndexGot[num]);
							}
						}
					}
				}
				if (backgroundWorker.CancellationPending)
				{
					e.Cancel = true;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0008B358 File Offset: 0x0008A358
		private void bkRealtimeGetRecords_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				if (e.Cancelled)
				{
					wgAppConfig.wgLog(CommonStr.strOperationCanceled);
				}
				else if (e.Error != null)
				{
					wgAppConfig.wgLog(string.Format("An error occurred: {0}", e.Error.Message));
				}
				else if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.GetRecordFirst)
				{
					this.getRecordsFromController(int.Parse(e.Result.ToString()));
					if (!this.bStopComm)
					{
						if (this.dealtDoorIndex == this.arrSelectedDoors.Count)
						{
							this.stepOfRealtimeGetRecords = frmConsole.StepOfRealtimeGetReocrds.GetFinished;
						}
						this.bkRealtimeGetRecords.RunWorkerAsync();
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = CommonStr.strStopComm,
							information = CommonStr.strStopComm
						});
						this.stepOfRealtimeGetRecords = frmConsole.StepOfRealtimeGetReocrds.Stop;
						this.btnRealtimeGetRecords.BackColor = Color.Transparent;
						this.btnRealtimeGetRecords.Text = CommonStr.strRealtimeGetRecords;
						wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strStopComm);
					}
				}
				else if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.GetFinished)
				{
					wgAppRunInfo.raiseAppRunInfoCommStatus("");
					if (this.watching == null)
					{
						if (wgTools.bUDPOnly64 > 0)
						{
							this.watching = frmADCT3000.watchingP64;
						}
						else
						{
							this.watching = new WatchingService();
						}
						this.watching.EventHandler += this.evtNewInfoCallBack;
					}
					this.timerUpdateDoorInfo.Enabled = false;
					this.watchingStartTime = DateTime.Now;
					wgTools.WriteLine("selectedControllers.Count=" + this.selectedControllersOfRealtimeGetRecords.Count.ToString());
					this.watching.WatchingController = this.selectedControllersOfRealtimeGetRecords;
					this.timerUpdateDoorInfo.Interval = 300;
					this.timerUpdateDoorInfo.Enabled = true;
					wgAppRunInfo.raiseAppRunInfoMonitors("2");
					this.stepOfRealtimeGetRecords = frmConsole.StepOfRealtimeGetReocrds.StartMonitoring;
					this.bkRealtimeGetRecords.RunWorkerAsync();
				}
				else if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.StartMonitoring)
				{
					this.stepOfRealtimeGetRecords = frmConsole.StepOfRealtimeGetReocrds.WaitGetRecord;
					this.bkRealtimeGetRecords.RunWorkerAsync();
				}
				else if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.WaitGetRecord)
				{
					if (this.bStopComm)
					{
						this.dealtDoorIndex = 0;
						this.stepOfRealtimeGetRecords = frmConsole.StepOfRealtimeGetReocrds.DelSwipe;
						this.bkRealtimeGetRecords.RunWorkerAsync();
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}", this.doorsNeedToGetRecords[this.dealtIndexOfDoorsNeedToGetRecords + 1].ToString()),
							information = string.Format("{0}", CommonStr.strAlreadyGotSwipeRecord)
						});
						this.displayNewestLog();
						this.dealtIndexOfDoorsNeedToGetRecords++;
						this.bkRealtimeGetRecords.RunWorkerAsync();
					}
				}
				else if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.DelSwipe)
				{
					this.stepOfRealtimeGetRecords = frmConsole.StepOfRealtimeGetReocrds.Stop;
					this.btnRealtimeGetRecords.BackColor = Color.Transparent;
					this.btnRealtimeGetRecords.Text = CommonStr.strRealtimeGetRecords;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0008B644 File Offset: 0x0008A644
		private void bkUploadAndGetRecords_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			if (this.CommOperate == "UPLOAD")
			{
				e.Result = this.uploadPrivilegeNow(this.CommOperateOption);
			}
			else if (this.CommOperate == "GETRECORDS")
			{
				e.Result = this.getRecordsNow();
			}
			else if (this.CommOperate == "GETPRIVILEGES_RENTING_HOUSE")
			{
				e.Result = this.getPrivilegesOfRentingHouseNow();
			}
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0008B6F0 File Offset: 0x0008A6F0
		private void bkUploadAndGetRecords_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				XMessageBox.Show(CommonStr.strOperationCanceled);
				wgAppConfig.wgLog(CommonStr.strOperationCanceled);
				return;
			}
			if (e.Error != null)
			{
				XMessageBox.Show(string.Format("An error occurred: {0}", e.Error.Message));
				return;
			}
			if (this.CommOperate == "UPLOAD")
			{
				this.uploadPrivilegeToController(int.Parse(e.Result.ToString()));
				if (this.dealtDoorIndex < this.arrSelectedDoors.Count)
				{
					this.bkUploadAndGetRecords.RunWorkerAsync();
					return;
				}
			}
			else if (this.CommOperate == "GETRECORDS")
			{
				this.getRecordsFromController(int.Parse(e.Result.ToString()));
				if (this.dealtDoorIndex < this.arrSelectedDoors.Count)
				{
					this.bkUploadAndGetRecords.RunWorkerAsync();
					return;
				}
			}
			else if (this.CommOperate == "GETPRIVILEGES_RENTING_HOUSE")
			{
				this.getPrivilegesOfRentingHouseFromController(int.Parse(e.Result.ToString()));
				if (this.dealtDoorIndex < this.arrSelectedDoors.Count)
				{
					this.bkUploadAndGetRecords.RunWorkerAsync();
				}
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0008B818 File Offset: 0x0008A818
		private int uploadPrivilegeNow(int Option)
		{
			int num = -1;
			long commTimeoutMsMin = wgUdpComm.CommTimeoutMsMin;
			try
			{
				if (this.control4uploadPrivilege == null)
				{
					this.control4uploadPrivilege = new icController();
				}
				if (this.pr4uploadPrivilege == null)
				{
					this.pr4uploadPrivilege = new icPrivilege();
				}
				this.controlConfigureUnused.RestoreDefault();
				this.controlConfigure4uploadPrivilege.Clear();
				this.controlTaskList4uploadPrivilege.Clear();
				this.controlHolidayList4uploadPrivilege.Clear();
				this.pr4uploadPrivilege.AllowUpload();
				string text = this.arrSelectedDoors[this.dealtDoorIndex].ToString();
				this.control4uploadPrivilege.GetInfoFromDBByDoorNameSpecial(text, this.dsWatchingDoorInfo);
				this.control4uploadPrivilege.runinfo.Clear();
				string text2 = "";
				string text3 = "";
				string text4 = null;
				int num2 = 3;
				int num3 = 300;
				int num4 = 0;
				while (num4 < num2 && this.control4uploadPrivilege.GetControllerRunInformationIP(-1) <= 0)
				{
					Thread.Sleep(num3);
					num4++;
				}
				if (this.control4uploadPrivilege.runinfo.wgcticks > 0U)
				{
					if (this.control4uploadPrivilege.runinfo.netSpeedCode != 0)
					{
						wgUdpComm.CommTimeoutMsMin = 2000L;
					}
					if (wgAppConfig.IsAccessControlBlue && !wgAppConfig.checkRSAController((long)this.control4uploadPrivilege.ControllerSN) && wgTools.doubleParse(this.control4uploadPrivilege.runinfo.driverVersion.Substring(1)) == 6.57)
					{
						bool flag = true;
						string systemParamByNO = wgAppConfig.getSystemParamByNO(49);
						DateTime now = DateTime.Now;
						if (!string.IsNullOrEmpty(systemParamByNO))
						{
							DateTime.TryParse(systemParamByNO, out now);
							if (now < DateTime.Parse("2018-01-01 00:00:00"))
							{
								flag = false;
							}
						}
						if (!flag)
						{
							string keyVal = wgAppConfig.GetKeyVal("KEY_F_INVALIDCON");
							if (!string.IsNullOrEmpty(keyVal) && keyVal.IndexOf(this.control4uploadPrivilege.ControllerSN.ToString() + ",") >= 0)
							{
								flag = true;
							}
						}
						if ((this.control4uploadPrivilege.runinfo.swipeEndIndex < 2000U && this.control4uploadPrivilege.runinfo.swipeStartIndex < 1000U) || flag)
						{
							InfoRow infoRow = new InfoRow();
							infoRow.desc = string.Format("!!!6.56[{0}] {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strNeedUpgradeDriver);
							infoRow.information = string.Format("!!!6.56--{0}: {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strSupposeUpgradeDriver);
							infoRow.detail = infoRow.information;
							infoRow.category = 5;
							wgRunInfoLog.addEvent(infoRow);
							int num5 = 0;
							int.TryParse(wgAppConfig.GetKeyVal("KEY_F_UPLOADPRIVILEGE"), out num5);
							num5++;
							wgAppConfig.UpdateKeyVal("KEY_F_UPLOADPRIVILEGE", num5.ToString());
							if (num5 < 0 || num5 > 100)
							{
								return num;
							}
							string keyVal2 = wgAppConfig.GetKeyVal("KEY_F_INVALIDCON");
							if (!string.IsNullOrEmpty(keyVal2))
							{
								if (keyVal2.IndexOf(this.control4uploadPrivilege.ControllerSN.ToString() + ",") < 0)
								{
									wgAppConfig.UpdateKeyVal("KEY_F_INVALIDCON", keyVal2 + this.control4uploadPrivilege.ControllerSN.ToString() + ",");
								}
							}
							else
							{
								wgAppConfig.UpdateKeyVal("KEY_F_INVALIDCON", this.control4uploadPrivilege.ControllerSN.ToString() + ",");
							}
						}
					}
					if (!wgAppConfig.checkRSAController((long)this.control4uploadPrivilege.ControllerSN) && wgTools.doubleParse(this.control4uploadPrivilege.runinfo.driverVersion.Substring(1)) == 6.62)
					{
						byte[] array = new byte[2048];
						byte[] array2 = new byte[2048];
						int num6 = 196608;
						if (this.control4uploadPrivilege.GetControlDataNoPasswordIP(num6, 1427899392, ref array) > 0 && this.control4uploadPrivilege.GetControlDataNoPasswordIP(num6 + 1024, 1427899392, ref array2) > 0 && !wgTools.IsAllFF(array2, 28, 256))
						{
							string keyVal3 = wgAppConfig.GetKeyVal("KEY_F_INVALIDCON");
							if (!string.IsNullOrEmpty(keyVal3))
							{
								if (keyVal3.IndexOf(this.control4uploadPrivilege.ControllerSN.ToString() + ",") < 0)
								{
									wgAppConfig.UpdateKeyVal("KEY_F_INVALIDCON", keyVal3 + this.control4uploadPrivilege.ControllerSN.ToString() + ",");
								}
							}
							else
							{
								wgAppConfig.UpdateKeyVal("KEY_F_INVALIDCON", this.control4uploadPrivilege.ControllerSN.ToString() + ",");
							}
							InfoRow infoRow2 = new InfoRow();
							infoRow2.desc = string.Format("[{0}] {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strNeedUpgradeDriver);
							infoRow2.information = string.Format("{0}: {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strSupposeUpgradeDriver);
							infoRow2.detail = infoRow2.information;
							infoRow2.category = 5;
							wgRunInfoLog.addEvent(infoRow2);
							return num;
						}
					}
					if (wgTools.doubleParse(this.control4uploadPrivilege.runinfo.driverVersion.Substring(1)) >= 6.64)
					{
						byte[] array3 = new byte[2048];
						byte[] array4 = new byte[2048];
						int num7 = 196608;
						if (this.control4uploadPrivilege.GetControlDataNoPasswordIP(num7, 1427899392, ref array3) > 0 && this.control4uploadPrivilege.GetControlDataNoPasswordIP(num7 + 1024, 1427899392, ref array4) > 0)
						{
							try
							{
								byte[] array5 = new byte[1024];
								byte[] array6 = new byte[1024];
								Array.Copy(array3, 28, array5, 0, 1024);
								Array.Copy(array4, 28, array6, 0, 1024);
								byte[] array7 = new byte[16];
								Array.Copy(array5, 896, array7, 0, 16);
								byte[] array8 = new byte[256];
								Array.Copy(array6, 0, array8, 0, 256);
								RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
								rsacryptoServiceProvider.FromXmlString(Program.Dpt4Database("luSWeZHP2nqo3pdFCebDHsTQfsnD5oiAjsldv6Hn0+fk3soS8cQxkG25zIp3AI2So3IPxyOTe07QsB11kDSK4qpl7LpvApGN5plkBWQ449mnqTqGFHiPoEJq0SQmKIkCbwd9A1F724+9Vm3rCi+icvwRHHMf/+cmU63XyNFNFf8XebgH8Q2IbeQEQOVf5TIG6S5H07M98rQk6ww/wyf42TuIldTanZN4/ygz04yCJ/cDAoZ1/khyKgaQ4E8GTTJiInCTSvvlWN57uR/KdXBKRgVcJ82Gsswa0ObHulF/em1NRQPvSdO/xZYVnfGItyBoluecetfvzeRcwvrhBY4BUgvomBaiCJ4gW68gwJhbMQA6rCEbOTC0nF9ipfxmm9sVmil9X2/1AaK3jd0Fl6VCkKcUEhvqL6qaNepMZtv075WCsvHwDnvniQAlRl0Wsb4+6FT7LZ4PL8EHPBhPYZVPuwAWT/fj31GFsyDiign9lWrsALJIDEwTfKFTSO5IO9jUsUNZGvoaUbSlCVRt9OsISOX9xBUjrrjqO4Hia7FJseI="));
								bool flag2 = rsacryptoServiceProvider.VerifyData(array7, Program.Dpt4Database("7TbkKQVZ4Al3BOP7opHTFQ=="), array8);
								if (flag2)
								{
									long num8 = (long)((ulong)array7[3]);
									num8 = (num8 << 8) + (long)((ulong)array7[2]);
									num8 = (num8 << 8) + (long)((ulong)array7[1]);
									num8 = (num8 << 8) + (long)((ulong)array7[0]);
									if (num8 != (long)this.control4uploadPrivilege.ControllerSN)
									{
										flag2 = false;
									}
									else
									{
										byte[] array9 = new byte[32];
										Array.Copy(array5, 896, array9, 0, 32);
										RSACryptoServiceProvider rsacryptoServiceProvider2 = new RSACryptoServiceProvider();
										byte[] array10 = new byte[256];
										Array.Copy(array6, 256, array10, 0, 256);
										rsacryptoServiceProvider2.FromXmlString("<RSAKeyValue><Modulus>tpNxzb5MyuA0f7v0p6rpcfCoUiO2DEJ+OZWfkZejQkGGw6/tf+qpSADmSLUtvRxgzWL+kQ18zi05WIDF+N8bAvFuLy5ocmgd4344wTEtXPzsS9M9QMRDnECOaxn73PYc8iFrVtnyPbjlWyCxJAffGG+jdZUwtY2P8w8SlMmysadPcXkPQwn9xf+PBl5AwlUhnsXSt+cvuLA82eEAeHZ5a60Tgdxde5vo7GV/08N8+1KoDlUQHbQ2Y/z+UOL6SHu42mgUK00xGyPcfkX5Z/m0yV4w6hx0ynpWXBSV1ly2tPop/0PRKr18XriQDHAnO9VCYL9Xb23IF9FIrwN1TTmjlw==</Modulus><Exponent>AQAB</Exponent><P>78W3yotRXiJHb1tK+ag+G7KQp88Ez12nZXsakUVWENCULqGOW18RTX4JyVlgI2ikakTY58VSjCI/G3vwJPC/MaiCd0OUpLcw+WlsAFAPNCHnCaA37uyLSNvDErhzI7QNU7U8cAWdt4pqIBZnfosHo4pkOz6CRrccxbY8CLe88aU=</P><Q>wu6+za2xwrz0Sp5KZlegn867VzScufnap4u9WC1jUqPXvxtZZ1gEu5iGtohfAewnGXVdB4NHSSv+ZcTt3dz9KQL1irzD+2nfn76EXJpJ1sJSBzw8kOeNICSDpj1VU+nu21F+joTRxvNUPQSFdQWPau/61PplhxAC8LzYxosfg4s=</Q><DP>EBNnkTLqD6+ornSmgDqhf2XngPjGT8jtskYgr+DANnxlkwLN8p5bIKD0swVFBtq7O0zGZjO6px6csJEMts1VZFTx+mETlD+cDvzceuDjRBGJ1gFh/qjpZccmOxkWzywBQL2EAda+jlG/b8tQwRE9P9/FfJtiTK92RGNFfygiiOk=</DP><DQ>d8IDeQsYJaK+lsUuwjddmDCDCe1pyBwHSF3igrTHy3KbTm53+7Som7P2N1gKvnyd/NcMw1vUxNCBZ3aYCHEHG1YVEhoscV43I8YBPX8QlB8oFAfe0ctf+XpVYyFVNcuHkjg7/0T8ODUv8JjBgQZceP9cY9I9479jfxEWQMwiHUc=</DQ><InverseQ>ST6+NibvUp2qp8fkoKPp0wB+lQ1wjTwFcRoed4wKGyRlkmlqhbUV4GlyIUSKgR0Y5q7Vf3s5NbRBoXq750lPGVb8PLULeuoSZQDEhHWsvfqzQyXjmKQTOAumCrxZmS2PhIPbAjFtHkUIiZt1xpchm7JlYnAJtBoyericL9lz2sE=</InverseQ><D>X35DGd+c/sfB7cV7M287ZrL+9udiworMxoWxzk7Hv1HSkOulhzBPli+SQxkwcSc0onhSub+aNaSFx/qikYbv89wfS2I0kxJN/7z3JmT4WxiW5xn3gbalvkYtt2sBxxKbFTHRmL2p7AqXO9Cflj7fu0I49rkvnuYKEpEhoNzWMAB1OI7fgEkSpUhsWKGAnXWb93LO/g0n9tBXh0PL3N6HbKmQfbGKeZuj9Y3QyN7okVtB7xXQ8GU5vkxHt2dBodwSAO7kMb91TLdikvd4QGUWN5nxZ/MfNvcFVO0ZyAUB8p6o8/FsaRgh39g9aQC8t6ZzMt3MoYH16NrXp60s/2K5cQ==</D></RSAKeyValue>");
										byte[] array11 = rsacryptoServiceProvider2.Decrypt(array10, true);
										if (array11.Length != array9.Length)
										{
											flag2 = false;
										}
										else
										{
											for (int i = 0; i < array11.Length; i++)
											{
												if (array11[i] != array9[i])
												{
													flag2 = false;
													break;
												}
											}
										}
										rsacryptoServiceProvider2.Clear();
									}
								}
								rsacryptoServiceProvider.Clear();
								if (!flag2)
								{
									RSACryptoServiceProvider rsacryptoServiceProvider3 = new RSACryptoServiceProvider();
									rsacryptoServiceProvider3.FromXmlString(Program.Dpt4Database("luSWeZHP2nqo3pdFCebDHn9E02cWiFP6UoW408FEGsKBr7prhHDtqEA7REbJZZCXghz5mchVOMJXWKpJnBohasJyXaGXuMy++WClCd/QGAy0odug3tDt6Qb1A5ZxLbRzsXnp6Mey0c4TrTZA2P6uPw1ZQt30CMF6PBPFcgCmBqas0ULjRgnRTGoJCQd5wZrk2tFhNssS7HXFbg2GDdUaGs38HE2dFWf5+9cmqVJdEVnO4YZQmCCFDxsL4m0oECVfpHRJDRTUJNTG4QrlreBOkQ+DojmkJJKlKXTV0BhmaxoUS1uBJNivkdAer4vTCTet6zLDN5+peRdngMDOgAPa10NPLr2lxrxVRjMjF9GxCW4bzXlAmbTqpgaZollzzD89gBf7vssE9CluavpybjKkBkKSr3dviGW+YqQhtTYyPi6Bi7FQhIIpxh1xiZ6tY6NRCoeX7dawowwfzCDDOREM9Hl4ZwSr7ZVzN7CKAbsIwMIIpoUXND4DGibauQj8FHJv5052exne7G3/r7FV2KsIvDjrBXQ9VnBPB05YyozMDwg="));
									flag2 = rsacryptoServiceProvider3.VerifyData(array7, Program.Dpt4Database("7TbkKQVZ4Al3BOP7opHTFQ=="), array8);
									if (flag2)
									{
										long num9 = (long)((ulong)array7[3]);
										num9 = (num9 << 8) + (long)((ulong)array7[2]);
										num9 = (num9 << 8) + (long)((ulong)array7[1]);
										num9 = (num9 << 8) + (long)((ulong)array7[0]);
										if (num9 != (long)this.control4uploadPrivilege.ControllerSN)
										{
											flag2 = false;
										}
										else
										{
											byte[] array12 = new byte[32];
											Array.Copy(array5, 896, array12, 0, 32);
											new RSACryptoServiceProvider();
											byte[] array13 = new byte[256];
											Array.Copy(array6, 256, array13, 0, 256);
											RSACryptoServiceProvider rsacryptoServiceProvider4 = new RSACryptoServiceProvider();
											rsacryptoServiceProvider4.FromXmlString("<RSAKeyValue><Modulus>8nIiVLAuDM2YwZJJO0D6dQl0KDlaV69Ul7qMx/AMn14MKTtd7IzFR2k5M2hVKnpX41fvcdMFPsmKaXZ9l70StXVy54Vuhp+9+HZK9RUiuCqfuBBRv4dRp0sINW8ah7N7UmfUfuhIwc0Opia/tCDpzanwAGeIiPJeb6HKwAaNMzO2Hjhv78nrSv7J6sbGJvUT1nE8t7Y15HZdouijQlt+hOJF5mcofU7N7gmPPFdWQGggJ7CFsUFWS3oz+9jSWGmbMGCRBywfYiar4jqclOEXTEQy+jvLp6BcBIpJT18D41oD+hQpj0309jhrHceU2+El/OIyQlTApXctOnj8W5Zfew==</Modulus><Exponent>AQAB</Exponent><P>+0Bj6Q5KTLdKvierrYLm0xf8tV1f3ebRNfkTv0DruanAXKAsTN5MF2Bej6Hy8Jo9V9CGCAZe2YjQ/HydN+MQLDqDE2SegOibhUdxA6Ej/mubIcq/AyXqKHAg7hUvuvJm192Pz8G/HlY1dMuIKr/mmaWiCPeomFIJPiKtf814vVU=</P><Q>9wcj1kPKfl3DdEDkGXwrwhVE4TBIHHLiw5F4NlSfz2VVk3RJ8vWxVwa/Q6ZQ/BzUDZo8Ir9XzNhJ5rZqAOrXrjCa06mOxE4Udx+zyqbK9qxXqWbOqrXc55B8rVlJZV8PUpqRuhvXV70XscnwmilJaQwY6wHoNOdQyq00WXOKKY8=</Q><DP>yCDv9DFnyVeud2zN+LVt+vL+XdB0jiJOvrkZm5uwqACosp766nefEHe4mpwbEL8q4Ym9WSMJ5ihuns77xApfVXt98LKS9odRaYYQZR2zt/IqbW47W9yVeYRnFn1VuYlflc5rwMbI3vc4OUHtwm981SHzwUTTzBB8QDwLCuO1qsk=</DP><DQ>v6B7TWCDOanU0F7yU06+xy0mpKWGmwMII/u5zDOzUPhj/SHtp5quejsSrhDbq+dVHEQ0OCREoPR7x4xkCNgkfczYni9fP6ruN0aRezgOJHKnAhNtaIHxnnS4MuUbiZbHVSXwu/WTBE38jn9/tKcoemPXBJ8TGc8DHmZkQKvfVsM=</DQ><InverseQ>6UEND7RayrdCfxoC1v37/89VS9NJeLWDVBXZlam/If2ZIQthTo8oNB7vTN40UwQcFhe4iSi+Se3ilYL3RCCJf6eh++gNyKDoNsJXkeFZ/9CYlXzpUsVl5xVFxuY+oZs73mioaWAEU+dsnfRLejDwabWcmrL6omsoK7UUESWuIBQ=</InverseQ><D>VlUQJooKC+MWVy+pRiFQTmbMZxptMjYr8E+sm/G9/O33dKNAQeCqy1AL32NymEqyizEgAa87+ey4je90r5jTVax5+zmHbwbpFdXBwV4CXARJlbix83rTN7g/Nw/WKaZe7qwsffhnGCPV6JJ5x0HFH8g4v1wewW1U9XlugkyS8TzyWLvYWIBL9Qk0WlkNKP29Tq+6AtXInBO2ovo1mgj/hi/6C2U25DwmwRumdTaIaTBqFKbn12IuXE4OrUP94dVseJHRhunxdHKx8CmGqd27Elg+lbZyw8kgCXG5RRF3pNa2+FAWT6lg1Jskb0lpQSmUwMk7o5urTb9e3VnuVSNYWQ==</D></RSAKeyValue>");
											byte[] array14 = rsacryptoServiceProvider4.Decrypt(array13, true);
											if (array14.Length != array12.Length)
											{
												flag2 = false;
											}
											else
											{
												for (int j = 0; j < array14.Length; j++)
												{
													if (array14[j] != array12[j])
													{
														flag2 = false;
														break;
													}
												}
											}
											rsacryptoServiceProvider4.Clear();
										}
									}
									rsacryptoServiceProvider3.Clear();
								}
								if (!flag2)
								{
									InfoRow infoRow3 = new InfoRow();
									infoRow3.desc = string.Format("[{0}] {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strNeedUpgradeDriver);
									infoRow3.information = string.Format("{0}: {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strSupposeUpgradeDriver);
									infoRow3.detail = infoRow3.information;
									infoRow3.category = 5;
									wgRunInfoLog.addEvent(infoRow3);
									return num;
								}
							}
							catch (Exception ex)
							{
								wgAppConfig.wgLog("oRSA4 Error :  " + ex.ToString());
								if (ex.ToString().IndexOf("System.Security.Cryptography.RSA.FromXmlString(String xmlString)") < 0)
								{
									InfoRow infoRow4 = new InfoRow();
									infoRow4.desc = string.Format("[{0}] {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strNeedUpgradeDriver);
									infoRow4.information = string.Format("{0}: {1}", this.control4uploadPrivilege.ControllerSN.ToString(), CommonStr.strSupposeUpgradeDriver);
									infoRow4.detail = infoRow4.information;
									infoRow4.category = 5;
									wgRunInfoLog.addEvent(infoRow4);
									return num;
								}
							}
						}
					}
					if (wgAppConfig.IsActivateCard19 && wgTools.doubleParse(this.control4uploadPrivilege.runinfo.driverVersion.Substring(1)) <= 7.68)
					{
						wgAppConfig.wgLog(string.Format("{0}[Driver Version ={1}]: {2}", this.control4uploadPrivilege.ControllerSN.ToString(), this.control4uploadPrivilege.runinfo.driverVersion, CommonStr.str19CardNeed770));
					}
				}
				for (int k = 0; k < num2; k++)
				{
					text4 = this.control4uploadPrivilege.GetProductInfoIP(ref text2, ref text3, -1);
					if (!string.IsNullOrEmpty(text4))
					{
						break;
					}
					Thread.Sleep(num3);
				}
				if (string.IsNullOrEmpty(text4))
				{
					wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.GetProductInfoIP Failed num =" + num.ToString(), new object[0]);
					return -13;
				}
				if (string.IsNullOrEmpty(this.strAllProductsDriversInfo))
				{
					string text5;
					wgAppConfig.getSystemParamValue(48, out text5, out text5, out this.strAllProductsDriversInfo);
				}
				if (!string.IsNullOrEmpty(this.strAllProductsDriversInfo))
				{
					if (this.strAllProductsDriversInfo.IndexOf(text2) < 0)
					{
						if (this.strAllProductsDriversInfo.IndexOf("SN") < 0)
						{
							this.strAllProductsDriversInfo += "\r\n";
						}
						this.strAllProductsDriversInfo += text2;
						wgAppConfig.setSystemParamValue(48, "ConInfo", "", this.strAllProductsDriversInfo);
					}
				}
				else
				{
					this.strAllProductsDriversInfo = text2;
					wgAppConfig.setSystemParamValue(48, "ConInfo", "", this.strAllProductsDriversInfo);
				}
				this.checkInvalidCon();
				if (!this.arrControllerTryAdjustTime.Contains(this.control4uploadPrivilege.ControllerSN) && (this.control4uploadPrivilege.runinfo.appError & 4) > 0)
				{
					if (this.control4uploadPrivilege.AdjustTimeIP(DateTime.Now) > 0)
					{
						wgAppConfig.wgLog(string.Format("{0}_{1}. SN = {2}", CommonStr.strUpload, CommonStr.strAdjustTimeOK, this.control4uploadPrivilege.ControllerSN.ToString()));
					}
					this.arrControllerTryAdjustTime.Add(this.control4uploadPrivilege.ControllerSN);
				}
				if (!this.arrControllerTryRestoreDefaultConfigure.Contains(this.control4uploadPrivilege.ControllerSN) && wgTools.doubleParse(this.control4uploadPrivilege.runinfo.driverVersion.Substring(1)) < 5.52 && (this.control4uploadPrivilege.runinfo.appError & 2) <= 0 && (this.control4uploadPrivilege.runinfo.appError & 1) > 0)
				{
					this.control4uploadPrivilege.RestoreDefaultConfigureIP();
					wgAppConfig.wgLog(string.Format("{0}_{1}. SN = {2}", CommonStr.strUpload, CommonStr.strRestoreDefaultParam, this.control4uploadPrivilege.ControllerSN.ToString()));
					this.arrControllerTryRestoreDefaultConfigure.Add(this.control4uploadPrivilege.ControllerSN);
					Thread.Sleep(5000);
				}
				if (wgMjController.IsFingerController(this.control4uploadPrivilege.ControllerSN))
				{
					byte[] array15 = new byte[4096];
					byte[] array16 = new byte[524288];
					int num10 = -1;
					for (int l = 0; l < array15.Length; l++)
					{
						array15[l] = byte.MaxValue;
					}
					for (int m = 0; m < array16.Length; m++)
					{
						array16[m] = byte.MaxValue;
					}
					string text6 = "";
					string text7 = "";
					int num11 = 0;
					try
					{
						using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
						{
							using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text6, (OleDbConnection)dbConnection) : new SqlCommand(text6, (SqlConnection)dbConnection)))
							{
								dbConnection.Open();
								text6 = "SELECT t_b_Consumer_Fingerprint.*, t_b_Consumer.f_CardNO  From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer.f_CardNO>0 AND t_b_Consumer_Fingerprint.f_ConsumerID = t_b_Consumer.f_ConsumerID  ORDER BY f_FingerNO ASC";
								dbCommand.CommandText = text6;
								DbDataReader dbDataReader = dbCommand.ExecuteReader();
								while (dbDataReader.Read())
								{
									num11 = (int)dbDataReader["f_FingerNO"];
									if (num11 <= 0 || num11 >= 1024)
									{
										break;
									}
									Array.Copy(BitConverter.GetBytes(long.Parse(dbDataReader["f_CardNO"].ToString())), 0, array15, (num11 - 1) * 4, 4);
									text7 = dbDataReader["f_FingerInfo"] as string;
									if (!string.IsNullOrEmpty(text7))
									{
										text7 = text7.Replace("\r\n", "");
										for (int n = 0; n < text7.Length; n += 2)
										{
											try
											{
												array16[(num11 - 1) * 512 + n / 2] = byte.Parse(text7.Substring(n, 2), NumberStyles.AllowHexSpecifier);
											}
											catch (Exception ex2)
											{
												wgAppConfig.wgLog(string.Concat(new object[]
												{
													ex2.ToString(),
													"\r\nfingerNO=",
													num11,
													",strFinger=",
													text7
												}));
											}
										}
									}
									if (num10 < num11)
									{
										num10 = num11;
									}
								}
								dbDataReader.Close();
							}
						}
						num = this.control4uploadPrivilege.UpdateFingerprintListIP(num10, array15, array16, text, -1);
						if (num <= 0)
						{
							wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " UpdateFingerprintListIP Failed num =" + num.ToString(), new object[0]);
							return -13;
						}
					}
					catch (Exception ex3)
					{
						wgAppConfig.wgLog(string.Concat(new object[]
						{
							ex3.ToString(),
							"\r\nfingerNO=",
							num11,
							",strFinger=",
							text7
						}));
					}
					return num;
				}
				if ((Option & 1) > 0)
				{
					icControllerConfigureFromDB.getControllerConfigureFromDBByControllerID(this.control4uploadPrivilege.ControllerID, ref this.controlConfigure4uploadPrivilege, ref this.controlTaskList4uploadPrivilege, ref this.controlHolidayList4uploadPrivilege);
					num = this.control4uploadPrivilege.UpdateConfigureIP(this.controlConfigure4uploadPrivilege, -1);
					if (num <= 0)
					{
						wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " updateConfigureIP Failed num =" + num.ToString(), new object[0]);
						return -13;
					}
					if (this.controlConfigure4uploadPrivilege.controlTaskList_enabled > 0 && (num = this.control4uploadPrivilege.UpdateControlTaskListIP(this.controlTaskList4uploadPrivilege, -1)) <= 0)
					{
						wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " updateControlTaskListIP Failed num =" + num.ToString(), new object[0]);
						return -13;
					}
					if (wgAppConfig.getParamValBoolByNO(121))
					{
						icControllerTimeSegList icControllerTimeSegList = new icControllerTimeSegList();
						if (wgAppConfig.getParamValBoolByNO(121))
						{
							icControllerTimeSegList.fillByDB();
						}
						num = this.control4uploadPrivilege.UpdateControlTimeSegListIP(icControllerTimeSegList, -1);
						if (num <= 0)
						{
							wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " updateControlTimeSegListIP Failed num =" + num.ToString(), new object[0]);
							return -13;
						}
						num = this.control4uploadPrivilege.UpdateHolidayListIP(this.controlHolidayList4uploadPrivilege.ToByte(), -1);
						if (num <= 0)
						{
							wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " UpdateHolidayListIP Failed num =" + num.ToString(), new object[0]);
							return -13;
						}
					}
				}
				if ((Option & 2) > 0)
				{
					int controllerIDByDoorName = this.pr4uploadPrivilege.getControllerIDByDoorName(text);
					if (controllerIDByDoorName > 0)
					{
						num = this.pr4uploadPrivilege.getPrivilegeByID(controllerIDByDoorName);
						if (num < 0)
						{
							wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.getPrivilegeByID Failed num =" + num.ToString(), new object[0]);
							return num;
						}
						if (this.pr4uploadPrivilege.PrivilegTotal > this.control4uploadPrivilege.ControllerSN % 100 && wgTools.bFindFalseACont)
						{
							if (this.control4uploadPrivilege.UpdateConfigureUnvalidIP(this.controlConfigureUnused, -1) == 1)
							{
								wgAppConfig.wgLog(".UpdateConfigureIP_E1_" + this.control4uploadPrivilege.ControllerSN.ToString());
							}
							else
							{
								wgTools.bFindFalseACont = false;
							}
						}
						num = this.pr4uploadPrivilege.upload(this.control4uploadPrivilege.ControllerSN, this.control4uploadPrivilege.IP, this.control4uploadPrivilege.PORT, text, -1);
						if (num < 0)
						{
							wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.upload Failed num =" + num.ToString(), new object[0]);
							return num;
						}
						string text8 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal =0,  f_lastConsoleUploadDateTime ={0}, f_lastConsoleUploadConsuemrsTotal ={1:d}, f_lastConsoleUploadPrivilege ={2:d}, f_lastConsoleUploadValidPrivilege ={3:d} WHERE f_ControllerID ={4:d}";
						wgAppConfig.runUpdateSql(string.Format(text8, new object[]
						{
							wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")),
							this.pr4uploadPrivilege.ConsumersTotal,
							this.pr4uploadPrivilege.PrivilegTotal,
							this.pr4uploadPrivilege.ValidPrivilege,
							controllerIDByDoorName
						}));
					}
				}
				if ((Option & 1) > 0 && this.controlTaskList4uploadPrivilege.taskCount > 0)
				{
					num = this.control4uploadPrivilege.RenewControlTaskListIP(-1);
					if (num < 0)
					{
						wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.renewControlTaskListIP Failed num =" + num.ToString(), new object[0]);
					}
				}
			}
			catch (Exception ex4)
			{
				num = -1;
				wgAppConfig.wgLog(ex4.ToString());
			}
			finally
			{
				wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
				if (wgTools.bUDPCloud > 0)
				{
					this.control4uploadPrivilege.Dispose();
					this.control4uploadPrivilege = null;
					this.pr4uploadPrivilege.Dispose();
					this.pr4uploadPrivilege = null;
				}
			}
			return num;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0008CC00 File Offset: 0x0008BC00
		private void uploadPrivilegeToController(int result)
		{
			if (this.control4uploadPrivilege == null)
			{
				this.control4uploadPrivilege = new icController();
			}
			this.control4uploadPrivilege.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
			this.arrDealtController.Add(this.control4uploadPrivilege.ControllerSN, result);
			int i;
			for (i = this.dealtDoorIndex; i < this.arrSelectedDoors.Count; i++)
			{
				this.control4uploadPrivilege.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[i].ToString(), this.dsWatchingDoorInfo);
				if (!this.arrDealtController.ContainsKey(this.control4uploadPrivilege.ControllerSN))
				{
					break;
				}
				if (this.arrDealtController[this.control4uploadPrivilege.ControllerSN] >= 0)
				{
					InfoRow infoRow = new InfoRow();
					infoRow.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[i].ToString(), this.control4uploadPrivilege.ControllerSN);
					if (i == this.dealtDoorIndex)
					{
						if ((this.CommOperateOption & 3) == 3)
						{
							infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadAllOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]);
							wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[i].ToString(), CommonStr.strUploadAllOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]));
						}
						else if ((this.CommOperateOption & 1) > 0)
						{
							infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadBasicConfigureOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]);
							wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[i].ToString(), CommonStr.strUploadBasicConfigureOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]));
						}
						else if ((this.CommOperateOption & 2) > 0)
						{
							infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadPrivilegesOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]);
							wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[i].ToString(), CommonStr.strUploadPrivilegesOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]));
						}
					}
					else
					{
						infoRow.information = string.Format("{0}", CommonStr.strAlreadyUploadPrivileges);
					}
					wgRunInfoLog.addEvent(infoRow);
				}
				else
				{
					foreach (object obj in this.lstDoors.Items)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if (listViewItem.Text == this.arrSelectedDoors[i].ToString())
						{
							if (this.arrDealtController[this.control4uploadPrivilege.ControllerSN] == wgGlobal.ERR_PRIVILEGES_OVER200K)
							{
								InfoRow infoRow2 = new InfoRow();
								infoRow2.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[i].ToString(), this.control4uploadPrivilege.ControllerSN);
								infoRow2.information = string.Format("{0}--[{1:d}]", wgTools.gADCT ? CommonStr.strUploadFail_200K : (wgTools.gWGYTJ ? CommonStr.strUploadFail_500 : CommonStr.strUploadFail_40K), listViewItem.Text);
								wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", this.arrSelectedDoors[i].ToString(), wgTools.gADCT ? CommonStr.strUploadFail_200K : (wgTools.gWGYTJ ? CommonStr.strUploadFail_500 : CommonStr.strUploadFail_40K)));
								wgRunInfoLog.addEvent(infoRow2);
								break;
							}
							wgRunInfoLog.addEventNotConnect(this.control4uploadPrivilege.ControllerSN, this.control4uploadPrivilege.IP, listViewItem);
							wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", listViewItem.Text, CommonStr.strCommFail));
							break;
						}
					}
				}
			}
			if (i < this.arrSelectedDoors.Count)
			{
				this.dealtDoorIndex = i;
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.control4uploadPrivilege.ControllerSN),
					information = string.Format("{0}", CommonStr.strUploadStart)
				});
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), CommonStr.strUploadingPrivileges));
			}
			else
			{
				this.dealtDoorIndex = i;
				this.btnRealtimeGetRecords.Enabled = true;
				if (this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting && this.btnServer.Text != CommonStr.strMonitoring)
				{
					this.btnStopOthers.BackColor = Color.Transparent;
					this.btnStopMonitor.BackColor = Color.Transparent;
				}
				this.btnGetRecords.Enabled = true;
				this.mnuGetRecordsToolStripMenuItem.Enabled = true;
				this.btnUpload.Enabled = true;
				this.mnuUploadToolStripMenuItem.Enabled = true;
			}
			if (wgTools.bUDPCloud > 0 && this.control4uploadPrivilege != null)
			{
				this.control4uploadPrivilege.Dispose();
				this.control4uploadPrivilege = null;
			}
			this.displayNewestLog();
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0008D1AC File Offset: 0x0008C1AC
		private int getRecordsNow()
		{
			this.swipe4GetRecords.Clear();
			return this.swipe4GetRecords.GetSwipeRecordsByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0008D1DC File Offset: 0x0008C1DC
		private void getRecordsFromController(int result)
		{
			this.control4getRecordsFromController = new icController();
			this.control4getRecordsFromController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
			this.arrDealtController.Add(this.control4getRecordsFromController.ControllerSN, result);
			int i;
			for (i = this.dealtDoorIndex; i < this.arrSelectedDoors.Count; i++)
			{
				this.control4getRecordsFromController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[i].ToString(), this.dsWatchingDoorInfo);
				if (!this.arrDealtController.ContainsKey(this.control4getRecordsFromController.ControllerSN))
				{
					break;
				}
				if (this.arrDealtController[this.control4getRecordsFromController.ControllerSN] >= 0)
				{
					InfoRow infoRow = new InfoRow();
					infoRow.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[i].ToString(), this.control4getRecordsFromController.ControllerSN);
					if (i == this.dealtDoorIndex)
					{
						infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strGetSwipeRecordOK, this.arrDealtController[this.control4getRecordsFromController.ControllerSN]);
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[i].ToString(), CommonStr.strGetSwipeRecordOK, this.arrDealtController[this.control4getRecordsFromController.ControllerSN]));
					}
					else
					{
						infoRow.information = string.Format("{0}", CommonStr.strAlreadyGotSwipeRecord);
					}
					wgRunInfoLog.addEvent(infoRow);
				}
				else
				{
					foreach (object obj in this.lstDoors.Items)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if (listViewItem.Text == this.arrSelectedDoors[i].ToString())
						{
							wgRunInfoLog.addEventNotConnect(this.control4getRecordsFromController.ControllerSN, this.control4getRecordsFromController.IP, listViewItem);
							wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", listViewItem.Text, CommonStr.strCommFail));
							break;
						}
					}
				}
			}
			if (i < this.arrSelectedDoors.Count)
			{
				this.dealtDoorIndex = i;
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.control4getRecordsFromController.ControllerSN),
					information = string.Format("{0}", CommonStr.strGetSwipeRecordStart)
				});
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), CommonStr.strGettingSwipeRecord));
			}
			else
			{
				this.dealtDoorIndex = i;
				if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.Stop)
				{
					this.btnRealtimeGetRecords.Enabled = true;
					if (this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting && this.btnServer.Text != CommonStr.strMonitoring)
					{
						this.btnStopOthers.BackColor = Color.Transparent;
						this.btnStopMonitor.BackColor = Color.Transparent;
					}
					this.btnGetRecords.Enabled = true;
					this.mnuGetRecordsToolStripMenuItem.Enabled = true;
					this.btnUpload.Enabled = true;
					this.mnuUploadToolStripMenuItem.Enabled = true;
				}
			}
			this.displayNewestLog();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0008D568 File Offset: 0x0008C568
		private int getPrivilegesOfRentingHouseNow()
		{
			int num = -1;
			string text = this.arrSelectedDoors[this.dealtDoorIndex].ToString();
			icController icController = new icController();
			icController.GetInfoFromDBByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
			int controllerSN = icController.ControllerSN;
			this.pri.AllowDownload();
			if (this.dtPrivilege != null)
			{
				this.dtPrivilege.Rows.Clear();
				this.dtPrivilege.Dispose();
				this.dtPrivilege = null;
				GC.Collect();
			}
			this.dtPrivilege = new DataTable("Privilege");
			this.dtPrivilege.Columns.Add("f_CardNO", Type.GetType("System.Int64"));
			this.dtPrivilege.Columns.Add("f_BeginYMD", Type.GetType("System.DateTime"));
			this.dtPrivilege.Columns.Add("f_EndYMD", Type.GetType("System.DateTime"));
			this.dtPrivilege.Columns.Add("f_PIN", Type.GetType("System.String"));
			this.dtPrivilege.Columns.Add("f_ControlSegID1", Type.GetType("System.Byte"));
			this.dtPrivilege.Columns["f_ControlSegID1"].DefaultValue = 0;
			this.dtPrivilege.Columns.Add("f_ControlSegID2", Type.GetType("System.Byte"));
			this.dtPrivilege.Columns["f_ControlSegID2"].DefaultValue = 0;
			this.dtPrivilege.Columns.Add("f_ControlSegID3", Type.GetType("System.Byte"));
			this.dtPrivilege.Columns["f_ControlSegID3"].DefaultValue = 0;
			this.dtPrivilege.Columns.Add("f_ControlSegID4", Type.GetType("System.Byte"));
			this.dtPrivilege.Columns["f_ControlSegID4"].DefaultValue = 0;
			this.dtPrivilege.Columns.Add("f_AllowFloors", Type.GetType("System.UInt64"));
			this.dtPrivilege.Columns["f_AllowFloors"].DefaultValue = 1099511627775L;
			this.dtPrivilege.Columns.Add("f_ConsumerName", Type.GetType("System.String"));
			int num2 = this.pri.DownloadIP(icController.ControllerSN, icController.IP, icController.PORT, "", ref this.dtPrivilege);
			using (StreamWriter streamWriter = new StreamWriter(this.fileName, true))
			{
				if (num2 > 0)
				{
					num = this.dtPrivilege.Rows.Count;
					if (this.dtPrivilege.Rows.Count >= 0)
					{
						if (this.dtPrivilege.Rows.Count > 0)
						{
							for (int i = 0; i < this.dtPrivilege.Rows.Count; i++)
							{
								if (!string.IsNullOrEmpty(wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"])) && this.user.addNew(this.consumerNO.ToString(), wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"]), 0, 1, 0, 1, DateTime.Now, DateTime.Parse("2099-12-31 23:59:59"), 345678, long.Parse(this.dtPrivilege.Rows[i]["f_CardNO"].ToString())) > 0)
								{
									this.consumerNO += 1L;
									this.addedPrivilegeCnt += 1L;
								}
								streamWriter.WriteLine(string.Format("{0};{1};{2};{3};{4};{5}", new object[]
								{
									text,
									controllerSN.ToString(),
									num,
									i + 1,
									this.dtPrivilege.Rows[i]["f_CardNO"].ToString(),
									wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"])
								}).ToString());
							}
							return num;
						}
						streamWriter.WriteLine(string.Format("{0};{1};{2};{3};{4};{5}", new object[]
						{
							text,
							controllerSN.ToString(),
							"0",
							"",
							"",
							""
						}).ToString());
					}
					return num;
				}
				streamWriter.WriteLine(string.Format("{0};{1};{2};{3};{4};{5}", new object[]
				{
					text,
					controllerSN.ToString(),
					"-1",
					"",
					"",
					""
				}).ToString());
			}
			return num;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0008DAC4 File Offset: 0x0008CAC4
		private void getPrivilegesOfRentingHouseFromController(int result)
		{
			this.control4getRecordsFromController = new icController();
			this.control4getRecordsFromController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.dsWatchingDoorInfo);
			this.arrDealtController.Add(this.control4getRecordsFromController.ControllerSN, result);
			int i;
			for (i = this.dealtDoorIndex; i < this.arrSelectedDoors.Count; i++)
			{
				this.control4getRecordsFromController.GetInfoFromDBByDoorNameSpecial(this.arrSelectedDoors[i].ToString(), this.dsWatchingDoorInfo);
				if (!this.arrDealtController.ContainsKey(this.control4getRecordsFromController.ControllerSN))
				{
					break;
				}
				if (this.arrDealtController[this.control4getRecordsFromController.ControllerSN] >= 0)
				{
					InfoRow infoRow = new InfoRow();
					infoRow.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[i].ToString(), this.control4getRecordsFromController.ControllerSN);
					if (i == this.dealtDoorIndex)
					{
						infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strGetPrivilegesOfRentingHouseOK, this.arrDealtController[this.control4getRecordsFromController.ControllerSN]);
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[i].ToString(), CommonStr.strGetPrivilegesOfRentingHouseOK, this.arrDealtController[this.control4getRecordsFromController.ControllerSN]));
					}
					else
					{
						infoRow.information = string.Format("{0}", CommonStr.strAlreadyGotPrivileges);
					}
					wgRunInfoLog.addEvent(infoRow);
				}
				else
				{
					foreach (object obj in this.lstDoors.Items)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if (listViewItem.Text == this.arrSelectedDoors[i].ToString())
						{
							wgRunInfoLog.addEventNotConnect(this.control4getRecordsFromController.ControllerSN, this.control4getRecordsFromController.IP, listViewItem);
							wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", listViewItem.Text, CommonStr.strCommFail));
							break;
						}
					}
				}
			}
			if (i < this.arrSelectedDoors.Count)
			{
				this.dealtDoorIndex = i;
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.control4getRecordsFromController.ControllerSN),
					information = string.Format("{0}", CommonStr.strGetPrivilegesOfRentingHouseStart)
				});
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), CommonStr.strGettingPrivilegesOfRentingHouse));
			}
			else
			{
				this.dealtDoorIndex = i;
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = string.Format(CommonStr.strAddNewUsers, new object[0]),
					information = string.Format(CommonStr.strAddNewUsersCount + this.addedPrivilegeCnt.ToString(), new object[0])
				});
				this.btnRealtimeGetRecords.Enabled = true;
				if (this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting && this.btnServer.Text != CommonStr.strMonitoring)
				{
					this.btnStopOthers.BackColor = Color.Transparent;
					this.btnStopMonitor.BackColor = Color.Transparent;
				}
				this.btnGetRecords.Enabled = true;
				this.mnuGetRecordsToolStripMenuItem.Enabled = true;
				this.btnUpload.Enabled = true;
				this.mnuUploadToolStripMenuItem.Enabled = true;
			}
			this.displayNewestLog();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0008DE90 File Offset: 0x0008CE90
		private void displayTools()
		{
			if (!this.grpTool.Visible)
			{
				this.chkNeedCheckLosePacket.Checked = this.bNeedCheckLosePacket;
				this.chkDisplayNewestSwipe.Checked = !string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DISPLAY_NEWEST_SWIPE"));
				this.grpTool.Visible = true;
				this.grpTool.Size = new Size(310, 221);
				string text;
				wgAppConfig.getSystemParamValue(48, out text, out text, out this.strAllProductsDriversInfo);
				new Thread(new ThreadStart(this.checkInvalidCon)).Start();
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0008DF28 File Offset: 0x0008CF28
		private void btnHideTools_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("DISPLAY_NEWEST_SWIPE", this.chkDisplayNewestSwipe.Checked ? "1" : "");
			this.bNeedCheckLosePacket = this.chkNeedCheckLosePacket.Checked;
			this.grpTool.Visible = false;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0008DF78 File Offset: 0x0008CF78
		private void loadConsumer4TooManyRecordOfAccess()
		{
			if (this.bPCCheckGlobalAntiBackOpen || this.bPCCheckMealOpen)
			{
				this.bNotWriteToAccessDBAsTooManyRecord = true;
			}
			if (wgAppConfig.IsAccessDB && !this.bLoadTooManyRecords)
			{
				if (wgAppConfig.GetKeyVal("KEY_NOTWRITETOACCESSDBASTOOMANYRECORD") == "1")
				{
					this.bNotWriteToAccessDBAsTooManyRecord = true;
				}
				if (!this.backgroundWorker1.IsBusy)
				{
					this.backgroundWorker1.RunWorkerAsync();
				}
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0008DFE0 File Offset: 0x0008CFE0
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
			text += " , a.f_ControllerID  FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			this.dt = new DataTable();
			this.dvDoors = new DataView(this.dt);
			this.dvDoors4Watching = new DataView(this.dt);
			this.dvDoors4Check = new DataView(this.dt);
			this.dvDoors4Video = new DataView(this.dt);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dt);
						}
					}
					goto IL_010E;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dt);
					}
				}
			}
			IL_010E:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dt);
			try
			{
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.imgDoor2 = new ImageList();
			this.imgDoor2.ImageSize = new Size(24, 32);
			this.imgDoor2.TransparentColor = SystemColors.Window;
			string systemParamByNO = wgAppConfig.getSystemParamByNO(22);
			if (!string.IsNullOrEmpty(systemParamByNO))
			{
				decimal num = decimal.Parse(systemParamByNO, CultureInfo.InvariantCulture);
				if (num != 1m && num > 0m && num < 100m)
				{
					this.imgDoor2.ImageSize = new Size((int)(24m * num), (int)(32m * num));
				}
			}
			this.imgDoor2.Images.Add(Resources.pConsole_Door_Unknown);
			this.imgDoor2.Images.Add(Resources.pConsole_Door_NormalClose);
			if (this.bActivateDisplayYellowWhenDoorOpen)
			{
				this.imgDoor2.Images.Add(Resources.pConsole_Door_NormalOpenYellow);
			}
			else
			{
				this.imgDoor2.Images.Add(Resources.pConsole_Door_NormalOpen);
			}
			this.imgDoor2.Images.Add(Resources.pConsole_Door_NotConnected);
			this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnClose);
			this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnOpen);
			this.imgDoor2.Images.Add(Resources.pConsole_Door_Unknown);
			this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnClose);
			this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnOpen);
			this.imgDoor2.Images.Add(Resources.pConsole_Door_NotConnected);
			this.lstDoors.LargeImageList = this.imgDoor2;
			this.lstDoors.SmallImageList = this.imgDoor2;
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("CONSOLE_DOORVIEW")))
				{
					string keyVal = wgAppConfig.GetKeyVal("CONSOLE_DOORVIEW");
					if (keyVal == View.Details.ToString())
					{
						this.lstDoors.View = View.LargeIcon;
					}
					else if (keyVal == View.LargeIcon.ToString())
					{
						this.lstDoors.View = View.LargeIcon;
					}
					else if (keyVal == View.List.ToString())
					{
						this.lstDoors.View = View.List;
					}
					else if (keyVal == View.SmallIcon.ToString())
					{
						this.lstDoors.View = View.SmallIcon;
					}
					else if (keyVal == View.Tile.ToString())
					{
						this.lstDoors.View = View.Tile;
					}
					else
					{
						this.lstDoors.View = View.LargeIcon;
					}
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			this.lstDoors.Items.Clear();
			if (this.dvDoors.Count > 0)
			{
				wgTools.WriteLine("this.lstDoors.Items.Add(itm); Start");
				this.lstDoors.BeginUpdate();
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					if (!string.IsNullOrEmpty(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"])))
					{
						ListViewItem listViewItem = new ListViewItem();
						listViewItem.Text = wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]);
						listViewItem.ImageIndex = 0;
						listViewItem.Tag = new frmConsole.DoorSetInfo((int)this.dvDoors[i]["f_DoorID"], (string)this.dvDoors[i]["f_DoorName"], (int)((byte)this.dvDoors[i]["f_DoorNO"]), (int)this.dvDoors[i]["f_ControllerSN"], this.dvDoors[i]["f_IP"].ToString(), (int)this.dvDoors[i]["f_PORT"], (int)this.dvDoors[i]["f_ConnectState"], (int)this.dvDoors[i]["f_ZoneID"])
						{
							Selected = 0
						};
						this.lstDoors.Items.Add(listViewItem);
					}
				}
				this.lstDoors.EndUpdate();
				wgTools.WriteLine("this.lstDoors.Items.Add(itm); End");
			}
			text = " SELECT a.f_ReaderNO, a.f_ReaderName , b.f_ControllerSN, a.f_ReaderID ";
			text += " FROM t_b_Reader a, t_b_Controller b WHERE  b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID ";
			this.dtReader = new DataTable();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
					{
						using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
						{
							oleDbDataAdapter2.Fill(this.dtReader);
						}
					}
					goto IL_06B5;
				}
			}
			using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
				{
					using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
					{
						sqlDataAdapter2.Fill(this.dtReader);
					}
				}
			}
			IL_06B5:
			if (this.dtReader.Rows.Count > 0)
			{
				for (int j = 0; j < this.dtReader.Rows.Count; j++)
				{
					this.ReaderName.Add(string.Format("{0}-{1}", this.dtReader.Rows[j]["f_ControllerSN"].ToString(), this.dtReader.Rows[j]["f_ReaderNO"].ToString()), this.dtReader.Rows[j]["f_ReaderName"].ToString());
					this.ReaderID.Add(string.Format("{0}-{1}", this.dtReader.Rows[j]["f_ControllerSN"].ToString(), this.dtReader.Rows[j]["f_ReaderNO"].ToString()), this.dtReader.Rows[j]["f_ReaderID"].ToString());
				}
			}
			this.dvReaders4Video = new DataView(this.dtReader);
			if (this.dtReader.Rows.Count > 0)
			{
				for (int k = 0; k < this.dtReader.Rows.Count; k++)
				{
					if (this.dtReader.Rows[k]["f_ReaderName"].ToString().ToUpper().IndexOf("BACKUP") > 0)
					{
						this.bExistPhotoBackup = true;
						break;
					}
				}
			}
			this.pcCheckAccess_Init();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0008E900 File Offset: 0x0008D900
		private void loadOperatorPrivilege()
		{
			bool flag;
			bool flag2;
			icOperator.getFrmOperatorPrivilege(base.Name.ToString(), out flag, out flag2);
			if (flag2)
			{
				icOperator.getFrmOperatorPrivilege("btnCheckController", out flag, out flag2);
				this.btnCheck.Visible = flag || flag2;
				this.mnuCheck.Visible = this.btnCheck.Visible;
				icOperator.getFrmOperatorPrivilege("btnAdjustTime", out flag, out flag2);
				this.btnSetTime.Visible = flag2;
				icOperator.getFrmOperatorPrivilege("btnUpload", out flag, out flag2);
				this.btnUpload.Visible = flag2;
				icOperator.getFrmOperatorPrivilege("btnMonitor", out flag, out flag2);
				this.btnServer.Visible = flag || flag2;
				icOperator.getFrmOperatorPrivilege("btnGetRecords", out flag, out flag2);
				this.btnGetRecords.Visible = flag2;
				icOperator.getFrmOperatorPrivilege("btnRemoteOpen", out flag, out flag2);
				this.btnRemoteOpen.Visible = flag2;
				this.mnuRemoteOpenToolStripMenuItem.Visible = flag2;
				icOperator.getFrmOperatorPrivilege("btnRealtimeGetRecords", out flag, out flag2);
				this.btnRealtimeGetRecords.Visible = flag2;
				this.btnMaps.Visible = icOperator.OperatePrivilegeVisible("btnMaps");
				return;
			}
			if (flag)
			{
				icOperator.getFrmOperatorPrivilege("btnCheckController", out flag, out flag2);
				this.btnCheck.Visible = flag || flag2;
				this.mnuCheck.Visible = this.btnCheck.Visible;
				this.btnSetTime.Visible = false;
				this.btnUpload.Visible = false;
				icOperator.getFrmOperatorPrivilege("btnMonitor", out flag, out flag2);
				this.btnServer.Visible = flag || flag2;
				icOperator.getFrmOperatorPrivilege("btnMaps", out flag, out flag2);
				this.btnMaps.Visible = flag2 || flag;
				this.btnGetRecords.Visible = false;
				this.btnRemoteOpen.Visible = false;
				this.btnRealtimeGetRecords.Visible = false;
				return;
			}
			base.Close();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0008EAE0 File Offset: 0x0008DAE0
		private void loadPhoto(long cardno)
		{
			if (this.bMainWindowDisplay)
			{
				this.pictureBox1.Visible = false;
				try
				{
					string photoFileName = wgAppConfig.getPhotoFileName(cardno);
					Image image = this.pictureBox1.Image;
					wgAppConfig.ShowMyImage(photoFileName, ref image);
					if (image != null)
					{
						this.pictureBox1.Size = new Size(this.richTxtInfo.Width - this.pictureBox1.Location.X * 2, this.richTxtInfo.Height - this.pictureBox1.Location.Y);
						this.pictureBox1.Image = image;
						this.pictureBox1.Visible = true;
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0008EBB8 File Offset: 0x0008DBB8
		private void loadPhotoByConsumerNO(string consumerNO)
		{
			if (this.bMainWindowDisplay)
			{
				this.pictureBox1.Visible = false;
				try
				{
					string photoFileNameByConsumerNO = wgAppConfig.getPhotoFileNameByConsumerNO(consumerNO);
					Image image = this.pictureBox1.Image;
					wgAppConfig.ShowMyImage(photoFileNameByConsumerNO, ref image);
					if (image != null)
					{
						this.pictureBox1.Size = new Size(this.richTxtInfo.Width - this.pictureBox1.Location.X * 2, this.richTxtInfo.Height - this.pictureBox1.Location.Y);
						this.pictureBox1.Image = image;
						this.pictureBox1.Visible = true;
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0008EC90 File Offset: 0x0008DC90
		private void loadReaderIDWithCamera()
		{
			DbConnection dbConnection;
			DbCommand dbCommand;
			if (wgAppConfig.IsAccessDB)
			{
				dbConnection = new OleDbConnection(wgAppConfig.dbConString);
				dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
			}
			else
			{
				dbConnection = new SqlConnection(wgAppConfig.dbConString);
				dbCommand = new SqlCommand("", dbConnection as SqlConnection);
			}
			string text = " SELECT t_b_CameraTriggerSource.f_Id, t_b_CameraTriggerSource.f_CameraId, t_b_CameraTriggerSource.f_ReaderID, t_b_Camera.f_CameraName";
			text += " FROM t_b_CameraTriggerSource INNER JOIN t_b_Camera ON t_b_CameraTriggerSource.f_CameraId = t_b_Camera.f_CameraId ";
			try
			{
				if (dbConnection.State != ConnectionState.Open)
				{
					dbConnection.Open();
				}
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					if (this.arrReaderIDWithCamera.IndexOf(dbDataReader["f_ReaderID"]) < 0)
					{
						this.arrReaderIDWithCamera.Add(dbDataReader["f_ReaderID"]);
						this.arrCameraName4ReaderIDWithCamera.Add(dbDataReader["f_CameraName"]);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(text + "\r\n" + ex.ToString(), EventLogEntryType.Error);
			}
			finally
			{
				if (dbConnection.State != ConnectionState.Closed)
				{
					dbConnection.Close();
				}
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0008EDAC File Offset: 0x0008DDAC
		private void loadZoneInfo()
		{
			new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
			int i = this.arrZoneID.Count;
			this.cboZone.Items.Clear();
			for (i = 0; i < this.arrZoneID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrZoneName[i].ToString()))
				{
					this.cboZone.Items.Add(CommonStr.strAllZones);
				}
				else
				{
					this.cboZone.Items.Add(this.arrZoneName[i].ToString());
				}
			}
			if (this.cboZone.Items.Count > 0)
			{
				this.cboZone.SelectedIndex = 0;
			}
			this.cboZone.Visible = true;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0008EE88 File Offset: 0x0008DE88
		private void checkInvalidCon()
		{
			string text = this.strAllProductsDriversInfo;
			if (!string.IsNullOrEmpty(text))
			{
				if (text.IndexOf("\r\n") >= 0)
				{
					text = text.Substring(text.IndexOf("\r\n") + "\r\n".Length);
				}
				int num = 0;
				bool flag = false;
				while (text.IndexOf("SN=", num) >= 0)
				{
					int num2 = text.IndexOf("SN=", num);
					num = num2 + 3;
					long num3 = long.Parse(text.Substring(num2 + 3, 9));
					int num4 = text.IndexOf(num3.ToString() + ",VER=");
					string text2 = text.Substring(num4 + (num3.ToString() + ",VER=").Length);
					if (text2.Length > 0)
					{
						string text3 = text2;
						string text4 = text2.Substring(text2.IndexOf(",DATE=") + ",DATE=".Length, text2.IndexOf(",DATA=") - (text2.IndexOf(",DATE=") + ",DATE=".Length));
						int num5 = 0;
						int.TryParse(text4.Replace("-", ""), out num5);
						double num6 = wgTools.doubleParse(text2.Substring(0, text2.IndexOf(",")));
						if ((num6 > 5.48 && num6 < 7.0) || (num6 == 5.48 && num5 > 20140923))
						{
							string text5 = text3.Substring(text3.IndexOf(",DATA=") + 94, 8);
							if (!wgAppConfig.checkRSAController(num3) && num6 == 6.62 && text5.Equals("20171104"))
							{
								string keyVal = wgAppConfig.GetKeyVal("KEY_F_INVALIDCON");
								if (!string.IsNullOrEmpty(keyVal) && keyVal.IndexOf(num3.ToString() + ",") >= 0)
								{
									if (!wgAppConfig.getParamValBoolByNO(64))
									{
										flag = true;
									}
									wgAppConfig.wgLog(string.Format("SN={0},VER={1}:  {2}", num3, num6, CommonStr.strSupposeUpgradeDriver));
								}
							}
						}
					}
				}
				if (flag)
				{
					wgAppConfig.setSystemParamValue(64, "special value 7.95", "1", wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
					new Thread(new ThreadStart(wgMail.sendMailOnce)).Start();
				}
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0008F0EC File Offset: 0x0008E0EC
		private void checkParam(string shouldBe, string inFact, string title, string desc, bool bEnable)
		{
			wgTools.WriteLine(title);
			if (shouldBe != inFact)
			{
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = "[" + desc + "]" + CommonStr.strNeedUpload,
					information = string.Concat(new string[]
					{
						title,
						": ",
						CommonStr.strShouldBe,
						shouldBe,
						CommonStr.strInfact,
						inFact
					}),
					category = 501
				});
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0008F174 File Offset: 0x0008E174
		private void checkParamPrivileges(string doorName, icController controller, int infactPrivileges)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.checkParamPrivileges_Acc(doorName, controller, infactPrivileges);
				return;
			}
			try
			{
				if (controller.ControllerID <= 999 && !wgMjController.IsElevator(controller.ControllerSN))
				{
					string text = "SELECT * FROM t_b_Controller WHERE f_ControllerID = " + controller.ControllerID.ToString();
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						this.cm4ParamPrivilege = new SqlCommand("select rowcnt from sysindexes where id=object_id(N't_d_Privilege') and name = N'PK_t_d_Privilege'", sqlConnection);
						if (int.Parse(this.cm4ParamPrivilege.ExecuteScalar().ToString()) <= 2000000)
						{
							this.cm4ParamPrivilege = new SqlCommand(text, sqlConnection);
							this.cm4ParamPrivilege.CommandText = " SELECT COUNT( DISTINCT t_b_Consumer.f_CardNO) FROM t_b_Consumer ,t_d_Privilege  WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL  AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID  and f_ControllerID =" + controller.ControllerID.ToString();
							string text2 = wgTools.SetObjToStr(this.cm4ParamPrivilege.ExecuteScalar());
							if (!string.IsNullOrEmpty(text2))
							{
								int num = int.Parse(text2);
								if (num != infactPrivileges)
								{
									wgRunInfoLog.addEvent(new InfoRow
									{
										desc = string.Concat(new string[]
										{
											"[",
											doorName,
											"]",
											CommonStr.strPrivileges,
											CommonStr.strNeedUpload
										}),
										information = string.Format(string.Concat(new string[]
										{
											"[",
											controller.ControllerSN.ToString(),
											"]",
											CommonStr.strPrivileges,
											CommonStr.strNeedUpload,
											" [{0:d}-{1:d}],[{2:d}-{3:d}-{4:d}]"
										}), new object[] { infactPrivileges, num, 9, 9, 9 }),
										category = 501
									});
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0008F3BC File Offset: 0x0008E3BC
		private void checkParamPrivileges_Acc(string doorName, icController controller, int infactPrivileges)
		{
			try
			{
				if (controller.ControllerID <= 999 && !wgMjController.IsElevator(controller.ControllerSN))
				{
					string text = "SELECT * FROM t_b_Controller WHERE f_ControllerID = " + controller.ControllerID.ToString();
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text2 = wgTools.SetObjToStr(new OleDbCommand(text, oleDbConnection)
						{
							CommandText = " SELECT COUNT(*) FROM (SELECT DISTINCT t_b_Consumer.f_CardNO FROM t_b_Consumer ,t_d_Privilege  WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL  AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID  and f_ControllerID =" + controller.ControllerID.ToString() + ")"
						}.ExecuteScalar());
						if (!string.IsNullOrEmpty(text2))
						{
							int num = int.Parse(text2);
							if (num != infactPrivileges)
							{
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = string.Concat(new string[]
									{
										"[",
										doorName,
										"]",
										CommonStr.strPrivileges,
										CommonStr.strNeedUpload
									}),
									information = string.Format(string.Concat(new string[]
									{
										"[",
										controller.ControllerSN.ToString(),
										"]",
										CommonStr.strPrivileges,
										CommonStr.strNeedUpload,
										" [{0:d}-{1:d}],[{2:d}-{3:d}-{4:d}]"
									}), new object[] { infactPrivileges, num, 9, 9, 9 }),
									category = 501
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0008F5C0 File Offset: 0x0008E5C0
		private void dgvRunInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex == 0)
				{
					string text = e.Value as string;
					if (text != null)
					{
						DataGridViewCell dataGridViewCell = this.dgvRunInfo[e.ColumnIndex, e.RowIndex];
						dataGridViewCell.ToolTipText = text;
						DataGridViewRow dataGridViewRow = this.dgvRunInfo.Rows[e.RowIndex];
						e.Value = InfoRow.getImage(text, ref dataGridViewRow);
						if (this.bCameraEnabled)
						{
							uint num = 0U;
							uint.TryParse(this.dgvRunInfo[1, e.RowIndex].Value as string, out num);
							uint num2 = num;
							if (num2 > 0U && this.arrCapturedFilename.Count >= 0)
							{
								int num3 = this.arrCapturedFilenameRecID.IndexOf(num2);
								if (num3 >= 0)
								{
									string text2 = this.arrCapturedFilename[num3] as string;
									if (!string.IsNullOrEmpty(text2))
									{
										int i = 5;
										DateTime now = DateTime.Now;
										while (i > 0)
										{
											i--;
											if (wgAppConfig.FileIsExisted(wgAppConfig.Path4AviJpg() + text2 + ".jpg"))
											{
												e.Value = Resources.eventlogPhoto;
												break;
											}
											if (num2 != 1U && (long)(this.dgvRunInfo.RowCount - 1) >= (long)((ulong)num2))
											{
												break;
											}
											Thread.Sleep(50);
										}
										wgTools.WgDebugWrite(DateTime.Now.Subtract(now).TotalMilliseconds.ToString(), new object[0]);
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0008F77C File Offset: 0x0008E77C
		private void dgvRunInfo_Click(object sender, EventArgs e)
		{
			this.lastPhotoCnt = 0;
			try
			{
				if (this.bCameraEnabled && this.photoavi != null)
				{
					this.photoavi.dtShowByClick = DateTime.Now;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0008F7C8 File Offset: 0x0008E7C8
		private void dgvRunInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			if (!this.bDataErrorExist)
			{
				this.bDataErrorExist = true;
				wgAppConfig.wgLog(string.Format("dgvRunInfo_DataError  ColumnIndex ={0}, RowIndex ={1}, exception={2}, Context ={3} ", new object[]
				{
					e.ColumnIndex,
					e.RowIndex,
					e.Exception.ToString(),
					e.Context
				}));
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0008F833 File Offset: 0x0008E833
		private void dgvRunInfo_KeyDown(object sender, KeyEventArgs e)
		{
			this.frmConsole_KeyDown(sender, e);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0008F840 File Offset: 0x0008E840
		private void dgvRunInfo_SelectionChanged(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			try
			{
				num2 = this.dgvRunInfo.Rows.Count;
				num = 1;
				if (this.dgvRunInfo.SelectedRows.Count > 0)
				{
					num = 3;
					DataGridViewRow dataGridViewRow = this.dgvRunInfo.SelectedRows[0];
					num = 4;
					if (dataGridViewRow != null)
					{
						num = 5;
						if (string.IsNullOrEmpty(this.oldInfoTitleString))
						{
							try
							{
								this.oldInfoTitleString = this.dataGridView2.Columns[0].HeaderText;
							}
							catch (Exception)
							{
							}
						}
						this.pictureBox1.Visible = false;
						num = 51;
						this.txtInfo.Text = dataGridViewRow.Cells[5].Value as string;
						num = 52;
						this.txtInfo.Visible = false;
						num = 53;
						this.richTxtInfo.Text = dataGridViewRow.Cells[5].Value as string;
						num = 54;
						this.lblInfoID.Text = dataGridViewRow.Cells[1].Value as string;
						num = 6;
						if (dataGridViewRow.Cells[6].Value != null && !string.IsNullOrEmpty(dataGridViewRow.Cells[6].Value as string))
						{
							num = 7;
							if (wgAppConfig.IsPhotoNameFromConsumerNO)
							{
								try
								{
									if ((dataGridViewRow.Cells[5].Value as string).IndexOf(wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO)) > 0)
									{
										string[] array = (dataGridViewRow.Cells[5].Value as string).Split(new char[] { '\r' });
										if (array.Length > 2)
										{
											string[] array2 = array[1].Split(new char[] { '\t' });
											if (array2.Length >= 2)
											{
												string text = array2[1];
												if (!string.IsNullOrEmpty(text))
												{
													this.loadPhotoByConsumerNO(text);
												}
											}
										}
									}
									goto IL_021F;
								}
								catch (Exception ex)
								{
									wgAppConfig.wgLog(ex.ToString());
									goto IL_021F;
								}
							}
							MjRec mjRec = new MjRec(dataGridViewRow.Cells[6].Value as string);
							if (mjRec.IsSwipeRecord)
							{
								this.loadPhoto(mjRec.CardID);
							}
						}
						IL_021F:
						if (this.bCameraEnabled)
						{
							DateTime timeCapturePhoto = this.photoavi.timeCapturePhoto;
							if (DateTime.Now >= timeCapturePhoto.AddMilliseconds(1000.0))
							{
								this.photoavi.Text = dataGridViewRow.Cells[1].Value.ToString();
								uint num3 = 0U;
								uint.TryParse(dataGridViewRow.Cells[1].Value as string, out num3);
								int num4 = this.arrCapturedFilenameRecID.IndexOf(num3);
								if (num4 >= 0)
								{
									this.photoavi.fileName = this.arrCapturedFilename[num4].ToString();
									if (this.lastPhotoCnt != this.arrCapturedFilenameRecID.Count)
									{
										this.photoavi.Visible = true;
										this.lastPhotoCnt = this.arrCapturedFilenameRecID.Count;
									}
									this.photoavi.reload();
								}
								else
								{
									this.photoavi.fileName = null;
									this.photoavi.reload();
								}
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(string.Format("step = {0}, rowCount = {1} \r\n", num.ToString(), num2.ToString()) + ex2.ToString());
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0008FBEC File Offset: 0x0008EBEC
		private void dgvSwipeRecords_SelectionChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0008FBEE File Offset: 0x0008EBEE
		public void displayNewestLog()
		{
			base.Invoke(new frmConsole.dlgtdisplayNewestLog(this.dlgtdisplayNewestLogEntry));
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0008FC04 File Offset: 0x0008EC04
		private void dlgtdisplayNewestLogEntry()
		{
			int num = 0;
			int num2 = 0;
			try
			{
				num2 = this.dgvRunInfo.Rows.Count;
				if (num2 > 0)
				{
					num = 1;
					if (this.dgvRunInfo.Rows[num2 - 1] != null)
					{
						num = 2;
						if (this.dgvRunInfo.Rows[num2 - 1].Cells != null)
						{
							num = 3;
							if (this.dgvRunInfo.Rows[num2 - 1].Cells[1].Value != null)
							{
								num = 4;
								if (num2 == 1)
								{
									this.dgvRunInfo.FirstDisplayedScrollingRowIndex = 0;
								}
								else
								{
									this.dgvRunInfo.FirstDisplayedScrollingRowIndex = Math.Max(0, num2 - this.dgvRunInfo.DisplayedRowCount(true) + 1);
								}
								num = 5;
								this.dgvRunInfo.Rows[num2 - 1].Selected = true;
								this.dgvRunInfo.Rows[num2 - 1].Selected = false;
								Application.DoEvents();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(string.Format("step = {0}, rowCount = {1} \r\n", num.ToString(), num2.ToString()) + ex.ToString());
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0008FD38 File Offset: 0x0008ED38
		private void evtNewInfoCallBack(string text)
		{
			wgTools.WgDebugWrite("Got text through callback! {0}", new object[] { text });
			frmConsole.receivedPktCount++;
			if (this.bPCCheckMealOpen)
			{
				lock (this.qMjRec4MealOpen.SyncRoot)
				{
					this.qMjRec4MealOpen.Enqueue(text);
				}
			}
			if (this.bPCCheckGlobalAntiBackOpen)
			{
				lock (this.qMjRec4GlobalAntiBack.SyncRoot)
				{
					this.qMjRec4GlobalAntiBack.Enqueue(text);
				}
			}
			lock (this.QueRecText.SyncRoot)
			{
				this.QueRecText.Enqueue(text);
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0008FE14 File Offset: 0x0008EE14
		public int getAllInfoRowsCount()
		{
			return frmConsole.infoRowsCount;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0008FE1C File Offset: 0x0008EE1C
		private void getConsumerInfo(MjRec mjrec)
		{
			if (this.bLoadTooManyRecords && !this.bActivateDisplayCertID && mjrec.IsSwipeRecord)
			{
				this.dvConsumer4Access.RowFilter = string.Format("f_CardNO={0}", mjrec.CardID);
				int num = 0;
				string text = "";
				string text2 = "";
				string text3 = "";
				if (this.dvConsumer4Access.Count > 0)
				{
					num = (int)this.dvConsumer4Access[0]["f_ConsumerID"];
					text = this.dvConsumer4Access[0]["f_ConsumerName"] as string;
					text2 = this.dvConsumer4Access[0]["f_ConsumerNO"] as string;
					text3 = "";
					this.dvGroups4Access.RowFilter = string.Format("f_GroupID={0}", this.dvConsumer4Access[0]["f_GroupID"]);
					if (this.dvGroups4Access.Count > 0)
					{
						text3 = this.dvGroups4Access[0]["f_GroupName"] as string;
					}
				}
				mjrec.UpdateOnlySimple(num, text, text3, text2);
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0008FF48 File Offset: 0x0008EF48
		private void GetDoorInfoFromDB()
		{
			if (this.dsWatchingDoorInfo == null)
			{
				string text = "";
				DbDataAdapter dbDataAdapter;
				if (wgAppConfig.IsAccessDB)
				{
					DbConnection dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					new OleDbCommand(text, dbConnection as OleDbConnection);
					dbDataAdapter = new OleDbDataAdapter(text, dbConnection as OleDbConnection);
				}
				else
				{
					DbConnection dbConnection = new SqlConnection(wgAppConfig.dbConString);
					new SqlCommand(text, dbConnection as SqlConnection);
					dbDataAdapter = new SqlDataAdapter(text, dbConnection as SqlConnection);
				}
				this.dsWatchingDoorInfo = new DataSet();
				text = " SELECT *  FROM t_b_Door   ";
				dbDataAdapter.SelectCommand.CommandText = text;
				dbDataAdapter.Fill(this.dsWatchingDoorInfo, "t_b_Door");
				text = " SELECT *  FROM t_b_Controller   ";
				dbDataAdapter.SelectCommand.CommandText = text;
				dbDataAdapter.Fill(this.dsWatchingDoorInfo, "t_b_Controller");
				text = " SELECT *  FROM t_b_Reader   ";
				dbDataAdapter.SelectCommand.CommandText = text;
				dbDataAdapter.Fill(this.dsWatchingDoorInfo, "t_b_Reader");
				text = "  SELECT t_b_Reader.f_ReaderName, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
				text += "   t_b_Door.f_DoorName,    t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName, t_b_Door.f_ControllerID      FROM t_b_Floor , t_b_Door, t_b_Controller, t_b_Reader    where t_b_Floor.f_DoorID = t_b_Door.f_DoorID and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID and t_b_Reader.f_ControllerID = t_b_Floor.f_ControllerID  AND  t_b_Reader.f_ReaderNO =1 ";
				dbDataAdapter.SelectCommand.CommandText = text;
				dbDataAdapter.Fill(this.dsWatchingDoorInfo, "t_b_Floor");
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00090060 File Offset: 0x0008F060
		private void ipweb_webDisable(ref byte[] snData)
		{
			for (int i = 0; i < snData.Length; i++)
			{
				snData[i] = 0;
			}
			for (int j = 96; j < 192; j++)
			{
				snData[j] = byte.MaxValue;
				snData[1024 + (j >> 3)] = snData[1024 + (j >> 3)] | (byte)(1 << (j & 7));
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000900BE File Offset: 0x0008F0BE
		private void lstDoors_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000900C0 File Offset: 0x0008F0C0
		private void checkVideoCH()
		{
			if (!wgAppConfig.IsActivateCameraManage)
			{
				this.existedVideoCHDll = 0;
				return;
			}
			if (!icOperator.OperatePrivilegeVisible("mnuCameraMonitor"))
			{
				this.existedVideoCHDll = 0;
				return;
			}
			if (this.existedVideoCHDll < 0)
			{
				try
				{
					CHCNetSDK.NET_DVR_Init();
					this.existedVideoCHDll = 1;
				}
				catch
				{
					this.existedVideoCHDll = 0;
				}
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00090124 File Offset: 0x0008F124
		public void onTimeRefreshPhotoAvi(MjRec mjrec, ref bool bToCapturePhoto, uint recID)
		{
			if (this.bCameraEnabled && this.existedVideoCHDll > 0)
			{
				try
				{
					this.dvReaders4Video.RowFilter = string.Format("  [f_ControllerSN]= {0} AND [f_ReaderNO] = {1} ", mjrec.ControllerSN.ToString(), mjrec.ReaderNo.ToString());
					if (this.dvReaders4Video.Count > 0)
					{
						int num = (int)this.dvReaders4Video[0]["f_ReaderID"];
						try
						{
							if (this.photoavi == null || this.photoavi.IsDisposed)
							{
								this.photoavi = null;
								this.photoavi = new dfrmPhotoAvi();
								this.photoavi.frmCaller = this;
								this.photoavi.Show(this);
							}
							this.photoavi.timeCapturePhoto = DateTime.Now;
							this.photoavi.Text = recID.ToString();
							this.photoavi.newCardNo = mjrec.CardID;
							if (!mjrec.IsUserCardNO)
							{
								this.photoavi.newCardNo = (long)((ulong)(-1));
							}
							this.photoavi.fileName = mjrec.ReadDate.ToString("yyyyMMdd_HHmmss_") + mjrec.CardID.ToString() + "_" + mjrec.ToStringRaw();
							this.photoavi.CaptureNewCardRecord(num, this.photoavi.fileName, ref bToCapturePhoto);
							this.photoavi.newCardInfo = mjrec.ToDisplayDetail().Replace("\t", " ");
							this.photoavi.Visible = true;
							this.photoavi.Activate();
							this.photoavi.reload();
						}
						catch (Exception ex)
						{
							wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
						}
						if (this.bExistPhotoBackup)
						{
							string text = (string)this.dvReaders4Video[0]["f_ReaderName"];
							this.dvReaders4Video.RowFilter = string.Format("  [f_ControllerSN]= {0} AND [f_ReaderName] = {1} ", mjrec.ControllerSN.ToString(), wgTools.PrepareStr(text + "BACKUP"));
							if (this.dvReaders4Video.Count > 0)
							{
								num = (int)this.dvReaders4Video[0]["f_ReaderID"];
								try
								{
									if (this.photoavi == null || this.photoavi.IsDisposed)
									{
										this.photoavi = null;
										this.photoavi = new dfrmPhotoAvi();
										this.photoavi.frmCaller = this;
										this.photoavi.Show(this);
									}
									this.photoavi.newCardNo = mjrec.CardID;
									if (!mjrec.IsUserCardNO)
									{
										this.photoavi.newCardNo = (long)((ulong)(-1));
									}
									this.photoavi.fileName = string.Concat(new string[]
									{
										mjrec.ReadDate.ToString("yyyyMMdd_HHmmss_"),
										mjrec.CardID.ToString(),
										"_",
										mjrec.ToStringRaw(),
										"_BACKUP"
									});
									this.photoavi.CaptureNewCardRecord(num, this.photoavi.fileName, ref bToCapturePhoto);
									this.photoavi.newCardInfo = mjrec.ToDisplayDetail().Replace("\t", " ");
									this.photoavi.Visible = true;
									this.photoavi.Activate();
									this.photoavi.reload();
								}
								catch (Exception ex2)
								{
									wgAppConfig.wgDebugWrite(ex2.ToString(), EventLogEntryType.Error);
								}
							}
						}
					}
				}
				catch (Exception ex3)
				{
					wgAppConfig.wgDebugWrite(ex3.ToString(), EventLogEntryType.Error);
				}
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000904F4 File Offset: 0x0008F4F4
		public void openCamera()
		{
			int num = 0;
			int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(172)), out num);
			if (num != 1)
			{
				if (!icOperator.OperatePrivilegeVisible("mnuCameraMonitor"))
				{
					this.existedVideoCHDll = 0;
					return;
				}
				if (this.existedVideoCHDll <= 0)
				{
					if (this.bFalseShowOnce)
					{
						this.bFalseShowOnce = false;
						if (wgAppConfig.GetKeyVal("KEY_Video_DontDisplayErrorInfo") != "1" && XMessageBox.Show(CommonStr.strVideoDllExisted, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
						{
							wgAppConfig.UpdateKeyVal("KEY_Video_DontDisplayErrorInfo", "1");
							return;
						}
					}
				}
				else
				{
					try
					{
						if (this.photoavi == null || this.photoavi.IsDisposed)
						{
							if (!string.IsNullOrEmpty(this.selectedCameraID))
							{
								this.photoavi = null;
								this.photoavi = new dfrmPhotoAvi();
								this.photoavi.selectedCameraID = this.selectedCameraID;
								this.prevSelectedCameraID = this.selectedCameraID;
								this.photoavi.frmCaller = this;
								this.photoavi.Show(this);
								this.bCameraEnabled = true;
							}
						}
						else if (this.prevSelectedCameraID == this.selectedCameraID)
						{
							if (!this.photoavi.Visible)
							{
								this.photoavi.Show(this);
							}
							this.bCameraEnabled = true;
						}
						else
						{
							if (!this.photoavi.IsDisposed)
							{
								this.photoavi.bWatching = false;
								this.photoavi.stopVideo();
								this.photoavi.Close();
							}
							this.photoavi = null;
							if (!string.IsNullOrEmpty(this.selectedCameraID))
							{
								this.photoavi = new dfrmPhotoAvi();
								this.photoavi.selectedCameraID = this.selectedCameraID;
								this.prevSelectedCameraID = this.selectedCameraID;
								this.photoavi.frmCaller = this;
								this.photoavi.Show(this);
								this.bCameraEnabled = true;
							}
						}
						if (this.bCameraEnabled && this.arrCapturedFilenameRecID.Count == 0)
						{
							this.photoavi.Hide();
						}
					}
					catch (Exception ex)
					{
						wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
					}
				}
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00090724 File Offset: 0x0008F724
		private void refreshWarnAVI()
		{
			if (this.activateWarnAvi)
			{
				if (this.dgvRunInfo.Rows.Count <= 0)
				{
					this.lastRefreshValidIndex = -1;
					return;
				}
				int i = this.lastRefreshValidIndex;
				if (i != this.dgvRunInfo.Rows.Count - 1 && i != this.dgvRunInfo.Rows.Count)
				{
					if (i < this.dgvRunInfo.Rows.Count)
					{
						i++;
					}
					else
					{
						i = this.dgvRunInfo.Rows.Count - 1;
					}
					if (this.dgvRunInfo.Rows[i] != null)
					{
						bool flag = false;
						for (int j = 0; j < 4; j++)
						{
							if (!this.photoaviWarnVisible[j])
							{
								flag = true;
							}
							else if (this.photoaviWarn[j] == null)
							{
								flag = true;
							}
							else if (this.photoaviWarn[j] != null && !this.photoaviWarn[j].Visible)
							{
								flag = true;
							}
						}
						if (flag)
						{
							while (i < this.dgvRunInfo.Rows.Count)
							{
								DataGridViewRow dataGridViewRow = this.dgvRunInfo.Rows[i];
								dataGridViewRow.Cells[1].Value.ToString();
								flag = false;
								string text;
								if ((text = dataGridViewRow.Cells["f_Category"].Value.ToString()) != null)
								{
									if (!(text == "1") && !(text == "3"))
									{
										if (!(text == "5"))
										{
											if (text == "501")
											{
												flag = true;
											}
										}
										else
										{
											flag = true;
										}
									}
									else
									{
										flag = true;
									}
								}
								if (!flag)
								{
									this.lastRefreshValidIndex = i;
									i++;
								}
								else
								{
									uint num = 0U;
									uint.TryParse(dataGridViewRow.Cells[1].Value as string, out num);
									int num2 = this.arrCapturedFilenameRecID.IndexOf(num);
									if (num2 >= 0)
									{
										for (int k = 0; k < 4; k++)
										{
											bool flag2 = false;
											if (!this.photoaviWarnVisible[k])
											{
												flag2 = true;
											}
											else if (this.photoaviWarn[k] == null)
											{
												flag2 = true;
											}
											else if (this.photoaviWarn[k] != null && !this.photoaviWarn[k].Visible)
											{
												flag2 = true;
											}
											if (flag2)
											{
												dfrmPhotoAvi dfrmPhotoAvi = this.photoaviWarn[k];
												if (dfrmPhotoAvi == null || dfrmPhotoAvi.IsDisposed)
												{
													dfrmPhotoAvi = new dfrmPhotoAvi();
													dfrmPhotoAvi.frmCaller = this;
													this.photoaviWarn[k] = dfrmPhotoAvi;
												}
												dfrmPhotoAvi.FormBorderStyle = FormBorderStyle.FixedDialog;
												dfrmPhotoAvi.btnEnd.Visible = false;
												dfrmPhotoAvi.btnFirst.Visible = false;
												dfrmPhotoAvi.bWatching = false;
												dfrmPhotoAvi.MaximizeBox = false;
												dfrmPhotoAvi.StartPosition = FormStartPosition.Manual;
												dfrmPhotoAvi.Location = new Point(base.Location.X + base.Width * 3 / 4 - (dfrmPhotoAvi.Width + 5) * k, base.Location.Y + 100);
												dfrmPhotoAvi.Show(this);
												dfrmPhotoAvi.fileName = this.arrCapturedFilename[num2].ToString();
												dfrmPhotoAvi.Text = dataGridViewRow.Cells[1].Value.ToString();
												this.lastRefreshValidIndex = i;
												this.photoaviWarnVisible[k] = true;
												dfrmPhotoAvi.bDisplayWarnAvi = true;
												dfrmPhotoAvi.reload();
												dfrmPhotoAvi.startTimer1();
												return;
											}
										}
									}
									i++;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00090A90 File Offset: 0x0008FA90
		private string selectDoorCamera()
		{
			string text = "";
			ArrayList arrayList = new ArrayList();
			this.arrReaderId4Camera.Clear();
			try
			{
				foreach (object obj in this.lstDoors.Items)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if ((listViewItem.Tag as frmConsole.DoorSetInfo).Selected == 1)
					{
						if (wgMjController.GetControllerType((listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN) == 4)
						{
							this.dvReaders4Video.RowFilter = string.Format("  [f_ControllerSN]= {0} AND [f_ReaderNO] = {1} ", (listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN.ToString(), (listViewItem.Tag as frmConsole.DoorSetInfo).DoorNO.ToString());
						}
						else
						{
							this.dvReaders4Video.RowFilter = string.Format("  [f_ControllerSN]= {0} AND ([f_ReaderNO] = {1} OR [f_ReaderNO] = {2}) ", (listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN.ToString(), ((listViewItem.Tag as frmConsole.DoorSetInfo).DoorNO * 2 - 1).ToString(), ((listViewItem.Tag as frmConsole.DoorSetInfo).DoorNO * 2).ToString());
						}
						for (int i = 0; i <= this.dvReaders4Video.Count - 1; i++)
						{
							string text2 = wgTools.SetObjToStr(wgAppConfig.getValBySql("SELECT f_CameraId FROM t_b_CameraTriggerSource where f_ReaderID = " + this.dvReaders4Video[i]["f_ReaderID"]));
							if (!string.IsNullOrEmpty(text2) && int.Parse(text2) > 0)
							{
								this.arrReaderId4Camera.Add((int)this.dvReaders4Video[i]["f_ReaderID"]);
								if (arrayList.IndexOf(text2) < 0)
								{
									arrayList.Add(text2);
									if (text == "")
									{
										text += text2;
									}
									else
									{
										text = text + "," + text2;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00090CD0 File Offset: 0x0008FCD0
		private void GlobalAntiBackOpen_DealNewRecord()
		{
			try
			{
				if (!wgAppConfig.IsAccessControlBlue && this.bPCCheckGlobalAntiBackOpen)
				{
					for (;;)
					{
						Thread.Sleep(100);
						if (this.stepOfRealtimeGetRecords >= frmConsole.StepOfRealtimeGetReocrds.StartMonitoring)
						{
							goto IL_0042;
						}
						if (this.stepOfRealtimeGetRecords == frmConsole.StepOfRealtimeGetReocrds.Stop)
						{
							break;
						}
						if (!this.bPCCheckGlobalAntiBackOpen || this.bGlobalAntiBackStop)
						{
							goto IL_0042;
						}
					}
					return;
					IL_0042:
					if (!this.bGlobalAntiBackStop && this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.Stop && this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.DelSwipe)
					{
						if (this.GlobalAntiBackOpen_updateLocationDT() == 0)
						{
							wgAppConfig.wgLogWithoutDB(CommonStr.strGlobalAntiPassbackStop2015, EventLogEntryType.Information, null);
							base.Invoke(new frmConsole.GlobalAntiBackOpen_updateInfo(this.addEventSpecstrGlobalAntiPassbackStop2015));
						}
						else
						{
							bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(121);
							this.Delay5SecUpdateDoor = 30;
							this.GlobalAntiBackOpen_SetControllerConfigure(true);
							this.Delay5SecUpdateDoor = 30;
							wgAppConfig.wgLogWithoutDB(CommonStr.strGlobalAntiPassbackStart2015, EventLogEntryType.Information, null);
							base.Invoke(new frmConsole.GlobalAntiBackOpen_updateInfo(this.addEventSpecstrGlobalAntiPassbackStart2015));
							try
							{
								this.qMjRec4GlobalAntiBack.Clear();
								DataView dataView = null;
								DataView dataView2 = null;
								if (wgAppConfig.getParamValBoolByNO(131))
								{
									string text = " SELECT t_b_ControllerTaskList.*,t_b_Door.f_DoorNO, t_b_Door.f_ControllerID FROM t_b_ControllerTaskList ";
									text += " LEFT JOIN t_b_Door ON t_b_ControllerTaskList.f_DoorID = t_b_Door.f_DoorID ";
									SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
									SqlCommand sqlCommand = new SqlCommand(text, sqlConnection);
									SqlCommand sqlCommand2 = new SqlCommand("Select * from [t_b_Controller]", sqlConnection);
									DataSet dataSet = new DataSet("task");
									SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
									SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2);
									sqlDataAdapter.Fill(dataSet, "task");
									sqlDataAdapter2.Fill(dataSet, "controller");
									dataView = new DataView(dataSet.Tables["task"]);
									dataView2 = new DataView(dataSet.Tables["controller"]);
								}
								try
								{
									while (this.bPCCheckGlobalAntiBackOpen && !this.bGlobalAntiBackStop)
									{
										do
										{
											this.bGlobalAntiBackDealing = true;
											if (this.qMjRec4GlobalAntiBack.Count <= 0)
											{
												Thread.Sleep(1);
											}
											else
											{
												string text2;
												lock (this.qMjRec4GlobalAntiBack.SyncRoot)
												{
													text2 = this.qMjRec4GlobalAntiBack.Dequeue() as string;
												}
												MjRec mjRec = new MjRec(text2);
												if (mjRec.ControllerSN > 0U)
												{
													try
													{
														if (this.arrSelectedControllers.IndexOf((int)mjRec.ControllerSN) >= 0 && mjRec.IsSwipeRecord)
														{
															if (!mjRec.GetUserInfoWithLocationFromDB())
															{
																int num = 1;
																for (int i = 0; i < this.dtGlobalAntiBackOpen_Readers4Exit.Rows.Count; i++)
																{
																	if ((long)((int)this.dtGlobalAntiBackOpen_Readers4Exit.Rows[i]["f_ControllerSN"]) == (long)((ulong)mjRec.ControllerSN) && (byte)this.dtGlobalAntiBackOpen_Readers4Exit.Rows[i]["f_DoorNO"] == mjRec.DoorNo)
																	{
																		num = 0;
																		break;
																	}
																}
																icController icController = new icController();
																icController.GetInfoFromDBByControllerSN((int)mjRec.ControllerSN);
																frmConsole.GlobalAntiBackOpen_clsControlInfoGlobalAntiBack globalAntiBackOpen_clsControlInfoGlobalAntiBack = new frmConsole.GlobalAntiBackOpen_clsControlInfoGlobalAntiBack();
																globalAntiBackOpen_clsControlInfoGlobalAntiBack.doorController = icController;
																globalAntiBackOpen_clsControlInfoGlobalAntiBack.DoorNo = (int)mjRec.DoorNo;
																globalAntiBackOpen_clsControlInfoGlobalAntiBack.CardID = mjRec.CardID;
																globalAntiBackOpen_clsControlInfoGlobalAntiBack.inOut = num;
																globalAntiBackOpen_clsControlInfoGlobalAntiBack.ConsumerName = mjRec.consumerName;
																globalAntiBackOpen_clsControlInfoGlobalAntiBack.consumerID = mjRec.consumerID;
																globalAntiBackOpen_clsControlInfoGlobalAntiBack.bGlobalAntiBack = true;
																ThreadPool.QueueUserWorkItem(new WaitCallback(this.GlobalAntiBackOpen_SendData), globalAntiBackOpen_clsControlInfoGlobalAntiBack);
															}
															else if (mjRec.consumerID > 0 && mjRec.doorEnabled > 0)
															{
																if (wgAppConfig.getParamValBoolByNO(131))
																{
																	dataView2.RowFilter = "f_ControllerSN = " + mjRec.ControllerSN.ToString();
																	if (dataView2.Count > 0)
																	{
																		dataView.RowFilter = string.Format(" [f_DoorID]=0 OR [f_controllerID]= {0}", dataView2[0]["f_ControllerID"].ToString());
																		if (dataView.Count > 0)
																		{
																			int num2 = 0;
																			long num3 = 0L;
																			DateTime now = DateTime.Now;
																			MjControlTaskItem mjControlTaskItem = new MjControlTaskItem();
																			for (int j = 0; j < dataView.Count; j++)
																			{
																				DataRowView dataRowView = dataView[j];
																				mjControlTaskItem.ymdStart = (DateTime)dataRowView["f_BeginYMD"];
																				mjControlTaskItem.ymdEnd = (DateTime)dataRowView["f_EndYMD"];
																				mjControlTaskItem.hms = (DateTime)dataRowView["f_OperateTime"];
																				int num4 = 0;
																				num4 = num4 * 2 + (int)((byte)dataRowView["f_Sunday"]);
																				num4 = num4 * 2 + (int)((byte)dataRowView["f_Saturday"]);
																				num4 = num4 * 2 + (int)((byte)dataRowView["f_Friday"]);
																				num4 = num4 * 2 + (int)((byte)dataRowView["f_Thursday"]);
																				num4 = num4 * 2 + (int)((byte)dataRowView["f_Wednesday"]);
																				num4 = num4 * 2 + (int)((byte)dataRowView["f_Tuesday"]);
																				num4 = num4 * 2 + (int)((byte)dataRowView["f_Monday"]);
																				mjControlTaskItem.weekdayControl = (byte)num4;
																				mjControlTaskItem.paramLoc = 0;
																				if ((int)dataRowView["f_DoorID"] == 0)
																				{
																					switch ((int)dataRowView["f_DoorControl"])
																					{
																					case 0:
																						mjControlTaskItem.paramValue = 3;
																						break;
																					case 1:
																						mjControlTaskItem.paramValue = 1;
																						break;
																					case 2:
																						mjControlTaskItem.paramValue = 2;
																						break;
																					default:
																						mjControlTaskItem.paramValue = 0;
																						mjControlTaskItem.paramLoc = 0;
																						break;
																					}
																				}
																				else if ((byte)dataRowView["f_DoorNO"] == mjRec.DoorNo)
																				{
																					switch ((int)dataRowView["f_DoorControl"])
																					{
																					case 0:
																						mjControlTaskItem.paramValue = 3;
																						break;
																					case 1:
																						mjControlTaskItem.paramValue = 1;
																						break;
																					case 2:
																						mjControlTaskItem.paramValue = 2;
																						break;
																					default:
																						mjControlTaskItem.paramValue = 0;
																						break;
																					}
																				}
																				if (mjControlTaskItem.paramValue != 0 && long.Parse(now.Date.ToString("yyyyMMdd")) >= long.Parse(mjControlTaskItem.ymdStart.ToString("yyyyMMdd")) && long.Parse(now.Date.ToString("yyyyMMdd")) <= long.Parse(mjControlTaskItem.ymdEnd.ToString("yyyyMMdd")))
																				{
																					int num5 = (int)now.DayOfWeek;
																					if (num5 == 0)
																					{
																						num5 = 7;
																					}
																					num5--;
																					if ((num4 & (1 << num5)) > 0 && long.Parse(now.ToString("HHmmss")) > long.Parse(mjControlTaskItem.hms.ToString("HHmmss")) && long.Parse(mjControlTaskItem.hms.ToString("HHmmss")) > num3)
																					{
																						num3 = long.Parse(mjControlTaskItem.hms.ToString("HHmmss"));
																						num2 = (int)mjControlTaskItem.paramValue;
																					}
																				}
																			}
																			if (num2 == 2)
																			{
																				goto IL_0B0E;
																			}
																		}
																	}
																}
																int num6 = 1;
																int lastInOut = mjRec.lastInOut;
																for (int k = 0; k < this.dtGlobalAntiBackOpen_Readers4Exit.Rows.Count; k++)
																{
																	if ((long)((int)this.dtGlobalAntiBackOpen_Readers4Exit.Rows[k]["f_ControllerSN"]) == (long)((ulong)mjRec.ControllerSN) && (byte)this.dtGlobalAntiBackOpen_Readers4Exit.Rows[k]["f_DoorNO"] == mjRec.DoorNo)
																	{
																		num6 = 0;
																		break;
																	}
																}
																if (mjRec.lastSwipeRecID <= 0 && mjRec.lastRemoteOpen_ControllerSN <= 0)
																{
																	if (this.GlobalAntiBackOpen_bFirstInThenOut && num6 == 0)
																	{
																		goto IL_0B0E;
																	}
																}
																else if (num6 == lastInOut)
																{
																	goto IL_0B0E;
																}
																if (this.checkTimeSegment(num6, mjRec.CardID) && this.checkLimitedPersons(num6, mjRec.CardID))
																{
																	if (mjRec.ReadDate.Year != 2009)
																	{
																		if (mjRec.ReadDate.Date > mjRec.endYMD.Date || mjRec.ReadDate.Date < mjRec.beginYMD.Date)
																		{
																			goto IL_0B0E;
																		}
																	}
																	else if (DateTime.Now.Date > mjRec.endYMD.Date || DateTime.Now.Date < mjRec.beginYMD.Date)
																	{
																		goto IL_0B0E;
																	}
																	if (wgAppConfig.IsPrivilegeTypeManagementModeActive)
																	{
																		if (mjRec.privilegeTypeID == 0)
																		{
																			goto IL_0B0E;
																		}
																		if (this.dtGlobalAntiBackOpen_PrivilegeType.Rows.Count > 0)
																		{
																			this.dvGlobalAntiBackOpen_PrivilegeType.RowFilter = string.Format("f_PrivilegeTypeID={0} and f_ControllerSN={1} and f_DoorNO ={2} ", mjRec.privilegeTypeID, mjRec.ControllerSN, mjRec.DoorNo);
																			if (this.dvGlobalAntiBackOpen_PrivilegeType.Count <= 0)
																			{
																				goto IL_0B0E;
																			}
																			if ((int)this.dvGlobalAntiBackOpen_PrivilegeType[0]["f_ControlSegID"] != 1)
																			{
																				if ((int)this.dvGlobalAntiBackOpen_PrivilegeType[0]["f_ControlSegID"] == 0)
																				{
																					goto IL_0B0E;
																				}
																				if (paramValBoolByNO)
																				{
																					this.dvGlobalAntiBackOpen_ControlTimeSeg.RowFilter = string.Format("f_ControlSegID = {0}", ((int)this.dvGlobalAntiBackOpen_PrivilegeType[0]["f_ControlSegID"]).ToString());
																					if (this.dvGlobalAntiBackOpen_ControlTimeSeg.Count <= 0)
																					{
																						goto IL_0B0E;
																					}
																				}
																			}
																		}
																	}
																	else if (wgAppConfig.getValBySql(string.Format("SELECT [t_d_Privilege].[f_ControlSegID] FROM [t_d_Privilege],[t_b_Controller] WHERE [f_ConsumerID]={0} AND [t_d_Privilege].[f_ControllerID]=[t_b_Controller].[f_ControllerID] AND [t_b_Controller].[f_ControllerSN]={1} AND [t_d_Privilege].[f_DoorNO]={2}", mjRec.consumerID, mjRec.ControllerSN, mjRec.DoorNo)) == 0)
																	{
																		goto IL_0B0E;
																	}
																	ControllerRunInformation runInfo = this.watching.GetRunInfo((int)mjRec.ControllerSN);
																	if (runInfo == null)
																	{
																		if (DateTime.Now <= this.watchingStartTime.AddSeconds(3.0))
																		{
																		}
																	}
																	else if (runInfo.dtNow.Year >= 2015 && Convert.ToDateTime(runInfo.dtNow).Subtract(mjRec.ReadDate).TotalSeconds > 3.0)
																	{
																		goto IL_0B0E;
																	}
																	icController icController2 = new icController();
																	icController2.GetInfoFromDBByControllerSN((int)mjRec.ControllerSN);
																	frmConsole.GlobalAntiBackOpen_clsControlInfoGlobalAntiBack globalAntiBackOpen_clsControlInfoGlobalAntiBack2 = new frmConsole.GlobalAntiBackOpen_clsControlInfoGlobalAntiBack();
																	globalAntiBackOpen_clsControlInfoGlobalAntiBack2.doorController = icController2;
																	globalAntiBackOpen_clsControlInfoGlobalAntiBack2.DoorNo = (int)mjRec.DoorNo;
																	globalAntiBackOpen_clsControlInfoGlobalAntiBack2.CardID = mjRec.CardID;
																	globalAntiBackOpen_clsControlInfoGlobalAntiBack2.inOut = num6;
																	if (this.GlobalAntiBackOpen_bPersonsInside)
																	{
																		if (num6 == 1)
																		{
																			this.GlobalAntiBackOpen_PersonsInside++;
																		}
																		else if (this.GlobalAntiBackOpen_PersonsInside > 0)
																		{
																			this.GlobalAntiBackOpen_PersonsInside--;
																		}
																	}
																	globalAntiBackOpen_clsControlInfoGlobalAntiBack2.ConsumerName = mjRec.consumerName;
																	globalAntiBackOpen_clsControlInfoGlobalAntiBack2.consumerID = mjRec.consumerID;
																	globalAntiBackOpen_clsControlInfoGlobalAntiBack2.bGlobalAntiBack = true;
																	ThreadPool.QueueUserWorkItem(new WaitCallback(this.GlobalAntiBackOpen_SendData), globalAntiBackOpen_clsControlInfoGlobalAntiBack2);
																}
															}
														}
													}
													catch (Exception)
													{
														break;
													}
												}
											}
											IL_0B0E:;
										}
										while (this.bPCCheckGlobalAntiBackOpen && !this.bGlobalAntiBackStop);
									}
								}
								catch (Exception ex)
								{
									wgAppConfig.wgDebugWrite(ex.ToString());
								}
							}
							catch (Exception ex2)
							{
								wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
							}
							finally
							{
								this.bGlobalAntiBackDealing = false;
							}
							this.GlobalAntiBackOpen_SetControllerConfigure(false);
							wgAppConfig.wgLogWithoutDB(CommonStr.strGlobalAntiPassbackStop2015, EventLogEntryType.Information, null);
							base.Invoke(new frmConsole.GlobalAntiBackOpen_updateInfo(this.addEventSpecstrGlobalAntiPassbackStop2015));
						}
					}
				}
			}
			catch (Exception ex3)
			{
				wgAppConfig.wgLog(ex3.ToString());
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00091914 File Offset: 0x00090914
		private int GlobalAntiBackOpen_findConsumerIDHalf(int id)
		{
			int num = 1;
			int num2 = this.dtGlobalAntiBackOpen_ConsumerLocation.Rows.Count;
			int num3 = 20;
			while (num <= num2 && num3-- > 0)
			{
				int num4 = num;
				num4 += num2;
				int num5 = num4 / 2;
				int num6 = (int)this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[num5 - 1]["f_ConsumerID"];
				if (num6 == id)
				{
					return num5 - 1;
				}
				if (id > num6)
				{
					num = num5 + 1;
				}
				else if (num5 != 1)
				{
					num2 = num5 - 1;
				}
				else
				{
					num = num2 + 1;
				}
			}
			return -1;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000919A0 File Offset: 0x000909A0
		private void GlobalAntiBackOpen_Init()
		{
			if (this.bPCCheckGlobalAntiBackOpen)
			{
				ThreadPool.SetMaxThreads(10, 10);
				int num = 0;
				int.TryParse(wgAppConfig.GetKeyVal("KEY_MaxThreadNum"), out num);
				if (num > 0)
				{
					ThreadPool.SetMaxThreads(num, num);
				}
				int num2 = 0;
				int num3 = 0;
				ThreadPool.GetMaxThreads(out num2, out num3);
				wgTools.WgDebugWrite(string.Format("ThreadPool.GetMaxThreads: {0},{1}", num2, num3), new object[0]);
				this.bGlobalAntiBackStop = false;
				if (this.threadGlobalAntiBackOpen_DealNewRecord != null && this.threadGlobalAntiBackOpen_DealNewRecord.IsAlive)
				{
					this.threadGlobalAntiBackOpen_DealNewRecord.Interrupt();
				}
				this.threadGlobalAntiBackOpen_DealNewRecord = new Thread(new ThreadStart(this.GlobalAntiBackOpen_DealNewRecord));
				this.threadGlobalAntiBackOpen_DealNewRecord.Start();
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00091A5C File Offset: 0x00090A5C
		private void GlobalAntiBackOpen_SendData(object info)
		{
			frmConsole.GlobalAntiBackOpen_clsControlInfoGlobalAntiBack globalAntiBackOpen_clsControlInfoGlobalAntiBack = info as frmConsole.GlobalAntiBackOpen_clsControlInfoGlobalAntiBack;
			try
			{
				icController doorController = globalAntiBackOpen_clsControlInfoGlobalAntiBack.doorController;
				if (doorController.RemoteOpenDoorIPNoStartDelay(globalAntiBackOpen_clsControlInfoGlobalAntiBack.DoorNo, 0U, globalAntiBackOpen_clsControlInfoGlobalAntiBack.CardID, (globalAntiBackOpen_clsControlInfoGlobalAntiBack.inOut > 0) ? 0 : 1) > 0 && globalAntiBackOpen_clsControlInfoGlobalAntiBack.bGlobalAntiBack)
				{
					if (wgAppConfig.runUpdateSql(string.Format("UPDATE [t_b_Consumer_Location] SET [f_Last_InOut] ={0},f_lastRemoteOpen_ReadDate={1},[f_lastRemoteOpen_ControllerSN] = {2},[f_lastRemoteOpen_DoorNO] = {3}  WHERE f_ConsumerID={4}", new object[]
					{
						globalAntiBackOpen_clsControlInfoGlobalAntiBack.inOut,
						wgTools.PrepareStr(DateTime.Now, true, wgTools.YMDHMSFormat),
						globalAntiBackOpen_clsControlInfoGlobalAntiBack.doorController.ControllerSN,
						globalAntiBackOpen_clsControlInfoGlobalAntiBack.DoorNo,
						globalAntiBackOpen_clsControlInfoGlobalAntiBack.consumerID
					})) <= 0)
					{
						wgAppConfig.runUpdateSql(string.Format("INSERT INTO [t_b_Consumer_Location](f_ConsumerID, [f_Last_InOut],f_lastRemoteOpen_ReadDate,f_lastRemoteOpen_ControllerSN,f_lastRemoteOpen_DoorNO) VALUES({0},{1},{2},{3},{4})", new object[]
						{
							globalAntiBackOpen_clsControlInfoGlobalAntiBack.consumerID,
							globalAntiBackOpen_clsControlInfoGlobalAntiBack.inOut,
							wgTools.PrepareStr(DateTime.Now, true, wgTools.YMDHMSFormat),
							globalAntiBackOpen_clsControlInfoGlobalAntiBack.doorController.ControllerSN,
							globalAntiBackOpen_clsControlInfoGlobalAntiBack.DoorNo
						}));
					}
					wgAppConfig.wgLog(string.Format("{0}[{1:d}],", doorController.GetDoorName(globalAntiBackOpen_clsControlInfoGlobalAntiBack.DoorNo), doorController.ControllerSN) + string.Format("{0}{1}\t[{2}]", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRecordRemoteOpenDoor_ByPCGlobalAntiBackOpen), globalAntiBackOpen_clsControlInfoGlobalAntiBack.CardID, globalAntiBackOpen_clsControlInfoGlobalAntiBack.ConsumerName));
				}
				else
				{
					wgAppConfig.wgLog(string.Format("{0}[{1:d}],", doorController.GetDoorName(globalAntiBackOpen_clsControlInfoGlobalAntiBack.DoorNo), doorController.ControllerSN) + string.Format("{0}{1}\t[{2}]", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK), globalAntiBackOpen_clsControlInfoGlobalAntiBack.CardID, globalAntiBackOpen_clsControlInfoGlobalAntiBack.ConsumerName));
				}
				doorController.Dispose();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00091C68 File Offset: 0x00090C68
		private void GlobalAntiBackOpen_SetControllerConfigure(bool bGlobalAntiBack)
		{
			bool flag = false;
			new ArrayList();
			new ArrayList();
			Queue queue = new Queue();
			ArrayList arrayList = new ArrayList();
			new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			bool flag2 = false;
			new ArrayList();
			new ArrayList();
			queue.Clear();
			arrayList.Clear();
			new ArrayList();
			new ArrayList();
			new icController();
			icController icController = new icController();
			for (int i = 0; i < this.arrSelectedControllers.Count; i++)
			{
				icController.GetInfoFromDBByControllerSN((int)this.arrSelectedControllers[i]);
				if (string.IsNullOrEmpty(icController.IP))
				{
					flag = true;
					break;
				}
			}
			wgTools.WriteLine("发送指令");
			int num = 0;
			int num2 = 3;
			DateTime dateTime = DateTime.Now.AddSeconds(2.0);
			DateTime now = DateTime.Now;
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			new ArrayList();
			wgUdpComm wgUdpComm = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList5 = new ArrayList();
			byte[] array = null;
			DateTime now2 = DateTime.Now;
			new ArrayList();
			DateTime now3 = DateTime.Now;
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 32;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
			if (bGlobalAntiBack)
			{
				wgMjControllerConfigure.pcControlSwipeTimeout = 30;
			}
			else
			{
				wgMjControllerConfigure.pcControlSwipeTimeout = 0;
			}
			wgMjControllerConfigure.paramData.CopyTo(wgpacketWith1152_internal.ucData, 0);
			wgMjControllerConfigure.needUpdate.CopyTo(wgpacketWith1152_internal.ucData, 1024);
			if (!bGlobalAntiBack)
			{
				icController.ControllerSN = -1;
				wgpacketWith1152_internal.iDevSnTo = (uint)icController.ControllerSN;
				wgpacketWith1152_internal.GetNewXid();
				byte[] array2 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
				wgUdpComm.udp_get_onlySend(array2, 300, wgpacketWith1152_internal.xid, "", 60000, ref array);
				wgAppConfig.wgLog(string.Format("[{0:d}],", icController.ControllerSN) + string.Format("{0} {1}", bGlobalAntiBack ? CommonStr.strGlobalAntiPassbackOn2015 : CommonStr.strGlobalAntiPassbackOff2015, CommonStr.strSuccessfully));
				long xidOfCommand = wgUdpComm.getXidOfCommand(array2);
				DateTime dateTime2 = DateTime.Now.AddMilliseconds(400.0);
				Thread.Sleep(5);
				Thread.Sleep(10);
				if (wgTools.bUDPCloud > 0)
				{
					Thread.Sleep(300);
				}
				for (;;)
				{
					array = wgUdpComm.GetPacket();
					if (array == null && wgTools.bUDPCloud > 0)
					{
						array = wgTools.wgcloud.GetPacket4get();
					}
					if (array != null)
					{
						num++;
						if (xidOfCommand == wgUdpComm.getXidOfCommand(array))
						{
							int num3 = (int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24);
							if (this.arrSelectedControllers.IndexOf(num3) >= 0 && arrayList4.IndexOf(num3) < 0)
							{
								arrayList4.Add(num3);
								if (arrayList4.Count == this.arrSelectedControllers.Count)
								{
									break;
								}
							}
						}
						array = null;
					}
					else
					{
						Thread.Sleep(5);
					}
					if (!(dateTime2 > DateTime.Now))
					{
						goto IL_0320;
					}
				}
				flag2 = true;
			}
			IL_0320:
			if (!flag2)
			{
				if (flag)
				{
					for (int j = 0; j < this.arrSelectedControllers.Count; j++)
					{
						icController.GetInfoFromDBByControllerSN((int)this.arrSelectedControllers[j]);
						if (string.IsNullOrEmpty(icController.IP))
						{
							wgpacketWith1152_internal.iDevSnTo = (uint)icController.ControllerSN;
							wgpacketWith1152_internal.GetNewXid();
							byte[] array3 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
							wgUdpComm.udp_get_onlySend(array3, 300, wgpacketWith1152_internal.xid, "", 60000, ref array);
							arrayList5.Add(wgUdpComm.getXidOfCommand(array3));
							arrayList2.Add(icController.ControllerSN);
							Thread.Sleep(5);
						}
					}
					Thread.Sleep(10);
					DateTime dateTime3 = DateTime.Now.AddMilliseconds(400.0);
					do
					{
						array = wgUdpComm.GetPacket();
						if (array == null && wgTools.bUDPCloud > 0)
						{
							array = wgTools.wgcloud.GetPacket4get();
						}
						if (array == null)
						{
							break;
						}
						num++;
						int num4 = arrayList5.IndexOf(wgUdpComm.getXidOfCommand(array));
						if (num4 >= 0 && arrayList4.IndexOf(arrayList2[num4]) < 0)
						{
							arrayList4.Add(arrayList2[num4]);
							if (arrayList4.Count == arrayList2.Count)
							{
								break;
							}
						}
						array = null;
					}
					while (dateTime3 > DateTime.Now);
					arrayList3.Clear();
				}
				if (arrayList4.Count == this.arrSelectedControllers.Count)
				{
					flag2 = true;
				}
				else
				{
					for (int j = 0; j < this.arrSelectedControllers.Count; j++)
					{
						icController.GetInfoFromDBByControllerSN((int)this.arrSelectedControllers[j]);
						if (!string.IsNullOrEmpty(icController.IP))
						{
							wgpacketWith1152_internal.iDevSnTo = (uint)icController.ControllerSN;
							wgpacketWith1152_internal.GetNewXid();
							byte[] array4 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
							wgUdpComm.udp_get_onlySend(array4, 300, wgpacketWith1152_internal.xid, icController.IP, icController.PORT, ref array);
							arrayList5.Add(wgUdpComm.getXidOfCommand(array4));
							arrayList2.Add(icController.ControllerSN);
							Thread.Sleep(5);
						}
					}
				}
				wgTools.WriteLine("发送全局反潜指令完成");
				dateTime = DateTime.Now.AddSeconds(2.0);
				while (arrayList4.Count != this.arrSelectedControllers.Count)
				{
					do
					{
						array = wgUdpComm.GetPacket();
						if (array != null)
						{
							num++;
							int num5 = arrayList5.IndexOf(wgUdpComm.getXidOfCommand(array));
							if (num5 >= 0 && arrayList4.IndexOf(arrayList2[num5]) < 0)
							{
								arrayList4.Add(arrayList2[num5]);
								if (arrayList4.Count == this.arrSelectedControllers.Count)
								{
									goto Block_27;
								}
							}
							array = null;
						}
						else
						{
							Thread.Sleep(10);
						}
					}
					while (dateTime >= DateTime.Now);
					IL_0628:
					wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList4.Count.ToString());
					if (flag2)
					{
						break;
					}
					num2--;
					if (num2 <= 0)
					{
						break;
					}
					DateTime now4 = DateTime.Now;
					for (int j = 0; j < this.arrSelectedControllers.Count; j++)
					{
						if (arrayList4.IndexOf(this.arrSelectedControllers[j]) < 0)
						{
							icController.GetInfoFromDBByControllerSN((int)this.arrSelectedControllers[j]);
							wgpacketWith1152_internal.iDevSnTo = (uint)icController.ControllerSN;
							wgpacketWith1152_internal.GetNewXid();
							byte[] array5 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
							wgUdpComm.udp_get_onlySend(array5, 300, wgpacketWith1152_internal.xid, icController.IP, icController.PORT, ref array);
							arrayList5.Add(wgUdpComm.getXidOfCommand(array5));
							arrayList2.Add(icController.ControllerSN);
							Thread.Sleep(5);
						}
					}
					dateTime = DateTime.Now.AddSeconds(1.0);
					wgTools.WriteLine("GlobalAntiBackOpen_SetControllerConfigure First Send Tries =" + num2);
					if (!flag2 && num2 > 0)
					{
						continue;
					}
					break;
					Block_27:
					flag2 = true;
					goto IL_0628;
				}
			}
			wgTools.WriteLine(string.Format("rcvpktCount =" + num.ToString(), new object[0]));
			wgUdpComm.Close();
			wgUdpComm.Dispose();
			for (int j = 0; j < this.arrSelectedControllers.Count; j++)
			{
				if (arrayList4.IndexOf(this.arrSelectedControllers[j]) >= 0)
				{
					wgAppConfig.wgLog(string.Format("[{0:d}],", this.arrSelectedControllers[j]) + string.Format("{0} {1}", bGlobalAntiBack ? CommonStr.strGlobalAntiPassbackOn2015 : CommonStr.strGlobalAntiPassbackOff2015, CommonStr.strSuccessfully));
				}
				else
				{
					wgAppConfig.wgLog(string.Format("[{0:d}],", this.arrSelectedControllers[j]) + string.Format("{0} {1}", bGlobalAntiBack ? CommonStr.strGlobalAntiPassbackOn2015 : CommonStr.strGlobalAntiPassbackOff2015, CommonStr.strFailed));
				}
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000924D4 File Offset: 0x000914D4
		private int GlobalAntiBackOpen_updateLocationDT()
		{
			if (wgAppConfig.IsAccessDB)
			{
				wgAppConfig.wgLogWithoutDB(CommonStr.strGlobalAntiPassbackOnlySupportSqlServer2015, EventLogEntryType.Information, null);
				base.Invoke(new frmConsole.GlobalAntiBackOpen_updateInfo(this.addEventSpecstrGlobalAntiPassbackOnlySupportSqlServer2015));
				return 0;
			}
			int num = 0;
			num = int.Parse("0" + wgAppConfig.getSystemParamByNO(182));
			string text;
			if (num == 0)
			{
				text = "DELETE  FROM [t_b_Consumer_Location] ";
				wgAppConfig.runSql(text);
				int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
				num = swipeRecordMaxRecIdOfDB;
				if (swipeRecordMaxRecIdOfDB > 0)
				{
					wgAppConfig.setSystemParamValue(182, "AntiBack_LastQueryRecID", swipeRecordMaxRecIdOfDB.ToString(), "2015-11-13 22:49:07");
				}
			}
			text = "DELETE  FROM [t_b_Consumer_Location]  WHERE  [f_ConsumerID] NOT IN (SELECT  t_b_Consumer.[f_ConsumerID]   FROM t_b_Consumer)  ";
			wgAppConfig.runSql(text);
			text = "INSERT INTO [t_b_Consumer_Location](f_ConsumerID) SELECT  t_b_Consumer.[f_ConsumerID]   FROM t_b_Consumer WHERE  t_b_Consumer.[f_ConsumerID] NOT IN (SELECT  t_b_Consumer_Location.[f_ConsumerID]   FROM t_b_Consumer_Location) ";
			wgAppConfig.runSql(text);
			text = " SELECT [t_b_Consumer_Location].*, \r\n          [t_b_Consumer].f_ConsumerName, \r\n          [t_b_Consumer].f_CardNO,\r\n          [t_b_Consumer].f_DoorEnabled, \r\n          [t_b_Consumer].f_BeginYMD,\r\n          [t_b_Consumer].f_EndYMD,\r\n          [t_b_Consumer].f_PrivilegeTypeID,\r\n          1 as f_last_InOut,\r\n          0 as  f_NeedUpdate\r\n  FROM [t_b_Consumer],[t_b_Consumer_Location] WHERE [t_b_Consumer].f_ConsumerID = [t_b_Consumer_Location].f_ConsumerID order by [t_b_Consumer_Location].f_consumerid asc  ";
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(text, new SqlConnection(wgAppConfig.dbConString));
			this.GlobalAntiBackOpen_ds.Clear();
			sqlDataAdapter.Fill(this.GlobalAntiBackOpen_ds, "t_b_Consumer_Location");
			this.dtGlobalAntiBackOpen_ConsumerLocation = this.GlobalAntiBackOpen_ds.Tables["t_b_Consumer_Location"];
			string systemParamNotes = wgAppConfig.getSystemParamNotes(183);
			text = "select r.f_ReaderID, r.f_ReaderNO, d.f_DoorID, c.f_ControllerSN, d.f_DoorNO \r\nfrom t_b_Reader r,t_b_Door d,t_b_Controller c\r\nwhere r.f_ControllerID = c.f_ControllerID\r\n   and ((c.f_ControllerSN>400000000 and r.f_ReaderNO=d.f_DoorNO and r.f_ControllerID = d.f_ControllerID ) \r\n   or (c.f_ControllerSN <400000000 and ((r.f_ReaderNO = (d.f_DoorNO*2)) or (r.f_ReaderNO = (d.f_DoorNO*2-1))) and r.f_ControllerID = d.f_ControllerID ))";
			if (!string.IsNullOrEmpty(systemParamNotes))
			{
				text = string.Format("{0} and (d.f_DoorID IN ({1}))", text, systemParamNotes);
			}
			else
			{
				wgAppConfig.setSystemParamValue(183, "AntiBack_Door4Exit", "", "");
				text = string.Format("{0} and (1<0)", text);
			}
			new SqlDataAdapter(string.Format("{0} {1}", text, " order by r.f_ReaderID "), new SqlConnection(wgAppConfig.dbConString)).Fill(this.GlobalAntiBackOpen_ds, "t_b_Readers4Exit");
			this.dtGlobalAntiBackOpen_Readers4Exit = this.GlobalAntiBackOpen_ds.Tables["t_b_Readers4Exit"];
			this.GlobalAntiBackOpen_bFirstInThenOut = wgAppConfig.getParamValBoolByNO(184);
			this.checkLimitedPersonsFlag = 0;
			int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamNotes(213)), out this.checkLimitedPersonsFlag);
			this.GlobalAntiBackOpen_bPersonsInside = this.checkLimitedPersonsFlag > 0;
			text = "  SELECT b.f_PrivilegeTypeID , a.f_PrivilegeRecID,  b.[f_PrivilegeTypeName],  a.f_DoorID, a.f_ControllerID, a.f_DoorNO, a.f_ControlSegID , c.f_ControllerSN ,c.f_Enabled \r\n  FROM t_d_Privilege_Of_PrivilegeType a,t_d_PrivilegeType  b, t_b_Controller c\r\n   WHERE a.f_ConsumerID=  b.f_PrivilegeTypeID  \r\n    and a.f_ControllerID = c.f_ControllerID ";
			sqlDataAdapter = new SqlDataAdapter(text, new SqlConnection(wgAppConfig.dbConString));
			sqlDataAdapter.Fill(this.GlobalAntiBackOpen_ds, "t_d_PrivilegeType");
			this.dtGlobalAntiBackOpen_PrivilegeType = this.GlobalAntiBackOpen_ds.Tables["t_d_PrivilegeType"];
			this.dvGlobalAntiBackOpen_PrivilegeType = new DataView(this.dtGlobalAntiBackOpen_PrivilegeType);
			sqlDataAdapter.Fill(this.GlobalAntiBackOpen_ds, "t_b_ControlSeg");
			this.dtGlobalAntiBackOpen_ControlTimeSeg = this.GlobalAntiBackOpen_ds.Tables["t_b_ControlSeg"];
			this.dvGlobalAntiBackOpen_ControlTimeSeg = new DataView(this.dtGlobalAntiBackOpen_ControlTimeSeg);
			DataView dataView = new DataView(this.dtGlobalAntiBackOpen_Readers4Exit);
			if (dataView.Count <= 0)
			{
				wgAppConfig.wgLogWithoutDB(CommonStr.strGlobalAntiPassbackSelectDoorAsExit2015, EventLogEntryType.Information, null);
				base.Invoke(new frmConsole.GlobalAntiBackOpen_updateInfo(this.addEventSpecstrGlobalAntiPassbackSelectDoorAsExit2015));
				return 0;
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < dataView.Count; i++)
			{
				arrayList.Add((int)dataView[i]["f_ReaderID"]);
			}
			text = string.Format("SELECT [f_RecID],[f_ConsumerID] ,[f_ReadDate],[f_CardNO] ,[f_Character],[f_ControllerSN],[f_ReaderID],[f_ReaderNO] FROM t_d_SwipeRecord WHERE  f_RecID >{0} and  f_Character >0 And f_ConsumerID > 0", num);
			int num2 = 0;
			int num3 = 0;
			int count = this.dtGlobalAntiBackOpen_ConsumerLocation.Rows.Count;
			int num4 = (int)this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[0]["f_ConsumerID"];
			int num5 = (int)this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[count - 1]["f_ConsumerID"];
			int count2 = dataView.Count;
			wgTools.WriteLine("updateLocation Start");
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					sqlCommand.CommandTimeout = 180;
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						num3++;
						if (num2 == 0)
						{
							num2 = (int)sqlDataReader["f_RecID"];
						}
						int num6 = this.GlobalAntiBackOpen_findConsumerIDHalf((int)sqlDataReader["f_ConsumerID"]);
						if (num6 >= 0)
						{
							DataRow dataRow = this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[num6];
							bool flag = false;
							if ((int)dataRow["f_lastSwipe_RecID"] == 0)
							{
								flag = true;
							}
							else if ((DateTime)sqlDataReader["f_ReadDate"] > (DateTime)dataRow["f_lastSwipe_ReadDate"])
							{
								flag = true;
							}
							if (flag && (DateTime)sqlDataReader["f_ReadDate"] <= DateTime.Now.AddDays(2.0))
							{
								dataRow["f_lastSwipe_CardNO"] = sqlDataReader["f_CardNO"];
								dataRow["f_lastSwipe_RecID"] = sqlDataReader["f_RecID"];
								dataRow["f_lastSwipe_ReadDate"] = sqlDataReader["f_ReadDate"];
								dataRow["f_lastSwipe_ReaderID"] = (byte)((int)sqlDataReader["f_ReaderID"] & 255);
								dataRow["f_lastSwipe_ControllerSN"] = sqlDataReader["f_ControllerSN"];
								dataRow["f_lastSwipe_InOut"] = 1;
								if (arrayList.IndexOf((int)sqlDataReader["f_ReaderID"]) >= 0)
								{
									dataRow["f_lastSwipe_InOut"] = 0;
								}
								dataRow["f_last_InOut"] = dataRow["f_lastSwipe_InOut"];
								dataRow["f_NeedUpdate"] = 1;
							}
						}
					}
					sqlDataReader.Close();
				}
			}
			wgTools.WriteLine("updateLocation End");
			using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand2 = new SqlCommand())
				{
					sqlCommand2.Connection = sqlConnection2;
					sqlConnection2.Open();
					for (int j = 0; j < this.dtGlobalAntiBackOpen_ConsumerLocation.Rows.Count; j++)
					{
						if ((int)this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_NeedUpdate"] > 0)
						{
							text = string.Format("UPDATE t_b_Consumer_Location SET f_lastSwipe_CardNO ={0}, f_lastSwipe_RecID={1}, f_lastSwipe_ReadDate ={2}, f_lastSwipe_ReaderID={3}, f_lastSwipe_ControllerSN={4},f_lastSwipe_InOut={5}, [f_Last_InOut]={6}  ", new object[]
							{
								this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_lastSwipe_CardNO"],
								this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_lastSwipe_RecID"],
								wgTools.PrepareStr(this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_lastSwipe_ReadDate"]),
								this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_lastSwipe_ReaderID"],
								this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_lastSwipe_ControllerSN"],
								this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_lastSwipe_InOut"],
								this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_last_InOut"]
							});
							text = string.Format("{0} WHERE f_ConsumerID = {1}", text, this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_ConsumerID"]);
							sqlCommand2.CommandText = text;
							sqlCommand2.ExecuteNonQuery();
							this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_NeedUpdate"] = 0;
						}
						if (this.GlobalAntiBackOpen_bPersonsInside && (byte)this.dtGlobalAntiBackOpen_ConsumerLocation.Rows[j]["f_lastSwipe_InOut"] == 1)
						{
							this.GlobalAntiBackOpen_PersonsInside++;
						}
					}
				}
			}
			if (num2 > num)
			{
				wgAppConfig.setSystemParamValue(182, "AntiBack_LastQueryRecID", num2.ToString(), "2015-11-13 22:49:07");
			}
			return 1;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00092D18 File Offset: 0x00091D18
		private bool checkTimeSegment(int inout, long ucardno)
		{
			if (this.checkTimeSegmentFlag != 0)
			{
				if (this.checkTimeSegmentFlag == -1)
				{
					this.checkTimeSegmentFlag = 0;
					string systemParamNotes = wgAppConfig.getSystemParamNotes(212);
					if (string.IsNullOrEmpty(systemParamNotes))
					{
						return true;
					}
					string[] array = systemParamNotes.Split(new char[] { ',' });
					for (int i = 0; i < 6; i++)
					{
						int num = i * 4;
						if ((!array[num].Equals("00:00") || !array[num + 1].Equals("23:59")) && int.Parse(array[num].Replace(":", "")) <= int.Parse(array[num + 1].Replace(":", "")))
						{
							this.arrInBegin.Add(int.Parse(array[num].Replace(":", "")));
							this.arrInEnd.Add(int.Parse(array[num + 1].Replace(":", "")));
						}
						if ((!array[num + 2].Equals("00:00") || !array[num + 3].Equals("23:59")) && int.Parse(array[num + 2].Replace(":", "")) <= int.Parse(array[num + 3].Replace(":", "")))
						{
							this.arrOutBegin.Add(int.Parse(array[num + 2].Replace(":", "")));
							this.arrOutEnd.Add(int.Parse(array[num + 3].Replace(":", "")));
						}
					}
					if (this.arrInBegin.Count <= 0 && this.arrOutBegin.Count <= 0)
					{
						return true;
					}
					this.checkTimeSegmentFlag = 1;
				}
				if (this.checkTimeSegmentFlag > 0)
				{
					int num2 = int.Parse(DateTime.Now.ToString("HHmm"));
					bool flag;
					if (inout == 0)
					{
						for (int j = 0; j < this.arrOutBegin.Count; j++)
						{
							if ((int)this.arrOutBegin[j] <= num2 && (int)this.arrOutEnd[j] >= num2)
							{
								return true;
							}
						}
						flag = false;
					}
					else
					{
						for (int k = 0; k < this.arrInBegin.Count; k++)
						{
							if ((int)this.arrInBegin[k] <= num2 && (int)this.arrInEnd[k] >= num2)
							{
								return true;
							}
						}
						flag = false;
					}
					if (!flag)
					{
						base.Invoke(new frmConsole.GlobalAntiBackOpen_updateInfoWithInfo(this.addEventSpecstrGlobalAntiPassbackWithInfo), new object[] { string.Format("{0}--{1}", CommonStr.strGlobalAntiPassbackInvalidTimeSegment, ucardno) });
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00093004 File Offset: 0x00092004
		private bool checkLimitedPersons(int inout, long ucardno)
		{
			if (this.checkLimitedPersonsFlag != 0)
			{
				if (this.checkLimitedPersonsFlag == -1)
				{
					this.checkLimitedPersonsFlag = 0;
					int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamNotes(213)), out this.checkLimitedPersonsFlag);
				}
				if (this.checkLimitedPersonsFlag <= 0)
				{
					return true;
				}
				if (inout == 0)
				{
					return true;
				}
				if (this.GlobalAntiBackOpen_PersonsInside >= this.checkLimitedPersonsFlag)
				{
					base.Invoke(new frmConsole.GlobalAntiBackOpen_updateInfoWithInfo(this.addEventSpecstrGlobalAntiPassbackWithInfo), new object[] { string.Format("{0}--{1}", CommonStr.strGlobalAntiPassbackLimitedPersons + string.Format("[{0}>={1}]", this.GlobalAntiBackOpen_PersonsInside, this.checkLimitedPersonsFlag), ucardno) });
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000930CC File Offset: 0x000920CC
		private void addEventSpecstrGlobalAntiPassbackOnlySupportSqlServer2015()
		{
			try
			{
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = "",
					information = CommonStr.strGlobalAntiPassbackOnlySupportSqlServer2015
				});
				this.dlgtdisplayNewestLogEntry();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00093124 File Offset: 0x00092124
		private void addEventSpecstrGlobalAntiPassbackSelectDoorAsExit2015()
		{
			try
			{
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = "",
					information = CommonStr.strGlobalAntiPassbackSelectDoorAsExit2015
				});
				this.dlgtdisplayNewestLogEntry();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0009317C File Offset: 0x0009217C
		private void addEventSpecstrGlobalAntiPassbackStart2015()
		{
			try
			{
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = "",
					information = CommonStr.strGlobalAntiPassbackStart2015
				});
				this.dlgtdisplayNewestLogEntry();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000931D4 File Offset: 0x000921D4
		private void addEventSpecstrGlobalAntiPassbackStop2015()
		{
			try
			{
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = "",
					information = CommonStr.strGlobalAntiPassbackStop2015
				});
				this.dlgtdisplayNewestLogEntry();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0009322C File Offset: 0x0009222C
		private void addEventSpecstrGlobalAntiPassbackWithInfo(string info)
		{
			try
			{
				wgRunInfoLog.addEvent(new InfoRow
				{
					desc = "",
					information = info
				});
				this.dlgtdisplayNewestLogEntry();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00093280 File Offset: 0x00092280
		private void loadGroupLimitData()
		{
			try
			{
				string text = "SELECT f_Value from t_b_MealSetup WHERE f_ID = 8";
				if (wgTools.SetObjToStr(wgAppConfig.getValStringBySql(text)).Equals("1"))
				{
					string text2 = " SELECT a.f_GroupID,a.f_GroupName,b.f_Enabled, b.f_Morning,b.f_Lunch, b.f_Evening,b.f_Other,0 as f_MorningCount,0 as f_LunchCount,0 as f_EveningCount,0 as f_OtherCount from t_b_Group a , t_b_group4MealLimit b where a.f_GroupID = b.f_GroupID and b.f_Enabled>0 order by f_GroupName  + '\\' ASC";
					if (wgAppConfig.IsAccessDB)
					{
						using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
						{
							using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
							{
								using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
								{
									oleDbDataAdapter.Fill(this.dsgroup4MealLimit, "groups");
								}
							}
							goto IL_00DD;
						}
					}
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
						{
							using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
							{
								sqlDataAdapter.Fill(this.dsgroup4MealLimit, "groups");
							}
						}
					}
					IL_00DD:
					this.dvgroup4MealLimit = new DataView(this.dsgroup4MealLimit.Tables["groups"]);
					if (this.dvgroup4MealLimit.Count > 0)
					{
						this.bActivegroup4MealLimit = true;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00093460 File Offset: 0x00092460
		private bool allowGroupLimit(string groupname, int mealid)
		{
			bool flag = true;
			if (this.bActivegroup4MealLimit && !string.IsNullOrEmpty(groupname))
			{
				try
				{
					this.dvgroup4MealLimit.RowFilter = "f_GroupName = " + wgTools.PrepareStr(groupname);
					if (this.dvgroup4MealLimit.Count > 0)
					{
						switch (mealid)
						{
						case 1:
							if (int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_Morning"])) <= int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_MorningCount"])))
							{
								flag = false;
							}
							break;
						case 2:
							if (int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_Lunch"])) <= int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_LunchCount"])))
							{
								flag = false;
							}
							break;
						case 3:
							if (int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_Evening"])) <= int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_EveningCount"])))
							{
								flag = false;
							}
							break;
						case 4:
							if (int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_Other"])) <= int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_OtherCount"])))
							{
								flag = false;
							}
							break;
						}
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			return flag;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00093620 File Offset: 0x00092620
		private bool clearGroupLimit()
		{
			if (this.bActivegroup4MealLimit)
			{
				try
				{
					this.dvgroup4MealLimit.RowFilter = "";
					for (int i = 0; i < this.dvgroup4MealLimit.Count; i++)
					{
						this.dvgroup4MealLimit[i]["f_MorningCount"] = 0;
						this.dvgroup4MealLimit[i]["f_LunchCount"] = 0;
						this.dvgroup4MealLimit[i]["f_EveningCount"] = 0;
						this.dvgroup4MealLimit[i]["f_OtherCount"] = 0;
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			return false;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000936F4 File Offset: 0x000926F4
		private bool increaseGroupLimit(string groupname, int mealid)
		{
			if (this.bActivegroup4MealLimit)
			{
				try
				{
					this.dvgroup4MealLimit.RowFilter = "f_GroupName = " + wgTools.PrepareStr(groupname);
					if (this.dvgroup4MealLimit.Count > 0)
					{
						switch (mealid)
						{
						case 1:
							this.dvgroup4MealLimit[0]["f_MorningCount"] = int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_MorningCount"])) + 1;
							return true;
						case 2:
							this.dvgroup4MealLimit[0]["f_LunchCount"] = int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_LunchCount"])) + 1;
							return true;
						case 3:
							this.dvgroup4MealLimit[0]["f_EveningCount"] = int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_EveningCount"])) + 1;
							return true;
						case 4:
							this.dvgroup4MealLimit[0]["f_OtherCount"] = int.Parse(wgTools.SetObjToStr(this.dvgroup4MealLimit[0]["f_OtherCount"])) + 1;
							return true;
						}
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00093890 File Offset: 0x00092890
		private void _dataTableControlHolidayLoad()
		{
			try
			{
				string text = "Select count(*) from t_b_ControlHolidays";
				if (wgAppConfig.runSql(text) != 0)
				{
					text = "SELECT f_Id, f_BeginYMDHMS, f_EndYMDHMS, f_Notes,f_forcework From t_b_ControlHolidays ";
					DataSet dataSet = new DataSet();
					using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
						{
							using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
							{
								dataAdapter.Fill(dataSet);
							}
						}
					}
					this.dtControlHoliday = dataSet.Tables[0];
					this.dvHolidays = new DataView(this.dtControlHoliday);
					this.dvHolidays.RowFilter = " f_forcework = 0";
					this.dvNeedWork = new DataView(this.dtControlHoliday);
					this.dvNeedWork.RowFilter = " f_forcework = 1";
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00093A1C File Offset: 0x00092A1C
		private int isValidTimePoint()
		{
			if (this.dtControlHoliday == null)
			{
				return 1;
			}
			DateTime now = DateTime.Now;
			if (this.dtDealt.Subtract(now).TotalSeconds != 0.0)
			{
				int num = 1;
				if (num > 0 && this.dvHolidays.Count > 0)
				{
					for (int i = 0; i < this.dvHolidays.Count; i++)
					{
						if ((DateTime)this.dvHolidays[i]["f_BeginYMDHMS"] <= now && now <= (DateTime)this.dvHolidays[i]["f_EndYMDHMS"])
						{
							num = 0;
							break;
						}
					}
				}
				if (this.dvNeedWork.Count > 0)
				{
					for (int j = 0; j < this.dvNeedWork.Count; j++)
					{
						if ((DateTime)this.dvNeedWork[j]["f_BeginYMDHMS"] <= now && now <= (DateTime)this.dvNeedWork[j]["f_EndYMDHMS"])
						{
							num = 2;
							break;
						}
					}
				}
				this.retDealtLast = num;
			}
			return this.retDealtLast;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00093B4C File Offset: 0x00092B4C
		private void pcCheckMealOpen_DealNewRecord()
		{
			if (!wgAppConfig.IsAccessControlBlue && this.bPCCheckMealOpen)
			{
				try
				{
					this._dataTableControlHolidayLoad();
					this.qMjRec4MealOpen.Clear();
					this.arrMealRecord.Clear();
					this.arrMealRecordDay.Clear();
					this.loadGroupLimitData();
					DateTime now = DateTime.Now;
					ArrayList arrayList = new ArrayList();
					ArrayList arrayList2 = new ArrayList();
					ArrayList arrayList3 = new ArrayList();
					try
					{
						DbConnection dbConnection;
						DbCommand dbCommand;
						DbDataAdapter dbDataAdapter;
						if (wgAppConfig.IsAccessDB)
						{
							dbConnection = new OleDbConnection(wgAppConfig.dbConString);
							dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
							dbDataAdapter = new OleDbDataAdapter();
						}
						else
						{
							dbConnection = new SqlConnection(wgAppConfig.dbConString);
							dbCommand = new SqlCommand("", dbConnection as SqlConnection);
							dbDataAdapter = new SqlDataAdapter();
						}
						string text = "  SELECT  t_b_Reader.f_ReaderID , t_b_Reader.[f_ReaderName], t_b_Reader.f_ReaderNO, t_b_Controller.f_ControllerSN, t_b_Controller.f_ControllerID  ";
						text += " FROM  t_b_Reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID  ORDER BY  t_b_Reader.f_ReaderID  ";
						dbCommand.CommandText = text;
						DataSet dataSet = new DataSet();
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataSet, "Reader4Meal");
						DataView dataView = new DataView(dataSet.Tables["Reader4Meal"]);
						ArrayList arrayList4 = new ArrayList();
						string text2 = "0";
						for (int i = 0; i < dataView.Count; i++)
						{
							if (this.arrSelectedControllers.IndexOf((int)dataView[i]["f_ControllerSN"]) >= 0 && arrayList4.IndexOf((int)dataView[i]["f_ControllerID"]) < 0)
							{
								arrayList4.Add((int)dataView[i]["f_ControllerID"]);
								text2 = text2 + "," + dataView[i]["f_ControllerID"].ToString();
							}
						}
						if (arrayList4.Count > 0)
						{
							text = "  SELECT DISTINCT t_d_privilege.f_ConsumerID, [f_ControlSegID]  ";
							text = text + " FROM  t_d_privilege  WHERE  " + string.Format("  t_d_privilege.f_ControllerID IN ({0})", text2);
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataSet, "Privilege");
							DataView dataView2 = new DataView(dataSet.Tables["Privilege"]);
							text = " SELECT * FROM t_b_ControlSeg  ";
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataSet, "ControlSeg");
							DataView dataView3 = new DataView(dataSet.Tables["ControlSeg"]);
							if (dataView2.Count > 0)
							{
								dbCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID = 5 or  f_ID = 2  or  f_ID = 3 or  f_ID = 4 ORDER BY f_ID ASC";
								dbConnection.Open();
								DbDataReader dbDataReader = dbCommand.ExecuteReader();
								while (dbDataReader.Read())
								{
									if ((int)dbDataReader["f_Value"] > 0)
									{
										DateTime dateTime = (DateTime)dbDataReader["f_BeginHMS"];
										dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
										DateTime dateTime2 = (DateTime)dbDataReader["f_EndHMS"];
										dateTime2 = dateTime2.AddSeconds((double)(59 - dateTime2.Second));
										arrayList.Add(dateTime);
										arrayList2.Add(dateTime2);
										arrayList3.Add((int)dbDataReader["f_ID"]);
									}
								}
								dbConnection.Close();
								if (arrayList.Count > 0)
								{
									int num = 4;
									if (int.Parse("0" + wgAppConfig.getSystemParamByNO(169)) > 0)
									{
										num = int.Parse("0" + wgAppConfig.getSystemParamByNO(169));
									}
									bool flag = false;
									IL_12F0:
									while (this.bPCCheckMealOpen && !this.bMealStop)
									{
										bool flag2 = false;
										bool flag3 = false;
										DateTime dateTime3 = DateTime.Now;
										DateTime dateTime4 = DateTime.Now;
										DateTime dateTime5 = DateTime.Now;
										DateTime dateTime6 = DateTime.Now;
										DateTime dateTime7 = DateTime.Now;
										DateTime dateTime8 = DateTime.Now;
										int num2 = 0;
										for (int j = 0; j < arrayList.Count; j++)
										{
											dateTime4 = (DateTime)arrayList[j];
											dateTime5 = (DateTime)arrayList2[j];
											dateTime4 = dateTime4.AddSeconds((double)(-(double)dateTime4.Second));
											dateTime5 = dateTime5.AddSeconds((double)(59 - dateTime5.Second));
											if (j == 0)
											{
												dateTime8 = DateTime.Parse(dateTime3.ToString("yyyy-MM-dd ") + dateTime4.ToString("HH:mm:ss"));
											}
											if (string.Compare(dateTime4.ToString("HH:mm"), dateTime5.ToString("HH:mm")) >= 0)
											{
												if (string.Compare(dateTime4.ToString("HH:mm"), dateTime3.ToString("HH:mm")) >= 0 && string.Compare(dateTime3.ToString("HH:mm"), dateTime5.ToString("HH:mm")) <= 0)
												{
													flag2 = true;
													dateTime6 = DateTime.Parse(dateTime3.AddDays(-1.0).ToString("yyyy-MM-dd ") + dateTime4.ToString("HH:mm:ss"));
													dateTime8 = dateTime8.AddDays(-1.0);
													dateTime7 = DateTime.Parse(dateTime3.ToString("yyyy-MM-dd ") + dateTime5.ToString("HH:mm:59"));
												}
												else if (string.Compare(dateTime4.ToString("HH:mm"), dateTime3.ToString("HH:mm")) <= 0)
												{
													flag2 = true;
													dateTime6 = DateTime.Parse(dateTime3.ToString("yyyy-MM-dd ") + dateTime4.ToString("HH:mm:ss"));
													dateTime7 = DateTime.Parse(dateTime3.AddDays(1.0).ToString("yyyy-MM-dd ") + dateTime5.ToString("HH:mm:59"));
												}
											}
											else if (string.Compare(dateTime4.ToString("HH:mm"), dateTime3.ToString("HH:mm")) <= 0 && string.Compare(dateTime3.ToString("HH:mm"), dateTime5.ToString("HH:mm")) <= 0)
											{
												flag2 = true;
												dateTime6 = DateTime.Parse(dateTime3.ToString("yyyy-MM-dd ") + dateTime4.ToString("HH:mm:ss"));
												dateTime7 = DateTime.Parse(dateTime3.ToString("yyyy-MM-dd ") + dateTime5.ToString("HH:mm:59"));
											}
											if (flag2)
											{
												if ((int)arrayList3[j] == 5)
												{
													flag3 = true;
												}
												num2 = (int)arrayList3[j] - 1;
												break;
											}
										}
										if (!flag2)
										{
											Thread.Sleep(30000);
										}
										else
										{
											string text3 = "";
											using (new StreamWriter(Application.StartupPath + "\\n3k_meal.log", true))
											{
											}
											using (new StreamWriter(Application.StartupPath + "\\n3k_mealDay.log", true))
											{
											}
											using (StreamReader streamReader = new StreamReader(Application.StartupPath + "\\n3k_meal.log"))
											{
												text3 = streamReader.ReadLine();
												if (!string.IsNullOrEmpty(text3))
												{
													if (text3 == dateTime6.ToString("yyyy-MM-dd HH:mm:ss"))
													{
														string text4;
														do
														{
															text4 = streamReader.ReadLine();
															if (!string.IsNullOrEmpty(text4))
															{
																ulong num3 = ulong.Parse(text4.Split(new char[] { ',' })[0]);
																if (this.arrMealRecord.IndexOf(num3) < 0)
																{
																	this.arrMealRecord.Add(num3);
																	if (this.bActivegroup4MealLimit)
																	{
																		string valStringBySql = wgAppConfig.getValStringBySql(" SELECT  f_GroupName  FROM t_b_Consumer INNER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE  t_b_Consumer.f_CardNO = " + text4.Split(new char[] { ',' })[0].ToString());
																		if (!string.IsNullOrEmpty(valStringBySql))
																		{
																			this.increaseGroupLimit(valStringBySql, num2);
																		}
																	}
																}
															}
														}
														while (!string.IsNullOrEmpty(text4));
													}
													else
													{
														if (string.Compare(text3, dateTime6.ToString("yyyy-MM-dd HH:mm:ss")) > 0 && string.Compare(text3, dateTime6.ToString("yyyy-MM-dd 23:59:59")) < 0)
														{
															if (!flag)
															{
																flag = true;
																XMessageBox.Show(this, CommonStr.strMealDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
															}
															Thread.Sleep(5000);
															continue;
														}
														text3 = "";
													}
												}
											}
											flag = false;
											if (string.IsNullOrEmpty(text3))
											{
												using (StreamWriter streamWriter3 = new StreamWriter(Application.StartupPath + "\\n3k_meal.log", false))
												{
													streamWriter3.WriteLine(dateTime6.ToString("yyyy-MM-dd HH:mm:ss"));
												}
											}
											using (new StreamWriter(Application.StartupPath + "\\n3k_mealDay.log", true))
											{
											}
											using (StreamReader streamReader2 = new StreamReader(Application.StartupPath + "\\n3k_mealDay.log"))
											{
												text3 = streamReader2.ReadLine();
												if (!string.IsNullOrEmpty(text3))
												{
													if (text3 == dateTime8.ToString("yyyy-MM-dd HH:mm:ss"))
													{
														string text5;
														do
														{
															text5 = streamReader2.ReadLine();
															if (!string.IsNullOrEmpty(text5))
															{
																this.arrMealRecordDay.Add(ulong.Parse(text5.Split(new char[] { ',' })[0]));
															}
														}
														while (!string.IsNullOrEmpty(text5));
													}
													else
													{
														text3 = "";
													}
												}
											}
											if (string.IsNullOrEmpty(text3))
											{
												using (StreamWriter streamWriter4 = new StreamWriter(Application.StartupPath + "\\n3k_mealDay.log", false))
												{
													streamWriter4.WriteLine(dateTime8.ToString("yyyy-MM-dd HH:mm:ss"));
												}
											}
											lock (this.qMjRec4MealOpen.SyncRoot)
											{
												this.qMjRec4MealOpen.Clear();
											}
											for (;;)
											{
												this.bMealDealing = true;
												if (this.qMjRec4MealOpen.Count <= 0)
												{
													Thread.Sleep(1);
													goto IL_1279;
												}
												string text6;
												lock (this.qMjRec4MealOpen.SyncRoot)
												{
													text6 = this.qMjRec4MealOpen.Dequeue() as string;
												}
												MjRec mjRec = new MjRec(text6);
												if (mjRec.ControllerSN > 0U)
												{
													try
													{
														if (this.arrSelectedControllers.IndexOf((int)mjRec.ControllerSN) < 0)
														{
															goto IL_12DD;
														}
													}
													catch (Exception)
													{
														goto IL_12F0;
													}
													if (!mjRec.IsSwipeRecord)
													{
														goto IL_1279;
													}
													mjRec.GetUserInfoFromDB();
													if (!(mjRec.ReadDate.Date <= mjRec.endYMD.Date) || !(mjRec.ReadDate.Date >= mjRec.beginYMD.Date) || !(mjRec.ReadDate.Date >= dateTime6) || !(mjRec.ReadDate.Date <= dateTime7))
													{
														goto IL_1279;
													}
													if (this.allowGroupLimit(mjRec.groupname, num2))
													{
														bool flag4 = false;
														bool flag5 = false;
														if (wgMjController.GetControllerType((int)mjRec.ControllerSN) == 4)
														{
															dataView.RowFilter = string.Format("f_ControllerSN ={0} and f_ReaderNO = {1} ", mjRec.ControllerSN, mjRec.ReaderNo);
														}
														else
														{
															dataView.RowFilter = string.Format("f_ControllerSN ={0} and (f_ReaderNO = {1} OR f_ReaderNO = {2} ) ", mjRec.ControllerSN, (int)(((mjRec.ReaderNo - 1) & 254) + 1), (int)(((mjRec.ReaderNo - 1) & 254) + 2));
														}
														if (dataView.Count > 0)
														{
															dataView.RowFilter = string.Format("f_ControllerSN ={0} and f_ReaderNO = {1} ", mjRec.ControllerSN, mjRec.ReaderNo);
															if (dataView.Count > 0)
															{
																flag4 = true;
															}
														}
														else
														{
															dataView.RowFilter = string.Format("f_ControllerSN ={0}  ", mjRec.ControllerSN);
															if (dataView.Count > 0)
															{
																flag5 = true;
															}
														}
														if (flag4 || flag5)
														{
															dataView2.RowFilter = string.Format(" f_ConsumerID = {0} ", mjRec.consumerID);
															if (dataView2.Count <= 0)
															{
																goto IL_1279;
															}
															bool flag6 = false;
															if (int.Parse(dataView2[0]["f_ControlSegID"].ToString()) == 1)
															{
																flag6 = true;
															}
															else if (int.Parse(dataView2[0]["f_ControlSegID"].ToString()) == 0)
															{
																flag6 = false;
															}
															else
															{
																dataView3.RowFilter = "[f_ControlSegID] =" + dataView2[0]["f_ControlSegID"].ToString();
																int num4 = this.isValidTimePoint();
																int num5 = 250;
																while (!flag6 && num5-- > 0 && dataView3.Count > 0)
																{
																	DateTime now2 = DateTime.Now;
																	if (DateTime.Parse(dataView3[0]["f_BeginYMD"].ToString()).Date <= now2.Date && DateTime.Parse(dataView3[0]["f_EndYMD"].ToString()).Date >= now2.Date)
																	{
																		int num6 = 0;
																		if (dataView3[0]["f_Monday"].ToString() == "1")
																		{
																			num6++;
																		}
																		if (dataView3[0]["f_Tuesday"].ToString() == "1")
																		{
																			num6 += 2;
																		}
																		if (dataView3[0]["f_Wednesday"].ToString() == "1")
																		{
																			num6 += 4;
																		}
																		if (dataView3[0]["f_Thursday"].ToString() == "1")
																		{
																			num6 += 8;
																		}
																		if (dataView3[0]["f_Friday"].ToString() == "1")
																		{
																			num6 += 16;
																		}
																		if (dataView3[0]["f_Saturday"].ToString() == "1")
																		{
																			num6 += 32;
																		}
																		if (dataView3[0]["f_Sunday"].ToString() == "1")
																		{
																			num6 += 64;
																		}
																		int num7 = (int)now2.DayOfWeek;
																		if (num7 == 0)
																		{
																			num7 = 7;
																		}
																		num7--;
																		if ((num6 & (1 << num7)) == 0)
																		{
																			if (dataView3[0]["f_ControlByHoliday"].ToString() == "1" && num4 == 2)
																			{
																				flag6 = true;
																				break;
																			}
																		}
																		else
																		{
																			flag6 = true;
																			if (dataView3[0]["f_ControlByHoliday"].ToString() == "1" && num4 == 0)
																			{
																				flag6 = false;
																				break;
																			}
																			break;
																		}
																	}
																	if (int.Parse(dataView3[0]["f_ControlSegIDLinked"].ToString()) > 0)
																	{
																		dataView3.RowFilter = "[f_ControlSegID] =" + dataView3[0]["f_ControlSegIDLinked"].ToString();
																	}
																}
															}
															if (!flag6)
															{
																goto IL_1279;
															}
															if (!flag4)
															{
																icController icController = new icController();
																icController.GetInfoFromDBByControllerSN((int)mjRec.ControllerSN);
																frmConsole.clsControlInfo clsControlInfo = new frmConsole.clsControlInfo();
																clsControlInfo.doorController = icController;
																clsControlInfo.DoorNo = (int)mjRec.DoorNo;
																clsControlInfo.CardID = mjRec.CardID;
																clsControlInfo.ConsumerName = mjRec.consumerName;
																clsControlInfo.bMeal = false;
																ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo);
																goto IL_1279;
															}
															if (this.arrMealRecord.IndexOf((ulong)mjRec.CardID) < 0)
															{
																int num8 = -1;
																if (num < 4)
																{
																	if (num == 3)
																	{
																		if (flag3)
																		{
																			num8 = this.arrMealRecordDay.IndexOf((ulong)mjRec.CardID);
																			if (num8 >= 0)
																			{
																				num8 = this.arrMealRecordDay.IndexOf((ulong)mjRec.CardID, num8 + 1);
																				if (num8 >= 0)
																				{
																					num8 = this.arrMealRecordDay.IndexOf((ulong)mjRec.CardID, num8 + 1);
																				}
																			}
																		}
																	}
																	else if (num == 2)
																	{
																		num8 = this.arrMealRecordDay.IndexOf((ulong)mjRec.CardID);
																		if (num8 >= 0)
																		{
																			num8 = this.arrMealRecordDay.IndexOf((ulong)mjRec.CardID, num8 + 1);
																		}
																	}
																	else if (num == 1)
																	{
																		num8 = this.arrMealRecordDay.IndexOf((ulong)mjRec.CardID);
																	}
																}
																if (num8 < 0)
																{
																	icController icController = new icController();
																	icController.GetInfoFromDBByControllerSN((int)mjRec.ControllerSN);
																	frmConsole.clsControlInfo clsControlInfo2 = new frmConsole.clsControlInfo();
																	clsControlInfo2.doorController = icController;
																	clsControlInfo2.DoorNo = (int)mjRec.DoorNo;
																	clsControlInfo2.CardID = mjRec.CardID;
																	clsControlInfo2.ConsumerName = mjRec.consumerName;
																	clsControlInfo2.bMeal = true;
																	ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo2);
																	this.increaseGroupLimit(mjRec.groupname, num2);
																	if (this.bAllowOperateExtendPort)
																	{
																		clsControlInfo2 = new frmConsole.clsControlInfo();
																		clsControlInfo2.doorController = icController;
																		clsControlInfo2.DoorNo = (int)mjRec.DoorNo;
																		clsControlInfo2.CardID = mjRec.CardID;
																		clsControlInfo2.ConsumerName = mjRec.consumerName;
																		clsControlInfo2.extendPortDelaySecond = 0;
																		clsControlInfo2.bMeal = false;
																		ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData4NotAllow), clsControlInfo2);
																		goto IL_1279;
																	}
																	goto IL_1279;
																}
																else
																{
																	if (this.bAllowOperateExtendPort)
																	{
																		icController icController = new icController();
																		icController.GetInfoFromDBByControllerSN((int)mjRec.ControllerSN);
																		frmConsole.clsControlInfo clsControlInfo3 = new frmConsole.clsControlInfo();
																		clsControlInfo3.doorController = icController;
																		clsControlInfo3.DoorNo = (int)mjRec.DoorNo;
																		clsControlInfo3.CardID = mjRec.CardID;
																		clsControlInfo3.ConsumerName = mjRec.consumerName;
																		clsControlInfo3.extendPortDelaySecond = 3;
																		clsControlInfo3.bMeal = false;
																		ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData4NotAllow), clsControlInfo3);
																		goto IL_1279;
																	}
																	goto IL_1279;
																}
															}
															else
															{
																if (this.bAllowOperateExtendPort)
																{
																	icController icController = new icController();
																	icController.GetInfoFromDBByControllerSN((int)mjRec.ControllerSN);
																	frmConsole.clsControlInfo clsControlInfo4 = new frmConsole.clsControlInfo();
																	clsControlInfo4.doorController = icController;
																	clsControlInfo4.DoorNo = (int)mjRec.DoorNo;
																	clsControlInfo4.CardID = mjRec.CardID;
																	clsControlInfo4.ConsumerName = mjRec.consumerName;
																	clsControlInfo4.extendPortDelaySecond = 3;
																	clsControlInfo4.bMeal = false;
																	ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData4NotAllow), clsControlInfo4);
																	goto IL_1279;
																}
																goto IL_1279;
															}
														}
													}
												}
												IL_12DD:
												if (!this.bPCCheckMealOpen || this.bMealStop)
												{
													goto IL_12F0;
												}
												continue;
												IL_1279:
												dateTime3 = DateTime.Now;
												if (string.Compare(dateTime3.ToString("HH:mm"), dateTime5.ToString("HH:mm")) == 0)
												{
													break;
												}
												if (dateTime3 < dateTime6 || dateTime3 > dateTime7)
												{
													goto IL_12C9;
												}
												goto IL_12DD;
											}
											this.arrMealRecord.Clear();
											this.clearGroupLimit();
											continue;
											IL_12C9:
											this.arrMealRecord.Clear();
											this.clearGroupLimit();
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						wgAppConfig.wgDebugWrite(ex.ToString());
					}
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
				}
				finally
				{
					this.bMealDealing = false;
				}
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00094FCC File Offset: 0x00093FCC
		private void pcCheckMealOpen_Init()
		{
			if (this.bPCCheckMealOpen)
			{
				ThreadPool.SetMaxThreads(10, 10);
				int num = 0;
				int.TryParse(wgAppConfig.GetKeyVal("KEY_MaxThreadNum"), out num);
				if (num > 0)
				{
					ThreadPool.SetMaxThreads(num, num);
				}
				int num2 = 0;
				int num3 = 0;
				ThreadPool.GetMaxThreads(out num2, out num3);
				wgTools.WgDebugWrite(string.Format("ThreadPool.GetMaxThreads: {0},{1}", num2, num3), new object[0]);
				this.bMealStop = false;
				this.threadpcCheckMealOpen_DealNewRecord = new Thread(new ThreadStart(this.pcCheckMealOpen_DealNewRecord));
				this.threadpcCheckMealOpen_DealNewRecord.Start();
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00095068 File Offset: 0x00094068
		private void SendData(object info)
		{
			frmConsole.clsControlInfo clsControlInfo = info as frmConsole.clsControlInfo;
			try
			{
				icController doorController = clsControlInfo.doorController;
				if (doorController.RemoteOpenDoorIPNoStartDelay(clsControlInfo.DoorNo, 0U, clsControlInfo.CardID, 0) > 0 && clsControlInfo.bMeal)
				{
					if (this.arrMealRecord.IndexOf((ulong)clsControlInfo.CardID) < 0)
					{
						this.arrMealRecord.Add((ulong)clsControlInfo.CardID);
						using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\n3k_meal.log", true))
						{
							streamWriter.WriteLine(string.Format("{0:d},\t{1},\t{2},\t{3},\t{4}", new object[]
							{
								clsControlInfo.CardID,
								clsControlInfo.ConsumerName,
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
								clsControlInfo.doorController.ControllerSN,
								clsControlInfo.DoorNo
							}));
						}
						this.arrMealRecordDay.Add((ulong)clsControlInfo.CardID);
						using (StreamWriter streamWriter2 = new StreamWriter(Application.StartupPath + "\\n3k_mealDay.log", true))
						{
							streamWriter2.WriteLine(string.Format("{0:d},\t{1},\t{2},\t{3},\t{4}", new object[]
							{
								clsControlInfo.CardID,
								clsControlInfo.ConsumerName,
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
								clsControlInfo.doorController.ControllerSN,
								clsControlInfo.DoorNo
							}));
						}
						wgAppConfig.wgLog(string.Format("{0}[{1:d}],", doorController.GetDoorName(clsControlInfo.DoorNo), doorController.ControllerSN) + string.Format("{0}{1}\t[{2}]", CommonStr.strRecordRemoteOpenDoor_ByPCMealOpen, clsControlInfo.CardID, clsControlInfo.ConsumerName));
					}
				}
				else
				{
					wgAppConfig.wgLog(string.Format("{0}[{1:d}],", doorController.GetDoorName(clsControlInfo.DoorNo), doorController.ControllerSN) + string.Format("{0}{1}\t[{2}]", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK), clsControlInfo.CardID, clsControlInfo.ConsumerName));
				}
				doorController.Dispose();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00095318 File Offset: 0x00094318
		private void SendData4NotAllow(object info)
		{
			frmConsole.clsControlInfo clsControlInfo = info as frmConsole.clsControlInfo;
			try
			{
				icController doorController = clsControlInfo.doorController;
				if (doorController.RemoteOpenExtboardIPNoStartDelay(clsControlInfo.DoorNo | 192, 0U, clsControlInfo.CardID, 0, clsControlInfo.extendPortDelaySecond) > 0 && clsControlInfo.bMeal)
				{
					if (this.arrMealRecord.IndexOf((ulong)clsControlInfo.CardID) < 0)
					{
						this.arrMealRecord.Add((ulong)clsControlInfo.CardID);
						using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\n3k_meal.log", true))
						{
							streamWriter.WriteLine(string.Format("{0:d},\t{1},\t{2},\t{3},\t{4}", new object[]
							{
								clsControlInfo.CardID,
								clsControlInfo.ConsumerName,
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
								clsControlInfo.doorController.ControllerSN,
								clsControlInfo.DoorNo
							}));
						}
						this.arrMealRecordDay.Add((ulong)clsControlInfo.CardID);
						using (StreamWriter streamWriter2 = new StreamWriter(Application.StartupPath + "\\n3k_mealDay.log", true))
						{
							streamWriter2.WriteLine(string.Format("{0:d},\t{1},\t{2},\t{3},\t{4}", new object[]
							{
								clsControlInfo.CardID,
								clsControlInfo.ConsumerName,
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
								clsControlInfo.doorController.ControllerSN,
								clsControlInfo.DoorNo
							}));
						}
						wgAppConfig.wgLog(string.Format("{0}[{1:d}],", doorController.GetDoorName(clsControlInfo.DoorNo), doorController.ControllerSN) + string.Format("{0}{1}\t[{2}]", CommonStr.strRecordRemoteOpenDoor_ByPCMealOpen, clsControlInfo.CardID, clsControlInfo.ConsumerName));
					}
				}
				else
				{
					wgAppConfig.wgLog(string.Format("{0}[{1:d}],", doorController.GetDoorName(clsControlInfo.DoorNo), doorController.ControllerSN) + string.Format("{0}{1}\t[{2}]", CommonStr.strRemoteOpenExtendPort_ByPCMealOpen, clsControlInfo.CardID, clsControlInfo.ConsumerName));
				}
				doorController.Dispose();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000955D0 File Offset: 0x000945D0
		private void pcCheckAccess_Init()
		{
			if (this.bPCCheckAccess)
			{
				try
				{
					string systemParamNotes = wgAppConfig.getSystemParamNotes(202);
					if (!string.IsNullOrEmpty(systemParamNotes))
					{
						this.dtPCCheckDoorMoreCards = DatatableToJson.JsonToDataTable(systemParamNotes);
						this.dvPCCheckDoorMoreCards = new DataView(this.dtPCCheckDoorMoreCards);
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				try
				{
					string text = "";
					try
					{
						string text2 = " SELECT a.f_GroupID,a.f_GroupName,b.f_GroupType,b.f_MoreCards,b.f_SoundFileName  from t_b_Group a, t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and b.f_GroupType=1 order by  f_GroupName  + '\\' ASC";
						if (wgAppConfig.IsAccessDB)
						{
							using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
							{
								using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
								{
									oleDbConnection.Open();
									OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
									if (oleDbDataReader.Read())
									{
										text = wgTools.SetObjToStr(oleDbDataReader["f_SoundFileName"]);
										if (wgTools.SetObjToStr(oleDbDataReader["f_MoreCards"]) == "0")
										{
											this.bNeedAllGroups = true;
										}
									}
									oleDbDataReader.Close();
								}
								goto IL_0174;
							}
						}
						using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
						{
							using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
							{
								sqlConnection.Open();
								SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
								if (sqlDataReader.Read())
								{
									text = wgTools.SetObjToStr(sqlDataReader["f_SoundFileName"]);
									if (wgTools.SetObjToStr(sqlDataReader["f_MoreCards"]) == "0")
									{
										this.bNeedAllGroups = true;
									}
								}
								sqlDataReader.Close();
							}
						}
						IL_0174:;
					}
					catch (Exception ex2)
					{
						wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
					}
					using (DataView dataView = new DataView(this.dvDoors.Table))
					{
						if (string.IsNullOrEmpty(text))
						{
							for (int i = 0; i <= dataView.Count - 1; i++)
							{
								this.checkAccess_arrDoor.Add(dataView[i]["f_DoorID"]);
								this.checkAccess_arrDoorName.Add(dataView[i]["f_DoorName"]);
								this.checkAccess_arrReaderNo.Add("");
								this.checkAccess_arrGroupName.Add("");
								this.checkAccess_arrCardId.Add("");
								this.checkAccess_arrConsumerName.Add("");
								this.checkAccess_arrCheckTime.Add("");
								this.checkAccess_arrCheckStartTime.Add(DateTime.Now);
								this.checkAccess_arrCount.Add(-1);
							}
						}
						else
						{
							string[] array = text.Split(new char[] { ',' });
							for (int j = 0; j <= dataView.Count - 1; j++)
							{
								for (int k = 0; k < array.Length; k++)
								{
									if (int.Parse(array[k]) == (int)dataView[j]["f_DoorID"])
									{
										this.checkAccess_arrDoor.Add(dataView[j]["f_DoorID"]);
										this.checkAccess_arrDoorName.Add(dataView[j]["f_DoorName"]);
										this.checkAccess_arrReaderNo.Add("");
										this.checkAccess_arrGroupName.Add("");
										this.checkAccess_arrCardId.Add("");
										this.checkAccess_arrConsumerName.Add("");
										this.checkAccess_arrCheckTime.Add("");
										this.checkAccess_arrCheckStartTime.Add(DateTime.Now);
										this.checkAccess_arrCount.Add(-1);
									}
								}
							}
						}
					}
					try
					{
						string text3 = " SELECT a.f_GroupID,a.f_GroupName,b.f_GroupType,b.f_MoreCards  from t_b_Group a, t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and b.f_CheckAccessActive = 1 and b.f_GroupType=0 order by f_GroupName  + '\\' ASC";
						if (wgAppConfig.IsAccessDB)
						{
							using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
							{
								using (OleDbCommand oleDbCommand2 = new OleDbCommand(text3, oleDbConnection2))
								{
									oleDbConnection2.Open();
									OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
									while (oleDbDataReader2.Read())
									{
										this.checkAccess_arrDB_GroupName.Add(oleDbDataReader2["f_GroupName"]);
										this.checkAccess_arrDB_MoreCards.Add(oleDbDataReader2["f_MoreCards"]);
									}
									oleDbDataReader2.Close();
								}
								return;
							}
						}
						using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
						{
							using (SqlCommand sqlCommand2 = new SqlCommand(text3, sqlConnection2))
							{
								sqlConnection2.Open();
								SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader();
								while (sqlDataReader2.Read())
								{
									this.checkAccess_arrDB_GroupName.Add(sqlDataReader2["f_GroupName"]);
									this.checkAccess_arrDB_MoreCards.Add(sqlDataReader2["f_MoreCards"]);
								}
								sqlDataReader2.Close();
							}
						}
					}
					catch (Exception ex3)
					{
						wgTools.WgDebugWrite(ex3.ToString(), new object[] { EventLogEntryType.Error });
					}
				}
				catch (Exception ex4)
				{
					wgTools.WgDebugWrite(ex4.ToString(), new object[] { EventLogEntryType.Error });
				}
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00095C58 File Offset: 0x00094C58
		private void pcCheckAccess_DealNewRecord(MjRec mjrec)
		{
			if (this.bPCCheckAccess)
			{
				if (!this.bNeedAllGroups)
				{
					mjrec.GetUserInfoFromDB();
					if (string.IsNullOrEmpty(mjrec.groupname))
					{
						return;
					}
					try
					{
						if (mjrec.ReadDate.Date <= mjrec.endYMD.Date && mjrec.ReadDate.Date >= mjrec.beginYMD.Date)
						{
							int num = -1;
							this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjrec.ControllerSN.ToString(), mjrec.DoorNo.ToString());
							if (this.dvDoors4Watching.Count > 0)
							{
								num = this.checkAccess_arrDoorName.IndexOf(this.dvDoors4Watching[0]["f_DoorName"].ToString());
							}
							if (num >= 0)
							{
								if (this.checkAccess_arrDB_GroupName.IndexOf(mjrec.groupname) >= 0)
								{
									DateTime dateTime = (DateTime)this.checkAccess_arrCheckStartTime[num];
									if (mjrec.ReadDate > dateTime.AddSeconds(20.0) || mjrec.ReadDate.AddSeconds(20.0) < (DateTime)this.checkAccess_arrCheckStartTime[num])
									{
										this.checkAccess_arrCount[num] = 0;
									}
									if ((int)this.checkAccess_arrCount[num] > 0 && (byte)this.checkAccess_arrReaderNo[num] == mjrec.ReaderNo && (string)this.checkAccess_arrGroupName[num] == mjrec.groupname)
									{
										if (this.checkAccess_arrCardId[num].ToString().IndexOf(mjrec.CardID.ToString().PadLeft(10, '0')) < 0)
										{
											ArrayList arrayList;
											int num2;
											(arrayList = this.checkAccess_arrCardId)[num2 = num] = arrayList[num2] + "," + mjrec.CardID.ToString().PadLeft(10, '0');
											ArrayList arrayList2;
											int num3;
											(arrayList2 = this.checkAccess_arrConsumerName)[num3 = num] = arrayList2[num3] + "\r\n" + mjrec.consumerName;
											this.checkAccess_arrCheckStartTime[num] = mjrec.ReadDate;
											this.checkAccess_arrCount[num] = (int)this.checkAccess_arrCount[num] + 1;
										}
									}
									else
									{
										this.checkAccess_arrReaderNo[num] = mjrec.ReaderNo;
										this.checkAccess_arrGroupName[num] = mjrec.groupname;
										this.checkAccess_arrCardId[num] = mjrec.CardID.ToString().PadLeft(10, '0');
										this.checkAccess_arrConsumerName[num] = mjrec.consumerName;
										this.checkAccess_arrCheckStartTime[num] = mjrec.ReadDate;
										this.checkAccess_arrCount[num] = 1;
									}
									if (base.IsHandleCreated)
									{
										this.mjrecNewest = null;
										MethodInvoker methodInvoker = new MethodInvoker(this.pcCheckAccess_DealOpen);
										base.BeginInvoke(methodInvoker);
									}
								}
								else
								{
									this.mjrecNewest = mjrec;
									this.mjrecNewestDoorName = this.dvDoors4Watching[0]["f_DoorName"].ToString();
								}
							}
						}
						return;
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
						return;
					}
				}
				this.pcCheckAccess_DealNewRecord4AllSame(mjrec);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00096014 File Offset: 0x00095014
		private void pcCheckAccess_DealNewRecord4AllSame(MjRec mjrec)
		{
			if (this.bPCCheckAccess)
			{
				mjrec.GetUserInfoFromDB();
				if (!string.IsNullOrEmpty(mjrec.groupname))
				{
					try
					{
						if (mjrec.ReadDate.Date <= mjrec.endYMD.Date && mjrec.ReadDate.Date >= mjrec.beginYMD.Date)
						{
							int num = -1;
							this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjrec.ControllerSN.ToString(), mjrec.DoorNo.ToString());
							if (this.dvDoors4Watching.Count > 0)
							{
								num = this.checkAccess_arrDoorName.IndexOf(this.dvDoors4Watching[0]["f_DoorName"].ToString());
							}
							if (num >= 0)
							{
								DateTime dateTime = (DateTime)this.checkAccess_arrCheckStartTime[num];
								if (mjrec.ReadDate > dateTime.AddSeconds(20.0) || mjrec.ReadDate.AddSeconds(20.0) < (DateTime)this.checkAccess_arrCheckStartTime[num])
								{
									this.checkAccess_arrCount[num] = 0;
								}
								if (this.checkAccess_arrDB_GroupName.IndexOf(mjrec.groupname) >= 0)
								{
									if ((int)this.checkAccess_arrCount[num] > 0 && (byte)this.checkAccess_arrReaderNo[num] == mjrec.ReaderNo && this.checkAccess_arrDB_GroupName.IndexOf(mjrec.groupname) >= 0)
									{
										if (this.checkAccess_arrCardId[num].ToString().IndexOf(mjrec.CardID.ToString().PadLeft(10, '0')) < 0)
										{
											ArrayList arrayList;
											int num2;
											(arrayList = this.checkAccess_arrCardId)[num2 = num] = arrayList[num2] + "," + mjrec.CardID.ToString().PadLeft(10, '0');
											ArrayList arrayList2;
											int num3;
											(arrayList2 = this.checkAccess_arrConsumerName)[num3 = num] = arrayList2[num3] + "\r\n" + mjrec.consumerName;
											this.checkAccess_arrCheckStartTime[num] = mjrec.ReadDate;
											ArrayList arrayList3;
											int num4;
											(arrayList3 = this.checkAccess_arrGroupName)[num4 = num] = arrayList3[num4] + "," + string.Format("({0})", mjrec.groupname);
											this.checkAccess_arrCount[num] = (int)this.checkAccess_arrCount[num] + 1;
										}
									}
									else
									{
										this.checkAccess_arrReaderNo[num] = mjrec.ReaderNo;
										this.checkAccess_arrGroupName[num] = string.Format("({0})", mjrec.groupname);
										this.checkAccess_arrCardId[num] = mjrec.CardID.ToString().PadLeft(10, '0');
										this.checkAccess_arrConsumerName[num] = mjrec.consumerName;
										this.checkAccess_arrCheckStartTime[num] = mjrec.ReadDate;
										this.checkAccess_arrCount[num] = 1;
									}
									if (base.IsHandleCreated)
									{
										this.mjrecNewest = null;
										MethodInvoker methodInvoker = new MethodInvoker(this.pcCheckAccess_DealOpen);
										base.BeginInvoke(methodInvoker);
									}
								}
								else
								{
									this.mjrecNewest = mjrec;
									this.mjrecNewestDoorName = this.dvDoors4Watching[0]["f_DoorName"].ToString();
								}
							}
						}
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
				}
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000963F4 File Offset: 0x000953F4
		private void pcCheckAccess_DealOpen()
		{
			if (this.bPCCheckAccess)
			{
				try
				{
					if (this.frm4PCCheckAccess == null)
					{
						this.frm4PCCheckAccess = new dfrmPCCheckAccess();
						this.frm4PCCheckAccess.frmCaller = this;
						this.frm4PCCheckAccess.TopMost = true;
					}
					if (this.frm4PCCheckAccess.bDealing)
					{
						if (this.mjrecNewest != null)
						{
							if (this.mjrecNewest.IsRemoteOpen && this.mjrecNewest.ReasonNo == 45 && this.mjrecNewestDoorName.Equals(this.frm4PCCheckAccess.strDoorFullName) && DateTime.Now.Subtract(this.dtimeShow4pcCheckAccess).TotalMilliseconds > 1000.0)
							{
								this.frm4PCCheckAccess.btnCancel_Click(null, null);
							}
							this.mjrecNewest = null;
						}
					}
					else if (!this.bNeedAllGroups)
					{
						for (int i = 0; i <= this.checkAccess_arrDoor.Count - 1; i++)
						{
							if ((int)this.checkAccess_arrCount[i] > 0)
							{
								int num = this.checkAccess_arrDB_GroupName.IndexOf(this.checkAccess_arrGroupName[i]);
								if (num >= 0)
								{
									bool flag = (int)this.checkAccess_arrCount[i] >= (int)this.checkAccess_arrDB_MoreCards[num];
									if (!flag && this.dvPCCheckDoorMoreCards != null)
									{
										this.dvPCCheckDoorMoreCards.RowFilter = string.Format("f_DoorID = {0}", this.checkAccess_arrDoor[i]);
										if (this.dvPCCheckDoorMoreCards.Count > 0 && (int)this.checkAccess_arrCount[i] >= int.Parse(this.dvPCCheckDoorMoreCards[0]["f_MoreCards"].ToString()))
										{
											flag = true;
										}
									}
									if (flag)
									{
										this.checkAccess_arrCount[i] = 0;
										this.frm4PCCheckAccess.bDealing = true;
										if (this.frm4PCCheckAccess.WindowState == FormWindowState.Minimized)
										{
											this.frm4PCCheckAccess.WindowState = FormWindowState.Normal;
										}
										this.frm4PCCheckAccess.strDoorId = this.checkAccess_arrDoor[i].ToString();
										this.frm4PCCheckAccess.strDoorFullName = this.checkAccess_arrDoorName[i].ToString();
										this.frm4PCCheckAccess.strGroupname = this.checkAccess_arrGroupName[i].ToString();
										this.frm4PCCheckAccess.strConsumername = this.checkAccess_arrConsumerName[i].ToString();
										this.frm4PCCheckAccess.strNow = ((DateTime)this.checkAccess_arrCheckStartTime[i]).ToString(wgTools.YMDHMSFormat);
										this.frm4PCCheckAccess.Show();
										this.dtimeShow4pcCheckAccess = DateTime.Now;
										break;
									}
								}
							}
						}
					}
					else
					{
						for (int j = 0; j <= this.checkAccess_arrDoor.Count - 1; j++)
						{
							if ((int)this.checkAccess_arrCount[j] > 0)
							{
								bool flag2 = true;
								string text = "";
								for (int k = 0; k < this.checkAccess_arrDB_GroupName.Count; k++)
								{
									int num2 = (this.checkAccess_arrGroupName[j].ToString().Length - this.checkAccess_arrGroupName[j].ToString().Replace(string.Format("({0})", this.checkAccess_arrDB_GroupName[k]), "").Length) / string.Format("({0})", this.checkAccess_arrDB_GroupName[k]).Length;
									if (num2 < (int)this.checkAccess_arrDB_MoreCards[k])
									{
										flag2 = false;
										break;
									}
									if (!string.IsNullOrEmpty(text))
									{
										text += ",";
									}
									text += this.checkAccess_arrDB_GroupName[k];
								}
								if (!flag2 && this.dvPCCheckDoorMoreCards != null)
								{
									this.dvPCCheckDoorMoreCards.RowFilter = string.Format("f_DoorID = {0}", this.checkAccess_arrDoor[j]);
									if (this.dvPCCheckDoorMoreCards.Count > 0 && (int)this.checkAccess_arrCount[j] >= int.Parse(this.dvPCCheckDoorMoreCards[0]["f_MoreCards"].ToString()))
									{
										flag2 = true;
									}
								}
								if (flag2)
								{
									this.frm4PCCheckAccess.strConsumername = this.checkAccess_arrConsumerName[j].ToString();
									this.frm4PCCheckAccess.strDoorFullName = string.Format("{0}", this.checkAccess_arrDoorName[j].ToString());
									this.checkAccess_arrCount[j] = 0;
									this.frm4PCCheckAccess.bDealing = true;
									if (this.frm4PCCheckAccess.WindowState == FormWindowState.Minimized)
									{
										this.frm4PCCheckAccess.WindowState = FormWindowState.Normal;
									}
									this.frm4PCCheckAccess.strDoorId = this.checkAccess_arrDoor[j].ToString();
									this.frm4PCCheckAccess.strGroupname = text;
									this.frm4PCCheckAccess.strNow = ((DateTime)this.checkAccess_arrCheckStartTime[j]).ToString(wgTools.YMDHMSFormat);
									this.frm4PCCheckAccess.Show();
									this.dtimeShow4pcCheckAccess = DateTime.Now;
									break;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0009696C File Offset: 0x0009596C
		private void pcWatchingDoorOpenWarn_Init()
		{
			try
			{
				if (wgAppConfig.getValBySql("SELECT COUNT(*) FROM t_b_ControllerNormalOpenTimeList") <= 0)
				{
					this.bWatchingDoorOpenWarn = false;
				}
				else
				{
					this.bWatchingDoorOpenWarn = true;
				}
			}
			catch
			{
				try
				{
					string text = "CREATE TABLE t_b_ControllerNormalOpenTimeList ( ";
					if (wgAppConfig.IsAccessDB)
					{
						text += "[f_Id]   AUTOINCREMENT NOT NULL ,[f_BeginYMD] datetime NOT NULL,         [f_EndYMD] datetime NOT NULL,           [f_OperateTime] datetime NOT NULL,           [f_OperateTimeEnd] datetime NOT NULL,           [f_Monday] byte NOT NULL,     [f_Tuesday] byte NOT NULL,     [f_Wednesday] byte NOT NULL,     [f_Thursday] byte NOT NULL,    [f_Friday] byte NOT NULL,     [f_Saturday] byte NOT NULL,     [f_Sunday] byte NOT NULL,     [f_DoorID] int NOT NULL,     [f_DoorControl] int NOT NULL,     f_Notes MEMO  ,f_FloorNames MEMO  ,  CONSTRAINT [PK_t_b_ControllerNormalOpenTimeList] PRIMARY KEY ( [f_ID])) ";
					}
					else
					{
						text += "[f_Id]   [int] IDENTITY (1, 1) NOT NULL ,[f_BeginYMD] [datetime] NOT NULL,         [f_EndYMD] [datetime] NOT NULL,           [f_OperateTime] [datetime] NOT NULL,           [f_OperateTimeEnd] [datetime] NOT NULL,           [f_Monday] [tinyint] NOT NULL,     [f_Tuesday] [tinyint] NOT NULL,     [f_Wednesday] [tinyint] NOT NULL,     [f_Thursday] [tinyint] NOT NULL,    [f_Friday] [tinyint] NOT NULL,     [f_Saturday] [tinyint] NOT NULL,     [f_Sunday] [tinyint] NOT NULL,     [f_DoorID] [int] NOT NULL,     [f_DoorControl] [int] NOT NULL,     f_Notes [ntext]  NULL  ,f_FloorNames [ntext]  NULL  ,  CONSTRAINT [PK_t_b_ControllerNormalOpenTimeList] PRIMARY KEY ( [f_ID])) ";
					}
					wgAppConfig.runUpdateSql(text);
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				try
				{
					string text = "CREATE TABLE t_s_wglog4DoorOpenTooLongWarn ( ";
					if (wgAppConfig.IsAccessDB)
					{
						text += "[f_RecID]   AUTOINCREMENT NOT NULL ,[f_EventType]  TEXT  (50) NULL,         [f_EventDesc] MEMO,           [f_UserID] int NULL,           [f_UserName] TEXT  (50) NULL,           [f_LogDateTime] datetime NULL ,      CONSTRAINT [PK_t_s_wglog4DoorOpenTooLongWarn] PRIMARY KEY ( [f_RecID])) ";
					}
					else
					{
						text += "[f_RecID]   [int] IDENTITY (1, 1) NOT NULL ,[f_EventType] [nvarchar] (50) NULL,         [f_EventDesc] [ntext]  NULL,           [f_UserID] [int] NOT NULL  DEFAULT (0),           [f_UserName] [nvarchar] (50) NULL,           [f_LogDateTime] [datetime] NULL ,      CONSTRAINT [PK_t_s_wglog4DoorOpenTooLongWarn] PRIMARY KEY ( [f_RecID])) ";
					}
					wgAppConfig.runUpdateSql(text);
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
				}
			}
			if (this.bWatchingDoorOpenWarn)
			{
				this.timerMax4DoorOpenWarn = (long)int.Parse(wgAppConfig.getSystemParamByNO(40));
				if (this.arrSelectedDoors4DoorOpenWarn == null)
				{
					this.arrSelectedDoors4DoorOpenWarn = new ArrayList();
					this.arrSelectedDoors4DoorOpenWarnOpenDoorTime = new ArrayList();
					this.arrSelectedDoors4DoorOpenWarnCloseDoorTime = new ArrayList();
				}
				else
				{
					this.arrSelectedDoors4DoorOpenWarn.Clear();
					this.arrSelectedDoors4DoorOpenWarnOpenDoorTime.Clear();
					this.arrSelectedDoors4DoorOpenWarnCloseDoorTime.Clear();
				}
				foreach (object obj in this.lstDoors.Items)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if ((listViewItem.Tag as frmConsole.DoorSetInfo).Selected > 0)
					{
						this.arrSelectedDoors4DoorOpenWarn.Add(listViewItem);
						this.arrSelectedDoors4DoorOpenWarnOpenDoorTime.Add(0L);
						this.arrSelectedDoors4DoorOpenWarnCloseDoorTime.Add(0L);
					}
				}
				this.bWatchingDoorOpenWarnStop = false;
				this.threadpcWatchingDoorOpenWarn_DealNewRecord = new Thread(new ThreadStart(this.pcWatchingDoorOpenWarn_DealNewRecord));
				this.threadpcWatchingDoorOpenWarn_DealNewRecord.Start();
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00096B7C File Offset: 0x00095B7C
		private void pcWatchingDoorOpenWarn_DealNewRecord()
		{
			if (!wgAppConfig.IsAccessControlBlue && this.bWatchingDoorOpenWarn)
			{
				try
				{
					while (this.bWatchingDoorOpenWarn && !this.bWatchingDoorOpenWarnStop)
					{
						do
						{
							this.checkSelectedDoorsStatus();
							Thread.Sleep(1000);
						}
						while (this.bWatchingDoorOpenWarn && !this.bWatchingDoorOpenWarnStop);
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00096BEC File Offset: 0x00095BEC
		private void consoleWatchDoorOpenWarndisplay(object oitm)
		{
			ListViewItem listViewItem = (ListViewItem)oitm;
			listViewItem.ImageIndex = 5;
			this.bWarnExisted = true;
			this.btnWarnExisted.Text = listViewItem.Text + " " + CommonStr.strDoorOpenTooLong;
			this.btnWarnExisted.Visible = this.bWarnExisted;
			this.btnWarnExisted.BackColor = Color.Red;
			this.timerWarn.Enabled = true;
			this.displayNewestLog();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00096C64 File Offset: 0x00095C64
		private void checkSelectedDoorsStatus()
		{
			DateTime now = DateTime.Now;
			long num = now.Ticks / 10000000L;
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			for (int i = 0; i < this.arrSelectedDoors4DoorOpenWarn.Count; i++)
			{
				ListViewItem listViewItem = (ListViewItem)this.arrSelectedDoors4DoorOpenWarn[i];
				if (listViewItem.ImageIndex != 3 && listViewItem.ImageIndex != 0)
				{
					bool flag = false;
					if (listViewItem.ImageIndex != 2 && listViewItem.ImageIndex != 5)
					{
						goto IL_03BD;
					}
					if ((long)this.arrSelectedDoors4DoorOpenWarnOpenDoorTime[i] == 0L)
					{
						this.arrSelectedDoors4DoorOpenWarnCloseDoorTime[i] = 0L;
						this.arrSelectedDoors4DoorOpenWarnOpenDoorTime[i] = num;
					}
					else if (num >= (long)this.arrSelectedDoors4DoorOpenWarnOpenDoorTime[i] + this.timerMax4DoorOpenWarn)
					{
						flag = true;
						string text = " SELECT t_b_ControllerNormalOpenTimeList.* FROM t_b_ControllerNormalOpenTimeList,t_b_Door ";
						text = text + " WHERE t_b_ControllerNormalOpenTimeList.[f_DoorID]=0 OR (t_b_ControllerNormalOpenTimeList.f_DoorID = t_b_Door.f_DoorID  AND t_b_Door.f_DoorName= " + wgTools.PrepareStr(listViewItem.Text) + ")";
						if (dbConnection == null)
						{
							if (wgAppConfig.IsAccessDB)
							{
								dbConnection = new OleDbConnection(wgAppConfig.dbConString);
								dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
							}
							else
							{
								dbConnection = new SqlConnection(wgAppConfig.dbConString + ";Connection Timeout=1");
								dbCommand = new SqlCommand("", dbConnection as SqlConnection);
								dbCommand.CommandTimeout = 1;
							}
						}
						dbCommand.CommandText = text;
						try
						{
							if (dbConnection.State != ConnectionState.Open)
							{
								dbConnection.Open();
							}
							dbCommand.CommandText = text;
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								MjControlTaskItem mjControlTaskItem = new MjControlTaskItem();
								mjControlTaskItem.ymdStart = (DateTime)dbDataReader["f_BeginYMD"];
								mjControlTaskItem.ymdEnd = (DateTime)dbDataReader["f_EndYMD"];
								mjControlTaskItem.hms = (DateTime)dbDataReader["f_OperateTime"];
								int num2 = 0;
								num2 = num2 * 2 + (int)((byte)dbDataReader["f_Sunday"]);
								num2 = num2 * 2 + (int)((byte)dbDataReader["f_Saturday"]);
								num2 = num2 * 2 + (int)((byte)dbDataReader["f_Friday"]);
								num2 = num2 * 2 + (int)((byte)dbDataReader["f_Thursday"]);
								num2 = num2 * 2 + (int)((byte)dbDataReader["f_Wednesday"]);
								num2 = num2 * 2 + (int)((byte)dbDataReader["f_Tuesday"]);
								num2 = num2 * 2 + (int)((byte)dbDataReader["f_Monday"]);
								mjControlTaskItem.weekdayControl = (byte)num2;
								int num3 = (int)now.DayOfWeek;
								if (num3 == 0)
								{
									num3 = 7;
								}
								num3--;
								if ((num2 & (1 << num3)) > 0 && mjControlTaskItem.ymdStart.Date <= now.Date && mjControlTaskItem.ymdEnd.Date >= now.Date && long.Parse(now.ToString("HHmmss")) >= long.Parse(mjControlTaskItem.hms.ToString("HHmmss")) && long.Parse(((DateTime)dbDataReader["f_OperateTimeEnd"]).ToString("HHmmss")) >= long.Parse(now.ToString("HHmmss")))
								{
									DateTime dateTime = (DateTime)dbDataReader["f_OperateTimeEnd"];
									num = DateTime.Parse(now.ToString("yyyy-MM-dd ") + dateTime.ToString("HH:mm:ss")).Ticks / 10000000L;
									flag = false;
									break;
								}
							}
							dbDataReader.Close();
							goto IL_040E;
						}
						catch (Exception ex)
						{
							wgAppConfig.wgDebugWrite(text + "\r\n" + ex.ToString(), EventLogEntryType.Error);
							goto IL_040E;
						}
						goto IL_03BD;
					}
					IL_040E:
					if (flag)
					{
						this.arrSelectedDoors4DoorOpenWarnOpenDoorTime[i] = DateTime.MaxValue.Ticks / 10000000L;
						InfoRow infoRow = new InfoRow();
						infoRow.desc = string.Format("{0}[{1:d}]", listViewItem.Text, (listViewItem.Tag as frmConsole.DoorSetInfo).ControllerSN);
						infoRow.information = string.Format("{0} {1}", listViewItem.Text, CommonStr.strDoorOpenTooLong);
						infoRow.category = 501;
						wgRunInfoLog.addEvent(infoRow);
						wgAppConfig.runUpdateSql(string.Concat(new object[]
						{
							"INSERT INTO [t_s_wglog4DoorOpenTooLongWarn]( [f_EventType], [f_EventDesc], [f_UserID], [f_UserName], [f_LogDateTime])  VALUES( ",
							wgTools.PrepareStrNUnicode(CommonStr.strDoorOpenTooLong),
							",",
							wgTools.PrepareStrNUnicode(infoRow.information),
							",",
							icOperator.OperatorID,
							",",
							wgTools.PrepareStrNUnicode(icOperator.OperatorName),
							",",
							wgTools.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss"),
							")"
						}));
						base.BeginInvoke(new frmConsole.deleconsoleWatchDoorOpenWarndisplay(this.consoleWatchDoorOpenWarndisplay), new object[] { listViewItem });
						goto IL_0565;
					}
					goto IL_0565;
					IL_03BD:
					if ((listViewItem.ImageIndex == 1 || listViewItem.ImageIndex == 4) && (long)this.arrSelectedDoors4DoorOpenWarnCloseDoorTime[i] == 0L)
					{
						this.arrSelectedDoors4DoorOpenWarnCloseDoorTime[i] = num;
						this.arrSelectedDoors4DoorOpenWarnOpenDoorTime[i] = 0L;
						goto IL_040E;
					}
					goto IL_040E;
				}
				IL_0565:;
			}
			if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
			{
				dbConnection.Close();
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0009721C File Offset: 0x0009621C
		private void RestoreAllSwipeInTheControllers()
		{
			using (icController icController = new icController())
			{
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					if (icController.RestoreAllSwipeInTheControllersIP() <= 0)
					{
						wgRunInfoLog.addEventNotConnect(icController.ControllerSN, icController.IP, listViewItem);
					}
					else
					{
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0}", CommonStr.strRestorAllSwipeRecords)
						});
					}
					this.displayNewestLog();
				}
				icController.Dispose();
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00097318 File Offset: 0x00096318
		public static Process RunningInstance()
		{
			Process currentProcess = Process.GetCurrentProcess();
			foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
			{
				if (process.Id != currentProcess.Id && Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == currentProcess.MainModule.FileName)
				{
					return process;
				}
			}
			return null;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00097390 File Offset: 0x00096390
		private void openTcpPort()
		{
			this.tcpServer1.Close();
			this.tcpServer1.Port = 60006;
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_TCPServerConfigPort")))
			{
				this.tcpServer1.Port = int.Parse(wgAppConfig.GetKeyVal("KEY_TCPServerConfigPort"));
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_TCPServerConfigEncoding")))
			{
				this.encodingOfTCP = wgAppConfig.GetKeyVal("KEY_TCPServerConfigEncoding");
			}
			this.tcpServer1.Open();
			this.tcpServer1.IdleTime = 50;
			this.tcpServer1.VerifyConnectionInterval = 100;
			this.tcpServer1.MaxCallbackThreads = 100;
			this.tcpServer1.MaxSendAttempts = 3;
			this.displayTcpServerStatus();
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00097448 File Offset: 0x00096448
		private void closeTcpPort()
		{
			this.tcpServer1.Close();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000974CC File Offset: 0x000964CC
		private void tcpServer1_OnConnect(TcpServerConnection connection)
		{
			frmConsole.invokeDelegate invokeDelegate = delegate
			{
				InfoRow infoRow = new InfoRow
				{
					desc = string.Format(CommonStr.strTCPClientConnected, new object[0]),
					information = string.Format("{0}={1}  ({2})", CommonStr.strTCPClientConnected, connection.Socket.Client.RemoteEndPoint.ToString(), this.encodingOfTCP)
				};
				wgRunInfoLog.addEvent(infoRow);
			};
			base.Invoke(invokeDelegate);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00097580 File Offset: 0x00096580
		private void tcpServer1_OnDataAvailable(TcpServerConnection connection)
		{
			if (connection.Socket.Available > 256)
			{
				connection.forceDisconnect();
				return;
			}
			byte[] data = this.readStream(connection.Socket);
			if (data.Length < "N3000".Length)
			{
				data = null;
				return;
			}
			if (data[0] != 78 || data[1] != 51 || data[2] != 48 || data[3] != 48 || data[4] != 48)
			{
				data = null;
				return;
			}
			if (data != null)
			{
				string dataStr = Encoding.GetEncoding(this.encodingOfTCP).GetString(data);
				frmConsole.invokeDelegate invokeDelegate = delegate
				{
					this.ParseCommand(data, connection);
					wgAppConfig.wgLogWithoutDB(string.Format("CON={0},Data={1}", connection.Socket.Client.RemoteEndPoint.ToString(), dataStr), EventLogEntryType.Information, null);
				};
				base.Invoke(invokeDelegate);
				data = null;
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00097688 File Offset: 0x00096688
		private void sendTCPdata(string data)
		{
			if (this.tcpServer1.IsOpen)
			{
				this.tcpServer1.Encoding = Encoding.GetEncoding(this.encodingOfTCP);
				this.tcpServer1.Send(data + this.FlagEnd);
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000976C4 File Offset: 0x000966C4
		private void sendTCPdata(string data, TcpServerConnection conn)
		{
			if (this.tcpServer1.IsOpen)
			{
				this.tcpServer1.Encoding = Encoding.GetEncoding(this.encodingOfTCP);
				this.tcpServer1.Send(data + this.FlagEnd, conn);
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00097701 File Offset: 0x00096701
		private void sendTCPdata(byte[] data, TcpServerConnection conn)
		{
			if (this.tcpServer1.IsOpen)
			{
				this.tcpServer1.Send(data, conn);
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00097720 File Offset: 0x00096720
		protected byte[] readStream(TcpClient client)
		{
			NetworkStream stream = client.GetStream();
			if (!stream.DataAvailable)
			{
				return null;
			}
			byte[] array = new byte[client.Available];
			int num = 0;
			try
			{
				num = stream.Read(array, 0, array.Length);
			}
			catch (IOException)
			{
			}
			if (num < array.Length)
			{
				byte[] array2 = array;
				array = new byte[num];
				Array.ConstrainedCopy(array2, 0, array, 0, num);
			}
			return array;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00097808 File Offset: 0x00096808
		private int ParseCommand(byte[] data, TcpServerConnection conn)
		{
			if (data.Length == 64 && data[0] == 23)
			{
				byte[] array = new byte[]
				{
					23, 0, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 2, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0
				};
				this.sendTCPdata(array, conn);
				return 1;
			}
			if (data.Length <= 256)
			{
				string text = Encoding.GetEncoding(this.encodingOfTCP).GetString(data).Trim();
				int num = 0;
				string[] array2 = new string[text.Length];
				string text2 = "";
				int num2 = 0;
				int i = 0;
				while (i < text.Length)
				{
					if (text.Substring(i, 1) == "\"")
					{
						if (text.Substring(i + 1).IndexOf("\"") <= 0)
						{
							this.sendTCPdata(string.Format("{0}\r\niRet= {1}, Failed", text, num2), conn);
							return 0;
						}
						text2 = "";
						array2[num] = text.Substring(i + 1, text.Substring(i + 1).IndexOf("\""));
						num++;
						i = i + text.Substring(i + 1).IndexOf("\"") + 2;
					}
					else
					{
						if (text.Substring(i, 1) == " ")
						{
							if (!string.IsNullOrEmpty(text2))
							{
								array2[num] = text2;
								text2 = "";
								num++;
							}
						}
						else
						{
							text2 += text.Substring(i, 1);
						}
						i++;
					}
				}
				if (!string.IsNullOrEmpty(text2))
				{
					array2[num] = text2;
					num++;
				}
				if (num > 0)
				{
					if (array2[0] == "POST")
					{
						byte[] array3 = new byte[]
						{
							23, 0, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 2, 0,
							0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
							0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
							0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
							0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
							0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
							0, 0, 0, 0
						};
						Encoding.GetEncoding(this.encodingOfTCP).GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).CopyTo(array3, 8);
						this.sendTCPdata(array3, conn);
						return 1;
					}
					if (array2[0] != "N3000")
					{
						this.sendTCPdata(string.Format("{0}\r\niRet={1}, Failed", text, num2), conn);
						return 0;
					}
				}
				string[] array4 = new string[num - 1];
				for (int j = 0; j < num - 1; j++)
				{
					array4[j] = array2[j + 1];
				}
				num2 = batchAutoRun.commandSpecialCall(array4);
				if (num2 > 0)
				{
					if (num2 == 3 && text.ToUpper().IndexOf("-GETALLDOORSTATUS") > 0)
					{
						if (string.IsNullOrEmpty(this.runningStatus))
						{
							num2 = 3;
							this.sendTCPdata(string.Format("{0}\r\niRet={1}, OK\r\n", text, 3) + "DoorStatus:\r\n", conn);
						}
						else if (this.dtTimeStop > this.watchingStartTime)
						{
							num2 = 3;
							this.sendTCPdata(string.Format("{0}\r\niRet={1}, OK\r\n", text, 3) + "DoorStatus:\r\n", conn);
						}
						else
						{
							num2 = 1;
							this.sendTCPdata(string.Format("{0}\r\niRet={1}, OK\r\n", text, 1) + "DoorStatus:\r\n" + this.runningStatus, conn);
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num2.ToString()));
					}
					else
					{
						this.sendTCPdata(string.Format("{0}\r\niRet={1}, OK", text, num2), conn);
					}
					return 1;
				}
				this.sendTCPdata(string.Format("{0}\r\niRet={1}, Failed", text, num2), conn);
			}
			return 0;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00097B48 File Offset: 0x00096B48
		private void displayTcpServerStatus()
		{
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00097B4C File Offset: 0x00096B4C
		private void tCPServerStartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.openTcpPort();
			}
			catch (FormatException)
			{
				MessageBox.Show("Port must be an integer", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
			catch (OverflowException)
			{
				MessageBox.Show("Port is too large", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00097BAC File Offset: 0x00096BAC
		private void txtInfoHaveNewInfoEntry()
		{
			if (frmConsole.dealingTxt <= 0 && this.watching.WatchingController != null)
			{
				Interlocked.Exchange(ref frmConsole.dealingTxt, 1);
				try
				{
					int num = 0;
					long ticks = DateTime.Now.Ticks;
					long num2 = 20000000L;
					long num3 = ticks + num2;
					int num4 = 0;
					if (this.QueRecText.Count > 0)
					{
						while (this.QueRecText.Count > 0)
						{
							if (this.bPCCheckMealOpen && this.bMealDealing && this.qMjRec4MealOpen.Count > 0)
							{
								Thread.Sleep(30);
							}
							else if (this.bPCCheckGlobalAntiBackOpen && this.bGlobalAntiBackDealing && this.qMjRec4GlobalAntiBack.Count > 0)
							{
								Thread.Sleep(30);
							}
							else
							{
								object obj;
								lock (this.QueRecText.SyncRoot)
								{
									obj = this.QueRecText.Dequeue();
								}
								if (this.txtInfoUpdateEntry(obj) > 0)
								{
									num4++;
									frmConsole.infoRowsCount++;
									num++;
									if (DateTime.Now.Ticks > num3)
									{
										num3 = DateTime.Now.Ticks + num2;
										this.displayNewestLog();
										wgRunInfoLog.addEventSpecial2();
										if (this.watching.WatchingController == null)
										{
											break;
										}
										if (!this.bNotWriteToAccessDBAsTooManyRecord)
										{
											if (wgAppConfig.IsAccessDB && this.QueRecText.Count > 200)
											{
												this.bNotWriteToAccessDBAsTooManyRecord = true;
												wgAppConfig.wgLog("NotWriteToAccessDBAsTooManyRecord Enabled. Because of QueRecText.Count = " + this.QueRecText.Count.ToString());
												wgAppConfig.UpdateKeyVal("KEY_NOTWRITETOACCESSDBASTOOMANYRECORD", "1");
											}
											if (this.bPCCheckGlobalAntiBackOpen || this.bPCCheckMealOpen)
											{
												this.bNotWriteToAccessDBAsTooManyRecord = true;
											}
											frmConsole.watchRecordLogCount += 1L;
											if (frmConsole.watchRecordLogCount > 10000L)
											{
												if (wgAppConfig.GetKeyVal("KEY_ALLOW_WatchRecordLogCount10000") == "1")
												{
													frmConsole.watchRecordLogCount = 0L;
												}
												else
												{
													this.bNotWriteToAccessDBAsTooManyRecord = true;
												}
											}
										}
									}
								}
							}
						}
					}
					if (num4 > 0)
					{
						wgRunInfoLog.addEventSpecial2();
						this.displayNewestLog();
						Application.DoEvents();
					}
				}
				catch
				{
				}
				Interlocked.Exchange(ref frmConsole.dealingTxt, 0);
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00097E18 File Offset: 0x00096E18
		private int txtInfoUpdateEntry(object info)
		{
			MjRec mjRec = new MjRec(info as string);
			if (mjRec.ControllerSN <= 0U)
			{
				return -1;
			}
			try
			{
				if (!this.watching.WatchingController.ContainsKey((int)mjRec.ControllerSN))
				{
					return -1;
				}
			}
			catch (Exception)
			{
				return -1;
			}
			InfoRow infoRow = new InfoRow();
			wgTools.WriteLine("new InfoRow");
			infoRow.category = mjRec.eventCategory;
			infoRow.desc = "";
			if (mjRec.addressIsReader)
			{
				if (this.ReaderName.ContainsKey(string.Format("{0}-{1}", mjRec.ControllerSN.ToString(), mjRec.ReaderNo.ToString())))
				{
					wgTools.WriteLine("ReaderName.ContainsKey(string.Format(");
					infoRow.desc = this.ReaderName[string.Format("{0}-{1}", mjRec.ControllerSN.ToString(), mjRec.ReaderNo.ToString())];
					mjRec.address = this.ReaderName[string.Format("{0}-{1}", mjRec.ControllerSN.ToString(), mjRec.ReaderNo.ToString())];
				}
				else
				{
					infoRow.desc = "";
				}
			}
			else
			{
				this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjRec.ControllerSN.ToString(), mjRec.DoorNo.ToString());
				if (this.dvDoors4Watching.Count > 0)
				{
					infoRow.desc = this.dvDoors4Watching[0]["f_DoorName"].ToString();
					mjRec.address = this.dvDoors4Watching[0]["f_DoorName"] as string;
				}
			}
			if (wgRunInfoLog.tcpServerEnabled > 0)
			{
				this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjRec.ControllerSN.ToString(), mjRec.DoorNo.ToString());
				if (this.dvDoors4Watching.Count > 0)
				{
					mjRec.doorName = this.dvDoors4Watching[0]["f_DoorName"] as string;
				}
			}
			if (this.player != null)
			{
				if (mjRec.IsPassed)
				{
					SystemSounds.Beep.Play();
				}
				else
				{
					this.player.Play();
				}
			}
			this.getConsumerInfo(mjRec);
			if (wgMjController.IsElevator((int)mjRec.ControllerSN) && this.watching.WatchingController.ContainsKey((int)mjRec.ControllerSN))
			{
				mjRec.updateOnlyFloorNames(this.watching.WatchingController[(int)mjRec.ControllerSN].GetFloorNames());
			}
			infoRow.information = mjRec.ToDisplayInfo();
			infoRow.detail = mjRec.ToDisplayDetail();
			if (this.bPCCheckMealOpen)
			{
				infoRow.information = infoRow.information.Replace(CommonStr.strRecordRemoteOpenDoor_ByUSBReader, CommonStr.strRecordRemoteOpenDoor_ByPCMealOpen);
				infoRow.detail = infoRow.detail.Replace(CommonStr.strRecordRemoteOpenDoor_ByUSBReader, CommonStr.strRecordRemoteOpenDoor_ByPCMealOpen);
				infoRow.information = wgAppConfig.ReplaceRemoteOpenDoor(infoRow.information);
				infoRow.detail = wgAppConfig.ReplaceRemoteOpenDoor(infoRow.detail);
			}
			if (this.bPCCheckGlobalAntiBackOpen)
			{
				infoRow.information = infoRow.information.Replace(CommonStr.strRecordRemoteOpenDoor_ByUSBReader, CommonStr.strRecordRemoteOpenDoor_ByPCGlobalAntiBackOpen);
				infoRow.detail = infoRow.detail.Replace(CommonStr.strRecordRemoteOpenDoor_ByUSBReader, CommonStr.strRecordRemoteOpenDoor_ByPCGlobalAntiBackOpen);
				infoRow.information = wgAppConfig.ReplaceRemoteOpenDoor(infoRow.information);
				infoRow.detail = wgAppConfig.ReplaceRemoteOpenDoor(infoRow.detail);
			}
			infoRow.MjRecStr = info as string;
			uint num = 0U;
			if ((mjRec.eventCategory == 4 || mjRec.eventCategory == 5) && ((this.bActivateDontDisplayDoorStatusRecords && (mjRec.CardID == 8L || mjRec.CardID == 9L)) || (this.bActivateDontDisplayRebootRecords && mjRec.CardID == 0L)))
			{
				wgRunInfoLog.addEventToLog1(infoRow, !this.bNotWriteToAccessDBAsTooManyRecord);
			}
			else
			{
				wgRunInfoLog.addEventSpecial1(infoRow, !this.bNotWriteToAccessDBAsTooManyRecord);
				num = (uint)wgRunInfoLog.eventRecID;
			}
			if (wgRunInfoLog.tcpServerEnabled > 0)
			{
				string text = mjRec.ToDisplayDetail4TCPServer();
				if (this.bPCCheckMealOpen)
				{
					text = wgAppConfig.ReplaceRemoteOpenDoor(text.Replace(CommonStr.strRecordRemoteOpenDoor_ByUSBReader, CommonStr.strRecordRemoteOpenDoor_ByPCMealOpen));
				}
				else if (this.bPCCheckGlobalAntiBackOpen)
				{
					text = wgAppConfig.ReplaceRemoteOpenDoor(text.Replace(CommonStr.strRecordRemoteOpenDoor_ByUSBReader, CommonStr.strRecordRemoteOpenDoor_ByPCGlobalAntiBackOpen));
				}
				if (wgRunInfoLog.tcpServerAutoLoad > 0)
				{
					this.sendTCPdata(string.Format("Rec: {0}\r\n{1}", infoRow.MjRecStr, text).Replace("\t", ""));
				}
			}
			if (wgRunInfoLog.logRecEventMode == 1)
			{
				wgAppConfig.wgLogRecEventOfController(string.Format("Rec: {0}\r\n{1}", infoRow.MjRecStr, infoRow.detail).Replace("\t", ""));
			}
			else if (wgRunInfoLog.logRecEventMode != 2)
			{
				if (wgRunInfoLog.logRecEventMode == 3)
				{
					if (!mjRec.IsSwipeRecord)
					{
						wgAppConfig.wgLogRecEventOfController(string.Format("Rec: {0}\r\n{1}", infoRow.MjRecStr, infoRow.detail).Replace("\t", ""));
					}
				}
				else if (wgRunInfoLog.logRecEventMode == 4)
				{
					if (mjRec.IsSwipeRecord)
					{
						wgAppConfig.wgLogRecEventOfController2(string.Format("Rec: {0}\r\n{1}", infoRow.MjRecStr, infoRow.detail).Replace("\t", ""));
					}
					else
					{
						wgAppConfig.wgLogRecEventOfController3(string.Format("Rec: {0}\r\n{1}", infoRow.MjRecStr, infoRow.detail).Replace("\t", ""));
					}
				}
			}
			if (mjRec.IsSwipeRecord || (this.bPCCheckGlobalAntiBackOpen && mjRec.IsRemoteOpen))
			{
				string text2 = mjRec.ToDisplayInfo4Led("");
				if (frmWatchingLED.ledPageSwipeMax > 0)
				{
					if (mjRec.isValidPassedSwipe())
					{
						int num2 = this.arrSwipeInCardNO4Led.IndexOf(mjRec.CardID);
						if (num2 >= 0)
						{
							this.arrSwipeInCardNO4Led.RemoveAt(num2);
							this.arrSwipeIn4Led.RemoveAt(num2);
						}
						string text3 = mjRec.ToDisplayInfo4LedMode1("");
						if (!string.IsNullOrEmpty(text3))
						{
							this.arrSwipeIn4Led.Add(text3);
							this.arrSwipeInCardNO4Led.Add(mjRec.CardID);
							if (this.arrSwipeIn4Led.Count > 1000)
							{
								ArrayList arrayList = new ArrayList();
								ArrayList arrayList2 = new ArrayList();
								for (int i = this.arrSwipeIn4Led.Count - 10; i < this.arrSwipeIn4Led.Count; i++)
								{
									arrayList.Add(this.arrSwipeIn4Led[i]);
									arrayList2.Add(this.arrSwipeInCardNO4Led[i]);
								}
								this.arrSwipeIn4Led.Clear();
								this.arrSwipeInCardNO4Led.Clear();
								for (int j = 0; j < arrayList.Count; j++)
								{
									this.arrSwipeIn4Led.Add(arrayList[j]);
									this.arrSwipeInCardNO4Led.Add(arrayList2[j]);
								}
							}
						}
					}
					int ledPageSwipeMax = frmWatchingLED.ledPageSwipeMax;
					if (this.arrSwipeIn4Led.Count <= 0)
					{
						for (int k = 0; k < ledPageSwipeMax * 3; k++)
						{
							text2 += "/ ";
						}
					}
					else
					{
						string text4 = (string)this.arrSwipeIn4Led[this.arrSwipeIn4Led.Count - 1];
						int num3 = 1;
						if (this.arrSwipeIn4Led.Count > 1)
						{
							for (int l = this.arrSwipeIn4Led.Count - 2; l >= 0; l--)
							{
								text4 = text4 + "/" + this.arrSwipeIn4Led[l];
								num3++;
								if (num3 >= ledPageSwipeMax)
								{
									break;
								}
							}
						}
						text2 = text2 + "/" + text4;
						if (num3 < ledPageSwipeMax)
						{
							for (int m = 0; m < ledPageSwipeMax * 3 - num3 * 3; m++)
							{
								text2 += "/ ";
							}
						}
					}
				}
				frmWatchingLED.sendLEDSwipeInfo(text2);
			}
			this.txtInfoUpdateEntry4RealtimeGetRecords(mjRec);
			bool flag = false;
			this.onTimeRefreshPhotoAvi(mjRec, ref flag, num);
			if (this.bCameraEnabled && flag && num > 0U)
			{
				string text5 = mjRec.ReadDate.ToString("yyyyMMdd_HHmmss_") + mjRec.CardID.ToString() + "_" + mjRec.ToStringRaw();
				this.arrCapturedFilenameRecID.Add(num);
				this.arrCapturedFilename.Add(text5);
			}
			this.pcCheckAccess_DealNewRecord(mjRec);
			if (this.frm4ShowPersonsInside != null)
			{
				this.frm4ShowPersonsInside.NextRefreshTime = DateTime.Now.AddMinutes(-1.0);
			}
			return 1;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000986A4 File Offset: 0x000976A4
		private void txtInfoUpdateEntry4RealtimeGetRecords(MjRec mjrec)
		{
			try
			{
				if (this.stepOfRealtimeGetRecords != frmConsole.StepOfRealtimeGetReocrds.Stop && wgAppConfig.DBIsConnected && this.realtimeGetRecordsSwipeIndexGot.ContainsKey((int)mjrec.ControllerSN))
				{
					if ((ulong)mjrec.IndexInDataFlash == (ulong)((long)this.realtimeGetRecordsSwipeIndexGot[(int)mjrec.ControllerSN]))
					{
						if (icSwipeRecord.AddNewSwipe_SynConsumerID(mjrec) >= 0)
						{
							this.realtimeGetRecordsSwipeIndexGot[(int)mjrec.ControllerSN] = (int)(mjrec.IndexInDataFlash + 1U);
							this.realtimeGetRecordsSwipeRawStringGot[(int)mjrec.ControllerSN] = mjrec.ToStringRaw();
							this.needDelSwipeControllers[(int)mjrec.ControllerSN] = 1;
						}
					}
					else if ((ulong)mjrec.IndexInDataFlash > (ulong)((long)this.realtimeGetRecordsSwipeIndexGot[(int)mjrec.ControllerSN]))
					{
						this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjrec.ControllerSN.ToString(), mjrec.DoorNo.ToString());
						if (this.dvDoors4Watching.Count > 0 && this.doorsNeedToGetRecords.IndexOf(this.dvDoors4Watching[0]["f_DoorName"].ToString(), Math.Max(0, this.dealtIndexOfDoorsNeedToGetRecords + 1)) < 0)
						{
							this.doorsNeedToGetRecords.Add(this.dvDoors4Watching[0]["f_DoorName"].ToString());
						}
					}
					else if ((ulong)(mjrec.IndexInDataFlash + 1U) == (ulong)((long)this.realtimeGetRecordsSwipeIndexGot[(int)mjrec.ControllerSN]) && this.realtimeGetRecordsSwipeRawStringGot[(int)mjrec.ControllerSN].CompareTo(mjrec.ToStringRaw()) != 0 && icSwipeRecord.AddNewSwipe_SynConsumerID(mjrec) >= 0)
					{
						this.realtimeGetRecordsSwipeRawStringGot[(int)mjrec.ControllerSN] = mjrec.ToStringRaw();
						this.needDelSwipeControllers[(int)mjrec.ControllerSN] = 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000988A8 File Offset: 0x000978A8
		private void UploadCommPassword(string pwd, string oldpwd)
		{
			using (icController icController = new icController())
			{
				this.bStopComm = false;
				ArrayList arrayList = new ArrayList();
				byte[] array = new byte[1152];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 0;
				}
				string text = "";
				if (!string.IsNullOrEmpty(pwd))
				{
					text = pwd.Substring(0, Math.Min(16, pwd.Length));
				}
				char[] array2 = text.PadRight(16, '\0').ToCharArray();
				int num = 16;
				int num2 = 0;
				while (num2 < 16 && num2 < array2.Length)
				{
					array[num] = (byte)(array2[num2] & 'ÿ');
					array[1024 + (num >> 3)] = array[1024 + (num >> 3)] | (byte)(1 << (num & 7));
					num++;
					num2++;
				}
				foreach (object obj in this.lstDoors.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (this.bStopComm)
					{
						break;
					}
					icController.GetInfoFromDBByDoorNameSpecial(listViewItem.Text, this.dsWatchingDoorInfo);
					if (arrayList.IndexOf(icController.ControllerSN) < 0)
					{
						arrayList.Add(icController.ControllerSN);
						if (string.IsNullOrEmpty(oldpwd))
						{
							icController.UpdateConfigureCPUSuperIP(array, "");
						}
						else
						{
							icController.UpdateConfigureCPUSuperIP(array, WGPacket.Dpt(oldpwd));
						}
						icController.RebootControllerIP();
						wgRunInfoLog.addEvent(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", listViewItem.Text, icController.ControllerSN),
							information = string.Format("{0} {1}", CommonStr.strSetCommPassword, "")
						});
						wgAppConfig.wgLog(".setComm_" + icController.ControllerSN.ToString());
					}
				}
			}
		}

		// Token: 0x0400091A RID: 2330
		private const int CommandMaxLen = 256;

		// Token: 0x0400091B RID: 2331
		private const int DelayOneCycle = 30;

		// Token: 0x0400091C RID: 2332
		private const int if_Category = 0;

		// Token: 0x0400091D RID: 2333
		private const int if_RecID = 1;

		// Token: 0x0400091E RID: 2334
		private const int if_Time = 2;

		// Token: 0x0400091F RID: 2335
		private const int if_Desc = 3;

		// Token: 0x04000920 RID: 2336
		private const int if_Info = 4;

		// Token: 0x04000921 RID: 2337
		private const int if_Detail = 5;

		// Token: 0x04000922 RID: 2338
		private const int if_MjRecStr = 6;

		// Token: 0x04000923 RID: 2339
		public const int MODE_Check = 1;

		// Token: 0x04000924 RID: 2340
		public const int MODE_SetTime = 2;

		// Token: 0x04000925 RID: 2341
		public const int MODE_Upload = 3;

		// Token: 0x04000926 RID: 2342
		public const int MODE_Server = 4;

		// Token: 0x04000927 RID: 2343
		public const int MODE_GetRecords = 5;

		// Token: 0x04000928 RID: 2344
		public const int MODE_RemoteOpen = 6;

		// Token: 0x04000929 RID: 2345
		private long addedPrivilegeCnt;

		// Token: 0x0400092A RID: 2346
		private bool bCameraEnabled;

		// Token: 0x0400092B RID: 2347
		private bool bDataErrorExist;

		// Token: 0x0400092C RID: 2348
		private bool bDirectToRealtimeGet;

		// Token: 0x0400092D RID: 2349
		private bool bExistPhotoBackup;

		// Token: 0x0400092E RID: 2350
		private bool bLoadTooManyRecords;

		// Token: 0x0400092F RID: 2351
		private bool bPCCheckGlobalAntiBackOpen;

		// Token: 0x04000930 RID: 2352
		private bool bGlobalAntiBackDealing;

		// Token: 0x04000931 RID: 2353
		private bool bGlobalAntiBackStop;

		// Token: 0x04000932 RID: 2354
		private bool GlobalAntiBackOpen_bFirstInThenOut;

		// Token: 0x04000933 RID: 2355
		private bool GlobalAntiBackOpen_bPersonsInside;

		// Token: 0x04000934 RID: 2356
		private int GlobalAntiBackOpen_PersonsInside;

		// Token: 0x04000935 RID: 2357
		private DataTable dtGlobalAntiBackOpen_ConsumerLocation;

		// Token: 0x04000936 RID: 2358
		private DataTable dtGlobalAntiBackOpen_ControlTimeSeg;

		// Token: 0x04000937 RID: 2359
		private DataTable dtGlobalAntiBackOpen_PrivilegeType;

		// Token: 0x04000938 RID: 2360
		private DataTable dtGlobalAntiBackOpen_Readers4Exit;

		// Token: 0x04000939 RID: 2361
		private DataView dvGlobalAntiBackOpen_ControlTimeSeg;

		// Token: 0x0400093A RID: 2362
		private DataView dvGlobalAntiBackOpen_PrivilegeType;

		// Token: 0x0400093B RID: 2363
		private Thread threadGlobalAntiBackOpen_DealNewRecord;

		// Token: 0x0400093C RID: 2364
		private Queue qMjRec4GlobalAntiBack = new Queue();

		// Token: 0x0400093D RID: 2365
		private DataSet GlobalAntiBackOpen_ds = new DataSet();

		// Token: 0x0400093E RID: 2366
		private bool bAllowOperateExtendPort;

		// Token: 0x0400093F RID: 2367
		private bool bPCCheckMealOpen;

		// Token: 0x04000940 RID: 2368
		private bool bMealDealing;

		// Token: 0x04000941 RID: 2369
		private bool bMealStop;

		// Token: 0x04000942 RID: 2370
		private bool bActivegroup4MealLimit;

		// Token: 0x04000943 RID: 2371
		private DataSet dsgroup4MealLimit = new DataSet();

		// Token: 0x04000944 RID: 2372
		private DataView dvgroup4MealLimit;

		// Token: 0x04000945 RID: 2373
		private Thread threadpcCheckMealOpen_DealNewRecord;

		// Token: 0x04000946 RID: 2374
		private Queue qMjRec4MealOpen = new Queue();

		// Token: 0x04000947 RID: 2375
		private ArrayList arrMealRecord = new ArrayList();

		// Token: 0x04000948 RID: 2376
		private ArrayList arrMealRecordDay = new ArrayList();

		// Token: 0x04000949 RID: 2377
		private DataTable dtControlHoliday;

		// Token: 0x0400094A RID: 2378
		private DataView dvHolidays;

		// Token: 0x0400094B RID: 2379
		private DataView dvNeedWork;

		// Token: 0x0400094C RID: 2380
		private bool bNeedCheckLosePacket;

		// Token: 0x0400094D RID: 2381
		private bool bOccuredDBNotConnect;

		// Token: 0x0400094E RID: 2382
		private bool bPCCheckAccess;

		// Token: 0x0400094F RID: 2383
		private bool bNeedAllGroups;

		// Token: 0x04000950 RID: 2384
		private ArrayList checkAccess_arrDoor = new ArrayList();

		// Token: 0x04000951 RID: 2385
		private ArrayList checkAccess_arrDoorName = new ArrayList();

		// Token: 0x04000952 RID: 2386
		private ArrayList checkAccess_arrGroupName = new ArrayList();

		// Token: 0x04000953 RID: 2387
		private ArrayList checkAccess_arrReaderNo = new ArrayList();

		// Token: 0x04000954 RID: 2388
		private ArrayList checkAccess_arrCardId = new ArrayList();

		// Token: 0x04000955 RID: 2389
		private ArrayList checkAccess_arrCheckStartTime = new ArrayList();

		// Token: 0x04000956 RID: 2390
		private ArrayList checkAccess_arrCheckTime = new ArrayList();

		// Token: 0x04000957 RID: 2391
		private ArrayList checkAccess_arrConsumerName = new ArrayList();

		// Token: 0x04000958 RID: 2392
		private ArrayList checkAccess_arrCount = new ArrayList();

		// Token: 0x04000959 RID: 2393
		private ArrayList checkAccess_arrDB_GroupName = new ArrayList();

		// Token: 0x0400095A RID: 2394
		private ArrayList checkAccess_arrDB_MoreCards = new ArrayList();

		// Token: 0x0400095B RID: 2395
		private DataTable dtPCCheckDoorMoreCards;

		// Token: 0x0400095C RID: 2396
		private DataView dvPCCheckDoorMoreCards;

		// Token: 0x0400095D RID: 2397
		private DateTime dtimeShow4pcCheckAccess = DateTime.Now;

		// Token: 0x0400095E RID: 2398
		public dfrmPCCheckAccess frm4PCCheckAccess;

		// Token: 0x0400095F RID: 2399
		private bool bPersonInsideStarted;

		// Token: 0x04000960 RID: 2400
		private bool bsoftwareWarnAutoResetWhenAllDoorAreClosed;

		// Token: 0x04000961 RID: 2401
		private bool bStartRealtimeGetRecords;

		// Token: 0x04000962 RID: 2402
		private bool bStopComm;

		// Token: 0x04000963 RID: 2403
		private bool bWarnExisted;

		// Token: 0x04000964 RID: 2404
		private long timerMax4DoorOpenWarn = 5L;

		// Token: 0x04000965 RID: 2405
		private bool bWatchingDoorOpenWarn = true;

		// Token: 0x04000966 RID: 2406
		private bool bWatchingDoorOpenWarnStop;

		// Token: 0x04000967 RID: 2407
		private ArrayList arrSelectedDoors4DoorOpenWarn;

		// Token: 0x04000968 RID: 2408
		private ArrayList arrSelectedDoors4DoorOpenWarnCloseDoorTime;

		// Token: 0x04000969 RID: 2409
		private ArrayList arrSelectedDoors4DoorOpenWarnOpenDoorTime;

		// Token: 0x0400096A RID: 2410
		private SqlCommand cm4ParamPrivilege;

		// Token: 0x0400096B RID: 2411
		private int CommOperateOption;

		// Token: 0x0400096C RID: 2412
		private long consumerNO;

		// Token: 0x04000971 RID: 2417
		private int dealtDoorIndex;

		// Token: 0x04000972 RID: 2418
		private int Delay5SecUpdateDoor;

		// Token: 0x04000973 RID: 2419
		private dfrmFind dfrmFind1;

		// Token: 0x04000974 RID: 2420
		private dfrmMultiThreadOperation dfrmMultiThreadOperation1Privilege;

		// Token: 0x04000975 RID: 2421
		private DataSet dsWatchingDoorInfo;

		// Token: 0x04000976 RID: 2422
		private DataTable dt;

		// Token: 0x04000977 RID: 2423
		private DataTable dtPrivilege;

		// Token: 0x04000978 RID: 2424
		private DataTable dtReader;

		// Token: 0x04000979 RID: 2425
		private DataView dvDoors;

		// Token: 0x0400097A RID: 2426
		private DataView dvDoors4Check;

		// Token: 0x0400097B RID: 2427
		private DataView dvDoors4Video;

		// Token: 0x0400097C RID: 2428
		private DataView dvDoors4Watching;

		// Token: 0x0400097D RID: 2429
		private DataView dvGroups4Access;

		// Token: 0x0400097E RID: 2430
		private DataView dvReaders4Video;

		// Token: 0x0400097F RID: 2431
		private dfrmLocate frm4ShowLocate;

		// Token: 0x04000980 RID: 2432
		private frmMaps frmMaps1;

		// Token: 0x04000981 RID: 2433
		private frmWatchingMoreRecords frmMoreRecords;

		// Token: 0x04000982 RID: 2434
		private frmWatchingLCD frmWatchingLCD;

		// Token: 0x04000983 RID: 2435
		private ImageList imgDoor2;

		// Token: 0x04000984 RID: 2436
		private int lastPhotoCnt;

		// Token: 0x04000985 RID: 2437
		private int lastRefreshValidIndex;

		// Token: 0x04000986 RID: 2438
		private MjRec mjrecNewest;

		// Token: 0x04000987 RID: 2439
		private string oldInfoTitleString;

		// Token: 0x04000988 RID: 2440
		private string oldZoneText;

		// Token: 0x04000989 RID: 2441
		private dfrmPhotoAvi photoavi;

		// Token: 0x0400098A RID: 2442
		private SoundPlayer player;

		// Token: 0x0400098B RID: 2443
		private int retDealtLast;

		// Token: 0x0400098C RID: 2444
		private string strAllProductsDriversInfo;

		// Token: 0x0400098D RID: 2445
		private DataTable tbRunInfoLog;

		// Token: 0x0400098E RID: 2446
		private DataView dv;

		// Token: 0x0400098F RID: 2447
		private ArrayList arrCapturedFilenameRecID = new ArrayList();

		// Token: 0x04000990 RID: 2448
		private ArrayList arrCapturedFilename = new ArrayList();

		// Token: 0x04000991 RID: 2449
		private Thread threadpcWatchingDoorOpenWarn_DealNewRecord;

		// Token: 0x04000992 RID: 2450
		private uint warnlastRecordCount;

		// Token: 0x04000994 RID: 2452
		private int watchingDealtDoorIndex;

		// Token: 0x04000995 RID: 2453
		private DateTime watchingStartTime;

		// Token: 0x04000996 RID: 2454
		public bool activateWarnAvi;

		// Token: 0x04000997 RID: 2455
		private ArrayList arrCameraName4ReaderIDWithCamera = new ArrayList();

		// Token: 0x04000998 RID: 2456
		private ArrayList arrCardRecordID = new ArrayList();

		// Token: 0x04000999 RID: 2457
		private ArrayList arrControllerTryAdjustTime = new ArrayList();

		// Token: 0x0400099A RID: 2458
		private ArrayList arrControllerTryRestoreDefaultConfigure = new ArrayList();

		// Token: 0x0400099B RID: 2459
		private Dictionary<int, int> arrDealtController = new Dictionary<int, int>();

		// Token: 0x0400099C RID: 2460
		private ArrayList arrInBegin = new ArrayList();

		// Token: 0x0400099D RID: 2461
		private ArrayList arrInEnd = new ArrayList();

		// Token: 0x0400099E RID: 2462
		private ArrayList arrOutBegin = new ArrayList();

		// Token: 0x0400099F RID: 2463
		private ArrayList arrOutEnd = new ArrayList();

		// Token: 0x040009A0 RID: 2464
		private ArrayList arrReaderId4Camera = new ArrayList();

		// Token: 0x040009A1 RID: 2465
		private ArrayList arrReaderIDWithCamera = new ArrayList();

		// Token: 0x040009A2 RID: 2466
		public ArrayList arrSelectDoors4Sign = new ArrayList();

		// Token: 0x040009A3 RID: 2467
		private ArrayList arrSelectedControllers = new ArrayList();

		// Token: 0x040009A4 RID: 2468
		private ArrayList arrSelectedDoors = new ArrayList();

		// Token: 0x040009A5 RID: 2469
		private ArrayList arrSelectedDoorsItem = new ArrayList();

		// Token: 0x040009A6 RID: 2470
		private ArrayList arrSwipeIn4Led = new ArrayList();

		// Token: 0x040009A7 RID: 2471
		private ArrayList arrSwipeInCardNO4Led = new ArrayList();

		// Token: 0x040009A8 RID: 2472
		private ArrayList arrWatchingRecordListIndex = new ArrayList();

		// Token: 0x040009A9 RID: 2473
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x040009AA RID: 2474
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x040009AB RID: 2475
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x040009AC RID: 2476
		private bool bActivateDisplayCertID = wgAppConfig.getParamValBoolByNO(187);

		// Token: 0x040009AD RID: 2477
		private bool bActivateDisplayYellowWhenDoorOpen = wgAppConfig.getParamValBoolByNO(173);

		// Token: 0x040009AE RID: 2478
		private bool bActivateDontDisplayDoorStatusRecords = wgAppConfig.getParamValBoolByNO(163);

		// Token: 0x040009AF RID: 2479
		private bool bActivateDontDisplayRebootRecords = wgAppConfig.getParamValBoolByNO(164);

		// Token: 0x040009B0 RID: 2480
		public static bool bCheckRunningSingle4CloudServer;

		// Token: 0x040009B1 RID: 2481
		private bool bFalseShowOnce = true;

		// Token: 0x040009B2 RID: 2482
		private byte[] blankImage = new byte[]
		{
			66, 77, 198, 0, 0, 0, 0, 0, 0, 0,
			118, 0, 0, 0, 40, 0, 0, 0, 11, 0,
			0, 0, 10, 0, 0, 0, 1, 0, 4, 0,
			0, 0, 0, 0, 80, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 16, 0, 0, 0,
			16, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			128, 0, 0, 128, 0, 0, 0, 128, 128, 0,
			128, 0, 0, 0, 128, 0, 128, 0, 128, 128,
			0, 0, 192, 192, 192, 0, 128, 128, 128, 0,
			0, 0, byte.MaxValue, 0, 0, byte.MaxValue, 0, 0, 0, byte.MaxValue,
			byte.MaxValue, 0, byte.MaxValue, 0, 0, 0, byte.MaxValue, 0, byte.MaxValue, 0,
			byte.MaxValue, byte.MaxValue, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, 240, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240,
			0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 0, 0,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 0, 0, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, 240, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240,
			0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 0, 0,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 0, 0
		};

		// Token: 0x040009B3 RID: 2483
		public bool bMainWindowDisplay = true;

		// Token: 0x040009B4 RID: 2484
		public bool bNotWriteToAccessDBAsTooManyRecord;

		// Token: 0x040009B5 RID: 2485
		private int checkLimitedPersonsFlag = -1;

		// Token: 0x040009B6 RID: 2486
		private int checkTimeSegmentFlag = -1;

		// Token: 0x040009B7 RID: 2487
		private string CommOperate = "";

		// Token: 0x040009B9 RID: 2489
		private wgMjControllerConfigure controlConfigure4uploadPrivilege = new wgMjControllerConfigure();

		// Token: 0x040009BA RID: 2490
		private wgMjControllerConfigure controlConfigureUnused = new wgMjControllerConfigure();

		// Token: 0x040009BB RID: 2491
		private wgMjControllerHolidaysList controlHolidayList4uploadPrivilege = new wgMjControllerHolidaysList();

		// Token: 0x040009BC RID: 2492
		private wgMjControllerTaskList controlTaskList4uploadPrivilege = new wgMjControllerTaskList();

		// Token: 0x040009BD RID: 2493
		private static int dealingTxt;

		// Token: 0x040009BE RID: 2494
		private int dealtIndexOfDoorsNeedToGetRecords = -1;

		// Token: 0x040009C0 RID: 2496
		private ArrayList doorsNeedToGetRecords = new ArrayList();

		// Token: 0x040009C1 RID: 2497
		private DateTime dtDealt = DateTime.Now;

		// Token: 0x040009C2 RID: 2498
		private DateTime dtlstDoorViewChange = DateTime.Now;

		// Token: 0x040009C3 RID: 2499
		private DateTime dtRunningUpdate = DateTime.Now;

		// Token: 0x040009C4 RID: 2500
		private DateTime dtTimeStop = DateTime.Now;

		// Token: 0x040009C5 RID: 2501
		public DataView dvConsumer4Access;

		// Token: 0x040009C6 RID: 2502
		private string encodingOfTCP = "GB2312";

		// Token: 0x040009C7 RID: 2503
		private int existedVideoCHDll = -1;

		// Token: 0x040009C8 RID: 2504
		private string fileName = "";

		// Token: 0x040009C9 RID: 2505
		public string FlagEnd = "\r\n";

		// Token: 0x040009CA RID: 2506
		public dfrmPersonsInside frm4ShowPersonsInside;

		// Token: 0x040009CB RID: 2507
		private static int infoRowsCount;

		// Token: 0x040009CC RID: 2508
		private ListView listViewNotDisplay = new ListView();

		// Token: 0x040009CD RID: 2509
		private string mjrecNewestDoorName = "";

		// Token: 0x040009CE RID: 2510
		private Dictionary<int, int> needDelSwipeControllers;

		// Token: 0x040009CF RID: 2511
		private byte[] oImage = new byte[]
		{
			66, 77, 198, 0, 0, 0, 0, 0, 0, 0,
			118, 0, 0, 0, 40, 0, 0, 0, 11, 0,
			0, 0, 10, 0, 0, 0, 1, 0, 4, 0,
			0, 0, 0, 0, 80, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 16, 0, 0, 0,
			16, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			128, 0, 0, 128, 0, 0, 0, 128, 128, 0,
			128, 0, 0, 0, 128, 0, 128, 0, 128, 128,
			0, 0, 192, 192, 192, 0, 128, 128, 128, 0,
			0, 0, byte.MaxValue, 0, 0, byte.MaxValue, 0, 0, 0, byte.MaxValue,
			byte.MaxValue, 0, byte.MaxValue, 0, 0, 0, byte.MaxValue, 0, byte.MaxValue, 0,
			byte.MaxValue, byte.MaxValue, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue, byte.MaxValue,
			0, 15, byte.MaxValue, 240, 0, 0, byte.MaxValue, 0, byte.MaxValue, 240,
			15, 240, 0, 0, 240, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 240,
			0, 0, 240, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 240, 0, 0,
			15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 0, 0, 15, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 0, 0, 240, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			240, 240, 0, 0, 240, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 240,
			0, 0, byte.MaxValue, 0, byte.MaxValue, 240, 15, 240, 0, 0,
			byte.MaxValue, byte.MaxValue, 0, 15, byte.MaxValue, 240, 0, 0
		};

		// Token: 0x040009D0 RID: 2512
		private string oldWarnInfo = "";

		// Token: 0x040009D1 RID: 2513
		private string oldWarnInfoTips = "";

		// Token: 0x040009D2 RID: 2514
		public dfrmPhotoAviSingle photoaviSingle;

		// Token: 0x040009D3 RID: 2515
		private dfrmPhotoAvi[] photoaviWarn = new dfrmPhotoAvi[4];

		// Token: 0x040009D4 RID: 2516
		private bool[] photoaviWarnVisible = new bool[4];

		// Token: 0x040009D6 RID: 2518
		private string prevSelectedCameraID = "";

		// Token: 0x040009D7 RID: 2519
		private wgMjControllerPrivilege pri = new wgMjControllerPrivilege();

		// Token: 0x040009D8 RID: 2520
		private Queue QueRecText = new Queue();

		// Token: 0x040009D9 RID: 2521
		private Dictionary<string, string> ReaderID = new Dictionary<string, string>();

		// Token: 0x040009DA RID: 2522
		private Dictionary<string, string> ReaderName = new Dictionary<string, string>();

		// Token: 0x040009DB RID: 2523
		private Dictionary<int, int> realtimeGetRecordsSwipeIndexGot = new Dictionary<int, int>();

		// Token: 0x040009DC RID: 2524
		private Dictionary<int, string> realtimeGetRecordsSwipeRawStringGot = new Dictionary<int, string>();

		// Token: 0x040009DD RID: 2525
		private static int receivedPktCount;

		// Token: 0x040009DE RID: 2526
		private string runningStatus = "";

		// Token: 0x040009DF RID: 2527
		private string selectedCameraID = "";

		// Token: 0x040009E0 RID: 2528
		private Dictionary<int, icController> selectedControllersOfRealtimeGetRecords;

		// Token: 0x040009E1 RID: 2529
		private ArrayList selectedControllersSNOfRealtimeGetRecords = new ArrayList();

		// Token: 0x040009E2 RID: 2530
		private string strRealMonitor = "";

		// Token: 0x040009E4 RID: 2532
		public int totalConsoleMode;

		// Token: 0x040009E5 RID: 2533
		private icConsumer user = new icConsumer();

		// Token: 0x040009E6 RID: 2534
		private static long watchRecordLogCount;

		// Token: 0x040009E7 RID: 2535
		private byte[] xImage = new byte[]
		{
			66, 77, 198, 0, 0, 0, 0, 0, 0, 0,
			118, 0, 0, 0, 40, 0, 0, 0, 11, 0,
			0, 0, 10, 0, 0, 0, 1, 0, 4, 0,
			0, 0, 0, 0, 80, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 16, 0, 0, 0,
			16, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			128, 0, 0, 128, 0, 0, 0, 128, 128, 0,
			128, 0, 0, 0, 128, 0, 128, 0, 128, 128,
			0, 0, 192, 192, 192, 0, 128, 128, 128, 0,
			0, 0, byte.MaxValue, 0, 0, byte.MaxValue, 0, 0, 0, byte.MaxValue,
			byte.MaxValue, 0, byte.MaxValue, 0, 0, 0, byte.MaxValue, 0, byte.MaxValue, 0,
			byte.MaxValue, byte.MaxValue, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 240, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, 240, 240, 0, 0, byte.MaxValue, 15, byte.MaxValue, byte.MaxValue,
			15, 240, 0, 0, byte.MaxValue, 240, byte.MaxValue, 240, byte.MaxValue, 240,
			0, 0, byte.MaxValue, byte.MaxValue, 15, 15, byte.MaxValue, 240, 0, 0,
			byte.MaxValue, byte.MaxValue, 15, 15, byte.MaxValue, 240, 0, 0, byte.MaxValue, byte.MaxValue,
			15, 15, byte.MaxValue, 240, 0, 0, byte.MaxValue, 240, byte.MaxValue, 240,
			byte.MaxValue, 240, 0, 0, byte.MaxValue, 15, byte.MaxValue, byte.MaxValue, 15, 240,
			0, 0, 240, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 240, 0, 0,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 240, 0, 0
		};

		// Token: 0x04000A27 RID: 2599
		private frmConsole.StepOfRealtimeGetReocrds stepOfRealtimeGetRecords;

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x0600055B RID: 1371
		private delegate void txtInfoHaveNewInfo();

		// Token: 0x02000043 RID: 67
		// (Invoke) Token: 0x0600055F RID: 1375
		private delegate void itmDisplayStatus(ListViewItem itm, int status);

		// Token: 0x02000044 RID: 68
		private class GlobalAntiBackOpen_clsControlInfoGlobalAntiBack
		{
			// Token: 0x04000A4A RID: 2634
			public bool bGlobalAntiBack = true;

			// Token: 0x04000A4B RID: 2635
			public long CardID;

			// Token: 0x04000A4C RID: 2636
			public int consumerID;

			// Token: 0x04000A4D RID: 2637
			public string ConsumerName;

			// Token: 0x04000A4E RID: 2638
			public icController doorController;

			// Token: 0x04000A4F RID: 2639
			public int DoorNo;

			// Token: 0x04000A50 RID: 2640
			public int inOut;
		}

		// Token: 0x02000045 RID: 69
		// (Invoke) Token: 0x06000564 RID: 1380
		private delegate void GlobalAntiBackOpen_updateInfo();

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x06000568 RID: 1384
		private delegate void GlobalAntiBackOpen_updateInfoWithInfo(string info);

		// Token: 0x02000047 RID: 71
		private class clsControlInfo
		{
			// Token: 0x04000A51 RID: 2641
			public bool bMeal;

			// Token: 0x04000A52 RID: 2642
			public long CardID;

			// Token: 0x04000A53 RID: 2643
			public string ConsumerName;

			// Token: 0x04000A54 RID: 2644
			public icController doorController;

			// Token: 0x04000A55 RID: 2645
			public int DoorNo;

			// Token: 0x04000A56 RID: 2646
			public int extendPortDelaySecond = 3;
		}

		// Token: 0x02000048 RID: 72
		// (Invoke) Token: 0x0600056D RID: 1389
		public delegate void invokeDelegate();

		// Token: 0x02000049 RID: 73
		// (Invoke) Token: 0x06000571 RID: 1393
		public delegate void deleconsoleWatchDoorOpenWarndisplay(object oitm);

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x06000575 RID: 1397
		private delegate void dlgtdisplayNewestLog();

		// Token: 0x0200004B RID: 75
		private class DoorSetInfo
		{
			// Token: 0x06000578 RID: 1400 RVA: 0x0009B478 File Offset: 0x0009A478
			public DoorSetInfo(int id, string name, int no, int sn, string ip, int port, int state, int zoneid)
			{
				this.DoorId = id;
				this.DoorName = name;
				this.DoorNO = no;
				this.ControllerSN = sn;
				this.IP = ip;
				this.PORT = port;
				this.ConnectState = state;
				this.ZoneID = zoneid;
			}

			// Token: 0x04000A57 RID: 2647
			public int ConnectState;

			// Token: 0x04000A58 RID: 2648
			public int ControllerSN;

			// Token: 0x04000A59 RID: 2649
			public int DoorId;

			// Token: 0x04000A5A RID: 2650
			public string DoorName = "";

			// Token: 0x04000A5B RID: 2651
			public int DoorNO;

			// Token: 0x04000A5C RID: 2652
			public string IP = "";

			// Token: 0x04000A5D RID: 2653
			public int PORT = 60000;

			// Token: 0x04000A5E RID: 2654
			public int Selected;

			// Token: 0x04000A5F RID: 2655
			public int ZoneID;
		}

		// Token: 0x0200004C RID: 76
		private enum StepOfRealtimeGetReocrds
		{
			// Token: 0x04000A61 RID: 2657
			Stop,
			// Token: 0x04000A62 RID: 2658
			GetRecordFirst,
			// Token: 0x04000A63 RID: 2659
			GetFinished,
			// Token: 0x04000A64 RID: 2660
			StartMonitoring,
			// Token: 0x04000A65 RID: 2661
			WaitGetRecord,
			// Token: 0x04000A66 RID: 2662
			DelSwipe,
			// Token: 0x04000A67 RID: 2663
			EndStep
		}
	}
}
