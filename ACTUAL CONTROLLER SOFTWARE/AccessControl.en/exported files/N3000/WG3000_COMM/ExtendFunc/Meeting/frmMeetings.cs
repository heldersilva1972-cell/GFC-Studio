using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x02000302 RID: 770
	public partial class frmMeetings : frmN3000
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x001DC405 File Offset: 0x001DB405
		public frmMeetings()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x001DC420 File Offset: 0x001DB420
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmMeetingSet dfrmMeetingSet = new dfrmMeetingSet())
			{
				dfrmMeetingSet.ShowDialog(this);
				this.loadMeetingData();
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x001DC460 File Offset: 0x001DB460
		private void btnAddress_Click(object sender, EventArgs e)
		{
			using (dfrmMeetingAdr dfrmMeetingAdr = new dfrmMeetingAdr())
			{
				dfrmMeetingAdr.ShowDialog();
			}
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x001DC498 File Offset: 0x001DB498
		private void btnDelete_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvMain.SelectedRows.Count <= 0)
			{
				if (this.dgvMain.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvMain.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvMain.SelectedRows[0].Index;
			}
			string text = string.Format("{0}\r\n\r\n{1}:  {2}", this.btnDelete.Text, this.dgvMain.Columns[0].HeaderText, this.dgvMain.Rows[num].Cells[0].Value.ToString());
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				wgAppConfig.runUpdateSql(" DELETE FROM t_d_Meeting WHERE [f_MeetingNO]= " + wgTools.PrepareStrNUnicode(this.dgvMain.Rows[num].Cells[0].Value.ToString()));
				wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingConsumer WHERE [f_MeetingNO]= " + wgTools.PrepareStrNUnicode(this.dgvMain.Rows[num].Cells[0].Value.ToString()));
				this.loadMeetingData();
			}
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x001DC5F0 File Offset: 0x001DB5F0
		private void btnEdit_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvMain.Rows.Count > 0)
			{
				try
				{
					num = this.dgvMain.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvMain.SelectedRows.Count <= 0)
			{
				if (this.dgvMain.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvMain.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvMain.SelectedRows[0].Index;
			}
			using (dfrmMeetingSet dfrmMeetingSet = new dfrmMeetingSet())
			{
				dfrmMeetingSet.curMeetingNo = this.dgvMain.Rows[num2].Cells[0].Value.ToString();
				if (dfrmMeetingSet.ShowDialog(this) == DialogResult.OK)
				{
					this.loadMeetingData();
				}
			}
			if (this.dgvMain.RowCount > 0)
			{
				if (this.dgvMain.RowCount > num)
				{
					this.dgvMain.CurrentCell = this.dgvMain[1, num];
					return;
				}
				this.dgvMain.CurrentCell = this.dgvMain[1, this.dgvMain.RowCount - 1];
			}
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x001DC744 File Offset: 0x001DB744
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x001DC74C File Offset: 0x001DB74C
		private void btnExport_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcel(this.dgvMain, this.Text);
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x001DC760 File Offset: 0x001DB760
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x001DC774 File Offset: 0x001DB774
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x001DC788 File Offset: 0x001DB788
		private void btnRealtimeSign_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvMain.SelectedRows.Count <= 0)
			{
				if (this.dgvMain.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvMain.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvMain.SelectedRows[0].Index;
			}
			try
			{
				DataView dataView = new DataView(this.dv.Table);
				dataView.RowFilter = "f_MeetingNO = " + wgTools.PrepareStr(this.dgvMain.Rows[num].Cells[0].Value.ToString());
				if (dataView.Count > 0)
				{
					if (!(((DateTime)dataView[0]["f_MeetingDateTime"]).ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")))
					{
						DateTime dateTime = (DateTime)dataView[0]["f_MeetingDateTime"];
						if (XMessageBox.Show(string.Concat(new string[]
						{
							dataView[0]["f_MeetingName"].ToString(),
							"\r\n\r\n",
							string.Format(CommonStr.strMeetingDate + ": ", new object[0]),
							dateTime.ToString("yyyy-MM-dd"),
							", ",
							CommonStr.strMeetingSystemDate,
							": ",
							DateTime.Now.ToString("yyyy-MM-dd"),
							" , ",
							CommonStr.strMeetingMismatch,
							"?"
						}), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
						{
							return;
						}
					}
					dfrmMeetingSign dfrmMeetingSign = new dfrmMeetingSign();
					dfrmMeetingSign.curMeetingNo = this.dgvMain.Rows[num].Cells[0].Value.ToString();
					base.Hide();
					dfrmMeetingSign.ShowDialog(this);
					base.Close();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x001DC9C8 File Offset: 0x001DB9C8
		private void btnStat_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvMain.SelectedRows.Count <= 0)
			{
				if (this.dgvMain.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvMain.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvMain.SelectedRows[0].Index;
			}
			try
			{
				if (new DataView(this.dv.Table)
				{
					RowFilter = "f_MeetingNO = " + wgTools.PrepareStr(this.dgvMain.Rows[num].Cells[0].Value.ToString())
				}.Count > 0)
				{
					new dfrmMeetingStatDetail
					{
						curMeetingNo = this.dgvMain.Rows[num].Cells[0].Value.ToString()
					}.ShowDialog(this);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x001DCAD0 File Offset: 0x001DBAD0
		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvMain.SelectedRows.Count <= 0)
			{
				if (this.dgvMain.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvMain.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvMain.SelectedRows[0].Index;
			}
			string text = Strings.Format(DateTime.Now, "yyyyMMdd_HHmmss");
			string text2 = string.Format("{0}\r\n\r\n{1}:  {2}", this.copyToolStripMenuItem.Text, this.dgvMain.Columns[0].HeaderText, this.dgvMain.Rows[num].Cells[0].Value.ToString()) + string.Format("\r\n\r\n{0}:  {1}", this.dgvMain.Columns[1].HeaderText, this.dgvMain.Rows[num].Cells[1].Value.ToString()) + string.Format("\r\n\r\n\r\n{0}:  {1}", CommonStr.strNewMettingNO, text);
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text2), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_d_Meeting SELECT {0} as [f_MeetingNO], [f_MeetingName], [f_MeetingAdr], [f_MeetingDateTime], [f_SignStartTime], [f_SignEndTime], [f_Content], [f_Notes] FROM t_d_Meeting WHERE [f_MeetingNO]= " + wgTools.PrepareStrNUnicode(this.dgvMain.Rows[num].Cells[0].Value.ToString()), wgTools.PrepareStr(text)));
				wgAppConfig.runUpdateSql(string.Format(" INSERT INTO t_d_MeetingConsumer SELECT {0} as [f_MeetingNO], [f_ConsumerID], [f_MeetingIdentity], [f_Seat],0 as [f_SignWay],null as [f_SignRealTime] ,0 as [f_RecID] ,null as [f_Notes]   FROM t_d_MeetingConsumer WHERE [f_MeetingNO]= " + wgTools.PrepareStrNUnicode(this.dgvMain.Rows[num].Cells[0].Value.ToString()), wgTools.PrepareStr(text)));
				this.loadMeetingData();
			}
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x001DCCAE File Offset: 0x001DBCAE
		private void dgvMain_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x001DCCBB File Offset: 0x001DBCBB
		private void frmMeetings_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x001DCCD0 File Offset: 0x001DBCD0
		private void frmMeetings_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x060016F5 RID: 5877 RVA: 0x001DCD44 File Offset: 0x001DBD44
		private void frmMeetings_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.btnAddress.Text = wgAppConfig.ReplaceMeeting(this.btnAddress.Text);
			this.btnAdd.Text = wgAppConfig.ReplaceMeeting(this.btnAdd.Text);
			this.btnEdit.Text = wgAppConfig.ReplaceMeeting(this.btnEdit.Text);
			this.btnDelete.Text = wgAppConfig.ReplaceMeeting(this.btnDelete.Text);
			for (int i = 0; i < this.dgvMain.Columns.Count; i++)
			{
				this.dgvMain.Columns[i].HeaderText = wgAppConfig.ReplaceMeeting(this.dgvMain.Columns[i].HeaderText);
			}
			this.loadOperatorPrivilege();
			this.loadMeetingData();
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x001DCE30 File Offset: 0x001DBE30
		public static string getStrMeetingIdentity(long id)
		{
			string text = "";
			try
			{
				long num = id;
				if (num <= 5L && num >= 0L)
				{
					switch ((int)num)
					{
					case 0:
						text = CommonStr.strMeetingIdentity0;
						return wgAppConfig.ReplaceSpecialWord(text, "KEY_strDelegate");
					case 1:
						text = CommonStr.strMeetingIdentity1;
						return wgAppConfig.ReplaceSpecialWord(text, "KEY_strNonvotingDelegate");
					case 2:
						text = CommonStr.strMeetingIdentity2;
						return wgAppConfig.ReplaceSpecialWord(text, "KEY_strInvitational");
					case 3:
						text = CommonStr.strMeetingIdentity3;
						return wgAppConfig.ReplaceSpecialWord(text, "KEY_strAudit");
					case 4:
						return CommonStr.strMeetingIdentity4;
					case 5:
						return CommonStr.strMeetingIdentity5;
					}
				}
				text = id.ToString();
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x001DCEF8 File Offset: 0x001DBEF8
		public static string getStrSignWay(long id)
		{
			string text = "";
			try
			{
				long num = id;
				if (num <= 2L && num >= 0L)
				{
					switch ((int)num)
					{
					case 0:
						return CommonStr.strSignWay0;
					case 1:
						return CommonStr.strSignWay1;
					case 2:
						return CommonStr.strSignWay2;
					}
				}
				text = id.ToString();
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x001DCF68 File Offset: 0x001DBF68
		private void loadMeetingData()
		{
			string text = "SELECT [f_MeetingNO], [f_MeetingName], [f_MeetingDateTime], [f_MeetingAdr], [f_Content], [f_Notes] FROM t_d_Meeting ";
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dt);
						}
					}
					goto IL_00CF;
				}
			}
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
			IL_00CF:
			DataGridView dataGridView = this.dgvMain;
			dataGridView.AutoGenerateColumns = false;
			dataGridView.DataSource = this.dv;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				dataGridView.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
				dataGridView.Columns[i].Name = this.dv.Table.Columns[i].ColumnName;
			}
			wgAppConfig.setDisplayFormatDate(dataGridView, "f_MeetingDateTime", wgTools.DisplayFormat_DateYMDHMSWeek);
			if (this.dv.Count > 0)
			{
				this.btnAdd.Enabled = true;
				this.btnEdit.Enabled = true;
				this.btnDelete.Enabled = true;
				this.btnPrint.Enabled = true;
				return;
			}
			this.btnAdd.Enabled = true;
			this.btnEdit.Enabled = false;
			this.btnDelete.Enabled = false;
			this.btnPrint.Enabled = false;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x001DD1AC File Offset: 0x001DC1AC
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuMeeting";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnEdit.Visible = false;
				this.btnDelete.Visible = false;
				this.btnRealtimeSign.Visible = false;
			}
		}

		// Token: 0x04002FA1 RID: 12193
		private DataTable dt;

		// Token: 0x04002FA2 RID: 12194
		private DataView dv;
	}
}
