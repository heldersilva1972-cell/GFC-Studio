using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036C RID: 876
	public partial class dfrmLeaveEdit : frmN3000
	{
		// Token: 0x06001C94 RID: 7316 RVA: 0x0025BD3C File Offset: 0x0025AD3C
		public dfrmLeaveEdit()
		{
			this.InitializeComponent();
			this.set();
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x0025BD50 File Offset: 0x0025AD50
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x0025BD5F File Offset: 0x0025AD5F
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0025BD6E File Offset: 0x0025AD6E
		private void dfrmLeaveEdit_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x0025BD70 File Offset: 0x0025AD70
		private void set()
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
			DataSet dataSet = new DataSet();
			string text = " SELECT *  FROM [t_a_HolidayType]";
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(dataSet, "t_a_HolidayType");
						}
					}
				}
				comShift_Acc.localizedHolidayType(dataSet.Tables["t_a_HolidayType"]);
			}
			else
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(dataSet, "t_a_HolidayType");
						}
					}
				}
				comShift.localizedHolidayType(dataSet.Tables["t_a_HolidayType"]);
			}
			this.cboHolidayType.Items.Clear();
			for (int i = 0; i <= dataSet.Tables["t_a_HolidayType"].Rows.Count - 1; i++)
			{
				this.cboHolidayType.Items.Add(dataSet.Tables["t_a_HolidayType"].Rows[i]["f_HolidayType"]);
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
		}
	}
}
