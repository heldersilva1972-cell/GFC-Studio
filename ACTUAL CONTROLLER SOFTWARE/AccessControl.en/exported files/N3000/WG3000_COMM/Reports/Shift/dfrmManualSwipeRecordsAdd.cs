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

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036D RID: 877
	public partial class dfrmManualSwipeRecordsAdd : frmN3000
	{
		// Token: 0x06001C9B RID: 7323 RVA: 0x0025C5DC File Offset: 0x0025B5DC
		public dfrmManualSwipeRecordsAdd()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x0025C638 File Offset: 0x0025B638
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

		// Token: 0x06001C9D RID: 7325 RVA: 0x0025C67C File Offset: 0x0025B67C
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

		// Token: 0x06001C9E RID: 7326 RVA: 0x0025C6FC File Offset: 0x0025B6FC
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.strGroupFilter == "")
			{
				icConsumerShare.selectAllUsers();
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ");
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter() + " AND  f_AttendEnabled > 0 ");
				return;
			}
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			if (this.strGroupFilter != "")
			{
				this.dv = new DataView(this.dt);
				this.dv.RowFilter = this.strGroupFilter;
				for (int i = 0; i < this.dv.Count; i++)
				{
					this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
				}
			}
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			wgTools.WriteLine("btnAddAllUsers_Click End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0025C890 File Offset: 0x0025B890
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0025C8A2 File Offset: 0x0025B8A2
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0025C8AC File Offset: 0x0025B8AC
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				icConsumerShare.selectNoneUsers();
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ");
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("({0}) AND ({1})", icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ", this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter() + " AND  f_AttendEnabled > 0 ");
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0025C987 File Offset: 0x0025B987
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0025C99C File Offset: 0x0025B99C
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			if (this.dgvSelectedUsers.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			int i = 0;
			string text = "";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					while (i < this.dgvSelectedUsers.Rows.Count)
					{
						string text2 = "";
						if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
						{
							while (i < this.dgvSelectedUsers.Rows.Count)
							{
								text2 = text2 + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerID"] + ",";
								if (text2.Length > 2000)
								{
									break;
								}
								i++;
							}
							text2 += "0";
						}
						else
						{
							i = this.dgvSelectedUsers.Rows.Count;
						}
						text = "INSERT INTO [t_d_ManualCardRecord] (f_ConsumerID, f_ReadDate,f_Note)";
						text = string.Concat(new string[]
						{
							text,
							" SELECT t_b_Consumer.f_ConsumerID , ",
							wgTools.PrepareStr(this.dtpStartDate.Value.ToString("yyyy-MM-dd") + this.dateEndHMS2.Value.ToString(" HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
							" AS [f_ReadDate] , ",
							wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
							" AS [f_Note]  FROM t_b_Consumer"
						});
						if (text2 != "")
						{
							text = text + " WHERE [f_ConsumerID] IN (" + text2 + " ) ";
						}
						sqlCommand.CommandText = text;
						if (sqlCommand.ExecuteNonQuery() <= 0)
						{
							flag = false;
							break;
						}
						flag = true;
						wgTools.WriteLine("INSERT INTO [t_d_Leave] End");
					}
				}
			}
			if (flag)
			{
				XMessageBox.Show(string.Concat(new string[]
				{
					(sender as Button).Text,
					"  ",
					i.ToString(),
					"  ",
					CommonStr.strUnit,
					"  ",
					CommonStr.strSuccessfully
				}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			XMessageBox.Show((sender as Button).Text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0025CC70 File Offset: 0x0025BC70
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			int i = 0;
			string text = "";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					while (i < this.dgvSelectedUsers.Rows.Count)
					{
						string text2 = "";
						if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
						{
							while (i < this.dgvSelectedUsers.Rows.Count)
							{
								text2 = text2 + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerID"] + ",";
								if (text2.Length > 2000)
								{
									break;
								}
								i++;
							}
							text2 += "0";
						}
						else
						{
							i = this.dgvSelectedUsers.Rows.Count;
						}
						text = "INSERT INTO [t_d_ManualCardRecord] (f_ConsumerID, f_ReadDate,f_Note)";
						text = string.Concat(new string[]
						{
							text,
							" SELECT t_b_Consumer.f_ConsumerID , ",
							wgTools.PrepareStr(this.dtpStartDate.Value.ToString("yyyy-MM-dd") + this.dateEndHMS2.Value.ToString(" HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
							" AS [f_ReadDate] , ",
							wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
							" AS [f_Note]  FROM t_b_Consumer"
						});
						if (text2 != "")
						{
							text = text + " WHERE [f_ConsumerID] IN (" + text2 + " ) ";
						}
						oleDbCommand.CommandText = text;
						if (oleDbCommand.ExecuteNonQuery() <= 0)
						{
							flag = false;
							break;
						}
						flag = true;
						wgTools.WriteLine("INSERT INTO [t_d_ManualCardRecord] End");
					}
				}
			}
			if (flag)
			{
				XMessageBox.Show(string.Concat(new string[]
				{
					(sender as Button).Text,
					"  ",
					i.ToString(),
					"  ",
					CommonStr.strUnit,
					"  ",
					CommonStr.strSuccessfully
				}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			XMessageBox.Show((sender as Button).Text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0025CF34 File Offset: 0x0025BF34
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0025CF44 File Offset: 0x0025BF44
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ";
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
					dataView.RowFilter = string.Format("{0} AND ({1})", icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ", this.strGroupFilter);
				}
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ");
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ", this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format(" {0}", icConsumerShare.getSelectedRowfilter() + " AND  f_AttendEnabled > 0 ");
			}
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0025D16D File Offset: 0x0025C16D
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0025D184 File Offset: 0x0025C184
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

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0025D1F4 File Offset: 0x0025C1F4
		private void dfrmManualSwipeRecordsAdd_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.loadGroupData();
			this.dtpStartDate.Value = DateTime.Today;
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
			this.dateEndHMS2.CustomFormat = "HH:mm";
			this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS2.Value = DateTime.Parse("08:30:00");
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0025D314 File Offset: 0x0025C314
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

		// Token: 0x06001CAB RID: 7339 RVA: 0x0025D3C8 File Offset: 0x0025C3C8
		private void loadUserData()
		{
			wgTools.WriteLine("loadUserData Start");
			string text = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID ";
			text += " FROM t_b_Consumer WHERE f_AttendEnabled > 0 ORDER BY f_ConsumerNO ASC ";
			this.dtUser = new DataTable();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUser);
						}
					}
					goto IL_00D0;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtUser);
					}
				}
			}
			IL_00D0:
			wgTools.WriteLine("da.Fill End");
			try
			{
				this.dtUser.PrimaryKey = new DataColumn[] { this.dtUser.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			this.dv = new DataView(this.dtUser);
			this.dvSelected = new DataView(this.dtUser);
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				this.dgvUsers.Columns[i].DataPropertyName = this.dtUser.Columns[i].ColumnName;
				this.dgvSelectedUsers.Columns[i].DataPropertyName = this.dtUser.Columns[i].ColumnName;
			}
			wgTools.WriteLine("loadUserData End");
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0025D63C File Offset: 0x0025C63C
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0025D65C File Offset: 0x0025C65C
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = icConsumerShare.getOptionalRowfilter() + " AND  f_AttendEnabled > 0 ";
			this.dvSelected.RowFilter = icConsumerShare.getSelectedRowfilter() + " AND  f_AttendEnabled > 0 ";
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dgvUsers.ColumnCount)
			{
				this.dgvUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
				this.dgvSelectedUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
				num++;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x0025D788 File Offset: 0x0025C788
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
			this.groupBox1.Enabled = true;
			this.btnOK.Enabled = true;
		}

		// Token: 0x0400371D RID: 14109
		private DataTable dt;

		// Token: 0x0400371E RID: 14110
		private DataTable dtUser;

		// Token: 0x0400371F RID: 14111
		private DataView dv;

		// Token: 0x04003720 RID: 14112
		private DataView dv1;

		// Token: 0x04003721 RID: 14113
		private DataView dv2;

		// Token: 0x04003722 RID: 14114
		private DataView dvSelected;

		// Token: 0x04003723 RID: 14115
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04003724 RID: 14116
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04003725 RID: 14117
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04003726 RID: 14118
		private string strGroupFilter = "";
	}
}
