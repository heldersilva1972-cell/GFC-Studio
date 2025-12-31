using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000373 RID: 883
	public partial class dfrmShiftNormalParamSet : frmN3000
	{
		// Token: 0x06001CEA RID: 7402 RVA: 0x002625F8 File Offset: 0x002615F8
		public dfrmShiftNormalParamSet()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00262660 File Offset: 0x00261660
		private void activeSwipeFourTimesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.activeSwipeFourTimesToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.setSystemParamValue(215, "NormalShiftABC", "2", "");
				base.Close();
			}
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x002626C0 File Offset: 0x002616C0
		private void activeSwipeTwiceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.activeSwipeTwiceToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.setSystemParamValue(215, "NormalShiftABC", "1", "");
				base.Close();
			}
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x0026271D File Offset: 0x0026171D
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x00262728 File Offset: 0x00261728
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.nudLateTimeout.Value >= this.nudLateAbsenceTimeout.Value)
			{
				XMessageBox.Show(this, CommonStr.strShiftNormalParamSet1, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.optReadCardTwoTimes.Checked && this.dtpOffduty0.Value <= this.dtpOnduty0.Value)
			{
				XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.optReadCardFourTimes.Checked)
			{
				if (this.dtpOffduty1.Value <= this.dtpOnduty1.Value)
				{
					XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (this.dtpOffduty2.Value <= this.dtpOnduty2.Value)
				{
					XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (this.dtpOffduty2.Value <= this.dtpOnduty1.Value)
				{
					XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			for (int i = 1; i <= 14; i++)
			{
				switch (i)
				{
				case 1:
					this.setAttendanceParam(i, this.nudLateTimeout.Value.ToString());
					break;
				case 2:
					this.setAttendanceParam(i, this.nudLateAbsenceTimeout.Value.ToString());
					break;
				case 3:
					this.setAttendanceParam(i, (this.cboLateAbsenceDay.SelectedIndex / 2m).ToString(CultureInfo.InvariantCulture));
					break;
				case 4:
					this.setAttendanceParam(i, this.nudLeaveTimeout.Value.ToString());
					break;
				case 5:
					this.setAttendanceParam(i, this.nudLeaveAbsenceTimeout.Value.ToString());
					break;
				case 6:
					this.setAttendanceParam(i, (this.cboLeaveAbsenceDay.SelectedIndex / 2m).ToString(CultureInfo.InvariantCulture));
					break;
				case 7:
					this.setAttendanceParam(i, this.nudOvertimeTimeout.Value.ToString());
					break;
				case 8:
					this.setAttendanceParam(i, this.dtpOnduty0.Value.ToString(wgTools.YMDHMSFormat));
					break;
				case 9:
					this.setAttendanceParam(i, this.dtpOffduty0.Value.ToString(wgTools.YMDHMSFormat));
					break;
				case 10:
					this.setAttendanceParam(i, this.dtpOnduty1.Value.ToString(wgTools.YMDHMSFormat));
					break;
				case 11:
					this.setAttendanceParam(i, this.dtpOffduty1.Value.ToString(wgTools.YMDHMSFormat));
					break;
				case 12:
					this.setAttendanceParam(i, this.dtpOnduty2.Value.ToString(wgTools.YMDHMSFormat));
					break;
				case 13:
					this.setAttendanceParam(i, this.dtpOffduty2.Value.ToString(wgTools.YMDHMSFormat));
					break;
				case 14:
					if (this.optReadCardTwoTimes.Checked)
					{
						this.setAttendanceParam(i, "2");
					}
					else
					{
						this.setAttendanceParam(i, "4");
					}
					break;
				}
			}
			this.saveParmExt();
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x00262AC0 File Offset: 0x00261AC0
		private void btnOption_Click(object sender, EventArgs e)
		{
			using (dfrmShiftNormalOption dfrmShiftNormalOption = new dfrmShiftNormalOption())
			{
				dfrmShiftNormalOption.ShowDialog();
			}
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x00262AF8 File Offset: 0x00261AF8
		private void dfrmShiftNormalParamSet_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x06001CF1 RID: 7409 RVA: 0x00262B38 File Offset: 0x00261B38
		private void dfrmShiftNormalParamSet_Load(object sender, EventArgs e)
		{
			this.dtpOnduty0.CustomFormat = "HH:mm";
			this.dtpOnduty0.Format = DateTimePickerFormat.Custom;
			this.dtpOnduty0.Value = DateTime.Parse("08:30:00");
			this.dtpOffduty0.CustomFormat = "HH:mm";
			this.dtpOffduty0.Format = DateTimePickerFormat.Custom;
			this.dtpOffduty0.Value = DateTime.Parse("17:30:00");
			this.dtpOnduty1.CustomFormat = "HH:mm";
			this.dtpOnduty1.Format = DateTimePickerFormat.Custom;
			this.dtpOnduty1.Value = DateTime.Parse("08:30:00");
			this.dtpOffduty1.CustomFormat = "HH:mm";
			this.dtpOffduty1.Format = DateTimePickerFormat.Custom;
			this.dtpOffduty1.Value = DateTime.Parse("12:00:00");
			this.dtpOnduty2.CustomFormat = "HH:mm";
			this.dtpOnduty2.Format = DateTimePickerFormat.Custom;
			this.dtpOnduty2.Value = DateTime.Parse("13:30:00");
			this.dtpOffduty2.CustomFormat = "HH:mm";
			this.dtpOffduty2.Format = DateTimePickerFormat.Custom;
			this.dtpOffduty2.Value = DateTime.Parse("17:30:00");
			this.loadOperatorPrivilege();
			this.getAttendanceParam();
			try
			{
				if (!(wgAppConfig.getSystemParamByNO(55).ToString() == "00:00") && !(wgAppConfig.getSystemParamByNO(55).ToString() == "00:00:00"))
				{
					this.btnOption.Visible = true;
				}
				if (wgAppConfig.getSystemParamByNO(56).ToString() == "1")
				{
					this.btnOption.Visible = true;
				}
				if (wgAppConfig.getSystemParamByNO(57).ToString() == "1")
				{
					this.btnOption.Visible = true;
				}
				if (wgAppConfig.getSystemParamByNO(54).ToString() == "1")
				{
					this.btnOption.Visible = true;
				}
				if (wgAppConfig.getSystemParamByNO(59).ToString() == "1")
				{
					this.btnOption.Visible = true;
				}
			}
			catch
			{
			}
			if (wgAppConfig.getParamValBoolByNO(215))
			{
				this.tabControl1.Visible = true;
				string text;
				string text2;
				string text3;
				wgAppConfig.getSystemParamValue(215, out text, out text2, out text3);
				if (string.IsNullOrEmpty(text3))
				{
					text3 = "#######";
				}
				this.arrNormalabc = text3.Split(new char[] { '#' });
				this.loadGrpExt(0);
				if (int.Parse(wgAppConfig.getSystemParamByNO(215)) == 2)
				{
					this.optReadCardFourTimes.Checked = true;
					this.optReadCardTwoTimes.Visible = false;
					this.grpbFourtimes.Visible = true;
					this.grpbTwoTimes.Visible = false;
				}
				else
				{
					this.optReadCardFourTimes.Visible = false;
					this.optReadCardTwoTimes.Checked = true;
					this.grpbFourtimes.Visible = false;
					this.grpbTwoTimes.Visible = true;
				}
			}
			else
			{
				this.tabControl1.Visible = false;
			}
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.normalShiftABCToolStripMenuItem.Visible = false;
				this.tabControl1.Visible = false;
			}
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00262E54 File Offset: 0x00261E54
		private void disableDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.disableDefaultToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.setSystemParamValue(215, "NormalShiftABC", "0", "");
				base.Close();
			}
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00262EB1 File Offset: 0x00261EB1
		private void funcCtrlShiftQ()
		{
			this.btnOption.Visible = true;
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00262EC0 File Offset: 0x00261EC0
		private void getAttendanceParam()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.getAttendanceParam_Acc();
				return;
			}
			string text = "SELECT * FROM t_a_Attendence";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						switch ((int)sqlDataReader["f_No"])
						{
						case 1:
							this.nudLateTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 2:
							this.nudLateAbsenceTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 3:
							this.cboLateAbsenceDay.SelectedIndex = (int)(decimal.Parse(sqlDataReader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2m);
							break;
						case 4:
							this.nudLeaveTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 5:
							this.nudLeaveAbsenceTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 6:
							this.cboLeaveAbsenceDay.SelectedIndex = (int)(decimal.Parse(sqlDataReader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2m);
							break;
						case 7:
							this.nudOvertimeTimeout.Value = int.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 8:
							this.dtpOnduty0.Value = DateTime.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 9:
							this.dtpOffduty0.Value = DateTime.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 10:
							this.dtpOnduty1.Value = DateTime.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 11:
							this.dtpOffduty1.Value = DateTime.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 12:
							this.dtpOnduty2.Value = DateTime.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 13:
							this.dtpOffduty2.Value = DateTime.Parse((string)sqlDataReader["f_Value"]);
							break;
						case 14:
							if (int.Parse((string)sqlDataReader["f_Value"]) == 4)
							{
								this.optReadCardFourTimes.Checked = true;
								this.grpbFourtimes.Visible = true;
								this.grpbTwoTimes.Visible = false;
							}
							else
							{
								this.optReadCardTwoTimes.Checked = true;
								this.grpbFourtimes.Visible = false;
								this.grpbTwoTimes.Visible = true;
							}
							break;
						}
					}
					sqlDataReader.Close();
				}
			}
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x00263240 File Offset: 0x00262240
		private void getAttendanceParam_Acc()
		{
			string text = "SELECT * FROM t_a_Attendence";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					while (oleDbDataReader.Read())
					{
						switch ((int)oleDbDataReader["f_No"])
						{
						case 1:
							this.nudLateTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 2:
							this.nudLateAbsenceTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 3:
							this.cboLateAbsenceDay.SelectedIndex = (int)(decimal.Parse(oleDbDataReader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2m);
							break;
						case 4:
							this.nudLeaveTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 5:
							this.nudLeaveAbsenceTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 6:
							this.cboLeaveAbsenceDay.SelectedIndex = (int)(decimal.Parse(oleDbDataReader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2m);
							break;
						case 7:
							this.nudOvertimeTimeout.Value = int.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 8:
							this.dtpOnduty0.Value = DateTime.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 9:
							this.dtpOffduty0.Value = DateTime.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 10:
							this.dtpOnduty1.Value = DateTime.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 11:
							this.dtpOffduty1.Value = DateTime.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 12:
							this.dtpOnduty2.Value = DateTime.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 13:
							this.dtpOffduty2.Value = DateTime.Parse((string)oleDbDataReader["f_Value"]);
							break;
						case 14:
							if (int.Parse((string)oleDbDataReader["f_Value"]) == 4)
							{
								this.optReadCardFourTimes.Checked = true;
								this.grpbFourtimes.Visible = true;
								this.grpbTwoTimes.Visible = false;
							}
							else
							{
								this.optReadCardTwoTimes.Checked = true;
								this.grpbFourtimes.Visible = false;
								this.grpbTwoTimes.Visible = true;
							}
							break;
						}
					}
					oleDbDataReader.Close();
				}
			}
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x002635B0 File Offset: 0x002625B0
		private void loadGrpExt(int normalshiftId)
		{
			this.lastTabIndex = normalshiftId;
			if (this.arrNormalabc.Length == 0)
			{
				this.arrNormalabc = new string[] { "", "", "", "", "", "", "", "" };
			}
			string text = this.arrNormalabc[normalshiftId];
			if (string.IsNullOrEmpty(text))
			{
				this.dtpOnduty0.CustomFormat = "HH:mm";
				this.dtpOnduty0.Format = DateTimePickerFormat.Custom;
				this.dtpOnduty0.Value = DateTime.Parse("08:30:00");
				this.dtpOffduty0.CustomFormat = "HH:mm";
				this.dtpOffduty0.Format = DateTimePickerFormat.Custom;
				this.dtpOffduty0.Value = DateTime.Parse("17:30:00");
				this.dtpOnduty1.CustomFormat = "HH:mm";
				this.dtpOnduty1.Format = DateTimePickerFormat.Custom;
				this.dtpOnduty1.Value = DateTime.Parse("08:30:00");
				this.dtpOffduty1.CustomFormat = "HH:mm";
				this.dtpOffduty1.Format = DateTimePickerFormat.Custom;
				this.dtpOffduty1.Value = DateTime.Parse("12:00:00");
				this.dtpOnduty2.CustomFormat = "HH:mm";
				this.dtpOnduty2.Format = DateTimePickerFormat.Custom;
				this.dtpOnduty2.Value = DateTime.Parse("13:30:00");
				this.dtpOffduty2.CustomFormat = "HH:mm";
				this.dtpOffduty2.Format = DateTimePickerFormat.Custom;
				this.dtpOffduty2.Value = DateTime.Parse("17:30:00");
				this.optReadCardTwoTimes.Checked = true;
				this.grpbFourtimes.Visible = false;
				this.grpbTwoTimes.Visible = true;
				if (int.Parse(wgAppConfig.getSystemParamByNO(215)) == 2)
				{
					this.optReadCardFourTimes.Checked = true;
					this.optReadCardTwoTimes.Visible = false;
					this.grpbFourtimes.Visible = true;
					this.grpbTwoTimes.Visible = false;
					return;
				}
			}
			else
			{
				string[] array = text.Split(new char[] { ';' });
				this.dtpOnduty0.Value = DateTime.Parse(array[0]);
				this.dtpOffduty0.Value = DateTime.Parse(array[1]);
				this.dtpOnduty1.Value = DateTime.Parse(array[2]);
				this.dtpOffduty1.Value = DateTime.Parse(array[3]);
				this.dtpOnduty2.Value = DateTime.Parse(array[4]);
				this.dtpOffduty2.Value = DateTime.Parse(array[5]);
			}
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00263848 File Offset: 0x00262848
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuShiftNormalConfigure";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnOK.Visible = false;
			}
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00263876 File Offset: 0x00262876
		private void optReadCardFourTimes_CheckedChanged(object sender, EventArgs e)
		{
			this.grpbTwoTimes.Visible = this.optReadCardTwoTimes.Checked;
			this.grpbFourtimes.Visible = this.optReadCardFourTimes.Checked;
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x002638A4 File Offset: 0x002628A4
		private void optReadCardTwoTimes_CheckedChanged(object sender, EventArgs e)
		{
			this.grpbTwoTimes.Visible = this.optReadCardTwoTimes.Checked;
			this.grpbFourtimes.Visible = this.optReadCardFourTimes.Checked;
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x002638D4 File Offset: 0x002628D4
		private void saveParmExt()
		{
			if (wgAppConfig.getParamValBoolByNO(215))
			{
				this.updateParamExt(this.lastTabIndex);
				string text = "";
				for (int i = 0; i < 8; i++)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "#";
					}
					text += this.arrNormalabc[i];
				}
				wgAppConfig.setSystemParamValue(215, "NormalShiftABC", this.optReadCardTwoTimes.Checked ? "1" : "2", text);
			}
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x00263958 File Offset: 0x00262958
		private void setAttendanceParam(int no, string val)
		{
			wgAppConfig.runUpdateSql("UPDATE t_a_Attendence  SET [f_value]=" + wgTools.PrepareStrNUnicode(val) + " WHERE [f_NO]= " + no.ToString());
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x0026397C File Offset: 0x0026297C
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lastTabIndex != this.tabControl1.SelectedIndex)
			{
				this.updateParamExt(this.lastTabIndex);
				this.loadGrpExt(this.tabControl1.SelectedIndex);
			}
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x002639B0 File Offset: 0x002629B0
		private void updateParamExt(int normalshiftId)
		{
			if (this.dtpOffduty0.Value <= this.dtpOnduty0.Value)
			{
				this.dtpOffduty0.Value = this.dtpOnduty0.Value;
			}
			if (this.dtpOffduty1.Value <= this.dtpOnduty1.Value)
			{
				this.dtpOffduty1.Value = this.dtpOnduty1.Value;
			}
			if (this.dtpOffduty2.Value <= this.dtpOnduty2.Value)
			{
				this.dtpOffduty2.Value = this.dtpOnduty2.Value;
			}
			if (this.dtpOffduty2.Value <= this.dtpOnduty1.Value)
			{
				this.dtpOffduty2.Value = this.dtpOnduty1.Value;
			}
			string text = "";
			text = string.Concat(new string[]
			{
				text,
				this.dtpOnduty0.Value.ToString("HH:mm:ss"),
				";",
				this.dtpOffduty0.Value.ToString("HH:mm:ss"),
				";",
				this.dtpOnduty1.Value.ToString("HH:mm:ss"),
				";",
				this.dtpOffduty1.Value.ToString("HH:mm:ss"),
				";",
				this.dtpOnduty2.Value.ToString("HH:mm:ss"),
				";",
				this.dtpOffduty2.Value.ToString("HH:mm:ss"),
				";"
			});
			if (this.optReadCardFourTimes.Checked)
			{
				text += "1";
			}
			else
			{
				text += "0";
			}
			this.arrNormalabc[normalshiftId] = text;
		}

		// Token: 0x04003794 RID: 14228
		private int lastTabIndex;

		// Token: 0x04003795 RID: 14229
		private string[] arrNormalabc = new string[] { "", "", "", "", "", "", "", "" };
	}
}
