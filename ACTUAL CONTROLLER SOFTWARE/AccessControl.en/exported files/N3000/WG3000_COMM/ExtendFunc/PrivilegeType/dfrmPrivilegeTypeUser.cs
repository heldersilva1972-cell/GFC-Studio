using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031D RID: 797
	public partial class dfrmPrivilegeTypeUser : frmN3000
	{
		// Token: 0x060018A3 RID: 6307 RVA: 0x00203338 File Offset: 0x00202338
		public dfrmPrivilegeTypeUser()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x002033E4 File Offset: 0x002023E4
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.loadUserData4BackWork();
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00203428 File Offset: 0x00202428
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
				return;
			}
			if (e.Error != null)
			{
				wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
				return;
			}
			this.loadUserData4BackWorkComplete(e.Result as DataTable);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x002034A8 File Offset: 0x002024A8
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			if (this.strGroupFilter == "")
			{
				string rowFilter = this.dv1.RowFilter;
				string rowFilter2 = this.dv2.RowFilter;
				this.dv1.Dispose();
				this.dv2.Dispose();
				this.dv1 = null;
				this.dv2 = null;
				this.dt.BeginLoadData();
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 1;
				}
				this.dt.EndLoadData();
				this.dv1 = new DataView(this.dt);
				this.dv1.RowFilter = rowFilter;
				this.dv2 = new DataView(this.dt);
				this.dv2.RowFilter = rowFilter2;
			}
			else
			{
				this.dv = new DataView(this.dt);
				this.dv.RowFilter = this.strGroupFilter;
				for (int j = 0; j < this.dv.Count; j++)
				{
					this.dv[j]["f_Selected"] = 1;
				}
			}
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			wgTools.WriteLine("btnAddAllUsers_Click End");
			Cursor.Current = Cursors.Default;
			this.updateCount();
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x00203697 File Offset: 0x00202697
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers);
			this.updateCount();
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x002036AA File Offset: 0x002026AA
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x002036BC File Offset: 0x002026BC
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				this.dt = ((DataView)this.dgvUsers.DataSource).Table;
				this.dv1 = (DataView)this.dgvUsers.DataSource;
				this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
				this.dgvUsers.DataSource = null;
				this.dgvSelectedUsers.DataSource = null;
				string rowFilter = this.dv1.RowFilter;
				string rowFilter2 = this.dv2.RowFilter;
				this.dv1.Dispose();
				this.dv2.Dispose();
				this.dv1 = null;
				this.dv2 = null;
				this.dt.BeginLoadData();
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 0;
				}
				this.dt.EndLoadData();
				this.dv1 = new DataView(this.dt);
				this.dv1.RowFilter = rowFilter;
				this.dv2 = new DataView(this.dt);
				this.dv2.RowFilter = rowFilter2;
				this.dgvUsers.DataSource = this.dv1;
				this.dgvSelectedUsers.DataSource = this.dv2;
				wgTools.WriteLine("btnDelAllUsers_Click End");
				Cursor.Current = Cursors.Default;
				this.updateCount();
			}
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00203856 File Offset: 0x00202856
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers);
			this.updateCount();
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0020386C File Offset: 0x0020286C
		private void btnOK_Click(object sender, EventArgs e)
		{
			string text = "";
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSureAgain + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				Cursor cursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
				this.updatePrivilege();
			}
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x002038B8 File Offset: 0x002028B8
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x002038C8 File Offset: 0x002028C8
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = "f_Selected = 0";
					this.strGroupFilter = "";
				}
				else
				{
					dataView.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
					if (num2 > 0)
					{
						if (num2 >= groupChildMaxNo)
						{
							dataView.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num);
							this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
						}
						else
						{
							dataView.RowFilter = "f_Selected = 0 ";
							string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
							dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
							this.strGroupFilter = string.Format("  {0} ", groupQuery);
						}
					}
				}
				this.updateCount();
			}
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00203A82 File Offset: 0x00202A82
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00203A98 File Offset: 0x00202A98
		private void dfrm_KeyDown(object sender, KeyEventArgs e)
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
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x00203B08 File Offset: 0x00202B08
		private void dfrmFirstCard_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			this.loadPrivilegeType();
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmFirstCard_Load_Acc(sender, e);
				return;
			}
			this.loadGroupData();
			this.updateCount();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00203C1C File Offset: 0x00202C1C
		private void dfrmFirstCard_Load_Acc(object sender, EventArgs e)
		{
			this.loadGroupData();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00203CB8 File Offset: 0x00202CB8
		private void dfrmPrivilegeTypeUser_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00203CF0 File Offset: 0x00202CF0
		private void loadGroupData()
		{
			new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
			for (int i = 0; i < this.arrGroupID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
				{
					this.cbof_GroupID.Items.Add(CommonStr.strAll);
				}
				else
				{
					this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
				}
			}
			if (this.cbof_GroupID.Items.Count > 0)
			{
				this.cbof_GroupID.SelectedIndex = 0;
			}
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00203DA4 File Offset: 0x00202DA4
		private void loadPrivilegeType()
		{
			this.arrPrivilegeTypeName.Clear();
			this.arrPrivilegeTypeID.Clear();
			try
			{
				string text = "SELECT * FROM t_d_PrivilegeType ORDER BY f_PrivilegeTypeID ";
				DataTable dataTable = new DataTable("t_d_PrivilegeType");
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbDataAdapter.Fill(dataTable);
							}
						}
						goto IL_00CA;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(dataTable);
						}
					}
				}
				IL_00CA:
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (object obj in dataTable.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						this.arrPrivilegeTypeID.Add(dataRow["f_PrivilegeTypeID"]);
						this.arrPrivilegeTypeName.Add(dataRow["f_PrivilegeTypeName"]);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			int count = this.arrPrivilegeTypeID.Count;
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x00203FE4 File Offset: 0x00202FE4
		private DataTable loadUserData4BackWork()
		{
			Thread.Sleep(100);
			wgTools.WriteLine("loadUserData Start");
			this.dtUser1 = new DataTable();
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT  t_b_Consumer.f_ConsumerID ";
				text = string.Concat(new string[]
				{
					text,
					" , f_ConsumerNO, f_ConsumerName, f_CardNO ",
					string.Format(" , IIF ( f_PrivilegeTypeID = {0} , 1 , 0 ) AS f_Selected ", this.PrivilegeTypeID),
					" , f_GroupID ",
					this.strPrivilegeType,
					" FROM t_b_Consumer  ORDER BY f_ConsumerNO ASC "
				});
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUser1);
						}
					}
					goto IL_017D;
				}
			}
			text = " SELECT  t_b_Consumer.f_ConsumerID ";
			text = string.Concat(new string[]
			{
				text,
				" , f_ConsumerNO, f_ConsumerName, f_CardNO ",
				string.Format(" ,  CASE WHEN  f_PrivilegeTypeID = {0}  THEN 1 ELSE 0 END AS f_Selected ", this.PrivilegeTypeID),
				" , f_GroupID ",
				this.strPrivilegeType,
				" FROM t_b_Consumer  ORDER BY f_ConsumerNO ASC "
			});
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
			IL_017D:
			wgTools.WriteLine("da.Fill End");
			this.updatePrivilegeTypeName(ref this.dtUser1);
			try
			{
				this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			dfrmPrivilegeTypeUser.lastLoadUsers = icConsumerShare.getUpdateLog();
			dfrmPrivilegeTypeUser.dtLastLoad = this.dtUser1;
			return this.dtUser1;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x00204228 File Offset: 0x00203228
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dvSelected.Sort = " f_ConsumerNo ASC ";
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
				this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x00204338 File Offset: 0x00203338
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.dgvUsers.DataSource == null)
			{
				Cursor.Current = Cursors.WaitCursor;
				return;
			}
			this.timer1.Enabled = false;
			Cursor.Current = Cursors.Default;
			this.lblWait.Visible = false;
			this.grpUsers.Enabled = true;
			this.btnOK.Enabled = true;
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00204398 File Offset: 0x00203398
		private void updateCount()
		{
			this.lblOptional.Text = this.dgvUsers.RowCount.ToString();
			this.lblSeleted.Text = this.dgvSelectedUsers.RowCount.ToString();
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x002043E4 File Offset: 0x002033E4
		private void updatePrivilege()
		{
			string rowFilter = this.dv.RowFilter;
			this.dv.RowFilter = "f_Selected = 0";
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
			bool flag = false;
			this.timer1.Enabled = true;
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			try
			{
				DataView dataView = (DataView)this.dgvSelectedUsers.DataSource;
				if (dbConnection.State != ConnectionState.Open)
				{
					dbConnection.Open();
				}
				try
				{
					int num = 2000;
					int i = 0;
					this.progressBar1.Maximum = 2 * this.dgvUsers.RowCount;
					string text2;
					while (i < this.dgvUsers.Rows.Count)
					{
						string text = "";
						while (i < this.dgvUsers.Rows.Count)
						{
							if ((int)((DataView)this.dgvUsers.DataSource)[i]["f_PrivilegeTypeID"] == this.PrivilegeTypeID)
							{
								text = text + ((DataView)this.dgvUsers.DataSource)[i]["f_ConsumerID"] + ",";
							}
							i++;
							if (text.Length > num)
							{
								break;
							}
						}
						text += "0";
						text2 = "DELETE FROM  [t_d_Privilege] ";
						text2 = text2 + " WHERE [f_ConsumerID] IN (" + text + " ) ";
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						text2 = string.Format("  UPDATE t_b_Consumer SET f_PrivilegeTypeID = 0 WHERE  [f_ConsumerID] IN ( {0} )", text);
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						int count = this.dgvSelectedUsers.Rows.Count;
						this.progressBar1.Value = i;
						Application.DoEvents();
					}
					num = 2000;
					i = 0;
					this.progressBar1.Maximum = 2 * this.dgvSelectedUsers.RowCount;
					while (i < this.dgvSelectedUsers.Rows.Count)
					{
						string text = "";
						while (i < this.dgvSelectedUsers.Rows.Count)
						{
							if ((int)((DataView)this.dgvSelectedUsers.DataSource)[i]["f_PrivilegeTypeID"] != this.PrivilegeTypeID)
							{
								text = text + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerID"] + ",";
							}
							i++;
							if (text.Length > num)
							{
								break;
							}
						}
						text += "0";
						text2 = "DELETE FROM  [t_d_Privilege] ";
						text2 = text2 + " WHERE [f_ConsumerID] IN (" + text + " ) ";
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						int count2 = this.dgvSelectedUsers.Rows.Count;
						this.progressBar1.Value = i;
						Application.DoEvents();
					}
					num = 2000;
					i = 0;
					this.progressBar1.Maximum = 2 * this.dgvSelectedUsers.RowCount;
					while (i < this.dgvSelectedUsers.Rows.Count)
					{
						string text = "";
						while (i < this.dgvSelectedUsers.Rows.Count)
						{
							if ((int)((DataView)this.dgvSelectedUsers.DataSource)[i]["f_PrivilegeTypeID"] != this.PrivilegeTypeID)
							{
								text = text + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerID"] + ",";
							}
							i++;
							if (text.Length > num)
							{
								break;
							}
						}
						text += "0";
						text2 = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
						text2 = string.Concat(new object[] { text2, " SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege_Of_PrivilegeType.f_DoorID,t_d_Privilege_Of_PrivilegeType.[f_ControlSegID] , t_d_Privilege_Of_PrivilegeType.[f_ControllerID], t_d_Privilege_Of_PrivilegeType.[f_DoorNO]  FROM t_d_Privilege_Of_PrivilegeType, t_b_Consumer  WHERE [t_b_Consumer].[f_ConsumerID] IN (", text, " )  AND (t_d_Privilege_Of_PrivilegeType.f_ConsumerID)= ", this.PrivilegeTypeID });
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						text2 = string.Format("  UPDATE t_b_Consumer SET f_PrivilegeTypeID = {0} WHERE f_ConsumerID IN ({1})", this.PrivilegeTypeID, text);
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						int count3 = this.dgvSelectedUsers.Rows.Count;
						this.progressBar1.Value = i;
						Application.DoEvents();
					}
					flag = true;
					string text3 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} ";
					text2 = string.Format(text3, wgTools.PrepareStrNUnicode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount);
					dbCommand.CommandText = text2;
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
				XMessageBox.Show(ex2.Message);
			}
			finally
			{
				if (dbCommand != null)
				{
					dbCommand.Dispose();
				}
				if (dbConnection.State != ConnectionState.Closed)
				{
					dbConnection.Close();
				}
				this.dfrmWait1.Hide();
			}
			this.progressBar1.Value = this.progressBar1.Maximum;
			Cursor.Current = Cursors.Default;
			this.dv.RowFilter = rowFilter;
			if (flag)
			{
				wgAppConfig.wgLog(this.Text + " " + CommonStr.strSuccessfully);
				XMessageBox.Show(this.Text + " " + CommonStr.strSuccessfully);
				base.DialogResult = DialogResult.OK;
				base.Close();
				return;
			}
			this.progressBar1.Value = 0;
			wgAppConfig.wgLog(this.Text + CommonStr.strOperateFailed);
			XMessageBox.Show(this.Text + CommonStr.strOperateFailed);
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00204A2C File Offset: 0x00203A2C
		private void updatePrivilegeTypeName(ref DataTable dtUser)
		{
			if (dtUser != null && dtUser.Rows.Count > 0 && this.arrPrivilegeTypeID.Count > 0)
			{
				foreach (object obj in dtUser.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					if ((int)dataRow["f_PrivilegeTypeID"] > 0)
					{
						int num = this.arrPrivilegeTypeID.IndexOf((int)dataRow["f_PrivilegeTypeID"]);
						if (num >= 0)
						{
							dataRow["f_PrivilegeTypeName"] = this.arrPrivilegeTypeName[num];
						}
					}
				}
			}
		}

		// Token: 0x04003273 RID: 12915
		private const int MoreCardGroupMaxLen = 9;

		// Token: 0x04003274 RID: 12916
		private DataTable dt;

		// Token: 0x04003275 RID: 12917
		private DataTable dtUser1;

		// Token: 0x04003276 RID: 12918
		private DataView dv;

		// Token: 0x04003277 RID: 12919
		private DataView dv1;

		// Token: 0x04003278 RID: 12920
		private DataView dv2;

		// Token: 0x04003279 RID: 12921
		private DataView dvSelected;

		// Token: 0x0400327A RID: 12922
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x0400327B RID: 12923
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x0400327C RID: 12924
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x0400327D RID: 12925
		private ArrayList arrPrivilegeTypeID = new ArrayList();

		// Token: 0x0400327E RID: 12926
		private ArrayList arrPrivilegeTypeName = new ArrayList();

		// Token: 0x0400327F RID: 12927
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x04003280 RID: 12928
		private static DataTable dtLastLoad;

		// Token: 0x04003281 RID: 12929
		private static string lastLoadUsers = "";

		// Token: 0x04003282 RID: 12930
		public int PrivilegeTypeID;

		// Token: 0x04003283 RID: 12931
		public string retValue = "0";

		// Token: 0x04003284 RID: 12932
		private string strGroupFilter = "";

		// Token: 0x04003285 RID: 12933
		private string strPrivilegeType = ", '' AS f_PrivilegeTypeName, f_PrivilegeTypeID ";
	}
}
