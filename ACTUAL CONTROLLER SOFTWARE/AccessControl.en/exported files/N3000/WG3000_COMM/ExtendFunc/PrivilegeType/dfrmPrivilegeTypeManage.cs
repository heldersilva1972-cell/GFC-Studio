using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031C RID: 796
	public partial class dfrmPrivilegeTypeManage : frmN3000
	{
		// Token: 0x06001891 RID: 6289 RVA: 0x002014BF File Offset: 0x002004BF
		public dfrmPrivilegeTypeManage()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvPrivilegeTypes);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x002014F0 File Offset: 0x002004F0
		private void _loadData()
		{
			string text = "SELECT  [f_PrivilegeTypeID], [f_PrivilegeTypeName],[f_Note] FROM t_d_PrivilegeType ORDER BY f_PrivilegeTypeName ";
			this.dt = new DataTable("t_d_PrivilegeType");
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
					goto IL_00BF;
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
			IL_00BF:
			if (this.dt.Rows.Count > 0)
			{
				this.btnDel.Enabled = true;
				this.btnEdit.Enabled = true;
				this.btnEditDoors.Enabled = true;
				this.btnEditUsers.Enabled = true;
			}
			else
			{
				this.btnDel.Enabled = false;
				this.btnEdit.Enabled = false;
				this.btnEditDoors.Enabled = false;
				this.btnEditUsers.Enabled = false;
			}
			DataView dataView = new DataView(this.dt);
			this.dvtemp = new DataView(this.dt);
			this.dgvPrivilegeTypes.AutoGenerateColumns = false;
			this.dgvPrivilegeTypes.DataSource = dataView;
			int num = 0;
			while (num < dataView.Table.Columns.Count && num < this.dgvPrivilegeTypes.ColumnCount)
			{
				this.dgvPrivilegeTypes.Columns[num].DataPropertyName = dataView.Table.Columns[num].ColumnName;
				num++;
			}
			this.dgvPrivilegeTypes.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			new DataView(this.dt);
			this.comboBox2.DataSource = dataView;
			this.comboBox2.DisplayMember = "f_PrivilegeTypeName";
			this.comboBox2.ValueMember = "f_PrivilegeTypeID";
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00201760 File Offset: 0x00200760
		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.Text = string.Format("{0}  {1} ", this.btnAdd.Text, "");
					if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
					{
						string strNewName = dfrmInputNewName.strNewName;
						this.dvtemp.RowFilter = " f_PrivilegeTypeName= " + wgTools.PrepareStr(strNewName);
						if (this.dvtemp.Count > 0)
						{
							XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						else
						{
							wgAppConfig.runUpdateSql(" INSERT INTO  t_d_PrivilegeType (f_PrivilegeTypeName) VALUES(" + wgTools.PrepareStrNUnicode(strNewName.ToString()) + ")");
							this._loadData();
							this.dvtemp.RowFilter = " f_PrivilegeTypeName= " + wgTools.PrepareStr(strNewName);
							if (this.dvtemp.Count > 0)
							{
								this.comboBox2.Text = strNewName;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0020187C File Offset: 0x0020087C
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0020188C File Offset: 0x0020088C
		private void btnDel_Click(object sender, EventArgs e)
		{
			try
			{
				string text = "";
				int num;
				if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
				{
					if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
				}
				if (num > 1)
				{
					text = this.dgvPrivilegeTypes.Rows[num - 1].Cells[1].Value.ToString();
				}
				string text2 = this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString();
				if (XMessageBox.Show(string.Format(CommonStr.strAreYouSureDelPrivilegeType + " \r\n\r\n{0} ?", text2), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					int num2 = 0;
					this.dvtemp.RowFilter = "  f_PrivilegeTypeName = " + wgTools.PrepareStr(this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
					if (this.dvtemp.Count > 0)
					{
						num2 = (int)this.dvtemp[0]["f_PrivilegeTypeID"];
					}
					Cursor.Current = Cursors.WaitCursor;
					try
					{
						DbConnection dbConnection;
						DbCommand dbCommand;
						if (wgAppConfig.IsAccessDB)
						{
							dbConnection = new OleDbConnection(wgAppConfig.dbConString);
							dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
						}
						else
						{
							dbConnection = new SqlConnection(wgAppConfig.dbConString);
							dbCommand = new SqlCommand("", dbConnection as SqlConnection);
						}
						dbConnection.Open();
						dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						string text3 = " DELETE FROM t_d_Privilege ";
						text3 += string.Format(" WHERE t_d_Privilege.f_ConsumerID IN ( SELECT f_ConsumerID FROM t_b_Consumer WHERE t_b_Consumer.f_PrivilegeTypeID = {0} ) ", num2.ToString());
						dbCommand.CommandText = text3;
						dbCommand.ExecuteNonQuery();
						text3 = " UPDATE t_b_Consumer SET f_PrivilegeTypeID = 0 ";
						text3 += string.Format("  WHERE f_PrivilegeTypeID = {0}  ", num2.ToString());
						dbCommand.CommandText = text3;
						dbCommand.ExecuteNonQuery();
						text3 = " DELETE FROM t_d_Privilege_Of_PrivilegeType ";
						if (this.dgvPrivilegeTypes.Rows.Count > 1)
						{
							text3 += string.Format(" WHERE f_ConsumerID = {0}  ", num2.ToString());
						}
						dbCommand.CommandText = text3;
						dbCommand.ExecuteNonQuery();
						text3 = " DELETE FROM t_d_PrivilegeType ";
						text3 = text3 + " WHERE f_PrivilegeTypeName = " + wgTools.PrepareStrNUnicode(this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
						dbCommand.CommandText = text3;
						dbCommand.ExecuteNonQuery();
						dbConnection.Close();
						this._loadData();
						if (!string.IsNullOrEmpty(text))
						{
							this.comboBox2.Text = text;
						}
						XMessageBox.Show(string.Format(" {0}  {1}.", this.btnDel.Text, CommonStr.strSuccessfully));
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
					Cursor.Current = Cursors.Default;
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00201BE4 File Offset: 0x00200BE4
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
				{
					if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
				}
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.Text = string.Format("{0}  {1} ", this.btnEdit.Text, this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
					dfrmInputNewName.strNewName = this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString();
					if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
					{
						string text = dfrmInputNewName.strNewName.Trim();
						if (!string.IsNullOrEmpty(text) && this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString() != text)
						{
							this.dvtemp.RowFilter = " f_PrivilegeTypeName= " + wgTools.PrepareStr(text);
							if (this.dvtemp.Count > 0)
							{
								XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							}
							else
							{
								wgAppConfig.runUpdateSql(" UPDATE t_d_PrivilegeType SET f_PrivilegeTypeName = " + wgTools.PrepareStrNUnicode(text.ToString()) + " WHERE f_PrivilegeTypeName = " + wgTools.PrepareStrNUnicode(this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString()));
								this._loadData();
								this.dvtemp.RowFilter = " f_PrivilegeTypeName= " + wgTools.PrepareStr(text);
								if (this.dvtemp.Count > 0)
								{
									this.comboBox2.Text = text;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00201E34 File Offset: 0x00200E34
		private void btnEditDoors_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
				{
					if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
				}
				int num2 = 0;
				this.dvtemp.RowFilter = "  f_PrivilegeTypeName = " + wgTools.PrepareStr(this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
				if (this.dvtemp.Count > 0)
				{
					num2 = (int)this.dvtemp[0]["f_PrivilegeTypeID"];
				}
				if (num2 > 0)
				{
					using (dfrmPrivilegeTypeDoors dfrmPrivilegeTypeDoors = new dfrmPrivilegeTypeDoors())
					{
						dfrmPrivilegeTypeDoors.PrivilegeTypeID = num2;
						dfrmPrivilegeTypeDoors.Text = dfrmPrivilegeTypeDoors.Text + " -- " + this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString();
						dfrmPrivilegeTypeDoors.ShowDialog(this);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00201FB0 File Offset: 0x00200FB0
		private void btnEditUsers_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
				{
					if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
				}
				int num2 = 0;
				this.dvtemp.RowFilter = "  f_PrivilegeTypeName = " + wgTools.PrepareStr(this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
				if (this.dvtemp.Count > 0)
				{
					num2 = (int)this.dvtemp[0]["f_PrivilegeTypeID"];
				}
				if (num2 > 0)
				{
					using (dfrmPrivilegeTypeUser dfrmPrivilegeTypeUser = new dfrmPrivilegeTypeUser())
					{
						dfrmPrivilegeTypeUser.PrivilegeTypeID = num2;
						dfrmPrivilegeTypeUser.Text = dfrmPrivilegeTypeUser.Text + " -- " + this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString();
						dfrmPrivilegeTypeUser.lblSelectedUsers.Text = string.Format("{0}  -- {1}", dfrmPrivilegeTypeUser.lblSelectedUsers.Text, this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
						dfrmPrivilegeTypeUser.ShowDialog(this);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00202174 File Offset: 0x00201174
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0020217C File Offset: 0x0020117C
		private void btnQueryDoors_Click(object sender, EventArgs e)
		{
			using (dfrmPrivilegeTypeDoorsType dfrmPrivilegeTypeDoorsType = new dfrmPrivilegeTypeDoorsType())
			{
				dfrmPrivilegeTypeDoorsType.ShowDialog();
			}
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x002021B4 File Offset: 0x002011B4
		private void btnSetNone_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.selectID == 0)
			{
				base.DialogResult = DialogResult.Cancel;
				base.Close();
				return;
			}
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbConnection.Open();
				if (this.selectID >= 0)
				{
					dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					string text = " DELETE FROM t_d_Privilege ";
					text += string.Format(" WHERE t_d_Privilege.f_ConsumerID  = {0}  ", this.selectConsumerID.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					text = " UPDATE t_b_Consumer SET f_PrivilegeTypeID = 0 ";
					text += string.Format("  WHERE f_ConsumerID = {0}  ", this.selectConsumerID.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgAppConfig.wgLog(string.Format("{0}---ConsumerID = {1}", this.btnSetNone.Text, this.selectConsumerID.ToString()));
				}
				else
				{
					dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					string text = " DELETE FROM t_d_Privilege ";
					text += string.Format(" WHERE t_d_Privilege.f_ConsumerID  IN  ( {0} ) ", this.strSqlSelected.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					text = string.Format(" UPDATE t_b_Consumer SET f_PrivilegeTypeID = {0} ", 0.ToString()) + string.Format("  WHERE f_ConsumerID IN ({0} )  ", this.strSqlSelected.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgAppConfig.wgLog(string.Format("{0}---ConsumerID IN ({1})", this.btnSetNone.Text, this.strSqlSelected));
				}
				dbConnection.Close();
				this.selectID = 0;
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x002023B4 File Offset: 0x002013B4
		private void btnSetSelected_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			int num = 0;
			this.dvtemp.RowFilter = "  f_PrivilegeTypeName = " + wgTools.PrepareStr(this.comboBox2.Text);
			if (this.dvtemp.Count > 0)
			{
				num = (int)this.dvtemp[0]["f_PrivilegeTypeID"];
			}
			if (num == 0)
			{
				XMessageBox.Show(CommonStr.strSelectPrivilegeType);
				return;
			}
			if (num == this.selectID)
			{
				base.DialogResult = DialogResult.Cancel;
				base.Close();
				return;
			}
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbConnection.Open();
				if (this.selectID >= 0)
				{
					dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					string text = " DELETE FROM t_d_Privilege ";
					text += string.Format(" WHERE t_d_Privilege.f_ConsumerID  = {0}  ", this.selectConsumerID.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					text = string.Format(" UPDATE t_b_Consumer SET f_PrivilegeTypeID = {0} ", num.ToString()) + string.Format("  WHERE f_ConsumerID = {0}  ", this.selectConsumerID.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					text = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
					text = string.Concat(new string[]
					{
						text,
						" SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege_Of_PrivilegeType.f_DoorID,t_d_Privilege_Of_PrivilegeType.[f_ControlSegID] , t_d_Privilege_Of_PrivilegeType.[f_ControllerID], t_d_Privilege_Of_PrivilegeType.[f_DoorNO]  FROM t_d_Privilege_Of_PrivilegeType, t_b_Consumer  WHERE [t_b_Consumer].[f_ConsumerID] = (",
						this.selectConsumerID.ToString(),
						" )  AND (t_d_Privilege_Of_PrivilegeType.f_ConsumerID)= ",
						num.ToString()
					});
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgAppConfig.wgLog(string.Format("{0}---ConsumerID = {1}", this.btnSetSelected.Text, this.selectConsumerID.ToString()));
				}
				else
				{
					dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					string text = " DELETE FROM t_d_Privilege ";
					text += string.Format(" WHERE t_d_Privilege.f_ConsumerID  IN  ( {0} ) ", this.strSqlSelected.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					text = string.Format(" UPDATE t_b_Consumer SET f_PrivilegeTypeID = {0} ", num.ToString()) + string.Format("  WHERE f_ConsumerID IN ({0} )  ", this.strSqlSelected.ToString());
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					text = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
					text = string.Concat(new string[]
					{
						text,
						" SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege_Of_PrivilegeType.f_DoorID,t_d_Privilege_Of_PrivilegeType.[f_ControlSegID] , t_d_Privilege_Of_PrivilegeType.[f_ControllerID], t_d_Privilege_Of_PrivilegeType.[f_DoorNO]  FROM t_d_Privilege_Of_PrivilegeType, t_b_Consumer  WHERE [t_b_Consumer].[f_ConsumerID] IN (",
						this.strSqlSelected.ToString(),
						" )  AND (t_d_Privilege_Of_PrivilegeType.f_ConsumerID)= ",
						num.ToString()
					});
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgAppConfig.wgLog(string.Format("{0}---ConsumerID IN ({1})", this.btnSetSelected.Text, this.strSqlSelected));
				}
				dbConnection.Close();
				this.selectID = num;
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x002026C8 File Offset: 0x002016C8
		private void comboBox1_KeyUp(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x002026CA File Offset: 0x002016CA
		private void comboBox2_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.operateMode == "SELECT" && e.KeyValue == 13)
			{
				this.btnSetSelected.PerformClick();
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x002026F4 File Offset: 0x002016F4
		private void dfrmPrivilegeType_Load(object sender, EventArgs e)
		{
			if (this.operateMode.Equals("MANAGE"))
			{
				this.btnSetNone.Visible = false;
				this.btnSetSelected.Visible = false;
				this.btnCancel.Visible = false;
			}
			else
			{
				this.btnAdd.Visible = false;
				this.btnEdit.Visible = false;
				this.btnEditDoors.Visible = false;
				this.btnEditUsers.Visible = false;
				this.btnDel.Visible = false;
				this.btnExit.Visible = false;
				this.btnQueryDoors.Visible = false;
			}
			this._loadData();
			if (!this.operateMode.Equals("MANAGE"))
			{
				if (this.selectID == 0)
				{
					this.btnSetNone.Enabled = false;
					this.comboBox2.Text = "";
					this.dgvPrivilegeTypes.ClearSelection();
				}
				else
				{
					this.dvtemp.RowFilter = "  f_PrivilegeTypeID = " + this.selectID.ToString();
					if (this.dvtemp.Count > 0)
					{
						this.comboBox2.Text = (string)this.dvtemp[0]["f_PrivilegeTypeName"];
					}
				}
			}
			this.comboBox2.Focus();
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0020283C File Offset: 0x0020183C
		private void dgvPrivilegeTypes_SelectionChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.comboBox2.Text))
			{
				try
				{
					if (this.dgvPrivilegeTypes.Rows.Count > 0)
					{
						int num;
						if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
						{
							if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
							{
								return;
							}
							num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
						}
						else
						{
							num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
						}
						this.comboBox2.Text = wgTools.SetObjToStr(this.dgvPrivilegeTypes.Rows[num].Cells[1].Value);
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
		}

		// Token: 0x0400325C RID: 12892
		private DataTable dt;

		// Token: 0x0400325D RID: 12893
		private DataView dvtemp;

		// Token: 0x0400325E RID: 12894
		public string operateMode = "MANAGE";

		// Token: 0x0400325F RID: 12895
		public int selectConsumerID;

		// Token: 0x04003260 RID: 12896
		public int selectID;

		// Token: 0x04003261 RID: 12897
		public string strSqlSelected = "";
	}
}
