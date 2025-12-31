namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002EB RID: 747
	public partial class dfrmFingerSupercard : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001569 RID: 5481 RVA: 0x001A8A68 File Offset: 0x001A7A68
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x001A8A88 File Offset: 0x001A7A88
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Finger.dfrmFingerSupercard));
			this.grpSuperCards = new global::System.Windows.Forms.GroupBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.txtSuperCard2 = new global::System.Windows.Forms.MaskedTextBox();
			this.label10 = new global::System.Windows.Forms.Label();
			this.txtSuperCard1 = new global::System.Windows.Forms.MaskedTextBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.grpSuperCards.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.grpSuperCards, "grpSuperCards");
			this.grpSuperCards.Controls.Add(this.label9);
			this.grpSuperCards.Controls.Add(this.txtSuperCard2);
			this.grpSuperCards.Controls.Add(this.label10);
			this.grpSuperCards.Controls.Add(this.txtSuperCard1);
			this.grpSuperCards.Controls.Add(this.label11);
			this.grpSuperCards.ForeColor = global::System.Drawing.Color.White;
			this.grpSuperCards.Name = "grpSuperCards";
			this.grpSuperCards.TabStop = false;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.BackColor = global::System.Drawing.Color.Transparent;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.txtSuperCard2, "txtSuperCard2");
			this.txtSuperCard2.Name = "txtSuperCard2";
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.BackColor = global::System.Drawing.Color.Transparent;
			this.label10.ForeColor = global::System.Drawing.Color.White;
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.txtSuperCard1, "txtSuperCard1");
			this.txtSuperCard1.Name = "txtSuperCard1";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.BackColor = global::System.Drawing.Color.Transparent;
			this.label11.ForeColor = global::System.Drawing.Color.White;
			this.label11.Name = "label11";
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
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.grpSuperCards);
			base.Name = "dfrmFingerSupercard";
			base.Load += new global::System.EventHandler(this.dfrmFingerSupercard_Load);
			this.grpSuperCards.ResumeLayout(false);
			this.grpSuperCards.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04002C44 RID: 11332
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002C45 RID: 11333
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002C46 RID: 11334
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002C47 RID: 11335
		private global::System.Windows.Forms.Label label10;

		// Token: 0x04002C48 RID: 11336
		private global::System.Windows.Forms.Label label11;

		// Token: 0x04002C49 RID: 11337
		private global::System.Windows.Forms.Label label9;

		// Token: 0x04002C4A RID: 11338
		public global::System.Windows.Forms.GroupBox grpSuperCards;

		// Token: 0x04002C4B RID: 11339
		public global::System.Windows.Forms.MaskedTextBox txtSuperCard1;

		// Token: 0x04002C4C RID: 11340
		public global::System.Windows.Forms.MaskedTextBox txtSuperCard2;
	}
}
