namespace WG3000_COMM.Basic
{
	// Token: 0x02000033 RID: 51
	public partial class dfrmUploadOption : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000380 RID: 896 RVA: 0x00065C6A File Offset: 0x00064C6A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00065C8C File Offset: 0x00064C8C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmUploadOption));
			this.chkBasicConfiguration = new global::System.Windows.Forms.CheckBox();
			this.chkAccessPrivilege = new global::System.Windows.Forms.CheckBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.chkBasicConfiguration, "chkBasicConfiguration");
			this.chkBasicConfiguration.BackColor = global::System.Drawing.Color.Transparent;
			this.chkBasicConfiguration.Checked = true;
			this.chkBasicConfiguration.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkBasicConfiguration.ForeColor = global::System.Drawing.Color.White;
			this.chkBasicConfiguration.Name = "chkBasicConfiguration";
			this.chkBasicConfiguration.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkAccessPrivilege, "chkAccessPrivilege");
			this.chkAccessPrivilege.BackColor = global::System.Drawing.Color.Transparent;
			this.chkAccessPrivilege.Checked = true;
			this.chkAccessPrivilege.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAccessPrivilege.ForeColor = global::System.Drawing.Color.White;
			this.chkAccessPrivilege.Name = "chkAccessPrivilege";
			this.chkAccessPrivilege.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
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
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.chkAccessPrivilege);
			base.Controls.Add(this.chkBasicConfiguration);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmUploadOption";
			base.Load += new global::System.EventHandler(this.dfrmUploadOption_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040006B7 RID: 1719
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040006B8 RID: 1720
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040006B9 RID: 1721
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040006BA RID: 1722
		private global::System.Windows.Forms.CheckBox chkAccessPrivilege;

		// Token: 0x040006BB RID: 1723
		private global::System.Windows.Forms.CheckBox chkBasicConfiguration;
	}
}
