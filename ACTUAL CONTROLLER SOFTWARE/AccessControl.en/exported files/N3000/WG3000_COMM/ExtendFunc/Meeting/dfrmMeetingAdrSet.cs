using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FE RID: 766
	public partial class dfrmMeetingAdrSet : frmN3000
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x001CD2DE File Offset: 0x001CC2DE
		public dfrmMeetingAdrSet()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvOptional);
			wgAppConfig.custDataGridview(ref this.dgvSelected);
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x001CD320 File Offset: 0x001CC320
		private void btnAddAllReaders_Click(object sender, EventArgs e)
		{
			try
			{
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 1;
				}
				this.dt.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x001CD3A8 File Offset: 0x001CC3A8
		private void btnAddOneReader_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvOptional);
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x001CD3B5 File Offset: 0x001CC3B5
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x001CD3C4 File Offset: 0x001CC3C4
		private void btnDeleteAllReaders_Click(object sender, EventArgs e)
		{
			try
			{
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 0;
				}
				this.dt.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x001CD44C File Offset: 0x001CC44C
		private void btnDeleteOneReader_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelected);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x001CD45C File Offset: 0x001CC45C
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			try
			{
				this.txtMeetingAdr.Text = this.txtMeetingAdr.Text.Trim();
				if (this.txtMeetingAdr.Text == "")
				{
					XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
				}
				else if (this.dvSelected.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strMeetingSelectReaderAsSign);
				}
				else
				{
					if (!this.txtMeetingAdr.ReadOnly)
					{
						SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand = new SqlCommand(" SELECT * FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.txtMeetingAdr.Text), sqlConnection);
						if (sqlConnection.State == ConnectionState.Closed)
						{
							sqlConnection.Open();
						}
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							sqlDataReader.Close();
							XMessageBox.Show(CommonStr.strMeetingNameIsDupliated);
							return;
						}
						sqlDataReader.Close();
					}
					Cursor cursor = Cursor.Current;
					wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.txtMeetingAdr.Text));
					if (this.dvSelected.Count > 0)
					{
						for (int i = 0; i <= this.dvSelected.Count - 1; i++)
						{
							string text = " INSERT INTO t_d_MeetingAdr";
							object obj = text + " (f_MeetingAdr, f_ReaderID) ";
							wgAppConfig.runUpdateSql(string.Concat(new object[]
							{
								obj,
								" Values(",
								wgTools.PrepareStrNUnicode(this.txtMeetingAdr.Text),
								",",
								this.dvSelected[i]["f_ReaderID"]
							}) + " )");
						}
					}
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x001CD66C File Offset: 0x001CC66C
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			try
			{
				this.txtMeetingAdr.Text = this.txtMeetingAdr.Text.Trim();
				if (this.txtMeetingAdr.Text == "")
				{
					XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
				}
				else if (this.dvSelected.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strMeetingSelectReaderAsSign);
				}
				else
				{
					if (!this.txtMeetingAdr.ReadOnly)
					{
						OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
						OleDbCommand oleDbCommand = new OleDbCommand();
						oleDbCommand = new OleDbCommand(" SELECT * FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.txtMeetingAdr.Text), oleDbConnection);
						if (oleDbConnection.State == ConnectionState.Closed)
						{
							oleDbConnection.Open();
						}
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							oleDbDataReader.Close();
							XMessageBox.Show(CommonStr.strMeetingNameIsDupliated);
							return;
						}
						oleDbDataReader.Close();
					}
					Cursor cursor = Cursor.Current;
					wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.txtMeetingAdr.Text));
					if (this.dvSelected.Count > 0)
					{
						for (int i = 0; i <= this.dvSelected.Count - 1; i++)
						{
							string text = " INSERT INTO t_d_MeetingAdr";
							object obj = text + " (f_MeetingAdr, f_ReaderID) ";
							wgAppConfig.runUpdateSql(string.Concat(new object[]
							{
								obj,
								" Values(",
								wgTools.PrepareStrNUnicode(this.txtMeetingAdr.Text),
								",",
								this.dvSelected[i]["f_ReaderID"]
							}) + " )");
						}
					}
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x001CD86C File Offset: 0x001CC86C
		private void dfrmMeetingAdr_Load(object sender, EventArgs e)
		{
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.Label1.Text = wgAppConfig.ReplaceMeeting(this.Label1.Text);
			this.Label11.Text = wgAppConfig.ReplaceMeeting(this.Label11.Text);
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmMeetingAdr_Load_Acc(sender, e);
				return;
			}
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			SqlCommand sqlCommand = new SqlCommand();
			try
			{
				if (this.curMeetingAdr == "")
				{
					sqlCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )     ", sqlConnection);
					SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
					sqlDataAdapter.Fill(this.ds, "optionalReader");
				}
				else
				{
					this.txtMeetingAdr.Text = this.curMeetingAdr;
					this.txtMeetingAdr.ReadOnly = true;
					sqlCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.curMeetingAdr) + ") ", sqlConnection);
					SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
					sqlDataAdapter.Fill(this.ds, "optionalReader");
					sqlCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.curMeetingAdr) + ") ", sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(this.ds, "optionalReader");
				}
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dv.RowFilter = " f_Selected = 0";
				this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected.RowFilter = " f_Selected = 1";
				this.dt = this.ds.Tables["optionalReader"];
				try
				{
					this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				}
				this.dv.RowFilter = "f_Selected = 0";
				this.dvSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dv;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x001CDBA8 File Offset: 0x001CCBA8
		private void dfrmMeetingAdr_Load_Acc(object sender, EventArgs e)
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			OleDbCommand oleDbCommand = new OleDbCommand();
			try
			{
				if (this.curMeetingAdr == "")
				{
					oleDbCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )   ", oleDbConnection);
					OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
					oleDbDataAdapter.Fill(this.ds, "optionalReader");
				}
				else
				{
					this.txtMeetingAdr.Text = this.curMeetingAdr;
					this.txtMeetingAdr.ReadOnly = true;
					oleDbCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.curMeetingAdr) + ") ", oleDbConnection);
					OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
					oleDbDataAdapter.Fill(this.ds, "optionalReader");
					oleDbCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.curMeetingAdr) + ") ", oleDbConnection);
					new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "optionalReader");
				}
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dv.RowFilter = " f_Selected = 0";
				this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected.RowFilter = " f_Selected = 1";
				this.dt = this.ds.Tables["optionalReader"];
				try
				{
					this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				}
				this.dv.RowFilter = "f_Selected = 0";
				this.dvSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dv;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x001CDE8C File Offset: 0x001CCE8C
		private void dgvOptional_DoubleClick(object sender, EventArgs e)
		{
			this.btnAddOneReader.PerformClick();
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x001CDE99 File Offset: 0x001CCE99
		private void dgvSelected_DoubleClick(object sender, EventArgs e)
		{
			this.btnDeleteOneReader.PerformClick();
		}

		// Token: 0x04002EB1 RID: 11953
		private DataTable dt;

		// Token: 0x04002EB2 RID: 11954
		private DataView dv;

		// Token: 0x04002EB3 RID: 11955
		private DataView dvSelected;

		// Token: 0x04002EB4 RID: 11956
		public string curMeetingAdr = "";

		// Token: 0x04002EB5 RID: 11957
		private DataSet ds = new DataSet("dsMeetingAdr");
	}
}
