using System;
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

namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FD RID: 765
	public partial class dfrmMeetingAdr : frmN3000
	{
		// Token: 0x0600167B RID: 5755 RVA: 0x001CC469 File Offset: 0x001CB469
		public dfrmMeetingAdr()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelected);
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x001CC494 File Offset: 0x001CB494
		private void btnAddMeetingAdr_Click(object sender, EventArgs e)
		{
			try
			{
				dfrmMeetingAdrSet dfrmMeetingAdrSet = new dfrmMeetingAdrSet();
				if (dfrmMeetingAdrSet.ShowDialog() == DialogResult.OK)
				{
					this.loadData();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x001CC4E8 File Offset: 0x001CB4E8
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x001CC4F0 File Offset: 0x001CB4F0
		private void btnDeleteMeetingAdr_Click(object sender, EventArgs e)
		{
			try
			{
				if (XMessageBox.Show(this.btnDeleteMeetingAdr.Text + ":" + ((DataRowView)this.lstMeetingAdr.SelectedItems[0]).Row[0].ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(((DataRowView)this.lstMeetingAdr.SelectedItems[0]).Row[0]));
					this.loadData();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x001CC5B8 File Offset: 0x001CB5B8
		private void btnSelectReader_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.lstMeetingAdr.SelectedItems.Count > 0 && new dfrmMeetingAdrSet
				{
					curMeetingAdr = ((DataRowView)this.lstMeetingAdr.SelectedItems[0]).Row[0].ToString()
				}.ShowDialog() == DialogResult.OK)
				{
					this.loadData();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x001CC64C File Offset: 0x001CB64C
		private void dfrmMeetingAdr_Load(object sender, EventArgs e)
		{
			this.loadData();
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.Label11.Text = wgAppConfig.ReplaceMeeting(this.Label11.Text);
			bool flag = false;
			string text = "mnuMeeting";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAddMeetingAdr.Visible = false;
				this.btnDeleteMeetingAdr.Visible = false;
				this.btnSelectReader.Visible = false;
			}
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x001CC6C4 File Offset: 0x001CB6C4
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
				this.ds.Clear();
				SqlCommand sqlCommand = new SqlCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", sqlConnection);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
				sqlDataAdapter.Fill(this.ds, "t_d_MeetingAdr");
				this.lstMeetingAdr.DisplayMember = "f_MeetingAdr";
				this.lstMeetingAdr.DataSource = this.ds.Tables["t_d_MeetingAdr"];
				sqlCommand = new SqlCommand("Select t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 1 as f_Selected,t_d_MeetingAdr.f_MeetingAdr from t_b_reader,t_d_MeetingAdr  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID ", sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(this.ds, "t_d_MeetingAdrReader");
				this.dv = new DataView(this.ds.Tables["t_d_MeetingAdrReader"]);
				this.dv.RowFilter = "1<0";
				this.dv.Sort = "f_ReaderID ASC ";
				if (this.lstMeetingAdr.SelectedItems.Count > 0)
				{
					this.dv.RowFilter = " f_MeetingAdr = " + wgTools.PrepareStr(((DataRowView)this.lstMeetingAdr.SelectedItems[0]).Row[0]);
					this.btnSelectReader.Enabled = true;
					this.btnDeleteMeetingAdr.Enabled = true;
				}
				DataTable dataTable = this.ds.Tables["t_d_MeetingAdrReader"];
				for (int i = 0; i < this.dgvSelected.Columns.Count; i++)
				{
					this.dgvSelected.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
				}
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dv;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x001CC8E8 File Offset: 0x001CB8E8
		public void loadData_Acc()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			try
			{
				this.ds.Clear();
				OleDbCommand oleDbCommand = new OleDbCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "t_d_MeetingAdr");
				this.lstMeetingAdr.DisplayMember = "f_MeetingAdr";
				this.lstMeetingAdr.DataSource = this.ds.Tables["t_d_MeetingAdr"];
				oleDbCommand = new OleDbCommand("Select t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 1 as f_Selected,t_d_MeetingAdr.f_MeetingAdr from t_b_reader,t_d_MeetingAdr  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "t_d_MeetingAdrReader");
				this.dv = new DataView(this.ds.Tables["t_d_MeetingAdrReader"]);
				this.dv.RowFilter = "1<0";
				this.dv.Sort = "f_ReaderID ASC ";
				if (this.lstMeetingAdr.SelectedItems.Count > 0)
				{
					this.dv.RowFilter = " f_MeetingAdr = " + wgTools.PrepareStr(((DataRowView)this.lstMeetingAdr.SelectedItems[0]).Row[0]);
					this.btnSelectReader.Enabled = true;
					this.btnDeleteMeetingAdr.Enabled = true;
				}
				DataTable dataTable = this.ds.Tables["t_d_MeetingAdrReader"];
				for (int i = 0; i < this.dgvSelected.Columns.Count; i++)
				{
					this.dgvSelected.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
				}
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dv;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x001CCAF4 File Offset: 0x001CBAF4
		private void lstMeetingAdr_DoubleClick(object sender, EventArgs e)
		{
			this.btnSelectReader.PerformClick();
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x001CCB04 File Offset: 0x001CBB04
		private void lstMeetingAdr_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.dv != null)
				{
					if (this.lstMeetingAdr.SelectedItems.Count > 0)
					{
						this.dv.RowFilter = " f_MeetingAdr = " + wgTools.PrepareStr(((DataRowView)this.lstMeetingAdr.SelectedItems[0]).Row[0]);
						this.btnSelectReader.Enabled = true;
						this.btnDeleteMeetingAdr.Enabled = true;
					}
					else
					{
						this.dv.RowFilter = " 1<0 ";
						this.btnSelectReader.Enabled = false;
						this.btnDeleteMeetingAdr.Enabled = false;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x04002EA3 RID: 11939
		private DataView dv;

		// Token: 0x04002EA4 RID: 11940
		private DataSet ds = new DataSet("dsMeetingAdr");
	}
}
