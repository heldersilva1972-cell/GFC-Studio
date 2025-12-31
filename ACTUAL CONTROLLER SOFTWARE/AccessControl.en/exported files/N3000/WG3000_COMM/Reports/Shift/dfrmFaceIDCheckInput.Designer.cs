namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000366 RID: 870
	public partial class dfrmFaceIDCheckInput : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001C51 RID: 7249 RVA: 0x002551D4 File Offset: 0x002541D4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x002551F4 File Offset: 0x002541F4
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmFaceIDCheckInput));
			this.lblInfo = new global::System.Windows.Forms.Label();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.btnStop = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.lblRuntime = new global::System.Windows.Forms.Label();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.BackColor = global::System.Drawing.Color.Transparent;
			this.lblInfo.ForeColor = global::System.Drawing.Color.White;
			this.lblInfo.Name = "lblInfo";
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			componentResourceManager.ApplyResources(this.btnStop, "btnStop");
			this.btnStop.BackColor = global::System.Drawing.Color.Transparent;
			this.btnStop.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnStop.ForeColor = global::System.Drawing.Color.White;
			this.btnStop.Name = "btnStop";
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new global::System.EventHandler(this.btnStop_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.lblRuntime, "lblRuntime");
			this.lblRuntime.BackColor = global::System.Drawing.Color.Transparent;
			this.lblRuntime.ForeColor = global::System.Drawing.Color.White;
			this.lblRuntime.Name = "lblRuntime";
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.lblRuntime);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnStop);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.lblInfo);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmFaceIDCheckInput";
			base.Load += new global::System.EventHandler(this.dfrmShiftAttReportCreate_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003685 RID: 13957
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003686 RID: 13958
		private global::System.Windows.Forms.Button btnStop;

		// Token: 0x04003687 RID: 13959
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04003688 RID: 13960
		private global::System.Windows.Forms.Label lblInfo;

		// Token: 0x04003689 RID: 13961
		private global::System.Windows.Forms.Label lblRuntime;

		// Token: 0x0400368A RID: 13962
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x0400368B RID: 13963
		private global::System.Windows.Forms.Timer timer1;
	}
}
