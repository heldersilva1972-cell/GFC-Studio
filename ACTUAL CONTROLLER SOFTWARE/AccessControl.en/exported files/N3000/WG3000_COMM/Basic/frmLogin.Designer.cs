namespace WG3000_COMM.Basic
{
	// Token: 0x02000050 RID: 80
	public partial class frmLogin : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060005C0 RID: 1472 RVA: 0x000A1679 File Offset: 0x000A0679
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000A1698 File Offset: 0x000A0698
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmLogin));
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.label3 = new global::System.Windows.Forms.Label();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.txtPassword = new global::System.Windows.Forms.TextBox();
			this.txtOperatorName = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.restoreToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.Red;
			this.label3.Name = "label3";
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			this.btnOK.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.frmLogin_KeyPress);
			componentResourceManager.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.frmLogin_KeyPress);
			componentResourceManager.ApplyResources(this.txtOperatorName, "txtOperatorName");
			this.txtOperatorName.Name = "txtOperatorName";
			this.txtOperatorName.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.frmLogin_KeyPress);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.restoreToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
			componentResourceManager.ApplyResources(this.restoreToolStripMenuItem, "restoreToolStripMenuItem");
			this.restoreToolStripMenuItem.Click += new global::System.EventHandler(this.restoreToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::WG3000_COMM.Properties.Resources.pLogin_bk;
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.txtPassword);
			base.Controls.Add(this.txtOperatorName);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.Name = "frmLogin";
			base.TopMost = true;
			base.Load += new global::System.EventHandler(this.frmLogin_Load);
			base.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.frmLogin_KeyPress);
			base.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
			base.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseMove);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000AD1 RID: 2769
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000AD2 RID: 2770
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04000AD3 RID: 2771
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000AD4 RID: 2772
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000AD5 RID: 2773
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000AD6 RID: 2774
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000AD7 RID: 2775
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000AD9 RID: 2777
		private global::System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;

		// Token: 0x04000ADA RID: 2778
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000ADB RID: 2779
		private global::System.Windows.Forms.TextBox txtOperatorName;

		// Token: 0x04000ADC RID: 2780
		private global::System.Windows.Forms.TextBox txtPassword;
	}
}
