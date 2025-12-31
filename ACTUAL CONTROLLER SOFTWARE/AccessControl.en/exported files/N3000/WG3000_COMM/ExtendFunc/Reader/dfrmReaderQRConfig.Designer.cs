namespace WG3000_COMM.ExtendFunc.Reader
{
	// Token: 0x02000322 RID: 802
	public partial class dfrmReaderQRConfig : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001915 RID: 6421 RVA: 0x0020C9AC File Offset: 0x0020B9AC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0020C9CC File Offset: 0x0020B9CC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Reader.dfrmReaderQRConfig));
			this.optDisableRS232_1 = new global::System.Windows.Forms.RadioButton();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.txtCustomPwd = new global::System.Windows.Forms.TextBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.chkExtern5 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern4 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern3 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern2 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern1 = new global::System.Windows.Forms.CheckBox();
			this.optEnabledRXToUDP = new global::System.Windows.Forms.RadioButton();
			this.optEnableCardInput = new global::System.Windows.Forms.RadioButton();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.optDisableRS232_1, "optDisableRS232_1");
			this.optDisableRS232_1.BackColor = global::System.Drawing.Color.Transparent;
			this.optDisableRS232_1.ForeColor = global::System.Drawing.Color.White;
			this.optDisableRS232_1.Name = "optDisableRS232_1";
			this.optDisableRS232_1.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.txtCustomPwd);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.groupBox4);
			this.groupBox1.Controls.Add(this.optDisableRS232_1);
			this.groupBox1.Controls.Add(this.optEnabledRXToUDP);
			this.groupBox1.Controls.Add(this.optEnableCardInput);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.txtCustomPwd, "txtCustomPwd");
			this.txtCustomPwd.Name = "txtCustomPwd";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.chkExtern5);
			this.groupBox4.Controls.Add(this.chkExtern4);
			this.groupBox4.Controls.Add(this.chkExtern3);
			this.groupBox4.Controls.Add(this.chkExtern2);
			this.groupBox4.Controls.Add(this.chkExtern1);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.chkExtern5, "chkExtern5");
			this.chkExtern5.Name = "chkExtern5";
			this.chkExtern5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern4, "chkExtern4");
			this.chkExtern4.Name = "chkExtern4";
			this.chkExtern4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern3, "chkExtern3");
			this.chkExtern3.Name = "chkExtern3";
			this.chkExtern3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern2, "chkExtern2");
			this.chkExtern2.Name = "chkExtern2";
			this.chkExtern2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern1, "chkExtern1");
			this.chkExtern1.Name = "chkExtern1";
			this.chkExtern1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnabledRXToUDP, "optEnabledRXToUDP");
			this.optEnabledRXToUDP.ForeColor = global::System.Drawing.Color.White;
			this.optEnabledRXToUDP.Name = "optEnabledRXToUDP";
			this.optEnabledRXToUDP.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableCardInput, "optEnableCardInput");
			this.optEnableCardInput.Checked = true;
			this.optEnableCardInput.ForeColor = global::System.Drawing.Color.White;
			this.optEnableCardInput.Name = "optEnableCardInput";
			this.optEnableCardInput.TabStop = true;
			this.optEnableCardInput.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
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
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Name = "dfrmReaderQRConfig";
			base.Load += new global::System.EventHandler(this.dfrmReaderConfig_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmRs232Config_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04003333 RID: 13107
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003334 RID: 13108
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003335 RID: 13109
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003336 RID: 13110
		private global::System.Windows.Forms.CheckBox chkExtern1;

		// Token: 0x04003337 RID: 13111
		private global::System.Windows.Forms.CheckBox chkExtern2;

		// Token: 0x04003338 RID: 13112
		private global::System.Windows.Forms.CheckBox chkExtern3;

		// Token: 0x04003339 RID: 13113
		private global::System.Windows.Forms.CheckBox chkExtern4;

		// Token: 0x0400333A RID: 13114
		private global::System.Windows.Forms.CheckBox chkExtern5;

		// Token: 0x0400333B RID: 13115
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400333C RID: 13116
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400333D RID: 13117
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400333E RID: 13118
		private global::System.Windows.Forms.RadioButton optDisableRS232_1;

		// Token: 0x0400333F RID: 13119
		private global::System.Windows.Forms.TextBox txtCustomPwd;

		// Token: 0x04003340 RID: 13120
		public global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04003341 RID: 13121
		public global::System.Windows.Forms.RadioButton optEnableCardInput;

		// Token: 0x04003342 RID: 13122
		public global::System.Windows.Forms.RadioButton optEnabledRXToUDP;
	}
}
