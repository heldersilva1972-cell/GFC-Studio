namespace WG3000_COMM.Basic
{
	// Token: 0x02000015 RID: 21
	public partial class dfrmFeedback : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000109 RID: 265 RVA: 0x0002659D File Offset: 0x0002559D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000265BC File Offset: 0x000255BC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmFeedback));
			this.txtContents = new global::System.Windows.Forms.TextBox();
			this.txtTitle = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtContents, "txtContents");
			this.txtContents.Name = "txtContents";
			componentResourceManager.ApplyResources(this.txtTitle, "txtTitle");
			this.txtTitle.Name = "txtTitle";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.txtContents);
			base.Controls.Add(this.txtTitle);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmFeedback";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002B7 RID: 695
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002B8 RID: 696
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040002B9 RID: 697
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040002BA RID: 698
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040002BB RID: 699
		private global::System.Windows.Forms.TextBox txtContents;

		// Token: 0x040002BC RID: 700
		private global::System.Windows.Forms.TextBox txtTitle;
	}
}
