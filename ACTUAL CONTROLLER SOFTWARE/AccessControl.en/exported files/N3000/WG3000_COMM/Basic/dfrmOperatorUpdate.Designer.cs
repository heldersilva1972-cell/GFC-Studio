namespace WG3000_COMM.Basic
{
	// Token: 0x02000021 RID: 33
	public partial class dfrmOperatorUpdate : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060001DC RID: 476 RVA: 0x0003F4B8 File Offset: 0x0003E4B8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0003F4D8 File Offset: 0x0003E4D8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmOperatorUpdate));
			this.label7 = new global::System.Windows.Forms.Label();
			this.txtName = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtPassword = new global::System.Windows.Forms.TextBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.txtConfirmedPassword = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.BackColor = global::System.Drawing.Color.Transparent;
			this.label7.ForeColor = global::System.Drawing.Color.White;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
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
			componentResourceManager.ApplyResources(this.txtConfirmedPassword, "txtConfirmedPassword");
			this.txtConfirmedPassword.Name = "txtConfirmedPassword";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtConfirmedPassword);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.txtPassword);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.txtName);
			base.Controls.Add(this.label2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmOperatorUpdate";
			base.Load += new global::System.EventHandler(this.dfrmOperatorUpdate_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040003FE RID: 1022
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040003FF RID: 1023
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000400 RID: 1024
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000401 RID: 1025
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000402 RID: 1026
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000403 RID: 1027
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04000404 RID: 1028
		private global::System.Windows.Forms.TextBox txtConfirmedPassword;

		// Token: 0x04000405 RID: 1029
		private global::System.Windows.Forms.TextBox txtPassword;

		// Token: 0x04000406 RID: 1030
		public global::System.Windows.Forms.TextBox txtName;
	}
}
