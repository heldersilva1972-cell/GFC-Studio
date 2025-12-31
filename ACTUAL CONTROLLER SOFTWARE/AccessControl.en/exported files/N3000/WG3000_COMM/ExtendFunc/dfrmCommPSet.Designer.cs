namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000234 RID: 564
	public partial class dfrmCommPSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010EB RID: 4331 RVA: 0x0013610C File Offset: 0x0013510C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0013612C File Offset: 0x0013512C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmCommPSet));
			this.txtPasswordNew = new global::System.Windows.Forms.TextBox();
			this.txtPasswordNewConfirm = new global::System.Windows.Forms.TextBox();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.txtPasswordPrev = new global::System.Windows.Forms.TextBox();
			this.txtPasswordPrevConfirm = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtPasswordNew, "txtPasswordNew");
			this.txtPasswordNew.Name = "txtPasswordNew";
			componentResourceManager.ApplyResources(this.txtPasswordNewConfirm, "txtPasswordNewConfirm");
			this.txtPasswordNewConfirm.Name = "txtPasswordNewConfirm";
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.txtPasswordPrev);
			this.groupBox1.Controls.Add(this.txtPasswordPrevConfirm);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.txtPasswordPrev, "txtPasswordPrev");
			this.txtPasswordPrev.Name = "txtPasswordPrev";
			componentResourceManager.ApplyResources(this.txtPasswordPrevConfirm, "txtPasswordPrevConfirm");
			this.txtPasswordPrevConfirm.Name = "txtPasswordPrevConfirm";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.BackColor = global::System.Drawing.Color.Transparent;
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
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
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.txtPasswordNew);
			base.Controls.Add(this.txtPasswordNewConfirm);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.Label3);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmCommPSet";
			base.Load += new global::System.EventHandler(this.dfrmCommPSet_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmCommPSet_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001E25 RID: 7717
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001E26 RID: 7718
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04001E27 RID: 7719
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001E28 RID: 7720
		internal global::System.Windows.Forms.Button btnOk;

		// Token: 0x04001E29 RID: 7721
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x04001E2A RID: 7722
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x04001E2B RID: 7723
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x04001E2C RID: 7724
		internal global::System.Windows.Forms.Label label4;

		// Token: 0x04001E2D RID: 7725
		internal global::System.Windows.Forms.TextBox txtPasswordNew;

		// Token: 0x04001E2E RID: 7726
		internal global::System.Windows.Forms.TextBox txtPasswordNewConfirm;

		// Token: 0x04001E2F RID: 7727
		internal global::System.Windows.Forms.TextBox txtPasswordPrev;

		// Token: 0x04001E30 RID: 7728
		internal global::System.Windows.Forms.TextBox txtPasswordPrevConfirm;
	}
}
