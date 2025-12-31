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

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000365 RID: 869
	public partial class dfrmAutoShift : frmN3000
	{
		// Token: 0x06001C35 RID: 7221 RVA: 0x00253104 File Offset: 0x00252104
		public dfrmAutoShift()
		{
			this.InitializeComponent();
			this.ProgressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0025317C File Offset: 0x0025217C
		private void _dataTableLoad()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this._dataTableLoad_Acc();
				return;
			}
			this.con = new SqlConnection(wgAppConfig.dbConString);
			this.dsConsumers = new DataSet("Users");
			string text = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM [t_b_Consumer]  LEFT OUTER JOIN t_b_Group ON  t_b_Group.f_GroupID = t_b_Consumer.f_GroupID  WHERE f_AttendEnabled = 1 ";
			text += " AND f_ShiftEnabled > 0 ";
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
					this.cmd = new SqlCommand("SELECT [f_ShiftID] & '-' & [f_ShiftName] as f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC", this.con);
				}
				else
				{
					this.cmd = new SqlCommand("SELECT CONVERT(nvarchar(50),[f_ShiftID]) + case when [f_ShiftName] IS NULL Then '' ELSE   '-' + [f_ShiftName] end  as f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC", this.con);
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

		// Token: 0x06001C37 RID: 7223 RVA: 0x002534DC File Offset: 0x002524DC
		private void _dataTableLoad_Acc()
		{
			OleDbConnection oleDbConnection = null;
			oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			this.dsConsumers = new DataSet("Users");
			string text = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM [t_b_Consumer]  LEFT OUTER JOIN t_b_Group ON  ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID ) WHERE f_AttendEnabled = 1 ";
			OleDbCommand oleDbCommand = new OleDbCommand(text + " AND f_ShiftEnabled > 0 ", oleDbConnection);
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
					oleDbCommand = new OleDbCommand("SELECT [f_ShiftID] & '-' & [f_ShiftName] as f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC", oleDbConnection);
				}
				else
				{
					oleDbCommand = new OleDbCommand("SELECT CONVERT(nvarchar(50),[f_ShiftID]) + case when [f_ShiftName] IS NULL Then '' ELSE   '-' + [f_ShiftName] end  as f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC", oleDbConnection);
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

		// Token: 0x06001C38 RID: 7224 RVA: 0x002537F8 File Offset: 0x002527F8
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

		// Token: 0x06001C39 RID: 7225 RVA: 0x00253888 File Offset: 0x00252888
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x00253890 File Offset: 0x00252890
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

		// Token: 0x06001C3B RID: 7227 RVA: 0x002538E4 File Offset: 0x002528E4
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

		// Token: 0x06001C3C RID: 7228 RVA: 0x0025394C File Offset: 0x0025294C
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
						using (comShift_Acc comShift_Acc = new comShift_Acc())
						{
							int num = comShift_Acc.shift_rule_checkValid(array.Length, array);
							if (num == 0)
							{
								this.ProgressBar1.Maximum = array2.Length;
								for (int i = 0; i <= array2.Length - 1; i++)
								{
									this.ProgressBar1.Value = i;
									num = comShift_Acc.shift_arrangeByRule(array2[i], value, value2, array.Length, array);
									if (num != 0)
									{
										XMessageBox.Show(this, comShift_Acc.errDesc(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
								XMessageBox.Show(this, comShift_Acc.errDesc(num) + "\r\n\r\n" + comShift_Acc.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							}
							goto IL_02F2;
						}
					}
					using (comShift comShift = new comShift())
					{
						int num = comShift.shift_rule_checkValid(array.Length, array);
						if (num == 0)
						{
							this.ProgressBar1.Maximum = array2.Length;
							for (int i = 0; i <= array2.Length - 1; i++)
							{
								this.ProgressBar1.Value = i;
								num = comShift.shift_arrangeByRule(array2[i], value, value2, array.Length, array);
								if (num != 0)
								{
									XMessageBox.Show(this, comShift.errDesc(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
							XMessageBox.Show(this, comShift.errDesc(num) + "\r\n\r\n" + comShift.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
					}
				}
				IL_02F2:;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			this.ProgressBar1.Value = 0;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x00253CC8 File Offset: 0x00252CC8
		private void cbof_ConsumerName_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ConsumerName);
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00253CD5 File Offset: 0x00252CD5
		private void cbof_ConsumerName_Leave(object sender, EventArgs e)
		{
			this.checkUserValid(this.cbof_ConsumerName);
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00253CE4 File Offset: 0x00252CE4
		private void cbof_Group_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_Group);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00253CF4 File Offset: 0x00252CF4
		private void cbof_Group_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.toolTip1.SetToolTip(this.cbof_Group, this.cbof_Group.Text);
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

		// Token: 0x06001C41 RID: 7233 RVA: 0x00253EBC File Offset: 0x00252EBC
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

		// Token: 0x06001C42 RID: 7234 RVA: 0x00253FC0 File Offset: 0x00252FC0
		private void dfrmAutoShift_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x00253FD8 File Offset: 0x00252FD8
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

		// Token: 0x06001C44 RID: 7236 RVA: 0x0025404C File Offset: 0x0025304C
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

		// Token: 0x06001C45 RID: 7237 RVA: 0x0025417C File Offset: 0x0025317C
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

		// Token: 0x06001C46 RID: 7238 RVA: 0x002541DC File Offset: 0x002531DC
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

		// Token: 0x06001C47 RID: 7239 RVA: 0x0025426C File Offset: 0x0025326C
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

		// Token: 0x06001C48 RID: 7240 RVA: 0x002542E8 File Offset: 0x002532E8
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

		// Token: 0x06001C49 RID: 7241 RVA: 0x0025439C File Offset: 0x0025339C
		private void lstOptionalShifts_DoubleClick(object sender, EventArgs e)
		{
			this.btnAddOne.PerformClick();
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x002543A9 File Offset: 0x002533A9
		private void lstSelectedShifts_DoubleClick(object sender, EventArgs e)
		{
			this.btnDeleteOne.PerformClick();
		}

		// Token: 0x04003657 RID: 13911
		private SqlCommand cmd;

		// Token: 0x04003658 RID: 13912
		private SqlConnection con;

		// Token: 0x04003659 RID: 13913
		private SqlDataAdapter daConsumers;

		// Token: 0x0400365A RID: 13914
		private DataSet dsConsumers;

		// Token: 0x0400365B RID: 13915
		private DataTable dtConsumers;

		// Token: 0x0400365C RID: 13916
		private DataTable dtOptionalShift;

		// Token: 0x0400365D RID: 13917
		private DataView dvConsumers;

		// Token: 0x0400365E RID: 13918
		private ArrayList arrConsumerCMIndex = new ArrayList();

		// Token: 0x0400365F RID: 13919
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04003660 RID: 13920
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04003661 RID: 13921
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04003662 RID: 13922
		private ArrayList arrSelectedShiftID = new ArrayList();

		// Token: 0x04003663 RID: 13923
		private ArrayList arrShiftID = new ArrayList();
	}
}
