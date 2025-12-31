namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036E RID: 878
	public partial class dfrmShiftAttReportCreate : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001CBF RID: 7359 RVA: 0x0025F7A4 File Offset: 0x0025E7A4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x0025F7C4 File Offset: 0x0025E7C4
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmShiftAttReportCreate));
			this.lblInfo = new global::System.Windows.Forms.Label();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.btnStop = new global::System.Windows.Forms.Button();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
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
			this.backgroundWorker1.WorkerReportsProgress = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.ProgressChanged += new global::System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.lblRuntime, "lblRuntime");
			this.lblRuntime.BackColor = global::System.Drawing.Color.Transparent;
			this.lblRuntime.ForeColor = global::System.Drawing.Color.White;
			this.lblRuntime.Name = "lblRuntime";
			this.timer1.Enabled = true;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.lblRuntime);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnStop);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.lblInfo);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmShiftAttReportCreate";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmShiftAttReportCreate_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmShiftAttReportCreate_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003754 RID: 14164
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003755 RID: 14165
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04003756 RID: 14166
		private global::System.Windows.Forms.Button btnStop;

		// Token: 0x04003757 RID: 14167
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04003758 RID: 14168
		private global::System.Windows.Forms.Label lblInfo;

		// Token: 0x04003759 RID: 14169
		private global::System.Windows.Forms.Label lblRuntime;

		// Token: 0x0400375A RID: 14170
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x0400375B RID: 14171
		private global::System.Windows.Forms.Timer timer1;
	}
}
