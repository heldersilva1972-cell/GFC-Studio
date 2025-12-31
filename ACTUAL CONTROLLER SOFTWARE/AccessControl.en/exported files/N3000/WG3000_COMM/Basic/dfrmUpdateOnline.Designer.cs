namespace WG3000_COMM.Basic
{
	// Token: 0x02000032 RID: 50
	public partial class dfrmUpdateOnline : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000379 RID: 889 RVA: 0x00065939 File Offset: 0x00064939
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00065958 File Offset: 0x00064958
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmUpdateOnline));
			this.txtInfo = new global::System.Windows.Forms.TextBox();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.PictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			((global::System.ComponentModel.ISupportInitialize)this.PictureBox1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtInfo, "txtInfo");
			this.txtInfo.Name = "txtInfo";
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.PictureBox1, "PictureBox1");
			this.PictureBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			this.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.PictureBox1);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.txtInfo);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmUpdateOnline";
			base.Load += new global::System.EventHandler(this.dfrmUpdateOnline_Load);
			((global::System.ComponentModel.ISupportInitialize)this.PictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040006B0 RID: 1712
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040006B1 RID: 1713
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040006B2 RID: 1714
		internal global::System.Windows.Forms.Button btnExit;

		// Token: 0x040006B3 RID: 1715
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040006B4 RID: 1716
		internal global::System.Windows.Forms.PictureBox PictureBox1;

		// Token: 0x040006B5 RID: 1717
		internal global::System.Windows.Forms.TextBox txtInfo;
	}
}
