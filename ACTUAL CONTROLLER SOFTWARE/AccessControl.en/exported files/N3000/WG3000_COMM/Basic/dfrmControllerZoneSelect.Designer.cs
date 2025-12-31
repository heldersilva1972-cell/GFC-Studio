namespace WG3000_COMM.Basic
{
	// Token: 0x0200000C RID: 12
	public partial class dfrmControllerZoneSelect : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00019054 File Offset: 0x00018054
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00019074 File Offset: 0x00018074
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmControllerZoneSelect));
			this.cboZone = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cboZone, "cboZone");
			this.cboZone.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboZone.FormattingEnabled = true;
			this.cboZone.Name = "cboZone";
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
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
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.cboZone);
			base.Controls.Add(this.label25);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmControllerZoneSelect";
			base.Load += new global::System.EventHandler(this.dfrmControllerZoneSelect_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400018D RID: 397
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400018E RID: 398
		private global::System.Windows.Forms.ComboBox cboZone;

		// Token: 0x0400018F RID: 399
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04000190 RID: 400
		public global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000191 RID: 401
		public global::System.Windows.Forms.Button btnOK;
	}
}
