namespace WG3000_COMM.ExtendFunc.QR2017
{
	// Token: 0x02000320 RID: 800
	public partial class dfrmCreateQR4Door : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001907 RID: 6407 RVA: 0x0020B3E8 File Offset: 0x0020A3E8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x0020B408 File Offset: 0x0020A408
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.QR2017.dfrmCreateQR4Door));
			this.printDocument1 = new global::System.Drawing.Printing.PrintDocument();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.scaleToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.valueToolStripMenuItem = new global::System.Windows.Forms.ToolStripTextBox();
			this.saveAsFileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.stbRunInfo = new global::System.Windows.Forms.StatusStrip();
			this.statRuninfo1 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statTimeDate = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.btnCopy = new global::System.Windows.Forms.Button();
			this.btnPrint = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.textBoxText = new global::System.Windows.Forms.TextBox();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.lblUser = new global::System.Windows.Forms.Label();
			this.lblInfo = new global::System.Windows.Forms.Label();
			this.pictureBoxQr = new global::System.Windows.Forms.PictureBox();
			this.contextMenuStrip1.SuspendLayout();
			this.stbRunInfo.SuspendLayout();
			this.panel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxQr).BeginInit();
			base.SuspendLayout();
			this.printDocument1.PrintPage += new global::System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.scaleToolStripMenuItem, this.saveAsFileToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.scaleToolStripMenuItem, "scaleToolStripMenuItem");
			this.scaleToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.valueToolStripMenuItem });
			this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
			componentResourceManager.ApplyResources(this.valueToolStripMenuItem, "valueToolStripMenuItem");
			this.valueToolStripMenuItem.Name = "valueToolStripMenuItem";
			this.valueToolStripMenuItem.TextChanged += new global::System.EventHandler(this.valueToolStripMenuItem_TextChanged);
			componentResourceManager.ApplyResources(this.saveAsFileToolStripMenuItem, "saveAsFileToolStripMenuItem");
			this.saveAsFileToolStripMenuItem.Name = "saveAsFileToolStripMenuItem";
			this.saveAsFileToolStripMenuItem.Click += new global::System.EventHandler(this.saveAsFileToolStripMenuItem_Click);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.stbRunInfo, "stbRunInfo");
			this.stbRunInfo.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_bottom;
			this.stbRunInfo.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.statRuninfo1, this.statTimeDate });
			this.stbRunInfo.Name = "stbRunInfo";
			componentResourceManager.ApplyResources(this.statRuninfo1, "statRuninfo1");
			this.statRuninfo1.BackColor = global::System.Drawing.Color.Transparent;
			this.statRuninfo1.ForeColor = global::System.Drawing.Color.White;
			this.statRuninfo1.Name = "statRuninfo1";
			this.statRuninfo1.Spring = true;
			componentResourceManager.ApplyResources(this.statTimeDate, "statTimeDate");
			this.statTimeDate.BackColor = global::System.Drawing.Color.Transparent;
			this.statTimeDate.ForeColor = global::System.Drawing.Color.White;
			this.statTimeDate.Image = global::WG3000_COMM.Properties.Resources.timequery;
			this.statTimeDate.Name = "statTimeDate";
			componentResourceManager.ApplyResources(this.btnCopy, "btnCopy");
			this.btnCopy.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCopy.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCopy.ForeColor = global::System.Drawing.Color.White;
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.UseVisualStyleBackColor = false;
			this.btnCopy.Click += new global::System.EventHandler(this.btnCopy_Click);
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.BackColor = global::System.Drawing.Color.Transparent;
			this.btnPrint.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.UseVisualStyleBackColor = false;
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.textBoxText, "textBoxText");
			this.textBoxText.Name = "textBoxText";
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = global::System.Drawing.Color.White;
			this.panel1.ContextMenuStrip = this.contextMenuStrip1;
			this.panel1.Controls.Add(this.lblUser);
			this.panel1.Controls.Add(this.lblInfo);
			this.panel1.Controls.Add(this.pictureBoxQr);
			this.panel1.Name = "panel1";
			componentResourceManager.ApplyResources(this.lblUser, "lblUser");
			this.lblUser.BackColor = global::System.Drawing.Color.White;
			this.lblUser.Name = "lblUser";
			componentResourceManager.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.BackColor = global::System.Drawing.Color.White;
			this.lblInfo.Name = "lblInfo";
			componentResourceManager.ApplyResources(this.pictureBoxQr, "pictureBoxQr");
			this.pictureBoxQr.BackColor = global::System.Drawing.Color.White;
			this.pictureBoxQr.Name = "pictureBoxQr";
			this.pictureBoxQr.TabStop = false;
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.stbRunInfo);
			base.Controls.Add(this.btnCopy);
			base.Controls.Add(this.btnPrint);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.textBoxText);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmCreateQR4Door";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmCreateQR_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmCreateQR_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.stbRunInfo.ResumeLayout(false);
			this.stbRunInfo.PerformLayout();
			this.panel1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxQr).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040032F8 RID: 13048
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040032F9 RID: 13049
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040032FA RID: 13050
		private global::System.Windows.Forms.Button btnCopy;

		// Token: 0x040032FB RID: 13051
		private global::System.Windows.Forms.Button btnPrint;

		// Token: 0x040032FC RID: 13052
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040032FD RID: 13053
		private global::System.Windows.Forms.Label lblInfo;

		// Token: 0x040032FE RID: 13054
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x040032FF RID: 13055
		private global::System.Windows.Forms.PictureBox pictureBoxQr;

		// Token: 0x04003300 RID: 13056
		private global::System.Drawing.Printing.PrintDocument printDocument1;

		// Token: 0x04003301 RID: 13057
		private global::System.Windows.Forms.ToolStripMenuItem saveAsFileToolStripMenuItem;

		// Token: 0x04003302 RID: 13058
		private global::System.Windows.Forms.ToolStripMenuItem scaleToolStripMenuItem;

		// Token: 0x04003303 RID: 13059
		private global::System.Windows.Forms.ToolStripStatusLabel statRuninfo1;

		// Token: 0x04003304 RID: 13060
		private global::System.Windows.Forms.ToolStripStatusLabel statTimeDate;

		// Token: 0x04003305 RID: 13061
		private global::System.Windows.Forms.StatusStrip stbRunInfo;

		// Token: 0x04003306 RID: 13062
		private global::System.Windows.Forms.TextBox textBoxText;

		// Token: 0x04003307 RID: 13063
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04003308 RID: 13064
		private global::System.Windows.Forms.ToolStripTextBox valueToolStripMenuItem;

		// Token: 0x04003309 RID: 13065
		public global::System.Windows.Forms.Label lblUser;
	}
}
