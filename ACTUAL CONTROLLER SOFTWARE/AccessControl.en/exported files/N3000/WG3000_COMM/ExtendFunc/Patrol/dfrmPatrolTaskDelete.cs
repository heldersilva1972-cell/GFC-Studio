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
	// Token: 0x0200030A RID: 778
	public partial class dfrmPatrolTaskDelete : frmN3000
	{
		// Token: 0x06001780 RID: 6016 RVA: 0x001E981C File Offset: 0x001E881C
		public dfrmPatrolTaskDelete()
		{
			this.InitializeComponent();
			this.ProgressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x001E9894 File Offset: 0x001E8894
		private void _dataTableLoad()
		{
			this.dsConsumers = new DataSet("Users");
			string text = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM ([t_b_Consumer] INNER JOIN t_d_PatrolUsers ON (t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID) )  LEFT OUTER JOIN t_b_Group ON  ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID ) ";
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
					goto IL_00DA;
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
			IL_00DA:
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

		// Token: 0x06001782 RID: 6018 RVA: 0x001E9A68 File Offset: 0x001E8A68
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x001E9A70 File Offset: 0x001E8A70
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			using (comPatrol comPatrol = new comPatrol())
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
							num = comPatrol.shift_arrange_delete(array[i], value, value2);
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
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				this.ProgressBar1.Value = 0;
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x001E9C98 File Offset: 0x001E8C98
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			using (comPatrol_Acc comPatrol_Acc = new comPatrol_Acc())
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
							num = comPatrol_Acc.shift_arrange_delete(array[i], value, value2);
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
					else
					{
						XMessageBox.Show(this, comPatrol_Acc.errDesc(num) + "\r\n\r\n" + comPatrol_Acc.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

		// Token: 0x06001785 RID: 6021 RVA: 0x001E9EB0 File Offset: 0x001E8EB0
		private void cbof_ConsumerName_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ConsumerName);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x001E9EBD File Offset: 0x001E8EBD
		private void cbof_ConsumerName_Leave(object sender, EventArgs e)
		{
			this.checkUserValid(this.cbof_ConsumerName);
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x001E9ECC File Offset: 0x001E8ECC
		private void cbof_Group_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_Group);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x001E9EDC File Offset: 0x001E8EDC
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

		// Token: 0x06001789 RID: 6025 RVA: 0x001EA068 File Offset: 0x001E9068
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

		// Token: 0x0600178A RID: 6026 RVA: 0x001EA16C File Offset: 0x001E916C
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

		// Token: 0x0600178B RID: 6027 RVA: 0x001EA1E0 File Offset: 0x001E91E0
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

		// Token: 0x0600178C RID: 6028 RVA: 0x001EA2DC File Offset: 0x001E92DC
		private void dfrmShiftDelete_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x001EA2F4 File Offset: 0x001E92F4
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

		// Token: 0x0600178E RID: 6030 RVA: 0x001EA354 File Offset: 0x001E9354
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

		// Token: 0x0600178F RID: 6031 RVA: 0x001EA3E4 File Offset: 0x001E93E4
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

		// Token: 0x06001790 RID: 6032 RVA: 0x001EA420 File Offset: 0x001E9420
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

		// Token: 0x0400307B RID: 12411
		private DataSet dsConsumers;

		// Token: 0x0400307C RID: 12412
		private DataTable dtConsumers;

		// Token: 0x0400307D RID: 12413
		private DataView dvConsumers;

		// Token: 0x0400307E RID: 12414
		private ArrayList arrConsumerCMIndex = new ArrayList();

		// Token: 0x0400307F RID: 12415
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04003080 RID: 12416
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04003081 RID: 12417
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04003082 RID: 12418
		private ArrayList arrSelectedShiftID = new ArrayList();

		// Token: 0x04003083 RID: 12419
		private ArrayList arrShiftID = new ArrayList();
	}
}
