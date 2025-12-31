using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200000E RID: 14
	public partial class dfrmDateTimeSelect : frmN3000
	{
		// Token: 0x060000AB RID: 171 RVA: 0x0001C638 File Offset: 0x0001B638
		public dfrmDateTimeSelect()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0001C65C File Offset: 0x0001B65C
		private void btnOption_Click(object sender, EventArgs e)
		{
			this.txtMoreTime.Visible = true;
			this.lblMoreTime.Visible = true;
			this.dateBeginHMS1.Visible = false;
			this.btnOption.Enabled = false;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0001C690 File Offset: 0x0001B690
		private bool checkValidAutoTime(string item, string NewVal)
		{
			try
			{
				if (string.IsNullOrEmpty(item))
				{
					return true;
				}
				string text = wgAppConfig.GetKeyVal("AutoUpdateTime");
				string text2 = wgAppConfig.GetKeyVal("AutoGetSwipeRecords");
				string text3 = wgAppConfig.GetKeyVal("AutoUploadPrivileges");
				string text4 = wgAppConfig.GetKeyVal("AutoUploadConfigure");
				string text5 = "";
				if (item != null)
				{
					if (!(item == "AutoUpdateTime"))
					{
						if (!(item == "AutoGetSwipeRecords"))
						{
							if (!(item == "AutoUploadPrivileges"))
							{
								if (item == "AutoUploadConfigure")
								{
									text4 = NewVal;
								}
							}
							else
							{
								text3 = NewVal;
							}
						}
						else
						{
							text2 = NewVal;
						}
					}
					else
					{
						text = NewVal;
					}
				}
				if (text5.IndexOf("[") > 0)
				{
					text5 = text5.Substring(0, text5.IndexOf("["));
				}
				string[] array = new string[] { text, text2, text3, text4 };
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						string[] array2 = array[i].Split(new char[] { ',' });
						for (int j = 0; j < array2.Length; j++)
						{
							DateTime dateTime;
							if (DateTime.TryParse(string.Format("2016-03-31 {0}", array2[j].Trim()), out dateTime))
							{
								arrayList.Add(dateTime);
							}
						}
					}
				}
				arrayList.Sort();
				if (arrayList.Count > 1)
				{
					for (int k = 1; k < arrayList.Count; k++)
					{
						if (arrayList[k - 1] == arrayList[k])
						{
							DateTime dateTime2 = (DateTime)arrayList[k - 1];
							XMessageBox.Show(string.Format(CommonStr.strFailedToSetTaskTimeMoreTask + " {0}", dateTime2.ToString("HH:mm")), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}
						if (((DateTime)arrayList[k - 1]).AddMinutes(10.0) > (DateTime)arrayList[k])
						{
							DateTime dateTime3 = (DateTime)arrayList[k - 1];
							DateTime dateTime4 = (DateTime)arrayList[k];
							XMessageBox.Show(string.Format(CommonStr.strFailedToSetTaskTimeTwoTask + " {0},{1}", dateTime3.ToString("HH:mm"), dateTime4.ToString("HH:mm")), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}
					}
					DateTime dateTime5 = (DateTime)arrayList[arrayList.Count - 1];
					DateTime dateTime6 = (DateTime)arrayList[0];
					if (dateTime5.AddMinutes(10.0) > dateTime6.AddDays(1.0))
					{
						DateTime dateTime7 = (DateTime)arrayList[arrayList.Count - 1];
						DateTime dateTime8 = (DateTime)arrayList[0];
						XMessageBox.Show(string.Format(CommonStr.strFailedToSetTaskTimeTwoTask + " {0},{1}", dateTime7.ToString("HH:mm"), dateTime8.ToString("HH:mm")), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0001CA00 File Offset: 0x0001BA00
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0001CA10 File Offset: 0x0001BA10
		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.timeInfo = "";
			if (this.txtMoreTime.Visible)
			{
				if (!string.IsNullOrEmpty(this.txtMoreTime.Text))
				{
					string[] array = this.txtMoreTime.Text.Split(new char[] { ',' });
					for (int i = 0; i < array.Length; i++)
					{
						if (!string.IsNullOrEmpty(array[i].Trim()))
						{
							DateTime dateTime;
							if (!DateTime.TryParse(string.Format("2016-03-31 {0}", array[i].Trim()), out dateTime))
							{
								XMessageBox.Show(this, CommonStr.strTimeInvalid, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
							if (string.IsNullOrEmpty(this.timeInfo))
							{
								this.timeInfo = dateTime.ToString("HH:mm");
							}
							else
							{
								this.timeInfo = this.timeInfo + "," + dateTime.ToString("HH:mm");
							}
						}
					}
				}
			}
			else
			{
				this.timeInfo = this.dateBeginHMS1.Value.ToString("HH:mm");
			}
			if (this.checkValidAutoTime(this.taskSheuleCommand, this.timeInfo))
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0001CB44 File Offset: 0x0001BB44
		private void dfrmDateTimeSelect_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.txtMoreTime.Visible = true;
				this.lblMoreTime.Visible = true;
				this.dateBeginHMS1.Visible = false;
				this.btnOption.Enabled = false;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0001CBB8 File Offset: 0x0001BBB8
		private void dfrmDateTimeSelect_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.timeInfo))
			{
				string[] array = this.timeInfo.Split(new char[] { ',' });
				if (array.Length > 1)
				{
					this.txtMoreTime.Text = this.timeInfo;
					this.txtMoreTime.Visible = true;
					this.lblMoreTime.Visible = true;
					this.dateBeginHMS1.Visible = false;
					this.btnOption.Enabled = false;
				}
				else
				{
					DateTime now = DateTime.Now;
					string text = now.ToString("yyyy-MM-dd ") + array[0];
					if (DateTime.TryParse(text, out now))
					{
						this.dateBeginHMS1.Value = now;
					}
				}
			}
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMD);
		}

		// Token: 0x040001C8 RID: 456
		public string taskSheuleCommand = "";

		// Token: 0x040001C9 RID: 457
		public string timeInfo = "";
	}
}
