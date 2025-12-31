using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000366 RID: 870
	public partial class dfrmFaceIDCheckInput : frmN3000
	{
		// Token: 0x06001C4D RID: 7245 RVA: 0x002550A0 File Offset: 0x002540A0
		public dfrmFaceIDCheckInput()
		{
			this.InitializeComponent();
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x002550F4 File Offset: 0x002540F4
		private void btnStop_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00255104 File Offset: 0x00254104
		private void dfrmShiftAttReportCreate_Load(object sender, EventArgs e)
		{
			this.strConsumerSql = "SELECT id FROM tRecognize ORDER BY ID DESC";
			long.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getValBySql(this.strConsumerSql)), out this.lastID);
			this.strConsumerSql = string.Format("SELECT id FROM tRecognize where id > {0} and success ='true' ORDER BY ID ", this.lastID);
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00255164 File Offset: 0x00254164
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				long num = 0L;
				long.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getValBySql(this.strConsumerSql)), out num);
				if (num > 0L)
				{
					this.lastID = num;
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0400367E RID: 13950
		public DateTime dtBegin;

		// Token: 0x0400367F RID: 13951
		public DateTime dtEnd;

		// Token: 0x04003680 RID: 13952
		public string groupName = "";

		// Token: 0x04003681 RID: 13953
		public long lastID;

		// Token: 0x04003682 RID: 13954
		private DateTime startTime = DateTime.Now;

		// Token: 0x04003683 RID: 13955
		public string strConsumerSql = "";

		// Token: 0x04003684 RID: 13956
		public int totalConsumer;
	}
}
