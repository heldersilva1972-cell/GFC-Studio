namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002E1 RID: 737
	public partial class dfrmFingerEnrollNext : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060014E3 RID: 5347 RVA: 0x0019CE86 File Offset: 0x0019BE86
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0019CEA8 File Offset: 0x0019BEA8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Finger.dfrmFingerEnrollNext));
			this.txtInfo = new global::System.Windows.Forms.TextBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnAddNext = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtInfo, "txtInfo");
			this.txtInfo.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.txtInfo.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.txtInfo.ForeColor = global::System.Drawing.Color.White;
			this.txtInfo.Name = "txtInfo";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnAddNext, "btnAddNext");
			this.btnAddNext.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddNext.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddNext.ForeColor = global::System.Drawing.Color.White;
			this.btnAddNext.Name = "btnAddNext";
			this.btnAddNext.UseVisualStyleBackColor = false;
			this.btnAddNext.Click += new global::System.EventHandler(this.btnAddNext_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtInfo);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnAddNext);
			base.Name = "dfrmFingerEnrollNext";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002B9F RID: 11167
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002BA0 RID: 11168
		private global::System.Windows.Forms.Button btnAddNext;

		// Token: 0x04002BA1 RID: 11169
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002BA2 RID: 11170
		public global::System.Windows.Forms.TextBox txtInfo;
	}
}
