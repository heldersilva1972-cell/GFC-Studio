using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000308 RID: 776
	public partial class dfrmPatrolSetup : frmN3000
	{
		// Token: 0x0600174E RID: 5966 RVA: 0x001E44C8 File Offset: 0x001E34C8
		public dfrmPatrolSetup()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvOptional);
			wgAppConfig.custDataGridview(ref this.dgvSelected);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage3.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x001E45A4 File Offset: 0x001E35A4
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

		// Token: 0x06001750 RID: 5968 RVA: 0x001E45E8 File Offset: 0x001E35E8
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

		// Token: 0x06001751 RID: 5969 RVA: 0x001E4668 File Offset: 0x001E3668
		private void btnAddAllReaders_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolReader = true;
			try
			{
				for (int i = 0; i < this.dtPatrolReader.Rows.Count; i++)
				{
					this.dtPatrolReader.Rows[i]["f_Selected"] = 1;
				}
				this.dtPatrolReader.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x001E46F4 File Offset: 0x001E36F4
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolUsers = true;
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
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x001E48E4 File Offset: 0x001E38E4
		private void btnAddOneReader_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolReader = true;
			wgAppConfig.selectObject(this.dgvOptional);
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x001E48F8 File Offset: 0x001E38F8
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolUsers = true;
			wgAppConfig.selectObject(this.dgvUsers);
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x001E490C File Offset: 0x001E390C
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x001E4914 File Offset: 0x001E3914
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolUsers = true;
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
			}
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x001E4AB0 File Offset: 0x001E3AB0
		private void btnDeleteAllReaders_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolReader = true;
			try
			{
				for (int i = 0; i < this.dtPatrolReader.Rows.Count; i++)
				{
					this.dtPatrolReader.Rows[i]["f_Selected"] = 0;
				}
				this.dtPatrolReader.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x001E4B3C File Offset: 0x001E3B3C
		private void btnDeleteOneReader_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolReader = true;
			wgAppConfig.deselectObject(this.dgvSelected);
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x001E4B50 File Offset: 0x001E3B50
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			this.bNeedUpdatePatrolUsers = true;
			wgAppConfig.deselectObject(this.dgvSelectedUsers);
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x001E4B64 File Offset: 0x001E3B64
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				wgAppConfig.setSystemParamValue(27, "", this.nudPatrolAbsentTimeout.Value.ToString(), "");
				wgAppConfig.setSystemParamValue(28, "", this.nudPatrolAllowTimeout.Value.ToString(), "");
				if (this.bNeedUpdatePatrolReader)
				{
					string text = " DELETE FROM t_b_Reader4Patrol ";
					wgAppConfig.runUpdateSql(text);
					if (this.dvPatrolReaderSelected.Count > 0)
					{
						for (int i = 0; i <= this.dvPatrolReaderSelected.Count - 1; i++)
						{
							text = " INSERT INTO t_b_Reader4Patrol";
							wgAppConfig.runUpdateSql(string.Concat(new object[]
							{
								text,
								" (f_ReaderID)  Values(",
								this.dvPatrolReaderSelected[i]["f_ReaderID"],
								" )"
							}));
						}
					}
				}
				if (this.bNeedUpdatePatrolUsers)
				{
					string text = " Delete  FROM t_d_PatrolUsers  ";
					wgAppConfig.runUpdateSql(text);
					if (this.dgvSelectedUsers.DataSource != null)
					{
						using (DataView dataView = this.dgvSelectedUsers.DataSource as DataView)
						{
							if (dataView.Count > 0)
							{
								for (int j = 0; j <= dataView.Count - 1; j++)
								{
									wgAppConfig.runUpdateSql("INSERT INTO [t_d_PatrolUsers](f_ConsumerID ) VALUES( " + dataView[j]["f_ConsumerID"].ToString() + ")");
								}
							}
						}
					}
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x001E4D34 File Offset: 0x001E3D34
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x001E4D44 File Offset: 0x001E3D44
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
					return;
				}
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
						return;
					}
					dataView.RowFilter = "f_Selected = 0 ";
					string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
					dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
					this.strGroupFilter = string.Format("  {0} ", groupQuery);
				}
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x001E4EF3 File Offset: 0x001E3EF3
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x001E4F08 File Offset: 0x001E3F08
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

		// Token: 0x0600175F RID: 5983 RVA: 0x001E4F78 File Offset: 0x001E3F78
		private void dfrmPatrolSetup_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			bool flag = false;
			string text = "mnuPatrolDetailData";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnOK.Visible = false;
				this.nudPatrolAbsentTimeout.Enabled = false;
				this.nudPatrolAllowTimeout.Enabled = false;
				this.btnDelAllUsers.Enabled = false;
				this.btnDeleteAllReaders.Enabled = false;
				this.btnDeleteOneReader.Enabled = false;
				this.btnDelOneUser.Enabled = false;
				this.btnAddAllReaders.Enabled = false;
				this.btnAddAllUsers.Enabled = false;
				this.btnAddOneReader.Enabled = false;
				this.btnAddOneUser.Enabled = false;
			}
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmPatrolSetup_Load_Acc(sender, e);
				return;
			}
			this.nudPatrolAbsentTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(27).ToString(), CultureInfo.InvariantCulture);
			this.nudPatrolAllowTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(28).ToString(), CultureInfo.InvariantCulture);
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			SqlCommand sqlCommand = new SqlCommand();
			try
			{
				sqlCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", sqlConnection);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
				sqlDataAdapter.Fill(this.ds, "optionalReader");
				sqlCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(this.ds, "optionalReader");
				this.dvPatrolReader = new DataView(this.ds.Tables["optionalReader"]);
				this.dvPatrolReader.RowFilter = " f_Selected = 0";
				this.dvPatrolReaderSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvPatrolReaderSelected.RowFilter = " f_Selected = 1";
				this.dtPatrolReader = this.ds.Tables["optionalReader"];
				try
				{
					this.dtPatrolReader.PrimaryKey = new DataColumn[] { this.dtPatrolReader.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
				}
				this.dvPatrolReader.RowFilter = "f_Selected = 0";
				this.dvPatrolReaderSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dvPatrolReader;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvPatrolReaderSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.loadGroupData();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.UserID2.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x001E5394 File Offset: 0x001E4394
		private void dfrmPatrolSetup_Load_Acc(object sender, EventArgs e)
		{
			this.nudPatrolAbsentTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(27).ToString(), CultureInfo.InvariantCulture);
			this.nudPatrolAllowTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(28).ToString(), CultureInfo.InvariantCulture);
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			OleDbCommand oleDbCommand = new OleDbCommand();
			try
			{
				oleDbCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "optionalReader");
				oleDbCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "optionalReader");
				this.dvPatrolReader = new DataView(this.ds.Tables["optionalReader"]);
				this.dvPatrolReader.RowFilter = " f_Selected = 0";
				this.dvPatrolReaderSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvPatrolReaderSelected.RowFilter = " f_Selected = 1";
				this.dtPatrolReader = this.ds.Tables["optionalReader"];
				try
				{
					this.dtPatrolReader.PrimaryKey = new DataColumn[] { this.dtPatrolReader.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
				}
				this.dvPatrolReader.RowFilter = "f_Selected = 0";
				this.dvPatrolReaderSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dvPatrolReader;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvPatrolReaderSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.loadGroupData();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.UserID2.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x001E56C0 File Offset: 0x001E46C0
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

		// Token: 0x06001762 RID: 5986 RVA: 0x001E5774 File Offset: 0x001E4774
		private DataTable loadUserData4BackWork()
		{
			Thread.Sleep(100);
			wgTools.WriteLine("loadUserData Start");
			this.dtUser1 = new DataTable();
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT  t_b_Consumer.f_ConsumerID ";
				text = text + " , f_ConsumerNO, f_ConsumerName, f_CardNO  , IIF ( t_d_PatrolUsers.f_ConsumerID IS NULL , 0 , 1 ) AS f_Selected  , f_GroupID  FROM t_b_Consumer " + string.Format(" LEFT OUTER JOIN t_d_PatrolUsers ON ( t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID)", new object[0]) + " WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUser1);
						}
					}
					goto IL_00FE;
				}
			}
			text = " SELECT  t_b_Consumer.f_ConsumerID ";
			text += " , f_ConsumerNO, f_ConsumerName, f_CardNO  , CASE WHEN t_d_PatrolUsers.f_ConsumerID IS NULL THEN 0 ELSE 1 END AS f_Selected  , f_GroupID  FROM t_b_Consumer  LEFT OUTER JOIN t_d_PatrolUsers ON ( t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID )  WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC ";
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
			IL_00FE:
			wgTools.WriteLine("da.Fill End");
			try
			{
				this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			dfrmPatrolSetup.lastLoadUsers = icConsumerShare.getUpdateLog();
			dfrmPatrolSetup.dtLastLoad = this.dtUser1;
			return this.dtUser1;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x001E592C File Offset: 0x001E492C
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

		// Token: 0x06001764 RID: 5988 RVA: 0x001E5A3C File Offset: 0x001E4A3C
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

		// Token: 0x0400300B RID: 12299
		private bool bNeedUpdatePatrolReader;

		// Token: 0x0400300C RID: 12300
		private bool bNeedUpdatePatrolUsers;

		// Token: 0x0400300D RID: 12301
		private DataTable dt;

		// Token: 0x0400300E RID: 12302
		private DataTable dtPatrolReader;

		// Token: 0x0400300F RID: 12303
		private DataTable dtUser1;

		// Token: 0x04003010 RID: 12304
		private DataView dv;

		// Token: 0x04003011 RID: 12305
		private DataView dv1;

		// Token: 0x04003012 RID: 12306
		private DataView dv2;

		// Token: 0x04003013 RID: 12307
		private DataView dvPatrolReader;

		// Token: 0x04003014 RID: 12308
		private DataView dvPatrolReaderSelected;

		// Token: 0x04003015 RID: 12309
		private DataView dvSelected;

		// Token: 0x04003016 RID: 12310
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04003017 RID: 12311
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04003018 RID: 12312
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04003019 RID: 12313
		public int DoorID;

		// Token: 0x0400301A RID: 12314
		private DataSet ds = new DataSet("dsPatrol");

		// Token: 0x0400301B RID: 12315
		private static DataTable dtLastLoad;

		// Token: 0x0400301C RID: 12316
		private static string lastLoadUsers = "";

		// Token: 0x0400301D RID: 12317
		public string retValue = "0";

		// Token: 0x0400301E RID: 12318
		private string strGroupFilter = "";
	}
}
