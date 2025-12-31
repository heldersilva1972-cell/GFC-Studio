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
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200000F RID: 15
	public partial class dfrmDeleteRecords : frmN3000
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x0001D06B File Offset: 0x0001C06B
		public dfrmDeleteRecords()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0001D07C File Offset: 0x0001C07C
		private void btnBackupDatabase_Click(object sender, EventArgs e)
		{
			using (dfrmDbCompact dfrmDbCompact = new dfrmDbCompact())
			{
				dfrmDbCompact.ShowDialog(this);
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0001D0B4 File Offset: 0x0001C0B4
		private void btnDeleteAllSwipeRecords_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show((sender as Button).Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				try
				{
					if (wgAppConfig.IsAccessDB)
					{
						wgAppConfig.runUpdateSql("Delete From t_d_SwipeRecord");
					}
					else
					{
						wgAppConfig.runUpdateSql("TRUNCATE TABLE t_d_SwipeRecord");
					}
					wgAppConfig.wgLog((sender as Button).Text);
					XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
					base.Close();
				}
				catch (Exception)
				{
					XMessageBox.Show(this.Text + CommonStr.strFailed);
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0001D15C File Offset: 0x0001C15C
		private void btnDeleteLog_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show((sender as Button).Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				try
				{
					if (wgAppConfig.IsAccessDB)
					{
						wgAppConfig.runUpdateSql("Delete From t_s_wglog");
					}
					else
					{
						wgAppConfig.runUpdateSql("TRUNCATE TABLE t_s_wglog");
					}
					wgAppConfig.wgLog((sender as Button).Text);
					XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
					base.Close();
				}
				catch (Exception)
				{
					XMessageBox.Show(this.Text + CommonStr.strFailed);
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0001D204 File Offset: 0x0001C204
		private void btnDeleteOldSwipeRecords_Click(object sender, EventArgs e)
		{
			string text = "";
			text = (sender as Button).Text + ": " + this.lblIndex.Text + this.nudSwipeRecordIndex.Value.ToString();
			if (this.nudIndexMin.Visible)
			{
				text = text + " ,   " + this.lblIndexMin.Text + this.nudIndexMin.Value.ToString();
			}
			text += "? ";
			if (XMessageBox.Show(text, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
			{
				return;
			}
			int num = -1;
			Cursor.Current = Cursors.WaitCursor;
			string text2 = "DELETE FROM t_d_SwipeRecord Where f_RecID <" + this.nudSwipeRecordIndex.Value.ToString();
			if (this.nudIndexMin.Visible)
			{
				text2 = text2 + "  AND  f_RecID >= " + this.nudIndexMin.Value.ToString();
			}
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
					{
						oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						num = oleDbCommand.ExecuteNonQuery();
					}
					goto IL_0198;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.Open();
				}
				using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
				{
					sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					num = sqlCommand.ExecuteNonQuery();
				}
			}
			IL_0198:
			Cursor.Current = Cursors.Default;
			if (num >= 0)
			{
				wgAppConfig.wgLog((sender as Button).Text + ": " + text2);
				wgAppConfig.wgLogWithoutDB(text + "\r\n" + text2, EventLogEntryType.Information, null);
				XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
				base.Close();
				return;
			}
			XMessageBox.Show(this.Text + CommonStr.strFailed);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0001D44C File Offset: 0x0001C44C
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0001D454 File Offset: 0x0001C454
		private void btnKeepSwipeRecords_Click(object sender, EventArgs e)
		{
			string text = "";
			text = (sender as Button).Text + ": " + this.label3.Text + this.numericUpDown2.Value.ToString();
			int valBySql = wgAppConfig.getValBySql("SELECT max(f_RecID) from t_d_SwipeRecord");
			if (valBySql <= this.numericUpDown2.Value)
			{
				XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
				return;
			}
			int num = -1;
			Cursor.Current = Cursors.WaitCursor;
			string text2 = "DELETE FROM t_d_SwipeRecord Where f_RecID <" + (valBySql - this.numericUpDown2.Value).ToString();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
					{
						oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						num = oleDbCommand.ExecuteNonQuery();
					}
					goto IL_0160;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.Open();
				}
				using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
				{
					sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					num = sqlCommand.ExecuteNonQuery();
				}
			}
			IL_0160:
			Cursor.Current = Cursors.Default;
			if (num >= 0)
			{
				wgAppConfig.wgLog((sender as Button).Text + ": " + text2);
				wgAppConfig.wgLogWithoutDB(text + "\r\n" + text2, EventLogEntryType.Information, null);
				XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
				base.Close();
				return;
			}
			XMessageBox.Show(this.Text + CommonStr.strFailed);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0001D664 File Offset: 0x0001C664
		private void dfrmDeleteRecords_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.lblIndexMin.Visible = true;
				this.nudIndexMin.Visible = true;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0001D6C0 File Offset: 0x0001C6C0
		private void dfrmDeleteRecords_Load(object sender, EventArgs e)
		{
		}
	}
}
