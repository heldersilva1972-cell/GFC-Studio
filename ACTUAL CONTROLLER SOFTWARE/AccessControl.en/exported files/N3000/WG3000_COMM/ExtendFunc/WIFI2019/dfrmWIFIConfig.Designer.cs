namespace WG3000_COMM.ExtendFunc.WIFI2019
{
	// Token: 0x0200032A RID: 810
	public partial class dfrmWIFIConfig : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001994 RID: 6548 RVA: 0x00216EF3 File Offset: 0x00215EF3
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x00216F14 File Offset: 0x00215F14
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.WIFI2019.dfrmWIFIConfig));
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.createShareSSIDNameAndCopyToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.decryptToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.optionToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deactiveWIFINeedRebootToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.txtSharePoint = new global::System.Windows.Forms.TextBox();
			this.chkDisplayPwd = new global::System.Windows.Forms.CheckBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.txtPassword = new global::System.Windows.Forms.TextBox();
			this.lblSSIDpwd = new global::System.Windows.Forms.Label();
			this.txtSSID = new global::System.Windows.Forms.TextBox();
			this.lblSSID = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.createShareSSIDNameAndCopyToolStripMenuItem, this.decryptToolStripMenuItem, this.optionToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.createShareSSIDNameAndCopyToolStripMenuItem, "createShareSSIDNameAndCopyToolStripMenuItem");
			this.createShareSSIDNameAndCopyToolStripMenuItem.Name = "createShareSSIDNameAndCopyToolStripMenuItem";
			this.createShareSSIDNameAndCopyToolStripMenuItem.Click += new global::System.EventHandler(this.createShareSSIDNameAndCopyToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.decryptToolStripMenuItem, "decryptToolStripMenuItem");
			this.decryptToolStripMenuItem.Name = "decryptToolStripMenuItem";
			this.decryptToolStripMenuItem.Click += new global::System.EventHandler(this.decryptToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.optionToolStripMenuItem, "optionToolStripMenuItem");
			this.optionToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.deactiveWIFINeedRebootToolStripMenuItem });
			this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
			componentResourceManager.ApplyResources(this.deactiveWIFINeedRebootToolStripMenuItem, "deactiveWIFINeedRebootToolStripMenuItem");
			this.deactiveWIFINeedRebootToolStripMenuItem.Name = "deactiveWIFINeedRebootToolStripMenuItem";
			this.deactiveWIFINeedRebootToolStripMenuItem.Click += new global::System.EventHandler(this.deactiveWIFINeedRebootToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.txtSharePoint, "txtSharePoint");
			this.txtSharePoint.Name = "txtSharePoint";
			this.txtSharePoint.ReadOnly = true;
			componentResourceManager.ApplyResources(this.chkDisplayPwd, "chkDisplayPwd");
			this.chkDisplayPwd.ForeColor = global::System.Drawing.Color.White;
			this.chkDisplayPwd.Name = "chkDisplayPwd";
			this.chkDisplayPwd.UseVisualStyleBackColor = true;
			this.chkDisplayPwd.CheckedChanged += new global::System.EventHandler(this.chkDisplayPwd_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			componentResourceManager.ApplyResources(this.lblSSIDpwd, "lblSSIDpwd");
			this.lblSSIDpwd.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSSIDpwd.ForeColor = global::System.Drawing.Color.White;
			this.lblSSIDpwd.Name = "lblSSIDpwd";
			componentResourceManager.ApplyResources(this.txtSSID, "txtSSID");
			this.txtSSID.Name = "txtSSID";
			componentResourceManager.ApplyResources(this.lblSSID, "lblSSID");
			this.lblSSID.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSSID.ForeColor = global::System.Drawing.Color.White;
			this.lblSSID.Name = "lblSSID";
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.txtSharePoint);
			base.Controls.Add(this.chkDisplayPwd);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.txtPassword);
			base.Controls.Add(this.lblSSIDpwd);
			base.Controls.Add(this.txtSSID);
			base.Controls.Add(this.lblSSID);
			base.Name = "dfrmWIFIConfig";
			base.Load += new global::System.EventHandler(this.dfrmWIFIConfig_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmWIFIConfig_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003445 RID: 13381
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003446 RID: 13382
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003447 RID: 13383
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003448 RID: 13384
		private global::System.Windows.Forms.CheckBox chkDisplayPwd;

		// Token: 0x04003449 RID: 13385
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x0400344A RID: 13386
		private global::System.Windows.Forms.ToolStripMenuItem createShareSSIDNameAndCopyToolStripMenuItem;

		// Token: 0x0400344B RID: 13387
		private global::System.Windows.Forms.ToolStripMenuItem deactiveWIFINeedRebootToolStripMenuItem;

		// Token: 0x0400344C RID: 13388
		private global::System.Windows.Forms.ToolStripMenuItem decryptToolStripMenuItem;

		// Token: 0x0400344D RID: 13389
		private global::System.Windows.Forms.Label lblSSID;

		// Token: 0x0400344E RID: 13390
		private global::System.Windows.Forms.Label lblSSIDpwd;

		// Token: 0x0400344F RID: 13391
		private global::System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;

		// Token: 0x04003450 RID: 13392
		private global::System.Windows.Forms.TextBox txtPassword;

		// Token: 0x04003451 RID: 13393
		private global::System.Windows.Forms.TextBox txtSharePoint;

		// Token: 0x04003452 RID: 13394
		private global::System.Windows.Forms.TextBox txtSSID;
	}
}
