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

namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x02000301 RID: 769
	public partial class dfrmMeetingStatDetail : frmN3000
	{
		// Token: 0x060016D0 RID: 5840 RVA: 0x001D91E4 File Offset: 0x001D81E4
		public dfrmMeetingStatDetail()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
			wgAppConfig.custDataGridview(ref this.dgvStat);
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x001D9292 File Offset: 0x001D8292
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x001D92A4 File Offset: 0x001D82A4
		private void btnExport_Click(object sender, EventArgs e)
		{
			if (this.dgvMain.Visible)
			{
				wgAppConfig.exportToExcel(this.dgvMain, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
				return;
			}
			wgAppConfig.exportToExcel(this.dgvStat, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x001D9324 File Offset: 0x001D8324
		private void btnLeave_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmManualSign dfrmManualSign = new dfrmManualSign())
				{
					dfrmManualSign.curMeetingNo = this.curMeetingNo;
					dfrmManualSign.curMode = "Leave";
					dfrmManualSign.Text = this.btnLeave.Text;
					if (dfrmManualSign.ShowDialog(this) == DialogResult.OK)
					{
						int selectedIndex = this.tabControl1.SelectedIndex;
						this.fillMeetingNum();
						this.tabControl1.SelectedIndex = 2;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x001D93D0 File Offset: 0x001D83D0
		private void btnManualSign_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmManualSign dfrmManualSign = new dfrmManualSign())
				{
					dfrmManualSign.curMeetingNo = this.curMeetingNo;
					dfrmManualSign.Text = this.btnManualSign.Text;
					if (dfrmManualSign.ShowDialog(this) == DialogResult.OK)
					{
						int selectedIndex = this.tabControl1.SelectedIndex;
						this.fillMeetingNum();
						this.tabControl1.SelectedIndex = selectedIndex;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x001D9474 File Offset: 0x001D8474
		private void btnOk_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x001D9483 File Offset: 0x001D8483
		private void btnOption_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x001D9488 File Offset: 0x001D8488
		private void btnPrint_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.dgvMain.Visible)
				{
					wgAppConfig.printdgv(this.dgvMain, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
				}
				else
				{
					wgAppConfig.printdgv(this.dgvStat, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x001D9550 File Offset: 0x001D8550
		private void btnRecreate_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				wgAppConfig.runUpdateSql(" UPDATE t_d_MeetingConsumer  SET [f_SignRealTime] = NULL  ,[f_RecID] = 0  WHERE f_SignWay = 0 AND  f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo));
				this.lngDealtRecordID = -1L;
				this.fillMeetingRecord(this.curMeetingNo);
				int selectedIndex = this.tabControl1.SelectedIndex;
				this.fillMeetingNum();
				this.tabControl1.SelectedIndex = selectedIndex;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.Cursor = Cursors.Default;
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x001D95F4 File Offset: 0x001D85F4
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.tabControl1.SelectedIndex;
			this.fillMeetingNum();
			this.tabControl1.SelectedIndex = selectedIndex;
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x001D961F File Offset: 0x001D861F
		private void chkActive_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x001D9621 File Offset: 0x001D8621
		private void dfrmMeetingStatDetail_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x001D9638 File Offset: 0x001D8638
		private void dfrmMeetingStatDetail_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x060016DD RID: 5853 RVA: 0x001D96AC File Offset: 0x001D86AC
		private void dfrmStd_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.btnManualSign.Text = wgAppConfig.ReplaceMeeting(this.btnManualSign.Text);
			bool flag = false;
			string text = "mnuMeeting";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnManualSign.Enabled = false;
				this.btnLeave.Enabled = false;
			}
			try
			{
				if (this.curMeetingNo == "")
				{
					base.DialogResult = DialogResult.Cancel;
					base.Close();
				}
				else
				{
					TabPage selectedTab = this.tabControl1.SelectedTab;
					this.fillMeetingNum();
					if (!string.IsNullOrEmpty(this.meetingName))
					{
						this.Text = this.Text + "[" + this.meetingName + "]";
					}
					this.tabControl1.SelectedTab = selectedTab;
					this.tabControl1_SelectedIndexChanged(null, null);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x001D97C4 File Offset: 0x001D87C4
		public void fillMeetingNum()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.fillMeetingNum_Acc();
				return;
			}
			try
			{
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), sqlConnection).ExecuteReader();
				DateTime dateTime = DateTime.Now;
				if (sqlDataReader.Read())
				{
					dateTime = (DateTime)sqlDataReader["f_MeetingDateTime"];
					this.meetingName = sqlDataReader["f_MeetingName"].ToString();
				}
				sqlDataReader.Close();
				int i;
				for (i = 0; i <= 5; i++)
				{
					this.arrMeetingNum[0, i] = 0L;
					this.arrMeetingNum[1, i] = 0L;
					this.arrMeetingNum[2, i] = 0L;
					this.arrMeetingNum[3, i] = 0L;
					this.arrMeetingNum[4, i] = 0L;
					this.arrMeetingNum[5, i] = 0L;
					this.arrMeetingNum[6, i] = 0L;
				}
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT  a.f_RecID,b.f_ConsumerName, '' as f_MeetingIdentityStr, a.f_Seat, a.f_SignRealTime,'' as f_SignWayStr, a.f_SignWay, a.f_MeetingIdentity  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), sqlConnection);
				this.ds.Clear();
				sqlDataAdapter.Fill(this.ds, "t_d_MeetingConsumer");
				DataView dataView = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				for (int j = 0; j <= dataView.Count - 1; j++)
				{
					this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_MeetingIdentityStr"] = frmMeetings.getStrMeetingIdentity(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_MeetingIdentity"].ToString()));
					this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_SignWayStr"] = frmMeetings.getStrSignWay(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_SignWay"].ToString()));
				}
				for (int j = 0; j <= 5; j++)
				{
					dataView.RowFilter = " f_MeetingIdentity = " + j;
					if (dataView.Count > 0)
					{
						this.arrMeetingNum[j, 0] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
						this.arrMeetingNum[j, 1] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND (f_SignWay = 2) ";
						this.arrMeetingNum[j, 2] = (long)dataView.Count;
						this.arrMeetingNum[j, 3] = Math.Max(0L, this.arrMeetingNum[j, 0] - this.arrMeetingNum[j, 1] - this.arrMeetingNum[j, 2]);
						dataView.RowFilter = string.Concat(new object[]
						{
							" f_MeetingIdentity = ",
							j,
							" AND  ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1))  AND f_SignRealTime > ",
							Strings.Format(dateTime, "#yyyy-MM-dd HH:mm:ss#")
						});
						this.arrMeetingNum[j, 4] = (long)dataView.Count;
						if (this.arrMeetingNum[j, 0] > 0L)
						{
							this.arrMeetingNum[j, 5] = this.arrMeetingNum[j, 1] * 1000L / this.arrMeetingNum[j, 0];
						}
					}
					this.arrMeetingNum[6, 0] += this.arrMeetingNum[j, 0];
					this.arrMeetingNum[6, 1] += this.arrMeetingNum[j, 1];
					this.arrMeetingNum[6, 2] += this.arrMeetingNum[j, 2];
					this.arrMeetingNum[6, 3] += this.arrMeetingNum[j, 3];
					this.arrMeetingNum[6, 4] += this.arrMeetingNum[j, 4];
					this.arrMeetingNum[6, 5] += this.arrMeetingNum[j, 5];
				}
				if (this.arrMeetingNum[6, 0] > 0L)
				{
					this.arrMeetingNum[6, 5] = this.arrMeetingNum[6, 1] * 1000L / this.arrMeetingNum[6, 0];
				}
				this.dvShould = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvInFact = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvLeave = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvAbsent = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvLate = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvShould.RowFilter = "";
				this.dvInFact.RowFilter = "( f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1) ";
				this.dvLeave.RowFilter = " f_SignWay = 2 ";
				this.dvAbsent.RowFilter = " f_SignWay =0 AND f_RecID <=0  ";
				this.dvLate.RowFilter = " ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) AND f_SignRealTime > " + Strings.Format(dateTime, "#yyyy-MM-dd HH:mm:ss#");
				DataTable dataTable = new DataTable("Stat");
				DataColumn dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingIdentity";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingShould";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingInFact";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingLeave";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingAbsent";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingLate";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingRatio";
				dataTable.Columns.Add(dataColumn);
				for (int j = 0; j <= 6; j++)
				{
					DataRow dataRow = dataTable.NewRow();
					if (j == 6)
					{
						dataRow[0] = CommonStr.strMeetingSubTotal;
					}
					else
					{
						dataRow[0] = frmMeetings.getStrMeetingIdentity((long)j);
					}
					for (i = 0; i <= 4; i++)
					{
						dataRow[i + 1] = this.arrMeetingNum[j, i];
						if (dataRow[i + 1].ToString() == "0")
						{
							dataRow[i + 1] = "";
						}
					}
					dataRow[6] = this.arrMeetingNum[j, 5] / 10L + "%";
					if (string.IsNullOrEmpty(dataRow[1].ToString()))
					{
						dataRow[6] = "";
					}
					dataTable.Rows.Add(dataRow);
				}
				DataTable dataTable2 = this.ds.Tables["t_d_MeetingConsumer"];
				this.dgvMain.AutoGenerateColumns = false;
				i = 0;
				while (i < dataTable2.Columns.Count && i < this.dgvMain.ColumnCount)
				{
					this.dgvMain.Columns[i].DataPropertyName = dataTable2.Columns[i].ColumnName;
					i++;
				}
				this.dgvMain.DataSource = this.dvShould;
				this.dgvMain.DefaultCellStyle.ForeColor = Color.Black;
				wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_SignRealTime", wgTools.DisplayFormat_DateYMDHMSWeek);
				this.dgvStat.AutoGenerateColumns = false;
				i = 0;
				while (i < dataTable.Columns.Count && i < this.dgvStat.ColumnCount)
				{
					this.dgvStat.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
					i++;
				}
				this.dgvStat.DataSource = dataTable;
				this.dgvStat.DefaultCellStyle.ForeColor = Color.Black;
				this.tabControl1.SelectedIndex = 5;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x001DA19C File Offset: 0x001D919C
		public void fillMeetingNum_Acc()
		{
			try
			{
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), oleDbConnection).ExecuteReader();
				DateTime dateTime = DateTime.Now;
				if (oleDbDataReader.Read())
				{
					dateTime = (DateTime)oleDbDataReader["f_MeetingDateTime"];
					this.meetingName = oleDbDataReader["f_MeetingName"].ToString();
				}
				oleDbDataReader.Close();
				int i;
				for (i = 0; i <= 5; i++)
				{
					this.arrMeetingNum[0, i] = 0L;
					this.arrMeetingNum[1, i] = 0L;
					this.arrMeetingNum[2, i] = 0L;
					this.arrMeetingNum[3, i] = 0L;
					this.arrMeetingNum[4, i] = 0L;
					this.arrMeetingNum[5, i] = 0L;
					this.arrMeetingNum[6, i] = 0L;
				}
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter("SELECT  a.f_RecID,b.f_ConsumerName, '' as f_MeetingIdentityStr, a.f_Seat, a.f_SignRealTime,'' as f_SignWayStr, a.f_SignWay, a.f_MeetingIdentity  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), oleDbConnection);
				this.ds.Clear();
				oleDbDataAdapter.Fill(this.ds, "t_d_MeetingConsumer");
				DataView dataView = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				for (int j = 0; j <= dataView.Count - 1; j++)
				{
					this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_MeetingIdentityStr"] = frmMeetings.getStrMeetingIdentity(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_MeetingIdentity"].ToString()));
					this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_SignWayStr"] = frmMeetings.getStrSignWay(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[j]["f_SignWay"].ToString()));
				}
				for (int j = 0; j <= 5; j++)
				{
					dataView.RowFilter = " f_MeetingIdentity = " + j;
					if (dataView.Count > 0)
					{
						this.arrMeetingNum[j, 0] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
						this.arrMeetingNum[j, 1] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND (f_SignWay = 2) ";
						this.arrMeetingNum[j, 2] = (long)dataView.Count;
						this.arrMeetingNum[j, 3] = Math.Max(0L, this.arrMeetingNum[j, 0] - this.arrMeetingNum[j, 1] - this.arrMeetingNum[j, 2]);
						dataView.RowFilter = string.Concat(new object[]
						{
							" f_MeetingIdentity = ",
							j,
							" AND  ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1))  AND f_SignRealTime > ",
							Strings.Format(dateTime, "#yyyy-MM-dd HH:mm:ss#")
						});
						this.arrMeetingNum[j, 4] = (long)dataView.Count;
						if (this.arrMeetingNum[j, 0] > 0L)
						{
							this.arrMeetingNum[j, 5] = this.arrMeetingNum[j, 1] * 1000L / this.arrMeetingNum[j, 0];
						}
					}
					this.arrMeetingNum[6, 0] += this.arrMeetingNum[j, 0];
					this.arrMeetingNum[6, 1] += this.arrMeetingNum[j, 1];
					this.arrMeetingNum[6, 2] += this.arrMeetingNum[j, 2];
					this.arrMeetingNum[6, 3] += this.arrMeetingNum[j, 3];
					this.arrMeetingNum[6, 4] += this.arrMeetingNum[j, 4];
					this.arrMeetingNum[6, 5] += this.arrMeetingNum[j, 5];
				}
				if (this.arrMeetingNum[6, 0] > 0L)
				{
					this.arrMeetingNum[6, 5] = this.arrMeetingNum[6, 1] * 1000L / this.arrMeetingNum[6, 0];
				}
				this.dvShould = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvInFact = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvLeave = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvAbsent = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvLate = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				this.dvShould.RowFilter = "";
				this.dvInFact.RowFilter = "( f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1) ";
				this.dvLeave.RowFilter = " f_SignWay = 2 ";
				this.dvAbsent.RowFilter = " f_SignWay =0 AND f_RecID <=0  ";
				this.dvLate.RowFilter = " ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) AND f_SignRealTime > " + Strings.Format(dateTime, "#yyyy-MM-dd HH:mm:ss#");
				DataTable dataTable = new DataTable("Stat");
				DataColumn dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingIdentity";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingShould";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingInFact";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingLeave";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingAbsent";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingLate";
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "strMeetingRatio";
				dataTable.Columns.Add(dataColumn);
				for (int j = 0; j <= 6; j++)
				{
					DataRow dataRow = dataTable.NewRow();
					if (j == 6)
					{
						dataRow[0] = CommonStr.strMeetingSubTotal;
					}
					else
					{
						dataRow[0] = frmMeetings.getStrMeetingIdentity((long)j);
					}
					for (i = 0; i <= 4; i++)
					{
						dataRow[i + 1] = this.arrMeetingNum[j, i];
						if (dataRow[i + 1].ToString() == "0")
						{
							dataRow[i + 1] = "";
						}
					}
					dataRow[6] = this.arrMeetingNum[j, 5] / 10L + "%";
					if (string.IsNullOrEmpty(dataRow[1].ToString()))
					{
						dataRow[6] = "";
					}
					dataTable.Rows.Add(dataRow);
				}
				DataTable dataTable2 = this.ds.Tables["t_d_MeetingConsumer"];
				this.dgvMain.AutoGenerateColumns = false;
				i = 0;
				while (i < dataTable2.Columns.Count && i < this.dgvMain.ColumnCount)
				{
					this.dgvMain.Columns[i].DataPropertyName = dataTable2.Columns[i].ColumnName;
					i++;
				}
				this.dgvMain.DataSource = this.dvShould;
				this.dgvMain.DefaultCellStyle.ForeColor = Color.Black;
				wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_SignRealTime", wgTools.DisplayFormat_DateYMDHMSWeek);
				this.dgvStat.AutoGenerateColumns = false;
				i = 0;
				while (i < dataTable.Columns.Count && i < this.dgvStat.ColumnCount)
				{
					this.dgvStat.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
					i++;
				}
				this.dgvStat.DataSource = dataTable;
				this.dgvStat.DefaultCellStyle.ForeColor = Color.Black;
				this.tabControl1.SelectedIndex = 5;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x001DAB60 File Offset: 0x001D9B60
		public void fillMeetingRecord(string MeetingNo)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.fillMeetingRecord_Acc(MeetingNo);
				return;
			}
			try
			{
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				SqlCommand sqlCommand = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo), sqlConnection);
				SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
				if (sqlDataReader.Read())
				{
					this.signStarttime = (DateTime)sqlDataReader["f_SignStartTime"];
					this.signEndtime = (DateTime)sqlDataReader["f_SignEndTime"];
					this.meetingAdr = wgTools.SetObjToStr(sqlDataReader["f_MeetingAdr"]);
				}
				sqlDataReader.Close();
				if (this.lngDealtRecordID == -1L && this.meetingAdr != "")
				{
					this.queryReaderStr = "";
					string text = "Select t_b_reader.* from t_b_reader,t_d_MeetingAdr  ";
					sqlCommand = new SqlCommand(text + " , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID  AND t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.meetingAdr), sqlConnection);
					sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.HasRows)
					{
						while (sqlDataReader.Read())
						{
							if (this.queryReaderStr == "")
							{
								this.queryReaderStr = " f_ReaderID IN ( " + sqlDataReader["f_ReaderID"];
							}
							else
							{
								this.queryReaderStr = this.queryReaderStr + " , " + sqlDataReader["f_ReaderID"];
							}
							if (this.arrControllerID.IndexOf(sqlDataReader["f_ControllerID"]) < 0)
							{
								this.arrControllerID.Add(sqlDataReader["f_ControllerID"]);
							}
						}
						this.queryReaderStr += ")";
					}
					sqlDataReader.Close();
				}
				if (this.lngDealtRecordID == -1L)
				{
					this.lngDealtRecordID = 0L;
				}
				string text2 = "";
				text2 = string.Concat(new string[]
				{
					text2,
					" ([f_ReadDate]>= ",
					wgTools.PrepareStr(this.signStarttime, true, "yyyy-MM-dd H:mm:ss"),
					") AND ([f_ReadDate]<= ",
					wgTools.PrepareStr(this.signEndtime, true, "yyyy-MM-dd H:mm:ss"),
					")"
				});
				string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text3 = string.Concat(new string[]
				{
					text3,
					"        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity, t_d_SwipeRecord.f_ConsumerID ",
					string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0]),
					" WHERE t_d_SwipeRecord.f_RecID > ",
					this.lngDealtRecordID.ToString(),
					" AND  t_d_SwipeRecord.f_ConsumerID IN (SELECT f_ConsumerID FROM t_d_MeetingConsumer WHERE f_SignWay=0 AND f_RecID =0 AND  f_MeetingNO = ",
					wgTools.PrepareStrNUnicode(MeetingNo),
					" )  "
				});
				if (this.queryReaderStr != "")
				{
					text3 = text3 + " AND " + this.queryReaderStr;
				}
				sqlCommand = new SqlCommand(text3 + " AND " + text2, sqlConnection);
				sqlDataReader = sqlCommand.ExecuteReader();
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = new ArrayList();
				if (sqlDataReader.HasRows)
				{
					while (sqlDataReader.Read())
					{
						int num = arrayList.IndexOf(sqlDataReader["f_ConsumerID"]);
						if (num < 0)
						{
							arrayList.Add(sqlDataReader["f_ConsumerID"]);
							arrayList2.Add((DateTime)sqlDataReader["f_ReadDate"]);
							arrayList3.Add(sqlDataReader["f_RecID"]);
						}
						else if ((DateTime)arrayList2[num] > (DateTime)sqlDataReader["f_ReadDate"])
						{
							arrayList2[num] = (DateTime)sqlDataReader["f_ReadDate"];
							arrayList3[num] = sqlDataReader["f_RecID"];
						}
					}
				}
				sqlDataReader.Close();
				if (arrayList.Count > 0)
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						string text = " UPDATE t_d_MeetingConsumer ";
						object obj = string.Concat(new object[]
						{
							text,
							" SET [f_SignRealTime] = ",
							wgTools.PrepareStr((DateTime)arrayList2[i], true, "yyyy-MM-dd H:mm:ss"),
							" ,[f_RecID] = ",
							arrayList3[i]
						});
						text = string.Concat(new object[]
						{
							obj,
							" WHERE f_ConsumerID = ",
							arrayList[i],
							" AND  f_MeetingNO = ",
							wgTools.PrepareStrNUnicode(MeetingNo)
						});
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
					}
				}
				sqlConnection.Close();
				this.lngDealtRecordID = (long)wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x001DB068 File Offset: 0x001DA068
		public void fillMeetingRecord_Acc(string MeetingNo)
		{
			try
			{
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo), oleDbConnection);
				OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
				if (oleDbDataReader.Read())
				{
					this.signStarttime = (DateTime)oleDbDataReader["f_SignStartTime"];
					this.signEndtime = (DateTime)oleDbDataReader["f_SignEndTime"];
					this.meetingAdr = wgTools.SetObjToStr(oleDbDataReader["f_MeetingAdr"]);
				}
				oleDbDataReader.Close();
				if (this.lngDealtRecordID == -1L && this.meetingAdr != "")
				{
					this.queryReaderStr = "";
					string text = "Select t_b_reader.* from t_b_reader,t_d_MeetingAdr  ";
					oleDbDataReader = new OleDbCommand(text + " , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID  AND t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.meetingAdr), oleDbConnection).ExecuteReader();
					if (oleDbDataReader.HasRows)
					{
						while (oleDbDataReader.Read())
						{
							if (this.queryReaderStr == "")
							{
								this.queryReaderStr = " f_ReaderID IN ( " + oleDbDataReader["f_ReaderID"];
							}
							else
							{
								this.queryReaderStr = this.queryReaderStr + " , " + oleDbDataReader["f_ReaderID"];
							}
							if (this.arrControllerID.IndexOf(oleDbDataReader["f_ControllerID"]) < 0)
							{
								this.arrControllerID.Add(oleDbDataReader["f_ControllerID"]);
							}
						}
						this.queryReaderStr += ")";
					}
					oleDbDataReader.Close();
				}
				if (this.lngDealtRecordID == -1L)
				{
					this.lngDealtRecordID = 0L;
				}
				string text2 = "";
				text2 = string.Concat(new string[]
				{
					text2,
					" ([f_ReadDate]>= ",
					wgTools.PrepareStr(this.signStarttime, true, "yyyy-MM-dd H:mm:ss"),
					") AND ([f_ReadDate]<= ",
					wgTools.PrepareStr(this.signEndtime, true, "yyyy-MM-dd H:mm:ss"),
					")"
				});
				string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text3 = string.Concat(new string[]
				{
					text3,
					"        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity, t_d_SwipeRecord.f_ConsumerID ",
					string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0]),
					" WHERE t_d_SwipeRecord.f_RecID > ",
					this.lngDealtRecordID.ToString(),
					" AND  t_d_SwipeRecord.f_ConsumerID IN (SELECT f_ConsumerID FROM t_d_MeetingConsumer WHERE f_SignWay=0 AND f_RecID =0 AND  f_MeetingNO = ",
					wgTools.PrepareStrNUnicode(MeetingNo),
					" )  "
				});
				if (this.queryReaderStr != "")
				{
					text3 = text3 + " AND " + this.queryReaderStr;
				}
				oleDbCommand = new OleDbCommand(text3 + " AND " + text2, oleDbConnection);
				oleDbDataReader = oleDbCommand.ExecuteReader();
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = new ArrayList();
				if (oleDbDataReader.HasRows)
				{
					while (oleDbDataReader.Read())
					{
						int num = arrayList.IndexOf(oleDbDataReader["f_ConsumerID"]);
						if (num < 0)
						{
							arrayList.Add(oleDbDataReader["f_ConsumerID"]);
							arrayList2.Add((DateTime)oleDbDataReader["f_ReadDate"]);
							arrayList3.Add(oleDbDataReader["f_RecID"]);
						}
						else if ((DateTime)arrayList2[num] > (DateTime)oleDbDataReader["f_ReadDate"])
						{
							arrayList2[num] = (DateTime)oleDbDataReader["f_ReadDate"];
							arrayList3[num] = oleDbDataReader["f_RecID"];
						}
					}
				}
				oleDbDataReader.Close();
				if (arrayList.Count > 0)
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						string text = " UPDATE t_d_MeetingConsumer ";
						object obj = string.Concat(new object[]
						{
							text,
							" SET [f_SignRealTime] = ",
							wgTools.PrepareStr((DateTime)arrayList2[i], true, "yyyy-MM-dd H:mm:ss"),
							" ,[f_RecID] = ",
							arrayList3[i]
						});
						text = string.Concat(new object[]
						{
							obj,
							" WHERE f_ConsumerID = ",
							arrayList[i],
							" AND  f_MeetingNO = ",
							wgTools.PrepareStrNUnicode(MeetingNo)
						});
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
					}
				}
				oleDbConnection.Close();
				this.lngDealtRecordID = (long)wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x001DB560 File Offset: 0x001DA560
		private void radioButton25_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x001DB564 File Offset: 0x001DA564
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.tabControl1.SelectedIndex)
			{
			case 0:
				this.dgvMain.DataSource = this.dvShould;
				break;
			case 1:
				this.dgvMain.DataSource = this.dvInFact;
				break;
			case 2:
				this.dgvMain.DataSource = this.dvLeave;
				break;
			case 3:
				this.dgvMain.DataSource = this.dvAbsent;
				break;
			case 4:
				this.dgvMain.DataSource = this.dvLate;
				break;
			}
			if (this.tabControl1.SelectedIndex >= 0 && this.tabControl1.SelectedIndex <= 4)
			{
				this.dgvStat.Visible = false;
				this.dgvMain.Visible = true;
				base.ActiveControl = this.dgvMain;
			}
			else
			{
				this.dgvStat.Visible = true;
				this.dgvMain.Visible = false;
				base.ActiveControl = this.dgvStat;
			}
			this.dgvMain.Dock = DockStyle.Fill;
			this.dgvStat.Dock = DockStyle.Fill;
		}

		// Token: 0x04002F6F RID: 12143
		private DataView dvAbsent;

		// Token: 0x04002F70 RID: 12144
		private DataView dvInFact;

		// Token: 0x04002F71 RID: 12145
		private DataView dvLate;

		// Token: 0x04002F72 RID: 12146
		private DataView dvLeave;

		// Token: 0x04002F73 RID: 12147
		private DataView dvShould;

		// Token: 0x04002F74 RID: 12148
		private DateTime signEndtime;

		// Token: 0x04002F75 RID: 12149
		private DateTime signStarttime;

		// Token: 0x04002F76 RID: 12150
		private ArrayList arrControllerID = new ArrayList();

		// Token: 0x04002F77 RID: 12151
		private long[,] arrMeetingNum = new long[7, 6];

		// Token: 0x04002F78 RID: 12152
		private ArrayList arrSignedCardNo = new ArrayList();

		// Token: 0x04002F79 RID: 12153
		private ArrayList arrSignedSeat = new ArrayList();

		// Token: 0x04002F7A RID: 12154
		private ArrayList arrSignedUser = new ArrayList();

		// Token: 0x04002F7B RID: 12155
		public string curMeetingNo = "";

		// Token: 0x04002F7C RID: 12156
		private DataSet ds = new DataSet();

		// Token: 0x04002F7D RID: 12157
		private long lngDealtRecordID = -1L;

		// Token: 0x04002F7E RID: 12158
		private string meetingAdr = "";

		// Token: 0x04002F7F RID: 12159
		private string meetingName = "";

		// Token: 0x04002F80 RID: 12160
		private string queryReaderStr = "";

		// Token: 0x04002F81 RID: 12161
		public int selectedPage = -1;
	}
}
