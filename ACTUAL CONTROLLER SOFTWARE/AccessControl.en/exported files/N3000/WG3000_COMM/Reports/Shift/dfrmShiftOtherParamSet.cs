using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000374 RID: 884
	public partial class dfrmShiftOtherParamSet : frmN3000
	{
		// Token: 0x06001D00 RID: 7424 RVA: 0x002650C1 File Offset: 0x002640C1
		public dfrmShiftOtherParamSet()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x002650CF File Offset: 0x002640CF
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x002650D8 File Offset: 0x002640D8
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.setShiftOtherAttendanceParam(1, this.nudLateTimeout.Value.ToString());
			this.setShiftOtherAttendanceParam(4, this.nudLeaveTimeout.Value.ToString());
			this.setShiftOtherAttendanceParam(7, this.nudOvertimeTimeout.Value.ToString());
			this.setShiftOtherAttendanceParam(17, this.nudAheadMinutes.Value.ToString());
			this.setShiftOtherAttendanceParam(18, this.nudAheadMinutes.Value.ToString());
			this.setShiftOtherAttendanceParam(19, this.nudAheadMinutes.Value.ToString());
			this.setShiftOtherAttendanceParam(20, this.nudOvertimeMinutes.Value.ToString());
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x002651AF File Offset: 0x002641AF
		private void dfrmShiftOtherParamSet_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.funcCtrlShiftQ();
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x002651EE File Offset: 0x002641EE
		private void dfrmShiftOtherParamSet_Load(object sender, EventArgs e)
		{
			this.loadOperatorPrivilege();
			this.getShiftOtherAttendanceParam();
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x002651FC File Offset: 0x002641FC
		private void funcCtrlShiftQ()
		{
			this.Label2.Visible = true;
			this.Label14.Visible = true;
			this.nudOvertimeMinutes.Visible = true;
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00265224 File Offset: 0x00264224
		private void getShiftOtherAttendanceParam()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.getShiftOtherAttendanceParam_Acc();
				return;
			}
			string text = "SELECT * FROM t_a_Shift_Attendence";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						int num = (int)sqlDataReader["f_No"];
						if (num <= 4)
						{
							if (num != 1)
							{
								if (num == 4)
								{
									this.nudLeaveTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
								}
							}
							else
							{
								this.nudLateTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
							}
						}
						else if (num != 7)
						{
							switch (num)
							{
							case 18:
								this.nudAheadMinutes.Value = int.Parse((string)sqlDataReader["f_Value"]);
								break;
							case 20:
								this.nudOvertimeMinutes.Value = int.Parse((string)sqlDataReader["f_Value"]);
								break;
							}
						}
						else
						{
							this.nudOvertimeTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
						}
					}
					sqlDataReader.Close();
				}
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x002653D8 File Offset: 0x002643D8
		private void getShiftOtherAttendanceParam_Acc()
		{
			string text = "SELECT * FROM t_a_Shift_Attendence";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					while (oleDbDataReader.Read())
					{
						int num = (int)oleDbDataReader["f_No"];
						if (num <= 4)
						{
							if (num != 1)
							{
								if (num == 4)
								{
									this.nudLeaveTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
								}
							}
							else
							{
								this.nudLateTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
							}
						}
						else if (num != 7)
						{
							switch (num)
							{
							case 18:
								this.nudAheadMinutes.Value = int.Parse((string)oleDbDataReader["f_Value"]);
								break;
							case 20:
								this.nudOvertimeMinutes.Value = int.Parse((string)oleDbDataReader["f_Value"]);
								break;
							}
						}
						else
						{
							this.nudOvertimeTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
						}
					}
					oleDbDataReader.Close();
				}
			}
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x00265580 File Offset: 0x00264580
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuShiftRule";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnOK.Visible = false;
			}
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x002655AE File Offset: 0x002645AE
		private void setShiftOtherAttendanceParam(int no, string val)
		{
			wgAppConfig.runUpdateSql("UPDATE t_a_Shift_Attendence  SET [f_value]=" + wgTools.PrepareStrNUnicode(val) + " WHERE [f_NO]= " + no.ToString());
		}
	}
}
