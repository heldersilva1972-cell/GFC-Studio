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
	// Token: 0x02000322 RID: 802
	public partial class dfrmReaderQRConfig : frmN3000
	{
		// Token: 0x06001910 RID: 6416 RVA: 0x0020C4EB File Offset: 0x0020B4EB
		public dfrmReaderQRConfig()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0020C4F9 File Offset: 0x0020B4F9
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0020C508 File Offset: 0x0020B508
		private void btnOK_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			dfrmReaderQRConfig.bsm4pwd = false;
			int num3 = 0;
			int num4 = 0;
			if (this.optDisableRS232_1.Checked)
			{
				num3 = 192;
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
				else if (this.optEnableCardInput.Checked)
				{
					num3 = 0;
					num = 5;
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
			}
			dfrmReaderQRConfig.controlConfigure.wgqr_option = num3;
			dfrmReaderQRConfig.controlConfigure.wgqr_extern = num4;
			this.paraUart1 = num;
			this.paraReader1 = num2;
			wgAppConfig.UpdateKeyVal("", "");
			wgAppConfig.UpdateKeyVal("KEY_WGQRCONFIG", string.Format("{0},{1},{2},{3},{4},{5}", new object[]
			{
				this.paraUart1,
				this.paraReader1,
				this.paraUart2,
				this.paraReader2,
				dfrmReaderQRConfig.controlConfigure.wgqr_extern,
				dfrmReaderQRConfig.controlConfigure.rs232_2_extern
			}));
			dfrmReaderQRConfig.pwd = this.txtCustomPwd.Text;
			base.DialogResult = DialogResult.OK;
			wgAppConfig.wgLog(this.Text + "  " + string.Format("KEY_WGQRCONFIG={0}", string.Format("{0},{1}", new object[] { this.paraUart1, this.paraReader1, this.paraUart2, this.paraReader2 })));
			wgAppConfig.wgLog(this.Text + "  " + string.Format("WGQR_option={0}", dfrmReaderQRConfig.controlConfigure.wgqr_option));
			wgAppConfig.wgLog(this.Text + "  " + string.Format("WGQR_extern={0}", dfrmReaderQRConfig.controlConfigure.wgqr_extern));
			base.Close();
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0020C760 File Offset: 0x0020B760
		private void dfrmReaderConfig_Load(object sender, EventArgs e)
		{
			bool flag = false;
			this.label5.Visible = flag;
			this.txtCustomPwd.Visible = flag;
			string keyVal = wgAppConfig.GetKeyVal("KEY_WGQRCONFIG");
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
					case 5:
						this.optEnableCardInput.Checked = true;
						break;
					case 6:
						this.label5.Visible = true;
						this.txtCustomPwd.Visible = true;
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
					}
				}
			}
			this.txtCustomPwd.Text = dfrmReaderQRConfig.pwd;
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0020C8EC File Offset: 0x0020B8EC
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
									this.label5.Visible = flag;
									this.txtCustomPwd.Visible = flag;
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

		// Token: 0x0400331C RID: 13084
		private const int RS232_EXT_ENABLE_CRC = 16;

		// Token: 0x0400331D RID: 13085
		private const int RS232_EXT_SFZNAME = 64;

		// Token: 0x0400331E RID: 13086
		private const int RS232_EXTERN_10_CARDINPUT = 2;

		// Token: 0x0400331F RID: 13087
		private const int RS232_EXTERN_DISCARDTIME = 8;

		// Token: 0x04003320 RID: 13088
		private const int RS232_EXTERN_QR_DIRECTOPEN = 1;

		// Token: 0x04003321 RID: 13089
		private const int RS232_EXTERN_TRUNC_32_DATA = 4;

		// Token: 0x04003322 RID: 13090
		private const int RS232_OPTION_CARD_INPUT = 2;

		// Token: 0x04003323 RID: 13091
		private const int RS232_OPTION_DIRECT_UPLOAD = 1;

		// Token: 0x04003324 RID: 13092
		private const int RS232_OPTION_DOOR = 32;

		// Token: 0x04003325 RID: 13093
		private const int RS232_OPTION_ENABLE = 128;

		// Token: 0x04003326 RID: 13094
		private const int RS232_OPTION_READER_INOUT_ACTIVE = 64;

		// Token: 0x04003327 RID: 13095
		private const int RS232_OPTION_SFZ_DIRECTOPEN = 2;

		// Token: 0x04003328 RID: 13096
		private const int RS232_OPTION_SFZ_ICCARD_INPUT = 16;

		// Token: 0x04003329 RID: 13097
		private const int RS232_OPTION_SFZ_ONLYREADID_ACTIVE = 8;

		// Token: 0x0400332A RID: 13098
		private const int RS232_OPTION_SM4_ACTIVE = 4;

		// Token: 0x0400332B RID: 13099
		private int paraReader1;

		// Token: 0x0400332C RID: 13100
		private int paraReader2;

		// Token: 0x0400332D RID: 13101
		private int paraUart1;

		// Token: 0x0400332E RID: 13102
		private int paraUart2;

		// Token: 0x0400332F RID: 13103
		public static bool bsm4pwd = false;

		// Token: 0x04003330 RID: 13104
		public static wgMjControllerConfigure controlConfigure = new wgMjControllerConfigure();

		// Token: 0x04003331 RID: 13105
		private static string pwd = "";

		// Token: 0x04003332 RID: 13106
		public static string wgsm4pwd = "";
	}
}
