using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000309 RID: 777
	public partial class dfrmPatrolTaskAutoPlan : frmN3000
	{
		// Token: 0x06001768 RID: 5992 RVA: 0x001E78D4 File Offset: 0x001E68D4
		public dfrmPatrolTaskAutoPlan()
		{
			this.InitializeComponent();
			this.ProgressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x001E794C File Offset: 0x001E694C
		private void _dataTableLoad()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this._dataTableLoad_Acc();
				return;
			}
			this.con = new SqlConnection(wgAppConfig.dbConString);
			this.dsConsumers = new DataSet("Users");
			string text = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM ([t_b_Consumer] INNER JOIN t_d_PatrolUsers ON (t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID) )  LEFT OUTER JOIN t_b_Group ON ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID )  ";
			this.cmd = new SqlCommand(text, this.con);
			this.daConsumers = new SqlDataAdapter(this.cmd);
			try
			{
				this.dsConsumers.Clear();
				this.daConsumers.Fill(this.dsConsumers, "Consumers");
				this.dtConsumers = this.dsConsumers.Tables["Consumers"];
				this.dvConsumers = new DataView(this.dtConsumers);
				this.dvConsumers.RowFilter = "";
				try
				{
					DataColumn[] array = new DataColumn[2];
					array[0] = this.dtConsumers.Columns["f_UserFullName"];
					this.dtConsumers.PrimaryKey = array;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				this.dtConsumers.AcceptChanges();
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			this.loadGroupData();
			try
			{
				if (wgAppConfig.IsAccessDB)
				{
					this.cmd = new SqlCommand("SELECT [f_RouteID] & '-' & [f_RouteName] as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", this.con);
				}
				else
				{
					this.cmd = new SqlCommand("SELECT CONVERT(nvarchar(50),[f_RouteID]) + case when [f_RouteName] IS NULL Then '' ELSE   '-' + [f_RouteName] end  as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", this.con);
				}
				try
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(this.cmd))
					{
						sqlDataAdapter.Fill(this.dsConsumers, "OptionalShift");
					}
					this.dtOptionalShift = this.dsConsumers.Tables["OptionalShift"];
					this.arrShiftID.Clear();
					this.lstOptionalShifts.Items.Clear();
					if (this.dtOptionalShift.Rows.Count > 0)
					{
						this.arrShiftID.Add(0);
						this.lstOptionalShifts.Items.Add("0*-" + CommonStr.strRest);
						for (int i = 0; i <= this.dtOptionalShift.Rows.Count - 1; i++)
						{
							this.lstOptionalShifts.Items.Add(this.dtOptionalShift.Rows[i][0]);
							this.arrShiftID.Add(this.dtOptionalShift.Rows[i][1]);
						}
					}
				}
				catch (Exception ex3)
				{
					wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
				}
				finally
				{
					this.con.Close();
				}
			}
			catch (Exception ex4)
			{
				wgTools.WgDebugWrite(ex4.ToString(), new object[0]);
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x001E7CA0 File Offset: 0x001E6CA0
		private void _dataTableLoad_Acc()
		{
			OleDbConnection oleDbConnection = null;
			oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			this.dsConsumers = new DataSet("Users");
			string text = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM ([t_b_Consumer] INNER JOIN t_d_PatrolUsers ON (t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID) )  LEFT OUTER JOIN t_b_Group ON  ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID ) ";
			OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection);
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
			try
			{
				this.dsConsumers.Clear();
				oleDbDataAdapter.Fill(this.dsConsumers, "Consumers");
				this.dtConsumers = this.dsConsumers.Tables["Consumers"];
				this.dvConsumers = new DataView(this.dtConsumers);
				this.dvConsumers.RowFilter = "";
				try
				{
					DataColumn[] array = new DataColumn[2];
					array[0] = this.dtConsumers.Columns["f_UserFullName"];
					this.dtConsumers.PrimaryKey = array;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				this.dtConsumers.AcceptChanges();
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			this.loadGroupData();
			try
			{
				if (wgAppConfig.IsAccessDB)
				{
					oleDbCommand = new OleDbCommand("SELECT [f_RouteID] & '-' & [f_RouteName] as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", oleDbConnection);
				}
				else
				{
					oleDbCommand = new OleDbCommand("SELECT CONVERT(nvarchar(50),[f_RouteID]) + case when [f_RouteName] IS NULL Then '' ELSE   '-' + [f_RouteName] end  as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", oleDbConnection);
				}
				try
				{
					using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand))
					{
						oleDbDataAdapter2.Fill(this.dsConsumers, "OptionalShift");
					}
					this.dtOptionalShift = this.dsConsumers.Tables["OptionalShift"];
					this.arrShiftID.Clear();
					this.lstOptionalShifts.Items.Clear();
					if (this.dtOptionalShift.Rows.Count > 0)
					{
						this.arrShiftID.Add(0);
						this.lstOptionalShifts.Items.Add("0*-" + CommonStr.strRest);
						for (int i = 0; i <= this.dtOptionalShift.Rows.Count - 1; i++)
						{
							this.lstOptionalShifts.Items.Add(this.dtOptionalShift.Rows[i][0]);
							this.arrShiftID.Add(this.dtOptionalShift.Rows[i][1]);
						}
					}
				}
				catch (Exception ex3)
				{
					wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
				}
				finally
				{
					oleDbConnection.Close();
				}
			}
			catch (Exception ex4)
			{
				wgTools.WgDebugWrite(ex4.ToString(), new object[0]);
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x001E7FB4 File Offset: 0x001E6FB4
		private void btnAddOne_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.lstOptionalShifts.SelectedIndex;
			if (selectedIndex >= 0)
			{
				this.lstSelectedShifts.Items.Add(this.lstOptionalShifts.Items[selectedIndex]);
				this.arrSelectedShiftID.Add(this.arrShiftID[selectedIndex]);
				if (this.lstSelectedShifts.Items.Count == 0 || this.dvConsumers.Count <= 0)
				{
					this.btnOK.Enabled = false;
					return;
				}
				this.btnOK.Enabled = true;
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x001E8044 File Offset: 0x001E7044
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x001E804C File Offset: 0x001E704C
		private void btnDeleteAll_Click(object sender, EventArgs e)
		{
			this.lstSelectedShifts.Items.Clear();
			this.arrSelectedShiftID.Clear();
			if (this.lstSelectedShifts.Items.Count == 0)
			{
				this.btnOK.Enabled = false;
				return;
			}
			this.btnOK.Enabled = true;
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x001E80A0 File Offset: 0x001E70A0
		private void btnDeleteOne_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.lstSelectedShifts.SelectedIndex;
			if (selectedIndex >= 0)
			{
				this.lstSelectedShifts.Items.RemoveAt(selectedIndex);
				this.arrSelectedShiftID.RemoveAt(selectedIndex);
				if (this.lstSelectedShifts.Items.Count == 0)
				{
					this.btnOK.Enabled = false;
					return;
				}
				this.btnOK.Enabled = true;
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x001E8108 File Offset: 0x001E7108
		private void btnOK_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				if (this.arrSelectedShiftID.Count > 0)
				{
					int[] array = new int[this.arrSelectedShiftID.Count - 1 + 1];
					int[] array2;
					if (this.cbof_ConsumerName.Text == CommonStr.strAll)
					{
						if (this.dvConsumers.Count <= 0)
						{
							XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						array2 = new int[this.dvConsumers.Count - 1 + 1];
						for (int i = 0; i <= this.dvConsumers.Count - 1; i++)
						{
							array2[i] = (int)this.dvConsumers[i]["f_ConsumerID"];
						}
					}
					else
					{
						array2 = new int[] { (int)this.dvConsumers[this.cbof_ConsumerName.SelectedIndex - 1]["f_ConsumerID"] };
					}
					DateTime value = this.dtpStartDate.Value;
					DateTime value2 = this.dtpEndDate.Value;
					for (int i = 0; i <= this.arrSelectedShiftID.Count - 1; i++)
					{
						array[i] = (int)this.arrSelectedShiftID[i];
					}
					if (wgAppConfig.IsAccessDB)
					{
						using (comPatrol_Acc comPatrol_Acc = new comPatrol_Acc())
						{
							int num = 0;
							if (num == 0)
							{
								this.ProgressBar1.Maximum = array2.Length;
								for (int i = 0; i <= array2.Length - 1; i++)
								{
									this.ProgressBar1.Value = i;
									num = comPatrol_Acc.shift_arrangeByRule(array2[i], value, value2, array.Length, array);
									if (num != 0)
									{
										XMessageBox.Show(this, comPatrol_Acc.errDesc(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
										break;
									}
								}
								if (num == 0)
								{
									this.ProgressBar1.Value = this.ProgressBar1.Maximum;
									XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								}
							}
							goto IL_02B4;
						}
					}
					using (comPatrol comPatrol = new comPatrol())
					{
						int num = 0;
						if (num == 0)
						{
							this.ProgressBar1.Maximum = array2.Length;
							for (int i = 0; i <= array2.Length - 1; i++)
							{
								this.ProgressBar1.Value = i;
								num = comPatrol.shift_arrangeByRule(array2[i], value, value2, array.Length, array);
								if (num != 0)
								{
									XMessageBox.Show(this, comPatrol.errDesc(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									break;
								}
							}
							if (num == 0)
							{
								this.ProgressBar1.Value = this.ProgressBar1.Maximum;
								XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							}
						}
						else
						{
							XMessageBox.Show(this, comPatrol.errDesc(num) + "\r\n\r\n" + comPatrol.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
					}
				}
				IL_02B4:;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			this.ProgressBar1.Value = 0;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x001E8444 File Offset: 0x001E7444
		private void cbof_ConsumerName_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ConsumerName);
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x001E8451 File Offset: 0x001E7451
		private void cbof_ConsumerName_Leave(object sender, EventArgs e)
		{
			this.checkUserValid(this.cbof_ConsumerName);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x001E8460 File Offset: 0x001E7460
		private void cbof_Group_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_Group);
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x001E8470 File Offset: 0x001E7470
		private void cbof_Group_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_Group, this.cbof_Group.Text);
			try
			{
				if (this.cbof_Group.SelectedIndex == 0 && this.arrGroupID[0].ToString() == "0")
				{
					this.dvConsumers.RowFilter = "";
				}
				else
				{
					this.dvConsumers.RowFilter = string.Concat(new string[]
					{
						" (f_GroupName = '",
						this.cbof_Group.Text,
						"' ) OR (f_GroupName like '",
						this.cbof_Group.Text,
						"\\%')"
					});
				}
				this.cbof_ConsumerName.Items.Clear();
				this.cbof_ConsumerName.Items.Add(CommonStr.strAll);
				this.arrConsumerCMIndex.Add("");
				for (int i = 0; i <= this.dvConsumers.Count - 1; i++)
				{
					this.cbof_ConsumerName.Items.Add(this.dvConsumers[i]["f_UserFullName"]);
					this.arrConsumerCMIndex.Add(i);
				}
				if (this.cbof_ConsumerName.Items.Count > 0)
				{
					this.cbof_ConsumerName.SelectedIndex = 0;
				}
				if (this.dvConsumers.Count <= 0)
				{
					this.btnOK.Enabled = false;
				}
				else if (this.lstSelectedShifts.Items.Count > 0)
				{
					this.btnOK.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x001E8638 File Offset: 0x001E7638
		public bool checkUserValid(ComboBox cbo)
		{
			try
			{
				string text = cbo.Text.ToUpper();
				int num = cbo.SelectedIndex;
				if (num < 0 || cbo.Text != cbo.Items[num].ToString())
				{
					num = -1;
					for (int i = 0; i < cbo.Items.Count; i++)
					{
						object obj = cbo.Items[i];
						if (Strings.UCase(wgTools.SetObjToStr(obj)).IndexOf(text) >= 0)
						{
							cbo.SelectedItem = cbo.Items[i];
							cbo.SelectedIndex = i;
							num = i;
							break;
						}
					}
					if (num < 0)
					{
						XMessageBox.Show(this, CommonStr.strUserNonexisted, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
					cbo.SelectedIndex = num;
				}
				return true;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return false;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x001E873C File Offset: 0x001E773C
		private void dfrmAutoShift_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x001E8754 File Offset: 0x001E7754
		private void dfrmAutoShift_KeyDown(object sender, KeyEventArgs e)
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
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x001E87C8 File Offset: 0x001E77C8
		private void dfrmAutoShift_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			try
			{
				this.Label3.Text = wgAppConfig.ReplaceFloorRoom(this.Label3.Text);
				base.KeyPreview = true;
				this._dataTableLoad();
				this.dtpStartDate.Value = DateTime.Now.Date;
				this.dtpEndDate.Value = DateTime.Now.Date;
				if (this.cbof_Group.Items.Count > 0)
				{
					this.cbof_Group.SelectedIndex = 0;
				}
				if (this.cbof_ConsumerName.Items.Count > 0)
				{
					this.cbof_ConsumerName.SelectedIndex = 0;
				}
				this.btnOK.Enabled = false;
				if (this.lstOptionalShifts.Items.Count == 0)
				{
					XMessageBox.Show(this, CommonStr.strNeedShift, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x001E88F8 File Offset: 0x001E78F8
		private void dtpEndDate_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				this.lblEndWeekday.Text = CommonStr.strWeekday + wgAppConfig.weekdayToChsName((int)this.dtpEndDate.Value.DayOfWeek);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x001E8958 File Offset: 0x001E7958
		private void dtpStartDate_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				this.dtpEndDate.MinDate = this.dtpStartDate.Value;
				this.lblStartWeekday.Text = CommonStr.strWeekday + wgAppConfig.weekdayToChsName((int)this.dtpStartDate.Value.DayOfWeek);
				this.lblShiftWeekday_update((int)this.dtpStartDate.Value.DayOfWeek);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x001E89E8 File Offset: 0x001E79E8
		private void lblShiftWeekday_update(int weekdayStart)
		{
			try
			{
				int num = weekdayStart;
				if (num >= 7)
				{
					num = 0;
				}
				this.lstShiftWeekday.Items.Clear();
				for (int i = 1; i <= 14; i++)
				{
					this.lstShiftWeekday.Items.Add(wgAppConfig.weekdayToChsName(num));
					num++;
					if (num >= 7)
					{
						num = 0;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x001E8A64 File Offset: 0x001E7A64
		private void loadGroupData()
		{
			new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
			for (int i = 0; i < this.arrGroupID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
				{
					this.cbof_Group.Items.Add(CommonStr.strAll);
				}
				else
				{
					this.cbof_Group.Items.Add(this.arrGroupName[i].ToString());
				}
			}
			if (this.cbof_Group.Items.Count > 0)
			{
				this.cbof_Group.SelectedIndex = 0;
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x001E8B18 File Offset: 0x001E7B18
		private void lstOptionalShifts_DoubleClick(object sender, EventArgs e)
		{
			this.btnAddOne.PerformClick();
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x001E8B25 File Offset: 0x001E7B25
		private void lstSelectedShifts_DoubleClick(object sender, EventArgs e)
		{
			this.btnDeleteOne.PerformClick();
		}

		// Token: 0x04003054 RID: 12372
		private SqlCommand cmd;

		// Token: 0x04003055 RID: 12373
		private SqlConnection con;

		// Token: 0x04003056 RID: 12374
		private SqlDataAdapter daConsumers;

		// Token: 0x04003057 RID: 12375
		private DataSet dsConsumers;

		// Token: 0x04003058 RID: 12376
		private DataTable dtConsumers;

		// Token: 0x04003059 RID: 12377
		private DataTable dtOptionalShift;

		// Token: 0x0400305A RID: 12378
		private DataView dvConsumers;

		// Token: 0x0400305B RID: 12379
		private ArrayList arrConsumerCMIndex = new ArrayList();

		// Token: 0x0400305C RID: 12380
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x0400305D RID: 12381
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x0400305E RID: 12382
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x0400305F RID: 12383
		private ArrayList arrSelectedShiftID = new ArrayList();

		// Token: 0x04003060 RID: 12384
		private ArrayList arrShiftID = new ArrayList();
	}
}
