using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x0200023B RID: 571
	public partial class dfrmControllerNormalOpenTimeList : frmN3000
	{
		// Token: 0x0600114B RID: 4427 RVA: 0x0013EE58 File Offset: 0x0013DE58
		public dfrmControllerNormalOpenTimeList()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvTaskList);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0013EE7C File Offset: 0x0013DE7C
		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (this.dtpBegin.Value.Date > this.dtpEnd.Value.Date)
			{
				XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!this.checkBox43.Checked && !this.checkBox44.Checked && !this.checkBox45.Checked && !this.checkBox46.Checked && !this.checkBox47.Checked && !this.checkBox48.Checked && !this.checkBox49.Checked)
			{
				XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.cboDoors.SelectedIndex >= 0)
			{
				bool flag = false;
				string text;
				if (this.dgvTaskList.Rows.Count == 0)
				{
					flag = true;
					if (wgAppConfig.IsAccessDB)
					{
						text = string.Format("ALTER TABLE t_b_ControllerNormalOpenTimeList ALTER COLUMN [f_Id] COUNTER (1, 1) ", new object[0]);
					}
					else
					{
						text = "  DBCC   CHECKIDENT   (t_b_ControllerNormalOpenTimeList,   RESEED,   0) ";
					}
					try
					{
						wgAppConfig.runUpdateSql(text);
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
				}
				text = " INSERT INTO t_b_ControllerNormalOpenTimeList(f_BeginYMD,[f_EndYMD],  [f_OperateTime] ,[f_OperateTimeEnd] ,";
				text = string.Concat(new object[]
				{
					text,
					"  [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday], [f_DoorID],  [f_DoorControl], [f_Notes])  VALUES ( ",
					this.getDateString(this.dtpBegin),
					" , ",
					this.getDateString(this.dtpEnd),
					" , ",
					this.getDateString(this.dtpTime),
					" , ",
					this.getDateString(this.dateEndHMS1),
					" , ",
					this.checkBox43.Checked ? "1" : "0",
					" , ",
					this.checkBox44.Checked ? "1" : "0",
					" , ",
					this.checkBox45.Checked ? "1" : "0",
					" , ",
					this.checkBox46.Checked ? "1" : "0",
					" , ",
					this.checkBox47.Checked ? "1" : "0",
					" , ",
					this.checkBox48.Checked ? "1" : "0",
					" , ",
					this.checkBox49.Checked ? "1" : "0",
					" , ",
					this.arrDoorID[this.cboDoors.SelectedIndex]
				});
				if (this.cboAccessMethod.SelectedIndex < 0)
				{
					this.cboAccessMethod.SelectedIndex = 0;
				}
				text = string.Concat(new object[]
				{
					text,
					" , ",
					this.cboAccessMethod.SelectedIndex,
					" , ",
					wgTools.PrepareStrNUnicode(this.textBox1.Text.Trim()),
					" )"
				});
				if (wgAppConfig.runUpdateSql(text) > 0)
				{
					if (flag && wgAppConfig.getValBySql("SELECT f_Id FROM t_b_ControllerNormalOpenTimeList") == 0)
					{
						wgAppConfig.runSql("DELETE FROM t_b_ControllerNormalOpenTimeList");
						wgAppConfig.runUpdateSql(text);
					}
					wgAppConfig.wgLog(string.Format("{0} {1} [{2}]", this.Text, this.btnAdd.Text, text));
					this.LoadTaskListData();
				}
			}
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0013F230 File Offset: 0x0013E230
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0013F238 File Offset: 0x0013E238
		private void btnDel_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvTaskList.Rows.Count > 0)
			{
				try
				{
					num = this.dgvTaskList.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvTaskList.SelectedRows.Count <= 0)
			{
				if (this.dgvTaskList.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvTaskList.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvTaskList.SelectedRows[0].Index;
			}
			string text = "";
			if (this.dgvTaskList.SelectedRows.Count > 1)
			{
				int num3 = 0;
				for (int i = 0; i < this.dgvTaskList.SelectedRows.Count; i++)
				{
					int num4 = int.MaxValue;
					for (int j = 0; j < this.dgvTaskList.SelectedRows.Count; j++)
					{
						num2 = this.dgvTaskList.SelectedRows[j].Index;
						int num5 = int.Parse(this.dgvTaskList.Rows[num2].Cells[0].Value.ToString());
						if (num5 > num3 && num5 < num4)
						{
							num4 = num5;
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						text += ",";
					}
					text += num4.ToString();
					num3 = num4;
				}
			}
			string text2;
			if (this.dgvTaskList.SelectedRows.Count <= 1)
			{
				text2 = string.Format("{0}\r\n{1}:  {2}", this.btnDel.Text, this.dgvTaskList.Columns[0].HeaderText, this.dgvTaskList.Rows[num2].Cells[0].Value.ToString());
				text2 = string.Format(CommonStr.strAreYouSure + " {0} ?", text2);
			}
			else
			{
				text2 = string.Format("{0}\r\n{1}=  {2}", this.btnDel.Text, CommonStr.strTaskNum, this.dgvTaskList.SelectedRows.Count.ToString());
				text2 = string.Format(CommonStr.strAreYouSure + " {0} ?", text2);
			}
			if (XMessageBox.Show(text2, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				string text3;
				if (string.IsNullOrEmpty(text))
				{
					text3 = " DELETE FROM t_b_ControllerNormalOpenTimeList WHERE [f_Id]= " + this.dgvTaskList.Rows[num2].Cells[0].Value.ToString();
				}
				else
				{
					text3 = string.Format(" DELETE FROM t_b_ControllerNormalOpenTimeList WHERE [f_Id] IN ({0}) ", text);
				}
				wgAppConfig.runUpdateSql(text3);
				wgAppConfig.wgLog(string.Format("{0} {1} [{2}]", this.Text, this.btnDel.Text, text3));
				this.LoadTaskListData();
				if (this.dgvTaskList.RowCount > 0)
				{
					if (this.dgvTaskList.RowCount > num)
					{
						this.dgvTaskList.CurrentCell = this.dgvTaskList[1, num];
						return;
					}
					this.dgvTaskList.CurrentCell = this.dgvTaskList[1, this.dgvTaskList.RowCount - 1];
				}
			}
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0013F570 File Offset: 0x0013E570
		private void btnEdit_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvTaskList.Rows.Count > 0)
			{
				try
				{
					num = this.dgvTaskList.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvTaskList.SelectedRows.Count <= 0)
			{
				if (this.dgvTaskList.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvTaskList.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvTaskList.SelectedRows[0].Index;
			}
			string text = "";
			if (this.dgvTaskList.SelectedRows.Count > 1)
			{
				int num3 = 0;
				for (int i = 0; i < this.dgvTaskList.SelectedRows.Count; i++)
				{
					int num4 = int.MaxValue;
					for (int j = 0; j < this.dgvTaskList.SelectedRows.Count; j++)
					{
						num2 = this.dgvTaskList.SelectedRows[j].Index;
						int num5 = int.Parse(this.dgvTaskList.Rows[num2].Cells[0].Value.ToString());
						if (num5 > num3 && num5 < num4)
						{
							num4 = num5;
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						text += ",";
					}
					text += num4.ToString();
					num3 = num4;
				}
			}
			bool flag = false;
			using (dfrmControllerNormalOpenTime dfrmControllerNormalOpenTime = new dfrmControllerNormalOpenTime())
			{
				if (this.dgvTaskList.SelectedRows.Count > 1)
				{
					dfrmControllerNormalOpenTime.Text = string.Format("{0}: [{1}]", this.btnEdit.Text, this.dgvTaskList.SelectedRows.Count.ToString());
					dfrmControllerNormalOpenTime.txtTaskIDs.Text = string.Format("({0})", text);
				}
				else
				{
					dfrmControllerNormalOpenTime.txtTaskIDs.Text = this.dgvTaskList.Rows[num2].Cells[0].Value.ToString();
				}
				if (dfrmControllerNormalOpenTime.ShowDialog() == DialogResult.OK)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.LoadTaskListData();
			}
			if (this.dgvTaskList.RowCount > 0)
			{
				if (this.dgvTaskList.RowCount > num)
				{
					this.dgvTaskList.CurrentCell = this.dgvTaskList[1, num];
					return;
				}
				this.dgvTaskList.CurrentCell = this.dgvTaskList[1, this.dgvTaskList.RowCount - 1];
			}
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0013F820 File Offset: 0x0013E820
		private void btnWarnLog_Click(object sender, EventArgs e)
		{
			using (dfrmLogQuery dfrmLogQuery = new dfrmLogQuery())
			{
				dfrmLogQuery.specialLog = "t_s_wglog4DoorOpenTooLongWarn";
				dfrmLogQuery.ShowDialog(this);
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0013F864 File Offset: 0x0013E864
		private void dfrmControllerTaskList_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0013F87C File Offset: 0x0013E87C
		private void dfrmControllerTaskList_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x06001153 RID: 4435 RVA: 0x0013F8F0 File Offset: 0x0013E8F0
		private void dfrmControllerTaskList_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessControlBlue)
			{
				base.Close();
				return;
			}
			this.bFindActive = false;
			this.dtpBegin.Value = DateTime.Now.Date;
			this.dtpEnd.Value = DateTime.Now.Date;
			this.dtpTime.CustomFormat = "HH:mm";
			this.dtpTime.Format = DateTimePickerFormat.Custom;
			this.dtpTime.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("23:59:00");
			this.label1.Visible = false;
			this.cboAccessMethod.Visible = false;
			this.loadDoorData();
			if (this.cboAccessMethod.Items.Count > 0)
			{
				this.cboAccessMethod.SelectedIndex = 0;
			}
			this.LoadTaskListData();
			wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMDWeek);
			this.loadOperatorPrivilege();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0013FA0D File Offset: 0x0013EA0D
		private void dgvTaskList_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0013FA1C File Offset: 0x0013EA1C
		private string getDateString(DateTimePicker dtp)
		{
			if (dtp == null)
			{
				return "NULL";
			}
			return wgTools.PrepareStr(dtp.Value.ToString(wgTools.YMDHMSFormat), true, wgTools.YMDHMSFormat);
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0013FA50 File Offset: 0x0013EA50
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			this.dtDoors = new DataTable();
			this.dvDoors = new DataView(this.dtDoors);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtDoors);
						}
					}
					goto IL_00DB;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtDoors);
					}
				}
			}
			IL_00DB:
			int count = this.dtDoors.Rows.Count;
			new icControllerZone().getAllowedControllers(ref this.dtDoors);
			this.cboDoors.Items.Clear();
			if (count == this.dtDoors.Rows.Count)
			{
				this.cboDoors.Items.Add(CommonStr.strAll);
				this.arrDoorID.Add(0);
			}
			if (this.dvDoors.Count > 0)
			{
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
					this.arrDoorID.Add(this.dvDoors[i]["f_DoorID"]);
				}
			}
			if (this.cboDoors.Items.Count > 0)
			{
				this.cboDoors.SelectedIndex = 0;
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0013FC88 File Offset: 0x0013EC88
		private void loadOperatorPrivilege()
		{
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0013FC8C File Offset: 0x0013EC8C
		private void LoadTaskListData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.LoadTaskListData_Acc();
				return;
			}
			string text = "  SELECT f_Id,  ";
			text = text + "   f_BeginYMD,    f_EndYMD,    ISNULL(CONVERT(char(5), f_OperateTime,108) , '00:00') AS [f_Time],   ISNULL(CONVERT(char(5), f_OperateTimeEnd,108) , '00:00') AS [f_TimeEnd],   [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday],    [f_Friday], [f_Saturday], [f_Sunday],   CASE WHEN a.f_DoorID=0 THEN  " + wgTools.PrepareStrNUnicode(CommonStr.strAll) + " ELSE b.f_DoorName END AS f_AdaptTo,  ' ' AS f_DoorControlDesc,  f_Notes,  a.f_DoorID,  a.f_DoorControl , c.f_ZoneID  FROM t_b_ControllerNormalOpenTimeList a LEFT JOIN t_b_Door b ON a.f_DoorID = b.f_DoorID  LEFT JOIN  t_b_Controller c on b.f_ControllerID = c.f_ControllerID ";
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
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
			new icControllerZone().getAllowedControllers(ref this.dt);
			this.dgvTaskList.AutoGenerateColumns = false;
			this.dgvTaskList.DataSource = this.dv;
			int i;
			for (i = 0; i < this.dt.Rows.Count; i++)
			{
				if ((int)this.dt.Rows[i]["f_DoorControl"] < this.cboAccessMethod.Items.Count)
				{
					this.dt.Rows[i]["f_DoorControlDesc"] = this.cboAccessMethod.Items[(int)this.dt.Rows[i]["f_DoorControl"]].ToString();
				}
			}
			i = 0;
			while (i < this.dv.Table.Columns.Count && i < this.dgvTaskList.ColumnCount)
			{
				this.dgvTaskList.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
				this.dgvTaskList.Columns[i].Name = this.dv.Table.Columns[i].ColumnName;
				i++;
			}
			wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_BeginYMD", wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_EndYMD", wgTools.DisplayFormat_DateYMDWeek);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0013FEEC File Offset: 0x0013EEEC
		private void LoadTaskListData_Acc()
		{
			string text = "  SELECT f_Id,  ";
			text = text + "   f_BeginYMD,    f_EndYMD,    IIF(f_OperateTime IS NULL , '00:00',  Format([f_OperateTime],'Short Time') ) AS [f_Time],   IIF(f_OperateTime IS NULL , '00:00',  Format([f_OperateTimeEnd],'Short Time') ) AS [f_TimeEnd],   [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday],    [f_Friday], [f_Saturday], [f_Sunday],   IIF ( a.f_DoorID=0 ,  " + wgTools.PrepareStrNUnicode(CommonStr.strAll) + " , b.f_DoorName ) AS f_AdaptTo,  ' ' AS f_DoorControlDesc,  f_Notes,  a.f_DoorID,  a.f_DoorControl , c.f_ZoneID  FROM (( t_b_ControllerNormalOpenTimeList a LEFT JOIN t_b_Door b ON a.f_DoorID = b.f_DoorID ) LEFT JOIN  t_b_Controller c on b.f_ControllerID = c.f_ControllerID ) ";
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						oleDbDataAdapter.Fill(this.dt);
					}
				}
			}
			new icControllerZone().getAllowedControllers(ref this.dt);
			this.dgvTaskList.AutoGenerateColumns = false;
			this.dgvTaskList.DataSource = this.dv;
			int i;
			for (i = 0; i < this.dt.Rows.Count; i++)
			{
				if ((int)this.dt.Rows[i]["f_DoorControl"] < this.cboAccessMethod.Items.Count)
				{
					this.dt.Rows[i]["f_DoorControlDesc"] = this.cboAccessMethod.Items[(int)this.dt.Rows[i]["f_DoorControl"]].ToString();
				}
			}
			i = 0;
			while (i < this.dv.Table.Columns.Count && i < this.dgvTaskList.ColumnCount)
			{
				this.dgvTaskList.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
				this.dgvTaskList.Columns[i].Name = this.dv.Table.Columns[i].ColumnName;
				i++;
			}
			wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_BeginYMD", wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_EndYMD", wgTools.DisplayFormat_DateYMDWeek);
		}

		// Token: 0x04001EC7 RID: 7879
		private DataTable dt;

		// Token: 0x04001EC8 RID: 7880
		private DataTable dtDoors;

		// Token: 0x04001EC9 RID: 7881
		private DataView dv;

		// Token: 0x04001ECA RID: 7882
		private DataView dvDoors;

		// Token: 0x04001ECB RID: 7883
		private ArrayList arrDoorID = new ArrayList();
	}
}
