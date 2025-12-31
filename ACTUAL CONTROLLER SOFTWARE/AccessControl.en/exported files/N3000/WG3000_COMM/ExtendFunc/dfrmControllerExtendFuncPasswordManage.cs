using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
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

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000236 RID: 566
	public partial class dfrmControllerExtendFuncPasswordManage : frmN3000
	{
		// Token: 0x060010F9 RID: 4345 RVA: 0x0013713C File Offset: 0x0013613C
		public dfrmControllerExtendFuncPasswordManage()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			wgAppConfig.custDataGridview(ref this.dataGridView3);
			wgAppConfig.custDataGridview(ref this.dataGridView4);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x001371DC File Offset: 0x001361DC
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			int num = (int)((object[])e.Argument)[0];
			int num2 = (int)((object[])e.Argument)[1];
			string text = (string)((object[])e.Argument)[2];
			e.Result = this.loadUserData(num, num2, text);
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0013725C File Offset: 0x0013625C
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
			if ((e.Result as DataView).Count < this.MaxRecord)
			{
				this.bLoadedFinished = true;
			}
			this.fillDgv(e.Result as DataView);
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0013731C File Offset: 0x0013631C
		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (this.txtPasswordNew.Text.Trim() == "")
			{
				this.txtPasswordNew.Text = "";
				return;
			}
			long num;
			if (!long.TryParse(this.txtPasswordNew.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (num <= 0L)
			{
				XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.cboReader.Items.Count > 0)
			{
				long num2 = num;
				string text = "  Insert INTO t_b_ReaderPassword (f_Password, f_BAll, f_ReaderID) ";
				text = text + " Values( " + num2;
				if (this.cboReader.Items[0].ToString() == CommonStr.strAll)
				{
					if (this.cboReader.SelectedIndex == 0)
					{
						text = string.Concat(new object[] { text, " , ", 1, " , ", 0 });
					}
					else
					{
						text = string.Concat(new object[]
						{
							text,
							" , ",
							0,
							" , ",
							this.arrReaderID[this.cboReader.SelectedIndex - 1]
						});
					}
				}
				else
				{
					text = string.Concat(new object[]
					{
						text,
						" , ",
						0,
						" , ",
						this.arrReaderID[this.cboReader.SelectedIndex]
					});
				}
				if (wgAppConfig.runUpdateSql(text + ")") > 0)
				{
					this.fillReaderPasswordGrid();
				}
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x001374E7 File Offset: 0x001364E7
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x001374F0 File Offset: 0x001364F0
		private void btnChangePassword_Click(object sender, EventArgs e)
		{
			if (this.dgvUsers.SelectedRows.Count <= 0)
			{
				if (this.dgvUsers.SelectedCells.Count <= 0)
				{
					return;
				}
				int rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
			}
			else
			{
				int index = this.dgvUsers.SelectedRows[0].Index;
			}
			int num = 0;
			DataGridView dataGridView = this.dgvUsers;
			DataGridViewColumn sortedColumn = dataGridView.SortedColumn;
			if (dataGridView.Rows.Count > 0)
			{
				try
				{
					num = dataGridView.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			using (dfrmSetPassword dfrmSetPassword = new dfrmSetPassword())
			{
				dfrmSetPassword.operatorID = 0;
				dfrmSetPassword.Text = string.Concat(new object[]
				{
					this.btnChangePassword.Text,
					"  [",
					this.dgvUsers.Rows[num].Cells[2].Value,
					"] "
				});
				if (dfrmSetPassword.ShowDialog(this) == DialogResult.OK)
				{
					string text = "Update t_b_Consumer ";
					string text2;
					if (wgTools.SetObjToStr(dfrmSetPassword.newPassword) == "")
					{
						text = text + "SET [f_PIN]=" + wgTools.PrepareStrNUnicode(0);
						text2 = "0";
					}
					else
					{
						text = text + "SET [f_PIN]=" + wgTools.PrepareStrNUnicode(dfrmSetPassword.newPassword);
						text2 = wgTools.SetObjToStr(dfrmSetPassword.newPassword);
					}
					if (wgAppConfig.runUpdateSql(text + "  WHERE  [f_ConsumerID]=" + this.dgvUsers.Rows[num].Cells[0].Value) == 1)
					{
						if (text2 == "0")
						{
							this.dgvUsers.Rows[num].Cells["strPwd"].Value = CommonStr.strPwdNoPassword;
						}
						else if (text2 == 345678.ToString())
						{
							this.dgvUsers.Rows[num].Cells["strPwd"].Value = CommonStr.strPwdUnChanged;
						}
						else
						{
							this.dgvUsers.Rows[num].Cells["strPwd"].Value = CommonStr.strPwdChanged;
						}
					}
				}
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00137780 File Offset: 0x00136780
		private void btnDel_Click(object sender, EventArgs e)
		{
			if (this.dataGridView3.SelectedRows.Count <= 0)
			{
				if (this.dataGridView3.SelectedCells.Count <= 0)
				{
					return;
				}
				int rowIndex = this.dataGridView3.SelectedCells[0].RowIndex;
			}
			else
			{
				int index = this.dataGridView3.SelectedRows[0].Index;
			}
			int num = 0;
			DataGridView dataGridView = this.dataGridView3;
			if (dataGridView.Rows.Count > 0)
			{
				try
				{
					num = dataGridView.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			if (wgAppConfig.runUpdateSql("DELETE FROM t_b_ReaderPassword  WHERE f_Id =" + ((int)dataGridView.Rows[num].Cells[0].Value).ToString()) > 0)
			{
				this.fillReaderPasswordGrid();
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00137860 File Offset: 0x00136860
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			this.updatePasswordKeypadEnableGrid();
			this.updateManualPasswordKeypadEnableGrid();
			XMessageBox.Show(CommonStr.strNeedUpload4Edit, wgTools.MSGTITLE, MessageBoxButtons.OK);
			base.Close();
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0013789B File Offset: 0x0013689B
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x001378A8 File Offset: 0x001368A8
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					this.strGroupFilter = "";
				}
				else
				{
					this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
					if (num2 > 0)
					{
						if (num2 >= groupChildMaxNo)
						{
							this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
						}
						else
						{
							string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
							this.strGroupFilter = string.Format("  {0} ", groupQuery);
						}
					}
				}
				((DataView)this.dgvUsers.DataSource).RowFilter = this.strGroupFilter;
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00137A14 File Offset: 0x00136A14
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBox1.Checked)
			{
				this.txtPasswordNew.UseSystemPasswordChar = false;
				this.dataGridView3.Columns[1].Visible = true;
				this.dataGridView3.Columns[2].Visible = false;
				return;
			}
			this.txtPasswordNew.UseSystemPasswordChar = true;
			this.dataGridView3.Columns[2].Visible = true;
			this.dataGridView3.Columns[1].Visible = false;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00137AA4 File Offset: 0x00136AA4
		private void dfrmControllerExtendFuncPasswordManage_FormClosed(object sender, FormClosedEventArgs e)
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

		// Token: 0x06001105 RID: 4357 RVA: 0x00137ADC File Offset: 0x00136ADC
		private void dfrmControllerExtendFuncPasswordManage_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00137AF4 File Offset: 0x00136AF4
		private void dfrmControllerExtendFuncPasswordManage_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x06001107 RID: 4359 RVA: 0x00137B68 File Offset: 0x00136B68
		private void dfrmControllerExtendFuncPasswordManage_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			this.fillPasswordKeypadEnableGrid();
			this.fillUsersPasswordGrid();
			this.fillReaderPasswordGrid();
			this.fillManualPasswordKeypadEnableGrid();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dataGridView3.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dataGridView4.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.tabPage1.BackColor = this.BackColor;
			this.tabPage2.BackColor = this.BackColor;
			this.tabPage3.BackColor = this.BackColor;
			this.tabPage4.BackColor = this.BackColor;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.Deptname.HeaderText = wgAppConfig.ReplaceFloorRoom(this.Deptname.HeaderText);
			this.ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			this.dfrmWait1.Hide();
			try
			{
				base.Owner.Show();
				base.Owner.Activate();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00137CF8 File Offset: 0x00136CF8
		private void dgvUsers_DoubleClick(object sender, EventArgs e)
		{
			this.btnChangePassword.PerformClick();
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00137D08 File Offset: 0x00136D08
		private void dgvUsers_Scroll(object sender, ScrollEventArgs e)
		{
			if (!this.bLoadedFinished && e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				wgTools.WriteLine(e.OldValue.ToString());
				wgTools.WriteLine(e.NewValue.ToString());
				DataGridView dataGridView = this.dgvUsers;
				if (e.NewValue > e.OldValue && (e.NewValue + 100 > dataGridView.Rows.Count || e.NewValue + dataGridView.Rows.Count / 10 > dataGridView.Rows.Count))
				{
					if (this.startRecordIndex <= dataGridView.Rows.Count)
					{
						if (!this.backgroundWorker1.IsBusy)
						{
							this.startRecordIndex += this.MaxRecord;
							this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
							return;
						}
					}
					else
					{
						wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + "#");
					}
				}
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00137E3C File Offset: 0x00136E3C
		private void fillDgv(DataView dv)
		{
			try
			{
				DataGridView dataGridView = this.dgvUsers;
				if (dataGridView.DataSource == null)
				{
					this.dgvUsers.AutoGenerateColumns = false;
					dataGridView.DataSource = dv;
					int num = 0;
					while (num < dv.Table.Columns.Count && num < dataGridView.ColumnCount)
					{
						dataGridView.Columns[num].DataPropertyName = dv.Table.Columns[num].ColumnName;
						num++;
					}
					wgAppConfig.ReadGVStyle(this, dataGridView);
					dataGridView.Columns[0].Visible = false;
					if (this.startRecordIndex == 0 && dv.Count >= this.MaxRecord)
					{
						this.startRecordIndex += this.MaxRecord;
						this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
					}
				}
				else if (dv.Count > 0)
				{
					int firstDisplayedScrollingRowIndex = dataGridView.FirstDisplayedScrollingRowIndex;
					DataView dataView = dataGridView.DataSource as DataView;
					dataView.Table.Merge(dv.Table);
					if (firstDisplayedScrollingRowIndex >= 0)
					{
						dataGridView.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					}
					dataGridView.Columns[0].Visible = false;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00137FC8 File Offset: 0x00136FC8
		private void fillManualPasswordKeypadEnableGrid()
		{
			string text = " SELECT ";
			text += " t_b_Reader.f_ReaderID , t_b_Controller.f_ControllerSN , t_b_Reader.f_ReaderNO , t_b_Reader.f_ReaderName , t_b_Reader.f_InputCardno_Enabled , t_b_Controller.f_ZoneID FROM t_b_Reader INNER JOIN t_b_Controller ON t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID  ORDER BY [f_ReaderID] ";
			this.dtReader = new DataTable();
			this.dvReader = new DataView(this.dtReader);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtReader);
						}
					}
					goto IL_00DB;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtReader);
					}
				}
			}
			IL_00DB:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dtReader);
			this.dataGridView4.AutoGenerateColumns = false;
			this.dataGridView4.DataSource = this.dvReader;
			this.removeUsedReaderOfElevator(ref this.dtReader);
			int num = 0;
			while (num < this.dvReader.Table.Columns.Count && num < this.dataGridView4.ColumnCount)
			{
				this.dataGridView4.Columns[num].DataPropertyName = this.dvReader.Table.Columns[num].ColumnName;
				num++;
			}
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x001381A0 File Offset: 0x001371A0
		private void fillPasswordKeypadEnableGrid()
		{
			string text = " SELECT ";
			text += " t_b_Reader.f_ReaderID , t_b_Controller.f_ControllerSN , t_b_Reader.f_ReaderNO , t_b_Reader.f_ReaderName , t_b_Reader.f_PasswordEnabled , t_b_Controller.f_ZoneID  FROM t_b_Reader INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  ORDER BY [f_ReaderID] ";
			this.dtPasswordKeypad = new DataTable();
			this.dvPasswordKeypad = new DataView(this.dtPasswordKeypad);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtPasswordKeypad);
						}
					}
					goto IL_00DB;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtPasswordKeypad);
					}
				}
			}
			IL_00DB:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dtPasswordKeypad);
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.DataSource = this.dvPasswordKeypad;
			this.removeUsedReaderOfElevator(ref this.dtPasswordKeypad);
			int num = 0;
			while (num < this.dvPasswordKeypad.Table.Columns.Count && num < this.dataGridView1.ColumnCount)
			{
				this.dataGridView1.Columns[num].DataPropertyName = this.dvPasswordKeypad.Table.Columns[num].ColumnName;
				num++;
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00138378 File Offset: 0x00137378
		private void fillReaderPasswordGrid()
		{
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = string.Format(" SELECT  t_b_ReaderPassword.f_Id , t_b_ReaderPassword.f_Password , '******' AS f_PasswordStar,  IIF ( f_BALL=1 , {0}, tt.f_ReaderName ) AS f_AdaptTo , tt.f_ZoneID  ", wgTools.PrepareStrNUnicode(CommonStr.strAll)) + string.Format(" FROM    (SELECT t_b_Reader.f_ReaderID,t_b_Reader.f_ReaderName,  t_b_Controller.f_ZoneID  from    t_b_Controller INNER JOIN t_b_Reader ON  t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID  ) As tt  Right JOIN t_b_ReaderPassword ON ( tt.f_ReaderID = t_b_ReaderPassword.f_ReaderID) ", new object[0]) + " ORDER BY [f_Id] ";
			}
			else
			{
				text = " SELECT ";
				text = text + " t_b_ReaderPassword.f_Id , t_b_ReaderPassword.f_Password ,  '******' AS f_PasswordStar  , CASE WHEN f_BALL=1 THEN " + wgTools.PrepareStrNUnicode(CommonStr.strAll) + " ELSE t_b_Reader.f_ReaderName  END AS f_AdaptTo , c.f_ZoneID  FROM t_b_ReaderPassword LEFT JOIN (t_b_Reader INNER JOIN t_b_Controller c ON c.f_ControllerID = t_b_Reader.f_ControllerID) ON t_b_Reader.f_ReaderID = t_b_ReaderPassword.f_ReaderID  ORDER BY [f_Id] ";
			}
			this.ds = new DataSet();
			this.dtReaderPassword = new DataTable();
			this.dvReaderPassword = new DataView(this.dtReaderPassword);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtReaderPassword);
						}
					}
					goto IL_0131;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtReaderPassword);
					}
				}
			}
			IL_0131:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dtReaderPassword);
			this.dataGridView3.AutoGenerateColumns = false;
			this.dataGridView3.DataSource = this.dvReaderPassword;
			int i = 0;
			while (i < this.dvReaderPassword.Table.Columns.Count && i < this.dataGridView3.ColumnCount)
			{
				this.dataGridView3.Columns[i].DataPropertyName = this.dvReaderPassword.Table.Columns[i].ColumnName;
				i++;
			}
			if (this.cboReader.Items.Count != 0)
			{
				return;
			}
			this.cboReader.Items.Clear();
			this.arrReaderID.Clear();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand("Select t_b_reader.*,t_b_Controller.f_ZoneID,t_b_Controller.f_ControllerSN from t_b_reader inner join t_b_Controller on t_b_controller.f_controllerID = t_b_reader.f_ControllerID ", oleDbConnection2))
					{
						using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
						{
							oleDbDataAdapter2.Fill(this.ds, "reader");
						}
					}
					goto IL_02B5;
				}
			}
			using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand2 = new SqlCommand("Select t_b_reader.*,t_b_Controller.f_ZoneID,t_b_Controller.f_ControllerSN from t_b_reader inner join t_b_Controller on t_b_controller.f_controllerID = t_b_reader.f_ControllerID ", sqlConnection2))
				{
					using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
					{
						sqlDataAdapter2.Fill(this.ds, "reader");
					}
				}
			}
			IL_02B5:
			this.dtReader = this.ds.Tables["reader"];
			int count = this.dtReader.Rows.Count;
			icControllerZone.getAllowedControllers(ref this.dtReader);
			if (count == this.dtReader.Rows.Count)
			{
				this.cboReader.Items.Add(CommonStr.strAll);
			}
			DataTable dataTable = this.ds.Tables["reader"];
			this.removeUsedReaderOfElevator(ref dataTable);
			if (this.ds.Tables["reader"].Rows.Count > 0)
			{
				for (i = 0; i < this.ds.Tables["reader"].Rows.Count; i++)
				{
					string text2 = wgTools.SetObjToStr(this.ds.Tables["reader"].Rows[i]["f_ReaderName"]);
					if (this.cboReader.FindStringExact(text2) < 0)
					{
						this.cboReader.Items.Add(text2);
						this.arrReaderID.Add(this.ds.Tables["reader"].Rows[i]["f_ReaderID"]);
					}
				}
			}
			if (this.cboReader.Items.Count > 0)
			{
				this.cboReader.SelectedIndex = 0;
			}
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00138850 File Offset: 0x00137850
		private void fillUsersPasswordGrid()
		{
			this.loadStyle();
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
			Cursor.Current = Cursors.WaitCursor;
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = string.Format(" SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO,  f_GroupName , IIF ( f_PIN=0, {0}, IIF (f_PIN = 345678, {1},{2}))  AS strPwd, t_b_Consumer.f_GroupID  ", wgTools.PrepareStrNUnicode(CommonStr.strPwdNoPassword), wgTools.PrepareStrNUnicode(CommonStr.strPwdUnChanged), wgTools.PrepareStrNUnicode(CommonStr.strPwdChanged)) + " FROM ( t_b_Consumer LEFT OUTER JOIN t_b_Group ON ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) ) ";
			}
			else
			{
				text = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO,  f_GroupName ";
				text = string.Concat(new string[]
				{
					text,
					" ,  CASE WHEN f_PIN=0 THEN ",
					wgTools.PrepareStrNUnicode(CommonStr.strPwdNoPassword),
					" ELSE   CASE WHEN f_PIN=345678 THEN ",
					wgTools.PrepareStrNUnicode(CommonStr.strPwdUnChanged),
					" ELSE  ",
					wgTools.PrepareStrNUnicode(CommonStr.strPwdChanged),
					" END    END  AS strPwd, t_b_Consumer.f_GroupID   FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID "
				});
			}
			this.reloadUserData(text);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x001389B8 File Offset: 0x001379B8
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuPasswordManagement";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnOK.Visible = false;
				this.btnChangePassword.Visible = false;
				this.btnAdd.Visible = false;
				this.btnDel.Visible = false;
				this.dataGridView1.ReadOnly = true;
				this.dataGridView3.ReadOnly = true;
				this.dataGridView4.ReadOnly = true;
				this.label1.Visible = false;
				this.label2.Visible = false;
				this.txtPasswordNew.Visible = false;
				this.cboReader.Visible = false;
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00138A64 File Offset: 0x00137A64
		private void loadStyle()
		{
			this.dgvUsers.AutoGenerateColumns = false;
			wgAppConfig.ReadGVStyle(this, this.dgvUsers);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00138A80 File Offset: 0x00137A80
		private DataView loadUserData(int startIndex, int maxRecords, string strSql)
		{
			wgTools.WriteLine("loadUserData Start");
			if (strSql.ToUpper().IndexOf("SELECT ") > 0)
			{
				strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
			}
			if (startIndex == 0)
			{
				this.recNOMax = "";
			}
			else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
			{
				strSql += string.Format(" AND f_ConsumerNO > {0}", wgTools.PrepareStrNUnicode(this.recNOMax));
			}
			else
			{
				strSql += string.Format(" WHERE f_ConsumerNO > {0}", wgTools.PrepareStrNUnicode(this.recNOMax));
			}
			strSql += " ORDER BY f_ConsumerNO ";
			this.dtUserData = new DataTable("users");
			this.dvUserData = new DataView(this.dtUserData);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUserData);
						}
					}
					goto IL_0187;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtUserData);
					}
				}
			}
			IL_0187:
			if (this.dtUserData.Rows.Count > 0)
			{
				this.recNOMax = this.dtUserData.Rows[this.dtUserData.Rows.Count - 1]["f_ConsumerNO"].ToString();
			}
			wgTools.WriteLine("loadUserData End");
			return this.dvUserData;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00138CBC File Offset: 0x00137CBC
		private void reloadUserData(string strsql)
		{
			if (!this.backgroundWorker1.IsBusy)
			{
				this.bLoadedFinished = false;
				this.startRecordIndex = 0;
				this.MaxRecord = 1000;
				if (!string.IsNullOrEmpty(strsql))
				{
					this.dgvSql = strsql;
				}
				this.dgvUsers.DataSource = null;
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00138D44 File Offset: 0x00137D44
		private void removeUsedReaderOfElevator(ref DataTable dt)
		{
			bool flag = true;
			while (flag)
			{
				flag = false;
				foreach (object obj in dt.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					if (wgMjController.IsElevator(int.Parse(dataRow["f_ControllerSN"].ToString())) && (int)dataRow["f_ReaderNO"] >= 2)
					{
						dt.Rows.Remove(dataRow);
						flag = true;
						dt.AcceptChanges();
					}
				}
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00138DE8 File Offset: 0x00137DE8
		private void txtPasswordNew_KeyPress(object sender, KeyPressEventArgs e)
		{
			int num;
			if (e.KeyChar != '\b' && !int.TryParse(e.KeyChar.ToString(), out num))
			{
				e.Handled = true;
			}
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00138E1C File Offset: 0x00137E1C
		private void updateManualPasswordKeypadEnableGrid()
		{
			this.dtReader = (this.dataGridView4.DataSource as DataView).Table;
			string text = "";
			for (int i = 0; i <= this.dtReader.Rows.Count - 1; i++)
			{
				if (this.dtReader.Rows[i]["f_InputCardno_Enabled"].ToString() == "1")
				{
					text = text + this.dtReader.Rows[i]["f_ReaderID"].ToString() + ",";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.runSql(string.Format(" UPDATE t_b_Reader SET  f_InputCardno_Enabled = 1 WHERE f_ReaderID IN ({0}) ", text + "0"));
			}
			text = "";
			for (int j = 0; j <= this.dtReader.Rows.Count - 1; j++)
			{
				if (this.dtReader.Rows[j]["f_InputCardno_Enabled"].ToString() != "1")
				{
					text = text + this.dtReader.Rows[j]["f_ReaderID"].ToString() + ",";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.runSql(string.Format(" UPDATE t_b_Reader SET  f_InputCardno_Enabled = 0 WHERE f_ReaderID IN ({0}) ", text + "0"));
			}
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00138F80 File Offset: 0x00137F80
		private void updatePasswordKeypadEnableGrid()
		{
			this.dtPasswordKeypad = (this.dataGridView1.DataSource as DataView).Table;
			string text = "";
			for (int i = 0; i <= this.dtPasswordKeypad.Rows.Count - 1; i++)
			{
				if (this.dtPasswordKeypad.Rows[i]["f_PasswordEnabled"].ToString() == "1")
				{
					text = text + this.dtPasswordKeypad.Rows[i]["f_ReaderID"].ToString() + ",";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.runSql(string.Format(" UPDATE t_b_Reader SET  f_PasswordEnabled = 1 WHERE f_ReaderID IN ({0}) ", text + "0"));
			}
			text = "";
			for (int j = 0; j <= this.dtPasswordKeypad.Rows.Count - 1; j++)
			{
				if (this.dtPasswordKeypad.Rows[j]["f_PasswordEnabled"].ToString() != "1")
				{
					text = text + this.dtPasswordKeypad.Rows[j]["f_ReaderID"].ToString() + ",";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.runSql(string.Format(" UPDATE t_b_Reader SET  f_PasswordEnabled = 0 WHERE f_ReaderID IN ({0}) ", text + "0"));
			}
		}

		// Token: 0x04001E3C RID: 7740
		private bool bLoadedFinished;

		// Token: 0x04001E3D RID: 7741
		private string dgvSql;

		// Token: 0x04001E3E RID: 7742
		private DataSet ds;

		// Token: 0x04001E3F RID: 7743
		private DataTable dtPasswordKeypad;

		// Token: 0x04001E40 RID: 7744
		private DataTable dtReader;

		// Token: 0x04001E41 RID: 7745
		private DataTable dtReaderPassword;

		// Token: 0x04001E42 RID: 7746
		private DataTable dtUserData;

		// Token: 0x04001E43 RID: 7747
		private DataView dvPasswordKeypad;

		// Token: 0x04001E44 RID: 7748
		private DataView dvReader;

		// Token: 0x04001E45 RID: 7749
		private DataView dvReaderPassword;

		// Token: 0x04001E46 RID: 7750
		private DataView dvUserData;

		// Token: 0x04001E47 RID: 7751
		private int startRecordIndex;

		// Token: 0x04001E48 RID: 7752
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04001E49 RID: 7753
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04001E4A RID: 7754
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04001E4B RID: 7755
		private ArrayList arrReaderID = new ArrayList();

		// Token: 0x04001E4D RID: 7757
		private int MaxRecord = 1000;

		// Token: 0x04001E4E RID: 7758
		private string recNOMax = "";

		// Token: 0x04001E4F RID: 7759
		private string strGroupFilter = "";
	}
}
