using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000242 RID: 578
	public partial class dfrmFirstCard : frmN3000
	{
		// Token: 0x060011C5 RID: 4549 RVA: 0x0014DBA0 File Offset: 0x0014CBA0
		public dfrmFirstCard()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0014DC08 File Offset: 0x0014CC08
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

		// Token: 0x060011C7 RID: 4551 RVA: 0x0014DC4C File Offset: 0x0014CC4C
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
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0014DCCC File Offset: 0x0014CCCC
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			if (this.strGroupFilter == "")
			{
				string rowFilter = this.dv1.RowFilter;
				string rowFilter2 = this.dv2.RowFilter;
				this.dv1.Dispose();
				this.dv2.Dispose();
				this.dv1 = null;
				this.dv2 = null;
				this.dt.BeginLoadData();
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 1;
				}
				this.dt.EndLoadData();
				this.dv1 = new DataView(this.dt);
				this.dv1.RowFilter = rowFilter;
				this.dv2 = new DataView(this.dt);
				this.dv2.RowFilter = rowFilter2;
			}
			else
			{
				this.dv = new DataView(this.dt);
				this.dv.RowFilter = this.strGroupFilter;
				for (int j = 0; j < this.dv.Count; j++)
				{
					this.dv[j]["f_Selected"] = 1;
				}
			}
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			wgTools.WriteLine("btnAddAllUsers_Click End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0014DEB5 File Offset: 0x0014CEB5
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0014DEC2 File Offset: 0x0014CEC2
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0014DED4 File Offset: 0x0014CED4
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				this.dt = ((DataView)this.dgvUsers.DataSource).Table;
				this.dv1 = (DataView)this.dgvUsers.DataSource;
				this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
				this.dgvUsers.DataSource = null;
				this.dgvSelectedUsers.DataSource = null;
				string rowFilter = this.dv1.RowFilter;
				string rowFilter2 = this.dv2.RowFilter;
				this.dv1.Dispose();
				this.dv2.Dispose();
				this.dv1 = null;
				this.dv2 = null;
				this.dt.BeginLoadData();
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 0;
				}
				this.dt.EndLoadData();
				this.dv1 = new DataView(this.dt);
				this.dv1.RowFilter = rowFilter;
				this.dv2 = new DataView(this.dt);
				this.dv2.RowFilter = rowFilter2;
				this.dgvUsers.DataSource = this.dv1;
				this.dgvSelectedUsers.DataSource = this.dv2;
				wgTools.WriteLine("btnDelAllUsers_Click End");
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0014E068 File Offset: 0x0014D068
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0014E078 File Offset: 0x0014D078
		private void btnOK_Click(object sender, EventArgs e)
		{
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			int num = 0;
			if (this.chkMonday.Checked)
			{
				num |= 1;
			}
			if (this.chkTuesday.Checked)
			{
				num |= 2;
			}
			if (this.chkWednesday.Checked)
			{
				num |= 4;
			}
			if (this.chkThursday.Checked)
			{
				num |= 8;
			}
			if (this.chkFriday.Checked)
			{
				num |= 16;
			}
			if (this.chkSaturday.Checked)
			{
				num |= 32;
			}
			if (this.chkSunday.Checked)
			{
				num |= 64;
			}
			string text = string.Concat(new object[]
			{
				"update t_b_door set f_FirstCard_Enabled =",
				this.chkActive.Checked ? 1 : 0,
				", f_FirstCard_BeginHMS=",
				wgTools.PrepareStr(this.dateBeginHMS1.Value.ToString(wgTools.YMDHMSFormat), false, ""),
				", f_FirstCard_BeginControl =",
				this.cboBeginControlStatus.SelectedIndex,
				", f_FirstCard_EndHMS=",
				wgTools.PrepareStr(this.dateEndHMS1.Value.ToString(wgTools.YMDHMSFormat), false, ""),
				", f_FirstCard_EndControl=",
				this.cboEndControlStatus.SelectedIndex,
				", f_FirstCard_Weekday=",
				num,
				" Where f_DoorID = ",
				this.DoorID
			});
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.ExecuteNonQuery();
					}
					goto IL_0210;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					sqlCommand.ExecuteNonQuery();
				}
			}
			IL_0210:
			text = " Delete  FROM t_d_doorFirstCardUsers  WHERE f_DoorID= " + this.DoorID;
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
					{
						oleDbConnection2.Open();
						oleDbCommand2.ExecuteNonQuery();
					}
					goto IL_02AF;
				}
			}
			using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
				{
					sqlConnection2.Open();
					sqlCommand2.ExecuteNonQuery();
				}
			}
			IL_02AF:
			if (this.chkActive.Checked)
			{
				if (this.dgvSelectedUsers.DataSource != null)
				{
					using (DataView dataView = this.dgvSelectedUsers.DataSource as DataView)
					{
						if (dataView.Count > 0)
						{
							int i = 0;
							while (i <= dataView.Count - 1)
							{
								text = "INSERT INTO [t_d_doorFirstCardUsers](f_ConsumerID, f_DoorID )";
								text = string.Concat(new string[]
								{
									text,
									" VALUES( ",
									dataView[i]["f_ConsumerID"].ToString(),
									", ",
									this.DoorID.ToString(),
									")"
								});
								if (wgAppConfig.IsAccessDB)
								{
									using (OleDbConnection oleDbConnection3 = new OleDbConnection(wgAppConfig.dbConString))
									{
										using (OleDbCommand oleDbCommand3 = new OleDbCommand(text, oleDbConnection3))
										{
											oleDbConnection3.Open();
											oleDbCommand3.ExecuteNonQuery();
										}
										goto IL_03E0;
									}
									goto IL_039F;
								}
								goto IL_039F;
								IL_03E0:
								i++;
								continue;
								IL_039F:
								using (SqlConnection sqlConnection3 = new SqlConnection(wgAppConfig.dbConString))
								{
									using (SqlCommand sqlCommand3 = new SqlCommand(text, sqlConnection3))
									{
										sqlConnection3.Open();
										sqlCommand3.ExecuteNonQuery();
									}
								}
								goto IL_03E0;
							}
						}
					}
				}
				this.retValue = "1";
			}
			else
			{
				this.retValue = "0";
			}
			base.DialogResult = DialogResult.OK;
			this.Cursor = cursor;
			base.Close();
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0014E5F4 File Offset: 0x0014D5F4
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0014E604 File Offset: 0x0014D604
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = "f_Selected = 0";
					this.strGroupFilter = "";
					return;
				}
				dataView.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
				this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
				int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
				int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
				int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
				if (num2 > 0)
				{
					if (num2 >= groupChildMaxNo)
					{
						dataView.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num);
						this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
						return;
					}
					dataView.RowFilter = "f_Selected = 0 ";
					string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
					dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
					this.strGroupFilter = string.Format("  {0} ", groupQuery);
				}
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0014E7B4 File Offset: 0x0014D7B4
		private void chkActive_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActive.Checked)
			{
				this.grpBegin.Visible = true;
				this.grpEnd.Visible = true;
				this.grpUsers.Visible = true;
				this.grpWeekdayControl.Visible = true;
				if (this.dgvUsers.DataSource != null)
				{
					DataView dataView = (DataView)this.dgvUsers.DataSource;
					this.dgvUsers.DataSource = null;
					this.dgvUsers.DataSource = dataView;
					return;
				}
			}
			else
			{
				this.grpBegin.Visible = false;
				this.grpEnd.Visible = false;
				this.grpUsers.Visible = false;
				this.grpWeekdayControl.Visible = false;
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0014E865 File Offset: 0x0014D865
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0014E87C File Offset: 0x0014D87C
		private void dfrm_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
					}
					this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0014E8EC File Offset: 0x0014D8EC
		private void dfrmFirstCard_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmFirstCard_Load_Acc(sender, e);
				return;
			}
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("08:00:00");
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS1.Value = DateTime.Parse("08:00:00");
			this.loadGroupData();
			string text = "SELECT t_b_Door.*  FROM  t_b_Door  ";
			text = text + " Where  f_DoorID = " + this.DoorID;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						this.controllerID = (int)sqlDataReader["f_ControllerID"];
						this.doorNo = int.Parse(sqlDataReader["f_DoorNo"].ToString());
						this.dateBeginHMS1.Value = wgTools.wgDateTimeParse(sqlDataReader["f_FirstCard_BeginHMS"]);
						this.dateEndHMS1.Value = wgTools.wgDateTimeParse(sqlDataReader["f_FirstCard_EndHMS"]);
						try
						{
							this.cboBeginControlStatus.SelectedIndex = (((int)sqlDataReader["f_FirstCard_BeginControl"] >= 0 && (int)sqlDataReader["f_FirstCard_BeginControl"] < 4) ? ((int)sqlDataReader["f_FirstCard_BeginControl"]) : 0);
							this.cboEndControlStatus.SelectedIndex = (((int)sqlDataReader["f_FirstCard_EndControl"] >= 0 && (int)sqlDataReader["f_FirstCard_EndControl"] < 4) ? ((int)sqlDataReader["f_FirstCard_EndControl"]) : 0);
						}
						catch (Exception)
						{
							if (this.cboBeginControlStatus.Items.Count > 0)
							{
								this.cboBeginControlStatus.SelectedIndex = 0;
							}
							if (this.cboEndControlStatus.Items.Count > 0)
							{
								this.cboEndControlStatus.SelectedIndex = 0;
							}
						}
						if (wgTools.SetObjToStr(sqlDataReader["f_FirstCard_Weekday"]) != "")
						{
							this.chkMonday.Checked = ((int)sqlDataReader["f_FirstCard_Weekday"] & 1) > 0;
							this.chkTuesday.Checked = ((int)sqlDataReader["f_FirstCard_Weekday"] & 2) > 0;
							this.chkWednesday.Checked = ((int)sqlDataReader["f_FirstCard_Weekday"] & 4) > 0;
							this.chkThursday.Checked = ((int)sqlDataReader["f_FirstCard_Weekday"] & 8) > 0;
							this.chkFriday.Checked = ((int)sqlDataReader["f_FirstCard_Weekday"] & 16) > 0;
							this.chkSaturday.Checked = ((int)sqlDataReader["f_FirstCard_Weekday"] & 32) > 0;
							this.chkSunday.Checked = ((int)sqlDataReader["f_FirstCard_Weekday"] & 64) > 0;
						}
						if ((int)sqlDataReader["f_FirstCard_Enabled"] > 0)
						{
							this.chkActive.Checked = true;
						}
						else
						{
							this.chkActive.Checked = false;
						}
						this.chkActive_CheckedChanged(null, null);
					}
					sqlDataReader.Close();
				}
			}
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0014ED48 File Offset: 0x0014DD48
		private void dfrmFirstCard_Load_Acc(object sender, EventArgs e)
		{
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("08:00:00");
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS1.Value = DateTime.Parse("08:00:00");
			this.loadGroupData();
			string text = "SELECT t_b_Door.*  FROM  t_b_Door  ";
			text = text + " Where  f_DoorID = " + this.DoorID;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					if (oleDbDataReader.Read())
					{
						this.controllerID = (int)oleDbDataReader["f_ControllerID"];
						this.doorNo = int.Parse(oleDbDataReader["f_DoorNo"].ToString());
						try
						{
							this.dateBeginHMS1.Value = wgTools.wgDateTimeParse(oleDbDataReader["f_FirstCard_BeginHMS"]);
							this.dateEndHMS1.Value = wgTools.wgDateTimeParse(oleDbDataReader["f_FirstCard_EndHMS"]);
							this.cboBeginControlStatus.SelectedIndex = (((int)oleDbDataReader["f_FirstCard_BeginControl"] >= 0 && (int)oleDbDataReader["f_FirstCard_BeginControl"] < 4) ? ((int)oleDbDataReader["f_FirstCard_BeginControl"]) : 0);
							this.cboEndControlStatus.SelectedIndex = (((int)oleDbDataReader["f_FirstCard_EndControl"] >= 0 && (int)oleDbDataReader["f_FirstCard_EndControl"] < 4) ? ((int)oleDbDataReader["f_FirstCard_EndControl"]) : 0);
						}
						catch (Exception)
						{
							if (this.cboBeginControlStatus.Items.Count > 0)
							{
								this.cboBeginControlStatus.SelectedIndex = 0;
							}
							if (this.cboEndControlStatus.Items.Count > 0)
							{
								this.cboEndControlStatus.SelectedIndex = 0;
							}
						}
						if (wgTools.SetObjToStr(oleDbDataReader["f_FirstCard_Weekday"]) != "")
						{
							this.chkMonday.Checked = ((int)oleDbDataReader["f_FirstCard_Weekday"] & 1) > 0;
							this.chkTuesday.Checked = ((int)oleDbDataReader["f_FirstCard_Weekday"] & 2) > 0;
							this.chkWednesday.Checked = ((int)oleDbDataReader["f_FirstCard_Weekday"] & 4) > 0;
							this.chkThursday.Checked = ((int)oleDbDataReader["f_FirstCard_Weekday"] & 8) > 0;
							this.chkFriday.Checked = ((int)oleDbDataReader["f_FirstCard_Weekday"] & 16) > 0;
							this.chkSaturday.Checked = ((int)oleDbDataReader["f_FirstCard_Weekday"] & 32) > 0;
							this.chkSunday.Checked = ((int)oleDbDataReader["f_FirstCard_Weekday"] & 64) > 0;
						}
						if ((int)oleDbDataReader["f_FirstCard_Enabled"] > 0)
						{
							this.chkActive.Checked = true;
						}
						else
						{
							this.chkActive.Checked = false;
						}
						this.chkActive_CheckedChanged(null, null);
					}
					oleDbDataReader.Close();
				}
			}
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0014F160 File Offset: 0x0014E160
		private void loadGroupData()
		{
			new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
			for (int i = 0; i < this.arrGroupID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
				{
					this.cbof_GroupID.Items.Add(CommonStr.strAll);
				}
				else
				{
					this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
				}
			}
			if (this.cbof_GroupID.Items.Count > 0)
			{
				this.cbof_GroupID.SelectedIndex = 0;
			}
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0014F214 File Offset: 0x0014E214
		private DataTable loadUserData4BackWork()
		{
			Thread.Sleep(100);
			wgTools.WriteLine("loadUserData Start");
			this.dtUser1 = new DataTable();
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT  t_b_Consumer.f_ConsumerID ";
				text = text + " , f_ConsumerNO, f_ConsumerName, f_CardNO  , IIF ( f_doorFirstCardUsersId IS NULL , 0 , 1 ) AS f_Selected  , f_GroupID  FROM t_b_Consumer " + string.Format(" LEFT OUTER JOIN t_d_doorFirstCardUsers ON ( t_b_Consumer.f_ConsumerID=t_d_doorFirstCardUsers.f_ConsumerID AND f_DoorID= {0})", this.DoorID.ToString()) + " WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUser1);
						}
					}
					goto IL_0130;
				}
			}
			text = " SELECT  t_b_Consumer.f_ConsumerID ";
			text = string.Concat(new object[] { text, " , f_ConsumerNO, f_ConsumerName, f_CardNO  , CASE WHEN f_doorFirstCardUsersId IS NULL THEN 0 ELSE 1 END AS f_Selected  , f_GroupID  FROM t_b_Consumer  LEFT OUTER JOIN t_d_doorFirstCardUsers ON t_b_Consumer.f_ConsumerID=t_d_doorFirstCardUsers.f_ConsumerID AND f_DoorID= ", this.DoorID, " WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC " });
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtUser1);
					}
				}
			}
			IL_0130:
			wgTools.WriteLine("da.Fill End");
			try
			{
				this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			dfrmFirstCard.lastLoadUsers = icConsumerShare.getUpdateLog();
			dfrmFirstCard.dtLastLoad = this.dtUser1;
			return this.dtUser1;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0014F400 File Offset: 0x0014E400
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dvSelected.Sort = " f_ConsumerNo ASC ";
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
				this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0014F510 File Offset: 0x0014E510
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.dgvUsers.DataSource == null)
			{
				Cursor.Current = Cursors.WaitCursor;
				return;
			}
			this.timer1.Enabled = false;
			Cursor.Current = Cursors.Default;
			this.lblWait.Visible = false;
			this.grpUsers.Enabled = true;
			this.btnOK.Enabled = true;
		}

		// Token: 0x04001FEA RID: 8170
		private const int MoreCardGroupMaxLen = 9;

		// Token: 0x04001FEB RID: 8171
		private int controllerID;

		// Token: 0x04001FEC RID: 8172
		private int doorNo;

		// Token: 0x04001FED RID: 8173
		private DataTable dt;

		// Token: 0x04001FEE RID: 8174
		private DataTable dtUser1;

		// Token: 0x04001FEF RID: 8175
		private DataView dv;

		// Token: 0x04001FF0 RID: 8176
		private DataView dv1;

		// Token: 0x04001FF1 RID: 8177
		private DataView dv2;

		// Token: 0x04001FF2 RID: 8178
		private DataView dvSelected;

		// Token: 0x04001FF3 RID: 8179
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04001FF4 RID: 8180
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04001FF5 RID: 8181
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04001FF9 RID: 8185
		public int DoorID;

		// Token: 0x04001FFA RID: 8186
		private static DataTable dtLastLoad;

		// Token: 0x04001FFD RID: 8189
		private static string lastLoadUsers = "";

		// Token: 0x04001FFE RID: 8190
		public string retValue = "0";

		// Token: 0x04001FFF RID: 8191
		private string strGroupFilter = "";
	}
}
