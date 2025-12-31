namespace WG3000_COMM.Basic
{
	// Token: 0x02000017 RID: 23
	public partial class dfrmInputNewName : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600011E RID: 286 RVA: 0x000283F3 File Offset: 0x000273F3
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00028414 File Offset: 0x00027414
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmInputNewName));
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtNewName = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtNewName, "txtNewName");
			this.txtNewName.Name = "txtNewName";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.label2);
			base.Controls.Add(this.txtNewName);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmInputNewName";
			base.TopMost = true;
			base.Load += new global::System.EventHandler(this.dfrmInputNewName_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002D7 RID: 727
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002D8 RID: 728
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040002D9 RID: 729
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040002DA RID: 730
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040002DB RID: 731
		public global::System.Windows.Forms.Label label1;

		// Token: 0x040002DC RID: 732
		public global::System.Windows.Forms.TextBox txtNewName;
	}
}
