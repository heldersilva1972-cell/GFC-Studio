namespace WG3000_COMM.Basic
{
	// Token: 0x0200003C RID: 60
	public partial class dfrmWait : global::System.Windows.Forms.Form
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x00074180 File Offset: 0x00073180
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000741A0 File Offset: 0x000731A0
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmWait));
			this.label1 = new global::System.Windows.Forms.Label();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.lblRuntime = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.xToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.label1.UseWaitCursor = true;
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.lblRuntime, "lblRuntime");
			this.lblRuntime.BackColor = global::System.Drawing.Color.Transparent;
			this.lblRuntime.ForeColor = global::System.Drawing.Color.White;
			this.lblRuntime.Name = "lblRuntime";
			this.lblRuntime.UseWaitCursor = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.xToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.xToolStripMenuItem, "xToolStripMenuItem");
			this.xToolStripMenuItem.Name = "xToolStripMenuItem";
			this.xToolStripMenuItem.Click += new global::System.EventHandler(this.xToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.UseWaitCursor = true;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			base.CancelButton = this.btnClose;
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.ControlBox = false;
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.lblRuntime);
			base.Controls.Add(this.label1);
			this.ForeColor = global::System.Drawing.Color.White;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmWait";
			base.TopMost = true;
			base.UseWaitCursor = true;
			base.Load += new global::System.EventHandler(this.dfrmWait_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040007E2 RID: 2018
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040007E3 RID: 2019
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040007E4 RID: 2020
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040007E5 RID: 2021
		private global::System.Windows.Forms.Label lblRuntime;

		// Token: 0x040007E6 RID: 2022
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040007E7 RID: 2023
		private global::System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;

		// Token: 0x040007E8 RID: 2024
		internal global::System.Windows.Forms.Button btnClose;
	}
}
