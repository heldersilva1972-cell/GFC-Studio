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

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000245 RID: 581
	public partial class dfrmPeripheralControlBoard : frmN3000
	{
		// Token: 0x06001208 RID: 4616 RVA: 0x00156BD4 File Offset: 0x00155BD4
		public dfrmPeripheralControlBoard()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00156C40 File Offset: 0x00155C40
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00156C48 File Offset: 0x00155C48
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.updateParamExt(this.tabControl1.SelectedIndex);
			this.saveParmExt();
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00156C70 File Offset: 0x00155C70
		private void btnOption_Click(object sender, EventArgs e)
		{
			using (dfrmPeripheralControlBoardSuper dfrmPeripheralControlBoardSuper = new dfrmPeripheralControlBoardSuper())
			{
				dfrmPeripheralControlBoardSuper.extControl = this.ext_controlSet[this.tabControl1.SelectedIndex];
				dfrmPeripheralControlBoardSuper.ext_warnSignalEnabled2 = this.ext_warnSignalEnabled2Set[this.tabControl1.SelectedIndex];
				if (dfrmPeripheralControlBoardSuper.ShowDialog(this) == DialogResult.OK)
				{
					this.ext_controlSet[this.tabControl1.SelectedIndex] = dfrmPeripheralControlBoardSuper.extControl;
					this.ext_warnSignalEnabled2Set[this.tabControl1.SelectedIndex] = dfrmPeripheralControlBoardSuper.ext_warnSignalEnabled2;
				}
			}
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00156D0C File Offset: 0x00155D0C
		private void chkActive_CheckedChanged(object sender, EventArgs e)
		{
			this.grpSet.Visible = this.chkActive.Checked;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00156D24 File Offset: 0x00155D24
		private void dfrmPeripheralControlBoard_Load(object sender, EventArgs e)
		{
			this.txtf_ControllerSN.Text = this.ControllerSN.ToString();
			this.txtf_ControllerNO.Text = this.ControllerNO.ToString();
			this.chkActiveDefault = this.chkActive.Text;
			int num = 0;
			string text = " SELECT b.f_ControllerID, b.f_PeripheralControl ";
			text = text + " FROM t_b_Controller b  WHERE  b.[f_ControllerNO] = " + this.ControllerNO.ToString() + " AND  b.f_Enabled >0 ";
			string text2 = "0";
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							text2 = wgTools.SetObjToStr(oleDbDataReader["f_PeripheralControl"]);
							num = (int)oleDbDataReader[0];
						}
						oleDbDataReader.Close();
					}
					goto IL_014D;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						text2 = wgTools.SetObjToStr(sqlDataReader["f_PeripheralControl"]);
						num = (int)sqlDataReader[0];
					}
					sqlDataReader.Close();
				}
			}
			IL_014D:
			string[] array = text2.Split(new char[] { ',' });
			if (array.Length != 27)
			{
				array = "126,30,30,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,10,10,0,0,0,0".Split(new char[] { ',' });
			}
			int num2 = 0;
			this.ext_AlarmControlMode = int.Parse(array[num2++]);
			this.ext_SetAlarmOnDelay = int.Parse(array[num2++]);
			this.ext_SetAlarmOffDelay = int.Parse(array[num2++]);
			this.ext_doorSet[0] = int.Parse(array[num2++]);
			this.ext_doorSet[1] = int.Parse(array[num2++]);
			this.ext_doorSet[2] = int.Parse(array[num2++]);
			this.ext_doorSet[3] = int.Parse(array[num2++]);
			this.ext_controlSet[0] = int.Parse(array[num2++]);
			this.ext_controlSet[1] = int.Parse(array[num2++]);
			this.ext_controlSet[2] = int.Parse(array[num2++]);
			this.ext_controlSet[3] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabledSet[0] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabledSet[1] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabledSet[2] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabledSet[3] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabled2Set[0] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabled2Set[1] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabled2Set[2] = int.Parse(array[num2++]);
			this.ext_warnSignalEnabled2Set[3] = int.Parse(array[num2++]);
			this.ext_timeoutSet[0] = decimal.Parse(array[num2++], CultureInfo.InvariantCulture);
			this.ext_timeoutSet[1] = decimal.Parse(array[num2++], CultureInfo.InvariantCulture);
			this.ext_timeoutSet[2] = decimal.Parse(array[num2++], CultureInfo.InvariantCulture);
			this.ext_timeoutSet[3] = decimal.Parse(array[num2++], CultureInfo.InvariantCulture);
			this.ext_active[0] = int.Parse(array[num2++]);
			this.ext_active[1] = int.Parse(array[num2++]);
			this.ext_active[2] = int.Parse(array[num2++]);
			this.ext_active[3] = int.Parse(array[num2++]);
			using (icController icController = new icController())
			{
				icController.GetInfoFromDBByControllerID(num);
				switch (wgMjController.GetControllerType(this.ControllerSN))
				{
				case 1:
					this.radioButton10.Text = icController.GetDoorName(1);
					this.radioButton11.Visible = false;
					this.radioButton12.Visible = false;
					this.radioButton13.Visible = false;
					this.radioButton25.Location = this.radioButton11.Location;
					break;
				case 2:
					this.radioButton10.Text = icController.GetDoorName(1);
					this.radioButton11.Text = icController.GetDoorName(2);
					this.radioButton12.Visible = false;
					this.radioButton13.Visible = false;
					this.radioButton25.Location = this.radioButton12.Location;
					break;
				default:
					this.radioButton10.Text = icController.GetDoorName(1);
					this.radioButton11.Text = icController.GetDoorName(2);
					this.radioButton12.Text = icController.GetDoorName(3);
					this.radioButton13.Text = icController.GetDoorName(4);
					break;
				}
			}
			this.tabControl1.SelectedTab = this.tabPage1;
			this.chkActive.Text = this.chkActiveDefault + " " + this.tabControl1.SelectedTab.Text;
			this.updateGrpExt(this.tabControl1.SelectedIndex);
			if (wgAppConfig.IsActivateOpenTooLongWarn)
			{
				this.checkBox85.Text = CommonStr.strOpenTooLongWarn2015;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00157374 File Offset: 0x00156374
		private void radioButton25_CheckedChanged(object sender, EventArgs e)
		{
			this.grpEvent.Visible = !this.radioButton25.Checked;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00157390 File Offset: 0x00156390
		private void saveParmExt()
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.ext_active[i] == 0)
				{
					this.ext_doorSet[i] = 0;
					this.ext_controlSet[i] = 0;
					this.ext_warnSignalEnabledSet[i] = 0;
					this.ext_warnSignalEnabled2Set[i] = 0;
					this.ext_timeoutSet[i] = 0m;
				}
			}
			string text = "";
			text = string.Concat(new string[]
			{
				text,
				this.ext_AlarmControlMode.ToString(),
				",",
				this.ext_SetAlarmOnDelay.ToString(),
				",",
				this.ext_SetAlarmOffDelay.ToString(),
				",",
				this.ext_doorSet[0].ToString(),
				",",
				this.ext_doorSet[1].ToString(),
				",",
				this.ext_doorSet[2].ToString(),
				",",
				this.ext_doorSet[3].ToString(),
				",",
				this.ext_controlSet[0].ToString(),
				",",
				this.ext_controlSet[1].ToString(),
				",",
				this.ext_controlSet[2].ToString(),
				",",
				this.ext_controlSet[3].ToString(),
				",",
				this.ext_warnSignalEnabledSet[0].ToString(),
				",",
				this.ext_warnSignalEnabledSet[1].ToString(),
				",",
				this.ext_warnSignalEnabledSet[2].ToString(),
				",",
				this.ext_warnSignalEnabledSet[3].ToString(),
				",",
				this.ext_warnSignalEnabled2Set[0].ToString(),
				",",
				this.ext_warnSignalEnabled2Set[1].ToString(),
				",",
				this.ext_warnSignalEnabled2Set[2].ToString(),
				",",
				this.ext_warnSignalEnabled2Set[3].ToString(),
				",",
				this.ext_timeoutSet[0].ToString(),
				",",
				this.ext_timeoutSet[1].ToString(),
				",",
				this.ext_timeoutSet[2].ToString(),
				",",
				this.ext_timeoutSet[3].ToString(),
				",",
				this.ext_active[0].ToString(),
				",",
				this.ext_active[1].ToString(),
				",",
				this.ext_active[2].ToString(),
				",",
				this.ext_active[3].ToString()
			});
			wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_PeripheralControl =" + wgTools.PrepareStrNUnicode(text) + "   WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int j = 0; j < 4; j++)
			{
				if ((this.ext_warnSignalEnabledSet[j] & 4) > 0)
				{
					num = 1;
				}
				if ((this.ext_warnSignalEnabledSet[j] & 2) > 0)
				{
					num2 = 1;
				}
				if ((this.ext_warnSignalEnabledSet[j] & 1) > 0)
				{
					num3 = 1;
				}
				if ((this.ext_warnSignalEnabledSet[j] & 16) > 0)
				{
					num4 = 1;
				}
			}
			if (num == 1)
			{
				wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_DoorInvalidOpen =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
			}
			if (num2 == 1)
			{
				wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_DoorOpenTooLong =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
			}
			if (num3 == 1)
			{
				wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_ForceWarn =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
			}
			if (num4 == 1)
			{
				wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_InvalidCardWarn =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
			}
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00157848 File Offset: 0x00156848
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lastTabIndex != this.tabControl1.SelectedIndex)
			{
				this.chkActive.Text = this.chkActiveDefault + " " + this.tabControl1.SelectedTab.Text;
				this.updateParamExt(this.lastTabIndex);
				this.updateGrpExt(this.tabControl1.SelectedIndex);
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x001578B0 File Offset: 0x001568B0
		private void updateGrpExt(int doorNum)
		{
			this.lastTabIndex = doorNum;
			if (this.ext_active[doorNum] <= 0)
			{
				this.chkActive.Checked = false;
				this.grpSet.Visible = false;
				this.radioButton10.Checked = true;
				this.checkBox84.Checked = false;
				this.checkBox85.Checked = false;
				this.checkBox86.Checked = false;
				this.checkBox87.Checked = false;
				this.checkBox88.Checked = false;
				this.checkBox89.Checked = false;
				this.checkBox90.Checked = false;
				this.nudDelay.Value = 10m;
				return;
			}
			this.chkActive.Checked = true;
			this.grpSet.Visible = true;
			int num = this.ext_doorSet[doorNum];
			switch (num)
			{
			case 0:
				this.radioButton10.Checked = true;
				break;
			case 1:
				this.radioButton11.Checked = true;
				break;
			case 2:
				this.radioButton12.Checked = true;
				break;
			case 3:
				this.radioButton13.Checked = true;
				break;
			default:
				if (num == 16)
				{
					this.radioButton25.Checked = true;
				}
				break;
			}
			if (!this.radioButton25.Checked)
			{
				this.grpEvent.Visible = true;
				int num2 = this.ext_warnSignalEnabledSet[doorNum];
				this.checkBox84.Checked = (num2 & 1) > 0;
				this.checkBox85.Checked = (num2 & 2) > 0;
				this.checkBox86.Checked = (num2 & 4) > 0;
				this.checkBox87.Checked = (num2 & 8) > 0;
				this.checkBox88.Checked = (num2 & 16) > 0;
				this.checkBox89.Checked = (num2 & 32) > 0;
				this.checkBox90.Checked = (num2 & 64) > 0;
			}
			else
			{
				this.grpEvent.Visible = false;
			}
			this.nudDelay.Value = this.ext_timeoutSet[doorNum];
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00157AA8 File Offset: 0x00156AA8
		private void updateParamExt(int doorNum)
		{
			if (!this.chkActive.Checked)
			{
				this.ext_active[doorNum] = 0;
				return;
			}
			this.ext_active[doorNum] = 1;
			int num = 0;
			if (this.radioButton10.Checked)
			{
				num = 0;
			}
			if (this.radioButton11.Checked)
			{
				num = 1;
			}
			if (this.radioButton12.Checked)
			{
				num = 2;
			}
			if (this.radioButton13.Checked)
			{
				num = 3;
			}
			if (this.radioButton25.Checked)
			{
				num = 16;
			}
			this.ext_doorSet[doorNum] = num;
			if (this.ext_controlSet[doorNum] == 0)
			{
				this.ext_controlSet[doorNum] = 1;
			}
			if (!this.radioButton25.Checked)
			{
				int num2 = 0;
				if (this.checkBox84.Checked)
				{
					num2 |= 1;
				}
				if (this.checkBox85.Checked)
				{
					num2 |= 2;
				}
				if (this.checkBox86.Checked)
				{
					num2 |= 4;
				}
				if (this.checkBox87.Checked)
				{
					num2 |= 8;
				}
				if (this.checkBox88.Checked)
				{
					num2 |= 16;
				}
				if (this.checkBox89.Checked)
				{
					num2 |= 32;
				}
				if (this.checkBox90.Checked)
				{
					num2 |= 64;
				}
				this.ext_warnSignalEnabledSet[doorNum] = num2;
			}
			this.ext_timeoutSet[doorNum] = this.nudDelay.Value;
		}

		// Token: 0x040020A2 RID: 8354
		private int ext_AlarmControlMode;

		// Token: 0x040020A3 RID: 8355
		private int ext_SetAlarmOffDelay;

		// Token: 0x040020A4 RID: 8356
		private int ext_SetAlarmOnDelay;

		// Token: 0x040020A5 RID: 8357
		private int lastTabIndex;

		// Token: 0x040020A6 RID: 8358
		private string chkActiveDefault = "";

		// Token: 0x040020A7 RID: 8359
		public int ControllerNO;

		// Token: 0x040020A8 RID: 8360
		public int ControllerSN;

		// Token: 0x040020A9 RID: 8361
		private int[] ext_active = new int[4];

		// Token: 0x040020AA RID: 8362
		private int[] ext_controlSet = new int[4];

		// Token: 0x040020AB RID: 8363
		private int[] ext_doorSet = new int[4];

		// Token: 0x040020AC RID: 8364
		private decimal[] ext_timeoutSet = new decimal[4];

		// Token: 0x040020AD RID: 8365
		private int[] ext_warnSignalEnabled2Set = new int[4];

		// Token: 0x040020AE RID: 8366
		private int[] ext_warnSignalEnabledSet = new int[4];
	}
}
