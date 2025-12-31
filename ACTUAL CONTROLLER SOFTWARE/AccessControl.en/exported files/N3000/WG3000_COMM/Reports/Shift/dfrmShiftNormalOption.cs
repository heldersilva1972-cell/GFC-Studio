using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000372 RID: 882
	public partial class dfrmShiftNormalOption : frmN3000
	{
		// Token: 0x06001CE1 RID: 7393 RVA: 0x00261A2F File Offset: 0x00260A2F
		public dfrmShiftNormalOption()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00261A3D File Offset: 0x00260A3D
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x00261A48 File Offset: 0x00260A48
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				wgAppConfig.setSystemParamValue(55, "", this.dtpOffduty0.Value.ToString("HH:mm"), "");
				wgAppConfig.setSystemParamValueBool(56, this.chkEarliest.Checked);
				wgAppConfig.setSystemParamValueBool(57, this.chkOnlyTwoTimes.Checked);
				wgAppConfig.setSystemParamValue(58, "", this.cboLeaveAbsenceTimeout.Text, "");
				wgAppConfig.setSystemParamValueBool(54, this.chkInvalidSwipe.Checked);
				wgAppConfig.setSystemParamValueBool(59, this.chkOnlyOnDuty.Checked);
				wgAppConfig.setSystemParamValue(210, "Param_FullAttendanceSpecialA", this.chkFullAttendanceSpecialA.Checked ? "1" : "0", "全勤 2018-12-02 18:55:06");
				if (!wgAppConfig.getParamValBoolByNO(153) && this.chkMoreCardShift.Checked)
				{
					string text = "UPDATE t_b_Consumer SET f_AttendEnabled = 0, f_ShiftEnabled = 0  WHERE ";
					for (int i = 1; i <= 9; i++)
					{
						text = text + " RTRIM(LTRIM(f_ConsumerNO)) LIKE " + wgTools.PrepareStrNUnicode("%-F" + i.ToString()) + " OR ";
					}
					wgAppConfig.runUpdateSql(text + " 1< 0 ");
				}
				wgAppConfig.setSystemParamValue(153, "More Card Shift OneUser", this.chkMoreCardShift.Checked ? "1" : "0", "One user has more cards for shift 20130508");
			}
			catch
			{
			}
			base.Close();
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x00261BD0 File Offset: 0x00260BD0
		private void chkOnlyOnDuty_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkOnlyOnDuty.Checked)
			{
				this.chkOnlyTwoTimes.Checked = false;
			}
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x00261BEB File Offset: 0x00260BEB
		private void chkOnlyTwoTimes_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkOnlyTwoTimes.Checked)
			{
				this.chkOnlyOnDuty.Checked = false;
			}
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x00261C08 File Offset: 0x00260C08
		private void dfrmShiftNormalOption_Load(object sender, EventArgs e)
		{
			this.dtpOffduty0.CustomFormat = "HH:mm";
			this.dtpOffduty0.Format = DateTimePickerFormat.Custom;
			this.dtpOffduty0.Value = DateTime.Parse("00:00:00");
			this.cboLeaveAbsenceTimeout.Items.Clear();
			double num = 0.0;
			for (int i = 0; i < 48; i++)
			{
				this.cboLeaveAbsenceTimeout.Items.Add(num.ToString("F1", CultureInfo.InvariantCulture));
				num += 0.5;
			}
			this.cboLeaveAbsenceTimeout.Text = "8.0";
			try
			{
				this.dtpOffduty0.Value = DateTime.Parse("2011-1-1 " + wgAppConfig.getSystemParamByNO(55).ToString());
			}
			catch
			{
			}
			try
			{
				this.chkEarliest.Checked = wgAppConfig.getSystemParamByNO(56).ToString() == "1";
			}
			catch
			{
			}
			try
			{
				this.chkOnlyTwoTimes.Checked = wgAppConfig.getSystemParamByNO(57).ToString() == "1";
			}
			catch
			{
			}
			try
			{
				this.cboLeaveAbsenceTimeout.Text = wgAppConfig.getSystemParamByNO(58).ToString();
			}
			catch
			{
			}
			try
			{
				this.chkInvalidSwipe.Checked = wgAppConfig.getSystemParamByNO(54).ToString() == "1";
			}
			catch
			{
			}
			try
			{
				this.chkOnlyOnDuty.Checked = wgAppConfig.getSystemParamByNO(59).ToString() == "1";
			}
			catch
			{
			}
			try
			{
				this.chkFullAttendanceSpecialA.Checked = wgAppConfig.getSystemParamByNO(210).ToString() == "1";
			}
			catch
			{
			}
			this.chkMoreCardShift.Checked = wgAppConfig.getParamValBoolByNO(153);
			this.loadOperatorPrivilege();
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x00261E28 File Offset: 0x00260E28
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuShiftNormalConfigure";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnOK.Visible = false;
			}
		}
	}
}
