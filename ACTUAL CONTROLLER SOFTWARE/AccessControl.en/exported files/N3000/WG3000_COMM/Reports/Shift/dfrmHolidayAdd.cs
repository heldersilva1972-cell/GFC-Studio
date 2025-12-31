using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000368 RID: 872
	public partial class dfrmHolidayAdd : frmN3000
	{
		// Token: 0x06001C60 RID: 7264 RVA: 0x00256298 File Offset: 0x00255298
		public dfrmHolidayAdd()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x002562BC File Offset: 0x002552BC
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x002562CC File Offset: 0x002552CC
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.txtHolidayName.Text = this.txtHolidayName.Text.Trim();
			if (this.txtHolidayName.Text == "")
			{
				XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			try
			{
				wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					"INSERT INTO [t_a_Holiday] ([f_Name], [f_Value], [f_Value1], [f_Value2],[f_Value3], [f_Type], [f_Note]) Values( ",
					wgTools.PrepareStrNUnicode(this.txtHolidayName.Text),
					", ",
					wgTools.PrepareStr(this.dtpStartDate.Value.ToString("yyyy-MM-dd")),
					", ",
					wgTools.PrepareStrNUnicode(this.cboStart.Text),
					", ",
					wgTools.PrepareStr(this.dtpEndDate.Value.ToString("yyyy-MM-dd")),
					", ",
					wgTools.PrepareStrNUnicode(this.cboEnd.Text),
					", ",
					wgTools.PrepareStrNUnicode(this.holidayType),
					", ",
					wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
					" )"
				}));
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00256454 File Offset: 0x00255454
		private void dfrmLeave_Load(object sender, EventArgs e)
		{
			try
			{
				this.dtpStartDate.Value = DateTime.Now.Date;
				this.dtpEndDate.Value = DateTime.Now.Date;
				this.cboStart.Items.Clear();
				this.cboStart.Items.AddRange(new string[]
				{
					CommonStr.strAM,
					CommonStr.strPM
				});
				this.cboEnd.Items.Clear();
				this.cboEnd.Items.AddRange(new string[]
				{
					CommonStr.strAM,
					CommonStr.strPM
				});
				if (this.cboStart.Items.Count > 0)
				{
					this.cboStart.SelectedIndex = 0;
				}
				if (this.cboEnd.Items.Count > 1)
				{
					this.cboEnd.SelectedIndex = 1;
				}
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00256594 File Offset: 0x00255594
		private void dtpStartDate_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.dtpStartDate.Value > this.dtpEndDate.Value)
				{
					this.cboEnd.Text = CommonStr.strPM;
				}
				this.dtpEndDate.MinDate = this.dtpStartDate.Value;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0400369D RID: 13981
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x0400369E RID: 13982
		public string holidayType = "2";
	}
}
