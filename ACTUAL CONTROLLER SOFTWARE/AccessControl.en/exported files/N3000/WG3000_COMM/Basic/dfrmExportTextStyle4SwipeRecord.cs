using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000012 RID: 18
	public partial class dfrmExportTextStyle4SwipeRecord : frmN3000
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00020A1A File Offset: 0x0001FA1A
		public dfrmExportTextStyle4SwipeRecord()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00020A54 File Offset: 0x0001FA54
		private void add(string Field, bool bFixed, string Info, dfrmExportTextStyle4SwipeRecord.fieldType typeA, string dispValue)
		{
			this.addWithMoreOption(Field, bFixed, Info, typeA, dispValue, true);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00020A64 File Offset: 0x0001FA64
		private void addWithMoreOption(string Field, bool bFixed, string Info, dfrmExportTextStyle4SwipeRecord.fieldType typeA, string dispValue, bool bNotLoad)
		{
			this.arrFields.Add(Field);
			this.arrFixedLength.Add(bFixed);
			this.arrSpecialSet.Add(Info);
			string text = dispValue;
			string text2;
			if (string.IsNullOrEmpty(Field))
			{
				text2 = Info;
				text += ((!string.IsNullOrEmpty(Info) && bNotLoad) ? ("  [" + Info + "]") : "");
			}
			else
			{
				text2 = this.dr[Field].ToString();
			}
			if (bFixed)
			{
				int num = int.Parse(Info);
				if (dfrmExportTextStyle4SwipeRecord.fieldType.PadLeft == typeA)
				{
					text2 = text2.Trim().PadLeft(num, '0');
					text2 = text2.Substring(text2.Length - num, num);
				}
				else
				{
					text2 = text2.PadRight(num, ' ').Substring(0, num);
				}
				text += ((!string.IsNullOrEmpty(Info) && bNotLoad) ? ("  [" + Info + "]") : "");
			}
			else
			{
				if (dfrmExportTextStyle4SwipeRecord.fieldType.DateTime == typeA)
				{
					if (!string.IsNullOrEmpty(Info))
					{
						text2 = DateTime.Parse(text2).ToString(Info);
					}
					text += ((!string.IsNullOrEmpty(Info) && bNotLoad) ? ("  [" + Info + "]") : "");
				}
				if (dfrmExportTextStyle4SwipeRecord.fieldType.Date == typeA)
				{
					if (!string.IsNullOrEmpty(Info))
					{
						DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
						dateTimeFormatInfo.ShortDatePattern = Info;
						text2 = DateTime.Parse(text2).ToString(Info, dateTimeFormatInfo);
					}
					text += ((!string.IsNullOrEmpty(Info) && bNotLoad) ? ("  [" + Info + "]") : "");
				}
			}
			this.lstSelected.Items.Add(text);
			this.ExportTextStyle += string.Format("{0}'{1}'{2}'{3}'{4}\"", new object[]
			{
				wgTools.SetObjToStr(Field),
				bFixed.ToString(),
				Info,
				typeA.ToString(),
				text
			});
			this.txtExport.Text = this.txtExport.Text + text2;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00020C7E File Offset: 0x0001FC7E
		private void btnAddAddr_Click(object sender, EventArgs e)
		{
			this.add("f_ReaderName", this.chkFixedLenthOfAddr.Checked, this.cboAddr.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.PadRight, CommonStr.strExportTextStyleAddr);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00020CAC File Offset: 0x0001FCAC
		private void btnAddCardNO_Click(object sender, EventArgs e)
		{
			this.add("f_CardNO", this.chkFixedLenthOfCardNO.Checked, this.cboCardNO.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.PadLeft, CommonStr.strExportTextStyleCardNO);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00020CDC File Offset: 0x0001FCDC
		private void btnAddDate_Click(object sender, EventArgs e)
		{
			string text = this.cboDateFormat.Text.ToString();
			if (!string.IsNullOrEmpty(this.cboDateSeparator.Text.ToString()))
			{
				string text2 = this.cboDateSeparator.Text.ToString();
				if (this.cboDateSeparator.SelectedIndex == this.cboDateSeparator.Items.Count - 1)
				{
					text2 = " ";
				}
				string text3 = text;
				if (text3 != null)
				{
					if (!(text3 == "yyyyMMdd"))
					{
						if (text3 == "MMddyyyy")
						{
							text = string.Format("MM{0}dd{0}yyyy", text2);
						}
						else if (text3 == "ddMMyyyy")
						{
							text = string.Format("dd{0}MM{0}yyyy", text2);
						}
					}
					else
					{
						text = string.Format("yyyy{0}MM{0}dd", text2);
					}
				}
			}
			this.add("f_ReadDate", false, text, dfrmExportTextStyle4SwipeRecord.fieldType.Date, CommonStr.strExportTextStyleDate);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00020DB1 File Offset: 0x0001FDB1
		private void btnAddDepartment_Click(object sender, EventArgs e)
		{
			this.add("f_GroupName", this.chkFixedLenthOfDepartment.Checked, this.cboDepartment.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.PadRight, wgAppConfig.ReplaceFloorRoom(CommonStr.strExportTextStyleDepartment));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00020DE4 File Offset: 0x0001FDE4
		private void btnAddDescription_Click(object sender, EventArgs e)
		{
			this.add("f_Desc", this.chkFixedLenthOfDescription.Checked, this.cboDescription.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.PadRight, CommonStr.strExportTextStyleDescription);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00020E12 File Offset: 0x0001FE12
		private void btnAddRecID_Click(object sender, EventArgs e)
		{
			this.add("f_RecID", this.chkFixedLenthOfRecID.Checked, this.cboRecID.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.PadLeft, CommonStr.strExportTextStyleRecID);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00020E40 File Offset: 0x0001FE40
		private void btnAddSeparator_Click(object sender, EventArgs e)
		{
			if (this.cboSeparator.SelectedIndex == this.cboSeparator.Items.Count - 1)
			{
				this.add("", false, " ", dfrmExportTextStyle4SwipeRecord.fieldType.None, CommonStr.strExportTextStyleSeparator);
				return;
			}
			this.add("", false, this.cboSeparator.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.None, CommonStr.strExportTextStyleSeparator);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00020EA6 File Offset: 0x0001FEA6
		private void btnAddTime_Click(object sender, EventArgs e)
		{
			this.add("f_ReadDate", false, this.cboTimeFormat.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.DateTime, CommonStr.strExportTextStyleTime);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00020ECA File Offset: 0x0001FECA
		private void btnAddUserID_Click(object sender, EventArgs e)
		{
			this.add("f_ConsumerNO", this.chkFixedLenthOfUserID.Checked, this.cboUserID.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.PadLeft, wgAppConfig.ReplaceWorkNO(CommonStr.strExportTextStyleUserID));
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00020EFD File Offset: 0x0001FEFD
		private void btnAddUserName_Click(object sender, EventArgs e)
		{
			this.add("f_ConsumerName", this.chkFixedLenthOfUserName.Checked, this.cboUserName.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.PadRight, CommonStr.strExportTextStyleUserName);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00020F2B File Offset: 0x0001FF2B
		private void btnAddValid_Click(object sender, EventArgs e)
		{
			this.add("f_Character", false, "", dfrmExportTextStyle4SwipeRecord.fieldType.None, CommonStr.strExportTextStyleValid);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00020F44 File Offset: 0x0001FF44
		private void btnAddWeekday_Click(object sender, EventArgs e)
		{
			this.add("f_ReadDate", false, this.cboWeekdayFormat.Text.ToString(), dfrmExportTextStyle4SwipeRecord.fieldType.DateTime, CommonStr.strExportTextStyleWeekday);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00020F68 File Offset: 0x0001FF68
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00020F78 File Offset: 0x0001FF78
		private void btnDeleteAll_Click(object sender, EventArgs e)
		{
			this.lstSelected.Items.Clear();
			this.txtExport.Text = "";
			this.arrFields.Clear();
			this.arrFixedLength.Clear();
			this.arrSpecialSet.Clear();
			this.ExportTextStyle = "";
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00020FD4 File Offset: 0x0001FFD4
		private void btnOK_Click(object sender, EventArgs e)
		{
			wgAppConfig.wgLog("Old KEY_ExportTextStyle:  " + wgAppConfig.GetKeyVal("KEY_ExportTextStyle"));
			wgAppConfig.UpdateKeyVal("KEY_ExportTextStyle", this.ExportTextStyle);
			wgAppConfig.wgLog("New KEY_ExportTextStyle:  " + wgAppConfig.GetKeyVal("KEY_ExportTextStyle"));
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00021030 File Offset: 0x00020030
		private void dfrmExportTextStyle4SwipeRecord_Load(object sender, EventArgs e)
		{
			this.cboCardNO.Text = "10";
			this.cboRecID.Text = "10";
			this.cboUserID.Items.Clear();
			for (int i = 4; i <= 64; i++)
			{
				this.cboUserID.Items.Add(i.ToString());
			}
			this.cboUserID.Text = "10";
			this.cboTimeFormat.Text = "HH:mm:ss";
			this.cboDateFormat.Text = "yyyyMMdd";
			this.cboWeekdayFormat.Text = "ddd";
			this.cboSeparator.Text = ",";
			this.groupBox4.Text = wgAppConfig.ReplaceWorkNO(this.groupBox4.Text);
			this.btnAddUserID.Text = wgAppConfig.ReplaceWorkNO(this.btnAddUserID.Text);
			this.groupBox6.Text = wgAppConfig.ReplaceFloorRoom(this.groupBox6.Text);
			this.btnAddDepartment.Text = wgAppConfig.ReplaceFloorRoom(this.btnAddDepartment.Text);
			string keyVal = wgAppConfig.GetKeyVal("KEY_ExportTextStyle");
			if (!string.IsNullOrEmpty(keyVal))
			{
				string[] array = keyVal.Split(new char[] { '"' });
				if (array.Length > 0)
				{
					for (int j = 0; j < array.Length; j++)
					{
						string[] array2 = array[j].Split(new char[] { '\'' });
						if (array2.Length == 5)
						{
							dfrmExportTextStyle4SwipeRecord.fieldType fieldType = dfrmExportTextStyle4SwipeRecord.fieldType.None;
							string text = array2[3].ToString();
							if (text != null)
							{
								if (!(text == "PadLeft"))
								{
									if (!(text == "PadRight"))
									{
										if (!(text == "DateTime"))
										{
											if (text == "Date")
											{
												fieldType = dfrmExportTextStyle4SwipeRecord.fieldType.Date;
											}
										}
										else
										{
											fieldType = dfrmExportTextStyle4SwipeRecord.fieldType.DateTime;
										}
									}
									else
									{
										fieldType = dfrmExportTextStyle4SwipeRecord.fieldType.PadRight;
									}
								}
								else
								{
									fieldType = dfrmExportTextStyle4SwipeRecord.fieldType.PadLeft;
								}
							}
							this.addWithMoreOption(array2[0], bool.Parse(array2[1]), array2[2], fieldType, array2[4], false);
						}
					}
				}
			}
		}

		// Token: 0x0400021A RID: 538
		private ArrayList arrFields = new ArrayList();

		// Token: 0x0400021B RID: 539
		private ArrayList arrFixedLength = new ArrayList();

		// Token: 0x0400021C RID: 540
		private ArrayList arrSpecialSet = new ArrayList();

		// Token: 0x0400021D RID: 541
		public DataRow dr;

		// Token: 0x0400021E RID: 542
		private string ExportTextStyle = "";

		// Token: 0x02000013 RID: 19
		private enum fieldType
		{
			// Token: 0x0400025F RID: 607
			None,
			// Token: 0x04000260 RID: 608
			PadLeft,
			// Token: 0x04000261 RID: 609
			PadRight,
			// Token: 0x04000262 RID: 610
			DateTime,
			// Token: 0x04000263 RID: 611
			Date
		}
	}
}
