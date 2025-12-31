namespace WG3000_COMM.Basic
{
	// Token: 0x0200002C RID: 44
	public partial class dfrmShowError : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0005D616 File Offset: 0x0005C616
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0005D638 File Offset: 0x0005C638
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmShowError));
			this.btnOK = new global::System.Windows.Forms.Button();
			this.txtErrorDetail = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnCopyDetail = new global::System.Windows.Forms.Button();
			this.btnDetail = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.txtErrorDetail, "txtErrorDetail");
			this.txtErrorDetail.Name = "txtErrorDetail";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnCopyDetail, "btnCopyDetail");
			this.btnCopyDetail.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCopyDetail.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCopyDetail.ForeColor = global::System.Drawing.Color.White;
			this.btnCopyDetail.Name = "btnCopyDetail";
			this.btnCopyDetail.UseVisualStyleBackColor = false;
			this.btnCopyDetail.Click += new global::System.EventHandler(this.btnCopyDetail_Click);
			componentResourceManager.ApplyResources(this.btnDetail, "btnDetail");
			this.btnDetail.Name = "btnDetail";
			this.btnDetail.UseVisualStyleBackColor = true;
			this.btnDetail.Click += new global::System.EventHandler(this.btnDetail_Click);
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtErrorDetail);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnCopyDetail);
			base.Controls.Add(this.btnDetail);
			base.MinimizeBox = false;
			base.Name = "dfrmShowError";
			base.Load += new global::System.EventHandler(this.dfrmShowError_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400060C RID: 1548
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400060D RID: 1549
		private global::System.Windows.Forms.Button btnCopyDetail;

		// Token: 0x0400060E RID: 1550
		private global::System.Windows.Forms.Button btnDetail;

		// Token: 0x0400060F RID: 1551
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000610 RID: 1552
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000611 RID: 1553
		private global::System.Windows.Forms.TextBox txtErrorDetail;
	}
}
