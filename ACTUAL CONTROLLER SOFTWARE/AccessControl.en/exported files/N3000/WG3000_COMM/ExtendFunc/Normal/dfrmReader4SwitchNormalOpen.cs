using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Normal
{
	// Token: 0x02000303 RID: 771
	public partial class dfrmReader4SwitchNormalOpen : frmN3000
	{
		// Token: 0x060016FC RID: 5884 RVA: 0x001DDB6A File Offset: 0x001DCB6A
		public dfrmReader4SwitchNormalOpen()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x001DDB9E File Offset: 0x001DCB9E
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x001DDBB0 File Offset: 0x001DCBB0
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.updatePasswordKeypadEnableGrid();
			if (this.chkSelectedUsers.Checked)
			{
				this.strEnabledReaders = string.Format("{0};{1}", this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex], this.strEnabledReaders);
			}
			else
			{
				this.strEnabledReaders = string.Format("{0};{1}", 0, this.strEnabledReaders);
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x001DDC28 File Offset: 0x001DCC28
		private void chkSelectedUsers_CheckedChanged(object sender, EventArgs e)
		{
			this.cbof_ControlSegID.Enabled = this.chkSelectedUsers.Checked;
			if (!this.chkSelectedUsers.Checked)
			{
				this.cbof_ControlSegID.SelectedIndex = 0;
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x001DDC5C File Offset: 0x001DCC5C
		private void dfrmReader4SwitchNormalOpen_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.chkSelectedUsers.Visible = wgAppConfig.getParamValBoolByNO(121);
			this.cbof_ControlSegID.Visible = wgAppConfig.getParamValBoolByNO(121);
			this.loadControlSegData();
			this.chkSelectedUsers.Checked = this.timeProfile > 0;
			if (this.chkSelectedUsers.Checked)
			{
				for (int i = 0; i < this.controlSegIDList.Length; i++)
				{
					if (this.controlSegIDList[i] == this.timeProfile)
					{
						this.cbof_ControlSegID.SelectedIndex = i;
						break;
					}
				}
			}
			this.cbof_ControlSegID.Enabled = this.chkSelectedUsers.Checked;
			this.fillPasswordKeypadEnableGrid();
			this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x001DDD20 File Offset: 0x001DCD20
		private void fillPasswordKeypadEnableGrid()
		{
			string text = " SELECT ";
			text += " t_b_Reader.f_ReaderID , t_b_Controller.f_ControllerSN , t_b_Reader.f_ReaderNO , t_b_Reader.f_ReaderName , 0 as f_Enabled , t_b_Controller.f_ZoneID  FROM t_b_Reader INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  ORDER BY [f_ReaderID] ";
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
			DataView dataView = new DataView(this.dtPasswordKeypad);
			if (!string.IsNullOrEmpty(this.strEnabledReaders))
			{
				dataView.RowFilter = string.Format("f_ReaderID IN ({0})", this.strEnabledReaders);
				for (int i = 0; i < dataView.Count; i++)
				{
					dataView[i]["f_Enabled"] = 1;
				}
			}
			else
			{
				for (int j = 0; j < dataView.Count; j++)
				{
					dataView[j]["f_Enabled"] = 1;
				}
			}
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.DataSource = this.dvPasswordKeypad;
			int num = 0;
			while (num < this.dvPasswordKeypad.Table.Columns.Count && num < this.dataGridView1.ColumnCount)
			{
				this.dataGridView1.Columns[num].DataPropertyName = this.dvPasswordKeypad.Table.Columns[num].ColumnName;
				num++;
			}
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x001DDF7C File Offset: 0x001DCF7C
		private void loadControlSegData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadControlSegData_Acc();
				return;
			}
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegIDList[0] = 1;
			new SqlCommand();
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				try
				{
					string text = " SELECT ";
					using (SqlCommand sqlCommand = new SqlCommand(text + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,    CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID    FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						int num = 1;
						while (sqlDataReader.Read())
						{
							this.cbof_ControlSegID.Items.Add(sqlDataReader["f_ControlSegID"]);
							this.controlSegIDList[num] = (int)sqlDataReader["f_ControlSegIDBak"];
							if (this.controlSegIDList[num] >= 16)
							{
								break;
							}
							num++;
						}
						sqlDataReader.Close();
						if (this.cbof_ControlSegID.Items.Count > 0)
						{
							this.cbof_ControlSegID.SelectedIndex = 0;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x001DE0D0 File Offset: 0x001DD0D0
		private void loadControlSegData_Acc()
		{
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegIDList[0] = 1;
			OleDbConnection oleDbConnection = null;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				try
				{
					string text = " SELECT ";
					using (OleDbCommand oleDbCommand = new OleDbCommand(text + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,   IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID   FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						int num = 1;
						while (oleDbDataReader.Read())
						{
							this.cbof_ControlSegID.Items.Add(oleDbDataReader["f_ControlSegID"]);
							this.controlSegIDList[num] = (int)oleDbDataReader["f_ControlSegIDBak"];
							if (this.controlSegIDList[num] >= 16)
							{
								break;
							}
							num++;
						}
						oleDbDataReader.Close();
						if (this.cbof_ControlSegID.Items.Count > 0)
						{
							this.cbof_ControlSegID.SelectedIndex = 0;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x001DE210 File Offset: 0x001DD210
		private void updatePasswordKeypadEnableGrid()
		{
			this.dtPasswordKeypad = (this.dataGridView1.DataSource as DataView).Table;
			int num = 0;
			string text = "";
			for (int i = 0; i <= this.dtPasswordKeypad.Rows.Count - 1; i++)
			{
				if (this.dtPasswordKeypad.Rows[i]["f_Enabled"].ToString() == "1")
				{
					text = text + this.dtPasswordKeypad.Rows[i]["f_ReaderID"].ToString() + ",";
					num++;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.strEnabledReaders = text + "0";
			}
			else
			{
				this.strEnabledReaders = "0";
			}
			if (num == this.dtPasswordKeypad.Rows.Count)
			{
				this.strEnabledReaders = ";";
			}
		}

		// Token: 0x04002FB8 RID: 12216
		private DataTable dtPasswordKeypad;

		// Token: 0x04002FB9 RID: 12217
		private DataView dvPasswordKeypad;

		// Token: 0x04002FBA RID: 12218
		private int[] controlSegIDList = new int[256];

		// Token: 0x04002FBB RID: 12219
		public string strEnabledReaders = "";

		// Token: 0x04002FBC RID: 12220
		public int timeProfile;
	}
}
