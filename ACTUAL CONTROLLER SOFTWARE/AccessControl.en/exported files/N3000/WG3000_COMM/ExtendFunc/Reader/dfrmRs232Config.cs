using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Reader
{
	// Token: 0x02000323 RID: 803
	public partial class dfrmRs232Config : frmN3000
	{
		// Token: 0x06001918 RID: 6424 RVA: 0x0020D030 File Offset: 0x0020C030
		public dfrmRs232Config()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0020D03E File Offset: 0x0020C03E
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0020D050 File Offset: 0x0020C050
		private void btnOK_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			dfrmRs232Config.bsm4pwd = false;
			int num3 = 0;
			int num4 = 0;
			if (this.optDisableRS232_1.Checked)
			{
				num3 = 0;
				num4 = 0;
				num = 0;
			}
			else
			{
				if (this.optEnabledRXToUDP.Checked)
				{
					num3 |= 129;
					num = 1;
				}
				else if (this.optEnableSFZDirectOpen.Checked)
				{
					num3 |= 138;
					num = 2;
				}
				else if (this.optEnableSFZ.Checked)
				{
					num3 |= 136;
					num = 3;
				}
				else if (this.optEnableCardInput_8Byte.Checked)
				{
					num3 |= 130;
					num = 4;
				}
				else if (this.optEnableCardInput.Checked)
				{
					num3 |= 130;
					num3 |= 132;
					num = 5;
				}
				else if (this.optEnableCardInput_32ByteCustomPwd.Checked)
				{
					num3 |= 130;
					num3 |= 132;
					num = 6;
				}
				else if (this.optEnableSFZWithICCard.Checked)
				{
					num3 |= 152;
					num = 7;
				}
				if (this.optDoor1In.Checked)
				{
					num2 = 0;
				}
				else if (this.optDoor1Out.Checked)
				{
					num3 |= 64;
					num2 = 1;
				}
				else if (this.optDoor2In.Checked)
				{
					num3 |= 32;
					num2 = 2;
				}
				else if (this.optDoor2Out.Checked)
				{
					num3 |= 32;
					num3 |= 64;
					num2 = 3;
				}
				if (this.chkExtern1.Checked)
				{
					num4 |= 1;
				}
				if (this.chkExtern2.Checked)
				{
					num4 |= 2;
				}
				if (this.chkExtern3.Checked)
				{
					num4 |= 4;
				}
				if (this.chkExtern4.Checked)
				{
					num4 |= 8;
				}
				if (this.chkExtern5.Checked)
				{
					num4 |= 16;
				}
				if (this.chkExtern6.Checked)
				{
					num4 |= 64;
				}
			}
			dfrmRs232Config.controlConfigure.rs232_1_option = num3;
			dfrmRs232Config.controlConfigure.rs232_1_extern = num4;
			this.paraUart1 = num;
			this.paraReader1 = num2;
			num3 = 0;
			num4 = 0;
			if (this.optDisableRS232_2.Checked)
			{
				num3 = 0;
				num4 = 0;
				num = 0;
			}
			else
			{
				if (this.optEnabledRXToUDP_2.Checked)
				{
					num3 |= 129;
					num = 1;
				}
				else if (this.optEnableSFZDirectOpen_2.Checked)
				{
					num3 |= 138;
					num = 2;
				}
				else if (this.optEnableSFZ_2.Checked)
				{
					num3 |= 136;
					num = 3;
				}
				else if (this.optEnableCardInput_8Byte_2.Checked)
				{
					num3 |= 130;
					num = 4;
				}
				else if (this.optEnableCardInput_2.Checked)
				{
					num3 |= 130;
					num3 |= 132;
					num = 5;
				}
				else if (this.optEnableCardInput_32ByteCustomPwd_2.Checked)
				{
					num3 |= 130;
					num3 |= 132;
					num = 6;
				}
				else if (this.optEnableSFZWithICCard_2.Checked)
				{
					num3 |= 152;
					num = 7;
				}
				if (this.optDoor1In_2.Checked)
				{
					num2 = 0;
				}
				else if (this.optDoor1Out_2.Checked)
				{
					num3 |= 64;
					num2 = 1;
				}
				else if (this.optDoor2In_2.Checked)
				{
					num3 |= 32;
					num2 = 2;
				}
				else if (this.optDoor2Out_2.Checked)
				{
					num3 |= 32;
					num3 |= 64;
					num2 = 3;
				}
				if (this.chkExtern1_2.Checked)
				{
					num4 |= 1;
				}
				if (this.chkExtern2_2.Checked)
				{
					num4 |= 2;
				}
				if (this.chkExtern3_2.Checked)
				{
					num4 |= 4;
				}
				if (this.chkExtern4_2.Checked)
				{
					num4 |= 8;
				}
				if (this.chkExtern5_2.Checked)
				{
					num4 |= 16;
				}
				if (this.chkExtern6_2.Checked)
				{
					num4 |= 64;
				}
			}
			dfrmRs232Config.controlConfigure.rs232_2_option = num3;
			dfrmRs232Config.controlConfigure.rs232_2_extern = num4;
			this.paraUart2 = num;
			this.paraReader2 = num2;
			wgAppConfig.UpdateKeyVal("", "");
			wgAppConfig.UpdateKeyVal("KEY_RS232CONFIG", string.Format("{0},{1},{2},{3},{4},{5}", new object[]
			{
				this.paraUart1,
				this.paraReader1,
				this.paraUart2,
				this.paraReader2,
				dfrmRs232Config.controlConfigure.rs232_1_extern,
				dfrmRs232Config.controlConfigure.rs232_2_extern
			}));
			if (this.optEnableCardInput_32ByteCustomPwd.Checked || this.optEnableCardInput_32ByteCustomPwd_2.Checked)
			{
				dfrmRs232Config.bsm4pwd = true;
				dfrmRs232Config.wgsm4pwd = this.txtCustomPwd.Text;
			}
			dfrmRs232Config.pwd = this.txtCustomPwd.Text;
			base.DialogResult = DialogResult.OK;
			wgAppConfig.wgLog(this.Text + "  " + string.Format("KEY_RS232CONFIG={0}", string.Format("{0},{1},{2},{3}", new object[] { this.paraUart1, this.paraReader1, this.paraUart2, this.paraReader2 })));
			wgAppConfig.wgLog(this.Text + "  " + string.Format("rs232_1_option={0}, rs232_2_option={1}", dfrmRs232Config.controlConfigure.rs232_1_option, dfrmRs232Config.controlConfigure.rs232_2_option));
			wgAppConfig.wgLog(this.Text + "  " + string.Format("rs232_1_extern={0}, rs232_2_extern={1}", dfrmRs232Config.controlConfigure.rs232_1_extern, dfrmRs232Config.controlConfigure.rs232_2_extern));
			base.Close();
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0020D5B4 File Offset: 0x0020C5B4
		private void dfrmReaderConfig_Load(object sender, EventArgs e)
		{
			bool flag = false;
			this.optEnableCardInput_8Byte.Visible = flag;
			this.optEnableCardInput_8Byte_2.Visible = flag;
			this.label5.Visible = flag;
			this.txtCustomPwd.Visible = flag;
			string keyVal = wgAppConfig.GetKeyVal("KEY_RS232CONFIG");
			if (!string.IsNullOrEmpty(keyVal))
			{
				string[] array = keyVal.Split(new char[] { ',' });
				if (array.Length == 4 || array.Length == 6)
				{
					int.TryParse(array[0], out this.paraUart1);
					int.TryParse(array[1], out this.paraReader1);
					int.TryParse(array[2], out this.paraUart2);
					int.TryParse(array[3], out this.paraReader2);
					switch (this.paraUart1)
					{
					case 0:
						this.optDisableRS232_1.Checked = true;
						break;
					case 1:
						this.optEnabledRXToUDP.Checked = true;
						break;
					case 2:
						this.optEnableSFZDirectOpen.Checked = true;
						break;
					case 3:
						this.optEnableSFZ.Checked = true;
						break;
					case 4:
						this.optEnableCardInput_8Byte.Checked = true;
						this.optEnableCardInput_8Byte.Visible = true;
						break;
					case 5:
						this.optEnableCardInput.Checked = true;
						break;
					case 6:
						this.optEnableCardInput_32ByteCustomPwd.Checked = true;
						this.optEnableCardInput_32ByteCustomPwd.Visible = true;
						this.label5.Visible = true;
						this.txtCustomPwd.Visible = true;
						break;
					case 7:
						this.optEnableSFZWithICCard.Checked = true;
						break;
					}
					switch (this.paraReader1)
					{
					case 0:
						this.optDoor1In.Checked = true;
						break;
					case 1:
						this.optDoor1Out.Checked = true;
						break;
					case 2:
						this.optDoor2In.Checked = true;
						break;
					case 3:
						this.optDoor2Out.Checked = true;
						break;
					}
					switch (this.paraUart2)
					{
					case 0:
						this.optDisableRS232_2.Checked = true;
						break;
					case 1:
						this.optEnabledRXToUDP_2.Checked = true;
						break;
					case 2:
						this.optEnableSFZDirectOpen_2.Checked = true;
						break;
					case 3:
						this.optEnableSFZ_2.Checked = true;
						break;
					case 4:
						this.optEnableCardInput_8Byte_2.Checked = true;
						this.optEnableCardInput_8Byte_2.Visible = true;
						break;
					case 5:
						this.optEnableCardInput_2.Checked = true;
						break;
					case 6:
						this.optEnableCardInput_32ByteCustomPwd_2.Checked = true;
						this.optEnableCardInput_32ByteCustomPwd_2.Visible = true;
						this.label5.Visible = true;
						this.txtCustomPwd.Visible = true;
						break;
					case 7:
						this.optEnableSFZWithICCard_2.Checked = true;
						break;
					}
					switch (this.paraReader2)
					{
					case 0:
						this.optDoor1In_2.Checked = true;
						break;
					case 1:
						this.optDoor1Out_2.Checked = true;
						break;
					case 2:
						this.optDoor2In_2.Checked = true;
						break;
					case 3:
						this.optDoor2Out_2.Checked = true;
						break;
					}
					if (array.Length == 6)
					{
						int num = 0;
						int.TryParse(array[4], out num);
						if ((num & 1) > 0)
						{
							this.chkExtern1.Checked = true;
						}
						if ((num & 2) > 0)
						{
							this.chkExtern2.Checked = true;
						}
						if ((num & 4) > 0)
						{
							this.chkExtern3.Checked = true;
						}
						if ((num & 8) > 0)
						{
							this.chkExtern4.Checked = true;
						}
						if ((num & 16) > 0)
						{
							this.chkExtern5.Checked = true;
						}
						if ((num & 64) > 0)
						{
							this.chkExtern6.Checked = true;
						}
						num = 0;
						int.TryParse(array[5], out num);
						if ((num & 1) > 0)
						{
							this.chkExtern1_2.Checked = true;
						}
						if ((num & 2) > 0)
						{
							this.chkExtern2_2.Checked = true;
						}
						if ((num & 4) > 0)
						{
							this.chkExtern3_2.Checked = true;
						}
						if ((num & 8) > 0)
						{
							this.chkExtern4_2.Checked = true;
						}
						if ((num & 16) > 0)
						{
							this.chkExtern5_2.Checked = true;
						}
						if ((num & 64) > 0)
						{
							this.chkExtern6_2.Checked = true;
						}
					}
				}
			}
			this.txtCustomPwd.Text = dfrmRs232Config.pwd;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0020D9D8 File Offset: 0x0020C9D8
		private void dfrmRs232Config_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.Shift && e.KeyValue == 81)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						try
						{
							if (e.Control && e.Shift && e.KeyValue == 81)
							{
								if (icOperator.OperatorID != 1)
								{
									XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								}
								else
								{
									bool flag = true;
									this.optEnableCardInput_8Byte.Visible = flag;
									this.optEnableCardInput_8Byte_2.Visible = flag;
									this.label5.Visible = flag;
									this.txtCustomPwd.Visible = flag;
									this.optEnableCardInput_32ByteCustomPwd.Visible = flag;
									this.optEnableCardInput_32ByteCustomPwd_2.Visible = flag;
								}
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04003343 RID: 13123
		private const int RS232_EXT_ENABLE_CRC = 16;

		// Token: 0x04003344 RID: 13124
		private const int RS232_EXT_SFZNAME = 64;

		// Token: 0x04003345 RID: 13125
		private const int RS232_EXTERN_10_CARDINPUT = 2;

		// Token: 0x04003346 RID: 13126
		private const int RS232_EXTERN_DISCARDTIME = 8;

		// Token: 0x04003347 RID: 13127
		private const int RS232_EXTERN_QR_DIRECTOPEN = 1;

		// Token: 0x04003348 RID: 13128
		private const int RS232_EXTERN_TRUNC_32_DATA = 4;

		// Token: 0x04003349 RID: 13129
		private const int RS232_OPTION_CARD_INPUT = 2;

		// Token: 0x0400334A RID: 13130
		private const int RS232_OPTION_DIRECT_UPLOAD = 1;

		// Token: 0x0400334B RID: 13131
		private const int RS232_OPTION_DOOR = 32;

		// Token: 0x0400334C RID: 13132
		private const int RS232_OPTION_ENABLE = 128;

		// Token: 0x0400334D RID: 13133
		private const int RS232_OPTION_READER_INOUT_ACTIVE = 64;

		// Token: 0x0400334E RID: 13134
		private const int RS232_OPTION_SFZ_DIRECTOPEN = 2;

		// Token: 0x0400334F RID: 13135
		private const int RS232_OPTION_SFZ_ICCARD_INPUT = 16;

		// Token: 0x04003350 RID: 13136
		private const int RS232_OPTION_SFZ_ONLYREADID_ACTIVE = 8;

		// Token: 0x04003351 RID: 13137
		private const int RS232_OPTION_SM4_ACTIVE = 4;

		// Token: 0x04003352 RID: 13138
		private int paraReader1;

		// Token: 0x04003353 RID: 13139
		private int paraReader2;

		// Token: 0x04003354 RID: 13140
		private int paraUart1;

		// Token: 0x04003355 RID: 13141
		private int paraUart2;

		// Token: 0x04003356 RID: 13142
		public static bool bsm4pwd = false;

		// Token: 0x04003357 RID: 13143
		public static wgMjControllerConfigure controlConfigure = new wgMjControllerConfigure();

		// Token: 0x04003358 RID: 13144
		private static string pwd = "";

		// Token: 0x04003359 RID: 13145
		public static string wgsm4pwd = "";
	}
}
