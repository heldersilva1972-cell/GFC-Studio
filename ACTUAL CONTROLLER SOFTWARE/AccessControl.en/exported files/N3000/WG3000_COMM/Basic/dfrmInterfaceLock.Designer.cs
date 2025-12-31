namespace WG3000_COMM.Basic
{
	// Token: 0x02000018 RID: 24
	public partial class dfrmInterfaceLock : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000126 RID: 294 RVA: 0x0002880E File Offset: 0x0002780E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00028830 File Offset: 0x00027830
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmInterfaceLock));
			this.btnOk = new global::System.Windows.Forms.Button();
			this.txtOperatorName = new global::System.Windows.Forms.TextBox();
			this.txtPassword = new global::System.Windows.Forms.TextBox();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.Label3 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOk.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOk.ForeColor = global::System.Drawing.Color.White;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = false;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			componentResourceManager.ApplyResources(this.txtOperatorName, "txtOperatorName");
			this.txtOperatorName.Name = "txtOperatorName";
			this.txtOperatorName.ReadOnly = true;
			this.txtOperatorName.TabStop = false;
			componentResourceManager.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			base.AcceptButton = this.btnOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.txtOperatorName);
			base.Controls.Add(this.txtPassword);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.Label3);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.Name = "dfrmInterfaceLock";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmInterfaceLock_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmSetPassword_Load);
			base.SizeChanged += new global::System.EventHandler(this.dfrmInterfaceLock_SizeChanged);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002E0 RID: 736
		private global::System.ComponentModel.Container components;

		// Token: 0x040002E1 RID: 737
		internal global::System.Windows.Forms.Button btnOk;

		// Token: 0x040002E2 RID: 738
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x040002E3 RID: 739
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x040002E4 RID: 740
		public global::System.Windows.Forms.TextBox txtOperatorName;

		// Token: 0x040002E5 RID: 741
		internal global::System.Windows.Forms.TextBox txtPassword;
	}
}
