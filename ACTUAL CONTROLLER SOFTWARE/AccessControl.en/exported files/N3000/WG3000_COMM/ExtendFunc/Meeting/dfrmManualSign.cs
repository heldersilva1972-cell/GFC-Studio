using System;
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
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FC RID: 764
	public partial class dfrmManualSign : frmN3000
	{
		// Token: 0x06001671 RID: 5745 RVA: 0x001CB3BA File Offset: 0x001CA3BA
		public dfrmManualSign()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x001CB3F4 File Offset: 0x001CA3F4
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x001CB404 File Offset: 0x001CA404
		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvSelectedUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvSelectedUsers.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvSelectedUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvSelectedUsers.SelectedRows[0].Index;
				}
				string text = " UPDATE t_d_MeetingConsumer ";
				if (wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					text,
					"SET f_SignWay = 0  , f_SignRealTime = NULL  , f_RecID = 0  WHERE  f_MeetingNO = ",
					wgTools.PrepareStrNUnicode(this.curMeetingNo),
					" AND f_ConsumerID = ",
					this.dgvSelectedUsers.Rows[num].Cells[0].Value.ToString()
				})) == 1)
				{
					base.Close();
					base.DialogResult = DialogResult.OK;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x001CB51C File Offset: 0x001CA51C
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvSelectedUsers);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x001CB530 File Offset: 0x001CA530
		private void btnOk_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvSelectedUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvSelectedUsers.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvSelectedUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvSelectedUsers.SelectedRows[0].Index;
				}
				if (this.curMode == "" || this.curMode.ToUpper() == "ManualSign".ToUpper())
				{
					string text = " UPDATE t_d_MeetingConsumer ";
					if (wgAppConfig.runUpdateSql(string.Concat(new string[]
					{
						text,
						"SET f_SignWay = 1  , f_SignRealTime = ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , f_RecID = 0  WHERE  f_MeetingNO = ",
						wgTools.PrepareStrNUnicode(this.curMeetingNo),
						" AND f_ConsumerID = ",
						this.dgvSelectedUsers.Rows[num].Cells[0].Value.ToString()
					})) == 1)
					{
						base.Close();
						base.DialogResult = DialogResult.OK;
						return;
					}
				}
				if (this.curMode.ToUpper() == "Leave".ToUpper())
				{
					string text2 = " UPDATE t_d_MeetingConsumer ";
					if (wgAppConfig.runUpdateSql(string.Concat(new string[]
					{
						text2,
						"SET f_SignWay = 2  , f_SignRealTime = ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , f_RecID = 0  WHERE  f_MeetingNO = ",
						wgTools.PrepareStrNUnicode(this.curMeetingNo),
						" AND f_ConsumerID = ",
						this.dgvSelectedUsers.Rows[num].Cells[0].Value.ToString()
					})) == 1)
					{
						base.Close();
						base.DialogResult = DialogResult.OK;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x001CB7D8 File Offset: 0x001CA7D8
		private void dfrmManualSign_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x001CB7F0 File Offset: 0x001CA7F0
		private void dfrmManualSign_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x06001678 RID: 5752 RVA: 0x001CB864 File Offset: 0x001CA864
		private void dfrmManualSign_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[5]);
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			base.KeyPreview = true;
			try
			{
				if (this.curMeetingNo == "")
				{
					base.Close();
					return;
				}
				this.dtpMeetingDate.Value = DateTime.Now.Date;
				this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd HH:mm:ss"));
				this.dtpMeetingTime.CustomFormat = "HH:mm:ss";
				this.dtpMeetingTime.Format = DateTimePickerFormat.Custom;
				this.dtUser1 = new DataTable();
				string text;
				if (wgAppConfig.IsAccessDB)
				{
					text = " SELECT  t_b_Consumer.f_ConsumerID ";
					text = text + " , f_MeetingIdentity,' ' as  f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO  , f_Seat  ,IIF (t_d_MeetingConsumer.f_MeetingIdentity IS NULL, 0,  IIF (  t_d_MeetingConsumer.f_MeetingIdentity <0 , 0 , 1 )) AS f_Selected  , f_GroupID  FROM t_b_Consumer  INNER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo) + ") ORDER BY f_ConsumerNO ASC ";
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbDataAdapter.Fill(this.dtUser1);
							}
						}
						goto IL_01CF;
					}
				}
				text = " SELECT  t_b_Consumer.f_ConsumerID ";
				text = text + " , f_MeetingIdentity,' ' as f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO  , f_Seat  , CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity IS NULL THEN 0 ELSE CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity < 0 THEN 0 ELSE 1 END END AS f_Selected  , f_GroupID  FROM t_b_Consumer  INNER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo) + ") ORDER BY f_ConsumerNO ASC ";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(this.dtUser1);
						}
					}
				}
				IL_01CF:
				for (int i = 0; i < this.dtUser1.Rows.Count; i++)
				{
					DataRow dataRow = this.dtUser1.Rows[i];
					if (!string.IsNullOrEmpty(dataRow["f_MeetingIdentity"].ToString()) && (int)dataRow["f_MeetingIdentity"] >= 0)
					{
						dataRow["f_MeetingIdentityStr"] = frmMeetings.getStrMeetingIdentity((long)((int)dataRow["f_MeetingIdentity"]));
					}
				}
				this.dtUser1.AcceptChanges();
				DataView dataView = new DataView(this.dtUser1);
				for (int j = 0; j < dataView.Table.Columns.Count; j++)
				{
					this.dgvSelectedUsers.Columns[j].DataPropertyName = this.dtUser1.Columns[j].ColumnName;
				}
				this.dgvSelectedUsers.DataSource = dataView;
				this.dgvSelectedUsers.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			Cursor.Current = cursor;
		}

		// Token: 0x04002E8D RID: 11917
		private DataTable dtUser1;

		// Token: 0x04002E8E RID: 11918
		public string curMeetingNo = "";

		// Token: 0x04002E8F RID: 11919
		public string curMode = "";

		// Token: 0x04002E90 RID: 11920
		private DataSet ds = new DataSet();
	}
}
