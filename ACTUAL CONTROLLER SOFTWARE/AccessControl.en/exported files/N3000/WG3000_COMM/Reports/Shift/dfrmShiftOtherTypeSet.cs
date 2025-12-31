using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000375 RID: 885
	public partial class dfrmShiftOtherTypeSet : frmN3000
	{
		// Token: 0x06001D0C RID: 7436 RVA: 0x00265E8E File Offset: 0x00264E8E
		public dfrmShiftOtherTypeSet()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00265EA8 File Offset: 0x00264EA8
		private void cbof_Readtimes_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = this.chkBOvertimeShift1;
			this.groupBox1.Visible = false;
			this.groupBox2.Visible = false;
			this.groupBox3.Visible = false;
			this.groupBox4.Visible = false;
			this.chkBOvertimeShift1.Visible = false;
			this.chkBOvertimeShift2.Visible = false;
			this.chkBOvertimeShift3.Visible = false;
			this.chkBOvertimeShift4.Visible = false;
			if (!string.IsNullOrEmpty(this.cbof_Readtimes.Text))
			{
				if (int.Parse(this.cbof_Readtimes.Text) >= 2)
				{
					this.groupBox1.Visible = true;
					checkBox = this.chkBOvertimeShift1;
				}
				if (int.Parse(this.cbof_Readtimes.Text) >= 4)
				{
					this.groupBox2.Visible = true;
					checkBox = this.chkBOvertimeShift2;
				}
				if (int.Parse(this.cbof_Readtimes.Text) >= 6)
				{
					this.groupBox3.Visible = true;
					checkBox = this.chkBOvertimeShift3;
				}
				if (int.Parse(this.cbof_Readtimes.Text) >= 8)
				{
					this.groupBox4.Visible = true;
					checkBox = this.chkBOvertimeShift4;
				}
			}
			checkBox.Visible = true;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x00265FD0 File Offset: 0x00264FD0
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00265FD8 File Offset: 0x00264FD8
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.cmdOK_Click_Acc(sender, e);
				return;
			}
			int num = 0;
			if (this.chkBOvertimeShift.Checked)
			{
				num = 1;
			}
			else if (this.chkBOvertimeShift1.Visible)
			{
				num = (this.chkBOvertimeShift1.Checked ? 2 : 0);
			}
			else if (this.chkBOvertimeShift2.Visible)
			{
				num = (this.chkBOvertimeShift2.Checked ? 2 : 0);
			}
			else if (this.chkBOvertimeShift3.Visible)
			{
				num = (this.chkBOvertimeShift3.Checked ? 2 : 0);
			}
			else if (this.chkBOvertimeShift4.Visible)
			{
				num = (this.chkBOvertimeShift4.Checked ? 2 : 0);
			}
			if (this.operateMode == "New")
			{
				using (comShift comShift = new comShift())
				{
					int num2 = comShift.shift_add(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, num);
					if (num2 != 0)
					{
						XMessageBox.Show(this, comShift.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						base.DialogResult = DialogResult.OK;
						base.Close();
					}
					return;
				}
			}
			using (comShift comShift2 = new comShift())
			{
				int num2 = comShift2.shift_update(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, num);
				if (num2 != 0)
				{
					XMessageBox.Show(this, comShift2.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x00266280 File Offset: 0x00265280
		private void cmdOK_Click_Acc(object sender, EventArgs e)
		{
			int num = 0;
			if (this.chkBOvertimeShift.Checked)
			{
				num = 1;
			}
			else if (this.chkBOvertimeShift1.Visible)
			{
				num = (this.chkBOvertimeShift1.Checked ? 2 : 0);
			}
			else if (this.chkBOvertimeShift2.Visible)
			{
				num = (this.chkBOvertimeShift2.Checked ? 2 : 0);
			}
			else if (this.chkBOvertimeShift3.Visible)
			{
				num = (this.chkBOvertimeShift3.Checked ? 2 : 0);
			}
			else if (this.chkBOvertimeShift4.Visible)
			{
				num = (this.chkBOvertimeShift4.Checked ? 2 : 0);
			}
			if (this.operateMode == "New")
			{
				using (comShift_Acc comShift_Acc = new comShift_Acc())
				{
					int num2 = comShift_Acc.shift_add(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, num);
					if (num2 != 0)
					{
						XMessageBox.Show(this, comShift_Acc.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						base.DialogResult = DialogResult.OK;
						base.Close();
					}
					return;
				}
			}
			using (comShift_Acc comShift_Acc2 = new comShift_Acc())
			{
				int num2 = comShift_Acc2.shift_update(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, num);
				if (num2 != 0)
				{
					XMessageBox.Show(this, comShift_Acc2.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x00266518 File Offset: 0x00265518
		private void dfrmShiftOtherTypeSet_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmShiftOtherTypeSet_Load_Acc(sender, e);
				return;
			}
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS2.CustomFormat = "HH:mm";
			this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS2.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS2.CustomFormat = "HH:mm";
			this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS2.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS3.CustomFormat = "HH:mm";
			this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS3.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS3.CustomFormat = "HH:mm";
			this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS3.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS4.CustomFormat = "HH:mm";
			this.dateBeginHMS4.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS4.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS4.CustomFormat = "HH:mm";
			this.dateEndHMS4.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS4.Value = DateTime.Parse("00:00:00");
			this.cbof_ShiftID.Items.Clear();
			for (int i = 1; i <= 99; i++)
			{
				this.cbof_ShiftID.Items.Add(i);
			}
			this.cbof_Readtimes.Items.Clear();
			this.cbof_Readtimes.Items.Add(2);
			this.cbof_Readtimes.Items.Add(4);
			this.cbof_Readtimes.Items.Add(6);
			this.cbof_Readtimes.Items.Add(8);
			if (this.operateMode == "New")
			{
				this.cbof_ShiftID.Enabled = true;
				string text = "SELECT f_ShiftID FROM t_b_ShiftSet  ORDER BY [f_ShiftID] ASC ";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							int num = this.cbof_ShiftID.Items.IndexOf((int)sqlDataReader[0]);
							if (num >= 0)
							{
								this.cbof_ShiftID.Items.RemoveAt(num);
							}
						}
						sqlDataReader.Close();
					}
				}
				if (this.cbof_ShiftID.Items.Count == 0)
				{
					base.Close();
					return;
				}
				this.cbof_ShiftID.Text = this.cbof_ShiftID.Items[0].ToString();
				this.curShiftID = int.Parse(this.cbof_ShiftID.Text);
				this.cbof_Readtimes.Text = this.cbof_Readtimes.Items[0].ToString();
			}
			else
			{
				this.cbof_ShiftID.Enabled = false;
				this.cbof_ShiftID.Text = this.curShiftID.ToString();
				string text = " SELECT * FROM t_b_ShiftSet WHERE [f_ShiftID]= " + this.curShiftID.ToString();
				using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
					{
						sqlConnection2.Open();
						SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader();
						if (sqlDataReader2.Read())
						{
							DateTime dateTime = DateTime.Parse("2010-3-10 00:00:00");
							if (DateTime.TryParse(sqlDataReader2["f_OnDuty1"].ToString(), out dateTime))
							{
								this.dateBeginHMS1.Value = dateTime;
							}
							if (DateTime.TryParse(sqlDataReader2["f_OnDuty2"].ToString(), out dateTime))
							{
								this.dateBeginHMS2.Value = dateTime;
							}
							if (DateTime.TryParse(sqlDataReader2["f_OnDuty3"].ToString(), out dateTime))
							{
								this.dateBeginHMS3.Value = dateTime;
							}
							if (DateTime.TryParse(sqlDataReader2["f_OnDuty4"].ToString(), out dateTime))
							{
								this.dateBeginHMS4.Value = dateTime;
							}
							if (DateTime.TryParse(sqlDataReader2["f_OffDuty1"].ToString(), out dateTime))
							{
								this.dateEndHMS1.Value = dateTime;
							}
							if (DateTime.TryParse(sqlDataReader2["f_OffDuty2"].ToString(), out dateTime))
							{
								this.dateEndHMS2.Value = dateTime;
							}
							if (DateTime.TryParse(sqlDataReader2["f_OffDuty3"].ToString(), out dateTime))
							{
								this.dateEndHMS3.Value = dateTime;
							}
							if (DateTime.TryParse(sqlDataReader2["f_OffDuty4"].ToString(), out dateTime))
							{
								this.dateEndHMS4.Value = dateTime;
							}
							this.cbof_Readtimes.Text = sqlDataReader2["f_Readtimes"].ToString();
							this.txtName.Text = wgTools.SetObjToStr(sqlDataReader2["f_ShiftName"].ToString());
							this.chkBOvertimeShift.Checked = int.Parse(sqlDataReader2["f_bOvertimeShift"].ToString()) == 1;
							this.chkBOvertimeShift1.Checked = int.Parse(sqlDataReader2["f_bOvertimeShift"].ToString()) == 2;
							this.chkBOvertimeShift2.Checked = int.Parse(sqlDataReader2["f_bOvertimeShift"].ToString()) == 2;
							this.chkBOvertimeShift3.Checked = int.Parse(sqlDataReader2["f_bOvertimeShift"].ToString()) == 2;
							this.chkBOvertimeShift4.Checked = int.Parse(sqlDataReader2["f_bOvertimeShift"].ToString()) == 2;
						}
						sqlDataReader2.Close();
					}
				}
			}
			this.cbof_Readtimes_SelectedIndexChanged(null, null);
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x00266BB0 File Offset: 0x00265BB0
		private void dfrmShiftOtherTypeSet_Load_Acc(object sender, EventArgs e)
		{
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS2.CustomFormat = "HH:mm";
			this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS2.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS2.CustomFormat = "HH:mm";
			this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS2.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS3.CustomFormat = "HH:mm";
			this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS3.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS3.CustomFormat = "HH:mm";
			this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS3.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS4.CustomFormat = "HH:mm";
			this.dateBeginHMS4.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS4.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS4.CustomFormat = "HH:mm";
			this.dateEndHMS4.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS4.Value = DateTime.Parse("00:00:00");
			this.cbof_ShiftID.Items.Clear();
			for (int i = 1; i <= 99; i++)
			{
				this.cbof_ShiftID.Items.Add(i);
			}
			this.cbof_Readtimes.Items.Clear();
			this.cbof_Readtimes.Items.Add(2);
			this.cbof_Readtimes.Items.Add(4);
			this.cbof_Readtimes.Items.Add(6);
			this.cbof_Readtimes.Items.Add(8);
			if (this.operateMode == "New")
			{
				this.cbof_ShiftID.Enabled = true;
				string text = "SELECT f_ShiftID FROM t_b_ShiftSet  ORDER BY [f_ShiftID] ASC ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							int num = this.cbof_ShiftID.Items.IndexOf((int)oleDbDataReader[0]);
							if (num >= 0)
							{
								this.cbof_ShiftID.Items.RemoveAt(num);
							}
						}
						oleDbDataReader.Close();
					}
				}
				if (this.cbof_ShiftID.Items.Count == 0)
				{
					base.Close();
					return;
				}
				this.cbof_ShiftID.Text = this.cbof_ShiftID.Items[0].ToString();
				this.curShiftID = int.Parse(this.cbof_ShiftID.Text);
				this.cbof_Readtimes.Text = this.cbof_Readtimes.Items[0].ToString();
			}
			else
			{
				this.cbof_ShiftID.Enabled = false;
				this.cbof_ShiftID.Text = this.curShiftID.ToString();
				string text = " SELECT * FROM t_b_ShiftSet WHERE [f_ShiftID]= " + this.curShiftID.ToString();
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
					{
						oleDbConnection2.Open();
						OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
						if (oleDbDataReader2.Read())
						{
							DateTime dateTime = DateTime.Parse("2010-3-10 00:00:00");
							if (DateTime.TryParse(oleDbDataReader2["f_OnDuty1"].ToString(), out dateTime))
							{
								this.dateBeginHMS1.Value = dateTime;
							}
							if (DateTime.TryParse(oleDbDataReader2["f_OnDuty2"].ToString(), out dateTime))
							{
								this.dateBeginHMS2.Value = dateTime;
							}
							if (DateTime.TryParse(oleDbDataReader2["f_OnDuty3"].ToString(), out dateTime))
							{
								this.dateBeginHMS3.Value = dateTime;
							}
							if (DateTime.TryParse(oleDbDataReader2["f_OnDuty4"].ToString(), out dateTime))
							{
								this.dateBeginHMS4.Value = dateTime;
							}
							if (DateTime.TryParse(oleDbDataReader2["f_OffDuty1"].ToString(), out dateTime))
							{
								this.dateEndHMS1.Value = dateTime;
							}
							if (DateTime.TryParse(oleDbDataReader2["f_OffDuty2"].ToString(), out dateTime))
							{
								this.dateEndHMS2.Value = dateTime;
							}
							if (DateTime.TryParse(oleDbDataReader2["f_OffDuty3"].ToString(), out dateTime))
							{
								this.dateEndHMS3.Value = dateTime;
							}
							if (DateTime.TryParse(oleDbDataReader2["f_OffDuty4"].ToString(), out dateTime))
							{
								this.dateEndHMS4.Value = dateTime;
							}
							this.cbof_Readtimes.Text = oleDbDataReader2["f_Readtimes"].ToString();
							this.txtName.Text = wgTools.SetObjToStr(oleDbDataReader2["f_ShiftName"].ToString());
							this.chkBOvertimeShift.Checked = int.Parse(oleDbDataReader2["f_bOvertimeShift"].ToString()) == 1;
							this.chkBOvertimeShift1.Checked = int.Parse(oleDbDataReader2["f_bOvertimeShift"].ToString()) == 2;
							this.chkBOvertimeShift2.Checked = int.Parse(oleDbDataReader2["f_bOvertimeShift"].ToString()) == 2;
							this.chkBOvertimeShift3.Checked = int.Parse(oleDbDataReader2["f_bOvertimeShift"].ToString()) == 2;
							this.chkBOvertimeShift4.Checked = int.Parse(oleDbDataReader2["f_bOvertimeShift"].ToString()) == 2;
						}
						oleDbDataReader2.Close();
					}
				}
			}
			this.cbof_Readtimes_SelectedIndexChanged(null, null);
		}

		// Token: 0x040037E0 RID: 14304
		public int curShiftID;

		// Token: 0x040037E1 RID: 14305
		public string operateMode = "";
	}
}
