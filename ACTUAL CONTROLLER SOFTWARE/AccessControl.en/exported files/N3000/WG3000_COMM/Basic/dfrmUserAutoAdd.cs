using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000037 RID: 55
	public partial class dfrmUserAutoAdd : frmN3000
	{
		// Token: 0x060003CB RID: 971 RVA: 0x0006C72C File Offset: 0x0006B72C
		public dfrmUserAutoAdd()
		{
			this.InitializeComponent();
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0006C788 File Offset: 0x0006B788
		private int _manualInput()
		{
			int num = -1;
			if (string.IsNullOrEmpty(this.txtStartNO.Text) || string.IsNullOrEmpty(this.txtEndNO.Text))
			{
				XMessageBox.Show(this, CommonStr.strCheckCard, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return num;
			}
			long num2 = long.Parse(this.txtStartNO.Text);
			long num3 = long.Parse(this.txtEndNO.Text);
			if (num2 <= 0L || num3 <= 0L)
			{
				XMessageBox.Show(this, CommonStr.strCheckCard, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return num;
			}
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSureAutoAddCard + ": {0:d}--{1:d} [{2:d}] ?", num2, num3, (num3 - num2 + 1L > 0L) ? (num3 - num2 + 1L) : 1L), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
			{
				return num;
			}
			icConsumer icConsumer = new icConsumer();
			int num4 = int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString());
			string text = "";
			if (this.chkOption.Checked && this.txtNOStartCaption.Text.Trim().Length > 0)
			{
				text = this.txtNOStartCaption.Text.TrimStart(new char[0]);
			}
			long num5 = icConsumer.ConsumerNONext(text);
			if (num5 < 0L)
			{
				XMessageBox.Show(this, wgAppConfig.ReplaceWorkNO(CommonStr.strAutoAddCardErrConsumerNO), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return num;
			}
			int num6 = 0;
			if (num3 - num2 + 1L > 10L)
			{
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
			}
			for (long num7 = num2; num7 <= num3; num7 += 1L)
			{
				if (this.bExit)
				{
					return num;
				}
				string text2;
				if (string.IsNullOrEmpty(text))
				{
					text2 = num5.ToString();
				}
				else if (this.chkConst.Checked && this.nudNOLength.Value - this.txtNOStartCaption.Text.Length > 0m)
				{
					text2 = string.Format("{0}{1}", text, num5.ToString().PadLeft((int)this.nudNOLength.Value - this.txtNOStartCaption.Text.Length, '0'));
				}
				else
				{
					text2 = string.Format("{0}{1}", text, num5.ToString());
				}
				int num8 = icConsumer.addNew(text2.ToString(), "N" + num7.ToString(), num4, 1, 0, 1, DateTime.Now, DateTime.Parse("2099-12-31 23:59:59"), 345678, num7);
				if (num8 >= 0)
				{
					num5 += 1L;
					num6++;
				}
				if (wgTools.gWGYTJ && num8 == -500)
				{
					XMessageBox.Show(this, CommonStr.strUsers_500, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
				if (num6 % 100 == 0)
				{
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num6));
					Application.DoEvents();
				}
			}
			wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num6));
			this.dfrmWait1.Hide();
			this.dfrmWait1.Refresh();
			Application.DoEvents();
			XMessageBox.Show(this, CommonStr.strAutoAddCard + "\r\n\r\n" + num6.ToString(), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return 1;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0006CADB File Offset: 0x0006BADB
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.bExit = true;
			base.Close();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0006CAEA File Offset: 0x0006BAEA
		private void btnCancel2_Click(object sender, EventArgs e)
		{
			this.bExit = true;
			base.Close();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0006CAFC File Offset: 0x0006BAFC
		private void btnDirectGetFromtheController_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.watching != null)
				{
					this.watching.EventHandler -= this.evtNewInfoCallBack;
					this.watching.StopWatch();
				}
				this.watching = null;
			}
			catch (Exception)
			{
			}
			this.btnDirectGetFromtheController.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				using (wgMjControllerPrivilege wgMjControllerPrivilege = new wgMjControllerPrivilege())
				{
					icController.GetInfoFromDBByDoorName(this.cboDoors.Text);
					wgMjControllerPrivilege.AllowDownload();
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
					this.arrlstSwipe.Clear();
					this.lstSwipe.Items.Clear();
					this.label3.Text = this.btnDirectGetFromtheController.Text + " ...";
					wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Start");
					this.Refresh();
					if (wgMjControllerPrivilege.DownloadIP(icController.ControllerSN, icController.IP, icController.PORT, "", ref this.dtPrivilege) > 0)
					{
						if (this.dtPrivilege.Rows.Count >= 0)
						{
							this.lblCount.Text = (this.lstSwipe.Items.Count + this.dtPrivilege.Rows.Count).ToString();
							wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Complete");
							this.label3.Text = CommonStr.strSuccessfully;
							this.Refresh();
							for (int i = 0; i < this.dtPrivilege.Rows.Count; i++)
							{
								if (string.IsNullOrEmpty(wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"])))
								{
									this.lstSwipe.Items.Add(wgAppConfig.displayPartCardNO(this.dtPrivilege.Rows[i]["f_CardNO"].ToString()));
									this.arrlstSwipe.Add(this.dtPrivilege.Rows[i]["f_CardNO"].ToString());
								}
								else
								{
									this.lstSwipe.Items.Add(string.Format("{0}_{1}", wgAppConfig.displayPartCardNO(this.dtPrivilege.Rows[i]["f_CardNO"].ToString()), wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"])));
									this.arrlstSwipe.Add(string.Format("{0}_{1}", this.dtPrivilege.Rows[i]["f_CardNO"].ToString(), wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"])));
								}
							}
						}
						else
						{
							this.label3.Text = CommonStr.strCommFail;
							wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Failed");
						}
					}
					else
					{
						this.label3.Text = CommonStr.strCommFail;
						wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Failed");
					}
					Cursor.Current = Cursors.Default;
					this.btnDirectGetFromtheController.Enabled = true;
				}
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0006D0D8 File Offset: 0x0006C0D8
		private void btnNext_Click(object sender, EventArgs e)
		{
			this.arrlstSwipe.Clear();
			this.lstSwipe.Items.Clear();
			this.lblInfo.Text = "";
			this.lblCount.Text = "";
			try
			{
				string keyVal = wgAppConfig.GetKeyVal("UserAutoAddSet");
				if (!string.IsNullOrEmpty(keyVal) && keyVal.IndexOf(",") > 0)
				{
					string text = keyVal.Substring(0, keyVal.IndexOf(","));
					string text2 = keyVal.Substring(keyVal.IndexOf(",") + 1);
					if (int.Parse(text) > 0)
					{
						this.chkConst.Checked = true;
						this.nudNOLength.Value = int.Parse(text);
						this.nudNOLength.Enabled = true;
					}
					this.txtNOStartCaption.Text = wgTools.SetObjToStr(text2);
					this.chkOption.Checked = true;
					this.groupBox4.Visible = true;
				}
			}
			catch (Exception)
			{
			}
			if (!this.optController.Checked || !string.IsNullOrEmpty(this.cboDoors.Text))
			{
				if (this.optManualInput.Checked)
				{
					this.label3.Visible = false;
					this.groupBox3.Visible = true;
					this.inputMode = 3;
				}
				else
				{
					this.label3.Visible = true;
					this.groupBox3.Visible = false;
					if (this.optUSBReader.Checked)
					{
						this.inputMode = 1;
					}
					else
					{
						this.inputMode = 2;
						this.controllerReaderInput();
						if (wgTools.gWGYTJ)
						{
							this.btnDirectGetFromtheController.Visible = true;
						}
					}
				}
				new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
				int i = this.arrGroupID.Count;
				this.cbof_GroupID.Items.Clear();
				for (i = 0; i < this.arrGroupID.Count; i++)
				{
					this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
				}
				if (this.cbof_GroupID.Items.Count > 0)
				{
					this.cbof_GroupID.SelectedIndex = 0;
				}
				this.groupBox2.Visible = true;
				this.btnCancel.Visible = false;
				this.btnNext.Visible = false;
				this.groupBox1.Visible = false;
				this.groupBox2.Visible = true;
				this.button1.Visible = true;
				this.btnOK.Visible = true;
				this.btnExit.Visible = true;
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0006D374 File Offset: 0x0006C374
		private void btnOK_Click(object sender, EventArgs e)
		{
			int num = -1;
			Cursor.Current = Cursors.WaitCursor;
			if (this.inputMode == 3)
			{
				num = this._manualInput();
			}
			else if (this.inputMode == 1 || this.inputMode == 2)
			{
				num = this.usbReaderInput();
			}
			Cursor.Current = Cursors.Default;
			if (!this.bExit && num >= 0)
			{
				try
				{
					string text = "";
					if (this.chkOption.Checked)
					{
						if (this.chkConst.Checked)
						{
							text = this.nudNOLength.Value.ToString();
						}
						else
						{
							text = "0";
						}
						text = text + "," + this.txtNOStartCaption.Text;
					}
					wgAppConfig.UpdateKeyVal("UserAutoAddSet", text);
				}
				catch (Exception)
				{
				}
				base.DialogResult = DialogResult.OK;
				icConsumerShare.setUpdateLog();
				base.Close();
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0006D454 File Offset: 0x0006C454
		private void button1_Click(object sender, EventArgs e)
		{
			this.groupBox2.Visible = false;
			this.btnCancel.Visible = true;
			this.btnNext.Visible = true;
			this.groupBox1.Visible = true;
			this.groupBox2.Visible = false;
			this.button1.Visible = false;
			this.btnOK.Visible = false;
			this.btnExit.Visible = false;
			if (this.optController.Checked)
			{
				try
				{
					this.watching.EventHandler -= this.evtNewInfoCallBack;
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0006D4FC File Offset: 0x0006C4FC
		private void cboDoors_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cboDoors);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0006D509 File Offset: 0x0006C509
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0006D516 File Offset: 0x0006C516
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0006D534 File Offset: 0x0006C534
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.nudNOLength.Enabled = this.chkConst.Checked;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0006D54C File Offset: 0x0006C54C
		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox4.Visible = this.chkOption.Checked;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0006D564 File Offset: 0x0006C564
		private void controllerReaderInput()
		{
			if (this.watching == null)
			{
				if (this.frmCall == null)
				{
					if (wgTools.bUDPOnly64 > 0)
					{
						this.watching = frmADCT3000.watchingP64;
					}
					else
					{
						this.watching = new WatchingService();
					}
				}
				else
				{
					(this.frmCall as frmUsers).startWatch();
					this.watching = (this.frmCall as frmUsers).watching;
				}
			}
			this.watching.EventHandler += this.evtNewInfoCallBack;
			Dictionary<int, icController> dictionary = new Dictionary<int, icController>();
			this.control = new icController();
			this.control.GetInfoFromDBByDoorName(this.cboDoors.Text);
			this.dvDoors4Watching.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboDoors.Text);
			this.selectedDoorNO = int.Parse(this.dvDoors4Watching[0]["f_DoorNO"].ToString());
			this.selectedControllerSN = this.control.ControllerSN;
			dictionary.Add(this.control.ControllerSN, this.control);
			this.watching.WatchingController = dictionary;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0006D688 File Offset: 0x0006C688
		private void dfrmUserAutoAdd_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0006D6C0 File Offset: 0x0006C6C0
		private void dfrmUserAutoAdd_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.watching != null)
			{
				this.watching.EventHandler -= this.evtNewInfoCallBack;
				if (this.frmCall == null)
				{
					this.watching.StopWatch();
				}
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0006D6F4 File Offset: 0x0006C6F4
		private void dfrmUserAutoAdd_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.inputMode == 1)
			{
				foreach (object obj in base.Controls)
				{
					try
					{
						(obj as Control).ImeMode = ImeMode.Off;
					}
					catch
					{
					}
				}
				if (!e.Control && !e.Alt && !e.Shift && e.KeyValue >= 48 && e.KeyValue <= 57)
				{
					if (this.inputCard.Length == 0)
					{
						this.timer1.Interval = 500;
						this.timer1.Enabled = true;
					}
					this.inputCard += (e.KeyValue - 48).ToString();
					return;
				}
			}
			else
			{
				if (2 == this.inputMode && e.KeyValue == 81 && e.Control && e.Shift)
				{
					this.btnDirectGetFromtheController.Visible = true;
				}
				if (e.KeyValue == 67 && e.Control)
				{
					string text = "";
					for (int i = 0; i < this.lstSwipe.Items.Count; i++)
					{
						text = text + (this.arrlstSwipe[i] as string) + "\r\n";
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
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0006D8A4 File Offset: 0x0006C8A4
		private void dfrmUserAutoAdd_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsActivateCard19)
			{
				this.txtStartNO.Mask = "9999999999999999999";
				this.txtEndNO.Mask = "9999999999999999999";
			}
			else
			{
				this.txtStartNO.Mask = "9999999999";
				this.txtEndNO.Mask = "9999999999";
			}
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.chkOption.Text = wgAppConfig.ReplaceWorkNO(this.chkOption.Text);
			this.txtNOStartCaption.Text = wgAppConfig.ReplaceWorkNO(this.txtNOStartCaption.Text);
			this.loadDoorData();
			this.btnCancel.Visible = true;
			this.btnNext.Visible = true;
			this.groupBox1.Visible = true;
			this.groupBox2.Visible = false;
			this.button1.Visible = false;
			this.btnOK.Visible = false;
			this.btnExit.Visible = false;
			if (this.bAutoAddBySwiping && this.dvDoors.Count > 0)
			{
				int num = -1;
				bool flag = true;
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					if (num == -1)
					{
						num = (int)this.dvDoors[i]["f_ControllerSN"];
					}
					else if (num != (int)this.dvDoors[i]["f_ControllerSN"])
					{
						flag = false;
						break;
					}
				}
				this.optController.Checked = true;
				if (flag)
				{
					this.btnNext.PerformClick();
				}
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0006DA38 File Offset: 0x0006CA38
		private void evtNewInfoCallBack(string text)
		{
			wgTools.WgDebugWrite("Got text through callback! {0}", new object[] { text });
			this.lstSwipe.Invoke(new dfrmUserAutoAdd.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { text });
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0006DA80 File Offset: 0x0006CA80
		private void loadDoorData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadDoorData_Acc();
				return;
			}
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						this.dt = new DataTable();
						this.dvDoors = new DataView(this.dt);
						this.dvDoors4Watching = new DataView(this.dt);
						sqlDataAdapter.Fill(this.dt);
						new icControllerZone().getAllowedControllers(ref this.dt);
						try
						{
							this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						this.cboDoors.Items.Clear();
						if (this.dvDoors.Count > 0)
						{
							for (int i = 0; i < this.dvDoors.Count; i++)
							{
								this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
							}
							if (this.cboDoors.Items.Count > 0)
							{
								this.cboDoors.SelectedIndex = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0006DC5C File Offset: 0x0006CC5C
		private void loadDoorData_Acc()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						this.dt = new DataTable();
						this.dvDoors = new DataView(this.dt);
						this.dvDoors4Watching = new DataView(this.dt);
						oleDbDataAdapter.Fill(this.dt);
						new icControllerZone().getAllowedControllers(ref this.dt);
						try
						{
							this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						this.cboDoors.Items.Clear();
						if (this.dvDoors.Count > 0)
						{
							for (int i = 0; i < this.dvDoors.Count; i++)
							{
								this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
							}
							if (this.cboDoors.Items.Count > 0)
							{
								this.cboDoors.SelectedIndex = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0006DE28 File Offset: 0x0006CE28
		private void optController_CheckedChanged(object sender, EventArgs e)
		{
			this.cboDoors.Enabled = this.optController.Checked;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0006DE40 File Offset: 0x0006CE40
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			if (this.inputCard.Length >= 8)
			{
				bool flag = false;
				foreach (object obj in this.arrlstSwipe)
				{
					if (obj as string == this.inputCard)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.arrlstSwipe.Add(this.inputCard);
					this.lstSwipe.Items.Add(wgAppConfig.displayPartCardNO(this.inputCard));
					this.lblInfo.Text = wgAppConfig.displayPartCardNO(this.inputCard);
				}
				else
				{
					this.lblInfo.Text = wgAppConfig.displayPartCardNO(this.inputCard) + CommonStr.strCardNOIsAdded;
				}
				this.lblCount.Text = this.lstSwipe.Items.Count.ToString();
			}
			this.inputCard = "";
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0006DF5C File Offset: 0x0006CF5C
		private void txtEndNO_KeyPress(object sender, KeyPressEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtEndNO);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0006DF69 File Offset: 0x0006CF69
		private void txtEndNO_KeyUp(object sender, KeyEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtEndNO);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0006DF78 File Offset: 0x0006CF78
		private void txtInfoUpdateEntry(object info)
		{
			MjRec mjRec = new MjRec(info as string);
			if (mjRec.ControllerSN > 0U && (ulong)mjRec.ControllerSN == (ulong)((long)this.selectedControllerSN))
			{
				InfoRow infoRow = new InfoRow();
				infoRow.category = mjRec.eventCategory;
				infoRow.desc = "";
				if (mjRec.IsSwipeRecord)
				{
					bool flag = false;
					string text = mjRec.CardID.ToString();
					foreach (object obj in this.arrlstSwipe)
					{
						if (obj as string == text)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.arrlstSwipe.Add(text);
						this.lstSwipe.Items.Add(wgAppConfig.displayPartCardNO(text));
						this.lblInfo.Text = wgAppConfig.displayPartCardNO(text);
					}
					else
					{
						this.lblInfo.Text = wgAppConfig.displayPartCardNO(text) + CommonStr.strCardNOIsAdded;
					}
					this.lblCount.Text = this.lstSwipe.Items.Count.ToString();
				}
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0006E0C0 File Offset: 0x0006D0C0
		private void txtStartNO_KeyPress(object sender, KeyPressEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtStartNO);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0006E0CD File Offset: 0x0006D0CD
		private void txtStartNO_KeyUp(object sender, KeyEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtStartNO);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0006E0DC File Offset: 0x0006D0DC
		private int usbReaderInput()
		{
			int num = -1;
			if (this.lstSwipe.Items.Count <= 0)
			{
				return num;
			}
			icConsumer icConsumer = new icConsumer();
			int num2 = int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString());
			string text = "";
			if (this.chkOption.Checked && this.txtNOStartCaption.Text.Trim().Length > 0)
			{
				text = this.txtNOStartCaption.Text.TrimStart(new char[0]);
			}
			long num3 = icConsumer.ConsumerNONext(text);
			int num4 = 0;
			if (num3 < 0L)
			{
				XMessageBox.Show(this, wgAppConfig.ReplaceWorkNO(CommonStr.strAutoAddCardErrConsumerNO), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return num;
			}
			if (this.lstSwipe.Items.Count > 10)
			{
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
			}
			for (int i = 0; i < this.arrlstSwipe.Count; i++)
			{
				if (this.bExit)
				{
					return num;
				}
				int num6;
				if ((this.arrlstSwipe[i] as string).IndexOf("_") <= 0)
				{
					long num5 = long.Parse(this.arrlstSwipe[i] as string);
					string text2;
					if (string.IsNullOrEmpty(text))
					{
						text2 = num3.ToString();
					}
					else if (this.chkConst.Checked && this.nudNOLength.Value - this.txtNOStartCaption.Text.Length > 0m)
					{
						text2 = string.Format("{0}{1}", text, num3.ToString().PadLeft((int)this.nudNOLength.Value - this.txtNOStartCaption.Text.Length, '0'));
					}
					else
					{
						text2 = string.Format("{0}{1}", text, num3.ToString());
					}
					num6 = icConsumer.addNew(text2.ToString(), "N" + num5.ToString(), num2, 1, 0, 1, DateTime.Now, DateTime.Parse("2099-12-31 23:59:59"), 345678, num5);
				}
				else
				{
					long num5 = long.Parse((this.arrlstSwipe[i] as string).Substring(0, (this.arrlstSwipe[i] as string).IndexOf("_")));
					string text3 = (this.arrlstSwipe[i] as string).Substring((this.arrlstSwipe[i] as string).IndexOf("_") + 1);
					if (string.IsNullOrEmpty(text3))
					{
						text3 = "N" + num5.ToString();
					}
					num6 = icConsumer.addNew(num3.ToString(), text3, num2, 1, 0, 1, DateTime.Now, DateTime.Parse("2099-12-31 23:59:59"), 345678, num5);
				}
				if (num6 >= 0)
				{
					num3 += 1L;
					num4++;
				}
				if (wgTools.gWGYTJ && num6 == -500)
				{
					XMessageBox.Show(this, CommonStr.strUsers_500, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
				if (num4 % 100 == 0)
				{
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num4));
					Application.DoEvents();
				}
			}
			wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num4));
			this.dfrmWait1.Hide();
			this.dfrmWait1.Refresh();
			Application.DoEvents();
			XMessageBox.Show(this, CommonStr.strAutoAddCard + "\r\n\r\n" + num4.ToString(), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return 1;
		}

		// Token: 0x04000747 RID: 1863
		private const int Mode_ControllerReader = 2;

		// Token: 0x04000748 RID: 1864
		private const int Mode_ManualInput = 3;

		// Token: 0x04000749 RID: 1865
		private const int Mode_USBReader = 1;

		// Token: 0x0400074B RID: 1867
		private bool bExit;

		// Token: 0x0400074C RID: 1868
		private int inputMode;

		// Token: 0x0400074D RID: 1869
		private int selectedControllerSN;

		// Token: 0x0400074E RID: 1870
		private int selectedDoorNO;

		// Token: 0x04000750 RID: 1872
		private DataTable dt;

		// Token: 0x04000751 RID: 1873
		private DataTable dtPrivilege;

		// Token: 0x04000752 RID: 1874
		private DataView dvDoors;

		// Token: 0x04000753 RID: 1875
		private DataView dvDoors4Watching;

		// Token: 0x04000754 RID: 1876
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04000755 RID: 1877
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04000756 RID: 1878
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04000757 RID: 1879
		private ArrayList arrlstSwipe = new ArrayList();

		// Token: 0x04000758 RID: 1880
		public bool bAutoAddBySwiping;

		// Token: 0x0400075A RID: 1882
		public Form frmCall;

		// Token: 0x0400075B RID: 1883
		private string inputCard = "";

		// Token: 0x02000038 RID: 56
		// (Invoke) Token: 0x060003EB RID: 1003
		private delegate void txtInfoUpdate(object info);
	}
}
