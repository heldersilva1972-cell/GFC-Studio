namespace WG3000_COMM.Basic
{
	// Token: 0x02000003 RID: 3
	public partial class dfrmAbout : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000280C File Offset: 0x0000180C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000282C File Offset: 0x0000182C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmAbout));
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.textBoxDescription = new global::System.Windows.Forms.TextBox();
			this.btnRegister = new global::System.Windows.Forms.Button();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.label5 = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.checkAndUpradeSoftwareToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.label6 = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.textBoxDescription, "textBoxDescription");
			this.textBoxDescription.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.textBoxDescription.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.textBoxDescription.ForeColor = global::System.Drawing.Color.White;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnRegister, "btnRegister");
			this.btnRegister.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRegister.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRegister.ForeColor = global::System.Drawing.Color.White;
			this.btnRegister.Name = "btnRegister";
			this.btnRegister.UseVisualStyleBackColor = false;
			this.btnRegister.Click += new global::System.EventHandler(this.btnRegister_Click);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.checkAndUpradeSoftwareToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.checkAndUpradeSoftwareToolStripMenuItem, "checkAndUpradeSoftwareToolStripMenuItem");
			this.checkAndUpradeSoftwareToolStripMenuItem.Name = "checkAndUpradeSoftwareToolStripMenuItem";
			this.checkAndUpradeSoftwareToolStripMenuItem.Click += new global::System.EventHandler(this.checkAndUpradeSoftwareToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.label6);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.btnRegister);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.textBoxDescription);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmAbout";
			base.Load += new global::System.EventHandler(this.dfrmAbout_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmAbout_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000006 RID: 6
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.ToolStripMenuItem checkAndUpradeSoftwareToolStripMenuItem;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.Label label6;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.TextBox textBoxDescription;

		// Token: 0x04000010 RID: 16
		internal global::System.Windows.Forms.Button btnClose;

		// Token: 0x04000011 RID: 17
		internal global::System.Windows.Forms.Button btnRegister;
	}
}
