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
	// Token: 0x0200036B RID: 875
	public partial class dfrmLeaveAdd : frmN3000
	{
		// Token: 0x06001C7D RID: 7293 RVA: 0x002593F0 File Offset: 0x002583F0
		public dfrmLeaveAdd()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x0025944C File Offset: 0x0025844C
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

		// Token: 0x06001C7F RID: 7295 RVA: 0x00259490 File Offset: 0x00258490
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

		// Token: 0x06001C80 RID: 7296 RVA: 0x00259510 File Offset: 0x00258510
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

		// Token: 0x06001C81 RID: 7297 RVA: 0x002596A4 File Offset: 0x002586A4
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x002596B6 File Offset: 0x002586B6
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x002596C0 File Offset: 0x002586C0
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

		// Token: 0x06001C84 RID: 7300 RVA: 0x0025979B File Offset: 0x0025879B
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x002597B0 File Offset: 0x002587B0
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
			if (this.cboHolidayType.Items.Count == 0)
			{
				return;
			}
			int num = 0;
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
						text = "INSERT INTO [t_d_Leave] (f_ConsumerID, f_Value ,f_Value1,f_Value2,f_Value3,f_HolidayType,f_Notes)";
						text = string.Concat(new string[]
						{
							text,
							" SELECT t_b_Consumer.f_ConsumerID , ",
							wgTools.PrepareStrNUnicode(this.dtpStartDate.Value.ToString("yyyy-MM-dd")),
							" AS [f_Value] , ",
							wgTools.PrepareStrNUnicode(this.cboStart.Text),
							" AS [f_Value1] , ",
							wgTools.PrepareStrNUnicode(this.dtpEndDate.Value.ToString("yyyy-MM-dd")),
							" AS [f_Value2] , ",
							wgTools.PrepareStrNUnicode(this.cboEnd.Text),
							" AS [f_Value3] , ",
							wgTools.PrepareStrNUnicode(this.cboHolidayType.Text),
							" AS [f_HolidayType] , ",
							wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
							" AS [f_Notes]  FROM t_b_Consumer"
						});
						if (text2 != "")
						{
							text = text + " WHERE [f_ConsumerID] IN (" + text2 + " ) ";
						}
						sqlCommand.CommandText = text;
						num = sqlCommand.ExecuteNonQuery();
						if (num <= 0)
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
				XMessageBox.Show(string.Concat(new object[]
				{
					(sender as Button).Text,
					"  ",
					num,
					"  ",
					CommonStr.strUnit,
					"  ",
					CommonStr.strSuccessfully
				}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			XMessageBox.Show((sender as Button).Text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00259B00 File Offset: 0x00258B00
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count <= 0)
			{
				return;
			}
			if (this.cboHolidayType.Items.Count == 0)
			{
				return;
			}
			int num = 0;
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
						text = "INSERT INTO [t_d_Leave] (f_ConsumerID, f_Value ,f_Value1,f_Value2,f_Value3,f_HolidayType,f_Notes)";
						text = string.Concat(new string[]
						{
							text,
							" SELECT t_b_Consumer.f_ConsumerID , ",
							wgTools.PrepareStrNUnicode(this.dtpStartDate.Value.ToString("yyyy-MM-dd")),
							" AS [f_Value] , ",
							wgTools.PrepareStrNUnicode(this.cboStart.Text),
							" AS [f_Value1] , ",
							wgTools.PrepareStrNUnicode(this.dtpEndDate.Value.ToString("yyyy-MM-dd")),
							" AS [f_Value2] , ",
							wgTools.PrepareStrNUnicode(this.cboEnd.Text),
							" AS [f_Value3] , ",
							wgTools.PrepareStrNUnicode(this.cboHolidayType.Text),
							" AS [f_HolidayType] , ",
							wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
							" AS [f_Notes]  FROM t_b_Consumer"
						});
						if (text2 != "")
						{
							text = text + " WHERE [f_ConsumerID] IN (" + text2 + " ) ";
						}
						oleDbCommand.CommandText = text;
						num = oleDbCommand.ExecuteNonQuery();
						if (num <= 0)
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
				XMessageBox.Show(string.Concat(new object[]
				{
					(sender as Button).Text,
					"  ",
					num,
					"  ",
					CommonStr.strUnit,
					"  ",
					CommonStr.strSuccessfully
				}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			XMessageBox.Show((sender as Button).Text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00259E40 File Offset: 0x00258E40
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00259E50 File Offset: 0x00258E50
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

		// Token: 0x06001C89 RID: 7305 RVA: 0x0025A079 File Offset: 0x00259079
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x0025A090 File Offset: 0x00259090
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

		// Token: 0x06001C8B RID: 7307 RVA: 0x0025A100 File Offset: 0x00259100
		private void dfrmLeaveAdd_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.cboStart.Items.Clear();
			this.cboStart.Items.AddRange(new string[]
			{
				CommonStr.strAM,
				CommonStr.strPM
			});
			this.cboEnd.Items.Clear();
			this.cboEnd.Items.AddRange(new string[]
			{
				CommonStr.strAM,
				CommonStr.strPM
			});
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.loadGroupData();
			this.ds = new DataSet();
			string text = " SELECT *  FROM [t_a_HolidayType]";
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.ds, "t_a_HolidayType");
						}
					}
				}
				comShift_Acc.localizedHolidayType(this.ds.Tables["t_a_HolidayType"]);
			}
			else
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(this.ds, "t_a_HolidayType");
						}
					}
				}
				comShift.localizedHolidayType(this.ds.Tables["t_a_HolidayType"]);
			}
			this.cboHolidayType.Items.Clear();
			for (int i = 0; i <= this.ds.Tables["t_a_HolidayType"].Rows.Count - 1; i++)
			{
				this.cboHolidayType.Items.Add(this.ds.Tables["t_a_HolidayType"].Rows[i]["f_HolidayType"]);
			}
			if (this.cboHolidayType.Items.Count >= 0)
			{
				this.cboHolidayType.SelectedIndex = 0;
			}
			this.dtpStartDate.Value = DateTime.Today;
			this.dtpEndDate.Value = DateTime.Today;
			if (this.cboStart.Items.Count > 0)
			{
				this.cboStart.SelectedIndex = 0;
			}
			if (this.cboEnd.Items.Count > 1)
			{
				this.cboEnd.SelectedIndex = 1;
			}
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x0025A4A4 File Offset: 0x002594A4
		private void dtpStartDate_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.dtpStartDate.Value > this.dtpEndDate.Value)
				{
					this.cboEnd.Text = CommonStr.strPM;
				}
				this.dtpEndDate.MinDate = this.dtpStartDate.Value;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0025A51C File Offset: 0x0025951C
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

		// Token: 0x06001C8E RID: 7310 RVA: 0x0025A5D0 File Offset: 0x002595D0
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

		// Token: 0x06001C8F RID: 7311 RVA: 0x0025A844 File Offset: 0x00259844
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0025A864 File Offset: 0x00259864
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = icConsumerShare.getOptionalRowfilter();
			this.dvSelected.RowFilter = icConsumerShare.getSelectedRowfilter();
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

		// Token: 0x06001C91 RID: 7313 RVA: 0x0025A97C File Offset: 0x0025997C
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

		// Token: 0x040036DC RID: 14044
		private DataSet ds;

		// Token: 0x040036DD RID: 14045
		private DataTable dt;

		// Token: 0x040036DE RID: 14046
		private DataTable dtUser;

		// Token: 0x040036DF RID: 14047
		private DataView dv;

		// Token: 0x040036E0 RID: 14048
		private DataView dv1;

		// Token: 0x040036E1 RID: 14049
		private DataView dv2;

		// Token: 0x040036E2 RID: 14050
		private DataView dvSelected;

		// Token: 0x040036E3 RID: 14051
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040036E4 RID: 14052
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040036E5 RID: 14053
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040036E6 RID: 14054
		private string strGroupFilter = "";
	}
}
