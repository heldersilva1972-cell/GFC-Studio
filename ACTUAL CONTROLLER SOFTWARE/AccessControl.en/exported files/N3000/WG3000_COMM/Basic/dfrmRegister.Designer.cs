namespace WG3000_COMM.Basic
{
	// Token: 0x02000029 RID: 41
	public partial class dfrmRegister : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060002FD RID: 765 RVA: 0x0005AD51 File Offset: 0x00059D51
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0005AD70 File Offset: 0x00059D70
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmRegister));
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.txtCompanyName = new global::System.Windows.Forms.TextBox();
			this.txtBuildingCompanyName = new global::System.Windows.Forms.TextBox();
			this.txtRegisterCode = new global::System.Windows.Forms.TextBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.Exit = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.txtCompanyName, "txtCompanyName");
			this.txtCompanyName.Name = "txtCompanyName";
			componentResourceManager.ApplyResources(this.txtBuildingCompanyName, "txtBuildingCompanyName");
			this.txtBuildingCompanyName.Name = "txtBuildingCompanyName";
			componentResourceManager.ApplyResources(this.txtRegisterCode, "txtRegisterCode");
			this.txtRegisterCode.Name = "txtRegisterCode";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.Exit, "Exit");
			this.Exit.BackColor = global::System.Drawing.Color.Transparent;
			this.Exit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.Exit.ForeColor = global::System.Drawing.Color.White;
			this.Exit.Name = "Exit";
			this.Exit.UseVisualStyleBackColor = false;
			this.Exit.Click += new global::System.EventHandler(this.Exit_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.Exit);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.txtRegisterCode);
			base.Controls.Add(this.txtBuildingCompanyName);
			base.Controls.Add(this.txtCompanyName);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmRegister";
			base.Load += new global::System.EventHandler(this.dfrmRegister_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040005CC RID: 1484
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040005CD RID: 1485
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040005CE RID: 1486
		private global::System.Windows.Forms.Button Exit;

		// Token: 0x040005CF RID: 1487
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040005D0 RID: 1488
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040005D1 RID: 1489
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040005D2 RID: 1490
		private global::System.Windows.Forms.TextBox txtBuildingCompanyName;

		// Token: 0x040005D3 RID: 1491
		private global::System.Windows.Forms.TextBox txtCompanyName;

		// Token: 0x040005D4 RID: 1492
		private global::System.Windows.Forms.TextBox txtRegisterCode;
	}
}
