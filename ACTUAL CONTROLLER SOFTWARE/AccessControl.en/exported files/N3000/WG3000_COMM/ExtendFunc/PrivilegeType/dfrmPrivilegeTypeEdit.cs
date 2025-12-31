using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031B RID: 795
	public partial class dfrmPrivilegeTypeEdit : frmN3000
	{
		// Token: 0x0600188A RID: 6282 RVA: 0x00200E14 File Offset: 0x001FFE14
		public dfrmPrivilegeTypeEdit()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00200E22 File Offset: 0x001FFE22
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00200E34 File Offset: 0x001FFE34
		private void btnOK_Click(object sender, EventArgs e)
		{
			int num = -1;
			try
			{
				string text = "SELECT  COUNT(*) FROM t_d_PrivilegeType WHERE f_PrivilegeTypeName =";
				object obj = text + wgTools.PrepareStrNUnicode(this.txtPrivilegeName.Text.ToString());
				num = wgAppConfig.getValBySql(string.Concat(new object[] { obj, " AND NOT (f_PrivilegeTypeID = ", this.PrivilegeTypeID, ")" }));
				if (num > 0)
				{
					XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (this.PrivilegeTypeID == 0)
				{
					text = " INSERT INTO  t_d_PrivilegeType (f_PrivilegeTypeName, f_Note) VALUES(";
					num = wgAppConfig.runUpdateSql(string.Concat(new string[]
					{
						text,
						wgTools.PrepareStrNUnicode(this.txtPrivilegeName.Text.ToString()),
						",",
						wgTools.PrepareStrNUnicode(this.txtNote.Text.ToString()),
						")"
					}));
				}
				else
				{
					num = wgAppConfig.runUpdateSql(string.Concat(new object[]
					{
						" UPDATE  t_d_PrivilegeType SET f_PrivilegeTypeName =",
						wgTools.PrepareStrNUnicode(this.txtPrivilegeName.Text.ToString()),
						", f_Note =",
						wgTools.PrepareStrNUnicode(this.txtNote.Text.ToString()),
						" WHERE f_PrivilegeTypeID = ",
						this.PrivilegeTypeID
					}));
				}
				num = wgAppConfig.getValBySql("SELECT TOP 1  f_PrivilegeTypeID FROM t_d_PrivilegeType WHERE f_PrivilegeTypeName =" + wgTools.PrepareStrNUnicode(this.txtPrivilegeName.Text.ToString()));
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			if (num >= 0)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
				return;
			}
			XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00201014 File Offset: 0x00200014
		private void dfrmPrivilegeSingle_Load(object sender, EventArgs e)
		{
			try
			{
				string text = "SELECT  * FROM t_d_PrivilegeType ";
				object obj = text;
				text = string.Concat(new object[] { obj, " WHERE (f_PrivilegeTypeID = ", this.PrivilegeTypeID, ")" });
				DataSet dataSet = new DataSet();
				DataTable dataTable = new DataTable();
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
						{
							dataAdapter.Fill(dataSet);
						}
					}
				}
				dataTable = dataSet.Tables[0];
				if (dataTable.Rows.Count > 0)
				{
					this.txtPrivilegeName.Text = (string)dataTable.Rows[0]["f_PrivilegeTypeName"];
					this.txtNote.Text = (string)dataTable.Rows[0]["f_Note"];
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x002011E4 File Offset: 0x002001E4
		private void txtPrivilegeName_TextChanged(object sender, EventArgs e)
		{
			this.btnOK.Enabled = this.txtPrivilegeName.TextLength > 0;
		}

		// Token: 0x04003254 RID: 12884
		public int PrivilegeTypeID;
	}
}
