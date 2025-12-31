using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000008 RID: 8
	public partial class dfrmControlHolidayAdd : frmN3000
	{
		// Token: 0x0600006A RID: 106 RVA: 0x0000EE1F File Offset: 0x0000DE1F
		public dfrmControlHolidayAdd()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000EE4A File Offset: 0x0000DE4A
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000EE5C File Offset: 0x0000DE5C
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					string.Format("INSERT INTO [{0}] ([f_BeginYMDHMS], [f_EndYMDHMS], [f_Notes], [f_forceWork])", this.tableName),
					" Values(  ",
					wgTools.PrepareStr(DateTime.Parse(this.dtpStartDate.Value.ToString("yyyy-MM-dd ") + this.dateBeginHMS1.Value.ToString("HH:mm")), true, wgTools.YMDHMSFormat),
					", ",
					wgTools.PrepareStr(DateTime.Parse(this.dtpEndDate.Value.ToString("yyyy-MM-dd ") + this.dateEndHMS1.Value.ToString("HH:mm:59")), true, wgTools.YMDHMSFormat),
					", ",
					wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
					", ",
					this.bHoliday ? "0" : "1",
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

		// Token: 0x0600006D RID: 109 RVA: 0x0000EFC0 File Offset: 0x0000DFC0
		private void dfrmLeave_Load(object sender, EventArgs e)
		{
			try
			{
				this.dtpStartDate.Value = DateTime.Now.Date;
				this.dtpEndDate.Value = DateTime.Now.Date;
				this.dateBeginHMS1.CustomFormat = "HH:mm";
				this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
				this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
				this.dateEndHMS1.CustomFormat = "HH:mm";
				this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
				this.dateEndHMS1.Value = DateTime.Parse("23:59:59");
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000F0B0 File Offset: 0x0000E0B0
		private void dtpStartDate_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				this.dtpEndDate.MinDate = this.dtpStartDate.Value;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x040000A1 RID: 161
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040000A2 RID: 162
		public bool bHoliday = true;

		// Token: 0x040000A3 RID: 163
		public string tableName = "t_b_ControlHolidays";
	}
}
