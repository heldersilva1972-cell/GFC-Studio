using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030C RID: 780
	public partial class dfrmRouteEdit : frmN3000
	{
		// Token: 0x06001799 RID: 6041 RVA: 0x001EB1D4 File Offset: 0x001EA1D4
		public dfrmRouteEdit()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvOptional);
			wgAppConfig.custDataGridview(ref this.dgvSelected);
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x001EB242 File Offset: 0x001EA242
		private void btnAddAllReaders_Click(object sender, EventArgs e)
		{
			this.dgvOptional.SelectAll();
			this.btnAddOneReader_Click(sender, e);
			this.dgvOptional.ClearSelection();
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x001EB264 File Offset: 0x001EA264
		private void btnAddOneReader_Click(object sender, EventArgs e)
		{
			this.saveDefaultValue();
			int count = this.dgvSelected.Rows.Count;
			if (this.routeSn < 0 && this.dgvSelected.Rows.Count > 0)
			{
				foreach (object obj in (this.dgvSelected.DataSource as DataView))
				{
					DataRowView dataRowView = (DataRowView)obj;
					if (this.routeSn < (int)dataRowView["f_Sn"])
					{
						this.routeSn = (int)dataRowView["f_Sn"];
					}
				}
				this.routeSn++;
			}
			if (this.routeSn <= 0)
			{
				this.routeSn = 1;
			}
			if (this.dgvSelected.Rows.Count <= 0)
			{
				this.datetimeFirstPatrol = this.dtpTime.Value.ToString("HH:mm");
				this.routeSn = 1;
				this.radioButton2.Checked = false;
				this.radioButton1.Checked = true;
			}
			else
			{
				this.datetimeFirstPatrol = (this.dgvSelected.DataSource as DataView)[0]["f_patroltime"].ToString();
				if ((this.dgvSelected.DataSource as DataView)[0]["f_NextDay"].ToString() == "1")
				{
					XMessageBox.Show(CommonStr.strPatrolPointFirstTimeNextDay);
					return;
				}
				if (string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.ToString("HH:mm")) > 0)
				{
					this.radioButton2.Checked = true;
				}
				else
				{
					if (string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.ToString("HH:mm")) == 0 && this.dtpTime.Value.ToString("HH:mm") == "00:00")
					{
						XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
						return;
					}
					this.radioButton2.Checked = false;
				}
			}
			DataGridView dataGridView = this.dgvOptional;
			int num;
			if (dataGridView.SelectedRows.Count <= 0)
			{
				if (dataGridView.SelectedCells.Count <= 0)
				{
					return;
				}
				num = dataGridView.SelectedCells[0].RowIndex;
			}
			else
			{
				num = dataGridView.SelectedRows[0].Index;
			}
			DataTable table = ((DataView)this.dgvSelected.DataSource).Table;
			using (DataTable table2 = ((DataView)dataGridView.DataSource).Table)
			{
				if (dataGridView.SelectedRows.Count > 0)
				{
					int count2 = dataGridView.SelectedRows.Count;
					int[] array = new int[count2];
					int num2 = 0;
					for (int i = 0; i < dataGridView.Rows.Count; i++)
					{
						if (dataGridView.Rows[i].Selected)
						{
							array[num2] = (int)dataGridView.Rows[i].Cells[this.colf_ReaderID].Value;
							num2++;
						}
					}
					for (int j = 0; j < count2; j++)
					{
						int num3 = array[j];
						DataRow dataRow = table2.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_NextDay"] = (this.radioButton2.Checked ? 1 : 0);
							dataRow["f_patroltime"] = this.dtpTime.Value.ToString("HH:mm");
							DataRow dataRow2 = table.NewRow();
							for (int k = 0; k < table2.Columns.Count; k++)
							{
								dataRow2[k] = dataRow[k];
							}
							dataRow2["f_Sn"] = this.routeSn;
							this.routeSn++;
							table.Rows.Add(dataRow2);
							if (this.chkAutoAdd.Checked)
							{
								if (this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value).Date == this.dtpTime.Value.Date)
								{
									if (this.radioButton2.Checked && string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value).ToString("HH:mm")) <= 0)
									{
										XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
										break;
									}
									this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value);
								}
								else
								{
									if (this.radioButton2.Checked)
									{
										XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
										break;
									}
									this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value);
									this.radioButton2.Checked = true;
								}
							}
						}
					}
				}
				else
				{
					int num4 = (int)dataGridView.Rows[num].Cells[this.colf_ReaderID].Value;
					DataRow dataRow = table2.Rows.Find(num4);
					if (dataRow != null)
					{
						dataRow["f_NextDay"] = (this.radioButton2.Checked ? 1 : 0);
						dataRow["f_patroltime"] = this.dtpTime.Value.ToString("HH:mm");
						DataRow dataRow3 = table.NewRow();
						for (int l = 0; l < table2.Columns.Count; l++)
						{
							dataRow3[l] = dataRow[l];
						}
						dataRow3["f_Sn"] = this.routeSn;
						this.routeSn++;
						table.Rows.Add(dataRow3);
						if (this.chkAutoAdd.Checked)
						{
							if (this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value).Date == this.dtpTime.Value.Date)
							{
								if (this.radioButton2.Checked && string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value).ToString("HH:mm")) <= 0)
								{
									XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
									table.AcceptChanges();
									return;
								}
								this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value);
							}
							else if (!this.radioButton2.Checked)
							{
								this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double)this.nudMinute.Value);
								this.radioButton2.Checked = true;
							}
						}
					}
				}
			}
			table.AcceptChanges();
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x001EBA38 File Offset: 0x001EAA38
		private void btnCopyFromOtherRoute_Click(object sender, EventArgs e)
		{
			using (dfrmPatrolTaskEdit dfrmPatrolTaskEdit = new dfrmPatrolTaskEdit())
			{
				if (dfrmPatrolTaskEdit.ShowDialog() == DialogResult.OK)
				{
					int routeID = dfrmPatrolTaskEdit.routeID;
					if (routeID != 0)
					{
						this.ds.Tables["selectedReader"].Clear();
						string text = "Select   f_NextDay, f_patroltime, f_Sn, t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 0 as f_Selected from t_d_PatrolRouteDetail,t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_PatrolRouteDetail.f_ReaderID and t_d_PatrolRouteDetail.f_RouteID = " + routeID.ToString();
						if (wgAppConfig.IsAccessDB)
						{
							using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
							{
								using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
								{
									using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
									{
										oleDbDataAdapter.Fill(this.ds, "selectedReader");
									}
								}
								return;
							}
						}
						using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
						{
							using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
							{
								using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
								{
									sqlDataAdapter.Fill(this.ds, "selectedReader");
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x001EBC04 File Offset: 0x001EAC04
		private void btnDeleteAllReaders_Click(object sender, EventArgs e)
		{
			this.dgvSelected.SelectAll();
			this.btnDeleteOneReader_Click(sender, e);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x001EBC1C File Offset: 0x001EAC1C
		private void btnDeleteOneReader_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvSelected;
			try
			{
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				using (DataTable table = ((DataView)dataGridView.DataSource).Table)
				{
					if (dataGridView.SelectedRows.Count > 0)
					{
						int count = dataGridView.SelectedRows.Count;
						int[] array = new int[count];
						for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
						{
							array[i] = (int)dataGridView.SelectedRows[i].Cells[this.colf_Sn].Value;
						}
						for (int j = 0; j < count; j++)
						{
							int num2 = array[j];
							DataRow dataRow = table.Rows.Find(num2);
							if (dataRow != null)
							{
								dataRow.Delete();
							}
						}
						table.AcceptChanges();
					}
					else
					{
						int num3 = (int)dataGridView.Rows[num].Cells[this.colf_Sn].Value;
						DataRow dataRow = table.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = 0;
							dataRow.Delete();
						}
						table.AcceptChanges();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x001EBDD4 File Offset: 0x001EADD4
		private void btnStartTimeUpdate_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvSelected.DataSource).Table;
			DataView dataView = (DataView)this.dgvSelected.DataSource;
			if (dataView.Count > 0)
			{
				if ((this.dgvSelected.DataSource as DataView)[0]["f_NextDay"].ToString() == "1")
				{
					XMessageBox.Show(CommonStr.strPatrolPointFirstTimeNextDay);
					return;
				}
				DateTime dateTime = DateTime.Parse(this.dtpTime.Value.ToString("yyyy-MM-dd ") + dataView[0]["f_patroltime"].ToString() + ":00");
				TimeSpan timeSpan = this.dtpTime.Value.Subtract(dateTime);
				dataView[0]["f_patroltime"] = this.dtpTime.Value.ToString("HH:mm");
				dateTime = DateTime.Parse(this.dtpTime.Value.ToString("yyyy-MM-dd HH:mm:00"));
				DateTime dateTime2 = dateTime.AddDays(1.0);
				for (int i = 1; i < table.Rows.Count; i++)
				{
					DateTime dateTime3 = DateTime.Parse(this.dtpTime.Value.AddDays((double)((int)table.Rows[i]["f_NextDay"])).ToString("yyyy-MM-dd ") + table.Rows[i]["f_patroltime"].ToString() + ":00").AddMinutes(timeSpan.TotalMinutes);
					if (dateTime3 >= dateTime2)
					{
						XMessageBox.Show(CommonStr.strPatrolErrPatrolTime);
						table.AcceptChanges();
						return;
					}
					table.Rows[i]["f_patroltime"] = dateTime3.ToString("HH:mm");
					table.Rows[i]["f_NextDay"] = ((dateTime3.Date != dateTime.Date) ? 1 : 0);
				}
				table.AcceptChanges();
			}
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x001EC018 File Offset: 0x001EB018
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x001EC020 File Offset: 0x001EB020
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtName.Text))
			{
				XMessageBox.Show(CommonStr.strNameNotEmpty);
				return;
			}
			if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
			{
				XMessageBox.Show(CommonStr.strNameNotEmpty);
				return;
			}
			this.Cursor = Cursors.WaitCursor;
			try
			{
				Cursor cursor = Cursor.Current;
				if (this.currentRouteID <= 0)
				{
					this.currentRouteID = int.Parse(this.cbof_RouteID.Text);
					wgAppConfig.runUpdateSql(string.Format(" INSERT INTO t_d_PatrolRouteList(f_RouteID, f_RouteName) VALUES({0},{1})", this.currentRouteID.ToString(), wgTools.PrepareStrNUnicode(this.txtName.Text)));
				}
				else
				{
					wgAppConfig.runUpdateSql(string.Format(" UPDATE t_d_PatrolRouteList SET f_RouteName ={1}  WHERE f_RouteID={0}", this.currentRouteID.ToString(), wgTools.PrepareStrNUnicode(this.txtName.Text)));
				}
				wgAppConfig.runUpdateSql(" DELETE FROM  t_d_PatrolRouteDetail WHERE f_RouteID = " + this.currentRouteID.ToString());
				if (this.dvSelected.Count > 0)
				{
					for (int i = 0; i <= this.dvSelected.Count - 1; i++)
					{
						wgAppConfig.runUpdateSql(string.Concat(new string[]
						{
							"INSERT INTO t_d_PatrolRouteDetail (f_RouteID, f_Sn, f_ReaderID, f_patroltime, f_NextDay) VALUES( ",
							this.currentRouteID.ToString(),
							",",
							(i + 1).ToString(),
							",",
							this.dvSelected[i]["f_ReaderID"].ToString(),
							",",
							wgTools.PrepareStrNUnicode(this.dvSelected[i]["f_patroltime"].ToString()),
							",",
							this.dvSelected[i]["f_NextDay"].ToString(),
							") "
						}));
					}
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x001EC268 File Offset: 0x001EB268
		private void dfrmMealOption_Load(object sender, EventArgs e)
		{
			this.dtpTime.CustomFormat = "HH:mm";
			this.dtpTime.Format = DateTimePickerFormat.Custom;
			this.dtpTime.Value = DateTime.Parse("00:00:00");
			this.loadData();
			this.dgvOptional.AutoGenerateColumns = false;
			this.dgvOptional.DataSource = this.dv;
			this.dgvSelected.AutoGenerateColumns = false;
			this.dgvSelected.DataSource = this.dvSelected;
			this.dvSelected.Sort = "f_NextDay ASC, f_patroltime asc, f_Sn asc";
			this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
			this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			this.dt = this.ds.Tables["optionalReader"];
			try
			{
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[this.colf_ReaderID] };
			}
			catch (Exception)
			{
				throw;
			}
			this.dt = this.ds.Tables["selectedReader"];
			try
			{
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[this.colf_Sn] };
			}
			catch (Exception)
			{
				throw;
			}
			for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
			{
				this.dgvOptional.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
				this.dgvSelected.Columns[i].DataPropertyName = this.dvSelected.Table.Columns[i].ColumnName;
			}
			this.cbof_RouteID.Items.Clear();
			for (int j = 1; j <= 99; j++)
			{
				this.cbof_RouteID.Items.Add(j);
			}
			string text = "";
			if (this.currentRouteID <= 0)
			{
				text = "New";
			}
			if (!(text == "New"))
			{
				this.cbof_RouteID.Enabled = false;
				this.cbof_RouteID.Text = this.currentRouteID.ToString();
				string text2 = " SELECT * FROM t_d_PatrolRouteList WHERE [f_RouteID]= " + this.currentRouteID.ToString();
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
								this.txtName.Text = wgTools.SetObjToStr(oleDbDataReader["f_RouteName"].ToString());
							}
							oleDbDataReader.Close();
						}
						goto IL_04DB;
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
							this.txtName.Text = wgTools.SetObjToStr(sqlDataReader["f_RouteName"].ToString());
						}
						sqlDataReader.Close();
					}
					goto IL_04DB;
				}
			}
			this.ds.Tables["selectedReader"].Clear();
			this.cbof_RouteID.Enabled = true;
			string text3 = "SELECT f_RouteID FROM t_d_PatrolRouteList  ORDER BY [f_RouteID] ASC ";
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
							int num = this.cbof_RouteID.Items.IndexOf((int)oleDbDataReader2[0]);
							if (num >= 0)
							{
								this.cbof_RouteID.Items.RemoveAt(num);
							}
						}
						oleDbDataReader2.Close();
					}
					goto IL_04A1;
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
						int num = this.cbof_RouteID.Items.IndexOf((int)sqlDataReader2[0]);
						if (num >= 0)
						{
							this.cbof_RouteID.Items.RemoveAt(num);
						}
					}
					sqlDataReader2.Close();
				}
			}
			IL_04A1:
			if (this.cbof_RouteID.Items.Count == 0)
			{
				base.Close();
				return;
			}
			this.cbof_RouteID.Text = this.cbof_RouteID.Items[0].ToString();
			IL_04DB:
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("RouteEditAutoIncrease")))
			{
				if (wgAppConfig.GetKeyVal("RouteEditAutoIncrease") == "0")
				{
					this.chkAutoAdd.Checked = false;
				}
				else
				{
					this.chkAutoAdd.Checked = true;
					try
					{
						this.nudMinute.Value = decimal.Parse(wgAppConfig.GetKeyVal("RouteEditAutoIncrease"), CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
					}
				}
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("RouteEditStartTime")))
			{
				try
				{
					this.dtpTime.Value = wgTools.wgDateTimeParse(wgAppConfig.GetKeyVal("RouteEditStartTime"));
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x001EC87C File Offset: 0x001EB87C
		public void loadData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadData_Acc();
				return;
			}
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			try
			{
				SqlCommand sqlCommand = new SqlCommand("Select  0 as f_NextDay,'' as f_patroltime, -1 as f_Sn, t_b_reader.f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader, t_b_Reader4Patrol  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader4Patrol.f_ReaderID = t_b_Reader.f_ReaderID  ", sqlConnection);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
				sqlDataAdapter.Fill(this.ds, "optionalReader");
				sqlCommand = new SqlCommand("Select   f_NextDay, f_patroltime, f_Sn, t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 0 as f_Selected from t_d_PatrolRouteDetail,t_b_reader,t_b_Reader4Patrol   , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_PatrolRouteDetail.f_ReaderID  AND t_b_Reader.f_ReaderID = t_b_Reader4Patrol.f_ReaderID  and t_d_PatrolRouteDetail.f_RouteID = " + this.currentRouteID.ToString(), sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(this.ds, "selectedReader");
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected = new DataView(this.ds.Tables["selectedReader"]);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x001EC960 File Offset: 0x001EB960
		public void loadData_Acc()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			try
			{
				OleDbCommand oleDbCommand = new OleDbCommand("Select  0 as f_NextDay,'' as f_patroltime, -1 as f_Sn, t_b_reader.f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader, t_b_Reader4Patrol  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader4Patrol.f_ReaderID = t_b_Reader.f_ReaderID  ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "optionalReader");
				oleDbCommand = new OleDbCommand("Select   f_NextDay, f_patroltime, f_Sn, t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 0 as f_Selected from t_d_PatrolRouteDetail,t_b_reader, t_b_Reader4Patrol  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_PatrolRouteDetail.f_ReaderID  AND t_b_Reader.f_ReaderID = t_b_Reader4Patrol.f_ReaderID  and t_d_PatrolRouteDetail.f_RouteID = " + this.currentRouteID.ToString(), oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "selectedReader");
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected = new DataView(this.ds.Tables["selectedReader"]);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x001ECA34 File Offset: 0x001EBA34
		private void saveDefaultValue()
		{
			if (this.dgvSelected.Rows.Count == 0)
			{
				wgAppConfig.UpdateKeyVal("RouteEditStartTime", this.dtpTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
				if (this.chkAutoAdd.Checked)
				{
					wgAppConfig.UpdateKeyVal("RouteEditAutoIncrease", this.nudMinute.Value.ToString());
					return;
				}
				wgAppConfig.UpdateKeyVal("RouteEditAutoIncrease", "0");
			}
		}

		// Token: 0x040030A0 RID: 12448
		private string datetimeFirstPatrol;

		// Token: 0x040030A1 RID: 12449
		private DataTable dt;

		// Token: 0x040030A3 RID: 12451
		private DataView dv;

		// Token: 0x040030A4 RID: 12452
		private DataView dvSelected;

		// Token: 0x040030A5 RID: 12453
		private int colf_ReaderID = 3;

		// Token: 0x040030A6 RID: 12454
		private int colf_Sn = 2;

		// Token: 0x040030A7 RID: 12455
		public int currentRouteID;

		// Token: 0x040030A8 RID: 12456
		private DataSet ds = new DataSet("dsMeal");

		// Token: 0x040030A9 RID: 12457
		private int routeSn = -1;
	}
}
