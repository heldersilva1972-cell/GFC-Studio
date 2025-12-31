namespace WG3000_COMM.Basic
{
	// Token: 0x0200002B RID: 43
	public partial class dfrmSetPassword : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600031D RID: 797 RVA: 0x0005D192 File Offset: 0x0005C192
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0005D1B4 File Offset: 0x0005C1B4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmSetPassword));
			this.txtPasswordNew = new global::System.Windows.Forms.TextBox();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.txtPasswordNewConfirm = new global::System.Windows.Forms.TextBox();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtPasswordNew, "txtPasswordNew");
			this.txtPasswordNew.Name = "txtPasswordNew";
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this.txtPasswordNewConfirm, "txtPasswordNewConfirm");
			this.txtPasswordNewConfirm.Name = "txtPasswordNewConfirm";
			componentResourceManager.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOk.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOk.ForeColor = global::System.Drawing.Color.White;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = false;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			base.AcceptButton = this.btnOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.txtPasswordNew);
			base.Controls.Add(this.txtPasswordNewConfirm);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.Label3);
			base.Controls.Add(this.btnCancel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmSetPassword";
			base.Load += new global::System.EventHandler(this.dfrmSetPassword_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000604 RID: 1540
		private global::System.ComponentModel.Container components;

		// Token: 0x04000605 RID: 1541
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000606 RID: 1542
		internal global::System.Windows.Forms.Button btnOk;

		// Token: 0x04000607 RID: 1543
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x04000608 RID: 1544
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x04000609 RID: 1545
		internal global::System.Windows.Forms.TextBox txtPasswordNew;

		// Token: 0x0400060A RID: 1546
		internal global::System.Windows.Forms.TextBox txtPasswordNewConfirm;
	}
}
