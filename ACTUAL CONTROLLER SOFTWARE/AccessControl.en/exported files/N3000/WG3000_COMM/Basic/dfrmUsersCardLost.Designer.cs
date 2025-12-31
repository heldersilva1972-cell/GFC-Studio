namespace WG3000_COMM.Basic
{
	// Token: 0x0200003A RID: 58
	public partial class dfrmUsersCardLost : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x00071F73 File Offset: 0x00070F73
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00071F94 File Offset: 0x00070F94
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmUsersCardLost));
			this.txtf_CardNO = new global::System.Windows.Forms.MaskedTextBox();
			this.txtf_ConsumerName = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.txtf_CardNONew = new global::System.Windows.Forms.MaskedTextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtf_CardNO, "txtf_CardNO");
			this.txtf_CardNO.Name = "txtf_CardNO";
			this.txtf_CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.txtf_ConsumerName, "txtf_ConsumerName");
			this.txtf_ConsumerName.Name = "txtf_ConsumerName";
			this.txtf_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.txtf_CardNONew, "txtf_CardNONew");
			this.txtf_CardNONew.Name = "txtf_CardNONew";
			this.txtf_CardNONew.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtf_CardNONew_KeyPress);
			this.txtf_CardNONew.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.txtf_CardNONew_KeyUp);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
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
			this.timer2.Tick += new global::System.EventHandler(this.timer2_Tick);
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.txtf_CardNONew);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.txtf_CardNO);
			base.Controls.Add(this.txtf_ConsumerName);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label3);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmUsersCardLost";
			base.Load += new global::System.EventHandler(this.dfrmUsersCardLost_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040007AC RID: 1964
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040007AD RID: 1965
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040007AE RID: 1966
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040007AF RID: 1967
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040007B0 RID: 1968
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040007B1 RID: 1969
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040007B2 RID: 1970
		private global::System.Windows.Forms.Timer timer2;

		// Token: 0x040007B3 RID: 1971
		public global::System.Windows.Forms.MaskedTextBox txtf_CardNO;

		// Token: 0x040007B4 RID: 1972
		public global::System.Windows.Forms.MaskedTextBox txtf_CardNONew;

		// Token: 0x040007B5 RID: 1973
		public global::System.Windows.Forms.TextBox txtf_ConsumerName;
	}
}
