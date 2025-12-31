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
	// Token: 0x02000370 RID: 880
	public partial class dfrmShiftDelete : frmN3000
	{
		// Token: 0x06001CC8 RID: 7368 RVA: 0x0026006C File Offset: 0x0025F06C
		public dfrmShiftDelete()
		{
			this.InitializeComponent();
			this.ProgressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x002600E4 File Offset: 0x0025F0E4
		private void _dataTableLoad()
		{
			this.dsConsumers = new DataSet("Users");
			string text = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM [t_b_Consumer]  LEFT OUTER JOIN t_b_Group ON ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID ) WHERE f_AttendEnabled = 1 ";
			text += " AND f_ShiftEnabled > 0 ";
			this.dsConsumers.Clear();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dsConsumers, "Consumers");
						}
					}
					goto IL_00E6;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dsConsumers, "Consumers");
					}
				}
			}
			IL_00E6:
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
			this.loadGroupData();
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x002602C4 File Offset: 0x0025F2C4
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x002602CC File Offset: 0x0025F2CC
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			using (comShift comShift = new comShift())
			{
				int num = 0;
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					int[] array;
					if (this.cbof_ConsumerName.Text == CommonStr.strAll)
					{
						if (this.dvConsumers.Count <= 0)
						{
							XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						array = new int[this.dvConsumers.Count - 1 + 1];
						for (int i = 0; i <= this.dvConsumers.Count - 1; i++)
						{
							array[i] = (int)this.dvConsumers[i]["f_ConsumerID"];
						}
					}
					else
					{
						array = new int[] { (int)this.dvConsumers[this.cbof_ConsumerName.SelectedIndex - 1]["f_ConsumerID"] };
					}
					DateTime value = this.dtpStartDate.Value;
					DateTime value2 = this.dtpEndDate.Value;
					if (num == 0)
					{
						this.ProgressBar1.Maximum = array.Length;
						for (int i = 0; i <= array.Length - 1; i++)
						{
							this.ProgressBar1.Value = i;
							num = comShift.shift_arrange_delete(array[i], value, value2);
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
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				this.ProgressBar1.Value = 0;
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x002604F4 File Offset: 0x0025F4F4
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			using (comShift_Acc comShift_Acc = new comShift_Acc())
			{
				int num = 0;
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					int[] array;
					if (this.cbof_ConsumerName.Text == CommonStr.strAll)
					{
						if (this.dvConsumers.Count <= 0)
						{
							XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						array = new int[this.dvConsumers.Count - 1 + 1];
						for (int i = 0; i <= this.dvConsumers.Count - 1; i++)
						{
							array[i] = (int)this.dvConsumers[i]["f_ConsumerID"];
						}
					}
					else
					{
						array = new int[] { (int)this.dvConsumers[this.cbof_ConsumerName.SelectedIndex - 1]["f_ConsumerID"] };
					}
					DateTime value = this.dtpStartDate.Value;
					DateTime value2 = this.dtpEndDate.Value;
					if (num == 0)
					{
						this.ProgressBar1.Maximum = array.Length;
						for (int i = 0; i <= array.Length - 1; i++)
						{
							this.ProgressBar1.Value = i;
							num = comShift_Acc.shift_arrange_delete(array[i], value, value2);
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
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				this.ProgressBar1.Value = 0;
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0026070C File Offset: 0x0025F70C
		private void cbof_ConsumerName_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ConsumerName);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00260719 File Offset: 0x0025F719
		private void cbof_ConsumerName_Leave(object sender, EventArgs e)
		{
			this.checkUserValid(this.cbof_ConsumerName);
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x00260728 File Offset: 0x0025F728
		private void cbof_Group_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_Group);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x00260738 File Offset: 0x0025F738
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
					this.dvConsumers.RowFilter = string.Format(" (f_GroupName = {0} ) OR (f_GroupName like {1})", wgTools.PrepareStr(this.cbof_Group.Text), wgTools.PrepareStr(string.Format("{0}\\%", this.cbof_Group.Text)));
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
				int count = this.dvConsumers.Count;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x002608C4 File Offset: 0x0025F8C4
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

		// Token: 0x06001CD2 RID: 7378 RVA: 0x002609C8 File Offset: 0x0025F9C8
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

		// Token: 0x06001CD3 RID: 7379 RVA: 0x00260A3C File Offset: 0x0025FA3C
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
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x00260B38 File Offset: 0x0025FB38
		private void dfrmShiftDelete_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00260B50 File Offset: 0x0025FB50
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

		// Token: 0x06001CD6 RID: 7382 RVA: 0x00260BB0 File Offset: 0x0025FBB0
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

		// Token: 0x06001CD7 RID: 7383 RVA: 0x00260C40 File Offset: 0x0025FC40
		private void lblShiftWeekday_update(int weekdayStart)
		{
			try
			{
				if (weekdayStart >= 7)
				{
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x00260C7C File Offset: 0x0025FC7C
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

		// Token: 0x04003760 RID: 14176
		private DataSet dsConsumers;

		// Token: 0x04003761 RID: 14177
		private DataTable dtConsumers;

		// Token: 0x04003762 RID: 14178
		private DataView dvConsumers;

		// Token: 0x04003763 RID: 14179
		private ArrayList arrConsumerCMIndex = new ArrayList();

		// Token: 0x04003764 RID: 14180
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04003765 RID: 14181
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04003766 RID: 14182
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04003767 RID: 14183
		private ArrayList arrSelectedShiftID = new ArrayList();

		// Token: 0x04003768 RID: 14184
		private ArrayList arrShiftID = new ArrayList();
	}
}
