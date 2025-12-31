using System;
using System.Collections;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200000A RID: 10
	public partial class dfrmController : frmN3000
	{
		// Token: 0x0600007C RID: 124 RVA: 0x000108AC File Offset: 0x0000F8AC
		public dfrmController()
		{
			this.InitializeComponent();
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage3.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0001093B File Offset: 0x0000F93B
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00010943 File Offset: 0x0000F943
		private void btnCancel2_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0001094C File Offset: 0x0000F94C
		private void btnNext_Click(object sender, EventArgs e)
		{
			this.mtxtbControllerNO.Text = this.mtxtbControllerNO.Text.Replace(" ", "");
			this.mtxtbControllerSN.Text = this.mtxtbControllerSN.Text.Replace(" ", "");
			int num;
			if (!int.TryParse(this.mtxtbControllerNO.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strIDWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (int.Parse(this.mtxtbControllerNO.Text) > 100000 || int.Parse(this.mtxtbControllerNO.Text) < 0)
			{
				XMessageBox.Show(this, CommonStr.strIDWrong + ", <1000000 , >0", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!int.TryParse(this.mtxtbControllerSN.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text)) == 0)
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!wgMjController.validSN(int.Parse(this.mtxtbControllerSN.Text)))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.optIPLarge.Checked)
			{
				if (string.IsNullOrEmpty(this.txtControllerIP.Text))
				{
					XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				string[] array = this.txtControllerIP.Text.Split(new char[] { '.' });
				IPAddress ipaddress;
				if (!IPAddress.TryParse(this.txtControllerIP.Text, out ipaddress) || array.Length != 4)
				{
					XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (this.m_OperateNew)
			{
				if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), 0))
				{
					XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (icController.IsExisted2NO(int.Parse(this.mtxtbControllerNO.Text), 0))
				{
					XMessageBox.Show(this, CommonStr.strControllerNOAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			else
			{
				if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), this.m_ControllerID))
				{
					XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (icController.IsExisted2NO(int.Parse(this.mtxtbControllerNO.Text), this.m_ControllerID))
				{
					XMessageBox.Show(this, CommonStr.strControllerNOAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (this.optIPLarge.Checked)
			{
				this.txtControllerIP.Text = this.txtControllerIP.Text.Replace(" ", "");
				if (string.IsNullOrEmpty(this.txtControllerIP.Text))
				{
					XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			int controllerType = wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text));
			if (controllerType > 0)
			{
				switch (controllerType)
				{
				case 1:
					this.tabControl1.Controls.Remove(this.tabPage1);
					this.tabPage1.Dispose();
					this.tabControl1.Controls.Remove(this.tabPage2);
					this.tabPage2.Dispose();
					if (int.Parse(this.mtxtbControllerSN.Text) >= 170000000 && int.Parse(this.mtxtbControllerSN.Text) <= 179999999)
					{
						this.label43.Text = CommonStr.strElevatorName;
						this.tabPage3.Text = CommonStr.strElevatorController;
						this.txtReaderName1A.Text = CommonStr.strElevator;
						if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 2)
						{
							this.tabPage3.Text = CommonStr.strElevatorController2;
							this.txtReaderName1A.Text = CommonStr.strElevator2;
						}
						else if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 3)
						{
							this.tabPage3.Text = CommonStr.strElevatorController3;
							this.txtReaderName1A.Text = CommonStr.strElevator3;
						}
						this.label36.Visible = false;
						this.label37.Visible = false;
						this.label38.Visible = false;
						this.label42.Visible = false;
						this.label44.Visible = false;
						this.groupBox22.Visible = false;
						this.nudDoorDelay1A.Visible = false;
						this.txtReaderName2A.Visible = false;
						this.chkAttend2A.Visible = false;
						goto IL_058B;
					}
					goto IL_058B;
				case 2:
					this.tabControl1.Controls.Remove(this.tabPage1);
					this.tabPage1.Dispose();
					this.tabControl1.Controls.Remove(this.tabPage3);
					this.tabPage3.Dispose();
					goto IL_058B;
				case 4:
					this.tabControl1.Controls.Remove(this.tabPage2);
					this.tabPage2.Dispose();
					this.tabControl1.Controls.Remove(this.tabPage3);
					this.tabPage3.Dispose();
					goto IL_058B;
				}
				this.tabControl1.Controls.Remove(this.tabPage2);
				this.tabPage2.Dispose();
				this.tabControl1.Controls.Remove(this.tabPage3);
				this.tabPage3.Dispose();
			}
			IL_058B:
			if (controllerType == 4 && icConsumer.gTimeSecondEnabled)
			{
				XMessageBox.Show(CommonStr.strTimeSecondAddControllerWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.m_OperateNew || wgMjController.GetControllerType(this.m_Controller.ControllerSN) != wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text)))
			{
				this.m_ControllerTypeChanged = true;
				switch (controllerType)
				{
				case 1:
					this.txtDoorName1A.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName1A.Text;
					break;
				case 2:
					this.txtDoorName1B.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName1B.Text;
					this.txtDoorName2B.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName2B.Text;
					break;
				default:
					this.txtDoorName1D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName1D.Text;
					this.txtDoorName2D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName2D.Text;
					this.txtDoorName3D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName3D.Text;
					this.txtDoorName4D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName4D.Text;
					break;
				}
			}
			else
			{
				this.m_ControllerTypeChanged = false;
				switch (controllerType)
				{
				case 1:
					this.chkDoorActive1A.Checked = this.m_Controller.GetDoorActive(1);
					this.optOnline1A.Checked = this.m_Controller.GetDoorControl(1) == 3;
					this.optNO1A.Checked = this.m_Controller.GetDoorControl(1) == 1;
					this.optNC1A.Checked = this.m_Controller.GetDoorControl(1) == 2;
					this.nudDoorDelay1A.Value = this.m_Controller.GetDoorDelay(1);
					this.txtReaderName1A.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(1), this.m_Controller.GetDoorName(1) + "-");
					this.chkAttend1A.Checked = this.m_Controller.GetReaderAsAttendActive(1);
					this.optDutyOnOff1A.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 3;
					this.optDutyOn1A.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 1;
					this.optDutyOff1A.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 2;
					this.txtReaderName2A.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(2), this.m_Controller.GetDoorName(1) + "-");
					this.chkAttend2A.Checked = this.m_Controller.GetReaderAsAttendActive(2);
					this.optDutyOnOff2A.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 3;
					this.optDutyOn2A.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 1;
					this.optDutyOff2A.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 2;
					if (this.m_Controller.ControllerSN == int.Parse(this.mtxtbControllerSN.Text))
					{
						this.txtDoorName1A.Text = this.m_Controller.GetDoorName(1);
					}
					else
					{
						this.txtDoorName1A.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(1), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
					}
					break;
				case 2:
					this.chkDoorActive1B.Checked = this.m_Controller.GetDoorActive(1);
					this.optOnline1B.Checked = this.m_Controller.GetDoorControl(1) == 3;
					this.optNO1B.Checked = this.m_Controller.GetDoorControl(1) == 1;
					this.optNC1B.Checked = this.m_Controller.GetDoorControl(1) == 2;
					this.nudDoorDelay1B.Value = this.m_Controller.GetDoorDelay(1);
					this.txtReaderName1B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(1), this.m_Controller.GetDoorName(1) + "-");
					this.chkAttend1B.Checked = this.m_Controller.GetReaderAsAttendActive(1);
					this.optDutyOnOff1B.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 3;
					this.optDutyOn1B.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 1;
					this.optDutyOff1B.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 2;
					this.txtReaderName2B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(2), this.m_Controller.GetDoorName(1) + "-");
					this.chkAttend2B.Checked = this.m_Controller.GetReaderAsAttendActive(2);
					this.optDutyOnOff2B.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 3;
					this.optDutyOn2B.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 1;
					this.optDutyOff2B.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 2;
					this.chkDoorActive2B.Checked = this.m_Controller.GetDoorActive(2);
					this.optOnline2B.Checked = this.m_Controller.GetDoorControl(2) == 3;
					this.optNO2B.Checked = this.m_Controller.GetDoorControl(2) == 1;
					this.optNC2B.Checked = this.m_Controller.GetDoorControl(2) == 2;
					this.nudDoorDelay2B.Value = this.m_Controller.GetDoorDelay(2);
					this.txtReaderName3B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(3), this.m_Controller.GetDoorName(2) + "-");
					this.chkAttend3B.Checked = this.m_Controller.GetReaderAsAttendActive(3);
					this.optDutyOnOff3B.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 3;
					this.optDutyOn3B.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 1;
					this.optDutyOff3B.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 2;
					this.txtReaderName4B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(4), this.m_Controller.GetDoorName(2) + "-");
					this.chkAttend4B.Checked = this.m_Controller.GetReaderAsAttendActive(4);
					this.optDutyOnOff4B.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 3;
					this.optDutyOn4B.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 1;
					this.optDutyOff4B.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 2;
					if (this.m_Controller.ControllerNO == int.Parse(this.mtxtbControllerNO.Text))
					{
						this.txtDoorName1B.Text = this.m_Controller.GetDoorName(1);
						this.txtDoorName2B.Text = this.m_Controller.GetDoorName(2);
					}
					else
					{
						this.txtDoorName1B.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(1), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
						this.txtDoorName2B.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(2), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
					}
					break;
				default:
					this.chkDoorActive1D.Checked = this.m_Controller.GetDoorActive(1);
					this.optOnline1D.Checked = this.m_Controller.GetDoorControl(1) == 3;
					this.optNO1D.Checked = this.m_Controller.GetDoorControl(1) == 1;
					this.optNC1D.Checked = this.m_Controller.GetDoorControl(1) == 2;
					this.nudDoorDelay1D.Value = this.m_Controller.GetDoorDelay(1);
					this.chkDoorActive2D.Checked = this.m_Controller.GetDoorActive(2);
					this.optOnline2D.Checked = this.m_Controller.GetDoorControl(2) == 3;
					this.optNO2D.Checked = this.m_Controller.GetDoorControl(2) == 1;
					this.optNC2D.Checked = this.m_Controller.GetDoorControl(2) == 2;
					this.nudDoorDelay2D.Value = this.m_Controller.GetDoorDelay(2);
					this.chkDoorActive3D.Checked = this.m_Controller.GetDoorActive(3);
					this.optOnline3D.Checked = this.m_Controller.GetDoorControl(3) == 3;
					this.optNO3D.Checked = this.m_Controller.GetDoorControl(3) == 1;
					this.optNC3D.Checked = this.m_Controller.GetDoorControl(3) == 2;
					this.nudDoorDelay3D.Value = this.m_Controller.GetDoorDelay(3);
					this.chkDoorActive4D.Checked = this.m_Controller.GetDoorActive(4);
					this.optOnline4D.Checked = this.m_Controller.GetDoorControl(4) == 3;
					this.optNO4D.Checked = this.m_Controller.GetDoorControl(4) == 1;
					this.optNC4D.Checked = this.m_Controller.GetDoorControl(4) == 2;
					this.nudDoorDelay4D.Value = this.m_Controller.GetDoorDelay(4);
					this.txtReaderName1D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(1), this.m_Controller.GetDoorName(1) + "-");
					this.chkAttend1D.Checked = this.m_Controller.GetReaderAsAttendActive(1);
					this.optDutyOnOff1D.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 3;
					this.optDutyOn1D.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 1;
					this.optDutyOff1D.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 2;
					this.txtReaderName2D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(2), this.m_Controller.GetDoorName(2) + "-");
					this.chkAttend2D.Checked = this.m_Controller.GetReaderAsAttendActive(2);
					this.optDutyOnOff2D.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 3;
					this.optDutyOn2D.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 1;
					this.optDutyOff2D.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 2;
					this.txtReaderName3D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(3), this.m_Controller.GetDoorName(3) + "-");
					this.chkAttend3D.Checked = this.m_Controller.GetReaderAsAttendActive(3);
					this.optDutyOnOff3D.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 3;
					this.optDutyOn3D.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 1;
					this.optDutyOff3D.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 2;
					this.txtReaderName4D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(4), this.m_Controller.GetDoorName(4) + "-");
					this.chkAttend4D.Checked = this.m_Controller.GetReaderAsAttendActive(4);
					this.optDutyOnOff4D.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 3;
					this.optDutyOn4D.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 1;
					this.optDutyOff4D.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 2;
					if (this.m_Controller.ControllerNO != int.Parse(this.mtxtbControllerNO.Text))
					{
						this.txtDoorName1D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(1), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
						this.txtDoorName2D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(2), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
						this.txtDoorName3D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(3), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
						this.txtDoorName4D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(4), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
					}
					else
					{
						this.txtDoorName1D.Text = this.m_Controller.GetDoorName(1);
						this.txtDoorName2D.Text = this.m_Controller.GetDoorName(2);
						this.txtDoorName3D.Text = this.m_Controller.GetDoorName(3);
						this.txtDoorName4D.Text = this.m_Controller.GetDoorName(4);
					}
					break;
				}
			}
			string text = "Select * from  [t_b_Reader] where NOT (f_DutyOnOff =3)";
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						bool flag = false;
						if (oleDbDataReader.Read())
						{
							flag = true;
						}
						oleDbDataReader.Close();
						if (flag && controllerType > 0)
						{
							switch (controllerType)
							{
							case 1:
								this.label33.Visible = true;
								this.groupBox20.Visible = true;
								this.groupBox14.Visible = true;
								break;
							case 2:
								this.label39.Visible = true;
								this.groupBox16.Visible = true;
								this.groupBox17.Visible = true;
								this.groupBox18.Visible = true;
								this.gpbAttend1B.Visible = true;
								break;
							case 4:
								this.label13.Visible = true;
								this.groupBox8.Visible = true;
								this.groupBox9.Visible = true;
								this.groupBox10.Visible = true;
								this.groupBox12.Visible = true;
								break;
							}
						}
					}
					goto IL_16C2;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					bool flag2 = false;
					if (sqlDataReader.Read())
					{
						flag2 = true;
					}
					sqlDataReader.Close();
					if (flag2 && controllerType > 0)
					{
						switch (controllerType)
						{
						case 1:
							this.label33.Visible = true;
							this.groupBox20.Visible = true;
							this.groupBox14.Visible = true;
							break;
						case 2:
							this.label39.Visible = true;
							this.groupBox16.Visible = true;
							this.groupBox17.Visible = true;
							this.groupBox18.Visible = true;
							this.gpbAttend1B.Visible = true;
							break;
						case 4:
							this.label13.Visible = true;
							this.groupBox8.Visible = true;
							this.groupBox9.Visible = true;
							this.groupBox10.Visible = true;
							this.groupBox12.Visible = true;
							break;
						}
					}
				}
			}
			IL_16C2:
			if (wgMjController.IsFingerController(int.Parse(this.mtxtbControllerSN.Text)))
			{
				this.label43.Text = CommonStr.strFingerName;
				this.tabPage3.Text = CommonStr.strFingerController;
				if (this.txtDoorName1A.Text.Length > 0 && this.txtDoorName1A.Text.IndexOf("m" + this.mtxtbControllerNO.Text.PadLeft(3, '0')) == 0)
				{
					this.txtDoorName1A.Text = "z" + this.txtDoorName1A.Text.Substring(1);
				}
				this.txtReaderName1A.Visible = false;
				this.label35.Visible = false;
				this.label36.Visible = false;
				this.label37.Visible = false;
				this.label38.Visible = false;
				this.label42.Visible = false;
				this.label44.Visible = false;
				this.groupBox22.Visible = false;
				this.nudDoorDelay1A.Visible = false;
				this.txtReaderName2A.Visible = false;
				this.chkAttend1A.Visible = false;
				this.chkAttend2A.Visible = false;
			}
			this.grpbDoorReader.Location = new Point(2, 5);
			base.Size = new Size(base.Size.Width, this.grpbDoorReader.Height + 40);
			base.AcceptButton = this.btnOK;
			this.grpbDoorReader.Visible = true;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00012204 File Offset: 0x00011204
		public void btnOK_Click(object sender, EventArgs e)
		{
			this.mtxtbControllerNO.Text = this.mtxtbControllerNO.Text.Replace(" ", "");
			this.mtxtbControllerSN.Text = this.mtxtbControllerSN.Text.Replace(" ", "");
			this.txtDoorName1A.Text = this.txtDoorName1A.Text.Trim();
			this.txtDoorName1B.Text = this.txtDoorName1B.Text.Trim();
			this.txtDoorName2B.Text = this.txtDoorName2B.Text.Trim();
			this.txtDoorName1D.Text = this.txtDoorName1D.Text.Trim();
			this.txtDoorName2D.Text = this.txtDoorName2D.Text.Trim();
			this.txtDoorName3D.Text = this.txtDoorName3D.Text.Trim();
			this.txtDoorName4D.Text = this.txtDoorName4D.Text.Trim();
			this.txtReaderName1A.Text = this.txtReaderName1A.Text.Trim();
			this.txtReaderName1B.Text = this.txtReaderName1B.Text.Trim();
			this.txtReaderName2B.Text = this.txtReaderName2B.Text.Trim();
			this.txtReaderName1D.Text = this.txtReaderName1D.Text.Trim();
			this.txtReaderName2D.Text = this.txtReaderName2D.Text.Trim();
			this.txtReaderName3D.Text = this.txtReaderName3D.Text.Trim();
			this.txtReaderName4D.Text = this.txtReaderName4D.Text.Trim();
			int num;
			if (!int.TryParse(this.mtxtbControllerNO.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strIDWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (int.Parse(this.mtxtbControllerNO.Text) > 100000 || int.Parse(this.mtxtbControllerNO.Text) < 0)
			{
				XMessageBox.Show(this, CommonStr.strIDWrong + ", <=100000 , >0", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!int.TryParse(this.mtxtbControllerSN.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text)) == 0)
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.optIPLarge.Checked && string.IsNullOrEmpty(this.txtControllerIP.Text))
			{
				XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.m_OperateNew)
			{
				if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), 0))
				{
					XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			else if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), this.m_ControllerID))
			{
				XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text)) == 4 && icConsumer.gTimeSecondEnabled)
			{
				XMessageBox.Show(CommonStr.strTimeSecondAddControllerWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.m_OperateNew && this.m_Controller == null)
			{
				this.m_Controller = new icController();
			}
			icController controller = this.m_Controller;
			controller.ControllerNO = int.Parse(this.mtxtbControllerNO.Text);
			controller.ControllerSN = int.Parse(this.mtxtbControllerSN.Text);
			controller.Note = this.txtNote.Text.ToString();
			controller.Active = this.chkControllerActive.Checked;
			controller.IP = "";
			controller.PORT = 60000;
			if (this.cbof_Zone.SelectedIndex < 0)
			{
				controller.ZoneID = 0;
			}
			else
			{
				controller.ZoneID = (int)this.arrZoneID[this.cbof_Zone.SelectedIndex];
			}
			if (this.optIPLarge.Checked)
			{
				controller.IP = this.txtControllerIP.Text;
				controller.PORT = (int)this.nudPort.Value;
			}
			switch (wgMjController.GetControllerType(controller.ControllerSN))
			{
			case 1:
				controller.SetDoorName(1, this.txtDoorName1A.Text);
				controller.SetDoorActive(1, this.chkDoorActive1A.Checked);
				if (!this.optOnline1A.Checked)
				{
					if (this.optNO1A.Checked)
					{
						controller.SetDoorControl(1, 1);
					}
					else if (this.optNC1A.Checked)
					{
						controller.SetDoorControl(1, 2);
					}
				}
				else
				{
					controller.SetDoorControl(1, 3);
				}
				controller.SetDoorDelay(1, (int)this.nudDoorDelay1A.Value);
				controller.SetReaderName(1, string.Format("{0}-{1}", this.txtDoorName1A.Text, this.txtReaderName1A.Text));
				controller.SetReaderName(2, string.Format("{0}-{1}", this.txtDoorName1A.Text, this.txtReaderName2A.Text));
				controller.SetReaderAsAttendActive(1, this.chkAttend1A.Checked);
				controller.SetReaderAsAttendActive(2, this.chkAttend2A.Checked);
				if (this.optDutyOnOff1A.Checked)
				{
					controller.SetReaderAsAttendControl(1, 3);
				}
				else if (this.optDutyOn1A.Checked)
				{
					controller.SetReaderAsAttendControl(1, 1);
				}
				else if (this.optDutyOff1A.Checked)
				{
					controller.SetReaderAsAttendControl(1, 2);
				}
				if (this.optDutyOnOff2A.Checked)
				{
					controller.SetReaderAsAttendControl(2, 3);
				}
				else if (this.optDutyOn2A.Checked)
				{
					controller.SetReaderAsAttendControl(2, 1);
				}
				else if (this.optDutyOff2A.Checked)
				{
					controller.SetReaderAsAttendControl(2, 2);
				}
				break;
			case 2:
				controller.SetDoorName(1, this.txtDoorName1B.Text);
				controller.SetDoorName(2, this.txtDoorName2B.Text);
				controller.SetDoorActive(1, this.chkDoorActive1B.Checked);
				controller.SetDoorActive(2, this.chkDoorActive2B.Checked);
				if (!this.optOnline1B.Checked)
				{
					if (this.optNO1B.Checked)
					{
						controller.SetDoorControl(1, 1);
					}
					else if (this.optNC1B.Checked)
					{
						controller.SetDoorControl(1, 2);
					}
				}
				else
				{
					controller.SetDoorControl(1, 3);
				}
				if (this.optOnline2B.Checked)
				{
					controller.SetDoorControl(2, 3);
				}
				else if (this.optNO2B.Checked)
				{
					controller.SetDoorControl(2, 1);
				}
				else if (this.optNC2B.Checked)
				{
					controller.SetDoorControl(2, 2);
				}
				controller.SetDoorDelay(1, (int)this.nudDoorDelay1B.Value);
				controller.SetDoorDelay(2, (int)this.nudDoorDelay2B.Value);
				controller.SetReaderName(1, string.Format("{0}-{1}", this.txtDoorName1B.Text, this.txtReaderName1B.Text));
				controller.SetReaderName(2, string.Format("{0}-{1}", this.txtDoorName1B.Text, this.txtReaderName2B.Text));
				controller.SetReaderName(3, string.Format("{0}-{1}", this.txtDoorName2B.Text, this.txtReaderName3B.Text));
				controller.SetReaderName(4, string.Format("{0}-{1}", this.txtDoorName2B.Text, this.txtReaderName4B.Text));
				controller.SetReaderAsAttendActive(1, this.chkAttend1B.Checked);
				controller.SetReaderAsAttendActive(2, this.chkAttend2B.Checked);
				controller.SetReaderAsAttendActive(3, this.chkAttend3B.Checked);
				controller.SetReaderAsAttendActive(4, this.chkAttend4B.Checked);
				if (this.optDutyOnOff1B.Checked)
				{
					controller.SetReaderAsAttendControl(1, 3);
				}
				else if (this.optDutyOn1B.Checked)
				{
					controller.SetReaderAsAttendControl(1, 1);
				}
				else if (this.optDutyOff1B.Checked)
				{
					controller.SetReaderAsAttendControl(1, 2);
				}
				if (this.optDutyOnOff2B.Checked)
				{
					controller.SetReaderAsAttendControl(2, 3);
				}
				else if (this.optDutyOn2B.Checked)
				{
					controller.SetReaderAsAttendControl(2, 1);
				}
				else if (this.optDutyOff2B.Checked)
				{
					controller.SetReaderAsAttendControl(2, 2);
				}
				if (this.optDutyOnOff3B.Checked)
				{
					controller.SetReaderAsAttendControl(3, 3);
				}
				else if (this.optDutyOn3B.Checked)
				{
					controller.SetReaderAsAttendControl(3, 1);
				}
				else if (this.optDutyOff3B.Checked)
				{
					controller.SetReaderAsAttendControl(3, 2);
				}
				if (this.optDutyOnOff4B.Checked)
				{
					controller.SetReaderAsAttendControl(4, 3);
				}
				else if (this.optDutyOn4B.Checked)
				{
					controller.SetReaderAsAttendControl(4, 1);
				}
				else if (this.optDutyOff4B.Checked)
				{
					controller.SetReaderAsAttendControl(4, 2);
				}
				break;
			default:
				controller.SetDoorName(1, this.txtDoorName1D.Text);
				controller.SetDoorName(2, this.txtDoorName2D.Text);
				controller.SetDoorName(3, this.txtDoorName3D.Text);
				controller.SetDoorName(4, this.txtDoorName4D.Text);
				controller.SetDoorActive(1, this.chkDoorActive1D.Checked);
				controller.SetDoorActive(2, this.chkDoorActive2D.Checked);
				controller.SetDoorActive(3, this.chkDoorActive3D.Checked);
				controller.SetDoorActive(4, this.chkDoorActive4D.Checked);
				if (this.optOnline1D.Checked)
				{
					controller.SetDoorControl(1, 3);
				}
				else if (this.optNO1D.Checked)
				{
					controller.SetDoorControl(1, 1);
				}
				else if (this.optNC1D.Checked)
				{
					controller.SetDoorControl(1, 2);
				}
				if (this.optOnline2D.Checked)
				{
					controller.SetDoorControl(2, 3);
				}
				else if (this.optNO2D.Checked)
				{
					controller.SetDoorControl(2, 1);
				}
				else if (this.optNC2D.Checked)
				{
					controller.SetDoorControl(2, 2);
				}
				if (this.optOnline3D.Checked)
				{
					controller.SetDoorControl(3, 3);
				}
				else if (this.optNO3D.Checked)
				{
					controller.SetDoorControl(3, 1);
				}
				else if (this.optNC3D.Checked)
				{
					controller.SetDoorControl(3, 2);
				}
				if (this.optOnline4D.Checked)
				{
					controller.SetDoorControl(4, 3);
				}
				else if (this.optNO4D.Checked)
				{
					controller.SetDoorControl(4, 1);
				}
				else if (this.optNC4D.Checked)
				{
					controller.SetDoorControl(4, 2);
				}
				controller.SetDoorDelay(1, (int)this.nudDoorDelay1D.Value);
				controller.SetDoorDelay(2, (int)this.nudDoorDelay2D.Value);
				controller.SetDoorDelay(3, (int)this.nudDoorDelay3D.Value);
				controller.SetDoorDelay(4, (int)this.nudDoorDelay4D.Value);
				controller.SetReaderName(1, string.Format("{0}-{1}", this.txtDoorName1D.Text, this.txtReaderName1D.Text));
				controller.SetReaderName(2, string.Format("{0}-{1}", this.txtDoorName2D.Text, this.txtReaderName2D.Text));
				controller.SetReaderName(3, string.Format("{0}-{1}", this.txtDoorName3D.Text, this.txtReaderName3D.Text));
				controller.SetReaderName(4, string.Format("{0}-{1}", this.txtDoorName4D.Text, this.txtReaderName4D.Text));
				controller.SetReaderAsAttendActive(1, this.chkAttend1D.Checked);
				controller.SetReaderAsAttendActive(2, this.chkAttend2D.Checked);
				controller.SetReaderAsAttendActive(3, this.chkAttend3D.Checked);
				controller.SetReaderAsAttendActive(4, this.chkAttend4D.Checked);
				if (this.optDutyOnOff1D.Checked)
				{
					controller.SetReaderAsAttendControl(1, 3);
				}
				else if (this.optDutyOn1D.Checked)
				{
					controller.SetReaderAsAttendControl(1, 1);
				}
				else if (this.optDutyOff1D.Checked)
				{
					controller.SetReaderAsAttendControl(1, 2);
				}
				if (this.optDutyOnOff2D.Checked)
				{
					controller.SetReaderAsAttendControl(2, 3);
				}
				else if (this.optDutyOn2D.Checked)
				{
					controller.SetReaderAsAttendControl(2, 1);
				}
				else if (this.optDutyOff2D.Checked)
				{
					controller.SetReaderAsAttendControl(2, 2);
				}
				if (this.optDutyOnOff3D.Checked)
				{
					controller.SetReaderAsAttendControl(3, 3);
				}
				else if (this.optDutyOn3D.Checked)
				{
					controller.SetReaderAsAttendControl(3, 1);
				}
				else if (this.optDutyOff3D.Checked)
				{
					controller.SetReaderAsAttendControl(3, 2);
				}
				if (this.optDutyOnOff4D.Checked)
				{
					controller.SetReaderAsAttendControl(4, 3);
				}
				else if (this.optDutyOn4D.Checked)
				{
					controller.SetReaderAsAttendControl(4, 1);
				}
				else if (this.optDutyOff4D.Checked)
				{
					controller.SetReaderAsAttendControl(4, 2);
				}
				break;
			}
			int num2;
			string text;
			if (this.m_OperateNew)
			{
				num2 = controller.AddIntoDB();
				text = string.Concat(new string[]
				{
					CommonStr.strAddController,
					":(",
					controller.ControllerNO.ToString(),
					")",
					controller.ControllerSN.ToString()
				});
			}
			else
			{
				num2 = controller.UpdateIntoDB(this.m_ControllerTypeChanged);
				text = string.Concat(new string[]
				{
					CommonStr.strUpdateController,
					":(",
					controller.ControllerNO.ToString(),
					")",
					controller.ControllerSN.ToString()
				});
			}
			if (num2 < 0)
			{
				XMessageBox.Show(this, CommonStr.strValWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				wgTools.WgDebugWrite("Controller Ret=" + num2.ToString(), new object[0]);
				return;
			}
			if (sender != null)
			{
				wgAppConfig.wgLog(text);
				base.Close();
				return;
			}
			this.txtDoorName1A.Text = "1" + CommonStr.strDoorNO;
			this.txtDoorName1B.Text = "1" + CommonStr.strDoorNO;
			this.txtDoorName2B.Text = "2" + CommonStr.strDoorNO;
			this.txtDoorName1D.Text = "1" + CommonStr.strDoorNO;
			this.txtDoorName2D.Text = "2" + CommonStr.strDoorNO;
			this.txtDoorName3D.Text = "3" + CommonStr.strDoorNO;
			this.txtDoorName4D.Text = "4" + CommonStr.strDoorNO;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000130A8 File Offset: 0x000120A8
		private void btnZoneManage_Click(object sender, EventArgs e)
		{
			using (frmZones frmZones = new frmZones())
			{
				frmZones.ShowDialog(this);
			}
			this.bEditZone = true;
			this.loadZoneInfo();
			if (this.m_Controller == null)
			{
				if (this.cbof_Zone.Items.Count > 0)
				{
					this.cbof_Zone.SelectedIndex = 0;
					return;
				}
			}
			else if (this.m_Controller.ZoneID > 0)
			{
				if (this.cbof_Zone.Items.Count > 0)
				{
					this.cbof_Zone.SelectedIndex = 0;
				}
				for (int i = 0; i < this.cbof_Zone.Items.Count; i++)
				{
					if ((int)this.arrZoneID[i] == this.m_Controller.ZoneID)
					{
						this.cbof_Zone.SelectedIndex = i;
						return;
					}
				}
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0001318C File Offset: 0x0001218C
		private void cbof_Zone_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_Zone);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00013199 File Offset: 0x00012199
		private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_Zone, this.cbof_Zone.Text);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000131B8 File Offset: 0x000121B8
		private void dfrmController_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				int controllerType = wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text));
				if (controllerType > 0)
				{
					switch (controllerType)
					{
					case 1:
						this.label33.Visible = true;
						this.groupBox20.Visible = true;
						this.groupBox14.Visible = true;
						return;
					case 2:
						this.label39.Visible = true;
						this.groupBox16.Visible = true;
						this.groupBox17.Visible = true;
						this.groupBox18.Visible = true;
						this.gpbAttend1B.Visible = true;
						break;
					case 3:
						break;
					case 4:
						this.label13.Visible = true;
						this.groupBox8.Visible = true;
						this.groupBox9.Visible = true;
						this.groupBox10.Visible = true;
						this.groupBox12.Visible = true;
						return;
					default:
						return;
					}
					return;
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000132DB File Offset: 0x000122DB
		private void dfrmController_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000132E0 File Offset: 0x000122E0
		private void dfrmController_Load(object sender, EventArgs e)
		{
			base.Visible = false;
			this.grpbDoorReader.Visible = false;
			this.loadZoneInfo();
			this.mtxtbControllerNO.Mask = "99990";
			this.mtxtbControllerSN.Mask = "000000000";
			if (this.m_OperateNew)
			{
				this.mtxtbControllerNO.Text = (icController.GetMaxControllerNO() + 1).ToString();
			}
			else
			{
				this.m_Controller = new icController();
				this.m_Controller.GetInfoFromDBByControllerID(this.m_ControllerID);
				this.m_Controller.ControllerID = this.m_ControllerID;
				this.mtxtbControllerNO.Text = this.m_Controller.ControllerNO.ToString();
				this.mtxtbControllerSN.Text = this.m_Controller.ControllerSN.ToString();
				this.txtNote.Text = this.m_Controller.Note.ToString();
				this.chkControllerActive.Checked = this.m_Controller.Active;
				if (this.m_Controller.IP == "")
				{
					this.optIPSmall.Checked = true;
				}
				else
				{
					this.optIPLarge.Checked = true;
					this.txtControllerIP.Text = this.m_Controller.IP;
					this.nudPort.Value = this.m_Controller.PORT;
				}
				if (this.m_Controller.ZoneID > 0)
				{
					if (this.cbof_Zone.Items.Count > 0)
					{
						this.cbof_Zone.SelectedIndex = 0;
					}
					for (int i = 0; i < this.cbof_Zone.Items.Count; i++)
					{
						if ((int)this.arrZoneID[i] == this.m_Controller.ZoneID)
						{
							this.cbof_Zone.SelectedIndex = i;
							break;
						}
					}
				}
			}
			this.grpbIP.Visible = this.optIPLarge.Checked;
			base.Visible = true;
			this.mtxtbControllerSN.Focus();
			this.btnZoneManage.Visible = false;
			bool flag = false;
			string text = "btnZoneManage";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && !flag)
			{
				this.btnZoneManage.Visible = true;
			}
			this.tabPage1.BackColor = this.BackColor;
			this.tabPage2.BackColor = this.BackColor;
			this.tabPage3.BackColor = this.BackColor;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0001354C File Offset: 0x0001254C
		private void loadZoneInfo()
		{
			new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
			int i = this.arrZoneID.Count;
			this.cbof_Zone.Items.Clear();
			for (i = 0; i < this.arrZoneID.Count; i++)
			{
				this.cbof_Zone.Items.Add(this.arrZoneName[i].ToString());
			}
			if (this.cbof_Zone.Items.Count > 0)
			{
				this.cbof_Zone.SelectedIndex = 0;
			}
			bool flag = true;
			this.label25.Visible = flag;
			this.cbof_Zone.Visible = flag;
			this.btnZoneManage.Visible = flag;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0001360F File Offset: 0x0001260F
		private void mtxtbControllerNO_KeyPress(object sender, KeyPressEventArgs e)
		{
			dfrmController.SNInput(ref this.mtxtbControllerNO);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0001361C File Offset: 0x0001261C
		private void mtxtbControllerNO_KeyUp(object sender, KeyEventArgs e)
		{
			dfrmController.SNInput(ref this.mtxtbControllerNO);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00013629 File Offset: 0x00012629
		private void mtxtbControllerSN_KeyPress(object sender, KeyPressEventArgs e)
		{
			dfrmController.SNInput(ref this.mtxtbControllerSN);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00013636 File Offset: 0x00012636
		private void mtxtbControllerSN_KeyUp(object sender, KeyEventArgs e)
		{
			dfrmController.SNInput(ref this.mtxtbControllerSN);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00013643 File Offset: 0x00012643
		private void optIPLarge_CheckedChanged(object sender, EventArgs e)
		{
			this.grpbIP.Visible = this.optIPLarge.Checked;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0001365C File Offset: 0x0001265C
		public static void SNInput(ref MaskedTextBox mtb)
		{
			if (mtb.Text.Length != mtb.Text.Trim().Length)
			{
				mtb.Text = mtb.Text.Trim();
			}
			else if (mtb.Text.Length == 0 && mtb.SelectionStart != 0)
			{
				mtb.SelectionStart = 0;
			}
			if (mtb.Text.Length > 0)
			{
				if (mtb.Text.IndexOf(" ") > 0)
				{
					mtb.Text = mtb.Text.Replace(" ", "");
				}
				if (mtb.Text.Length > 9 && long.Parse(mtb.Text) >= (long)((ulong)(-1)))
				{
					mtb.Text = mtb.Text.Substring(0, mtb.Text.Length - 1);
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0001373E File Offset: 0x0001273E
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00013746 File Offset: 0x00012746
		public int ControllerID
		{
			get
			{
				return this.m_ControllerID;
			}
			set
			{
				this.m_ControllerID = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0001374F File Offset: 0x0001274F
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00013757 File Offset: 0x00012757
		public bool OperateNew
		{
			get
			{
				return this.m_OperateNew;
			}
			set
			{
				this.m_OperateNew = value;
			}
		}

		// Token: 0x040000C8 RID: 200
		private int m_ControllerID;

		// Token: 0x040000C9 RID: 201
		private bool m_ControllerTypeChanged;

		// Token: 0x040000CA RID: 202
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x040000CB RID: 203
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x040000CC RID: 204
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x040000CD RID: 205
		public bool bEditZone;

		// Token: 0x040000CE RID: 206
		private bool m_OperateNew = true;
	}
}
